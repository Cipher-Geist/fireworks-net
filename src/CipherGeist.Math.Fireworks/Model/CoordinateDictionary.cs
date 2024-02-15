namespace CipherGeist.Math.Fireworks.Model;

/// <summary>
/// A coordinate dictionary. WIP.
/// </summary>
public class CoordinateDictionary : IEquatable<CoordinateDictionary>
{
	#region IEquatable<Dimension> Implementation.
	/// <summary>
	/// Determines whether the specified <see cref="CoordinateDictionary"/> is equal to the current one.
	/// </summary>
	/// <param name="obj">The <see cref="CoordinateDictionary"/> object to compare with the current one.</param>
	/// <returns><c>true</c> if the specified <see cref="CoordinateDictionary"/> is equal to the current one;
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

		return Equals(obj as CoordinateDictionary);
	}

	/// <summary>
	/// Serves as a hash function for a particular type.
	/// </summary>
	/// <returns>A hash code for the current <see cref="CoordinateDictionary"/>.</returns>
	public override int GetHashCode()
	{
		unchecked
		{
			int hash = (int)2_166_136_261;
			if (Coordinates != null)
			{
				foreach (var coordinate in Coordinates)
				{
					hash = (hash * 16_777_619) ^ coordinate.Key.Range.GetHashCode();
					hash = (hash * 16_777_619) ^ coordinate.Value.GetHashCode();
				}
			}

			return hash;
		}
	}

	/// <summary>
	/// Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">An object to compare with this object.</param>
	/// <returns><c>true</c> if the current object is equal to the other parameter; otherwise <c>false</c>.</returns>
	public bool Equals(CoordinateDictionary? other)
	{
		if (other is null)
		{
			return false;
		}

		if (ReferenceEquals(other, this))
		{
			return true;
		}

		if (other.Coordinates == null)
		{
			return Coordinates == null;
		}

		if (!other.Coordinates.Any())
		{
			return !Coordinates.Any();
		}

		// #TODO Sort this.
		return false;
	}
	#endregion // IEquatable<Dimension> Implementation.

	/// <summary>
	/// Gets the collection of dimension intervals and their respective solution values.
	/// </summary>
	public Dictionary<Dimension, double>? Coordinates { get; }
}
