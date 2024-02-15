namespace CipherGeist.Math.Fireworks.Explode;

/// <summary>
/// Exploder base class. 
/// </summary>
public abstract class ExploderBase
{
	/// <summary>
	/// Minimum allowed explosion sparks number (not rounded).
	/// </summary>
	protected readonly double _minAllowedExplosionSparksNumberExact;

	/// <summary>
	/// Maximum allowed explosion sparks number (not rounded).
	/// </summary>
	protected readonly double _maxAllowedExplosionSparksNumberExact;

	/// <summary>
	/// Minimum allowed explosion sparks number (rounded).
	/// </summary>
	protected readonly int _minAllowedExplosionSparksNumber;

	/// <summary>
	/// Maximum allowed explosion sparks number (rounded).
	/// </summary>
	protected readonly int _maxAllowedExplosionSparksNumber;

	/// <summary>
	/// The extremum fireworks selector being used.
	/// </summary>
	protected readonly IExtremumFireworkSelector _extremumFireworkSelector;

	/// <summary>
	/// Initializes a new instance of the <see cref="Exploder"/> class.
	/// </summary>
	/// <param name="extremumFireworkSelector">The extremum firework selector.</param>
	/// <param name="settings">The exploder settings.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="settings"/> 
	/// or <paramref name="extremumFireworkSelector"/> is <c>null</c>.</exception>
	public ExploderBase(IExtremumFireworkSelector extremumFireworkSelector, ExploderSettings settings)
	{
		_extremumFireworkSelector = extremumFireworkSelector ?? throw new ArgumentNullException(nameof(extremumFireworkSelector));
		Settings = settings ?? throw new ArgumentNullException(nameof(settings));

		_minAllowedExplosionSparksNumberExact = settings.ExplosionSparksNumberLowerBound * settings.ExplosionSparksNumberModifier;
		_maxAllowedExplosionSparksNumberExact = settings.ExplosionSparksNumberUpperBound * settings.ExplosionSparksNumberModifier;

		_minAllowedExplosionSparksNumber = (int)System.Math.Round(_minAllowedExplosionSparksNumberExact, MidpointRounding.AwayFromZero);
		_maxAllowedExplosionSparksNumber = (int)System.Math.Round(_maxAllowedExplosionSparksNumberExact, MidpointRounding.AwayFromZero);
	}

	/// <summary>
	/// Calculates the explosion amplitude.
	/// </summary>
	/// <param name="focus">The explosion focus.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <returns>The explosion amplitude.</returns>
	protected virtual double CalculateAmplitude(Firework focus, IEnumerable<Firework> currentFireworks)
	{
		var currentFireworkQualities = currentFireworks.Select(fw => fw.Quality);
		var bestFireworkQuality = _extremumFireworkSelector.SelectBest(currentFireworks).Quality;

		return Settings.ExplosionSparksMaximumAmplitude * (focus.Quality - bestFireworkQuality + double.Epsilon) /
			(currentFireworkQualities.Sum(fw => fw - bestFireworkQuality) + double.Epsilon);
	}

	/// <summary>
	/// Defines the count of the explosion sparks.
	/// </summary>
	/// <param name="focus">The explosion focus.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <param name="currentFireworkQualities">The current firework qualities.</param>
	/// <returns>The number of explosion sparks created by that explosion.</returns>
	protected virtual int CountExplosionSparks(
		Firework focus,
		IEnumerable<Firework> currentFireworks,
		IEnumerable<double> currentFireworkQualities)
	{
		var explosionSparksNumberExact = CountExplosionSparksExact(focus, currentFireworks, currentFireworkQualities);
		if (explosionSparksNumberExact.IsLess(_minAllowedExplosionSparksNumberExact))
		{
			return _minAllowedExplosionSparksNumber;
		}

		if (explosionSparksNumberExact.IsGreater(_maxAllowedExplosionSparksNumberExact))
		{
			return _maxAllowedExplosionSparksNumber;
		}

		return (int)System.Math.Round(explosionSparksNumberExact, MidpointRounding.AwayFromZero);
	}

	/// <summary>
	/// Defines the count of the specific sparks.
	/// </summary>
	/// <param name="focus">The explosion focus.</param>
	/// <param name="currentFireworkQualities">The current firework qualities.</param>
	/// <returns>The number of specific sparks created by that explosion.</returns>
	protected virtual int CountSpecificSparks(Firework focus, IEnumerable<double> currentFireworkQualities)
	{
		Debug.Assert(Settings != null, "Settings is null");
		return Settings.SpecificSparksPerExplosionNumber;
	}

	/// <summary>
	/// Defines the exact (not rounded) count of the explosion sparks.
	/// </summary>
	/// <param name="focus">The explosion focus.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <param name="currentFireworkQualities">The current firework qualities.</param>
	/// <returns>The exact (not rounded) number of explosion sparks created by that explosion.</returns>
	private double CountExplosionSparksExact(
		Firework focus,
		IEnumerable<Firework> currentFireworks,
		IEnumerable<double> currentFireworkQualities)
	{
		var worstFireworkQuality = _extremumFireworkSelector.SelectWorst(currentFireworks).Quality;
		return Settings.ExplosionSparksNumberModifier * (worstFireworkQuality - focus.Quality + double.Epsilon) /
			(currentFireworkQualities.Sum(fq => worstFireworkQuality - fq) + double.Epsilon);
	}

	/// <summary>
	/// Gets the settings being used by the exploder.
	/// </summary>
	public ExploderSettings Settings { get; }
}
