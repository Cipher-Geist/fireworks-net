using CipherGeist.Math.Fireworks.Exceptions;

namespace CipherGeist.Math.Fireworks.Distances;

/// <summary>
/// Base class for distance calculators.
/// </summary>
public abstract class DistanceBase : IDistance
{
	/// <summary>
	/// A collection of <see cref="Dimension"/>s - needed for 
	/// <see cref="Solution"/>-to-<see cref="double"/>-array conversion.
	/// </summary>
	private readonly IEnumerable<Dimension> _dimensions;

	/// <summary>
	/// Initializes a new instance of <see cref="DistanceBase"/> with defined collection of <see cref="Dimension"/>s.
	/// </summary>
	/// <param name="dimensions">The collection of <see cref="Dimension"/>s
	/// - needed for <see cref="Solution"/>-to-<see cref="double"/>-array conversion.</param>
	protected DistanceBase(IEnumerable<Dimension> dimensions)
	{
		_dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
	}

	/// <summary>
	/// Calculates distance between two entities. Entities coordinates 
	/// are represented by <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first entity.</param>
	/// <param name="second">The second entity.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	public abstract double Calculate(double[] first, double[] second);

	/// <summary>
	/// Calculates distance between two <see cref="Solution"/>s. Solution coordinates 
	/// are to be stored in <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first solution.</param>
	/// <param name="second">The second solution.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	public virtual double Calculate(ISolution first, ISolution second)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		if (first == second)
		{
			return 0.0;
		}

		GetCoordinates(first, second, out double[] firstCoordinates, out double[] secondCoordinates);
		return Calculate(firstCoordinates, secondCoordinates);
	}

	/// <summary>
	/// Calculates distance between two <see cref="Solution"/>s. Solution coordinates 
	/// are to be stored in <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first solution as coordinates.</param>
	/// <param name="second">The second solution as coordinates.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	public virtual double Calculate(IDictionary<Dimension, double> first, IDictionary<Dimension, double> second)
	{
		ArgumentNullException.ThrowIfNull(first);
		ArgumentNullException.ThrowIfNull(second);

		if (first == second)
		{
			return 0.0;
		}

		GetCoordinates(first, second, out double[] firstCoordinates, out double[] secondCoordinates);
		return Calculate(firstCoordinates, secondCoordinates);
	}

	/// <summary>
	/// Calculates distance between a solution and an entity. Entity coordinates 
	/// are represented by <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The solution.</param>
	/// <param name="second">The entity.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	public virtual double Calculate(Solution first, double[] second)
	{
		double[] firstCoordinates = GetCoordinates(first);

		if (firstCoordinates.Length == second.Length)
		{
			throw new CoordinateLengthMismatchException();
		}

		return Calculate(firstCoordinates, second);
	}

	/// <summary>
	/// Converts <paramref name="solution"/> to <see cref="double"/> array.
	/// </summary>
	/// <param name="solution">The <see cref="Solution"/> instance to be converted.</param>
	/// <returns><see cref="double"/> array that corresponds to the coordinates of <paramref name="solution"/>.</returns>
	protected virtual double[] GetCoordinates(ISolution solution)
	{
		int dimensionCounter = 0;
		double[] coordinates = new double[_dimensions.Count()];

		foreach (Dimension dimension in _dimensions)
		{
			coordinates[dimensionCounter] = solution.Coordinates[dimension];
			dimensionCounter++;
		}

		return coordinates;
	}

	/// <summary>
	/// Converts <paramref name="first"/> and <paramref name="second"/> to <see cref="double"/> arrays.
	/// </summary>
	/// <param name="first">The first <see cref="Solution"/> instance to be converted.</param>
	/// <param name="second">The second <see cref="Solution"/> instance to be converted.</param>
	/// <param name="firstCoordinates">The <see cref="double"/> array that corresponds to the <paramref name="first"/> solution.</param>
	/// <param name="secondCoordinates">The <see cref="double"/> array that corresponds to the <paramref name="second"/> solution.</param>
	protected virtual void GetCoordinates(ISolution first, ISolution second, out double[] firstCoordinates, out double[] secondCoordinates)
	{
		GetCoordinates(first.Coordinates, second.Coordinates, out firstCoordinates, out secondCoordinates);
	}

	/// <summary>
	/// Converts <paramref name="first"/> and <paramref name="second"/> to <see cref="double"/> arrays.
	/// </summary>
	/// <param name="first">The first set of coordinates to be converted.</param>
	/// <param name="second">The second set of coordinates to be converted.</param>
	/// <param name="firstCoordinates">The <see cref="double"/> array that corresponds to the <paramref name="first"/> solution.</param>
	/// <param name="secondCoordinates">The <see cref="double"/> array that corresponds to the <paramref name="second"/> solution.</param>
	protected virtual void GetCoordinates(
		IDictionary<Dimension, double> first,
		IDictionary<Dimension, double> second,
		out double[] firstCoordinates,
		out double[] secondCoordinates)
	{
		firstCoordinates = new double[_dimensions.Count()];
		secondCoordinates = new double[_dimensions.Count()];

		int dimensionCounter = 0;
		foreach (Dimension dimension in _dimensions)
		{
			firstCoordinates[dimensionCounter] = first[dimension];
			secondCoordinates[dimensionCounter] = second[dimension];
			dimensionCounter++;
		}
	}
}