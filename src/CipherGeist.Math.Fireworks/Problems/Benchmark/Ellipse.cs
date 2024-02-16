﻿namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Represents Ellipse test function, as used in 2010 paper.
/// </summary>
public sealed class Ellipse : BenchmarkProblem
{
	private const int _dimensionality = 10;
	private const double _minDimensionValue = -100.0;
	private const double _maxDimensionValue = 100.0;
	private const double _minInitialDimensionValue = 15.0;
	private const double _maxInitialDimensionValue = 30.0;
	private const double _knownBestQuality = 0.0;
	private const ProblemTarget _problemTarget = ProblemTarget.Minimum;

	/// <summary>
	/// Initializes a new instance of the <see cref="Ellipse"/> class.
	/// </summary>
	/// <param name="dimensions">Dimensions of the problem.</param>
	/// <param name="initialDimensionRanges">Initial dimension ranges, to be used to 
	/// create initial fireworks.</param>
	/// <param name="targetFunction">Quality function.</param>
	/// <param name="knownSolution">Known solution.</param>
	/// <param name="target">Problem target.</param>
	private Ellipse(
		IList<Dimension> dimensions, 
		IDictionary<Dimension, Interval> initialDimensionRanges, 
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		Solution knownSolution, 
		ProblemTarget target)
			: base(dimensions, initialDimensionRanges, targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Ellipse"/> class.
	/// </summary>
	/// <returns><see cref="Ellipse"/> instance that represents
	/// Ellipse test function, as used in 2010 paper.</returns>
	public static Ellipse Create()
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
				double sum = 0.0;
				int denominator = _dimensionality - 1;
				for (int i = 0; i < _dimensionality; i++)
				{
					double value = c[dimensions[i]];
					sum += System.Math.Pow(10.0, 4.0 * (i / denominator)) * System.Math.Pow(value, 2.0);
				}

				return sum;
			});

		return new Ellipse(dimensions, initialDimensionRanges, func, new Solution(knownBestCoordinates, _knownBestQuality), _problemTarget);
	}
}