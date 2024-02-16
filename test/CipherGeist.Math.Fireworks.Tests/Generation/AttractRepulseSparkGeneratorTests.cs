namespace CipherGeist.Math.Fireworks.Tests.Generation;

public class AttractRepulseSparkGeneratorTests : AbstractSourceData
{
	[TestCaseSource(nameof(DataForTestCreationInstanceOfAttractRepulseGenerator))]
	public void CreateIntaceOfAttractRepulseGeneratorPassEachParameterAsNullAndOtherIsCorrectArgumentNullExceptionThrown(
		Solution bestSolution, 
		IEnumerable<Dimension> dimensions, 
		ContinuousUniformDistribution distribution,
		System.Random randomizer, 
		string expectedParamName)
	{
		var exeption = Assert.Throws<ArgumentNullException>(
			() => new AttractRepulseSparkGenerator(bestSolution, dimensions, distribution, randomizer));

		Assert.That(exeption.ParamName, Is.EqualTo(expectedParamName));
	}
}