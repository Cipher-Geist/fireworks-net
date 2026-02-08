namespace CipherGeist.Math.Fireworks.Tests.Selection;

public class BestSelectorTests
{
	private readonly int samplingNumber;
	private readonly int countFireworks;
	private readonly Func<IEnumerable<Firework>, Firework> getBest;
	private readonly IEnumerable<Firework> allFireworks;
	private readonly BestFireworkSelector bestSelector;

	public BestSelectorTests()
	{
		samplingNumber = SelectorTestsHelper.SamplingNumber;
		countFireworks = SelectorTestsHelper.CountFireworks;
		getBest = SelectorTestsHelper.GetBest;
		allFireworks = SelectorTestsHelper.Fireworks;
		bestSelector = new BestFireworkSelector(getBest, samplingNumber);
	}

	[Test]
	public void SelectFireworksPresentAllParamReturnsEqualFireworks()
	{
		var expectedFireworks = SelectorTestsHelper.BestFireworks;
		var resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

		Assert.That(resultingFireworks, Is.Not.SameAs(expectedFireworks));
		Assert.That(resultingFireworks, Is.EqualTo(expectedFireworks));
	}

	[Test]
	public void SelectFireworksPresentAllParamReturnsNonEqualFireworks()
	{
		var expectedFireworks = SelectorTestsHelper.NonBestFireworks;
		var resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

		Assert.That(resultingFireworks, Is.Not.SameAs(expectedFireworks));
		Assert.That(resultingFireworks, Is.Not.EqualTo(expectedFireworks));
	}

	[Test]
	public void SelectFireworksNegativeNumberAs2ndParamExceptionThrown()
	{
		string expectedParamName = "numberToSelect";
		int samplingNumber = -1;

		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			bestSelector.SelectFireworks(allFireworks, samplingNumber);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[Test]
	public void SelectFireworksAsManyAspossibleWithoutException()
	{
		int samplingNumber = countFireworks + 1;
		var sampledFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

		Assert.That(sampledFireworks.Count(), Is.EqualTo(countFireworks));
	}

	[Test]
	public void SelectFireworksCountFireworksEqual2ndParamReturnsEqualFireworks()
	{
		var expectedFireworks = allFireworks;
		int samplingNumber = countFireworks;

		var resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

		Assert.That(resultingFireworks, Is.Not.SameAs(expectedFireworks));
		Assert.That(resultingFireworks, Is.EqualTo(expectedFireworks));
	}

	[Test]
	public void SelectFireworksNullAs2ndParamReturnsEmptyCollectionFireworks()
	{
		var expectedFireworks = new List<Firework>();
		int samplingNumber = 0;

		var resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

		Assert.That(resultingFireworks, Is.Not.SameAs(expectedFireworks));
		Assert.That(resultingFireworks, Is.EqualTo(expectedFireworks));
	}
}