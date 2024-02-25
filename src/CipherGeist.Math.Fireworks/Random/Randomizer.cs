using MathNet.Numerics.Random;

namespace CipherGeist.Math.Fireworks.Random;

/// <summary>
/// Represents a pseudo-random number generator.
/// </summary>
/// <remarks>Uses <see cref="MersenneTwister"/> thread-safe wrapper
/// around <see cref="Random"/> that comes with Math.NET Numerics.</remarks>
public class Randomizer : System.Random, IRandomizer
{
	private readonly System.Random _randomizer;
	private readonly RandomizerType _randomizerType;

	/// <summary>
	/// Initializes a new instance of the <see cref="Randomizer"/> class.
	/// </summary>
	public Randomizer()
	{
		_randomizer = new MersenneTwister();
		_randomizerType = RandomizerType.MersenneTwister;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Randomizer"/> class using the specified pseudo random number generator.
	/// </summary>
	/// <param name="randomizerType">The type of the pseudo random number generator.</param>
	public Randomizer(RandomizerType randomizerType)
	{
		_randomizer = RandomizerFactory.GetRandomizer(randomizerType);
		_randomizerType = randomizerType;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Randomizer"/> class.
	/// </summary>
	/// <param name="seed">The seed.</param>
	public Randomizer(int seed)
		: this(seed, true)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Randomizer"/> class using the specified pseudo random number generator.
	/// </summary>
	/// <param name="randomizerType">The type of the pseudo random number generator.</param>
	/// <param name="seed">The seed.</param>
	public Randomizer(RandomizerType randomizerType, int seed)
		: this(randomizerType, seed, true)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Randomizer"/> class.
	/// </summary>
	/// <param name="seed">The seed.</param>
	/// <param name="threadSafe">If set to <c>true</c>, a thread safe version
	/// is used. Default value is <c>true</c>.</param>
	public Randomizer(int seed, bool threadSafe)
	{
		_randomizer = new MersenneTwister(seed, threadSafe);
		_randomizerType = RandomizerType.MersenneTwister;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Randomizer"/> class using the specified pseudo random number generator.
	/// </summary>
	/// <param name="randomizerType">The type of the pseudo random number generator.</param>
	/// <param name="seed">The seed.</param>
	/// <param name="threadSafe">If set to <c>true</c>, a thread safe version
	/// is used. Default value is <c>true</c>.</param>
	public Randomizer(RandomizerType randomizerType, int seed, bool threadSafe)
	{
		_randomizer = RandomizerFactory.GetRandomizer(randomizerType, seed, threadSafe);
		_randomizerType = randomizerType;
	}

	/// <summary>
	/// Returns a nonnegative random number.
	/// </summary>
	/// <returns>
	/// A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>.
	/// </returns>
	public override int Next()
	{
		return _randomizer.Next();
	}

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
	public override int Next(int maxValue)
	{
		return _randomizer.Next(maxValue);
	}

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
	public override int Next(int minValue, int maxValue)
	{
		return _randomizer.Next(minValue, maxValue);
	}

	/// <summary>
	/// Fills the elements of a specified array of bytes with random numbers.
	/// </summary>
	/// <param name="buffer">An array of bytes to contain random numbers.</param>
	public override void NextBytes(byte[] buffer)
	{
		_randomizer.NextBytes(buffer);
	}

	/// <summary>
	/// Returns a random number between 0.0 and 1.0.
	/// </summary>
	/// <returns>
	/// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
	/// </returns>
	public override double NextDouble()
	{
		return _randomizer.NextDouble();
	}

	/// <summary>
	/// Gets the type of the pseudo random number generator.
	/// </summary>
	public RandomizerType RandomizerType 
	{ 
		get { return _randomizerType; } 
	}
}