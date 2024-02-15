namespace CipherGeist.Math.Fireworks.Problems;

/// <summary>
/// Describes a problem that is to be solved.
/// </summary>
public class Problem
{
	private readonly Func<IDictionary<Dimension, double>, double> _targetFunction;

	/// <summary>
	/// Initializes a new instance of <see cref="Problem"/> type.
	/// </summary>
	/// <param name="dimensions">The dimensions of the problem.</param>
	/// <param name="initialDimensionRanges">The initial ranges of the problem dimensions.</param>
	/// <param name="targetFunction">The quality function that needs to be optimized.</param>
	/// <param name="target">Target of the problem.</param>
	/// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> or
	/// <paramref name="initialDimensionRanges"/> or <paramref name="targetFunction"/> is <c>null</c>.
	/// </exception>
	/// <exception cref="ArgumentException">if <paramref name="dimensions"/>.Count is zero or
	/// <paramref name="dimensions"/>.Count differs from <paramref name="initialDimensionRanges"/>.Count
	/// or <paramref name="dimensions"/> does not contain same keys as <paramref name="initialDimensionRanges"/>.
	/// </exception>
	public Problem(
		IList<Dimension> dimensions,
		IDictionary<Dimension, Interval> initialDimensionRanges,
		Func<IDictionary<Dimension, double>, double> targetFunction,
		ProblemTarget target)
	{
		ArgumentNullException.ThrowIfNull(dimensions);
		if (dimensions.Count == 0)
		{
			throw new ArgumentException(string.Empty, nameof(dimensions));
		}

		ArgumentNullException.ThrowIfNull(initialDimensionRanges);
		if (initialDimensionRanges.Count != dimensions.Count)
		{
			throw new ArgumentException(string.Empty, nameof(initialDimensionRanges));
		}

		foreach (Dimension variable in dimensions)
		{
			if (!initialDimensionRanges.ContainsKey(variable))
			{
				throw new ArgumentException(string.Empty, nameof(initialDimensionRanges));
			}
		}

		Dimensions = dimensions;
		InitialDimensionRanges = initialDimensionRanges;
		_targetFunction = targetFunction ?? throw new ArgumentNullException(nameof(targetFunction));
		Target = target;
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Problem"/> type with default initial ranges.
	/// </summary>
	/// <param name="dimensions">The dimensions of the problem.</param>
	/// <param name="targetFunction">The quality function that needs to be optimized.</param>
	/// <param name="target">Target of the problem.</param>
	/// <param name="description">The problem description.</param>
	/// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> or
	/// <paramref name="targetFunction"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException">if <paramref name="dimensions"/>.Count is zero.
	/// </exception>
	public Problem(
		IList<Dimension> dimensions, 
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		ProblemTarget target, 
		string description)
			: this(dimensions, CreateDefaultInitialRanges(dimensions), targetFunction, target)
	{
		Description = description;
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Problem"/> type with default initial ranges.
	/// </summary>
	/// <param name="dimensions">The dimensions of the problem.</param>
	/// <param name="targetFunction">The quality function that needs to be optimized.</param>
	/// <param name="target">Target of the problem.</param>
	/// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> or
	/// <paramref name="targetFunction"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException">if <paramref name="dimensions"/>.Count is zero.
	/// </exception>
	public Problem(
		IList<Dimension> dimensions, 
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		ProblemTarget target)
			: this(dimensions, CreateDefaultInitialRanges(dimensions), targetFunction, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Problem"/> type.
	/// </summary>
	/// <param name="dimensions">The dimensions of the problem.</param>
	/// <param name="targetFunction">The quality function that needs to be optimized.</param>
	/// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> or 
	/// <paramref name="targetFunction"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentException">if <paramref name="dimensions"/>.Count is zero.
	/// </exception>
	public Problem(
		IList<Dimension> dimensions, 
		Func<IDictionary<Dimension, double>, double> targetFunction)
			: this(dimensions, CreateDefaultInitialRanges(dimensions), targetFunction, ProblemTarget.Minimum)
	{
	}

	/// <summary>
	/// Calculates the quality of the solution.
	/// </summary>
	/// <param name="coordinateValues">The solution coordinates.</param>
	/// <returns>Quality of the solution at <paramref name="coordinateValues"/> coordinates.</returns>
	/// <exception cref="ArgumentNullException">if <paramref name="coordinateValues"/> is <c>null</c>.</exception>
	public virtual double CalculateQuality(IDictionary<Dimension, double> coordinateValues)
	{
		ArgumentNullException.ThrowIfNull(coordinateValues);

		Debug.Assert(_targetFunction != null, "Target function is null");

		OnQualityCalculating(new QualityCalculatingEventArgs(coordinateValues));
		var result = _targetFunction(coordinateValues);

		OnQualityCalculated(new QualityCalculatedEventArgs(coordinateValues, result));
		return result;
	}

	/// <summary>
	/// Creates the collection of initial ranges for given <see cref="Dimension"/>s from
	/// ranges of <paramref name="dimensions"/>.
	/// </summary>
	/// <param name="dimensions">The collection of <see cref="Dimension"/>s to create initial ranges for.</param>
	/// <returns>The collection of initial ranges for given <see cref="Dimension"/>s from ranges of <paramref name="dimensions"/>.</returns>
	/// <exception cref="ArgumentNullException">if <paramref name="dimensions"/> is <c>null</c>.</exception>
	public static IDictionary<Dimension, Interval> CreateDefaultInitialRanges(IList<Dimension> dimensions)
	{
		ArgumentNullException.ThrowIfNull(dimensions);

		var initialRanges = new Dictionary<Dimension, Interval>(dimensions.Count);
		foreach (Dimension dimension in dimensions)
		{
			initialRanges.Add(dimension, dimension.Range);
		}

		return initialRanges;
	}

	/// <summary>
	/// Firing an event before calculating quality of a firework.
	/// </summary>
	/// <param name="eventArgs">Event arguments.</param>
	protected virtual void OnQualityCalculating(QualityCalculatingEventArgs eventArgs)
	{
		QualityCalculating?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Firing an event after calculating quality of a firework.
	/// </summary>
	/// <param name="eventArgs">Event arguments.</param>
	protected virtual void OnQualityCalculated(QualityCalculatedEventArgs eventArgs)
	{
		QualityCalculated?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Gets the dimensions of the problem.
	/// </summary>
	public IList<Dimension> Dimensions { get; private set; }

	/// <summary>
	/// Gets the initial ranges for the problem dimensions.
	/// </summary>
	public IDictionary<Dimension, Interval> InitialDimensionRanges { get; private set; }

	/// <summary>
	/// Gets the target of the problem (minimize or maximize it).
	/// </summary>
	public ProblemTarget Target { get; private set; }

	/// <summary>
	/// Gets the problem description. 
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Fired before the solution quality is calculated.
	/// </summary>
	public event EventHandler<QualityCalculatingEventArgs>? QualityCalculating;

	/// <summary>
	/// Fired after the solution quality is calculated.
	/// </summary>
	public event EventHandler<QualityCalculatedEventArgs>? QualityCalculated;
}