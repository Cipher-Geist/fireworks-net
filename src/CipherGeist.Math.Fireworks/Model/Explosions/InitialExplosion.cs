namespace CipherGeist.Math.Fireworks.Model.Explosions;

/// <summary>
/// Represents an initial explosion - fake explosion that is used to generate initial fireworks.
/// </summary>
public class InitialExplosion : ExplosionBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InitialExplosion"/> class.
	/// </summary>
	/// <param name="stepNumber">The number of step this explosion took place at.</param>
	/// <param name="initialSparksNumber">The number of sparks (i.e. initial fireworks) to be generated.</param>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="initialSparksNumber"/> is less than zero.</exception>
	public InitialExplosion(int stepNumber, int initialSparksNumber)
		: base(stepNumber, new Dictionary<FireworkType, int> { { FireworkType.Initial, initialSparksNumber } })
	{
		ArgumentOutOfRangeException.ThrowIfNegative(initialSparksNumber);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="InitialExplosion"/> class.
	/// </summary>
	/// <param name="initialSparksNumber">The number of sparks (i.e. initial fireworks) to be generated.</param>
	public InitialExplosion(int initialSparksNumber)
		: this(0, initialSparksNumber)
	{
	}
}