namespace CipherGeist.Math.Fireworks.Fit;

/// <summary>
/// Polynomial approximation of a function.
/// </summary>
public class PolynomialFit : IFit
{
	private readonly int _order;

	/// <summary>
	/// Initializes a new instance of the <see cref="PolynomialFit"/> class.
	/// </summary>
	/// <param name="order">The order of polynomial function.</param>
	/// <exception cref="System.ArgumentOutOfRangeException"> if <paramref name="order"/>
	/// is less than zero.</exception>
	public PolynomialFit(int order)
	{
		if (order < 0)
			throw new ArgumentOutOfRangeException(nameof(order));

		_order = order;
	}

	/// <summary>
	/// Least-Squares fitting the points (x,y), where x-s represented as 
	/// <paramref name="argumentValues"/> and y-s represented as 
	/// <paramref name="functionValues"/>.
	/// </summary>
	/// <param name="argumentValues">A firework coordinates.</param>
	/// <param name="functionValues">A firework qualities.</param>
	/// <returns>Approximated polynomial function.</returns>
	/// <exception cref="System.ArgumentNullException"> if 
	/// <paramref name="argumentValues"/> and <paramref name="functionValues"/>
	/// is <c>null</c>.</exception>
	public Func<double, double> Approximate(double[] argumentValues, double[] functionValues)
	{
		ArgumentNullException.ThrowIfNull(argumentValues);
		ArgumentNullException.ThrowIfNull(functionValues);

		return MathNet.Numerics.Fit.PolynomialFunc(argumentValues, functionValues, _order);
	}
}