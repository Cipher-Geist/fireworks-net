namespace CipherGeist.Math.Fireworks.Explode;

/// <summary>
/// Explosion generator, per 2010 paper.
/// </summary>
public class Exploder : ExploderBase, IExploder<FireworkExplosion>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Exploder"/> class.
	/// </summary>
	/// <param name="extremumFireworkSelector">The extremum firework selector.</param>
	/// <param name="settings">The exploder settings.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="settings"/> 
	/// or <paramref name="extremumFireworkSelector"/> is <c>null</c>.</exception>
	public Exploder(IExtremumFireworkSelector extremumFireworkSelector, ExploderSettings settings)
		: base(extremumFireworkSelector, settings)
	{
	}

	/// <summary>
	/// Creates an explosion.
	/// </summary>
	/// <param name="focus">The explosion focus (center).</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <param name="currentStepNumber">The current step number.</param>
	/// <returns>New explosion.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="focus"/> or <paramref name="currentFireworks"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="currentStepNumber"/> is less than zero or less than birth step number of the <paramref name="focus"/>.</exception>
	public virtual FireworkExplosion Explode(Firework focus, IEnumerable<Firework> currentFireworks, int currentStepNumber)
	{
		ArgumentNullException.ThrowIfNull(focus);
		ArgumentNullException.ThrowIfNull(currentFireworks);
		ArgumentOutOfRangeException.ThrowIfNegative(currentStepNumber);

		// Not '<=' here because that would limit possible algorithm implementations.
		ArgumentOutOfRangeException.ThrowIfLessThan(currentStepNumber, focus.BirthStepNumber);

		var amplitude = CalculateAmplitude(focus, currentFireworks);
		var currentFireworkQualities = currentFireworks.Select(fw => fw.Quality);

		var sparkCounts = new Dictionary<FireworkType, int>
		{
			{ FireworkType.ExplosionSpark, CountExplosionSparks(focus, currentFireworks, currentFireworkQualities) },
			{ FireworkType.SpecificSpark, CountSpecificSparks(focus, currentFireworkQualities) }
		};

		return new FireworkExplosion(focus, currentStepNumber, amplitude, sparkCounts);
	}
}