namespace CipherGeist.Math.Fireworks.Tests.Extensions;

public class RandomizerExtentionsTests
{
	private readonly IRandomizer _randomizer;

	public RandomizerExtentionsTests()
	{
		_randomizer = new Randomizer();
	}

	public static IEnumerable<object[]> NextDoubleData
	{
		get
		{
			var randomizer = new Randomizer();
			const double minInclusive = 10.05D;
			const double intervalLength = 11.984D;
			return new[] 
			{
				new object[] { randomizer, double.NaN,              intervalLength,          "minInclusive" },
				new object[] { randomizer, double.PositiveInfinity, intervalLength,          "minInclusive" },
				new object[] { randomizer, double.NegativeInfinity, intervalLength,          "minInclusive" },
				new object[] { randomizer, minInclusive,            double.NaN,              "intervalLength" },
				new object[] { randomizer, minInclusive,            double.PositiveInfinity, "intervalLength" },
				new object[] { randomizer, minInclusive,            double.NegativeInfinity, "intervalLength" }
			};
		}
	}

	public static IEnumerable<object[]> NextInt32sData
	{
		get
		{
			var randomizer = new Randomizer();
			const int minInclusive = 10;
			const int maxExclusive = 17;
			const int neededValuesNumber = 3;
			return new[] 
			{
				new object[] { randomizer, -1,                 minInclusive, maxExclusive, "neededValuesNumber" },
				new object[] { randomizer, neededValuesNumber, 15,           maxExclusive, "neededValuesNumber" },
				new object[] { randomizer, neededValuesNumber, 18,           maxExclusive, "maxExclusive" }
			};
		}
	}

	[TestCaseSource(nameof(NextDoubleData))]
	public void NextDouble_NegativeDoubleArgs_ExceptionThrown(
		IRandomizer random, 
		double minInclusive, 
		double intervalLength, 
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			random.NextDouble(minInclusive, intervalLength);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[TestCaseSource(nameof(NextInt32sData))]
	public void NextInt32s_NegativeIntArgs_ExceptionThrown(
		IRandomizer random, 
		int neededValuesNumber, 
		int minInclusive, 
		int maxExclusive, 
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			random.NextInt32s(neededValuesNumber, minInclusive, maxExclusive);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[TestCaseSource(nameof(NextInt32sData))]
	public void NextUniqueInt32s_NegativeIntArgs_ExceptionThrown(
		IRandomizer random, 
		int neededValuesNumber, 
		int minInclusive, 
		int maxExclusive, 
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			random.NextUniqueInt32s(neededValuesNumber, minInclusive, maxExclusive);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}