namespace CipherGeist.Math.Fireworks.Generation.Dynamic;

/// <summary>
/// Implementation as outlined in 
/// Zheng, S.; Tan, Y. Dynamic search in fireworks algorithm. In Proceedings of the 2014 IEEE Congress on
/// Evolutionary Computation, Beijing, China, 6–11 July 2014; pp. 3222–3229.
/// https://www.cil.pku.edu.cn/docs/20190509160339679223.pdf
/// </summary>
public class DynamicExplosionSparkGenerator : SparkGeneratorBase<FireworkExplosion>
{
	private readonly IEnumerable<Dimension> _dimensions;
	private readonly IRandomizer _randomizer;

	private const double OFFSET_DISPLACEMENT_RANDOM_MIN = -1.0;
	private const double OFFSET_DISPLACEMENT_RANDOM_MAX = 1.0;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExplosionSparkGenerator2012"/> class.
	/// </summary>
	/// <param name="dimensions">The dimensions to fit generated sparks into.</param>
	/// <param name="randomizer">The randomizer.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="dimensions"/> 
	/// or <paramref name="randomizer"/> is <c>null</c>.</exception>
	public DynamicExplosionSparkGenerator(IEnumerable<Dimension> dimensions, IRandomizer randomizer)
	{
		_dimensions = dimensions;
		_randomizer = randomizer;
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
		ArgumentOutOfRangeException.ThrowIfNegative(birthOrder);

		var spark = new Firework(GeneratedSparkType, explosion.StepNumber, birthOrder, explosion.ParentFirework);
		ArgumentNullException.ThrowIfNull(spark.Coordinates, nameof(spark.Coordinates));

		// Mapping rule.
		foreach (var dimension in _dimensions)
		{
			// Coin flip.
			if (_randomizer.NextBoolean())
			{
				var offsetDisplacement = explosion.Amplitude * 
					_randomizer.NextDouble(
						OFFSET_DISPLACEMENT_RANDOM_MIN, 
						OFFSET_DISPLACEMENT_RANDOM_MAX);

				spark.Coordinates[dimension] += offsetDisplacement;

				if (!dimension.IsValueInRange(spark.Coordinates[dimension]))
				{
					spark.Coordinates[dimension] = dimension.Range.Minimum + (_randomizer.NextDouble() * dimension.Range.Length);
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