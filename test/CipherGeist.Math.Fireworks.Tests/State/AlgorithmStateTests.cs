using CipherGeist.Math.Fireworks.Model;
using CipherGeist.Math.Fireworks.Model.Fireworks;
using CipherGeist.Math.Fireworks.State;
using NUnit.Framework;
using System;
using System.Linq;

namespace CipherGeist.Math.Fireworks.Tests.State
{
	public class AlgorithmStateTests
	{
		private readonly AlgorithmState _state;

		public AlgorithmStateTests()
		{
			_state = new AlgorithmState(Enumerable.Empty<Firework>(), 0, new Solution(0.0));
		}

		[Test]
		public void ConstructorNullFireworksExceptionThrown()
		{
			var actualException = Assert.Throws<ArgumentNullException>(() =>
				new AlgorithmState(null, 0, new Solution(0.0)));
			Assert.AreEqual("value", actualException.ParamName);
		}

		[Test]
		public void ConstructorNegativeValueExceptionThrown()
		{
			var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
				new AlgorithmState(Enumerable.Empty<Firework>(), -1, new Solution(0.0)));
			Assert.AreEqual("value", actualException.ParamName);
		}

		[Test]
		public void ConstructorNullValueExceptionThrown()
		{
			var actualException = Assert.Throws<ArgumentNullException>(() =>
				new AlgorithmState(Enumerable.Empty<Firework>(), 0, null));
			Assert.AreEqual("value", actualException.ParamName);
		}

		[Test]
		public void FireworksSetterNullFireworksExceptionThrown()
		{
			var actualException = Assert.Throws<ArgumentNullException>(() => _state.Fireworks = null);
			Assert.AreEqual("value", actualException.ParamName);
		}

		[Test]
		public void StepNumberSetterNegativeValueExceptionThrown()
		{
			var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => _state.StepNumber = -1);
			Assert.AreEqual("value", actualException.ParamName);
		}

		[Test]
		public void BestSolutionSetterNullValueExceptionThrown()
		{
			var actualException = Assert.Throws<ArgumentNullException>(() => _state.BestSolution = null);
			Assert.AreEqual("value", actualException.ParamName);
		}
	}
}