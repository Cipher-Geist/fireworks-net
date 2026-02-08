namespace CipherGeist.Math.Fireworks.Tests.Generation;

public class ExplosionSparkGeneratorTests
{
	public static IEnumerable<object?[]> ProblemData
	{
		get
		{
			var randomizer = new Randomizer();
			var dimensions = new List<Dimension>();

			return new[] 
			{
				new object?[] { null,       randomizer, "dimensions" },
				new object?[] { dimensions, null,       "randomizer" }
			};
		}
	}

	[TestCaseSource(nameof(ProblemData))]
	public void ExplosionSparkGenerator_NegativeParams_ArgumentNullExceptionThrown(
		IEnumerable<Dimension> dimensions,
		IRandomizer randomizer,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(() => new ExplosionSparkGenerator(dimensions, randomizer));
		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}