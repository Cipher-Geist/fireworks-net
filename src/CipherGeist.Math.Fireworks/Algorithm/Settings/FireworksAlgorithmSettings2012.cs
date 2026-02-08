namespace CipherGeist.Math.Fireworks.Algorithm.Settings;

/// <summary>
/// Stores user-defined constants that control algorithm run.
/// </summary>
/// <remarks>Uses notation described in 2012 paper.</remarks>
public class FireworksAlgorithmSettings2012 : FireworksAlgorithmSettings
{
	/// <summary>
	/// Initializes a new instance of the <see cref="FireworksAlgorithmSettings2012"/> class.
	/// </summary>
	public FireworksAlgorithmSettings2012()
	{ 
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FireworksAlgorithmSettings2012"/> class.
	/// </summary>
	/// <param name="fireworksAlgorithmSettings">The base settings to use.</param>
	public FireworksAlgorithmSettings2012(FireworksAlgorithmSettings fireworksAlgorithmSettings)
	{
		LocationsNumber = fireworksAlgorithmSettings.LocationsNumber;
		ExplosionSparksNumberModifier = fireworksAlgorithmSettings.ExplosionSparksNumberModifier;
		ExplosionSparksNumberLowerBound = fireworksAlgorithmSettings.ExplosionSparksNumberLowerBound;
		ExplosionSparksNumberUpperBound = fireworksAlgorithmSettings.ExplosionSparksNumberUpperBound;
		ExplosionSparksMaximumAmplitude = fireworksAlgorithmSettings.ExplosionSparksMaximumAmplitude;
		SpecificSparksNumber = fireworksAlgorithmSettings.SpecificSparksNumber;
		SpecificSparksPerExplosionNumber = fireworksAlgorithmSettings.SpecificSparksPerExplosionNumber;
	}

	/// <summary>
	/// Gets or sets the order of polynomial function.
	/// </summary>
	public int FunctionOrder { get; set; }

	/// <summary>
	/// Gets or sets the number of sparks to select.
	/// </summary>
	public int SamplingNumber { get; set; }
}