namespace CipherGeist.Math.Fireworks.Mutation;

/// <summary>
/// Wrapper for <see cref="AttractRepulseSparkGenerator"/>, as described in 2013 GPU paper.
/// </summary>
public class AttractRepulseSparkMutator : IFireworkMutator
{
	private readonly ISparkGenerator<FireworkExplosion> _generator;

	/// <summary>
	/// Initializes a new instance of the <see cref="AttractRepulseSparkMutator"/> class.
	/// </summary>
	/// <param name="generator">Attract-Repulse generator to generate a spark.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="generator"/> is <c>null</c>.</exception>
	public AttractRepulseSparkMutator(ISparkGenerator<FireworkExplosion> generator)
	{
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
	}

	/// <summary>
	/// Changes the <paramref name="firework"/>.
	/// </summary>
	/// <param name="firework">The <see cref="MutableFirework"/> to be changed.</param>
	/// <param name="explosion">The <see cref="FireworkExplosion"/> that contains explosion characteristics.</param>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="firework"/> 
	/// or <paramref name="explosion"/> is <c>null</c>.</exception>
	public void MutateFirework(ref MutableFirework firework, FireworkExplosion explosion)
	{
		ArgumentNullException.ThrowIfNull(firework);
		ArgumentNullException.ThrowIfNull(explosion);

		var newState = _generator.CreateSpark(explosion);
		firework.Update(newState);
	}
}