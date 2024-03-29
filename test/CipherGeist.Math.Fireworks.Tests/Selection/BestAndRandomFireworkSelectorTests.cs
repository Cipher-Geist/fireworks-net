namespace CipherGeist.Math.Fireworks.Tests.Selection;

public class BestAndRandomFireworkSelectorTests
{
	private static readonly Func<IEnumerable<Firework>, Firework> _getBest = SelectorTestsHelper.GetBest;
	private static readonly int _samplingNumber = SelectorTestsHelper.SamplingNumber;
	private static readonly IRandomizer _randomizer = new Randomizer();

	#region TestData.
	public static IEnumerable<object?[]> ProblemData
	{
		get
		{
			Func<IEnumerable<Firework>, Firework> best = _getBest;
			int samplingNumberParam = _samplingNumber;
			IRandomizer randomizerParam = _randomizer;

			return new[]
			{
				new object?[] { null,            best,  samplingNumberParam, "randomizer" },
				new object?[] { randomizerParam, null,  samplingNumberParam, "bestFireworkSelector" }
			};
		}
	}
	public static IEnumerable<object[]> ProblemData2
	{
		get
		{
			Func<IEnumerable<Firework>, Firework> best = _getBest;
			int samplingNumberParam = -1;
			IRandomizer randomizerParam = _randomizer;

			return new[]
			{
				new object[] { randomizerParam, best,  samplingNumberParam, "locationsNumber" }
			};
		}
	}
	#endregion // TestData.

	[TestCaseSource(nameof(ProblemData))]
	public void BestAndRandomFireworkSelectorNegativeParamsArgumentNullExceptionThrown(
		IRandomizer randomizer,
		Func<IEnumerable<Firework>, Firework> bestFireworkSelector,
		int locationsNumber,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(() =>
		{
			new BestAndRandomFireworkSelector(randomizer, bestFireworkSelector, locationsNumber);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[TestCaseSource(nameof(ProblemData2))]
	public void BestAndRandomFireworkSelectorNegative3rdParamsArgumentOutOfRangeExceptionThrown(
		IRandomizer randomizer,
		Func<IEnumerable<Firework>, Firework> bestFireworkSelector,
		int locationsNumber,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			new BestAndRandomFireworkSelector(randomizer, bestFireworkSelector, locationsNumber);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}