namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Rastrigrin test function, as used in 2010 paper.
/// </summary>
/// <remarks>http://en.wikipedia.org/wiki/Rastrigin_function</remarks>
public sealed class Rastrigrin : BenchmarkProblem
{
	private const int _dimensionality = 2;
	private const double _minDimensionValue = -100.0;
	private const double _maxDimensionValue = 100.0;
	private const double _minInitialDimensionValue = 30.0;
	private const double _maxInitialDimensionValue = 50.0;
	private const double _knownBestQuality = 0.0;
	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Rastrigrin"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="initialDimensionRanges">Initial dimension ranges, to be used to create initial fireworks.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Rastrigrin(
		IList<Dimension> dimensions, 
		IDictionary<Dimension, Interval> initialDimensionRanges, 
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		Solution knownSolution, 
		ProblemTarget target)
			: base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Rastrigrin"/> class.
	/// </summary>
	/// <returns><see cref="Rastrigrin"/> instance that represents
	/// Rastrigrin test function, as used in 2010 paper.</returns>
	public static Rastrigrin Create()
	{
		var dimensions = new Dimension[_dimensionality];
		var initialDimensionRanges = new Dictionary<Dimension, Interval>(_dimensionality);
		var knownBestCoordinates = new Dictionary<Dimension, double>(_dimensionality);

		for (int i = 0; i < _dimensionality; i++)
		{
			dimensions[i] = new Dimension(new Interval(_minDimensionValue, _maxDimensionValue));
			initialDimensionRanges.Add(dimensions[i], new Interval(_minInitialDimensionValue, _maxInitialDimensionValue));
			knownBestCoordinates.Add(dimensions[i], 0.0);
		}

		var func = new Func<IDictionary<Dimension, double>, double>(
			(c) =>
			{
				return c.Values.Sum(v => System.Math.Pow(v, 2.0) - (10.0 * System.Math.Cos(2.0 * System.Math.PI * v)) + 10.0);
			});

		return new Rastrigrin(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}