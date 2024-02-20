namespace CipherGeist.Math.Fireworks.Selection;

/// <summary>
/// Selects <see cref="Firework"/>s that will stay around for the next step:
/// takes the best <see cref="Firework"/> and randomly chooses others, per 2012 paper.
/// </summary>
public class BestAndRandomFireworkSelector : FireworkSelectorBase
{
	private readonly System.Random _randomizer;
	private readonly Func<IEnumerable<Firework>, Firework> _bestFireworkSelector;

	/// <summary>
	/// Initializes a new instance of the <see cref="BestAndRandomFireworkSelector"/> class.
	/// </summary>
	/// <param name="randomizer">The random number generator.</param>
	/// <param name="bestFireworkSelector">The function that can be used to select best <see cref="Firework"/>.</param>
	/// <param name="locationsNumber">The number of <see cref="Firework"/>s to be selected.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="randomizer"/>
	/// or <paramref name="bestFireworkSelector"/> is <c>null</c>.
	/// </exception>
	public BestAndRandomFireworkSelector(
		System.Random randomizer,
		Func<IEnumerable<Firework>,
		Firework> bestFireworkSelector,
		int locationsNumber)
		: base(locationsNumber)
	{
		_randomizer = randomizer ?? throw new ArgumentNullException(nameof(randomizer));
		_bestFireworkSelector = bestFireworkSelector ?? throw new ArgumentNullException(nameof(bestFireworkSelector));
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BestAndRandomFireworkSelector"/> class.
	/// </summary>
	/// <param name="randomizer">The random number generator.</param>
	/// <param name="bestFireworkSelector">The function that can be used to select 
	/// best <see cref="Firework"/>.</param>
	/// <remarks>It is assumed that number of <see cref="Firework"/>s to be selected
	/// differs from step to step and hence is passed to the <c>Select</c> method.</remarks>
	public BestAndRandomFireworkSelector(System.Random randomizer, Func<IEnumerable<Firework>, Firework> bestFireworkSelector)
		: this(randomizer, bestFireworkSelector, 0)
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

		var selectedLocations = new List<Firework>(numberToSelect);
		if (numberToSelect == 0)
		{
			return selectedLocations;
		}

		// 1. Find a firework with best quality - it will be kept anyways.
		var bestFirework = _bestFireworkSelector(from);
		selectedLocations.Add(bestFirework);

		if (numberToSelect > 1)
		{
			// 2. Select others randomly
			var fromWithoutBest = new List<Firework>(from);
			fromWithoutBest.Remove(bestFirework);

			int currentFirework = 0;
			var selectedFireworksIndices = _randomizer.NextUniqueInt32s(numberToSelect - 1, 0, fromWithoutBest.Count());
			foreach (var firework in fromWithoutBest)
			{
				Debug.Assert(firework != null, "Firework is null");

				if (selectedFireworksIndices.Contains(currentFirework))
				{
					selectedLocations.Add(firework);
				}

				currentFirework++;
			}
		}

		return selectedLocations;
	}
}