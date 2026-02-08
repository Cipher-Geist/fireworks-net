namespace CipherGeist.Math.Fireworks.Tests.Explode;

[TestFixture]
public class DynamicExploderTests
{
	private InitialSparkGenerator _initialSparkGenerator;
	private ExplosionSparkGenerator2012 _explosionSparkGenerator;
	private DynamicExploder _dynamicExploder;

	[SetUp]
	public void SetUp()
	{
		var target = ProblemTarget.Minimum;
		var func = new Func<IDictionary<Dimension, double>, double>((c) => { return 0.0; });
		var dimensions = new List<Dimension>
		{
			new Dimension(new Interval(-1, 1)),
			new Dimension(new Interval(-1, 1))
		};
		var problem = new Problem(dimensions, func, target);

		var randomizer = new Randomizer();
		var bestWorstFireworkSelector = new ExtremumFireworkSelector(problem.Target);

		_initialSparkGenerator = new InitialSparkGenerator(problem.Dimensions, problem.InitialDimensionRanges, randomizer);
		_explosionSparkGenerator = new ExplosionSparkGenerator2012(problem.Dimensions, randomizer);

		var bestSelector = new Func<IEnumerable<Firework>, Firework>(bestWorstFireworkSelector.SelectBest);
		var bestAndRandomFireworkSelector = new BestAndRandomFireworkSelector(randomizer, bestSelector);

		var exploderSettings = new ExploderSettings
		{
			ExplosionSparksNumberModifier = 50.0,
			ExplosionSparksNumberLowerBound = 0.04,
			ExplosionSparksNumberUpperBound = 0.8,
			ExplosionSparksMaximumAmplitude = 20.0,
			SpecificSparksPerExplosionNumber = 1,
			AmplificationCoefficent = 1.2,
			ReductionCoefficent = 0.9
		};

		var exploder = new Exploder(bestWorstFireworkSelector, exploderSettings);
		var distanceCalculator = new EuclideanDistance(problem.Dimensions);

		_dynamicExploder = new DynamicExploder(bestWorstFireworkSelector, problem.Target, exploderSettings);
	}

	[Test]
	public void DoesInitializeCorrectly()
	{
		int locationsNumber = 2;

		var initialExplosion = new InitialExplosion(locationsNumber);
		var fireworks = _initialSparkGenerator.CreateSparks(initialExplosion);
		Assert.That(fireworks.Count(), Is.EqualTo(locationsNumber));

		_dynamicExploder.InitializeCoreFirework(fireworks);
		Assert.That(_dynamicExploder.CoreFirework, Is.Not.Null);
		Assert.That(
			_dynamicExploder.CoreFireworkAmplitutePreviousGeneration, 
			Is.EqualTo(_dynamicExploder.Settings.ExplosionSparksMaximumAmplitude));
	}

	[Test]
	public void UpdateCoreFireworksCorrectlySetsCoreFireworkAmplitude()
	{
		int locationsNumber = 2;

		var initialExplosion = new InitialExplosion(locationsNumber);
		var fireworks = _initialSparkGenerator.CreateSparks(initialExplosion).ToList();
		Assert.That(fireworks.Count, Is.EqualTo(locationsNumber));

		// Set qualities of the intialized fireworks.
		fireworks[0].Quality = 10.0;
		fireworks[1].Quality = 20.0;

		_dynamicExploder.InitializeCoreFirework(fireworks);
		Assert.That(_dynamicExploder.CoreFirework, Is.EqualTo(fireworks[0]));

		var explosionSparks = new List<Firework>();
		foreach (var firework in fireworks)
		{
			var dynamicExplosion = _dynamicExploder.Explode(firework, fireworks, 1);
			explosionSparks = explosionSparks.Concat(_explosionSparkGenerator.CreateSparks(dynamicExplosion)).ToList();
		}

		var newCoreFirework = explosionSparks
			.Where(fw => fw.ParentFirework == _dynamicExploder.CoreFirework)
			.First();
		newCoreFirework.Quality = 0.0;

		_dynamicExploder.UpdateCoreFirework(explosionSparks);
		Assert.That(_dynamicExploder.CoreFirework, Is.EqualTo(newCoreFirework));
	}
}
