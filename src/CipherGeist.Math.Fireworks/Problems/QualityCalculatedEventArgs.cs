namespace CipherGeist.Math.Fireworks.Problems;

/// <summary>
/// Arguments of the Problem.QualityCalculated event.
/// </summary>
public class QualityCalculatedEventArgs : QualityCalculatingEventArgs
{
	/// <summary>
	/// Initializes a new instance of <see cref="QualityCalculatedEventArgs"/> type.
	/// </summary>
	/// <param name="coordinateValues">The coordinates of the solution to calculate
	/// quality for.</param>
	/// <param name="quality">The calculated solution quality.</param>
	public QualityCalculatedEventArgs(IDictionary<Dimension, double> coordinateValues, double quality)
		: base(coordinateValues)
	{
		Quality = quality;
	}

	/// <summary>
	/// Gets the quality of the solution.
	/// </summary>
	public double Quality { get; private set; }
}