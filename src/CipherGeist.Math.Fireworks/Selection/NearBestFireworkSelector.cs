﻿namespace CipherGeist.Math.Fireworks.Selection;

/// <summary>
/// Selects <see cref="Firework"/>s that will stay around for the next step
/// based on the distance between the best <see cref="Firework"/> and each other
/// <see cref="Firework"/>s, per 2012 paper.
/// </summary>
public class NearBestFireworkSelector : FireworkSelectorBase
{
	private readonly IDistance _distanceCalculator;
	private readonly Func<IEnumerable<Firework>, Firework> _bestFireworkSelector;

	/// <summary>
	/// Initializes a new instance of the <see cref="NearBestFireworkSelector"/> class.
	/// </summary>
	/// <param name="distanceCalculator">The distance calculator.</param>
	/// <param name="bestFireworkSelector">The function that can be used to select 
	/// best <see cref="Firework"/>.</param>
	/// <param name="locationsNumber">The number of <see cref="Firework"/>s to be selected.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="distanceCalculator"/>
	/// or <paramref name="bestFireworkSelector"/> is <c>null</c>.
	/// </exception>
	public NearBestFireworkSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector, int locationsNumber)
		: base(locationsNumber)
	{
		_distanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));
		_bestFireworkSelector = bestFireworkSelector ?? throw new ArgumentNullException(nameof(bestFireworkSelector));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NearBestFireworkSelector"/> class.
	/// </summary>
	/// <param name="distanceCalculator">The distance calculator.</param>
	/// <param name="bestFireworkSelector">The function that can be used to select 
	/// best <see cref="Firework"/>.</param>
	/// <remarks>It is assumed that number of <see cref="Firework"/>s to be selected
	/// differs from step to step and hence is passed to the <c>Select</c> method.</remarks>
	public NearBestFireworkSelector(IDistance distanceCalculator, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
		: this(distanceCalculator, bestFireworkSelector, 0)
	{
	}

	/// <summary>
	/// Selects <paramref name="numberToSelect"/> <see cref="Firework"/>s from
	/// the <paramref name="from"/> collection. Selected <see cref="Firework"/>s
	/// are stored in the new collection, <paramref name="from"/> is not modified.
	/// </summary>
	/// <param name="from">A collection to select <see cref="Firework"/>s from.</param>
	/// <param name="numberToSelect">The number of <see cref="Firework"/>s to select.</param>
	/// <returns>
	/// A subset of <see cref="Firework"/>s.
	/// </returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="from"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="numberToSelect"/>
	/// is less than zero or greater than the number of elements in <paramref name="from"/>.
	/// </exception>
	public override IEnumerable<Firework> SelectFireworks(IEnumerable<Firework> from, int numberToSelect)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(numberToSelect);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(numberToSelect, from.Count());

		if (numberToSelect == from.Count())
		{
			return new List<Firework>(from);
		}

		// Have List<T> instead of IList<T> here since List<>.AddRange() is used below.
		var selectedLocations = new List<Firework>(numberToSelect);
		if (numberToSelect > 0)
		{
			Debug.Assert(_bestFireworkSelector != null, "Best firework selector is null");

			// 1. Find a firework with best quality
			var bestFirework = _bestFireworkSelector(from);

			// 2. Calculate distances between the best firework and other fireworks
			var distances = CalculateDistances(from, bestFirework);
			Debug.Assert(distances != null, "Distance collection is null");

			// 3. Select nearest individuals
			var sortedDistances = distances.OrderBy(p => p.Value, new DoubleExtensionComparer());
			Debug.Assert(sortedDistances != null, "Sorted distances collection is null");

			var nearestLocations = sortedDistances.Take(numberToSelect).Select(sp => sp.Key);
			Debug.Assert(nearestLocations != null, "Nearest locations collection is null");

			selectedLocations.AddRange(nearestLocations);
		}

		return selectedLocations;
	}

	/// <summary>
	/// Calculates the distances between each <see cref="Firework"/> and best <see cref="Firework"/>.
	/// </summary>
	/// <param name="fireworks">The collection of <see cref="Firework"/>s to calculate distances between.</param>
	/// <param name="bestFirework">The best <see cref="Firework"/> to calculate distances between.</param>
	/// <returns>A map. Key is a <see cref="Firework"/>. Value is a distance
	/// between that <see cref="Firework"/> and the best <see cref="Firework"/>.
	/// </returns>
	/// <remarks>Returned collection does not include the <paramref name="bestFirework"/>.</remarks>
	/// <exception cref="ArgumentNullException"> if <paramref name="fireworks"/> or 
	/// <paramref name="bestFirework"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException"> if <paramref name="fireworks"/> is empty.
	/// </exception>
	protected virtual IDictionary<Firework, double> CalculateDistances(IEnumerable<Firework> fireworks, Firework bestFirework)
	{
		ArgumentNullException.ThrowIfNull(fireworks);
		ArgumentNullException.ThrowIfNull(bestFirework);

		if (fireworks.Count() == 0)
		{
			throw new ArgumentException(string.Empty, nameof(fireworks));
		}

		Debug.Assert(_distanceCalculator != null, "Distance calculator is null");

		var distances = new Dictionary<Firework, double>(fireworks.Count() - 1);
		foreach (var firework in fireworks)
		{
			Debug.Assert(firework != null, "Firework is null");

			if (firework != bestFirework)
			{
				double distance = _distanceCalculator.Calculate(bestFirework, firework);
				distances.Add(firework, distance);
			}
		}

		return distances;
	}
}