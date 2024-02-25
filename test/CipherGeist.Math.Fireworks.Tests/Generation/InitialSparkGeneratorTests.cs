using Moq;

namespace CipherGeist.Math.Fireworks.Tests.Generation;

[TestFixture]
public class InitialSparkGeneratorTests
{
	private InitialSparkGenerator _sparkGenerator;
	private Mock<IRandomizer> _randomizerMock;

	[SetUp]
	public void SetUp()
	{
		var dimensions = new List<Dimension>();
		var initialRanges = new Dictionary<Dimension, Interval>
		{
			{ new Dimension(new Interval(0, 10)), new Interval(0, 10) },
			{ new Dimension(new Interval(0, 20)), new Interval(0, 20) },
			{ new Dimension(new Interval(0, 30)), new Interval(0, 30) }
		};
		_randomizerMock = new Mock<IRandomizer>();
		_sparkGenerator = new InitialSparkGenerator(dimensions, initialRanges, _randomizerMock.Object);
	}

	public static IEnumerable<object?[]> ProblemData
	{
		get
		{
			var initialRanges = new Dictionary<Dimension, Interval>();
			var dimensions = new List<Dimension>();
			var randomizer = new Randomizer();

			return new[]
			{
				new object?[] { null,       initialRanges, randomizer, "dimensions" },
				new object?[] { dimensions, null,          randomizer, "initialRanges" },
				new object?[] { dimensions, initialRanges, null,       "randomizer" }
			};
		}
	}

	public static IEnumerable<object?[]> ProblemData2
	{
		get
		{
			var dimensions = new List<Dimension>();
			var randomizer = new Randomizer();

			return new[]
			{
				new object?[] { null,       randomizer, "dimensions" },
				new object?[] { dimensions, null,       "randomizer" }
			};
		}
	}

	[TestCaseSource(nameof(ProblemData))]
	public void InitialSparkGeneratorNegativeParamsArgumentNullExceptionThrown(
		IEnumerable<Dimension> dimensions,
		IDictionary<Dimension, Interval> initialRandes,
		IRandomizer randomizer,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(() =>
		{
			new InitialSparkGenerator(dimensions, initialRandes, randomizer);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
	}

	[Test]
	public void CreateSpark_ValidExplosionAndBirthOrder_ReturnsSpark()
	{
		var explosion = new InitialExplosion(0, 0);
		var birthOrder = 1;

		var spark = _sparkGenerator.CreateSpark(explosion, birthOrder);

		Assert.That(spark.FireworkType, Is.EqualTo(FireworkType.Initial));
		Assert.That(spark.BirthOrder, Is.EqualTo(birthOrder));
	}

	[Test]
	public void CreateSpark_NegativeBirthOrder_ThrowsArgumentOutOfRangeException()
	{
		var explosion = new InitialExplosion(0, 0);
		var birthOrder = -1;
		Assert.Throws<ArgumentOutOfRangeException>(() => _sparkGenerator.CreateSpark(explosion, birthOrder));
	}
}