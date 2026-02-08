namespace CipherGeist.Math.Fireworks.StopConditions;

/// <summary>
/// Stops when the quality of the current best <see cref="ISolution"/> does not 
/// change for a pre-determined number of iterations/steps.
/// </summary>
public class ConvergenceStopCondition : CounterStopCondition
{
	private Solution? _currentSolution;
	private int _stagnationCount;

	/// <summary>
	/// Initializes a new instance of the <see cref=" ConvergenceStopCondition"/> class.
	/// </summary>
	/// <param name="threshold">The threshold.</param>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="threshold"/> is less than zero.</exception>
	public ConvergenceStopCondition(int threshold) 
		: base(threshold)
	{
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
	public override bool ShouldStop(IAlgorithmState state)
	{
		try
		{
			if (state.BestSolution == _currentSolution)
			{
				Interlocked.Increment(ref _stagnationCount);
				if (_stagnationCount >= Threshold)
				{
					return true;
				}
			}

			_stagnationCount = 0;
			return false;
		}
		finally
		{
			_currentSolution = state.BestSolution;
		}
	}
}