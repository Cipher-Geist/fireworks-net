using CipherGeist.Math.Fireworks.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CipherGeist.Math.Fireworks.Tests.Algorithm.Impl;

[TestFixture]
public class FireworksAlgorithm2012Tests
{
	private Mock<ILogger<FireworksAlgorithm2012>> _mockLogger;
	private FireworksAlgorithmSettings2012 _settings2012;
	private TimedExecutor _timedExecutor;
	private IRandomizer _randomizer;

	private const double TOLERANCE = 10E-3;
	private const int MAXIMUM_ITERATIONS = 10_000;

	[SetUp]
	public void SetUp()
	{
		_mockLogger = new Mock<ILogger<FireworksAlgorithm2012>>();
		_settings2012 = new FireworksAlgorithmSettings2012
		{
			LocationsNumber = 8,
			ExplosionSparksNumberModifier = 50.0,
			ExplosionSparksNumberLowerBound = 0.04,
			ExplosionSparksNumberUpperBound = 0.8,
			ExplosionSparksMaximumAmplitude = 40.0,
			SpecificSparksNumber = 8,
			SpecificSparksPerExplosionNumber = 1,
			FunctionOrder = 2,
			SamplingNumber = 5
		};
		_timedExecutor = new TimedExecutor();
		_randomizer = new Randomizer(RandomizerType.MersenneTwister);
	}

	public static IEnumerable<object[]> SolutionsData
	{
		get
		{
			return new[]
			{
				new object[] { nameof(Sphere2012), Sphere2012.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Cigar), Cigar.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Ellipse), Ellipse.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Griewank), Griewank.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Rastrigrin), Rastrigrin.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Sphere), Sphere.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Tablet), Tablet.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Beale), Beale.Create(), 0.01, MAXIMUM_ITERATIONS },
				new object[] { nameof(Levi), Levi.Create(), TOLERANCE, MAXIMUM_ITERATIONS },
				new object[] { nameof(Easom), Easom.Create(), TOLERANCE, MAXIMUM_ITERATIONS }
			};
		}
	}

	[TestCaseSource(nameof(SolutionsData))]
	public void FireworksAlgorithm2012DoesSolve(object testName, object problemObject, object error, int maximumIterations)
	{
		var problem = (BenchmarkProblem)problemObject;

		StepCounterStopCondition? stepCounterStopCondition = new (maximumIterations);
		problem.QualityCalculated += stepCounterStopCondition!.IncrementCounter!;

		var distanceCalculator = new EuclideanDistance(problem!.Dimensions!);
		var stopChain = ChainStopCondition
			.From(stepCounterStopCondition)
			.Or(new CoordinateProximityStopCondition(problem!.KnownSolution!, distanceCalculator, TOLERANCE));

		var fireworksAlgorithm = new FireworksAlgorithm2012(problem, stopChain, _randomizer, _settings2012, _mockLogger.Object);
		fireworksAlgorithm.OnStopConditionSatisfied += FireworksAlgorithm_OnStopConditionSatisfied!;

		var solution = _timedExecutor.Execute(() => fireworksAlgorithm.Solve());
		Assert.That(solution, Is.Not.Null);

		Console.WriteLine(
			$"Coords = ({string.Join(", ", solution!.Coordinates!.Select(kvp => kvp!.Value!.ToString()))}), " +
			$"Quality = {solution!.Quality!}");

		foreach (var kvp in solution!.Coordinates!)
		{
			Assert.That(kvp!.Value!, Is.EqualTo(problem!.KnownSolution!?.Coordinates?[kvp!.Key!]).Within((double)error));
		}
	}

	private void FireworksAlgorithm_OnStopConditionSatisfied(object sender, AlgorithmStateEventArgs e)
	{
		Console.WriteLine($"Solution = {e!.AlgorithmState!?.BestSolution:F3}, StepNumber = {e!.AlgorithmState!?.StepNumber:N0}");
	}

	private static readonly TestProblem _testProblem = InitializeTestProblem();
	private static readonly int _positiveValue = 29;

	public class TestProblem : Problem
	{
		public TestProblem(
			IList<Dimension> dimensions,
			IDictionary<Dimension, Interval> initialRanges,
			Func<IDictionary<Dimension, double>, double> targetFunction,
			ProblemTarget target)
				: base(dimensions, initialRanges, targetFunction, target)
		{
		}
	}

	public static IEnumerable<object?[]> ProblemFireworksAlgorithmData
	{
		get
		{
			var testStopCondition = new CounterStopCondition(1);
			var testFireworksAlgoritmSettings = new FireworksAlgorithmSettings2012();

			return new[]
			{
				new object?[] { null,         testStopCondition, testFireworksAlgoritmSettings, "problem" },
				new object?[] { _testProblem, null,              testFireworksAlgoritmSettings, "stopCondition" },
				new object?[] { _testProblem, testStopCondition, null,                          "settings" }
			};
		}
	}

	public static IEnumerable<object[]> FireworksAlgorithmData
	{
		get { return new[] { new object[] { _positiveValue, 2 } }; }
	}

	public static TestProblem InitializeTestProblem()
	{
		var dimensions = new List<Dimension>();
		var initialRanges = new Dictionary<Dimension, Interval>();
		var targetFunction =
			new Func<IDictionary<Dimension, double>, double>(
				(c) =>
				{
					return _positiveValue;
				});

		var target = ProblemTarget.Minimum;
		var range = new Interval(0, 1);

		dimensions.Add(new Dimension(range));
		initialRanges.Add(dimensions[0], range);

		return new TestProblem(dimensions, initialRanges, targetFunction, target);
	}

	[TestCaseSource(nameof(ProblemFireworksAlgorithmData))]
	public void ConstructorNegativeParamsArgumentNullExceptionThrown(
		Problem problem,
		IStopCondition stopCondition,
		FireworksAlgorithmSettings2012 settings,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(() =>
		{
			new FireworksAlgorithm2012(problem, stopCondition, _randomizer, settings, _mockLogger.Object);
		});

		Assert.That(actualException, Is.Not.Null);
		Assert.That(actualException!.ParamName!, Is.EqualTo(expectedParamName));
	}
}
