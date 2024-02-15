namespace CipherGeist.Math.Fireworks.Model.Explosions;

/// <summary>
/// Represents an explosion, which gave rise to a fireworks collection.
/// </summary>
public class EliteExplosion : ExplosionBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EliteExplosion"/> class.
	/// </summary>
	/// <param name="stepNumber">The number of step this explosion took place at.</param>
	/// <param name="fireworksNumber">The number of fireworks.</param>
	/// <param name="fireworks">The collection that stores numbers of fireworks.</param>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="fireworksNumber"/> is less than zero.</exception>
	/// <exception cref="ArgumentNullException"> if <paramref name="fireworks"/> is <c>null</c>.</exception>
	public EliteExplosion(int stepNumber, int fireworksNumber, IEnumerable<Firework> fireworks)
		: base(stepNumber, new Dictionary<FireworkType, int> { { FireworkType.ExplosionSpark, fireworksNumber } })
	{
		ArgumentOutOfRangeException.ThrowIfNegative(fireworksNumber);
		Fireworks = fireworks ?? throw new ArgumentNullException(nameof(fireworks));
	}

	/// <summary>
	/// Gets or sets the fireworks collection.
	/// </summary>
	public IEnumerable<Firework> Fireworks { get; set; }
}
