namespace CipherGeist.Math.Fireworks.Tests.Extensions;

public class RandomExtentionsTests
{
	private readonly System.Random _random;

	public RandomExtentionsTests()
	{
		_random = new System.Random();
	}

	public static IEnumerable<object[]> NextDoubleData
	{
		get
		{
			var rnd = new System.Random();
			const double minInclusive = 10.05D;
			const double intervalLength = 11.984D;
			return new[] 
			{
				new object[] { rnd, double.NaN,              intervalLength,          "minInclusive" },
				new object[] { rnd, double.PositiveInfinity, intervalLength,          "minInclusive" },
				new object[] { rnd, double.NegativeInfinity, intervalLength,          "minInclusive" },
				new object[] { rnd, minInclusive,            double.NaN,              "intervalLength" },
				new object[] { rnd, minInclusive,            double.PositiveInfinity, "intervalLength" },
				new object[] { rnd, minInclusive,            double.NegativeInfinity, "intervalLength" }
			};
		}
	}

	public static IEnumerable<object[]> NextInt32sData
	{
		get
		{
			var rnd = new System.Random();
			const int minInclusive = 10;
			const int maxExclusive = 17;
			const int neededValuesNumber = 3;
			return new[] 
			{
				new object[] { rnd, -1,                 minInclusive, maxExclusive, "neededValuesNumber" },
				new object[] { rnd, neededValuesNumber, 15,           maxExclusive, "neededValuesNumber" },
				new object[] { rnd, neededValuesNumber, 18,           maxExclusive, "maxExclusive" }
			};
		}
	}

	[TestCaseSource(nameof(NextDoubleData))]
	public void NextDouble_NegativeDoubleArgs_ExceptionThrown(
		System.Random random, 
		double minInclusive, 
		double intervalLength, 
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => random.NextDouble(minInclusive, intervalLength));

		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[TestCaseSource(nameof(NextInt32sData))]
	public void NextInt32s_NegativeIntArgs_ExceptionThrown(
		System.Random random, 
		int neededValuesNumber, 
		int minInclusive, 
		int maxExclusive, 
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => random.NextInt32s(neededValuesNumber, minInclusive, maxExclusive));

		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[TestCaseSource(nameof(NextInt32sData))]
	public void NextUniqueInt32s_NegativeIntArgs_ExceptionThrown(
		System.Random random, 
		int neededValuesNumber, 
		int minInclusive, 
		int maxExclusive, 
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
			random.NextUniqueInt32s(neededValuesNumber, minInclusive, maxExclusive));

		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}