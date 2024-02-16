namespace CipherGeist.Math.Fireworks.Tests.Extensions;

public class DoubleExtensionsTests
{
	private const double _lesserValue = 10.05D;
	private const double _greaterValue = 11.984D;

	[Theory]
	[TestCase(_lesserValue, _greaterValue, false)]
	[TestCase(_lesserValue, _lesserValue, true)]
	public void IsEqual_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
	{
		Assert.That(firstValue.IsEqual(secondValue), Is.EqualTo(expected));
	}

	[Theory]
	[TestCase(_lesserValue, _greaterValue, true)]
	[TestCase(_lesserValue, _lesserValue, false)]
	[TestCase(_greaterValue, _lesserValue, false)]
	public void IsLess_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
	{
		Assert.That(firstValue.IsLess(secondValue), Is.EqualTo(expected));
	}

	[Theory]
	[TestCase(_lesserValue, _greaterValue, true)]
	[TestCase(_lesserValue, _lesserValue, true)]
	[TestCase(_greaterValue, _lesserValue, false)]
	public void IsLessOrEqual_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
	{
		Assert.That(firstValue.IsLessOrEqual(secondValue), Is.EqualTo(expected));
	}

	[Theory]
	[TestCase(_lesserValue, _greaterValue, false)]
	[TestCase(_lesserValue, _lesserValue, false)]
	[TestCase(_greaterValue, _lesserValue, true)]
	public void IsGreater_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
	{
		Assert.That(firstValue.IsGreater(secondValue), Is.EqualTo(expected));
	}

	[Theory]
	[TestCase(_lesserValue, _greaterValue, false)]
	[TestCase(_lesserValue, _lesserValue, true)]
	[TestCase(_greaterValue, _lesserValue, true)]
	public void IsGreaterOrEqual_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, bool expected)
	{
		Assert.That(firstValue.IsGreaterOrEqual(secondValue), Is.EqualTo(expected));
	}

	[Test]
	public void ToStringInvariant_PassedDouble_ReturnsValidString()
	{
		string expectedValueString = "10.05";

		string valueString = _lesserValue.ToStringInvariant();

		Assert.That(valueString, Is.EqualTo(expectedValueString));
	}
}

public class DoubleExtensionComparerTests
{
	private readonly DoubleExtensionComparer comparer;
	private const double _lesserValue = 10.05D;
	private const double _greaterValue = 11.984D;

	public DoubleExtensionComparerTests()
	{
		comparer = new DoubleExtensionComparer();
	}

	[Theory]
	[TestCase(_lesserValue, _greaterValue, -1)]
	[TestCase(_lesserValue, _lesserValue, 0)]
	[TestCase(_greaterValue, _lesserValue, 1)]
	public void Compare_PassedDifferentArgs_ReturnsExpected(double firstValue, double secondValue, int expected)
	{
		Assert.That(comparer.Compare(firstValue, secondValue), Is.EqualTo(expected));
	}
}