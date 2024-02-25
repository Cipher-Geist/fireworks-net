using MathNet.Numerics.Random;

namespace CipherGeist.Math.Fireworks.Random;

/// <summary>
/// A factory for different randomizers.
/// </summary>
internal class RandomizerFactory
{
	/// <summary>
	/// Get a randomizer.
	/// </summary>
	/// <param name="randomizerType">The type of randomizer.</param>
	/// <returns>The randomizer object.</returns>
	public static System.Random GetRandomizer(RandomizerType randomizerType)
	{
		return randomizerType switch
		{
			RandomizerType.MersenneTwister => new MersenneTwister(true),
			RandomizerType.ThreadSafeDefault => new SystemRandomSource(true),
			_ => new SystemRandomSource(),
		};
	}

	/// <summary>
	/// Get a randomizer.
	/// </summary>
	/// <param name="randomizerType">The type of randomizer.</param>
	/// <param name="seed">The random seed.</param>
	/// <param name="threadSafe">Thread safe? This is more expensive...</param>
	/// <returns>The randomizer object.</returns>
	public static System.Random GetRandomizer(RandomizerType randomizerType, int seed, bool threadSafe)
	{
		return randomizerType switch
		{
			RandomizerType.MersenneTwister => new MersenneTwister(seed, threadSafe),
			RandomizerType.ThreadSafeDefault => new SystemRandomSource(seed, threadSafe),
			_ => new SystemRandomSource(),
		};
	}
}
