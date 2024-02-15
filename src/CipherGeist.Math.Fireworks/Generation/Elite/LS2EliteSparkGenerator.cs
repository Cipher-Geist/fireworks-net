using Interval = CipherGeist.Math.Fireworks.Model.Interval;

namespace CipherGeist.Math.Fireworks.Generation.Elite;

/// <summary>
/// Elite strategy spark generator using first order functions, per 2012 paper.
/// </summary>
public class LS2EliteSparkGenerator : EliteSparkGenerator
{
	private readonly IDifferentiator _differentiator;
	private readonly ISolver _solver;

	/// <summary>
	/// Initializes a new instance of the <see cref="LS2EliteSparkGenerator"/> class.
	/// </summary>
	/// <param name="dimensions">The dimensions to fit generated sparks into.</param>
	/// <param name="polynomialFit">The polynomial fit.</param>
	/// <param name="differentiator">A function differentiator.</param>
	/// <param name="solver">A polynomial function solver.</param>
	/// <exception cref="ArgumentNullException"> if <paramref name="differentiator"/> or <paramref name="solver"/> is <c>null</c>.
	/// </exception>
	public LS2EliteSparkGenerator(IEnumerable<Dimension> dimensions, IFit polynomialFit, IDifferentiator differentiator, ISolver solver)
		: base(dimensions, polynomialFit)
	{
		_differentiator = differentiator ?? throw new ArgumentNullException(nameof(differentiator));
		_solver = solver ?? throw new ArgumentNullException(nameof(solver));
	}

	/// <summary>
	/// Calculates elite point with help differentiating <paramref name="func"/> and solving the resulting function.
	/// </summary>
	/// <param name="func">The function to calculate elite point.</param>
	/// <param name="variationRange">Represents an interval to calculate elite point.</param>
	/// <returns>The coordinate of elite point.</returns>
	protected override double CalculateElitePoint(Func<double, double> func, Interval variationRange)
	{
		ArgumentNullException.ThrowIfNull(func);
		ArgumentNullException.ThrowIfNull(variationRange, nameof(variationRange));

		Func<double, double> derivative = _differentiator.Differentiate(func);
		return _solver.Solve(derivative, variationRange);
	}
}