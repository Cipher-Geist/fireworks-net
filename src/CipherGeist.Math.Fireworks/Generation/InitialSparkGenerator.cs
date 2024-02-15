namespace CipherGeist.Math.Fireworks.Generation;

/// <summary>
/// Conventional initial spark generator, as described in 2010 paper.
/// </summary>
public class InitialSparkGenerator : SparkGeneratorBase<InitialExplosion>
{
	private readonly IEnumerable<Dimension> _dimensions;
	private readonly IDictionary<Dimension, Interval> _initialRanges;
	private readonly System.Random _randomizer;

	/// <summary>
	/// Initializes a new instance of the <see cref="InitialSparkGenerator"/> class.
	/// </summary>
	/// <param name="dimensions">The dimensions to fit generated sparks into.</param>
	/// <param name="initialRanges">The initial ranges for <paramref name="dimensions"/>.</param>
	/// <param name="randomizer">The randomizer.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="dimensions"/>
	/// or <paramref name="initialRanges"/> or <paramref name="randomizer"/> is <c>null</c>.
	/// </exception>
	public InitialSparkGenerator(
		IEnumerable<Dimension> dimensions, 
		IDictionary<Dimension, Interval> initialRanges, 
		System.Random randomizer)
	{
		_dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
		_initialRanges = initialRanges ?? throw new ArgumentNullException(nameof(initialRanges));
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
	public override Firework CreateSpark(InitialExplosion explosion, int birthOrder)
	{
		ArgumentNullException.ThrowIfNull(explosion);
		ArgumentOutOfRangeException.ThrowIfNegative(birthOrder);

		var spark = new Firework(GeneratedSparkType, 0, birthOrder);

		foreach (Dimension dimension in _dimensions)
		{
			var dimensionRange = _initialRanges[dimension];
			spark.Coordinates[dimension] = _randomizer.NextDouble(dimensionRange);
		}

		return spark;
	}

	/// <summary>
	/// Gets the type of the generated spark.
	/// </summary>
	public override FireworkType GeneratedSparkType 
	{ 
		get { return FireworkType.Initial; } 
	}
}