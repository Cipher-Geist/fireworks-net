using CipherGeist.Math.Fireworks.Generation;
using CipherGeist.Math.Fireworks.Model.Explosions;
using CipherGeist.Math.Fireworks.Model.Fireworks;
using CipherGeist.Math.Fireworks.Mutation;
using NUnit.Framework;
using System;

namespace CipherGeist.Math.Fireworks.Tests.Mutation
{
	public class AttractRepulseSparkMutatorTests : CipherGeist.Math.Fireworks.Tests.Generation.AbstractSourceData
	{
		[Test]
		public void CreateInstanceOfAttractRepulseSparkMutator_PassValidParameter()
		{
			ISparkGenerator<FireworkExplosion> generator = CreateAttractRepulseSparkGenerator();
			AttractRepulseSparkMutator mutator = new AttractRepulseSparkMutator(generator);
			Assert.NotNull(mutator);
		}

		[Test]
		public void CreateInstanceOfAttractRepulseSparkMutatorPassParameterAsNullArgumentNullExceptionThrown()
		{
			const string expectedParamName = "generator";
			ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new AttractRepulseSparkMutator(null));
			Assert.AreEqual(expectedParamName, exception.ParamName);
		}

		[TestCaseSource(nameof(DataForTestMethodMutateFireworkOfAttractRepulseSparkMutator))]
		public void MutateFireworkPassEachParameterAsNullAndOtherIsCorrectArgumentNullExceptionThrown(
			MutableFirework mutableFirework, FireworkExplosion explosion, String expectedParamName)
		{
			var generator = CreateAttractRepulseSparkGenerator();
			var mutator = new AttractRepulseSparkMutator(generator);

			var exception = Assert.Throws<ArgumentNullException>(() => mutator.MutateFirework(ref mutableFirework, explosion));
			Assert.AreEqual(expectedParamName, exception.ParamName);
		}

		[Test, Explicit]
		public void MutateFireworkPassValidParametersShouldChangeFireworkState()
		{
			// TODO re-write test. 
			//Interval range = new Interval(-10, 10);
			//IList<Dimension> dimensions = new List<Dimension>();
			//dimensions.Add(new Dimension(range));
			//dimensions.Add(new Dimension(range));
			//dimensions.Add(new Dimension(range));

			//var coordinatesBefore = new Dictionary<Dimension, double>();
			//var coordinatesAfter = new Dictionary<Dimension, double>();

			//foreach (Dimension dimension in dimensions)
			//{
			//	coordinatesBefore.Add(dimension, 0);
			//	coordinatesAfter.Add(dimension, 1);
			//}

			//var mutableFirework = new MutableFirework(FireworkType.SpecificSpark, 0, 0, coordinatesBefore);
			//var mutateFirework = new MutableFirework(FireworkType.SpecificSpark, 1, 0, coordinatesAfter); // Present state mutable firework after mutate

			//FireworkExplosion explosion = CreateFireworkExplosion(mutableFirework);
			//ISparkGenerator<FireworkExplosion> generator = CreateAttractRepulseSparkGenerator();
			//AttractRepulseSparkMutator mutator = new AttractRepulseSparkMutator(generator);

			//mutator.MutateFirework(ref mutableFirework, explosion);

			//Assert.NotNull(mutableFirework);
			//Assert.AreEqual(mutableFirework.BirthStepNumber, mutateFirework.BirthStepNumber);
			//Assert.AreEqual(mutableFirework.Quality, mutateFirework.Quality);
			//double dimensionValueBefore;
			//double dimensionValueAfter;
			//foreach (Dimension dimension in dimensions)
			//{
			//	mutableFirework.Coordinates.TryGetValue(dimension, out dimensionValueBefore);
			//	mutateFirework.Coordinates.TryGetValue(dimension, out dimensionValueAfter);
			//	Assert.AreEqual(dimensionValueBefore, dimensionValueAfter);
			//}
		}
	}
}