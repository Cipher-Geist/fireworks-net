using CipherGeist.Math.Fireworks.Mutation;

namespace CipherGeist.Math.Fireworks.Tests.Mutation
{
	public class AttractRepulseSparkMutatorTests : CipherGeist.Math.Fireworks.Tests.Generation.AbstractSourceData
	{
		[Test]
		public void CreateInstanceOfAttractRepulseSparkMutator_PassValidParameter()
		{
			var generator = CreateAttractRepulseSparkGenerator();
			var mutator = new AttractRepulseSparkMutator(generator);
			Assert.That(mutator, Is.Not.Null);
		}

		[TestCaseSource(nameof(DataForTestMethodMutateFireworkOfAttractRepulseSparkMutator))]
		public void MutateFireworkPassEachParameterAsNullAndOtherIsCorrectArgumentNullExceptionThrown(
			MutableFirework mutableFirework, FireworkExplosion explosion, string expectedParamName)
		{
			var generator = CreateAttractRepulseSparkGenerator();
			var mutator = new AttractRepulseSparkMutator(generator);

			var exception = Assert.Throws<ArgumentNullException>(() => mutator.MutateFirework(ref mutableFirework, explosion));
			Assert.That(exception.ParamName, Is.EqualTo(expectedParamName));
		}

		[Test, Explicit]
		public void MutateFireworkPassValidParametersShouldChangeFireworkState()
		{
			// TODO re-write test. 
			var range = new Interval(-10, 10);
			var dimensions = new List<Dimension>();

			dimensions.Add(new Dimension(range));
			dimensions.Add(new Dimension(range));
			dimensions.Add(new Dimension(range));

			var coordinatesBefore = new Dictionary<Dimension, double>();
			var coordinatesAfter = new Dictionary<Dimension, double>();

			foreach (Dimension dimension in dimensions)
			{
				coordinatesBefore.Add(dimension, 0);
				coordinatesAfter.Add(dimension, 1);
			}

			var mutableFirework = new MutableFirework(FireworkType.SpecificSpark, 0, 0, coordinatesBefore);
			var mutateFirework = new MutableFirework(FireworkType.SpecificSpark, 1, 0, coordinatesAfter); // Present state mutable firework after mutate

			var explosion = CreateFireworkExplosion(mutableFirework);
			var generator = CreateAttractRepulseSparkGenerator();
			var mutator = new AttractRepulseSparkMutator(generator);

			mutator.MutateFirework(ref mutableFirework, explosion);

			Assert.That(mutableFirework, Is.Not.Null);
			Assert.Multiple(() =>
			{
				Assert.That(mutateFirework.BirthStepNumber, Is.EqualTo(mutableFirework.BirthStepNumber));
				Assert.That(mutateFirework.Quality, Is.EqualTo(mutableFirework.Quality));
			});

			double dimensionValueBefore;
			double dimensionValueAfter;
			foreach (var dimension in dimensions)
			{
				mutableFirework.Coordinates.TryGetValue(dimension, out dimensionValueBefore);
				mutateFirework.Coordinates.TryGetValue(dimension, out dimensionValueAfter);
				Assert.That(dimensionValueAfter, Is.EqualTo(dimensionValueBefore));
			}
		}
	}
}