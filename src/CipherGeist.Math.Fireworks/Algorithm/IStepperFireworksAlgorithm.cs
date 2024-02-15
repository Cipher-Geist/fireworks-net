﻿namespace CipherGeist.Math.Fireworks.Algorithm;

/// <summary>
/// Fireworks algorithm with possibility of step-by-step execution.
/// </summary>
public interface IStepperFireworksAlgorithm : IFireworksAlgorithm
{
	/// <summary>
	/// Creates the initial algorithm state (before the run starts).
	/// </summary>
	/// <returns>Instane of class implementing <see cref="IAlgorithmState"/>, that represents 
	/// initial state (before the run starts).</returns>
	IAlgorithmState Initialize();

	/// <summary>
	/// Makes another iteration of the algorithm.
	/// </summary>
	/// <returns>State of the algorithm after the step.</returns>
	IAlgorithmState MakeStep();

	/// <summary>
	/// Tells if no further steps should be made.
	/// </summary>
	/// <returns><c>true</c> if next step should be made. Otherwise <c>false</c>.</returns>
	bool ShouldStop();

	/// <summary>
	/// Fired when any of the stopping conditions are met.
	/// </summary>
	event EventHandler<AlgorithmStateEventArgs> OnStopConditionSatisfied;

	/// <summary>
	/// Fired when a step of the IStepperFireworksAlgorithm is completed.
	/// </summary>
	event EventHandler<AlgorithmStateEventArgs> OnStepCompleted;
}