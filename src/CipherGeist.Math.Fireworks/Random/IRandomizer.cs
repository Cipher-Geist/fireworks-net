namespace CipherGeist.Math.Fireworks.Random;

/// <summary>
/// A random number generator.
/// </summary>
public interface IRandomizer
{
	/// <summary>
	/// Returns a nonnegative random number.
	/// </summary>
	/// <returns>
	/// A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>.
	/// </returns>
	int Next();
	/// <summary>
	/// Returns a nonnegative random number less than the specified maximum.
	/// </summary>
	/// <param name="maxValue">The exclusive upper bound of the random number to be generated.
	/// <paramref name="maxValue"/> must be greater than or equal to zero.</param>
	/// <returns>
	/// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>;
	/// that is, the range of return values ordinarily includes zero but not <paramref name="maxValue"/>.
	/// However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
	/// </returns>
	int Next(int maxValue);

	/// <summary>
	/// Returns a random number within a specified range.
	/// </summary>
	/// <param name="minValue">The inclusive lower bound of the random number returned.</param>
	/// <param name="maxValue">The exclusive upper bound of the random number returned.
	/// <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
	/// </param>
	/// <returns>
	/// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than
	/// <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/>
	/// but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>,
	/// <paramref name="minValue"/> is returned.
	/// </returns>
	int Next(int minValue, int maxValue);

	/// <summary>
	/// Fills the elements of a specified array of bytes with random numbers.
	/// </summary>
	/// <param name="buffer">An array of bytes to contain random numbers.</param>
	void NextBytes(byte[] buffer);

	/// <summary>
	/// Returns a random number between 0.0 and 1.0.
	/// </summary>
	/// <returns>
	/// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
	/// </returns>
	double NextDouble();

	/// <summary>
	/// Gets the type of the pseudo random number generator.
	/// </summary>
	RandomizerType RandomizerType { get; }
}