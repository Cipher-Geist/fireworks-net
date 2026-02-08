namespace CipherGeist.Math.Fireworks.Tests.Model;

public class DimensionTests
{
	private readonly Dimension dimension;

	public DimensionTests()
	{
		dimension = new Dimension(new Interval(5, 15.5));
	}

	[Theory]
	[TestCase(10, true)]
	[TestCase(16, false)]
	[TestCase(4.5, false)]
	public void IsValueInRangeCalculationPositiveExpected(double value, bool expected)
	{
		bool actual = dimension.IsValueInRange(value);

		Assert.That(actual, Is.EqualTo(expected));
	}
}