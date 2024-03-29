﻿namespace CipherGeist.Math.Fireworks.Generation;

/// <summary>
/// Implementation of Attract-Repulse mutation algorithm, as described in 2013 GPU paper.
/// </summary>
public class AttractRepulseSparkGenerator : SparkGeneratorBase<FireworkExplosion>
{
	/// <summary>
	/// The current best solution in global scope.
	/// </summary>
	private readonly Solution _bestSolution;
	private readonly IEnumerable<Dimension> _dimensions;
	private readonly IContinuousDistribution _distribution;
	private readonly IRandomizer _randomizer;

	/// <summary>
	/// Initializes a new instance of the <see cref="AttractRepulseSparkGenerator"/> class.
	/// </summary>
	/// <param name="bestSolution">The current best solution in global scope.</param>
	/// <param name="dimensions">The dimensions to fit generated sparks into.</param>
	/// <param name="distribution">The distribution to be used to obtain scaling factor.</param>
	/// <param name="randomizer">The randomizer.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="bestSolution"/>
	/// or <paramref name="dimensions"/> or <paramref name="distribution"/> or <paramref name="randomizer"/> is <c>null</c>.
	/// </exception>
	public AttractRepulseSparkGenerator(
		Solution bestSolution,
		IEnumerable<Dimension> dimensions,
		IContinuousDistribution distribution,
		IRandomizer randomizer)
	{
		_bestSolution = bestSolution ?? throw new ArgumentNullException(nameof(bestSolution));
		_dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
		_distribution = distribution ?? throw new ArgumentNullException(nameof(distribution));
		_randomizer = randomizer ?? throw new ArgumentNullException(nameof(randomizer));
	}

	/// <summary>
	/// Creates the spark from the explosion.
	/// </summary>
	/// <param name="explosion">The explosion that gives birth to the spark.</param>
	/// <param name="birthOrder">The number of spark in the collection of sparks born by
	/// this generator within one step.</param>
	/// <returns>A spark for the specified explosion.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="explosion"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="birthOrder"/> is less than zero.</exception>
	public override Firework CreateSpark(FireworkExplosion explosion, int birthOrder)
	{
		ArgumentNullException.ThrowIfNull(explosion, nameof(explosion));
		ArgumentOutOfRangeException.ThrowIfNegative(birthOrder);

		var spark = new Firework(GeneratedSparkType, explosion.StepNumber, birthOrder, explosion.ParentFirework);

		// Attract-Repulse scaling factor. (1-δ, 1+δ)
		var scalingFactor = _distribution.Sample();

		Solution? copyOfBestSolution = null;
		lock (_bestSolution)
		{
			copyOfBestSolution = new Solution(_bestSolution.Coordinates, _bestSolution.Quality);
		}

		foreach (var dimension in _dimensions)
		{
			// Coin flip.
			if (_randomizer.NextBoolean())
			{
				spark.Coordinates[dimension] += (spark.Coordinates[dimension] - copyOfBestSolution.Coordinates[dimension]) * scalingFactor;
				if (!dimension.IsValueInRange(spark.Coordinates[dimension]))
				{
					spark.Coordinates[dimension] = dimension.Range.Minimum +
						(System.Math.Abs(spark.Coordinates[dimension]) % dimension.Range.Length);
				}
			}
		}

		return spark;
	}

	/// <summary>
	/// Gets the type of the generated spark.
	/// </summary>
	public override FireworkType GeneratedSparkType 
	{ 
		get { return FireworkType.SpecificSpark; } 
	}
}