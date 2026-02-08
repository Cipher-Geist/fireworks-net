namespace CipherGeist.Math.Fireworks.Tests.Generation;

public abstract class AbstractSourceData
{
	public const double _amplitude = 1.0D;
	public const double _delta = 0.1D;

	public static IEnumerable<object?[]> DataForTestMethodExplodeOfParallelExploder
	{
		get
		{
			var epicenter = new Firework(FireworkType.SpecificSpark, 0, 0);
			var qualities = new List<double>();

			return new[]
			{
				new object?[] { null,      qualities,  0, typeof(ArgumentNullException),       "epicenter" },
				new object?[] { epicenter, null,       0, typeof(ArgumentNullException),       "currentFireworkQualities" },
				new object?[] { epicenter, qualities, -1, typeof(ArgumentOutOfRangeException), "currentStepNumber" }
			};
		}
	}

	public static IEnumerable<object?[]> DataForTestCreationInstanceOfAttractRepulseGenerator
	{
		get
		{
			var bestSolution = new Solution(0);
			var dimensions = new List<Dimension>();
			var distribution = new ContinuousUniformDistribution(_amplitude - _delta, _amplitude + _delta);
			var randomizer = new Randomizer();

			return new[]
			{
				new object?[] { null,         dimensions, distribution, randomizer, "bestSolution" },
				new object?[] { bestSolution, null,       distribution, randomizer, "dimensions" },
				new object?[] { bestSolution, dimensions, null,         randomizer, "distribution" },
				new object?[] { bestSolution, dimensions, distribution, null,       "randomizer" }
			};
		}
	}

	public static IEnumerable<object?[]> DataForTestMethodMutateFireworkOfAttractRepulseSparkMutator
	{
		get
		{
			var coordinates = new Dictionary<Dimension, double>();
			var mutableFirework = new MutableFirework(FireworkType.SpecificSpark, 0, 0, coordinates);

			var epicenter = mutableFirework;
			var sparks = new Dictionary<FireworkType, int>();
			var explosion = new FireworkExplosion(epicenter, 1, _amplitude, sparks);

			return new[]
			{
				new object?[] { mutableFirework, null,      "explosion" },
				new object?[] { null,            explosion, "firework" }
			};
		}
	}

	public static ISparkGenerator<FireworkExplosion> CreateAttractRepulseSparkGenerator()
	{
		var bestSolution = new Solution(0);
		var dimensions = new List<Dimension>();
		var distribution = new ContinuousUniformDistribution(_amplitude - _delta, _amplitude + _delta);

		var randomizer = new Randomizer();
		var generator = new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer);

		return generator;
	}

	public static FireworkExplosion CreateFireworkExplosion(Firework epicenter)
	{
		var sparks = new Dictionary<FireworkType, int>();
		return new FireworkExplosion(epicenter, 1, _amplitude, sparks);
	}
}