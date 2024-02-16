namespace CipherGeist.Math.Fireworks.Tests.Differentiation;

public class DifferentiationTests
{
	private readonly Differentiator differentiator;
	private readonly Func<double, double> targetFunc;
	private readonly double inputValue;

	public DifferentiationTests()
	{
		differentiator = new Differentiator();
		targetFunc = x => x * x;
		inputValue = 3;
	}

	[Test]
	public void DifferentiatePresentAllParamReturnsEqualResult()
	{
		double expectedResult = 6;

		Func<double, double> resultingFunc = differentiator.Differentiate(targetFunc);
		double actualResult = resultingFunc(inputValue);

		Assert.That(actualResult, Is.EqualTo(expectedResult));
	}

	[Test]
	public void DifferentiatePresentAllParamReturnsNonEquaResult()
	{
		double expectedResult = 5;

		Func<double, double> resultingFunc = differentiator.Differentiate(targetFunc);
		double actualResult = resultingFunc(inputValue);

		Assert.That(actualResult, Is.Not.EqualTo(expectedResult));
	}

	[Test]
	public void DifferentiateNullAs1stParamExceptionThrown()
	{
		string expectedParamName = "func";
		Func<double, double>? func = null;

		ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => differentiator.Differentiate(func));

		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}