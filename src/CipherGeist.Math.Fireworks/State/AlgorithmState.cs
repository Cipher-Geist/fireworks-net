namespace CipherGeist.Math.Fireworks.State;

/// <summary>
/// Stores current algorithm state.
/// </summary>
/// <remarks>This class is not thread-safe.</remarks>
public class AlgorithmState : IAlgorithmState
{
	private IEnumerable<Firework> _fireworks;
	private int _stepNumber;
	private Solution _bestSolution;

	/// <summary>
	/// Initializes a new instance of <see cref="AlgorithmState"/>.
	/// </summary>
	/// <param name="fireworks">A collection of fireworks for this state.</param>
	/// <param name="stepNumber">Current step number.</param>
	/// <param name="bestSolution">The best solution in this state.</param>
	public AlgorithmState(IEnumerable<Firework> fireworks, int stepNumber, Solution bestSolution)
	{
		Id = new TId();
		Fireworks = fireworks ?? [];
		StepNumber = stepNumber;
		BestSolution = bestSolution ?? new Solution(0.001);
	}

	/// <summary>
	/// Gets unique state identifier.
	/// </summary>
	public TId Id { get; private set; }

	/// <summary>
	/// Gets or sets a collection of current fireworks.
	/// </summary>
	/// <exception cref="ArgumentNullException"> if <paramref name="value"/> is <c>null</c>.</exception>
	public IEnumerable<Firework> Fireworks
	{
		get { return _fireworks; }
		set
		{
			_fireworks = value ?? [];
		}
	}

	/// <summary>
	/// Gets or sets the step number.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="value"/> is less than zero.</exception>
	public int StepNumber
	{
		get { return _stepNumber; }
		set
		{
			ArgumentOutOfRangeException.ThrowIfNegative(value);
			_stepNumber = value;
		}
	}

	/// <summary>
	/// Gets or sets the best solution among <see cref="AlgorithmState"/>.Fireworks.
	/// </summary>
	/// <exception cref="ArgumentNullException"> if <paramref name="value"/> is <c>null</c>.</exception>
	public Solution BestSolution
	{
		get => _bestSolution;
		set
		{
			_bestSolution = value ?? throw new ArgumentNullException(nameof(value));
		}
	}
}