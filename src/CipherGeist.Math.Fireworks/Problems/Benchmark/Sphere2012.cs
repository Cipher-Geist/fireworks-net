namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Sphere test function, as used in 2012 paper.
/// </summary>
/// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
/// <remarks>https://www.lri.fr/~hansen/Tech-Report-May-30-05.pdf</remarks>
public sealed class Sphere2012 : BenchmarkProblem
{
	private const int _dimensionality = 10;

	private const double _minDimensionValue = -100.0;
	private const double _maxDimensionValue = 100.0;

	private const double _minInitialDimensionValue = 30.0;
	private const double _maxInitialDimensionValue = 50.0;

	private const double _knownBestQuality = -450.0;
	private const double _functionBias = -450.0;

	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Sphere2012"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
	/// create initial fireworks.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Sphere2012(
		IList<Dimension> dimensions,
		Func<IDictionary<Dimension, double>, double> targetFunction,
		Solution knownSolution,
		ProblemTarget target)
		: base(dimensions, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Sphere2012"/> class.
	/// </summary>
	/// <returns><see cref="Sphere2012"/> instance that represents
	/// Sphere test function, as used in 2012 paper.</returns>
	/// <param name="shift">Shift the function away from the default coordinate solution.</param>
	public static Sphere2012 Create(bool shift = false)
	{
		var dimensions = new Dimension[_dimensionality];
		var shiftValues = new Dictionary<Dimension, double>(_dimensionality);

		var knownBestCoordinates = new Dictionary<Dimension, double>(_dimensionality);
		var initialDimensionRanges = new Dictionary<Dimension, Interval>(_dimensionality);

		for (int i = 0; i < _dimensionality; i++)
		{
			dimensions[i] = new Dimension(new Interval(_minDimensionValue, _maxDimensionValue));
			initialDimensionRanges.Add(dimensions[i], new Interval(_minInitialDimensionValue, _maxInitialDimensionValue));

			shiftValues[dimensions[i]] = shift ?
				0.5 * (dimensions[i].Range.Maximum - dimensions[i].Range.Minimum) / 2 :
				0.0;

			knownBestCoordinates.Add(dimensions[i], shiftValues[dimensions[i]]);
		}

		var func = new Func<IDictionary<Dimension, double>, double>(
			(c) =>
			{
				return c.Sum(kvp => System.Math.Pow(kvp.Value - shiftValues[kvp.Key], 2.0)) + _functionBias;
			});

		return new Sphere2012(dimensions, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}