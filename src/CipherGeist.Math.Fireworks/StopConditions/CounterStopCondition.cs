namespace CipherGeist.Math.Fireworks.StopConditions;

/// <summary>
/// Counts something; stops when count exceeds some threshold.
/// </summary>
public class CounterStopCondition : IStopCondition
{
	private int _count;

	/// <summary>
	/// Initializes a new instance of the <see cref="CounterStopCondition"/> class.
	/// </summary>
	/// <param name="threshold">The threshold.</param>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="threshold"/> is less than zero.</exception>
	public CounterStopCondition(int threshold)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(threshold);

		_count = 0;
		Threshold = threshold;
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
	public virtual bool ShouldStop(IAlgorithmState state)
	{
		return _count >= Threshold;
	}

	/// <summary>
	/// Increments the internal counter.
	/// </summary>
	public virtual void IncrementCounter()
	{
		Interlocked.Increment(ref _count);
	}

	/// <summary>
	/// Increments the internal counter. Can be used as an event handler.
	/// </summary>
	/// <param name="sender">The event sender.</param>
	/// <param name="eventArgs">The event arguments.</param>
	public virtual void IncrementCounter(object sender, object eventArgs)
	{
		IncrementCounter();
	}

	/// <summary>
	/// Gets the threshold. Exceeding this threshold is a
	/// stop condition for <see cref="CounterStopCondition"/> user.
	/// </summary>
	public int Threshold { get; private set; }
}