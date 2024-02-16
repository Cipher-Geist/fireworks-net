using CipherGeist.Math.Fireworks.Tests.Helpers;

namespace CipherGeist.Math.Fireworks.Tests.Algorithm.Impl;

[TestFixture]
public class FireworksAlgorithmTests
{
	private DynamicFireworksAlgorithmSettings _dynamicSettings;
	private TimedExecutor _timedExecutor;

	private const double ERROR = 10E-3;
	private const int MAXIMUM_ITERATIONS = 10_000;

	[SetUp]
	public void SetUp()
	{
		_dynamicSettings = new DynamicFireworksAlgorithmSettings
		{
			LocationsNumber = 8,
			ExplosionSparksNumberModifier = 50.0,
			ExplosionSparksNumberLowerBound = 0.04,
			ExplosionSparksNumberUpperBound = 0.8,
			ExplosionSparksMaximumAmplitude = 20.0,
			SpecificSparksNumber = 8,
			SpecificSparksPerExplosionNumber = 1,
			FunctionOrder = 2,
			SamplingNumber = 5,
			AmplificationCoefficent = 1.2,
			ReductionCoefficent = 0.9
		};
		_timedExecutor = new TimedExecutor();
	}

	public static IEnumerable<object[]> SolutionsData
	{
		get
		{
			return new[]
			{
				new object[] { nameof(Sphere2012), Sphere2012.Create(), ERROR, MAXIMUM_ITERATIONS },
				new object[] { $"Shifted{nameof(Sphere2012)}", Sphere2012.Create(true), ERROR, MAXIMUM_ITERATIONS },
				new object[] { nameof(Cigar), Cigar.Create(), ERROR, MAXIMUM_ITERATIONS },
				new object[] { $"Shifted{nameof(Cigar)}", Cigar.Create(true), 0.05, 100_000 },
				new object[] { nameof(Ellipse), Ellipse.Create(), ERROR, MAXIMUM_ITERATIONS },
				new object[] { nameof(Griewank), Griewank.Create(), ERROR, MAXIMUM_ITERATIONS },
				new object[] { nameof(Rastrigrin), Rastrigrin.Create(), ERROR, MAXIMUM_ITERATIONS },
				new object[] { nameof(Sphere), Sphere.Create(), ERROR, MAXIMUM_ITERATIONS },
				new object[] { nameof(Tablet), Tablet.Create(), ERROR, MAXIMUM_ITERATIONS },
				new object[] { nameof(Beale), Beale.Create(), 0.01 , MAXIMUM_ITERATIONS }
			};
		}
	}

	[TestCaseSource(nameof(SolutionsData))]
	public void DynamicFireworksAlgorithmDoesSolve(object testName, object problemObject, object error, int maximumIterations)
	{
		var problem = (BenchmarkProblem)problemObject;

		StepCounterStopCondition? stepCounterStopCondition = new (maximumIterations);
		problem.QualityCalculated += stepCounterStopCondition!.IncrementCounter!;

		var distanceCalculator = new EuclideanDistance(problem!.Dimensions!);
		var stopChain = ChainStopCondition
			.From(stepCounterStopCondition)
			.Or(new CoordinateProximityStopCondition(problem!.KnownSolution!, distanceCalculator, 1E-3));

		var fireworksAlgorithm = new DynamicFireworksAlgorithm(problem, stopChain, _dynamicSettings);
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
			var testFireworksAlgoritmSettings = new FireworksAlgorithmSettings();

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
		FireworksAlgorithmSettings settings,
		string expectedParamName)
	{
		var actualException = Assert.Throws<ArgumentNullException>(() => new FireworksAlgorithm(problem, stopCondition, settings));

		Assert.NotNull(actualException);
		Assert.That(actualException!.ParamName!, Is.EqualTo(expectedParamName));
	}
}
