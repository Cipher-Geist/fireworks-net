namespace CipherGeist.Math.Fireworks.Algorithm;

/// <summary>
/// Base class for Fireworks Algorithm implementation.
/// </summary>
/// <typeparam name="TSettings">Algorithm settings type.</typeparam>
public abstract class StepperFireworksAlgorithmBase<TSettings> :
	FireworksAlgorithmBase<TSettings>,
	IStepperFireworksAlgorithm 
		where TSettings : class
{
	/// <summary>
	/// The current algorithm state.
	/// </summary>
	protected AlgorithmState? _state;

	/// <summary>
	/// Initializes a new instance of the <see cref="FireworksAlgorithmBase{TSettings}"/> class.
	/// </summary>
	/// <param name="problem">The problem to be solved by the algorithm.</param>
	/// <param name="stopCondition">The stop condition for the algorithm.</param>
	/// <param name="settings">The algorithm settings.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="problem"/> or <paramref name="stopCondition"/> 
	/// or <paramref name="settings"/> is <c>null</c>.</exception>
	protected StepperFireworksAlgorithmBase(Problem problem, IStopCondition stopCondition, TSettings settings)
		: base(problem, stopCondition, settings)
	{
	}

	/// <summary>
	/// Creates the initial algorithm state (before the run starts).
	/// </summary>
	/// <returns>Instane of class implementing <see cref="IAlgorithmState"/>, that represents 
	/// initial state (before the run starts).</returns>
	public IAlgorithmState Initialize()
	{
		InitializeImpl();
		ArgumentNullException.ThrowIfNull(_state, nameof(_state));
		return new AlgorithmState(_state.Fireworks, _state.StepNumber, _state.BestSolution);
	}

	/// <summary>
	/// Creates the initial algorithm state (before the run starts).
	/// </summary>
	/// <remarks>On each call re-creates the initial state (i.e. returns new object each time).</remarks>
	protected abstract void InitializeImpl();

	/// <summary>
	/// Solves the specified problem by running the algorithm.
	/// </summary>
	/// <returns><see cref="Solution"/> instance that represents
	/// best solution found during the algorithm run.</returns>
	public override Solution? Solve()
	{
		InitializeImpl();

		while (!ShouldStop())
		{
			MakeStepImpl();
		}

		return _state?.BestSolution;
	}

	/// <summary>
	/// Makes another iteration of the algorithm.
	/// </summary>
	/// <returns>State of the algorithm after the step.</returns>
	public IAlgorithmState MakeStep()
	{
		MakeStepImpl();
		Debug.Assert(_state != null, "State is null");

		OnStepCompleted?.Invoke(this, new AlgorithmStateEventArgs(_state));
		return new AlgorithmState(_state.Fireworks, _state.StepNumber, _state.BestSolution);
	}

	/// <summary>
	/// Makes another iteration of the algorithm.
	/// </summary>
	protected abstract void MakeStepImpl();

	/// <summary>
	/// Tells if no further steps should be made.
	/// </summary>
	/// <returns><c>true</c> if next step should be made. Otherwise <c>false</c>.</returns>
	public bool ShouldStop()
	{
		bool stopConditionSatisfied = StopCondition.ShouldStop(_state!);
		if (stopConditionSatisfied)
		{
			OnStopConditionSatisfied?.Invoke(this, new AlgorithmStateEventArgs(_state!));
		}

		return stopConditionSatisfied;
	}

	/// <summary>
	/// Raise when a step of the IStepperFireworksAlgorithm is completed.
	/// </summary>
	/// <param name="e">The event arguments.</param>
	protected virtual void RaiseStepCompleted(AlgorithmStateEventArgs e)
	{
		OnStepCompleted?.Invoke(this, e);
	}

	/// <summary>
	/// Fired when any of the stopping conditions are met.
	/// </summary>
	public event EventHandler<AlgorithmStateEventArgs>? OnStopConditionSatisfied;

	/// <summary>
	/// Fired when a step of the IStepperFireworksAlgorithm is completed.
	/// </summary>
	public event EventHandler<AlgorithmStateEventArgs>? OnStepCompleted;
}