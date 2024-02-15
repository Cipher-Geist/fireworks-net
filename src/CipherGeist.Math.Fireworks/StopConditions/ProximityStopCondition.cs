namespace CipherGeist.Math.Fireworks.StopConditions;

/// <summary>
/// Stops when some <see cref="ISolution"/> (typically the current best one)
/// approaches close enough to some expected <see cref="ISolution"/>.
/// </summary>
public abstract class ProximityStopCondition : IStopCondition
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ProximityStopCondition"/> class.
	/// </summary>
	/// <param name="expectation">The the expected <see cref="ISolution"/> (an etalon).</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="expectation"/> is <c>null</c>.</exception>
	protected ProximityStopCondition(ISolution expectation)
	{
		Expectation = expectation ?? throw new ArgumentNullException(nameof(expectation));
	}

	/// <summary>
	/// Tells if an algorithm that is currently in <paramref name="state"/> state
	/// should stop (and don't make further steps) or not.
	/// </summary>
	/// <param name="state">The current algorithm state.</param>
	/// <returns>
	/// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
	/// state should stop (and don't make further steps). Otherwise <c>false</c>.
	/// </returns>
	public abstract bool ShouldStop(IAlgorithmState state);

	/// <summary>
	/// Gets the expected <see cref="Solution"/> (an etalon).
	/// </summary>
	public ISolution Expectation { get; private set; }
}