namespace CipherGeist.Math.Fireworks.Generation;

/// <summary>
/// Conventional Explosion spark generator, as described in 2010 paper.
/// </summary>
public class ExplosionSparkGenerator : SparkGeneratorBase<FireworkExplosion>
{
	private readonly IEnumerable<Dimension> _dimensions;
	private readonly System.Random _randomizer;

	private const double OFFSET_DISPLACEMENT_RANDOM_MIN = -1.0;
	private const double OFFSET_DISPLACEMENT_RANDOM_MAX = 1.0;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExplosionSparkGenerator"/> class.
	/// </summary>
	/// <param name="dimensions">The dimensions to fit generated sparks into.</param>
	/// <param name="randomizer">The randomizer.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="dimensions"/> or <paramref name="randomizer"/> is <c>null</c>.
	/// </exception>
	public ExplosionSparkGenerator(IEnumerable<Dimension> dimensions, System.Random randomizer)
	{
		_dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
		_randomizer = randomizer ?? throw new ArgumentNullException(nameof(randomizer));
	}

	/// <summary>
	/// Creates the spark from the explosion.
	/// </summary>
	/// <param name="explosion">The explosion that gives birth to the spark.</param>
	/// <param name="birthOrder">The number of spark in the collection of sparks born by this generator within one step.</param>
	/// <returns>A spark for the specified explosion.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="explosion"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="birthOrder"/> is less than zero.</exception>
	public override Firework CreateSpark(FireworkExplosion explosion, int birthOrder)
	{
		ArgumentNullException.ThrowIfNull(explosion);
		ArgumentOutOfRangeException.ThrowIfNegative(birthOrder);

		var spark = new Firework(GeneratedSparkType, explosion.StepNumber, birthOrder, explosion.ParentFirework);
		foreach (var dimension in _dimensions)
		{
			// Coin flip.
			if (_randomizer.NextBoolean())
			{
				double offsetDisplacement = explosion.Amplitude *
					_randomizer.NextDouble(
						OFFSET_DISPLACEMENT_RANDOM_MIN, 
						OFFSET_DISPLACEMENT_RANDOM_MAX);

				spark.Coordinates[dimension] += offsetDisplacement;

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
		get { return FireworkType.ExplosionSpark; } 
	}
}