using CipherGeist.Math.Fireworks.Model;
using CipherGeist.Math.Fireworks.Solving;
using NUnit.Framework;
using System;

namespace CipherGeist.Math.Fireworks.Tests.Solving
{
	public class SolverTests
	{
		private readonly Solver _solver;
		private readonly Func<double, double> _targetFunc;
		private readonly Interval _range;

		public SolverTests()
		{
			_solver = new Solver();
			_targetFunc = x => x + x;
			_range = new Interval(-5, 5);
		}

		[Test]
		public void SolvePresentAllParamReturnsEqualRoot()
		{
			double expectedRoot = 0;
			double actualRoot = _solver.Solve(_targetFunc, _range);
			Assert.AreEqual(expectedRoot, actualRoot);
		}

		[Test]
		public void SolvePresentAllParamReturnsNonEqualRoot()
		{
			double expectedRoot = 1;
			double actualRoot = _solver.Solve(_targetFunc, _range);
			Assert.AreNotEqual(expectedRoot, actualRoot);
		}

		[Test]
		public void SolveNullAs1stParamExceptionThrown()
		{
			string expectedParamName = "func";
			Func<double, double> func = null;

			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => _solver.Solve(func, _range));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}

		[Test]
		public void SolveNullAs2ndParamExceptionThrown()
		{
			string expectedParamName = "variationRange";
			Interval testRange = null;

			ArgumentNullException actualException = Assert.Throws<ArgumentNullException>(() => _solver.Solve(_targetFunc, testRange));

			Assert.NotNull(actualException);
			Assert.AreEqual(expectedParamName, actualException.ParamName);
		}
	}
}