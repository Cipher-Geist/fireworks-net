namespace CipherGeist.Math.Fireworks.Distances;

/// <summary>
/// Distance calculator that finds Euclidean distance between two entities.
/// </summary>
public class EuclideanDistance : DistanceBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EuclideanDistance"/> class.
	/// </summary>
	/// <param name="dimensions">The collection of <see cref="Dimension"/>s
	/// - needed for <see cref="Solution"/>-to-<see cref="double"/>-array
	/// conversion.</param>
	public EuclideanDistance(IEnumerable<Dimension> dimensions)
		: base(dimensions)
	{
	}

	/// <summary>
	/// Calculates Euclidean distance between two entities. Entities coordinates
	/// are represented by <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first entity.</param>
	/// <param name="second">The second entity.</param>
	/// <returns>
	/// The distance between <paramref name="first"/> and <paramref name="second"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="first"/> or <paramref name="second"/> is <c>null</c>.</exception>
	/// <exception cref="CoordinateLengthMismatchException"> if <paramref name="second"/>'s 
	/// length is not equal to <paramref name="first"/>'s length.</exception>
	public override double Calculate(double[] first, double[] second)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		if (first.Length != second.Length)
		{
			throw new CoordinateLengthMismatchException();
		}

		return MathNet.Numerics.Distance.Euclidean(first, second);
	}
}