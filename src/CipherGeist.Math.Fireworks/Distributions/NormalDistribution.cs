using MathNet.Numerics.Distributions;

namespace CipherGeist.Math.Fireworks.Distributions;

/// <summary>
/// Normal (Gaussian) distribution.
/// </summary>
public class NormalDistribution : IContinuousDistribution
{
	private readonly Normal _internalNormal;

	/// <summary>
	/// Initializes a new instance of the <see cref="NormalDistribution"/> class.
	/// </summary>
	/// <param name="mean">The mean.</param>
	/// <param name="standardDeviation">The standard deviation.</param>
	public NormalDistribution(double mean, double standardDeviation)
	{
		_internalNormal = new Normal(mean, standardDeviation);
	}

	/// <summary>
	/// Draws a random sample from the distribution.
	/// </summary>
	/// <returns>
	/// A sample from the distribution.
	/// </returns>
	public double Sample()
	{
		return _internalNormal.Sample();
	}

	/// <summary>
	/// Draws a sequence of random samples from the distribution.
	/// </summary>
	/// <returns>
	/// An infinite sequence of samples from the distribution.
	/// </returns>
	public IEnumerable<double> Samples()
	{
		return _internalNormal.Samples();
	}
}