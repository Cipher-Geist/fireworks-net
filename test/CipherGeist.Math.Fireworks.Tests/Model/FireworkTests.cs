using CipherGeist.Math.Fireworks.Model;
using CipherGeist.Math.Fireworks.Model.Fireworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CipherGeist.Math.Fireworks.Tests.Model
{
	public class FireworkTests
	{
		#region Constructor tests

		[Test]
		public void ConstructorNegativeBirthStepNumberExceptionThrown()
		{
			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, -1, 0, new Dictionary<Dimension, double>()));

			Assert.NotNull(actualException);
			Assert.AreEqual("birthStepNumber", actualException.ParamName);
		}

		[Test]
		public void ConstructorNegativeBirthOrderExceptionThrown()
		{
			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, 0, -1, new Dictionary<Dimension, double>()));

			Assert.NotNull(actualException);
			Assert.AreEqual("birthOrder", actualException.ParamName);
		}

		[Test]
		public void ConstructorNullCoordinatesExceptionThrown()
		{
			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => new Firework(FireworkType.Initial, 0, 0, coordinates: null));

			Assert.NotNull(actualException);
			Assert.AreEqual("coordinates", actualException.ParamName);
		}

		[Test]
		public void ConstructorValidFireworkTypeSetsPassedFireworkType()
		{
			FireworkType expectedType = FireworkType.Initial;

			Firework result = new Firework(expectedType, 0, 0, new Dictionary<Dimension, double>());

			Assert.AreEqual(expectedType, result.FireworkType);
		}

		[Test]
		public void ConstructorValidBirthStepNumberSetsPassedBirthStepNumber()
		{
			int expectedBirthStepNumber = 10;

			Firework result = new Firework(FireworkType.Initial, expectedBirthStepNumber, 0, new Dictionary<Dimension, double>());

			Assert.AreEqual(expectedBirthStepNumber, result.BirthStepNumber);
		}

		[Test]
		public void ConstructorValidBirthOrderSetsPassedBirthOrder()
		{
			int expectedBirthOrder = 2;

			Firework result = new Firework(FireworkType.Initial, 0, expectedBirthOrder, new Dictionary<Dimension, double>());

			Assert.AreEqual(expectedBirthOrder, result.BirthOrder);
		}

		[Test]
		public void ConstructorValidCoordinatesSetsPassedCoordinates()
		{
			Dictionary<Dimension, double> expectedCoordinates = new Dictionary<Dimension, double>
			{
				{ new Dimension(new Interval(-10.0, 20.0)), 10.0 },
				{ new Dimension(new Interval(50.0, 120.0)), 67.85 }
			};

			Firework result = new Firework(FireworkType.Initial, 0, 0, expectedCoordinates);

			Assert.AreEqual(expectedCoordinates, result.Coordinates);
		}

		#endregion

		#region Constructor2 tests

		[Test]
		public void Constructor2NegativeBirthStepNumberExceptionThrown()
		{
			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, -1, 0));

			Assert.NotNull(actualException);
			Assert.AreEqual("birthStepNumber", actualException.ParamName);
		}

		[Test]
		public void Constructor2NegativeBirthOrderExceptionThrown()
		{
			ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, 0, -1));

			Assert.NotNull(actualException);
			Assert.AreEqual("birthOrder", actualException.ParamName);
		}

		[Test]
		public void Constructor2ValidFireworkTypeSetsPassedFireworkType()
		{
			FireworkType expectedType = FireworkType.Initial;

			Firework result = new Firework(expectedType, 0, 0);

			Assert.AreEqual(expectedType, result.FireworkType);
		}

		[Test]
		public void Constructor2ValidBirthStepNumberSetsPassedBirthStepNumber()
		{
			int expectedBirthStepNumber = 10;

			Firework result = new Firework(FireworkType.Initial, expectedBirthStepNumber, 0);

			Assert.AreEqual(expectedBirthStepNumber, result.BirthStepNumber);
		}

		[Test]
		public void Constructor2ValidBirthOrderSetsPassedBirthOrder()
		{
			int expectedBirthOrder = 2;

			Firework result = new Firework(FireworkType.Initial, 0, expectedBirthOrder);

			Assert.AreEqual(expectedBirthOrder, result.BirthOrder);
		}

		[Test]
		public void Constructor2ValidArgumentsSetsEmptyCoordinates()
		{
			Firework result = new Firework(FireworkType.Initial, 0, 0);

			Assert.NotNull(result.Coordinates);
			Assert.AreEqual(0, result.Coordinates.Count);
		}

		[Test]
		public void LabelValidFireworkGetsValidLabel()
		{
			var result = new Firework(FireworkType.ExplosionSpark, 2, 17);
			string label = result.Label;

			Assert.AreEqual("2.ExplosionSpark.17", label);
		}
		#endregion
	}
}