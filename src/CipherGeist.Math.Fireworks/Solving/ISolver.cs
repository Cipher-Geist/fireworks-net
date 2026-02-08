namespace CipherGeist.Math.Fireworks.Solving;

/// <summary>
/// Function root finder.
/// </summary>
public interface ISolver
{
	/// <summary>
	/// Start the solver.
	/// </summary>
	/// <param name="func">The function to solve.</param>
	/// <param name="variationRange">The variation range for the target function.</param>
	/// <returns>The solution.</returns>
	double Solve(Func<double, double> func, Interval variationRange);
}