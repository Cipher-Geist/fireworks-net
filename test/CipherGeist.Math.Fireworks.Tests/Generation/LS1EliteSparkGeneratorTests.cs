namespace CipherGeist.Math.Fireworks.Tests.Generation;

public class LS1EliteSparkGeneratorTests
{
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

	[TestCaseSource(nameof(ProblemData))]
	public void LS1EliteStrategyGeneratorNegativeParamsArgumentNullExceptionThrown(
		IEnumerable<Dimension> dimensions,
		IFit polynomialFit,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(
			() => new LS1EliteSparkGenerator(dimensions, polynomialFit));

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}