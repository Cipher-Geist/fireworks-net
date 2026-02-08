namespace CipherGeist.Math.Fireworks.Amplitude;

/// <summary>
/// Generic amplitude generator.
/// </summary>
public class AmplitudeGenerator : AmplitudeGeneratorBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AmplitudeGenerator"/> class.
	/// </summary>
	/// <param name="extremumFireworkSelector">The extremum firework selector to use.</param>
	/// <param name="settings">The exploder settings.</param>
	public AmplitudeGenerator(IExtremumFireworkSelector extremumFireworkSelector, ExploderSettings settings)
		: base(extremumFireworkSelector, settings)
	{
	}

	/// <summary>
	/// Calculate the relevent amplitude.
	/// </summary>
	/// <param name="focus">The target firework.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <param name="currentStepNumber">The current algorithmic step number.</param>
	/// <returns>The required amplitude.</returns>
	public override double CalculateAmplitude(Firework focus, IEnumerable<Firework> currentFireworks, int currentStepNumber)
	{
		return CalculateAmplitudeBase(focus, currentFireworks, currentStepNumber);
	}
}