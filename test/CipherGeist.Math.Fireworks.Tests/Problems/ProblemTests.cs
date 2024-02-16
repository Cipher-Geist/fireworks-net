using CipherGeist.Math.Fireworks.Model;
using CipherGeist.Math.Fireworks.Problems;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CipherGeist.Math.Fireworks.Tests.Problems
{
	public class ProblemTests
	{
		#region Test Data. 
		public static TestProblem GetTestProblem()
		{
			var dimensions = new List<Dimension>();
			var initialRanges = new Dictionary<Dimension, Interval>();
			var targetFunction =
				new Func<IDictionary<Dimension, double>, double>(
					(c) =>
					{
						return 29;
					}
				);
			ProblemTarget target = ProblemTarget.Minimum;

			var range = new Interval(0, 1);
			dimensions.Add(new Dimension(range));
			initialRanges.Add(dimensions[0], range);

			var testProblem = new TestProblem(dimensions, initialRanges, targetFunction, target);

			return testProblem;
		}

		public static IEnumerable<object[]> ProblemDataOfArgumentNullExceptionTrown
		{
			get
			{
				IList<Dimension> dimensions = new List<Dimension>();
				IDictionary<Dimension, Interval> initialRanges = new Dictionary<Dimension, Interval>();
				Func<IDictionary<Dimension, double>, double> targetFunction =
					new Func<IDictionary<Dimension, double>, double>(
						(c) =>
						{
							return 29;
						}
					);
				ProblemTarget target = ProblemTarget.Minimum;

				Interval range = new Interval(0, 1);
				dimensions.Add(new Dimension(range));
				initialRanges.Add(new KeyValuePair<Dimension, Interval>(dimensions[0], range));

				return new[]
				{
					new object[] { null,       initialRanges, targetFunction, target, "dimensions"},
					new object[] { dimensions, null,          targetFunction, target, "initialDimensionRanges"},
					new object[] { dimensions, initialRanges, null,           target, "targetFunction"}
				};
			}
		}

		public static IEnumerable<object[]> ProblemDataOfArgumentExceptionTrown
		{
			get
			{
				IList<Dimension> dimensions = new List<Dimension>();
				IDictionary<Dimension, Interval> initialRanges = new Dictionary<Dimension, Interval>();
				Func<IDictionary<Dimension, double>, double> targetFunction =
					new Func<IDictionary<Dimension, double>, double>(
						(c) =>
						{
							return 29;
						}
					);
				ProblemTarget target = ProblemTarget.Minimum;

				Interval range = new Interval(0, 1);
				dimensions.Add(new Dimension(range));
				initialRanges.Add(new KeyValuePair<Dimension, Interval>(dimensions[0], range));

				IDictionary<Dimension, Interval> badRanges = new Dictionary<Dimension, Interval>();

				Interval range2 = new Interval(2, 3);
				badRanges.Add(new KeyValuePair<Dimension, Interval>(new Dimension(range2), range2));

				return new[] {
					new object[] { new List<Dimension>(), initialRanges,                         targetFunction, target, "dimensions"},
					new object[] { dimensions,            new Dictionary<Dimension, Interval>(), targetFunction, target, "initialRanges"},
					new object[] { dimensions,            badRanges,                             targetFunction, target, "initialRanges"}
				};
			}
		}
		#endregion // Test Data.

		public class TestProblem : Problem
		{
			public TestProblem(IList<Dimension> dimensions, IDictionary<Dimension, Interval> initialRanges, Func<IDictionary<Dimension, double>, double> targetFunction, ProblemTarget target)
				: base(dimensions, initialRanges, targetFunction, target)
			{
			}
		}

		[TestCaseSource(nameof(ProblemDataOfArgumentNullExceptionTrown))]
		public void ProblemNegativeParamsArgumentNullExceptionThrown(IList<Dimension> dimensions,
			IDictionary<Dimension,
			Interval> initialRanges,
			Func<IDictionary<Dimension, double>, double> targetFunction,
			ProblemTarget target,
			string expectedParamName)
		{
			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() =>
			new TestProblem(dimensions, initialRanges, targetFunction, target));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[TestCase(null, "coordinateValues")]
		public void CalculateQualityNegativeParamsArgumentNullExceptionTrown(
			IDictionary<Dimension, double> coordinateValues, string expectedParamName)
		{
			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() =>
			GetTestProblem().CalculateQuality(coordinateValues));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[TestCase(null, "dimensions")]
		public void CreateDefaultInitialRangesNegativeParamsArgumentNullExceptionTrown(IList<Dimension> dimensions, string expectedParamName)
		{
			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() =>
			Problem.CreateDefaultInitialRanges(dimensions));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}
	}
}