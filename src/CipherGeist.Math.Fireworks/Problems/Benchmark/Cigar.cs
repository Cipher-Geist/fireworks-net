namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Cigar test function, as used in 2010 paper.
/// </summary>
public sealed class Cigar : BenchmarkProblem
{
	private const int _dimensionality = 2;
	private const double _minDimensionValue = -100.0;
	private const double _maxDimensionValue = 100.0;
	private const double _minInitialDimensionValue = 15.0;
	private const double _maxInitialDimensionValue = 30.0;
	private const double _knownBestQuality = 0.0;
	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Cigar"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="initialDimensionRanges">Initial dimension ranges, to be used to create initial fireworks.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Cigar(
		IList<Dimension> dimensions,
		IDictionary<Dimension, Interval> initialDimensionRanges,
		Func<IDictionary<Dimension, double>, double> targetFunction,
		Solution knownSolution,
		ProblemTarget target)
		: base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Cigar"/> class.
	/// </summary>
	/// <returns><see cref="Cigar"/> instance that represents Cigar test function, as used in 2010 paper.</returns>
	/// <param name="shift">Shift the function away from the default coordinate solution.</param>
	public static Cigar Create(bool shift = false)
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
				double sum = System.Math.Pow(c[dimensions[0]] - shiftValues[dimensions[0]], 2.0);
				for (int i = 1; i < _dimensionality; i++)
				{
					double value = c[dimensions[i]] - shiftValues[dimensions[i]];
					sum += System.Math.Pow(10.0, 6.0) * System.Math.Pow(value, 2.0);
				}

				return sum;
			});

		return new Cigar(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}