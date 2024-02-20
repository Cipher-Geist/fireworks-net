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

		var resultingFunc = differentiator.Differentiate(targetFunc);
		var actualResult = resultingFunc(inputValue);

		Assert.That(actualResult, Is.EqualTo(expectedResult));
	}

	[Test]
	public void DifferentiatePresentAllParamReturnsNonEquaResult()
	{
		double expectedResult = 5;

		var resultingFunc = differentiator.Differentiate(targetFunc);
		var actualResult = resultingFunc(inputValue);

		Assert.That(actualResult, Is.Not.EqualTo(expectedResult));
	}

	[Test]
	public void DifferentiateNullAs1stParamExceptionThrown()
	{
		string expectedParamName = "func";
		Func<double, double>? func = null;

		var actualException = Assert.Throws<ArgumentNullException>(() => differentiator.Differentiate(func));

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}