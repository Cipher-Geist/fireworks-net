namespace CipherGeist.Math.Fireworks.Tests.Explode;

public class ExploderTests
{
	private readonly Exploder _exploder;
	private readonly IExtremumFireworkSelector _extremumFireworkSelector;

	public ExploderTests()
	{
		_extremumFireworkSelector = new ExtremumFireworkSelector(ProblemTarget.Minimum);
		_exploder = new Exploder(_extremumFireworkSelector, new ExploderSettings());
	}

	[Test]
	public void ExplodeNegativeCurrentStepNumberExceptionThrown()
	{
		string expectedParamName = "currentStepNumber";

		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_exploder.Explode(new Firework(FireworkType.Initial, 1, 0), Enumerable.Empty<Firework>(), -1);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[Test]
	public void ExplodeStepNumberLessThanFocusBirthStepNumberExceptionThrown()
	{
		string expectedParamName = "currentStepNumber";

		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_exploder.Explode(new Firework(FireworkType.Initial, 1, 0), Enumerable.Empty<Firework>(), 0);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}