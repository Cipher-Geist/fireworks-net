﻿namespace CipherGeist.Math.Fireworks.State;

/// <summary>
/// Represents current algorithm state.
/// </summary>
public interface IAlgorithmState
{
	/// <summary>
	/// Gets a collection of current fireworks.
	/// </summary>
	IEnumerable<Firework> Fireworks { get; }

	/// <summary>
	/// Gets the step number.
	/// </summary>
	int StepNumber { get; }

	/// <summary>
	/// Gets the best solution among Fireworks.
	/// </summary>
	Solution BestSolution { get; }
}