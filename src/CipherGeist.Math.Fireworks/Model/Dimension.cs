namespace CipherGeist.Math.Fireworks.Model;

/// <summary>
/// Represents a continuous variable that can take any <see cref="double"/> value within specified <see cref="Interval"/>.
/// </summary>
/// <remarks>Immutable.</remarks>
public class Dimension : IEquatable<Dimension>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Dimension"/> class.
	/// </summary>
	/// <param name="range">The range for this dimension.</param>
	public Dimension(Interval range)
	{
		Range = range ?? throw new ArgumentNullException(nameof(range));
		Id = new TId();
	}

	/// <summary>
	/// Checks if a value is in the range.
	/// </summary>
	/// <param name="valueToCheck">The value to check.</param>
	/// <returns>True if it is in the range and false otherwise.</returns>
	public bool IsValueInRange(double valueToCheck)
	{
		return Range.IsInRange(valueToCheck);
	}

	#region Comparison Operators.
	/// <summary>
	/// Determines whether values of two instances of <see cref="Dimension"/> are equal.
	/// </summary>
	/// <param name="left">First instance of <see cref="Dimension"/>.</param>
	/// <param name="right">Second instance of <see cref="Dimension"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> value is equal to the <paramref name="right"/> 
	/// value; otherwise <c>false</c>.</returns>
	public static bool operator ==(Dimension left, Dimension right)
	{
		if (left is null)
		{
			return right is null;
		}

		return left.Equals(right);
	}

	/// <summary>
	/// Determines whether values of two instances of <see cref="Dimension"/> are not equal.
	/// </summary>
	/// <param name="left">First instance of <see cref="Dimension"/>.</param>
	/// <param name="right">Second instance of <see cref="Dimension"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> value is not equal to the <paramref name="right"/> 
	/// value; otherwise <c>false</c>.</returns>
	public static bool operator !=(Dimension left, Dimension right)
	{
		if (left is null)
		{
			return right is object;
		}

		return !left.Equals(right);
	}
	#endregion // Comparison Operators.

	#region IEquatable<Dimension> Implementation.
	/// <summary>
	/// Determines whether the specified <see cref="Dimension"/> is equal to the current one.
	/// </summary>
	/// <param name="obj">The <see cref="Dimension"/> object to compare with the current one.</param>
	/// <returns><c>true</c> if the specified <see cref="Dimension"/> is equal to the current one;
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

		return Equals(obj as Dimension);
	}

	/// <summary>
	/// Serves as a hash function for a particular type.
	/// </summary>
	/// <returns>A hash code for the current <see cref="Dimension"/>.</returns>
	public override int GetHashCode()
	{
		unchecked
		{
			int hash = (int)2_166_136_261;

			hash = (hash * 16_777_619) ^ Range.GetHashCode();
			hash = (hash * 16_777_619) ^ Id.GetHashCode();

			return hash;
		}
	}

	/// <summary>
	/// Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">An object to compare with this object.</param>
	/// <returns><c>true</c> if the current object is equal to the other parameter; otherwise <c>false</c>.</returns>
	public bool Equals(Dimension? other)
	{
		if (other is null)
		{
			return false;
		}

		if (ReferenceEquals(other, this))
		{
			return true;
		}

		return Id.Equals(other.Id) && Range.Equals(other.Range);
	}
	#endregion // IEquatable<Dimension> Implementation.

	/// <summary>
	/// Gets the unique TId ascociated with this range object.
	/// </summary>
	public TId Id { get; private set; }

	/// <summary>
	/// Gets the range interval.
	/// </summary>
	public Interval Range { get; private set; }

	/// <inheritdoc/>
	public override string ToString()
	{
		return Range.ToString();
	}
}