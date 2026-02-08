namespace CipherGeist.Math.Fireworks.Distances;

/// <summary>
/// Calculates distance between two entities.
/// </summary>
public interface IDistance
{
	/// <summary>
	/// Calculates distance between two entities. Entities coordinates 
	/// are represented by <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first entity.</param>
	/// <param name="second">The second entity.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	double Calculate(double[] first, double[] second);

	/// <summary>
	/// Calculates distance between two <see cref="Solution"/>s. Solution coordinates 
	/// are to be stored in <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first solution.</param>
	/// <param name="second">The second solution.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	double Calculate(ISolution first, ISolution second);

	/// <summary>
	/// Calculates distance between two <see cref="Solution"/>s. Solution coordinates 
	/// are to be stored in <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The first solution as coordinates.</param>
	/// <param name="second">The second solution as coordinates.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	double Calculate(IDictionary<Dimension, double> first, IDictionary<Dimension, double> second);

	/// <summary>
	/// Calculates distance between a solution and an entity. Entity coordinates 
	/// are represented by <paramref name="first"/> and <paramref name="second"/>.
	/// </summary>
	/// <param name="first">The solution.</param>
	/// <param name="second">The entity.</param>
	/// <returns>The distance between <paramref name="first"/> and <paramref name="second"/>.</returns>
	double Calculate(Solution first, double[] second);
}