using CipherGeist.Math.Fireworks.Model.Fireworks;
using CipherGeist.Math.Fireworks.Selection;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CipherGeist.Math.Fireworks.Tests.Selection
{
	public class BestAndRandomFireworkSelectorTests
	{
		#region TestData
		private static readonly Func<IEnumerable<Firework>, Firework> _getBest = SelectorTestsHelper.GetBest;
		private static readonly int _samplingNumber = SelectorTestsHelper.SamplingNumber;
		private static readonly System.Random _randomizer = new System.Random();

		public static IEnumerable<object[]> ProblemData
		{
			get
			{

				Func<IEnumerable<Firework>, Firework> best = _getBest;
				int samplingNumberParam = _samplingNumber;
				System.Random randomizerParam = _randomizer;

				return new[]
				{
					new object[] { null,           best,  samplingNumberParam , "randomizer"},
					new object[] {randomizerParam, null,  samplingNumberParam , "bestFireworkSelector"}
				};
			}
		}
		public static IEnumerable<object[]> ProblemData2
		{
			get
			{
				Func<IEnumerable<Firework>, Firework> best = _getBest;
				int samplingNumberParam = -1;
				System.Random randomizerParam = _randomizer;

				return new[]
				{
					new object[] { randomizerParam,best,  samplingNumberParam , "locationsNumber"}
				};
			}
		}
		#endregion

		[TestCaseSource(nameof(ProblemData))]
		public void BestAndRandomFireworkSelectorNegativeParamsArgumentNullExceptionThrown(System.Random randomizer,
			Func<IEnumerable<Firework>, Firework> bestFireworkSelector,
			int locationsNumber,
			string expectedParamName)
		{
			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new BestAndRandomFireworkSelector(randomizer, bestFireworkSelector, locationsNumber));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[TestCaseSource(nameof(ProblemData2))]
		public void BestAndRandomFireworkSelectorNegative3rdParamsArgumentOutOfRangeExceptionThrown(System.Random randomizer,
			Func<IEnumerable<Firework>, Firework> bestFireworkSelector,
			int locationsNumber,
			string expectedParamName)
		{
			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new BestAndRandomFireworkSelector(randomizer, bestFireworkSelector, locationsNumber));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}
	}
}