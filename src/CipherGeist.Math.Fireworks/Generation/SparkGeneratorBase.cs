namespace CipherGeist.Math.Fireworks.Generation;

/// <summary>
/// Base class for spark generators.
/// </summary>
/// <typeparam name="TExplosion">Type of the explosion that produces sparks.</typeparam>
public abstract class SparkGeneratorBase<TExplosion> : ISparkGenerator<TExplosion> 
	where TExplosion : ExplosionBase
{
	/// <summary>
	/// Creates the sparks from the explosion.
	/// </summary>
	/// <param name="explosion">The explosion that is the source of sparks.</param>
	/// <returns>
	/// A collection of sparks for the specified explosion.
	/// </returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="explosion"/> is <c>null</c>.</exception>
	public virtual IEnumerable<Firework> CreateSparks(TExplosion explosion)
	{
		ArgumentNullException.ThrowIfNull(explosion);

		if (!explosion.SparkCounts.TryGetValue(GeneratedSparkType, out int desiredNumberOfSparks))
		{
			return Enumerable.Empty<Firework>();
		}

		var sparks = new List<Firework>(desiredNumberOfSparks);
		for (int i = 0; i < desiredNumberOfSparks; ++i)
		{
			sparks.Add(CreateSpark(explosion, i));
		}

		return sparks;
	}

	/// <summary>
	/// Creates the spark from the explosion.
	/// </summary>
	/// <param name="explosion">The explosion that is the source of sparks.</param>
	/// <returns>A spark for the specified explosion.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="explosion"/> is <c>null</c>.</exception>
	public virtual Firework CreateSpark(TExplosion explosion)
	{
		return CreateSpark(explosion, 0);
	}

	/// <summary>
	/// Creates the spark from the explosion.
	/// </summary>
	/// <param name="explosion">The explosion that is the source of sparks.</param>
	/// <param name="birthOrder">The number of spark in the collection of sparks born by this generator within one step.</param>
	/// <returns>A spark for the specified explosion.</returns>
	public abstract Firework CreateSpark(TExplosion explosion, int birthOrder);

	/// <summary>
	/// Gets the type of the generated spark.
	/// </summary>
	public abstract FireworkType GeneratedSparkType { get; }
}