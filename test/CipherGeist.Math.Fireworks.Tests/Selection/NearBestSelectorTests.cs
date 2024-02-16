using Moq;

namespace CipherGeist.Math.Fireworks.Tests.Selection;

public class NearBestSelectorTests
{
	private readonly int _samplingNumber;
	private readonly int _countFireworks;
	private readonly Func<IEnumerable<Firework>, Firework> _getBest;
	private readonly Firework _bestFirework;
	private readonly List<Firework> _allFireworks;
	private readonly Mock<IDistance> _distanceCalculator;
	private readonly NearBestFireworkSelector _nearBestSelector;

	public NearBestSelectorTests()
	{
		_samplingNumber = SelectorTestsHelper.SamplingNumber;
		_countFireworks = SelectorTestsHelper.CountFireworks;
		_getBest = SelectorTestsHelper.GetBest;

		_bestFirework = SelectorTestsHelper.FirstBestFirework;
		_allFireworks = new List<Firework>(SelectorTestsHelper.Fireworks);
		_distanceCalculator = new Mock<IDistance>();

		for (int i = 1; i < 10; i++)
		{
			_distanceCalculator
				.Setup(m => m.Calculate(_bestFirework, _allFireworks[i]))
				.Returns(i);
		}

		_nearBestSelector = new NearBestFireworkSelector(_distanceCalculator.Object, _getBest, _samplingNumber);
	}

	[Test]
	public void SelectFireworksPresentAllParamReturnsEqualFireworks()
	{
		var expectedFireworks = SelectorTestsHelper.NearBestFireworks;
		var resultingFireworks = _nearBestSelector.SelectFireworks(_allFireworks, _samplingNumber);

		Assert.AreNotSame(expectedFireworks, resultingFireworks);
		Assert.AreEqual(expectedFireworks, resultingFireworks);
	}

	[Test]
	public void SelectFireworksPresentAllParamReturnsNonEqualFireworks()
	{
		var expectedFireworks = SelectorTestsHelper.NonNearBestFirework;
		var resultingFireworks = _nearBestSelector.SelectFireworks(_allFireworks, _samplingNumber);

		Assert.AreNotSame(expectedFireworks, resultingFireworks);
		Assert.AreNotEqual(expectedFireworks, resultingFireworks);
	}

	[Test]
	public void SelectFireworksNullAs1stParamExceptionThrown()
	{
		string expectedParamName = "from";
		IEnumerable<Firework> currentFireworks = null;

		ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() =>
		_nearBestSelector.SelectFireworks(currentFireworks, _samplingNumber));

		Assert.NotNull(actualException);
		Assert.AreEqual(expectedParamName, actualException.ParamName);
	}

	[Test]
	public void SelectFireworksNegativeNumberAs2ndParamExceptionThrown()
	{
		string expectedParamName = "numberToSelect";
		int samplingNumber = -1;

		ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		_nearBestSelector.SelectFireworks(_allFireworks, samplingNumber));

		Assert.NotNull(actualException);
		Assert.AreEqual(expectedParamName, actualException.ParamName);
	}

	[Test]
	public void SelectFireworksGreatNumberAs2ndParamExceptionThrown()
	{
		string expectedParamName = "numberToSelect";
		int samplingNumber = _countFireworks + 1;

		ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
		_nearBestSelector.SelectFireworks(_allFireworks, samplingNumber));

		Assert.NotNull(actualException);
		Assert.AreEqual(expectedParamName, actualException.ParamName);
	}

	[Test]
	public void SelectFireworksCountFireworksEqual2ndParamReturnsEqualFireworks()
	{
		IEnumerable<Firework> expectedFireworks = _allFireworks;
		int samplingNumber = _countFireworks;

		IEnumerable<Firework> resultingFireworks = _nearBestSelector.SelectFireworks(_allFireworks, samplingNumber);

		Assert.AreNotSame(expectedFireworks, resultingFireworks);
		Assert.AreEqual(expectedFireworks, resultingFireworks);
	}

	[Test]
	public void SelectFireworksNullAs2ndParamReturnsEmptyCollectionFireworks()
	{
		IEnumerable<Firework> expectedFireworks = new List<Firework>();
		int samplingNumber = 0;

		IEnumerable<Firework> resultingFireworks = _nearBestSelector.SelectFireworks(_allFireworks, samplingNumber);

		Assert.AreNotSame(expectedFireworks, resultingFireworks);
		Assert.AreEqual(expectedFireworks, resultingFireworks);
	}
}