﻿namespace CipherGeist.Math.Fireworks.Algorithm.Settings;

/// <summary>
/// Stores user-defined constants that control algorithm run.
/// </summary>
/// <remarks>Uses notation described in 2010 paper.</remarks>
public class FireworksAlgorithmSettings
{
	/// <summary>
	/// Gets or sets the n - Number of fireworks (initial or selected on each step).
	/// </summary>
	public int LocationsNumber { get; set; }

	/// <summary>
	/// Gets or sets the m - Parameter controlling the total number of sparks generated by LocationsNumber.
	/// </summary>
	public double ExplosionSparksNumberModifier { get; set; }

	/// <summary>
	/// Gets or sets the a - Constant, has to be 0 &lt; a &lt; ExplosionSparksNumberUpperBound.
	/// </summary>
	public double ExplosionSparksNumberLowerBound { get; set; }

	/// <summary>
	/// Gets or sets the b - Constant, has to be ExplosionSparksNumberLowerBound &lt; b &lt; 1.
	/// </summary>
	public double ExplosionSparksNumberUpperBound { get; set; }

	/// <summary>
	/// Gets or sets the Â - Maximum explosion amplitude.
	/// </summary>
	public double ExplosionSparksMaximumAmplitude { get; set; }

	/// <summary>
	/// Gets or sets the m̂ - Number of specific sparks generated by all step explosions.
	/// </summary>
	public int SpecificSparksNumber { get; set; }

	/// <summary>
	/// Gets or sets the number of specific sparks generated by an explosion.
	/// </summary>
	/// <remarks>No such setting in the 2010 paper.</remarks>
	public int SpecificSparksPerExplosionNumber { get; set; }
}