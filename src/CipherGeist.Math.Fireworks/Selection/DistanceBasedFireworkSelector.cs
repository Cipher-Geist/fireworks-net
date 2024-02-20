﻿namespace CipherGeist.Math.Fireworks.Selection;

/// <summary>
/// Selects <see cref="Firework"/>s that will stay around for the next step
/// based on the distance between the <see cref="Firework"/>s, per 2010 paper.
/// </summary>
public class DistanceBasedFireworkSelector : FireworkSelectorBase
{
	private readonly IDistance _distanceCalculator;
	private readonly IExtremumFireworkSelector _extremumFireworkSelector;

	/// <summary>
	/// Initializes a new instance of the <see cref="DistanceBasedFireworkSelector"/> class.
	/// </summary>
	/// <param name="distanceCalculator">The distance calculator.</param>
	/// <param name="extremumFireworkSelector">The extremum firework selector.</param>
	/// <param name="locationsNumber">The number of <see cref="Firework"/>s to be selected.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="distanceCalculator"/>
	/// or <paramref name="extremumFireworkSelector"/> is <c>null</c>.
	/// </exception>
	public DistanceBasedFireworkSelector(
		IDistance distanceCalculator,
		IExtremumFireworkSelector extremumFireworkSelector,
		int locationsNumber)
			: base(locationsNumber)
	{
		_distanceCalculator = distanceCalculator;
		_extremumFireworkSelector = extremumFireworkSelector;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DistanceBasedFireworkSelector"/> class.
	/// </summary>
	/// <param name="distanceCalculator">The distance calculator.</param>
	/// <param name="extremumFireworkSelector">The extremum firework selector.</param>
	/// <remarks>It is assumed that number of <see cref="Firework"/>s to be selected
	/// differs from step to step and hence is passed to the <c>Select</c> method.</remarks>
	public DistanceBasedFireworkSelector(IDistance distanceCalculator, IExtremumFireworkSelector extremumFireworkSelector)
		: this(distanceCalculator, extremumFireworkSelector, 0)
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

		if (numberToSelect == 0)
		{
			return Enumerable.Empty<Firework>();
		}

		if (numberToSelect == from.Count())
		{
			return new List<Firework>(from);
		}

		// Have List<T> instead of IList<T> here since List<>.AddRange() is used below.
		var selectedLocations = new List<Firework>(numberToSelect);

		// 1. Find a firework with best quality - it will be kept anyways
		var bestFirework = _extremumFireworkSelector.SelectBest(from);
		selectedLocations.Add(bestFirework);

		if (numberToSelect > 1)
		{
			// 2. Calculate distances between all fireworks
			var distances = CalculateDistances(from);
			Debug.Assert(distances != null, "Distance collection is null");

			// 3. Calculate probabilities for each firework
			var probabilities = CalculateProbabilities(distances);
			Debug.Assert(probabilities != null, "Probability collection is null");

			// 4. Select desiredLocationsNumber - 1 of fireworks based on the probabilities
			var sortedProbabilities = probabilities.OrderByDescending(p => p.Value, new DoubleExtensionComparer());
			Debug.Assert(sortedProbabilities != null, "Sorted probabilities collection is null");

			var otherSelectedLocations = sortedProbabilities
				.Where(sp => sp.Key != bestFirework)
				.Take(numberToSelect - 1)
				.Select(sp => sp.Key);
			Debug.Assert(otherSelectedLocations != null, "Other selected locations collection is null");

			selectedLocations.AddRange(otherSelectedLocations);
		}

		return selectedLocations;
	}

	/// <summary>
	/// Calculates the sums of distances between each <see cref="Firework"/> and other <see cref="Firework"/>s.
	/// </summary>
	/// <param name="fireworks">The collection of <see cref="Firework"/>s to calculate distances between.</param>
	/// <returns>A map. Key is a <see cref="Firework"/>. Value is a sum of distances
	/// between that <see cref="Firework"/> and all other <see cref="Firework"/>s in the
	/// <paramref name="fireworks"/> collection.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="fireworks"/> is <c>null</c>.</exception>
	protected virtual IDictionary<Firework, double> CalculateDistances(IEnumerable<Firework> fireworks)
	{
		// TODO: Maybe there is more efficient way to calc distance between each and each points?
		Debug.Assert(_distanceCalculator != null, "Distance calculator is null");

		var distances = new Dictionary<Firework, double>(fireworks.Count());
		foreach (var firework in fireworks)
		{
			Debug.Assert(firework != null, "Firework is null");

			distances.Add(firework, 0.0);
			foreach (var otherFirework in fireworks)
			{
				Debug.Assert(otherFirework != null, "Other firework is null");

				distances[firework] += _distanceCalculator.Calculate(firework, otherFirework);
			}
		}

		return distances;
	}

	/// <summary>
	/// Calculates the probabilities of each firework to be selected.
	/// </summary>
	/// <param name="distances">The sums of distances between each <see cref="Firework"/> and other 
	/// <see cref="Firework"/>s.</param>
	/// <returns>A map. Key is a <see cref="Firework"/>. Value is a probability for that <see cref="Firework"/>
	/// to be selected.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="distances"/> is <c>null</c>.</exception>
	protected virtual IDictionary<Firework, double> CalculateProbabilities(IDictionary<Firework, double> distances)
	{
		ArgumentNullException.ThrowIfNull(distances);

		var probabilities = new Dictionary<Firework, double>(distances.Count());
		var distancesSum = distances.Values.Sum();

		Debug.Assert(!distancesSum.IsEqual(0.0), "Distances sum is 0");

		foreach (var distance in distances)
		{
			Debug.Assert(distance.Key != null, "Firework is null");

			var probability = distance.Value / distancesSum;
			probabilities.Add(distance.Key, probability);
		}

		return probabilities;
	}
}