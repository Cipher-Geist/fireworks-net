namespace CipherGeist.Math.Fireworks.Amplitude;

/// <summary>
/// The base class for all Amplitude generators. 
/// </summary>
public abstract class AmplitudeGeneratorBase : IAmplitudeGenerator
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AmplitudeGeneratorBase"/> class.
	/// </summary>
	/// <param name="extremumFireworkSelector">The extremum fireworks selector to use.</param>
	/// <param name="settings">The exploder settings.</param>
	public AmplitudeGeneratorBase(IExtremumFireworkSelector extremumFireworkSelector, ExploderSettings settings)
	{
		ExtremumFireworkSelector = extremumFireworkSelector;
		Settings = settings;
	}

	/// <summary>
	/// Calculate the relevent amplitude.
	/// </summary>
	/// <param name="focus">The target firework.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <param name="currentStepNumber">The current algorithmic step number.</param>
	/// <returns>The required amplitude.</returns>
	public abstract double CalculateAmplitude(Firework focus, IEnumerable<Firework> currentFireworks, int currentStepNumber);

	/// <summary>
	/// Calculate the relevent amplitude.
	/// </summary>
	/// <param name="focus">The target firework.</param>
	/// <param name="currentFireworks">The collection of fireworks that exist at the moment of explosion.</param>
	/// <param name="currentStepNumber">The current algorithmic step number.</param>
	/// <returns>The required amplitude.</returns>
	public double CalculateAmplitudeBase(Firework focus, IEnumerable<Firework> currentFireworks, int currentStepNumber)
	{
		var currentFireworkQualities = currentFireworks.Select(fw => fw.Quality);
		var bestFireworkQuality = ExtremumFireworkSelector.SelectBest(currentFireworks).Quality;

		if (double.IsNaN(bestFireworkQuality) || double.IsInfinity(bestFireworkQuality))
		{
			throw new ArgumentOutOfRangeException(nameof(bestFireworkQuality));
		}

		return Settings.ExplosionSparksMaximumAmplitude * (focus.Quality - bestFireworkQuality + double.Epsilon) /
			(currentFireworkQualities.Sum(fw => fw - bestFireworkQuality) + double.Epsilon);
	}

	/// <summary>
	/// Gets the extreamum firework selector. 
	/// </summary>
	public IExtremumFireworkSelector ExtremumFireworkSelector { get; }

	/// <summary>
	/// Gets the current exploder settings. 
	/// </summary>
	public ExploderSettings Settings { get; }
}