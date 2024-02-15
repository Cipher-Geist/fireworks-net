﻿namespace CipherGeist.Math.Fireworks.Problems.Benchmark;

/// <summary>
/// Describes a benchmark problem, which has known solution and
/// can be used to test the algorithm implementation.
/// </summary>
public class BenchmarkProblem : Problem
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BenchmarkProblem"/> class.
	/// </summary>
	/// <param name="dimensions">The dimensions of the problem.</param>
	/// <param name="initialDimensionRanges">The initial ranges of the problem dimensions.</param>
	/// <param name="targetFunction">The quality function that needs to be optimized.</param>
	/// <param name="knownSolution">The known solution.</param>
	/// <param name="target">Target of the problem.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="knownSolution"/>
	/// is <c>null</c>.</exception>
	/// <exception cref="System.ArgumentException"> if <paramref name="knownSolution"/>
	/// has neither coordinates nor quality.</exception>
	public BenchmarkProblem(
		IList<Dimension> dimensions, 
		IDictionary<Dimension, Interval> initialDimensionRanges,
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		Solution knownSolution, 
		ProblemTarget target)
			: base(dimensions, initialDimensionRanges, targetFunction, target)
	{
		ArgumentNullException.ThrowIfNull(knownSolution);

		if (knownSolution.Coordinates == null && double.IsNaN(knownSolution.Quality))
		{
			// We have neither coordinates of a known solution nor its quality.
			// Therefore, such known solution is useless.
			throw new ArgumentException(string.Empty, nameof(knownSolution));
		}

		KnownSolution = knownSolution;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BenchmarkProblem"/> class.
	/// Initial ranges of the problem dimensions match those from <paramref name="dimensions"/>.
	/// </summary>
	/// <param name="dimensions">The dimensions of the problem.</param>
	/// <param name="targetFunction">The quality function that needs to be optimized.</param>
	/// <param name="knownSolution">The known solution.</param>
	/// <param name="target">Target of the problem.</param>
	public BenchmarkProblem(
		IList<Dimension> dimensions,
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		Solution knownSolution, 
		ProblemTarget target)
			: this(dimensions, CreateDefaultInitialRanges(dimensions), targetFunction, knownSolution, target)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BenchmarkProblem"/> class.
	/// Initial ranges of the problem dimensions match those from <paramref name="dimensions"/>.
	/// <paramref name="targetFunction"/> is to be minimized.
	/// </summary>
	/// <param name="dimensions">The dimensions of the problem.</param>
	/// <param name="targetFunction">The quality function that needs to be optimized.</param>
	/// <param name="knownSolution">The known solution.</param>
	public BenchmarkProblem(
		IList<Dimension> dimensions, 
		Func<IDictionary<Dimension, double>, double> targetFunction, 
		Solution knownSolution)
			: this(dimensions, CreateDefaultInitialRanges(dimensions), targetFunction, knownSolution, ProblemTarget.Minimum)
	{
	}

	/// <summary>
	/// Gets the known problem <see cref="Solution"/>.
	/// </summary>
	public Solution KnownSolution { get; private set; }
}