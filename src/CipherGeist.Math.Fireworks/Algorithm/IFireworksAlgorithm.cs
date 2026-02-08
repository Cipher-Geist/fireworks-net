namespace CipherGeist.Math.Fireworks.Algorithm;

/// <summary>
/// Represents a fireworks algorithm.
/// </summary>
public interface IFireworksAlgorithm
{
	/// <summary>
	/// Solves the specified problem by running the algorithm.
	/// </summary>
	/// <returns><see cref="Solution"/> instance that represents best solution found during the algorithm run.</returns>
	Solution? Solve();

	/// <summary>
	/// Gets the problem to be solved by the algorithm.
	/// </summary>
	Problem? ProblemToSolve { get; }

	/// <summary>
	/// Gets the stop condition for the algorithm.
	/// </summary>
	IStopCondition? StopCondition { get; }

	/// <summary>
	/// Gets the extremum firework selector.
	/// </summary>
	IExtremumFireworkSelector? BestWorstFireworkSelector { get; }
}