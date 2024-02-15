namespace CipherGeist.Math.Fireworks.Algorithm.Settings;

/// <summary>
/// Settings for the Dynamic Fireworks algorithm.
/// </summary>
public class DynamicFireworksAlgorithmSettings : FireworksAlgorithmSettings2012
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DynamicFireworksAlgorithmSettings"/> class.
	/// </summary>
	public DynamicFireworksAlgorithmSettings() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="DynamicFireworksAlgorithmSettings"/> class.
	/// </summary>
	/// <param name="fireworksAlgorithmSettings">The 2012 settings to use as the base.</param>
	public DynamicFireworksAlgorithmSettings(FireworksAlgorithmSettings2012 fireworksAlgorithmSettings)
	{
		LocationsNumber = fireworksAlgorithmSettings.LocationsNumber;
		ExplosionSparksNumberModifier = fireworksAlgorithmSettings.ExplosionSparksNumberModifier;
		ExplosionSparksNumberLowerBound = fireworksAlgorithmSettings.ExplosionSparksNumberLowerBound;
		ExplosionSparksNumberUpperBound = fireworksAlgorithmSettings.ExplosionSparksNumberUpperBound;
		ExplosionSparksMaximumAmplitude = fireworksAlgorithmSettings.ExplosionSparksMaximumAmplitude;
		SpecificSparksNumber = fireworksAlgorithmSettings.SpecificSparksNumber;
		SpecificSparksPerExplosionNumber = fireworksAlgorithmSettings.SpecificSparksPerExplosionNumber;
		FunctionOrder = fireworksAlgorithmSettings.FunctionOrder;
		SamplingNumber = fireworksAlgorithmSettings.SamplingNumber;
	}

	/// <summary>
	/// Gets or sets the amplification coefficent (C_a) used in the Dynamic Fireworks Algorithm.
	/// </summary>
	public double AmplificationCoefficent { get; set; }

	/// <summary>
	/// Gets or sets the reduction coefficent (C_r) used in the Dynamic Fireworks Algorithm.
	/// </summary>
	public double ReductionCoefficent { get; set; }
}