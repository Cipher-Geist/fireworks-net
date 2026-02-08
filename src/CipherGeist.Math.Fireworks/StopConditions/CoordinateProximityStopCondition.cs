namespace CipherGeist.Math.Fireworks.StopConditions;

/// <summary>
/// Stops when coordinates of some <see cref="Solution"/> (typically
/// the current best one) gets close enough to the coordinates of
/// the etalon <see cref="Solution"/>.
/// </summary>
public class CoordinateProximityStopCondition : ProximityStopCondition
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CoordinateProximityStopCondition"/> class.
	/// </summary>
	/// <param name="expectation">The the expected <see cref="Solution"/> (an etalon).</param>
	/// <param name="distanceCalculator">The distance calculator.</param>
	/// <param name="distanceThreshold">The distance threshold.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="distanceCalculator"/>
	/// is <c>null</c>.</exception>
	/// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="distanceThreshold"/>
	/// is <see cref="double.NaN"/> or <see cref="double.PositiveInfinity"/> or
	/// <see cref="double.NegativeInfinity"/>.</exception>
	public CoordinateProximityStopCondition(
		ISolution expectation, 
		IDistance distanceCalculator, 
		double distanceThreshold)
			: base(expectation)
	{
		if (double.IsNaN(distanceThreshold) || double.IsInfinity(distanceThreshold))
		{
			throw new ArgumentOutOfRangeException(nameof(distanceThreshold));
		}

		DistanceCalculator = distanceCalculator ?? throw new ArgumentNullException(nameof(distanceCalculator));
		DistanceThreshold = distanceThreshold;
	}

	/// <summary>
	/// Tells if an algorithm that is currently in <paramref name="state"/> state should stop (and 
	/// don't make further steps) or not. Stops if the distance between the <paramref name="state"/>'s Best 
	/// Solution and the <see cref="CoordinateProximityStopCondition"/>. Expectation is less than
	/// or equal to <see cref="CoordinateProximityStopCondition"/>.DistanceThreshold.
	/// </summary>
	/// <param name="state">The current algorithm state.</param>
	/// <returns>
	/// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
	/// state should stop (and don't make further steps). Otherwise <c>false</c>.
	/// </returns>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="state"/> is <c>null</c>.</exception>
	public override bool ShouldStop(IAlgorithmState state)
	{
		ArgumentNullException.ThrowIfNull(state);

		Debug.Assert(DistanceCalculator != null, "Distance calculator is null");
		Debug.Assert(state.BestSolution != null, "State best solution is null");
		Debug.Assert(Expectation != null, "Expectation is null");

		Debug.Assert(!double.IsNaN(DistanceThreshold), "Distance threshold is NaN");
		Debug.Assert(!double.IsInfinity(DistanceThreshold), "Distance threshold is Infinity");

		double distance = DistanceCalculator.Calculate(state.BestSolution, Expectation);

		Debug.Assert(!double.IsNaN(distance), "Distance is NaN");
		Debug.Assert(!double.IsInfinity(distance), "Distance is Infinity");

		return distance.IsLessOrEqual(DistanceThreshold);
	}

	/// <summary>
	/// Gets the distance calculator.
	/// </summary>
	public IDistance DistanceCalculator { get; private set; }

	/// <summary>
	/// Gets the distance threshold.
	/// </summary>
	public double DistanceThreshold { get; private set; }
}