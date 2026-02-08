using System.Globalization;

namespace CipherGeist.Math.Fireworks.Model;

/// <summary>
/// Represents an interval (range).
/// </summary>
/// <remarks>Immutable.</remarks>
public sealed class Interval : IEquatable<Interval>, IFormattable
{
	/// <summary>
	/// Stores string format of the <see cref="Interval"/> determined
	/// during initialization. It should be used when interval boundaries
	/// have to be formatted.
	/// </summary>
	private readonly string _cachedStringFormat;

	/// <summary>
	/// Initializes a new instance of <see cref="Interval"/>.
	/// </summary>
	/// <param name="minimum">Lower boundary.</param>
	/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
	/// <param name="maximum">Upper boundary.</param>
	/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="minimum"/> or
	/// <paramref name="maximum"/> is <see cref="double.NaN"/>. If <paramref name="minimum"/>
	/// is greater than <paramref name="maximum"/>.</exception>
	/// <remarks><see cref="double.NegativeInfinity"/> and <see cref="double.PositiveInfinity"/>
	/// boundaries will be always open (exclusive).</remarks>
	public Interval(
		double minimum,
		bool isMinimumOpen,
		double maximum,
		bool isMaximumOpen)
	{
		Interval.ValidateBoundaries(minimum, maximum);

		Minimum = minimum;
		Maximum = maximum;

		Length = System.Math.Abs(Maximum - Minimum);

		IsMinimumOpen = double.IsNegativeInfinity(Minimum) ? true : isMinimumOpen;
		IsMaximumOpen = double.IsPositiveInfinity(Maximum) ? true : isMaximumOpen;

		IsOpen = IsMinimumOpen && IsMaximumOpen;

		_cachedStringFormat = (IsMinimumOpen ? "(" : "[") + "{0}, {1}" + (IsMaximumOpen ? ")" : "]");
	}

	/// <summary>
	/// Initializes a new instance of <see cref="Interval"/> which is
	/// closed (inclusive) from both sides.
	/// </summary>
	/// <param name="minimum">Lower boundary.</param>
	/// <param name="maximum">Upper boundary.</param>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="minimum"/> or
	/// <paramref name="maximum"/> is <see cref="double.NaN"/>. If <paramref name="minimum"/>
	/// is greater than <paramref name="maximum"/>.</exception>
	public Interval(
		double minimum,
		double maximum)
		: this(minimum, false, maximum, false)
	{
	}

	/// <summary>
	/// Checks whether a <paramref name="value"/> belongs to the <see cref="Interval"/>.
	/// </summary>
	/// <param name="value">Value that needs to be tested.</param>
	/// <returns><c>true</c> if <paramref name="value"/> belongs to the <see cref="Interval"/>;
	/// otherwise <c>false</c>.</returns>
	public bool IsInRange(double value)
	{
		if (value.IsLess(Minimum) || value.IsGreater(Maximum))
		{
			return false;
		}

		if (value.IsGreater(Minimum) && value.IsLess(Maximum))
		{
			return true;
		}

		bool isMinEdge = value.IsEqual(Minimum);
		if (IsMinimumOpen && isMinEdge)
		{
			return false;
		}

		bool isMaxEdge = value.IsEqual(Maximum);
		if (IsMaximumOpen && isMaxEdge)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Checks whether an <paramref name="otherRange"/> belongs to the <see cref="Interval"/>.
	/// </summary>
	/// <param name="otherRange"><see cref="Interval"/> that needs to be tested.</param>
	/// <returns><c>true</c> if both <see cref="Interval.Minimum"/> and <see cref="Interval.Maximum"/>
	/// of <paramref name="otherRange"/> belong to the <see cref="Interval"/>;
	/// otherwise <c>false</c>.</returns>
	public bool IsInRange(Interval otherRange)
	{
		return IsInRange(otherRange.Minimum) && IsInRange(otherRange.Maximum);
	}

	#region ToString() overloads
	/// <summary>
	/// Converts <see cref="Interval"/> value to string with desired <paramref name="format"/> 
	/// for <see cref="double"/> to <see cref="string"/> conversion.
	/// </summary>
	/// <param name="format"><see cref="double"/> to <see cref="string"/> format.</param>
	/// <returns>String representation of this <see cref="Interval"/> instance. Boundaries
	/// are formatted with <paramref name="format"/>.</returns>
	public string ToString(string format)
	{
		return ToString(format, CultureInfo.CurrentCulture);
	}

	/// <summary>
	/// Formats the value of the current instance using the specified format.
	/// </summary>
	/// <param name="format">The format to use or a null reference to use
	/// the default format.</param>
	/// <param name="formatProvider">The provider to use to format the value or a null reference
	/// to obtain the numeric format information from the current locale
	/// setting of the operating system.</param>
	/// <returns>The value of the current instance in the specified format.</returns>
	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		return string.Format(
			_cachedStringFormat, 
			Minimum.ToString(format, formatProvider), 
			Maximum.ToString(format, formatProvider));
	}

	/// <summary>
	/// Converts <see cref="Interval"/> value to <see cref="string"/> 
	/// with <see cref="CultureInfo.InvariantCulture"/>.
	/// </summary>
	/// <returns>String representation of this <see cref="Interval"/> 
	/// instance - for <see cref="CultureInfo.InvariantCulture"/>.</returns>
	public string ToStringInvariant()
	{
		return ToStringInvariant(null);
	}

	/// <summary>
	/// Converts <see cref="Interval"/> value to string with desired <paramref name="format"/> 
	/// for <see cref="double"/> to <see cref="string"/> conversion and <see cref="CultureInfo.InvariantCulture"/>.
	/// </summary>
	/// <param name="format"><see cref="double"/> to <see cref="string"/> format.</param>
	/// <returns>String representation of this <see cref="Interval"/> instance. Boundaries
	/// are formatted with <paramref name="format"/> - for <see cref="CultureInfo.InvariantCulture"/>.</returns>
	public string ToStringInvariant(string? format)
	{
		return string.Format(
			_cachedStringFormat, 
			Minimum.ToString(format, CultureInfo.InvariantCulture), 
			Maximum.ToString(format, CultureInfo.InvariantCulture));
	}

	#endregion

	#region Factory methods

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationValue"/>; ends are closed (inclusive).
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationValue">Desired <see cref="Interval.Length"/> / 2.</param>
	/// <returns>New instance of <see cref="Interval"/>. Its minimum is
	/// <paramref name="mean"/> - <paramref name="deviationValue"/> and its
	/// maximum is <paramref name="mean"/> + <paramref name="deviationValue"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> 
	/// or <paramref name="deviationValue"/> is <see cref="double.NaN"/>. If 
	/// <paramref name="mean"/> or <paramref name="deviationValue"/> is 
	/// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
	/// If <paramref name="deviationValue"/> is less than zero.</exception>
	public static Interval Create(double mean, double deviationValue)
	{
		return Interval.Create(mean, deviationValue, false, false);
	}

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationValue"/>.
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationValue">Desired <see cref="Interval.Length"/> / 2.</param>
	/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
	/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
	/// <returns>New instance of <see cref="Interval"/>. Its minimum is
	/// <paramref name="mean"/> - <paramref name="deviationValue"/> and its
	/// maximum is <paramref name="mean"/> + <paramref name="deviationValue"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
	/// <paramref name="deviationValue"/> is <see cref="double.NaN"/>. If 
	/// <paramref name="mean"/> or <paramref name="deviationValue"/> is 
	/// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
	/// If <paramref name="deviationValue"/> is less than zero.</exception>
	public static Interval Create(double mean, double deviationValue, bool isMinimumOpen, bool isMaximumOpen)
	{
		Interval.ValidateMean(mean);
		Interval.ValidateDeviationValue(deviationValue);

		double min = mean - deviationValue;
		double max = mean + deviationValue;

		return new Interval(min, isMinimumOpen, max, isMaximumOpen);
	}

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationPercent"/>; ends are closed (inclusive).
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
	/// and resulting <see cref="Interval.Minimum"/> (or <see cref="Interval.Minimum"/>)
	/// expressed in percents of <paramref name="mean"/>.</param>
	/// <returns>New instance of <see cref="Interval"/> which is <paramref name="mean"/>
	/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> is 
	/// <see cref="double.NaN"/>. If <paramref name="mean"/> is <see cref="double.NegativeInfinity"/>
	/// or <see cref="double.PositiveInfinity"/>. If <paramref name="deviationPercent"/> is less than zero.
	/// </exception>
	public static Interval Create(double mean, int deviationPercent)
	{
		return Interval.Create(mean, deviationPercent, false, false);
	}

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationPercent"/>.
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
	/// and resulting <see cref="Interval.Minimum"/> (or <see cref="Interval.Minimum"/>)
	/// expressed in percents of <paramref name="mean"/>.</param>
	/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
	/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
	/// <returns>New instance of <see cref="Interval"/> which is <paramref name="mean"/>
	/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>.</returns>
	/// <exception cref="ArgumentException">If <paramref name="mean"/> is <see cref="double.NaN"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> is
	/// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
	/// If <paramref name="deviationPercent"/> is less than zero.</exception>
	public static Interval Create(double mean, int deviationPercent, bool isMinimumOpen, bool isMaximumOpen)
	{
		Interval.ValidateMean(mean);
		Interval.ValidateDeviationPercent(deviationPercent);

		var min = double.NaN;
		var max = double.NaN;
		var deviationValue = deviationPercent / 100.0;

		if (mean.IsGreaterOrEqual(0.0))
		{
			min = mean - (deviationValue * mean);
			max = mean + (deviationValue * mean);
		}
		else
		{
			min = mean + (deviationValue * mean);
			max = mean - (deviationValue * mean);
		}

		return new Interval(min, isMinimumOpen, max, isMaximumOpen);
	}

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationValue"/> but never exceed
	/// the restrictions; ends are closed (inclusive).
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationValue">Desired <see cref="Interval.Length"/> / 2.</param>
	/// <param name="minRestriction">Lower boundary restriction. <see cref="Interval.Minimum"/>
	/// of the resulting <see cref="Interval"/> instance will not be less than this value.</param>
	/// <param name="maxRestriction">Upper boundary restriction. <see cref="Interval.Maximum"/>
	/// of the resulting <see cref="Interval"/> instance will not be greater than this value.</param>
	/// <returns>New instance of <see cref="Interval"/>. Its minimum is
	/// MAX(<paramref name="mean"/> - <paramref name="deviationValue"/>; <paramref name="minRestriction"/>)
	/// and its maximum is MIN(<paramref name="mean"/> + <paramref name="deviationValue"/>;
	/// <paramref name="maxRestriction"/>).</returns>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
	/// <paramref name="deviationValue"/> or <paramref name="minRestriction"/> or 
	/// <paramref name="maxRestriction"/> is <see cref="double.NaN"/>. If 
	/// <paramref name="mean"/> or <paramref name="deviationValue"/> is 
	/// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
	/// If <paramref name="deviationValue"/> is less than zero.
	/// If <paramref name="minRestriction"/> is greater than <paramref name="maxRestriction"/>.
	/// </exception>
	public static Interval CreateWithRestrictions(
		double mean,
		double deviationValue,
		double minRestriction,
		double maxRestriction)
	{
		return Interval.CreateWithRestrictions(mean, deviationValue, minRestriction, maxRestriction, false, false);
	}

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationValue"/> but never exceed
	/// the restrictions.
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationValue">Desired <see cref="Interval.Length"/> / 2.</param>
	/// <param name="minRestriction">Lower boundary restriction. <see cref="Interval.Minimum"/>
	/// of the resulting <see cref="Interval"/> instance will not be less than this value.</param>
	/// <param name="maxRestriction">Upper boundary restriction. <see cref="Interval.Maximum"/>
	/// of the resulting <see cref="Interval"/> instance will not be greater than this value.</param>
	/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
	/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
	/// <returns>New instance of <see cref="Interval"/>. Its minimum is
	/// MAX(<paramref name="mean"/> - <paramref name="deviationValue"/>; <paramref name="minRestriction"/>)
	/// and its maximum is MIN(<paramref name="mean"/> + <paramref name="deviationValue"/>;
	/// <paramref name="maxRestriction"/>).</returns>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
	/// <paramref name="deviationValue"/> or <paramref name="minRestriction"/> or 
	/// <paramref name="maxRestriction"/> is <see cref="double.NaN"/>. If <paramref name="mean"/>
	/// or <paramref name="deviationValue"/> is <see cref="double.NegativeInfinity"/> or
	/// <see cref="double.PositiveInfinity"/>. If <paramref name="deviationValue"/> is
	/// less than zero. If <paramref name="minRestriction"/> is greater than
	/// <paramref name="maxRestriction"/>.</exception>
	public static Interval CreateWithRestrictions(
		double mean,
		double deviationValue,
		double minRestriction,
		double maxRestriction,
		bool isMinimumOpen,
		bool isMaximumOpen)
	{
		Interval.ValidateMean(mean);
		Interval.ValidateDeviationValue(deviationValue);
		Interval.ValidateBoundaries(minRestriction, maxRestriction);

		double min = mean - deviationValue;
		if (min.IsLess(minRestriction))
		{
			min = minRestriction;
		}

		double max = mean + deviationValue;
		if (max.IsGreater(maxRestriction))
		{
			max = maxRestriction;
		}

		return new Interval(min, isMinimumOpen, max, isMaximumOpen);
	}

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationPercent"/> but never exceed
	/// the restrictions.
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
	/// and resulting <see cref="Interval.Minimum"/> (or <see cref="Interval.Minimum"/>)
	/// expressed in percents of <paramref name="mean"/>.</param>
	/// <param name="minRestriction">Lower boundary restriction. <see cref="Interval.Minimum"/>
	/// of the resulting <see cref="Interval"/> instance will not be less than this value.</param>
	/// <param name="maxRestriction">Upper boundary restriction. <see cref="Interval.Maximum"/>
	/// of the resulting <see cref="Interval"/> instance will not be greater than this value.</param>
	/// <returns>New instance of <see cref="Interval"/> which is <paramref name="mean"/>
	/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>. Restrictions
	/// are applied and can replace <see cref="Interval.Minimum"/> and <see cref="Interval.Maximum"/>
	/// in the result.</returns>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
	/// <paramref name="minRestriction"/> or <paramref name="maxRestriction"/> is 
	/// <see cref="double.NaN"/>. If <paramref name="mean"/> is <see cref="double.NegativeInfinity"/>
	/// or <see cref="double.PositiveInfinity"/>. If <paramref name="deviationPercent"/> is less than 
	/// zero. If <paramref name="minRestriction"/> is greater than <paramref name="maxRestriction"/>.
	/// </exception>
	public static Interval CreateWithRestrictions(
		double mean,
		int deviationPercent,
		double minRestriction,
		double maxRestriction)
	{
		return Interval.CreateWithRestrictions(mean, deviationPercent, minRestriction, maxRestriction, false, false);
	}

	/// <summary>
	/// Creates new instance of <see cref="Interval"/>. Boundaries are calculated from
	/// <paramref name="mean"/> and <paramref name="deviationPercent"/> but never exceed
	/// the restrictions.
	/// </summary>
	/// <param name="mean">Mean (middle) of the interval.</param>
	/// <param name="deviationPercent">Desired distance between <paramref name="mean"/>
	/// and resulting <see cref="Interval.Minimum"/> (or <see cref="Interval.Minimum"/>)
	/// expressed in percents of <paramref name="mean"/>.</param>
	/// <param name="minRestriction">Lower boundary restriction. <see cref="Interval.Minimum"/>
	/// of the resulting <see cref="Interval"/> instance will not be less than this value.</param>
	/// <param name="maxRestriction">Upper boundary restriction. <see cref="Interval.Maximum"/>
	/// of the resulting <see cref="Interval"/> instance will not be greater than this value.</param>
	/// <param name="isMinimumOpen">Whether lower boundary is open (exclusive) or not.</param>
	/// <param name="isMaximumOpen">Whether upper boundary is open (exclusive) or not.</param>
	/// <returns>New instance of <see cref="Interval"/> which is <paramref name="mean"/>
	/// +- <paramref name="deviationPercent"/> * <paramref name="mean"/>. Restrictions
	/// are applied and can replace <see cref="Interval.Minimum"/> and <see cref="Interval.Maximum"/>
	/// in the result.</returns>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> or 
	/// <paramref name="minRestriction"/> or <paramref name="maxRestriction"/> is 
	/// <see cref="double.NaN"/>. If <paramref name="mean"/> is <see cref="double.NegativeInfinity"/>
	/// or <see cref="double.PositiveInfinity"/>. If <paramref name="deviationPercent"/> 
	/// is less than zero. If <paramref name="minRestriction"/> is greater than <paramref name="maxRestriction"/>.
	/// </exception>
	public static Interval CreateWithRestrictions(
		double mean,
		int deviationPercent,
		double minRestriction,
		double maxRestriction,
		bool isMinimumOpen,
		bool isMaximumOpen)
	{
		Interval.ValidateMean(mean);
		Interval.ValidateDeviationPercent(deviationPercent);
		Interval.ValidateBoundaries(minRestriction, maxRestriction);

		double min = double.NaN;
		double max = double.NaN;
		double deviationValue = deviationPercent / 100.0;

		if (mean.IsGreaterOrEqual(0.0))
		{
			min = mean - (deviationValue * mean);
			max = mean + (deviationValue * mean);
		}
		else
		{
			min = mean + (deviationValue * mean);
			max = mean - (deviationValue * mean);
		}

		if (min.IsLess(minRestriction))
		{
			min = minRestriction;
		}

		if (max.IsGreater(maxRestriction))
		{
			max = maxRestriction;
		}

		return new Interval(min, isMinimumOpen, max, isMaximumOpen);
	}

	#endregion

	#region Validation

	/// <summary>
	/// Validates mean (middle) value of the <see cref="Interval"/>.
	/// </summary>
	/// <param name="mean">Mean (middle) value of the <see cref="Interval"/>
	/// that needs to be validated.</param>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="mean"/> is
	/// <see cref="double.NaN"/>. If <paramref name="mean"/> is 
	/// <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>.
	/// </exception>
	private static void ValidateMean(double mean)
	{
		if (double.IsNaN(mean))
		{
			throw new ArgumentOutOfRangeException(nameof(mean));
		}

		if (double.IsInfinity(mean))
		{
			throw new ArgumentOutOfRangeException(nameof(mean));
		}
	}

	/// <summary>
	/// Validates deviation value of the <see cref="Interval"/>.
	/// </summary>
	/// <param name="deviationValue">Deviation value of the <see cref="Interval"/>
	/// that needs to be validated.</param>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="deviationValue"/>
	/// is <see cref="double.NaN"/>. If <paramref name="deviationValue"/>
	/// is <see cref="double.NegativeInfinity"/> or <see cref="double.PositiveInfinity"/>
	/// or is less than zero.</exception>
	private static void ValidateDeviationValue(double deviationValue)
	{
		if (double.IsNaN(deviationValue))
		{
			throw new ArgumentOutOfRangeException(nameof(deviationValue));
		}

		if (double.IsInfinity(deviationValue))
		{
			throw new ArgumentOutOfRangeException(nameof(deviationValue));
		}

		if (deviationValue.IsLess(0.0))
		{
			throw new ArgumentOutOfRangeException(nameof(deviationValue));
		}
	}

	/// <summary>
	/// Validates deviation percent of the <see cref="Interval"/>.
	/// </summary>
	/// <param name="deviationPercent">Deviation percent of the <see cref="Interval"/>
	/// that needs to be validated.</param>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="deviationPercent"/>
	/// is less than zero.</exception>
	private static void ValidateDeviationPercent(int deviationPercent)
	{
		if (deviationPercent < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(deviationPercent));
		}
	}

	/// <summary>
	/// Validates lower and upper boundaries of the <see cref="Interval"/>.
	/// </summary>
	/// <param name="minimum">Lower boundary of the <see cref="Interval"/>
	/// that needs to be validated.</param>
	/// <param name="maximum">Upper boundary of the <see cref="Interval"/>
	/// that needs to be validated.</param>
	/// <exception cref="ArgumentOutOfRangeException">If <paramref name="minimum"/>
	/// or <paramref name="maximum"/> is <see cref="double.NaN"/>. If 
	/// <paramref name="minimum"/> is greater than <paramref name="maximum"/>.
	/// </exception>
	private static void ValidateBoundaries(double minimum, double maximum)
	{
		if (double.IsNaN(minimum))
		{
			throw new ArgumentOutOfRangeException(nameof(minimum));
		}

		if (double.IsNaN(maximum))
		{
			throw new ArgumentOutOfRangeException(nameof(maximum));
		}

		if (minimum.IsGreater(maximum))
		{
			throw new ArgumentOutOfRangeException(nameof(minimum));
		}
	}

	#endregion

	#region Object overrides

	/// <summary>
	/// Determines whether the specified <see cref="Interval"/> is equal to the current one.
	/// </summary>
	/// <param name="obj">The <see cref="Interval"/> object to compare with the current one.</param>
	/// <returns><c>true</c> if the specified <see cref="Interval"/> is equal to the current one;
	/// otherwise <c>false</c>.</returns>
	public override bool Equals(object? obj)
	{
		return Equals(obj as Interval);
	}

	/// <summary>
	/// Serves as a hash function for a particular type.
	/// </summary>
	/// <returns>A hash code for the current <see cref="Interval"/>.</returns>
	public override int GetHashCode()
	{
		// http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
		unchecked
		{
			int hash = 17;
			hash = (hash * 29) + Minimum.GetHashCode();
			hash = (hash * 29) + Maximum.GetHashCode();
			hash = (hash * 29) + IsMinimumOpen.GetHashCode();
			hash = (hash * 29) + IsMaximumOpen.GetHashCode();

			return hash;
		}
	}

	/// <summary>
	/// Returns a string that represents the current <see cref="Interval"/> instance.
	/// </summary>
	/// <returns>A string that represents the current <see cref="Interval"/> instance.</returns>
	public override string ToString()
	{
		return ToString(null, CultureInfo.CurrentCulture);
	}

	#endregion

	#region Comparison operators

	/// <summary>
	/// Determines whether values of two instances of <see cref="Interval"/> are equal.
	/// </summary>
	/// <param name="left">First instance of <see cref="Interval"/>.</param>
	/// <param name="right">Second instance of <see cref="Interval"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> value is equal to the <paramref name="right"/> 
	/// value; otherwise <c>false</c>.</returns>
	public static bool operator ==(Interval left, Interval right)
	{
		if (left is null)
		{
			return right is null;
		}

		return left.Equals(right);
	}

	/// <summary>
	/// Determines whether values of two instances of <see cref="Interval"/> are not equal.
	/// </summary>
	/// <param name="left">First instance of <see cref="Interval"/>.</param>
	/// <param name="right">Second instance of <see cref="Interval"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> value is not equal to the <paramref name="right"/> 
	/// value; otherwise <c>false</c>.</returns>
	public static bool operator !=(Interval left, Interval right)
	{
		if (left is null)
		{
			return right is not null;
		}

		return !left.Equals(right);
	}

	#endregion

	#region IEquatable<Interval>

	/// <summary>
	/// Indicates whether the current object is equal to another object of the same type.
	/// </summary>
	/// <param name="other">An object to compare with this object.</param>
	/// <returns><c>true</c> if the current object is equal to the other parameter; otherwise <c>false</c>.</returns>
	public bool Equals(Interval? other)
	{
		if (other is null)
		{
			return false;
		}

		if (ReferenceEquals(other, this))
		{
			return true;
		}

		return Minimum.IsEqual(other.Minimum) &&
			   Maximum.IsEqual(other.Maximum) &&
			   (IsMinimumOpen == other.IsMinimumOpen) &&
			   (IsMaximumOpen == other.IsMaximumOpen);
	}

	#endregion

	/// <summary>
	/// Gets lower boundary of the <see cref="Interval"/>.
	/// </summary>
	public double Minimum { get; private set; }

	/// <summary>
	/// Gets upper boundary of the <see cref="Interval"/>.
	/// </summary>
	public double Maximum { get; private set; }

	/// <summary>
	/// Gets <see cref="Interval"/> length. Always positive.
	/// </summary>
	/// <remarks><see cref="Interval.Length"/> = <see cref="Math"/>.Abs(
	/// <see cref="Interval.Maximum"/> - <see cref="Interval.Minimum"/>).</remarks>
	public double Length { get; private set; }

	/// <summary>
	/// Gets a value indicating whether an lower boundary of <see cref="Interval"/> is open 
	/// (i.e. minimum possible value is exclusive) or closed
	/// (i.e. minimum possible value is inclusive).
	/// </summary>
	public bool IsMinimumOpen { get; private set; }

	/// <summary>
	/// Gets a value indicating whether an upper boundary of <see cref="Interval"/> is open 
	/// (i.e. maximum possible value is exclusive) or closed
	/// (i.e. maximum possible value is inclusive).
	/// </summary>
	public bool IsMaximumOpen { get; private set; }

	/// <summary>
	/// Gets a value indicating whether <see cref="Interval"/> is open. <c>true</c> if both 
	/// <see cref="Interval.IsMinimumOpen"/> and <see cref="Interval.IsMaximumOpen"/>
	/// are <c>true</c>.
	/// </summary>
	public bool IsOpen { get; private set; }
}