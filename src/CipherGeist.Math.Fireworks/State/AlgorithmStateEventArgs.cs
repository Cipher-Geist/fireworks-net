namespace CipherGeist.Math.Fireworks.State;

/// <summary>
/// Allows the algorithm state to be used as an event argument. 
/// </summary>
public class AlgorithmStateEventArgs : EventArgs
{
	/// <summary>
	/// Ctor.
	/// </summary>
	/// <param name="algorithmState">The algorithm state.</param>
	public AlgorithmStateEventArgs(IAlgorithmState? algorithmState)
	{
		AlgorithmState = algorithmState;
	}

	/// <summary>
	/// Ctor.
	/// </summary>
	/// <param name="problem">Provides the information for the problem being solved.</param>
	/// <param name="algorithmState">The algorithm state.</param>
	public AlgorithmStateEventArgs(Problem? problem, IAlgorithmState? algorithmState)
		: this(algorithmState)
	{
		Problem = problem;
	}

	/// <summary>
	/// Gets the information for the problem being solved.
	/// </summary>
	public Problem? Problem { get; }

	/// <summary>
	/// Gets the algorithm state for the event.
	/// </summary>
	public IAlgorithmState? AlgorithmState { get; }
}