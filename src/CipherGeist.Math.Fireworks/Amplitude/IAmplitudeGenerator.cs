namespace CipherGeist.Math.Fireworks.Amplitude;

/// <summary>
/// The contract for all amplitude generators.
/// </summary>
public interface IAmplitudeGenerator
{
	/// <summary>
	/// Calculate the relevent amplitude.
	/// </summary>
	/// <param name="focus">The target firework.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <param name="currentStepNumber">The current algorithmic step number.</param>
	/// <returns>The required amplitude.</returns>
	double CalculateAmplitude(Firework focus, IEnumerable<Firework> currentFireworks, int currentStepNumber);

	/// <summary>
	/// Gets the extreamum firework selector. 
	/// </summary>
	IExtremumFireworkSelector ExtremumFireworkSelector { get; }

	/// <summary>
	/// Gets the exploder settings. 
	/// </summary>
	ExploderSettings Settings { get; }
}