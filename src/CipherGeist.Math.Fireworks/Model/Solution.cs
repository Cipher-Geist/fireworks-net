namespace CipherGeist.Math.Fireworks.Model;

/// <summary>
/// Represents a solution - a point in problem space defined by coordinates
/// and corresponding quality (value of target function).
/// </summary>
public class Solution : IEquatable<Solution>, ISolution
{
	/// <summary>
	/// Initializes a new instance of <see cref="Solution"/> with
	/// defined coordinates and quality.
	/// </summary>
	/// <param name="coordinates">Solution coordinates in problem space.</param>
	/// <param name="quality">Solution quality (value of target function).</param>
	public Solution(IDictionary<Dimension, double>? coordinates, double quality)
	{
		Coordinates = coordinates == null ? null : new Dictionary<Dimension, double>(coordinates);
		Quality = quality;
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Solution"/> with
	/// defined coordinates.
	/// </summary>
	/// <param name="coordinates">Solution coordinates in problem space.</param>
	/// <remarks><see cref="Quality"/> is set to <see cref="double.NaN"/>.</remarks>
	public Solution(IDictionary<Dimension, double> coordinates)
		: this(coordinates, double.NaN)
	{
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Solution"/> with
	/// defined quality.
	/// </summary>
	/// <param name="quality">Solution quality (value of target function).</param>
	/// <remarks><see cref="Coordinates"/> is set to <c>null</c>.</remarks>
	public Solution(double quality)
		: this(null, quality)
	{
	}

	#region Comparison Operators.
	/// <summary>
	/// Determines whether values of two instances of <see cref="Solution"/> are equal.
	/// </summary>
	/// <param name="left">First instance of <see cref="Solution"/>.</param>
	/// <param name="right">Second instance of <see cref="Solution"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> value is equal to the <paramref name="right"/> 
	/// value; otherwise <c>false</c>.</returns>
	public static bool operator ==(Solution? left, Solution? right)
	{
		// If both are null, or both are same instance, return true.
		if (ReferenceEquals(left, right))
		{
			return true;
		}

		// If one is null, but not both, return false.
		if (left is null || right is null)
		{
			return false;
		}

		// Return true if the fields for both objects are equal.
		return left.Equals(right);
	}

	/// <summary>
	/// Determines whether values of two instances of <see cref="Solution"/> are not equal.
	/// </summary>
	/// <param name="left">First instance of <see cref="Solution"/>.</param>
	/// <param name="right">Second instance of <see cref="Solution"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> value is not equal to the <paramref name="right"/> 
	/// value; otherwise <c>false</c>.</returns>
	public static bool operator !=(Solution? left, Solution? right)
	{
		return !(left == right);
	}
	#endregion // Comparison Operators.

	#region IEquatable<Solution>.
	/// <summary>
	/// Determines whether the specified <see cref="Solution"/> is equal to the current one.
	/// </summary>
	/// <param name="obj">The <see cref="Solution"/> object to compare with the current one.</param>
	/// <returns><c>true</c> if the specified <see cref="Solution"/> is equal to the current one;
	/// otherwise <c>false</c>.</returns>
	public override bool Equals(object? obj)
	{
		if (obj is null)
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != GetType())
		{
			return false;
		}

		return Equals(obj as Solution);
	}

	/// <summary>
	/// Serves as a hash function for a particular type.
	/// </summary>
	/// <returns>A hash code for the current <see cref="Solution"/>.</returns>
	public override int GetHashCode()
	{
		unchecked
		{
			int hash = (int)2_166_136_261;
			if (Coordinates != null)
			{
				hash = (hash * 16_777_619) ^ Coordinates.GetHashCode();
			}

			hash = (hash * 16_777_619) ^ Quality.GetHashCode();
			return hash;
		}
	}

	/// <summary>
	/// Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">An object to compare with this object.</param>
	/// <returns><c>true</c> if the current object is equal to the other parameter; otherwise <c>false</c>.</returns>
	public bool Equals(Solution? other)
	{
		if (other is null)
		{
			return false;
		}

		if (ReferenceEquals(other, this))
		{
			return true;
		}

		bool coordinatesEqual;
		if (Coordinates == null)
		{
			coordinatesEqual = other.Coordinates == null;
		}
		else
		{
			coordinatesEqual =
				Coordinates.Keys.Count == other.Coordinates?.Keys.Count &&
				Coordinates.Keys.All(k => other.Coordinates.ContainsKey(k) &&
				Equals(other.Coordinates[k], Coordinates[k]));
		}

		return coordinatesEqual && Quality.IsEqual(other.Quality);
	}
	#endregion // IEquatable<Solution>.

	/// <summary>
	/// Gets or sets solution coordinates in problem space.
	/// TODO: Think of replacing Dictionary with some derived class, like CoordinateDictionary.
	/// </summary>
	public IDictionary<Dimension, double>? Coordinates { get; protected set; }

	/// <summary>
	/// Gets or sets solution quality (value of target function).
	/// </summary>
	public double Quality { get; set; }
}