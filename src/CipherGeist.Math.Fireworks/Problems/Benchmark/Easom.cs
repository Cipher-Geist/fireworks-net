namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Easom test function..
/// </summary>
/// <remarks>http://en.wikipedia.org/wiki/Test_functions_for_optimization</remarks>
public class Easom : BenchmarkProblem
{
	private const int _dimensionality = 2;
	private const double _minDimensionValue = -100.0;
	private const double _maxDimensionValue = 100.0;
	private const double _knownBestQuality = 0.0;
	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Easom"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Easom(
		IList<Dimension> dimensions, 
		Func<IDictionary<Dimension, double>, double> targetFunction,
		Solution knownSolution, 
		ProblemTarget target)
			: base(dimensions, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Easom"/> class.
	/// </summary>
	/// <returns><see cref="Easom"/> instance that represents Beale test function.</returns>
	public static Easom Create()
	{
		var dimensions = new Dimension[_dimensionality];
		var knownBestCoordinates = new Dictionary<Dimension, double>(_dimensionality);

		for (int i = 0; i < _dimensionality; i++)
		{
			dimensions[i] = new Dimension(new Interval(_minDimensionValue, _maxDimensionValue));
		}

		knownBestCoordinates.Add(dimensions[0], System.Math.PI);
		knownBestCoordinates.Add(dimensions[1], System.Math.PI);

		var func = new Func<IDictionary<Dimension, double>, double>(
			(c) =>
			{
				return -System.Math.Cos(c[dimensions[0]]) * System.Math.Cos(c[dimensions[1]]) * 
					System.Math.Exp(-System.Math.Pow(c[dimensions[0]] - System.Math.PI, 2) - 
						System.Math.Pow(c[dimensions[1]] - System.Math.PI, 2));
			});

		return new Easom(dimensions, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}