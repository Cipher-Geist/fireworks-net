namespace CipherGeist.Math.Fireworks.Tests.Model;

public class IntervalTests
{
	private readonly Interval _range;

	public IntervalTests()
	{
		_range = new Interval(-1.0, false, 5.5, true);
	}

	public static IEnumerable<object[]> RangeData
	{
		get
		{
			var range1 = Interval.Create(100, 50, true, false);
			var range2 = Interval.CreateWithRestrictions(100, 50, 40, 160, true, false);

			var range3 = new Interval(40, false, 60, true);
			var range4 = new Interval(40, false, 60, false);

			return new[] 
			{
				new object[] { range1, range2,      true },
				new object[] { range1, range3,      false },
				new object[] { range4, range3,      false },
				new object[] { range1, "badObject", false }
			};
		}
	}

	public static IEnumerable<object[]> RangeDataOfCreateWithRestrictionsMethod
	{
		get
		{
			int deviationPercent = 1;
			double mean = 0.0;
			double deviationValue = 1.0;
			double minRestriction = 0;
			double maxRestriction = 2;

			return new[] 
			{
				new object[] { mean,                     deviationPercent,        double.NaN,     maxRestriction, "minimum" },
				new object[] { mean,                     deviationPercent,        minRestriction, double.NaN,     "maximum" },
				new object[] { mean,                     deviationPercent,        2,              -2,             "minimum" },
				new object[] { double.NaN,               deviationPercent,        minRestriction, maxRestriction, "mean" },
				new object[] { double.PositiveInfinity,  deviationPercent,        minRestriction, maxRestriction, "mean" },
				new object[] { double.NegativeInfinity,  deviationPercent,        minRestriction, maxRestriction, "mean" },
				new object[] { double.NaN,               deviationValue,          minRestriction, maxRestriction, "mean" },
				new object[] { double.PositiveInfinity,  deviationValue,          minRestriction, maxRestriction, "mean" },
				new object[] { double.NegativeInfinity,  deviationValue,          minRestriction, maxRestriction, "mean" },
				new object[] { mean,                     -1,                      minRestriction, maxRestriction, "deviationPercent" },
				new object[] { mean,                     double.NaN,              minRestriction, maxRestriction, "deviationValue" },
				new object[] { mean,                     double.PositiveInfinity, minRestriction, maxRestriction, "deviationValue" },
				new object[] { mean,                     double.NegativeInfinity, minRestriction, maxRestriction, "deviationValue" }
			};
		}
	}

	public static IEnumerable<object[]> RangeDataOfCreateMethod
	{
		get
		{
			int deviationPercent = 1;
			double mean = 0.0;
			double deviationValue = 1.0;

			return new[] 
			{
				new object[] { double.NaN,               deviationPercent,        "mean" },
				new object[] { double.PositiveInfinity,  deviationPercent,        "mean" },
				new object[] { double.NegativeInfinity,  deviationPercent,        "mean" },
				new object[] { double.NaN,               deviationValue,          "mean" },
				new object[] { double.PositiveInfinity,  deviationValue,          "mean" },
				new object[] { double.NegativeInfinity,  deviationValue,          "mean" },
				new object[] { mean,                     -1,                      "deviationPercent" },
				new object[] { mean,                     double.NaN,              "deviationValue" },
				new object[] { mean,                     double.PositiveInfinity, "deviationValue" },
				new object[] { mean,                     double.NegativeInfinity, "deviationValue" }
			};
		}
	}

	[Theory]
	[TestCase(10, false)]
	[TestCase(16, false)]
	[TestCase(-4.5, false)]
	[TestCase(5.5, false)]
	[TestCase(0.0, true)]
	[TestCase(3.8, true)]
	[TestCase(-1.0, true)]
	public void IsValueInRange_Calculation_PositiveExpected(double value, bool expected)
	{
		bool actual = _range.IsInRange(value);
		Assert.That(actual, Is.EqualTo(expected));
	}

	[TestCaseSource(nameof(RangeData))]
	public void GetHashCode_RangesVariations_PositiveExpected(object range1, object obj, bool expected)
	{
		int hash1 = range1.GetHashCode();
		int hash2 = obj.GetHashCode();

		bool actual = hash1 == hash2;

		Assert.That(actual, Is.EqualTo(expected));
	}

	[TestCaseSource(nameof(RangeData))]
	public void Equals_RangesVariations_PositiveExpected(object range1, object obj, bool expected)
	{
		bool actual = range1.Equals(obj);

		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void Range_NaNAs1tsParam_ArgumentOutOfRangeExceptionThrown()
	{
		double min = double.NaN;
		double max = 0.0D;
		string expectedParamName = "minimum";

		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Interval(min, max));

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[Test]
	public void Range_NaNAs2ndParam_ArgumentOutOfRangeExceptionThrown()
	{
		double min = 0.0D;
		double max = double.NaN;
		string expectedParamName = "maximum";

		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Interval(min, max));

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[Test]
	public void Range_СonfusedAs1stAND2ndParam_ArgumentOutOfRangeExceptionThrown()
	{
		double min = 1.0D;
		double max = -1.0D;
		string expectedParamName = "minimum";

		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Interval(min, max));

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[TestCaseSource(nameof(RangeDataOfCreateMethod))]
	public void Create_NegativeParams_ArgumentOutOfRangeExceptionThrown(double mean, object deviation, string expectedParamName)
	{
		ArgumentOutOfRangeException actualException;
		if (deviation is double)
		{
			actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Interval.Create(mean, (double)deviation));
		}
		else
		{
			actualException = Assert.Throws<ArgumentOutOfRangeException>(() => Interval.Create(mean, (int)deviation));
		}

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[TestCaseSource(nameof(RangeDataOfCreateWithRestrictionsMethod))]
	public void CreateWithRestrictions_NegativeParams_ArgumentOutOfRangeExceptionThrown(
		double mean, 
		object deviation, 
		double minRestriction, 
		double maxRestriction, 
		string expectedParamName)
	{
		ArgumentOutOfRangeException actualException;
		if (deviation is double)
		{
			actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				Interval.CreateWithRestrictions(mean, (double)deviation, minRestriction, maxRestriction);
			});
		}
		else
		{
			actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				Interval.CreateWithRestrictions(mean, (int)deviation, minRestriction, maxRestriction);
			});
		}

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}