namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Beale test function..
/// </summary>
/// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
public class Beale : BenchmarkProblem
{
	private const int _dimensionality = 2;
	private const double _minDimensionValue = -4.5;
	private const double _maxDimensionValue = 4.5;
	private const double _knownBestQuality = 0.0;
	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Beale"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Beale(
		IList<Dimension> dimensions, 
		Func<IDictionary<Dimension, double>, double> targetFunction,
		Solution knownSolution, 
		ProblemTarget target)
			: base(dimensions, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Beale"/> class.
	/// </summary>
	/// <returns><see cref="Beale"/> instance that represents Beale test function.</returns>
	public static Beale Create()
	{
		var dimensions = new Dimension[_dimensionality];
		var knownBestCoordinates = new Dictionary<Dimension, double>(_dimensionality);

		for (int i = 0; i < _dimensionality; i++)
		{
			dimensions[i] = new Dimension(new Interval(_minDimensionValue, _maxDimensionValue));
		}

		knownBestCoordinates.Add(dimensions[0], 3.0);
		knownBestCoordinates.Add(dimensions[1], 0.5);

		var func = new Func<IDictionary<Dimension, double>, double>(
			(c) =>
			{
				double d1 = System.Math.Pow(1.500 - c[dimensions[0]] + (c[dimensions[0]] * c[dimensions[1]]), 2);
				double d2 = System.Math.Pow(2.250 - c[dimensions[0]] + (c[dimensions[0]] * System.Math.Pow(c[dimensions[1]], 2)), 2);
				double d3 = System.Math.Pow(2.625 - c[dimensions[0]] + (c[dimensions[0]] * System.Math.Pow(c[dimensions[1]], 3)), 2);
				return d1 + d2 + d3;
			});

		return new Beale(dimensions, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}