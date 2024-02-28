namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Levi test function..
/// </summary>
/// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
public class Levi : BenchmarkProblem
{
	private const int _dimensionality = 2;
	private const double _minDimensionValue = -10.0;
	private const double _maxDimensionValue = 10.0;
	private const double _knownBestQuality = 0.0;
	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Levi"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Levi(
		IList<Dimension> dimensions, 
		Func<IDictionary<Dimension, double>, double> targetFunction,
		Solution knownSolution, 
		ProblemTarget target)
			: base(dimensions, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Levi"/> class.
	/// </summary>
	/// <returns><see cref="Levi"/> instance that represents Beale test function.</returns>
	public static Levi Create()
	{
		var dimensions = new Dimension[_dimensionality];
		var knownBestCoordinates = new Dictionary<Dimension, double>(_dimensionality);

		for (int i = 0; i < _dimensionality; i++)
		{
			dimensions[i] = new Dimension(new Interval(_minDimensionValue, _maxDimensionValue));
		}

		knownBestCoordinates.Add(dimensions[0], 1);
		knownBestCoordinates.Add(dimensions[1], 1);

		var func = new Func<IDictionary<Dimension, double>, double>(
			(c) =>
			{
				return 
					System.Math.Pow(System.Math.Sin(3 * System.Math.PI * c[dimensions[0]]), 2) +
					(System.Math.Pow(c[dimensions[0]] - 1, 2) * (1 + System.Math.Pow(System.Math.Sin(3 * System.Math.PI * c[dimensions[1]]), 2))) +
					(System.Math.Pow(c[dimensions[1]] - 1, 2) * (1 + System.Math.Pow(System.Math.Sin(2 * System.Math.PI * c[dimensions[1]]), 2)));
			});

		return new Levi(dimensions, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}