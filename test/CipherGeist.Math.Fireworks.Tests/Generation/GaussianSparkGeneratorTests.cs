namespace CipherGeist.Math.Fireworks.Tests.Generation;

public class GaussianSparkGeneratorTests
{
	private class TestDistribution : IContinuousDistribution
	{
		public double Sample()
		{
			return 0;
		}

		public IEnumerable<double> Samples()
		{
			return new List<double>();
		}
	}

	public static IEnumerable<object?[]> ProblemData
	{
		get
		{
			var distribution = new TestDistribution();
			var dimensions = new List<Dimension>();
			var randomizer = new Randomizer();

			return new[]
			{
				new object?[] { null,       distribution, randomizer, "dimensions" },
				new object?[] { dimensions, null,         randomizer, "distribution" },
				new object?[] { dimensions, distribution, null,       "randomizer" }
			};
		}
	}

	[TestCaseSource(nameof(ProblemData))]
	public void GaussianSparkGeneratorNegativeParamsArgumentNullExceptionThrown(
		IEnumerable<Dimension> dimensions,
		IContinuousDistribution distribution,
		IRandomizer randomizer,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(() =>
		{
			new GaussianSparkGenerator(dimensions, distribution, randomizer);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}