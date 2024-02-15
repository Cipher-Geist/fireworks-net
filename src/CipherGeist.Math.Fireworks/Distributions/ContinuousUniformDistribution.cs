using MathNet.Numerics.Distributions;

namespace CipherGeist.Math.Fireworks.Distributions;

/// <summary>
/// Continuous Uniform distribution.
/// </summary>
public class ContinuousUniformDistribution : IContinuousDistribution
{
	private readonly ContinuousUniform _internalContinuousUniform;

	/// <summary>
	/// Initializes a new instance of the <see cref="ContinuousUniformDistribution"/> class.
	/// </summary>
	/// <param name="lower">Lower bound. Interval: lower ≤ upper.</param>
	/// <param name="upper">Upper bound. Interval: lower ≤ upper.</param>
	public ContinuousUniformDistribution(double lower, double upper)
	{
		_internalContinuousUniform = new ContinuousUniform(lower, upper);
	}

	/// <summary>
	/// Draws a random sample from the distribution.
	/// </summary>
	/// <returns>
	/// A sample from the distribution.
	/// </returns>
	public double Sample()
	{
		return _internalContinuousUniform.Sample();
	}

	/// <summary>
	/// Draws a sequence of random samples from the distribution.
	/// </summary>
	/// <returns>
	/// An infinite sequence of samples from the distribution.
	/// </returns>
	public IEnumerable<double> Samples()
	{
		return _internalContinuousUniform.Samples();
	}
}