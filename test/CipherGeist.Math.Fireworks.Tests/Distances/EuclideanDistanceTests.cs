using MathNet.Numerics;

namespace CipherGeist.Math.Fireworks.Tests.Distances;

public class EuclideanDistanceTests
{
	private readonly EuclideanDistance _euclideanDistance;
	private readonly IEnumerable<Dimension> _dimensionList;
	private const int _dimensionsCount = 4;
	private const double _lowerLimit = 0;
	private const double _upperLimit = 20.0;

	public EuclideanDistanceTests()
	{
		var range = new Interval(_lowerLimit, _upperLimit);
		_dimensionList = new List<Dimension>(_dimensionsCount);

		for (int i = 0; i < _dimensionsCount; i++)
		{
			((List<Dimension>)_dimensionList).Add(new Dimension(range));
		}

		_euclideanDistance = new EuclideanDistance(_dimensionList);
	}

	[Test]
	public void CalculateCorrectValuesCalculated()
	{
		double[] first = { 0, 0, 0 };
		double[] second = { 0, 0, 0 };
		double[] first2 = { 5, 7, 8 };
		double[] second2 = { 5, 7, 8 };
		double[] first3 = { 6, 6, 8 };

		Assert.That(_euclideanDistance.Calculate(first, second), Is.EqualTo(0).Within(5));
		Assert.That(_euclideanDistance.Calculate(first2, second2), Is.EqualTo(0).Within(5));
		Assert.IsTrue(!0.0d.AlmostEqual(_euclideanDistance.Calculate(first3, second2), 2));
	}

	[Test]
	public void CalculateCorrectValuesCalculatedFromSolutionCoordinates()
	{
		var dimensions = new List<Dimension>
		{
			new Dimension(new Interval(-1, 1)),
			new Dimension(new Interval(-1, 1))
		};
		var euclideanDistance = new EuclideanDistance(dimensions);

		var coordinatesFirst = new Dictionary<Dimension, double>
		{
			{ dimensions[0], 0.0 },
			{ dimensions[1], 0.0 }
		};
		var coordinatesSecond = new Dictionary<Dimension, double>
		{
			{ dimensions[0], 0.0 },
			{ dimensions[1], 1.0 }
		};

		Assert.That(euclideanDistance.Calculate(coordinatesFirst, coordinatesSecond), Is.EqualTo(1.0));
	}

	[Test]
	public void CalculateCorrectValuesCalculatedFromSolution()
	{
		var dimensions = new List<Dimension>
		{
			new Dimension(new Interval(-1, 1)),
			new Dimension(new Interval(-1, 1))
		};
		var euclideanDistance = new EuclideanDistance(dimensions);

		var coordinatesFirst = new Dictionary<Dimension, double>
		{
			{ dimensions[0], 0.0 },
			{ dimensions[1], 0.0 }
		};
		var coordinatesSecond = new Dictionary<Dimension, double>
		{
			{ dimensions[0], 0.0 },
			{ dimensions[1], 1.0 }
		};

		var first = new Solution(coordinatesFirst, 0.0);
		var second = new Solution(coordinatesSecond, 0.0);

		Assert.That(euclideanDistance.Calculate(first, second), Is.EqualTo(1.0));
	}

	[Test]
	public void CalculateNegaviteSecondParamElementsCountExceptionThrown()
	{
		double[] first = { 5, 7, 8, 8 };
		double[] second = { 5, 7, 8 };
		string expectedParamName = "second";

		var actualException = Assert.Throws<ArgumentException>(() => _euclideanDistance.Calculate(first, second));

		Assert.NotNull(actualException);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}
}