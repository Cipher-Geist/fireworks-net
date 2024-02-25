namespace CipherGeist.Math.Fireworks.Tests.Selection.Extremum;

[TestFixture]
public class ExtremumFireworkSelectorTests
{
	[TestCase(ProblemTarget.Minimum, 5)]
	[TestCase(ProblemTarget.Maximum, 20)]
	public void SelectBest_WhenProblemTargetIsMinimum_ShouldReturnFireworkWithLowestQuality(
		ProblemTarget problemTarget, 
		double expected)
	{
		var selector = new ExtremumFireworkSelector(problemTarget);
		var fireworks = new List<Firework>
		{
			new Firework(FireworkType.Initial, 0, 0) { Quality = 10 },
			new Firework(FireworkType.SpecificSpark, 0, 1) { Quality = 5 },
			new Firework(FireworkType.Initial, 0, 2) { Quality = 20 }
		};

		var bestFirework = selector.SelectBest(fireworks);

		Assert.That(bestFirework.Quality, Is.EqualTo(expected));
	}
}
