namespace CipherGeist.Math.Fireworks.Tests.Differentiation;

[TestFixture]
public class DifferentiationTests
{
	private readonly Differentiator _differentiator;
	private readonly Func<double, double> _linearFunc;
	private readonly Func<double, double> _trigFunc;
	private readonly Func<double, double> _expFunc;
	private readonly double _inputValue;
	private double _tolerance = 1e-5;

	public DifferentiationTests()
	{
		_differentiator = new Differentiator();
		_linearFunc = x => (2 * x) + 3; // Linear function
		_trigFunc = x => System.Math.Sin(x); // Trigonometric function
		_expFunc = x => System.Math.Exp(x); // Exponential function
		_inputValue = System.Math.PI; // A common value for testing, especially with trig functions
	}

	[Test]
	public void DifferentiateLinearFunctionCorrectResult()
	{
		double expectedResult = 2;

		var resultingFunc = _differentiator.Differentiate(_linearFunc);
		var actualResult = resultingFunc(_inputValue);

		Assert.That(actualResult, Is.EqualTo(expectedResult).Within(_tolerance));
	}

	[Test]
	public void DifferentiateTrigFunctionCorrectResult()
	{
		double expectedResult = System.Math.Cos(_inputValue);

		var resultingFunc = _differentiator.Differentiate(_trigFunc);
		var actualResult = resultingFunc(_inputValue);

		Assert.That(actualResult, Is.EqualTo(expectedResult).Within(_tolerance));
	}

	[Test]
	public void DifferentiateExpFunctionCorrectResult()
	{
		double expectedResult = System.Math.Exp(_inputValue);

		var resultingFunc = _differentiator.Differentiate(_expFunc);
		var actualResult = resultingFunc(_inputValue);

		Assert.That(actualResult, Is.EqualTo(expectedResult).Within(_tolerance));
	}

	[Test]
	public void DifferentiatePerformance()
	{
		Func<double, double> complexFunc = x => System.Math.Exp(-x) * System.Math.Sin(x);
		var watch = System.Diagnostics.Stopwatch.StartNew();
		var resultingFunc = _differentiator.Differentiate(complexFunc);
		watch.Stop();

		var executionTime = watch.ElapsedMilliseconds;
		Console.WriteLine($"Execution Time: {executionTime} ms");

		// Assert that the differentiation process completes within a reasonable time frame
		// This threshold might need adjustment based on expected performance criteria
		Assert.That(executionTime, Is.LessThan(1000), "Differentiation took too long.");
	}
}