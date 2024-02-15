using CipherGeist.Math.Fireworks.Model;
using MathNet.Numerics;
using System;

namespace CipherGeist.Math.Fireworks.Solving
{
	/// <summary>
	/// Function root finder.
	/// </summary>
	public class Solver : ISolver
	{
		/// <summary>
		/// Start the solver.
		/// </summary>
		/// <param name="func">The function to solve.</param>
		/// <param name="variationRange">The variation range for the target function.</param>
		/// <returns>The solution.</returns>
		public double Solve(Func<double, double> func, Interval variationRange)
		{
			ArgumentNullException.ThrowIfNull(func);
			ArgumentNullException.ThrowIfNull(variationRange);

			return FindRoots.OfFunction(func, variationRange.Minimum, variationRange.Maximum);
		}
	}
}