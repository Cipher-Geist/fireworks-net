namespace CipherGeist.Math.Fireworks.Algorithm.Impl;

/// <summary>
/// Fireworks Algorithm implementation, per 2012 paper.
/// </summary>
public sealed class FireworksAlgorithm2012 : StepperFireworksAlgorithmBase<FireworksAlgorithmSettings2012>
{
	private const double NORMAL_DISTRIBUTION_MEAN = 1.0;
	private const double NORMAL_DISTRIBUTION_STD_DEV = 1.0;

	/// <summary>
	/// Initializes a new instance of the <see cref="FireworksAlgorithm2012"/> class.
	/// </summary>
	/// <param name="problem">The problem to be solved by the algorithm.</param>
	/// <param name="stopCondition">The stop condition for the algorithm.</param>
	/// <param name="settings">The algorithm settings.</param>
	public FireworksAlgorithm2012(Problem problem, IStopCondition stopCondition, FireworksAlgorithmSettings2012 settings)
		: base(problem, stopCondition, settings)
	{
		Randomizer = new DefaultRandom();
		BestWorstFireworkSelector = new ExtremumFireworkSelector(problem.Target);
		Distribution = new NormalDistribution(NORMAL_DISTRIBUTION_MEAN, NORMAL_DISTRIBUTION_STD_DEV);

		InitialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialDimensionRanges, Randomizer);
		ExplosionSparkGenerator = new ExplosionSparkGenerator2012(problem.Dimensions, Randomizer);
		GaussianSparkGenerator = new GaussianSparkGenerator2012(problem.Dimensions, Distribution);

		DistanceCalculator = new EuclideanDistance(problem.Dimensions);
		LocationSelector = new DistanceBasedFireworkSelector(DistanceCalculator, BestWorstFireworkSelector, Settings.LocationsNumber);
		SamplingSelector = new BestFireworkSelector(new Func<IEnumerable<Firework>, Firework>(BestWorstFireworkSelector.SelectBest));

		ExploderSettings = new ExploderSettings
		{
			ExplosionSparksNumberModifier = settings.ExplosionSparksNumberModifier,
			ExplosionSparksNumberLowerBound = settings.ExplosionSparksNumberLowerBound,
			ExplosionSparksNumberUpperBound = settings.ExplosionSparksNumberUpperBound,
			ExplosionSparksMaximumAmplitude = settings.ExplosionSparksMaximumAmplitude,
			SpecificSparksPerExplosionNumber = settings.SpecificSparksPerExplosionNumber
		};

		Exploder = new Exploder(BestWorstFireworkSelector, ExploderSettings);
		Differentiator = new Differentiator();

		PolynomialFit = new PolynomialFit(Settings.FunctionOrder);
		FunctionSolver = new Solver();

		EliteStrategyGenerator = new LS2EliteSparkGenerator(problem.Dimensions, PolynomialFit, Differentiator, FunctionSolver);
	}

	/// <summary>
	/// Creates the initial algorithm state (before the run starts).
	/// </summary>
	/// <remarks>On each call re-creates the initial state (i.e. returns 
	/// new object each time).</remarks>
	protected override void InitializeImpl()
	{
		var initialExplosion = new InitialExplosion(Settings.LocationsNumber);
		Debug.Assert(initialExplosion != null, "Initial explosion is null");

		var fireworks = InitialSparkGenerator.CreateSparks(initialExplosion);
		Debug.Assert(fireworks != null, "Initial firework collection is null");

		CalculateQualities(fireworks);
		_state = new AlgorithmState(fireworks, 0, BestWorstFireworkSelector?.SelectBest(fireworks));
	}

	/// <summary>
	/// Represents one iteration of the algorithm.
	/// </summary>
	protected override void MakeStepImpl()
	{
		ArgumentNullException.ThrowIfNull(_state, nameof(_state));

		int stepNumber = _state.StepNumber + 1;
		var specificSparkParentIndices = Randomizer.NextUniqueInt32s(Settings.SpecificSparksNumber, 0, Settings.LocationsNumber);

		int currentFirework = 0;
		var explosionSparks = new List<Firework>();
		var specificSparks = new List<Firework>(Settings.SpecificSparksNumber);

		foreach (Firework firework in _state.Fireworks)
		{
			var explosion = Exploder.Explode(firework, _state.Fireworks, stepNumber);
			var fireworkExplosionSparks = ExplosionSparkGenerator.CreateSparks(explosion);

			explosionSparks = explosionSparks.Concat(fireworkExplosionSparks).ToList();
			if (specificSparkParentIndices.Contains(currentFirework))
			{
				var fireworkSpecificSparks = GaussianSparkGenerator.CreateSparks(explosion);
				specificSparks = specificSparks.Concat(fireworkSpecificSparks).ToList();
			}
			currentFirework++;
		}

		CalculateQualities(explosionSparks);
		CalculateQualities(specificSparks);

		var allFireworks = _state.Fireworks.Concat(explosionSparks.Concat(specificSparks));

		var selectedFireworks = LocationSelector.SelectFireworks(allFireworks).ToList();
		var samplingFireworks = SamplingSelector.SelectFireworks(selectedFireworks, Settings.SamplingNumber);

		var eliteExplosion = new EliteExplosion(stepNumber, Settings.SamplingNumber, samplingFireworks);
		var eliteFirework = EliteStrategyGenerator.CreateSpark(eliteExplosion);
		CalculateQuality(eliteFirework);

		var worstFirework = BestWorstFireworkSelector?.SelectWorst(selectedFireworks);

		ArgumentNullException.ThrowIfNull(worstFirework, nameof(worstFirework));

		if (ShouldReplaceWorstWithElite(worstFirework, eliteFirework))
		{
			selectedFireworks.Remove(worstFirework);
			selectedFireworks.Add(eliteFirework);
		}

		_state.Fireworks = selectedFireworks;
		_state.StepNumber = stepNumber;
		_state.BestSolution = BestWorstFireworkSelector?.SelectBest(selectedFireworks);

		RaiseStepCompleted(new AlgorithmStateEventArgs(_state));
	}

	/// <summary>
	/// Compares two <see cref="Firework"/>s and determines if it is necessary to replace the worst one 
	/// with the elite one according to the elite strategy.
	/// </summary>
	/// <param name="worst">The worst firework on current step.</param>
	/// <param name="elite">The elite firework on current step calculated by 
	/// elite strategy</param>
	/// <returns><c>true</c> if necessary replace <paramref name="worst"/> with 
	/// <paramref name="elite"/>.</returns>
	public bool ShouldReplaceWorstWithElite(Firework worst, Firework elite)
	{
		return ProblemToSolve.Target == ProblemTarget.Minimum ?
			worst.Quality.IsGreater(elite.Quality) :
			worst.Quality.IsLess(elite.Quality);
	}

	#region Properties
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
	public ISparkGenerator<FireworkExplosion> GaussianSparkGenerator { get; set; }

	/// <summary>
	/// Gets or sets the distance calculator.
	/// </summary>
	public IDistance DistanceCalculator { get; set; }

	/// <summary>
	/// Gets or sets the location selector.
	/// </summary>
	public IFireworkSelector LocationSelector { get; set; }

	/// <summary>
	/// Gets or sets the sampling selector.
	/// </summary>
	public IFireworkSelector SamplingSelector { get; set; }

	/// <summary>
	/// Gets or sets the explosion settings.
	/// </summary>
	public ExploderSettings ExploderSettings { get; set; }

	/// <summary>
	/// Gets or sets the explosion generator.
	/// </summary>
	public IExploder<FireworkExplosion> Exploder { get; set; }

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
	#endregion // Properties
}