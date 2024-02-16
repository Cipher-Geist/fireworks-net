namespace CipherGeist.Math.Fireworks.Tests.Generation;

public class LS2EliteSparkGeneratorTests
{
	public static IEnumerable<object?[]> ProblemData
	{
		get
		{
			var polynomialFit = new PolynomialFit(0);
			var dimensions = new List<Dimension>();
			var differentiator = new Differentiator();
			var solver = new Solver();

			return new[] 
			{
				new object?[] { null,       polynomialFit, differentiator, solver, "dimensions" },
				new object?[] { dimensions, null,          differentiator, solver, "polynomialFit" },
				new object?[] { dimensions, polynomialFit, null,           solver, "differentiator" },
				new object?[] { dimensions, polynomialFit, differentiator, null,   "solver" }
			};
		}
	}

	[TestCaseSource(nameof(ProblemData))]
	public void LS2EliteStrategyGeneratorNegativeParamsArgumentNullExceptionThrown(
		IEnumerable<Dimension> dimensions,
		IFit polynomialFit,
		IDifferentiator differentiator,
		ISolver solver,
		string expectedParamName)
	{
		ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(
			() => new LS2EliteSparkGenerator(dimensions, polynomialFit, differentiator, solver));

		Assert.NotNull(actualException);
		Assert.AreEqual(expectedParamName, actualException.ParamName);
	}
}