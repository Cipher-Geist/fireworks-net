namespace CipherGeist.Math.Fireworks.Tests.State;

public class AlgorithmStateTests
{
	private readonly AlgorithmState _state;

	public AlgorithmStateTests()
	{
		_state = new AlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));
	}

	[Test]
	public void ConstructorNegativeValueExceptionThrown()
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			new AlgorithmState(Enumerable.Empty<Firework>(), -1, new Solution(0.0));
		});

		Assert.That(actualException.ParamName, Is.EqualTo("value"));
	}

	[Test]
	public void StepNumberSetterNegativeValueExceptionThrown()
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => _state.StepNumber = -1);
		Assert.That(actualException.ParamName, Is.EqualTo("value"));
	}
}