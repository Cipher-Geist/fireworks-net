

namespace CipherGeist.Math.Fireworks.Test.Problems
{
	[TestFixture]
	public class FireworksAlgorithmTests
	{
		private FireworksAlgorithmSettings _settings;
		private FireworksAlgorithmSettings2012 _settings2012;

		private DynamicFireworksAlgorithmSettings _dynamicSettings;

		private TimedExecutor _timedExecutor;

		private const double ERROR = 10E-3;
		private const int MAXIMUM_ITERATIONS = 10_000;

		[SetUp]
		public void SetUp()
		{
			_settings = new FireworksAlgorithmSettings
			{
				LocationsNumber = 8,
				ExplosionSparksNumberModifier = 50.0,
				ExplosionSparksNumberLowerBound = 0.04,
				ExplosionSparksNumberUpperBound = 0.8,
				ExplosionSparksMaximumAmplitude = 40.0,
				SpecificSparksNumber = 8,
				SpecificSparksPerExplosionNumber = 1
			};
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
			_enhancedSettings = new EnhancedFireworksAlgorithmSettings
			{
				LocationsNumber = 8,
				ExplosionSparksNumberModifier = 50.0,
				ExplosionSparksNumberLowerBound = 0.04,
				ExplosionSparksNumberUpperBound = 0.8,
				ExplosionSparksMaximumAmplitude = 20.0,
				ExplosionSparksInitialAmplitudeFactor = 0.02,
				ExplosionSparksFinalAmplitudeFactor = 0.001,
				MaximumIterationCount = MAXIMUM_ITERATIONS,
				SpecificSparksNumber = 8,
				SpecificSparksPerExplosionNumber = 1,
				FunctionOrder = 2,
				SamplingNumber = 10
			};
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
		public void EnhancedFireworksAlgorithmDoesSolve(object testName, object problemObject, object error, int maximumIterations)
		{
			var problem = (BenchmarkProblem)problemObject;

			var stepCounterStopCondition = new StepCounterStopCondition(maximumIterations);
			problem.QualityCalculated += stepCounterStopCondition.IncrementCounter;

			var distanceCalculator = new EuclideanDistance(problem.Dimensions);
			var stopChain = ChainStopCondition
				.From(stepCounterStopCondition)
				.Or(new CoordinateProximityStopCondition(problem.KnownSolution, distanceCalculator, 1E-3));

			var fireworksAlgorithm = new EnhancedFireworksAlgorithm(problem, stopChain, _enhancedSettings);
			fireworksAlgorithm.OnStopConditionSatisfied += FireworksAlgorithm_OnStopConditionSatisfied;

			var solution = _timedExecutor.Execute(() => fireworksAlgorithm.Solve());
			Console.WriteLine($"Coords = ({String.Join(", ", solution.Coordinates.Select(kvp => kvp.Value.ToString()))}), Quality = {solution.Quality}");

			foreach (var kvp in solution.Coordinates)
				Assert.AreEqual(problem.KnownSolution.Coordinates[kvp.Key], kvp.Value, (double)error);
		}

		[TestCaseSource(nameof(SolutionsData))]
		public void DynamicFireworksAlgorithmDoesSolve(object testName, object problemObject, object error, int maximumIterations)
		{
			var problem = (BenchmarkProblem)problemObject;

			var stepCounterStopCondition = new StepCounterStopCondition(maximumIterations);
			problem.QualityCalculated += stepCounterStopCondition.IncrementCounter;

			var distanceCalculator = new EuclideanDistance(problem.Dimensions);
			var stopChain = ChainStopCondition
				.From(stepCounterStopCondition)
				.Or(new CoordinateProximityStopCondition(problem.KnownSolution, distanceCalculator, 1E-3));

			var fireworksAlgorithm = new DynamicFireworksAlgorithm(problem, stopChain, _dynamicSettings);
			fireworksAlgorithm.OnStopConditionSatisfied += FireworksAlgorithm_OnStopConditionSatisfied;

			var solution = _timedExecutor.Execute(() => fireworksAlgorithm.Solve());
			Console.WriteLine($"Coords = ({String.Join(", ", solution.Coordinates.Select(kvp => kvp.Value.ToString()))}), Quality = {solution.Quality}");

			foreach (var kvp in solution.Coordinates)
				Assert.AreEqual(problem.KnownSolution.Coordinates[kvp.Key], kvp.Value, (double)error);
		}

		private void FireworksAlgorithm_OnStopConditionSatisfied(object sender, AlgorithmStateEventArgs e)
		{
			Console.WriteLine($"Solution = {e.AlgorithmState.BestSolution:F3}, StepNumber = {e.AlgorithmState.StepNumber:N0}");
		}

		private static readonly TestProblem testProblem = InitializeTestProblem();
		private static readonly int positiveValue = 29;

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

		public static IEnumerable<object[]> ProblemFireworksAlgorithmData
		{
			get
			{
				var testStopCondition = new CounterStopCondition(1);
				var testFireworksAlgoritmSettings = new FireworksAlgorithmSettings();

				return new[] {
					new object[] { null,        testStopCondition, testFireworksAlgoritmSettings, "problem"},
					new object[] { testProblem, null,              testFireworksAlgoritmSettings, "stopCondition"},
					new object[] { testProblem, testStopCondition, null,                          "settings"}
				};
			}
		}

		public static IEnumerable<object[]> FireworksAlgorithmData
		{
			get { return new[] { new object[] { positiveValue, 2 } }; }
		}

		public static TestProblem InitializeTestProblem()
		{
			// Init test problem
			var dimensions = new List<Dimension>();
			IDictionary<Dimension, Interval> initialRanges = new Dictionary<Dimension, Interval>();
			Func<IDictionary<Dimension, double>, double> targetFunction =
				new Func<IDictionary<Dimension, double>, double>(
					(c) =>
					{
						return positiveValue;
					}
				);

			var target = ProblemTarget.Minimum;

			var range = new Interval(0, 1);
			dimensions.Add(new Dimension(range));
			initialRanges.Add(new KeyValuePair<Dimension, Interval>(dimensions[0], range));

			return new TestProblem(dimensions, initialRanges, targetFunction, target);
		}

		private FireworksAlgorithm GetFireworksAlgorithm()
		{
			var testStopCondition = new CounterStopCondition(1);
			testProblem.QualityCalculated += testStopCondition.IncrementCounter;

			var testFireworksAlgoritmSetting = new FireworksAlgorithmSettings
			{
				LocationsNumber = 1,
				ExplosionSparksNumberModifier = 1,
				ExplosionSparksNumberLowerBound = 0.04,
				ExplosionSparksNumberUpperBound = 0.8,
				ExplosionSparksMaximumAmplitude = 0.5,
				SpecificSparksNumber = 1,
				SpecificSparksPerExplosionNumber = 1
			};

			return new FireworksAlgorithm(testProblem, testStopCondition, testFireworksAlgoritmSetting);
		}

		[TestCaseSource(nameof(ProblemFireworksAlgorithmData))]
		public void ConstructorNegativeParamsArgumentNullExceptionThrown(
			Problem problem,
			IStopCondition stopCondition,
			FireworksAlgorithmSettings settings,
			string expectedParamName)
		{
			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() =>
				new FireworksAlgorithm(problem, stopCondition, settings));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[TestCaseSource(nameof(FireworksAlgorithmData))]
		public void SolveCalculationPositiveExpected(double expectedValue, int precision)
		{
			var fireworksAlgorithm = GetFireworksAlgorithm();
			double value = fireworksAlgorithm.Solve().Quality;

			Assert.AreEqual(expectedValue, value, precision);

			testProblem.QualityCalculated -= ((CounterStopCondition)fireworksAlgorithm.StopCondition).IncrementCounter;
		}
	}
}
