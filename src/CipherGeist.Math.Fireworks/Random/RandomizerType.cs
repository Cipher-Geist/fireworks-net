namespace CipherGeist.Math.Fireworks.Random;

/// <summary>
/// The types of available randomzers.
/// </summary>
public enum RandomizerType
{
	/// <summary>
	/// Default.
	/// </summary>
	Default,
	/// <summary>
	/// Thread safe default.
	/// </summary>
	ThreadSafeDefault,
	/// <summary>
	/// MersenneTwister.
	/// </summary>
	MersenneTwister
}