using CipherGeist.Math.Fireworks.Model.Explosions;
using NUnit.Framework;
using System;

namespace CipherGeist.Math.Fireworks.Tests.Model
{
	public class InitialExplosionTests
	{
		[Test]
		public void InitialExplosion_NegativeAs1stParam_ExceptionThrown()
		{
			int initialSparksNumber = -1;
			string expectedParamName = "initialSparksNumber";

			var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new InitialExplosion(initialSparksNumber));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}
	}
}