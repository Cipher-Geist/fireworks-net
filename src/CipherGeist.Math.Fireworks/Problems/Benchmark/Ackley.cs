namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Ackley test function, as used in 2010 paper.
/// </summary>
/// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
public sealed class Ackley : BenchmarkProblem
{
	private const int _dimensionality = 10;
	private const double _minDimensionValue = -5.0;
	private const double _maxDimensionValue = 5.0;
	private const double _minInitialDimensionValue = -5.0;
	private const double _maxInitialDimensionValue = 5.0;
	private const double _knownBestQuality = 0.0;
	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Ackley"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="initialDimensionRanges">Initial dimension ranges, to be used to create initial fireworks.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Ackley(
		IList<Dimension> dimensions, 
		IDictionary<Dimension, Interval> initialDimensionRanges, 
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		Solution knownSolution, 
		ProblemTarget target)
			: base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Ackley"/> class.
	/// </summary>
	/// <returns><see cref="Ackley"/> instance that represents
	/// Ackley test function, as used in 2010 paper.</returns>
	public static Ackley Create()
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
				double firstSum = 0.0;
				double secondSum = 0.0;
				foreach (double value in c.Values)
				{
					firstSum += System.Math.Pow(value, 2.0);
					secondSum += System.Math.Cos(2.0 * System.Math.PI * System.Math.Pow(value, 2.0));
				}

				return 20.0 + System.Math.E - (20.0 * System.Math.Exp(-0.2 * System.Math.Sqrt((1 / _dimensionality) * firstSum))) - System.Math.Exp((1 / _dimensionality) * secondSum);
			});

		return new Ackley(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}