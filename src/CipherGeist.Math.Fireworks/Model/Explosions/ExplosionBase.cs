namespace CipherGeist.Math.Fireworks.Model.Explosions;

/// <summary>
/// Base class used to represent a firework explosion.
/// </summary>
public abstract class ExplosionBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ExplosionBase"/> class.
	/// </summary>
	/// <param name="stepNumber">The number of step this explosion took place at.</param>
	/// <param name="sparkCounts">The collection that stores numbers of sparks generated 
	/// during the explosion, per spark (firework) type.</param>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="stepNumber"/> is less than zero.</exception>
	/// <exception cref="ArgumentNullException"> if <paramref name="sparkCounts"/> is <c>null</c>.</exception>
	protected ExplosionBase(int stepNumber, IDictionary<FireworkType, int> sparkCounts)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(stepNumber);

		Id = new TId();
		StepNumber = stepNumber;
		SparkCounts = sparkCounts ?? throw new ArgumentNullException(nameof(sparkCounts));
	}

	/// <summary>
	/// Gets the unique TId of the explosion.
	/// </summary>
	public TId Id { get; private set; }

	/// <summary>
	/// Gets the number of step this explosion took place at.
	/// </summary>
	public int StepNumber { get; private set; }

	/// <summary>
	/// Gets the collection that stores numbers of sparks generated during the explosion, per spark (firework) type.
	/// </summary>
	public IDictionary<FireworkType, int> SparkCounts { get; private set; }
}