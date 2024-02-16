namespace CipherGeist.Math.Fireworks.Tests.Generation;

public class EliteSparkGeneratorTests
{
	public class TestEliteStrategyGenerator : EliteSparkGenerator
	{
		public TestEliteStrategyGenerator(IEnumerable<Dimension> dimensions, IFit polynomialFit)
			: base(dimensions, polynomialFit)
		{
		}

		protected override double CalculateElitePoint(Func<double, double> func, Interval variationRange)
		{
			return 0;
		}

		public IDictionary<Dimension, Func<double, double>> TestApproximateFitnessLandscapes(IEnumerable<Firework> fireworks)
		{
			return ApproximateFitnessLandscapes(fireworks);
		}
	}

	public static IEnumerable<object?[]> ProblemData
	{
		get
		{
			var polynomialFit = new PolynomialFit(0);
			var dimensions = new List<Dimension>();

			return new[] 
			{
				new object?[] { null,       polynomialFit, "dimensions" },
				new object?[] { dimensions, null,          "polynomialFit" }
			};
		}
	}

	public static TestEliteStrategyGenerator GetTestEliteStrategyGenerator()
	{
		return new TestEliteStrategyGenerator(new List<Dimension>(), new PolynomialFit(0));
	}

	[TestCaseSource(nameof(ProblemData))]
	public void ProblemNegativeParamsArgumentNullExceptionThrown(
		IEnumerable<Dimension> dimensions,
		IFit polynomialFit,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(
			() => new TestEliteStrategyGenerator(dimensions, polynomialFit));

		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[Theory]
	[TestCase(null, "fireworks")]
	public void ApproximateFitnessLandscapesNegativeParamsArgumentNullExceptionThrown(IEnumerable<Firework> fireworks, string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(
			() => GetTestEliteStrategyGenerator().TestApproximateFitnessLandscapes(fireworks));

		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}