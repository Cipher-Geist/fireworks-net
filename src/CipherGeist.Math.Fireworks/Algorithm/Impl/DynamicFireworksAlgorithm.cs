namespace CipherGeist.Math.Fireworks.Algorithm.Impl;

/// <summary>
/// Implementation of 
/// S.Q. Zheng, Andreas Janecek, J.Z. Li, and Y. Tan, "Dynamic Search in Fireworks Algorithm", 2014 
/// IEEE World Conference on Computational Intelligence (IEEE WCCI'2014) - IEEE Congress on Evolutionary 
/// Computation (CEC'2014), July 07-11, 2014, Beijing International Convention Center (BICC), 
/// Beijing, China, pp. 3222-3229.
/// </summary>
public sealed class DynamicFireworksAlgorithm : StepperFireworksAlgorithmBase<DynamicFireworksAlgorithmSettings>
{
	private const double NORMAL_DISTRIBUTION_MEAN = 1.0;
	private const double NORMAL_DISTRIBUTION_STD_DEV = 1.0;

	/// <summary>
	/// Initializes a new instance of the <see cref="EnhancedFireworksAlgorithm"/> class.
	/// </summary>
	/// <param name="problem">The problem to be solved by the algorithm.</param>
	/// <param name="stopCondition">The stop condition for the algorithm.</param>
	/// <param name="settings">The algorithm settings.</param>
	/// <param name="logger">The logger.</param>
	public DynamicFireworksAlgorithm(
		Problem problem, 
		IStopCondition stopCondition, 
		DynamicFireworksAlgorithmSettings settings, 
		ILogger<DynamicFireworksAlgorithm> logger)
			: base(problem, stopCondition, settings, logger)
	{
		Randomizer = new DefaultRandom();

		InitialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialDimensionRanges, Randomizer);
		ExplosionSparkGenerator = new ExplosionSparkGenerator2012(problem.Dimensions, Randomizer);

		var distribution = new NormalDistribution(NORMAL_DISTRIBUTION_MEAN, NORMAL_DISTRIBUTION_STD_DEV);
		GaussianSparkGenerator = new GaussianSparkGenerator2012(problem.Dimensions, distribution);

		var bestSelector = new Func<IEnumerable<Firework>, Firework>(BestWorstFireworkSelector.SelectBest);
		BestAndRandomFireworkSelector = new BestAndRandomFireworkSelector(Randomizer, bestSelector);

		ExploderSettings = new ExploderSettings
		{
			ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
			ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
			ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
			ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
			SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber,
			AmplificationCoefficent = settings.AmplificationCoefficent,
			ReductionCoefficent = settings.ReductionCoefficent
		};

		Exploder = new Exploder(BestWorstFireworkSelector, ExploderSettings);
		DynamicExploder = new DynamicExploder(BestWorstFireworkSelector, problem.Target, ExploderSettings);

		Differentiator = new Differentiator();
		PolynomialFit = new PolynomialFit(Settings.FunctionOrder);

		FunctionSolver = new Solver();
		EliteStrategyGenerator = new LS2EliteSparkGenerator(problem.Dimensions, PolynomialFit, Differentiator, FunctionSolver);
	}

	#region Internal Steps.
	/// <summary>
	/// Creates the initial algorithm state (before the run starts).
	/// </summary>
	/// <remarks>On each call re-creates the initial state (i.e. returns new object each time).</remarks>
	protected override void InitializeImpl()
	{
		var initialExplosion = new InitialExplosion(Settings.LocationsNumber);
		ArgumentNullException.ThrowIfNull(initialExplosion, nameof(initialExplosion));

		var fireworks = InitialSparkGenerator.CreateSparks(initialExplosion);
		ArgumentNullException.ThrowIfNull(fireworks, nameof(fireworks));

		CalculateQualities(fireworks);
		_state = new AlgorithmState(fireworks, 0, BestWorstFireworkSelector.SelectBest(fireworks));

		DynamicExploder.InitializeCoreFirework(_state.Fireworks);
	}

	/// <summary>
	/// Represents one iteration of the algorithm.
	/// </summary>
	protected override void MakeStepImpl()
	{
		int currentFirework = 0;
		var specificSparkParentIndices = Randomizer.NextUniqueInt32s(Settings.SpecificSparksNumber, 0, Settings.LocationsNumber);

		var explosionSparks = new List<Firework>();
		var specificSparks = new List<Firework>(Settings.SpecificSparksNumber);

		int stepNumber = _state!.StepNumber + 1;

		foreach (var firework in _state.Fireworks)
		{
			var dynamicExplosion = DynamicExploder.Explode(firework, _state.Fireworks, stepNumber);
			var fireworkExplosionSparks = ExplosionSparkGenerator.CreateSparks(dynamicExplosion);

			explosionSparks = explosionSparks.Concat(fireworkExplosionSparks).ToList();
			if (specificSparkParentIndices.Contains(currentFirework))
			{
				var fireworkSpecificSparks = GaussianSparkGenerator.CreateSparks(dynamicExplosion);
				specificSparks = specificSparks.Concat(fireworkSpecificSparks).ToList();
			}

			currentFirework++;
		}

		CalculateQualities(specificSparks);
		CalculateQualities(explosionSparks);

		DynamicExploder.UpdateCoreFirework(explosionSparks);

		var allFireworks = _state.Fireworks.Concat(explosionSparks.Concat(specificSparks));
		var selectedFireworks = BestAndRandomFireworkSelector.SelectFireworks(allFireworks, Settings.SamplingNumber).ToList();

		_state.Fireworks = selectedFireworks;
		_state.StepNumber = stepNumber;
		_state.BestSolution = BestWorstFireworkSelector.SelectBest(selectedFireworks);

		RaiseStepCompleted(new AlgorithmStateEventArgs(ProblemToSolve, _state));
	}

	/// <summary>
	/// Compares two <see cref="Firework"/>s and determines if it is necessary to replace the worst one 
	/// with the elite one according to the elite strategy.
	/// </summary>
	/// <param name="worst">The worst firework on current step.</param>
	/// <param name="elite">The elite firework on current step calculated by elite strategy</param>
	/// <returns><c>true</c> if necessary replace <paramref name="worst"/> with <paramref name="elite"/>.</returns>
	public bool ShouldReplaceWorstWithElite(Firework worst, Firework elite)
	{
		return ProblemToSolve.Target == ProblemTarget.Minimum
			? worst.Quality.IsGreater(elite.Quality)
			: worst.Quality.IsLess(elite.Quality);
	}
	#endregion // Internal Steps.

	#region Properties.
	/// <summary>
	/// Gets or sets the randomizer.
	/// </summary>
	public System.Random Randomizer { get; set; }

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
	public ISparkGenerator<FireworkExplosion> GaussianSparkGenerator { get; set; }

	/// <summary>
	/// Gets or sets the location selector.
	/// </summary>
	public IFireworkSelector BestAndRandomFireworkSelector { get; set; }

	/// <summary>
	/// Gets or sets the explosion settings.
	/// </summary>
	public ExploderSettings ExploderSettings { get; set; }

	/// <summary>
	/// Gets or sets the explosion generator.
	/// </summary>
	public IExploder<FireworkExplosion> Exploder { get; set; }

	/// <summary>
	/// Gets or sets the explosion generator.
	/// </summary>
	public DynamicExploder DynamicExploder { get; set; }

	/// <summary>
	/// Gets or sets the differentiator.
	/// </summary>
	public IDifferentiator Differentiator { get; set; }

	/// <summary>
	/// Gets or sets the ploynomial fit.
	/// </summary>
	public IFit PolynomialFit { get; set; }

	/// <summary>
	/// Gets or sets the function solver.
	/// </summary>
	public ISolver FunctionSolver { get; set; }

	/// <summary>
	/// Gets or sets the elite strategy generator.
	/// </summary>
	public ISparkGenerator<EliteExplosion> EliteStrategyGenerator { get; set; }
	#endregion // Properties.
}
