namespace CipherGeist.Math.Fireworks.Model.Explosions;

/// <summary>
/// Represents an explosion produced by the firework.
/// </summary>
public class FireworkExplosion : ExplosionBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FireworkExplosion"/> class.
	/// </summary>
	/// <param name="parentFirework">The firework that has exploded (focus of an explosion).</param>
	/// <param name="stepNumber">The number of step this explosion took place at.</param>
	/// <param name="amplitude">The amplitude of an explosion.</param>
	/// <param name="sparkCounts">The collection that stores numbers of sparks generated during the explosion, per spark (firework) type.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="parentFirework"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="amplitude"/> is <see cref="double.NaN"/> or infinity.</exception>
	public FireworkExplosion(Firework parentFirework, int stepNumber, double amplitude, IDictionary<FireworkType, int> sparkCounts)
		: base(stepNumber, sparkCounts)
	{
		if (double.IsNaN(amplitude) || double.IsInfinity(amplitude))
		{
			throw new ArgumentOutOfRangeException(nameof(amplitude));
		}

		ParentFirework = parentFirework ?? throw new ArgumentNullException(nameof(parentFirework));
		Amplitude = amplitude;
	}

	/// <summary>
	/// Gets the firework that has exploded (center of an explosion).
	/// </summary>
	public Firework ParentFirework { get; private set; }

	/// <summary>
	/// Gets the amplitude of an explosion.
	/// </summary>
	public double Amplitude { get; private set; }
}