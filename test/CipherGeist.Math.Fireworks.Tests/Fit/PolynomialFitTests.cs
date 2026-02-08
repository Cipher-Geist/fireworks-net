using MathNet.Numerics;

namespace CipherGeist.Math.Fireworks.Tests.Fit;

public class PolynomialFitTests
{
	private readonly PolynomialFit _polynomialFit;
	private readonly double[] _coordinates;
	private readonly double[] _qualities;

	public PolynomialFitTests()
	{
		_polynomialFit = new PolynomialFit(1);

		// Test data taken from https://github.com/mathnet/mathnet-numerics/tree/master/src/UnitTests.
		_coordinates = Enumerable
			.Range(1, 6)
			.Select(Convert.ToDouble)
			.ToArray();

		_qualities = new[] { 4.986, 2.347, 2.061, -2.995, -2.352, -5.782 };
	}

	[Test]
	public void PolynomailFitNegative_Throws()
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new PolynomialFit(-1));
	}

	[Test]
	public void ApproximatePresentAllParamReturnsEqualResults()
	{
		Func<double, double> expectedFunc = x => 7.01013 - (2.08551 * x);
		Func<double, double> actualFunc = _polynomialFit.Approximate(_coordinates, _qualities);

		foreach (double value in Enumerable.Range(-3, 10))
		{
			Assert.That(actualFunc(value), Is.EqualTo(expectedFunc(value)).Within(2));
		}
	}

	[Test]
	public void ApproximatePresentAllParamReturnsNonEqualResults()
	{
		Func<double, double> expectedFunc = x => 5.02435 - (2.08551 * x);
		Func<double, double> actualFunc = _polynomialFit.Approximate(_coordinates, _qualities);

		foreach (double value in Enumerable.Range(-3, 10))
		{
			Assert.That(!expectedFunc(value).AlmostEqual(actualFunc(value), 2), Is.True);
		}
	}
}