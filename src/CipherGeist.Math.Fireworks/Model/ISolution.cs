namespace CipherGeist.Math.Fireworks.Model;

/// <summary>
/// The contract for solutions.
/// </summary>
public interface ISolution
{
	/// <summary>
	/// Gets solution coordinates in problem space.
	/// #TODO: Think of replacing Dictionary with some derived class, like CoordinateDictionary
	/// </summary>
	IDictionary<Dimension, double>? Coordinates { get; }

	/// <summary>
	/// Gets or sets solution quality (value of target function).
	/// </summary>
	double Quality { get; set; }
}