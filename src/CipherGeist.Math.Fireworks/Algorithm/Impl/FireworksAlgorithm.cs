namespace CipherGeist.Math.Fireworks.Algorithm.Impl;

/// <summary>
/// Fireworks Algorithm implementation, per 2010 paper.
/// </summary>
public sealed class FireworksAlgorithm : StepperFireworksAlgorithmBase<FireworksAlgorithmSettings>
{
	private const double NORMAL_DISTRIBUTION_MEAN = 1.0;
	private const double NORMAL_DISTRIBUTION_STD_DEV = 1.0;

	/// <summary>
	/// Initializes a new instance of the <see cref="FireworksAlgorithm"/> class.
	/// </summary>
	/// <param name="problem">The problem to be solved by the algorithm.</param>
	/// <param name="stopCondition">The stop condition for the algorithm.</param>
	/// <param name="settings">The algorithm settings.</param>
	public FireworksAlgorithm(Problem problem, IStopCondition stopCondition, FireworksAlgorithmSettings settings)
		: base(problem, stopCondition, settings)
	{
		Randomizer = new DefaultRandom();
		BestWorstFireworkSelector = new ExtremumFireworkSelector(problem.Target);

		Distribution = new NormalDistribution(NORMAL_DISTRIBUTION_MEAN, NORMAL_DISTRIBUTION_STD_DEV);
		InitialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialDimensionRanges, Randomizer);

		ExplosionSparkGenerator = new ExplosionSparkGenerator(problem.Dimensions, Randomizer);
		SpecificSparkGenerator = new GaussianSparkGenerator(problem.Dimensions, Distribution, Randomizer);

		DistanceCalculator = new EuclideanDistance(problem.Dimensions);
		LocationSelector = new DistanceBasedFireworkSelector(DistanceCalculator, BestWorstFireworkSelector, Settings.LocationsNumber);

		ExploderSettings = new ExploderSettings
		{
			ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
			ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
			ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
			ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
			SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber
		};
		Exploder = new Exploder(BestWorstFireworkSelector, ExploderSettings);
	}

	/// <summary>
	/// Creates the initial algorithm state (before the run starts).
	/// </summary>
	/// <remarks>On each call re-creates the initial state (i.e. returns 
	/// new object each time).</remarks>
	protected override void InitializeImpl()
	{
		InitialExplosion initialExplosion = new InitialExplosion(Settings.LocationsNumber);
		ArgumentNullException.ThrowIfNull(initialExplosion, nameof(initialExplosion));

		IEnumerable<Firework> fireworks = InitialSparkGenerator.CreateSparks(initialExplosion);
		ArgumentNullException.ThrowIfNull(fireworks, nameof(fireworks));

		CalculateQualities(fireworks);
		_state = new AlgorithmState(fireworks, 0, BestWorstFireworkSelector?.SelectBest(fireworks));
	}

	/// <summary>
	/// Makes another iteration of the algorithm.
	/// </summary>
	protected override void MakeStepImpl()
	{
		int stepNumber = _state!.StepNumber + 1;
		IEnumerable<int> specificSparkParentIndices = Randomizer.NextUniqueInt32s(Settings.SpecificSparksNumber, 0, Settings.LocationsNumber);

		int currentFirework = 0;
		IEnumerable<Firework> explosionSparks = new List<Firework>();
		IEnumerable<Firework> specificSparks = new List<Firework>(Settings.SpecificSparksNumber);
		foreach (Firework firework in _state.Fireworks)
		{
			FireworkExplosion explosion = Exploder.Explode(firework, _state.Fireworks, stepNumber);

			IEnumerable<Firework> fireworkExplosionSparks = ExplosionSparkGenerator.CreateSparks(explosion);
			Debug.Assert(fireworkExplosionSparks != null, "Firework explosion sparks collection is null");

			explosionSparks = explosionSparks.Concat(fireworkExplosionSparks);
			if (specificSparkParentIndices.Contains(currentFirework))
			{
				IEnumerable<Firework> fireworkSpecificSparks = SpecificSparkGenerator.CreateSparks(explosion);
				Debug.Assert(fireworkSpecificSparks != null, "Firework specific sparks collection is null");

				specificSparks = specificSparks.Concat(fireworkSpecificSparks);
			}

			currentFirework++;
		}

		CalculateQualities(explosionSparks);
		CalculateQualities(specificSparks);

		IEnumerable<Firework> allFireworks = _state.Fireworks.Concat(explosionSparks.Concat(specificSparks));
		IEnumerable<Firework> selectedFireworks = LocationSelector.SelectFireworks(allFireworks);

		_state.Fireworks = selectedFireworks;
		_state.StepNumber = stepNumber;
		_state.BestSolution = BestWorstFireworkSelector?.SelectBest(selectedFireworks);

		RaiseStepCompleted(new AlgorithmStateEventArgs(_state));
	}

	/// <summary>
	/// Gets or sets the randomizer.
	/// </summary>
	public System.Random Randomizer { get; set; }

	/// <summary>
	/// Gets or sets the continuous univariate probability distribution.
	/// </summary>
	public IContinuousDistribution Distribution { get; set; }

	/// <summary>
	/// Gets or sets the initial spark generator.
	/// </summary>
	public ISparkGenerator<InitialExplosion> InitialSparkGenerator { get; set; }

	/// <summary>
	/// Gets or sets the explosion spark generator.
	/// </summary>
	public ISparkGenerator<FireworkExplosion> ExplosionSparkGenerator { get; set; }

	/// <summary>
	/// Gets or sets the specific spark generator.
	/// </summary>
	public ISparkGenerator<FireworkExplosion> SpecificSparkGenerator { get; set; }

	/// <summary>
	/// Gets or sets the distance calculator.
	/// </summary>
	public IDistance DistanceCalculator { get; set; }

	/// <summary>
	/// Gets or sets the location selector.
	/// </summary>
	public IFireworkSelector LocationSelector { get; set; }

	/// <summary>
	/// Gets or sets the explosion settings.
	/// </summary>
	public ExploderSettings ExploderSettings { get; set; }

	/// <summary>
	/// Gets or sets the explosion generator.
	/// </summary>
	public IExploder<FireworkExplosion> Exploder { get; set; }
}