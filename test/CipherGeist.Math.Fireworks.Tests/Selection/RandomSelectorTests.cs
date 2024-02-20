namespace CipherGeist.Math.Fireworks.Tests.Selection;

public class RandomSelectorTests
{
	private readonly int _samplingNumber;
	private readonly int _countFireworks;
	private readonly System.Random _randomizer;
	private readonly IEnumerable<Firework> _allFireworks;
	private readonly RandomFireworkSelector _randomSelector;

	public RandomSelectorTests()
	{
		_samplingNumber = SelectorTestsHelper.SamplingNumber;
		_countFireworks = SelectorTestsHelper.CountFireworks;
		_randomizer = new System.Random();
		_allFireworks = SelectorTestsHelper.Fireworks;
		_randomSelector = new RandomFireworkSelector(_randomizer, _samplingNumber);
	}

	[Test]
	public void SelectFireworksPresentAllParamReturnsExistsFireworks()
	{
		var resultingFireworks = _randomSelector.SelectFireworks(_allFireworks, _samplingNumber);
		foreach (var firework in resultingFireworks)
		{
			Assert.That(_allFireworks, Does.Contain(firework));
		}
	}

	[Test]
	public void SelectFireworksPresentAllParamReturnsNonEqualCollections()
	{
		var firstResultingFireworks = _randomSelector.SelectFireworks(_allFireworks, _samplingNumber);
		var secondResultingFireworks = _randomSelector.SelectFireworks(_allFireworks, _samplingNumber);

		Assert.That(secondResultingFireworks, Is.Not.SameAs(firstResultingFireworks));
		Assert.That(secondResultingFireworks, Is.Not.EqualTo(firstResultingFireworks));
	}

	[Test]
	public void SelectFireworksNegativeNumberAs2ndParamExceptionThrown()
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_randomSelector.SelectFireworks(_allFireworks, -1);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo("numberToSelect"));
	}

	[Test]
	public void SelectFireworksGreatNumberAs2ndParamExceptionThrown()
	{
		var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_randomSelector.SelectFireworks(_allFireworks, _countFireworks + 1);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo("numberToSelect"));
	}

	[Test]
	public void SelectFireworksCountFireworksEqual2ndParamReturnsEqualFireworks()
	{
		var resultingFireworks = _randomSelector.SelectFireworks(_allFireworks, _countFireworks);

		Assert.That(resultingFireworks, Is.Not.SameAs(_allFireworks));
		Assert.That(resultingFireworks, Is.EqualTo(_allFireworks));
	}

	[Test]
	public void SelectFireworksZeroAs2ndParamReturnsEmptyCollectionFireworks()
	{
		var resultingFireworks = _randomSelector.SelectFireworks(_allFireworks, 0);

		Assert.That(resultingFireworks, Is.Not.SameAs(Enumerable.Empty<Firework>()));
		Assert.That(resultingFireworks, Is.EqualTo(Enumerable.Empty<Firework>()));
	}
}