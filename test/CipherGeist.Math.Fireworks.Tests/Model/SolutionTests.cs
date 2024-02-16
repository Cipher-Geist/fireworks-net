using CipherGeist.Math.Fireworks.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace CipherGeist.Math.Fireworks.Tests.Model
{
	public class SolutionTests
	{
		private const double quality1 = 1.0D;
		private const double quality2 = 2.0D;

		public static IEnumerable<object[]> SolutionsData
		{
			get
			{
				var first = new Solution(quality1);
				return new[] {
					new object[] { first, new Solution(null, quality2),                                false },
					new object[] { first, new Solution(new Dictionary<Dimension, double>(), quality1), false },
					new object[] { first, "badObject",                                                 false }
				};
			}
		}

		[TestCase(99.99, 99.99, true)]
		[TestCase(99.99, 00.99, false)]
		public void EqualQualityShouldWithIdenticalCoordinatesShouldReturnEqual(double quality1, double quality2, bool equal)
		{
			var dim1 = new Dimension(new Interval(-1.0, 1.0));
			var dim2 = new Dimension(new Interval(-2.0, 2.0));
			var dictionary = new Dictionary<Dimension, double>
			{
				{ dim1, 0.0 },
				{ dim2, 0.0 }
			};

			var solution1 = new Solution(dictionary, quality1);
			var solution2 = new Solution(dictionary, quality2);

			Assert.AreEqual(equal, solution1.Equals(solution2));
		}

		[Test]
		public void EqualCoordinatesAndDifferentQualityShouldReturnNotEqual()
		{
			var dim1 = new Dimension(new Interval(1, 1));
			var dim2 = new Dimension(new Interval(2, 2));

			var dictionary1 = new Dictionary<Dimension, double>
			{
				{ dim1, 0.0 },
				{ dim2, 0.0 }
			};
			var dictionary2 = new Dictionary<Dimension, double>
			{
				{ dim1, 0.0 },
				{ dim2, 0.0 }
			};

			var solution1 = new Solution(dictionary1, 0);
			var solution2 = new Solution(dictionary2, 1);

			Assert.AreNotEqual(solution1, solution2);
		}

		[Test]
		public void DifferentCoordinatesAndEqualQualityShouldReturnEqual()
		{
			var dim1 = new Dimension(new Interval(1, 1));
			var dim2 = new Dimension(new Interval(2, 2));

			var dim3 = new Dimension(new Interval(1, 1));
			var dim4 = new Dimension(new Interval(2, 2));

			var dictionary1 = new Dictionary<Dimension, double>
			{
				{ dim1, 0.0 },
				{ dim2, 0.0 }
			};
			var dictionary2 = new Dictionary<Dimension, double>
			{
				{ dim3, 0.0 },
				{ dim4, 0.0 }
			};

			var solution1 = new Solution(dictionary1, 0);
			var solution2 = new Solution(dictionary2, 0);

			Assert.AreNotEqual(solution1, solution2);
		}

		[TestCaseSource(nameof(SolutionsData))]
		public void EqualsSolutionsVariationsPositiveExpected(object solution, object obj, bool expected)
		{
			bool actual = solution.Equals(obj);
			Assert.AreEqual(expected, actual);
		}

		[TestCaseSource(nameof(SolutionsData))]
		public void ComparingOperatorsSolutionsVariationsPositiveExpected(object solution, object obj, bool expected)
		{
			bool actual = (solution == obj);
			bool actual2 = !(solution != obj);

			Assert.AreEqual(expected, actual);
			Assert.AreEqual(expected, actual2);
		}

		[TestCaseSource(nameof(SolutionsData))]
		public void GetHashCodeSolutionsVariationsPositiveExpected(object solution, object obj, bool expected)
		{
			bool actual = (solution.GetHashCode() == obj.GetHashCode());
			Assert.AreEqual(expected, actual);
		}
	}
}