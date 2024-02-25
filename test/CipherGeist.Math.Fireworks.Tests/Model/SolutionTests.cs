namespace CipherGeist.Math.Fireworks.Tests.Model;

public class SolutionTests
{
	private const double _quality1 = 1.0D;
	private const double _quality2 = 2.0D;

	public static IEnumerable<object[]> SolutionsData
	{
		get
		{
			var first = new Solution(_quality1);
			return new[] 
			{
				new object[] { first, new Solution(_quality2),										false },
				new object[] { first, new Solution(new Dictionary<Dimension, double>(), _quality1), false },
				new object[] { first, "badObject",                                                  false }
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

		Assert.That(solution1.Equals(solution2), Is.EqualTo(equal));
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

		Assert.That(solution2, Is.Not.EqualTo(solution1));
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

		Assert.That(solution2, Is.Not.EqualTo(solution1));
	}

	[TestCaseSource(nameof(SolutionsData))]
	public void EqualsSolutionsVariationsPositiveExpected(object solution, object obj, bool expected)
	{
		bool actual = solution.Equals(obj);
		Assert.That(actual, Is.EqualTo(expected));
	}

	[TestCaseSource(nameof(SolutionsData))]
	public void ComparingOperatorsSolutionsVariationsPositiveExpected(object solution, object obj, bool expected)
	{
		bool actual = solution == obj;
		bool actual2 = !(solution != obj);

		Assert.That(actual, Is.EqualTo(expected));
		Assert.That(actual2, Is.EqualTo(expected));
	}

	[TestCaseSource(nameof(SolutionsData))]
	public void GetHashCodeSolutionsVariationsPositiveExpected(object solution, object obj, bool expected)
	{
		bool actual = solution.GetHashCode() == obj.GetHashCode();
		Assert.That(actual, Is.EqualTo(expected));
	}
}