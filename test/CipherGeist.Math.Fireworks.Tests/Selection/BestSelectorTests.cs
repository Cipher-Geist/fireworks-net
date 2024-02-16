using CipherGeist.Math.Fireworks.Model.Fireworks;
using CipherGeist.Math.Fireworks.Selection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherGeist.Math.Fireworks.Tests.Selection
{
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
			IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.BestFireworks;

			IEnumerable<Firework> resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

			Assert.AreNotSame(expectedFireworks, resultingFireworks);
			Assert.AreEqual(expectedFireworks, resultingFireworks);
		}

		[Test]
		public void SelectFireworksPresentAllParamReturnsNonEqualFireworks()
		{
			IEnumerable<Firework> expectedFireworks = SelectorTestsHelper.NonBestFireworks;

			IEnumerable<Firework> resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

			Assert.AreNotSame(expectedFireworks, resultingFireworks);
			Assert.AreNotEqual(expectedFireworks, resultingFireworks);
		}

		[Test]
		public void SelectFireworksNullAs1stParamExceptionThrown()
		{
			string expectedParamName = "from";
			IEnumerable<Firework> currentFireworks = null;

			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => bestSelector.SelectFireworks(currentFireworks));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[Test]
		public void SelectFireworksNegativeNumberAs2ndParamExceptionThrown()
		{
			string expectedParamName = "numberToSelect";
			int samplingNumber = -1;

			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => bestSelector.SelectFireworks(allFireworks, samplingNumber));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[Test]
		public void SelectFireworksAsManyAspossibleWithoutException()
		{
			int samplingNumber = countFireworks + 1;
			var sampledFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);
			Assert.AreEqual(countFireworks, sampledFireworks.Count());
		}

		[Test]
		public void SelectFireworksCountFireworksEqual2ndParamReturnsEqualFireworks()
		{
			IEnumerable<Firework> expectedFireworks = allFireworks;
			int samplingNumber = countFireworks;

			IEnumerable<Firework> resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

			Assert.AreNotSame(expectedFireworks, resultingFireworks);
			Assert.AreEqual(expectedFireworks, resultingFireworks);
		}

		[Test]
		public void SelectFireworksNullAs2ndParamReturnsEmptyCollectionFireworks()
		{
			IEnumerable<Firework> expectedFireworks = new List<Firework>();
			int samplingNumber = 0;

			IEnumerable<Firework> resultingFireworks = bestSelector.SelectFireworks(allFireworks, samplingNumber);

			Assert.AreNotSame(expectedFireworks, resultingFireworks);
			Assert.AreEqual(expectedFireworks, resultingFireworks);
		}
	}
}