namespace CipherGeist.Math.Fireworks.Problems;

/// <summary>
/// Arguments of the Problem.QualityCalculating event.
/// </summary>
public class QualityCalculatingEventArgs : EventArgs
{
	/// <summary>
	/// Initializes a new instance of <see cref="QualityCalculatingEventArgs"/> type.
	/// </summary>
	/// <param name="coordinateValues">The coordinates of the solution to calculate
	/// quality for.</param>
	public QualityCalculatingEventArgs(IDictionary<Dimension, double> coordinateValues)
	{
		CoordinateValues = coordinateValues;
	}

	/// <summary>
	/// Gets the coordinates of the solution to calculate quality for.
	/// </summary>
	public IDictionary<Dimension, double> CoordinateValues { get; private set; }
}