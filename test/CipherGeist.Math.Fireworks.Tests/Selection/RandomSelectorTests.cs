using CipherGeist.Math.Fireworks.Model.Fireworks;
using CipherGeist.Math.Fireworks.Selection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherGeist.Math.Fireworks.Tests.Selection
{
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
			foreach (Firework firework in resultingFireworks)
				Assert.That(_allFireworks, Does.Contain(firework));
		}

		[Test]
		public void SelectFireworksPresentAllParamReturnsNonEqualCollections()
		{
			var firstResultingFireworks = _randomSelector.SelectFireworks(_allFireworks, _samplingNumber);
			var secondResultingFireworks = _randomSelector.SelectFireworks(_allFireworks, _samplingNumber);

			Assert.AreNotSame(firstResultingFireworks, secondResultingFireworks);
			Assert.AreNotEqual(firstResultingFireworks, secondResultingFireworks);
		}

		[Test]
		public void SelectFireworksNullAs1stParamExceptionThrown()
		{
			string expectedParamName = "from";
			IEnumerable<Firework> currentFireworks = null;

			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() =>
				_randomSelector.SelectFireworks(currentFireworks, _samplingNumber));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[Test]
		public void SelectFireworksNegativeNumberAs2ndParamExceptionThrown()
		{
			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
				_randomSelector.SelectFireworks(_allFireworks, -1));

			Assert.NotNull(actualException);
			Assert.AreEqual("numberToSelect", actualException.ParamName);
		}

		[Test]
		public void SelectFireworksGreatNumberAs2ndParamExceptionThrown()
		{
			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
				_randomSelector.SelectFireworks(_allFireworks, _countFireworks + 1));

			Assert.NotNull(actualException);
			Assert.AreEqual("numberToSelect", actualException.ParamName);
		}

		[Test]
		public void SelectFireworksCountFireworksEqual2ndParamReturnsEqualFireworks()
		{
			IEnumerable<Firework> resultingFireworks = _randomSelector.SelectFireworks(_allFireworks, _countFireworks);

			Assert.AreNotSame(_allFireworks, resultingFireworks);
			Assert.AreEqual(_allFireworks, resultingFireworks);
		}

		[Test]
		public void SelectFireworksZeroAs2ndParamReturnsEmptyCollectionFireworks()
		{
			IEnumerable<Firework> resultingFireworks = _randomSelector.SelectFireworks(_allFireworks, 0);

			Assert.AreNotSame(Enumerable.Empty<Firework>(), resultingFireworks);
			Assert.AreEqual(Enumerable.Empty<Firework>(), resultingFireworks);
		}
	}
}