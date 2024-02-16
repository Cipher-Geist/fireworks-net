namespace CipherGeist.Math.Fireworks.Tests.Generation;

public class InitialSparkGeneratorTests
{
	public static IEnumerable<object?[]> ProblemData
	{
		get
		{
			var initialRandes = new Dictionary<Dimension, Interval>();
			var dimensions = new List<Dimension>();
			var randomizer = new System.Random();

			return new[] 
			{
				new object?[] { null,       initialRandes, randomizer, "dimensions" },
				new object?[] { dimensions, null,          randomizer, "initialRanges" },
				new object?[] { dimensions, initialRandes, null,       "randomizer" }
			};
		}
	}

	public static IEnumerable<object?[]> ProblemData2
	{
		get
		{
			var dimensions = new List<Dimension>();
			var randomizer = new System.Random();

			return new[] 
			{
				new object?[] { null,       randomizer, "dimensions" },
				new object?[] { dimensions, null,       "randomizer" }
			};
		}
	}

	[TestCaseSource(nameof(ProblemData))]
	public void InitialSparkGeneratorNegativeParamsArgumentNullExceptionThrown(
		IEnumerable<Dimension> dimensions,
		IDictionary<Dimension, Interval> initialRandes,
		System.Random randomizer,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(() => new InitialSparkGenerator(dimensions, initialRandes, randomizer));
		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}