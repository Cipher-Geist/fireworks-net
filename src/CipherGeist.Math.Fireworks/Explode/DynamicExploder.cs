namespace CipherGeist.Math.Fireworks.Explode;

/// <summary>
/// Implementation as outlined in 
/// Zheng, S.; Tan, Y. Dynamic search in fireworks algorithm. In Proceedings of the 2014 IEEE Congress on
/// Evolutionary Computation, Beijing, China, 6–11 July 2014; pp. 3222–3229.
/// https://www.cil.pku.edu.cn/docs/20190509160339679223.pdf
/// </summary>
public class DynamicExploder : ExploderBase, IExploder<FireworkExplosion>
{
	private readonly ProblemTarget _problemTarget;

	/// <summary>
	/// Initializes a new instance of the <see cref="Exploder"/> class.
	/// </summary>
	/// <param name="extremumFireworkSelector">The extremum firework selector.</param>
	/// <param name="problemTarget">The problem target (maximum or minimum).</param>
	/// <param name="settings">The exploder settings.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="settings"/> 
	/// or <paramref name="extremumFireworkSelector"/> is <c>null</c>.</exception>
	public DynamicExploder(
		IExtremumFireworkSelector extremumFireworkSelector,
		ProblemTarget problemTarget,
		ExploderSettings settings)
			: base(extremumFireworkSelector, settings)
	{
		_problemTarget = problemTarget;
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

		double amplitude = CalculateAmplitude(focus, currentFireworks);

		var currentFireworkQualities = currentFireworks.Select(fw => fw.Quality);
		var sparkCounts = new Dictionary<FireworkType, int>
		{
			{ FireworkType.ExplosionSpark, CountExplosionSparks(focus, currentFireworks, currentFireworkQualities) },
			{ FireworkType.SpecificSpark, CountSpecificSparks(focus, currentFireworkQualities) }
		};

		return new FireworkExplosion(focus, currentStepNumber, amplitude, sparkCounts);
	}

	/// <summary>
	/// Initialize the core firework (CF) according to the dynFWA algorithm.
	/// </summary>
	/// <param name="currentFireworks">The current list of all fireworks.</param>
	public void InitializeCoreFirework(IEnumerable<Firework> currentFireworks)
	{
		CoreFirework ??= _extremumFireworkSelector.SelectBest(currentFireworks);
		CoreFireworkAmplitutePreviousGeneration = Settings.ExplosionSparksMaximumAmplitude;
	}

	/// <summary>
	/// Set the core firework (CF) according to the dynFWA algorithm.
	/// </summary>
	/// <param name="explosionSparks">The current list of all fireworks.</param>
	public void UpdateCoreFirework(IEnumerable<Firework> explosionSparks)
	{
		ArgumentNullException.ThrowIfNull(CoreFirework, nameof(CoreFirework));

		var candidateBest = _extremumFireworkSelector.SelectBest(explosionSparks);
		var amplificationFactor = Settings.ReductionCoefficent;

		if (IsCandidateBestQualityFirework(CoreFirework, candidateBest, _problemTarget))
		{
			amplificationFactor = Settings.AmplificationCoefficent; // * 5;
			CoreFirework = candidateBest;
		}

		var amplitude = CalculateAmplitude(CoreFirework, explosionSparks);
		CoreFireworkAmplitutePreviousGeneration = amplitude * amplificationFactor;
	}

	/// <summary>
	/// Calculates the explosion amplitude.
	/// </summary>
	/// <param name="focus">The explosion focus.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <returns>The explosion amplitude.</returns>
	protected override double CalculateAmplitude(Firework focus, IEnumerable<Firework> currentFireworks)
	{
		double amplitude = focus == CoreFirework
			? CoreFireworkAmplitutePreviousGeneration
			: base.CalculateAmplitude(focus, currentFireworks);

		amplitude += amplitude < double.Epsilon ? double.Epsilon : 0.0;
		return amplitude;
	}

	private bool IsCandidateBestQualityFirework(Firework coreFirework, Firework candidate, ProblemTarget problemTarget)
	{
		return problemTarget == ProblemTarget.Minimum
			? candidate.Quality.IsLess(coreFirework.Quality)
			: candidate.Quality.IsGreater(coreFirework.Quality);
	}

	/// <summary>
	/// Gets the Core Firework (CF).
	/// </summary>
	public Firework? CoreFirework { get; private set; }

	/// <summary>
	/// Gets the Core Firework amplitude from the previous generation.
	/// </summary>
	public double CoreFireworkAmplitutePreviousGeneration { get; private set; }
}