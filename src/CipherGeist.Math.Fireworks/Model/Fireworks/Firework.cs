using System.Globalization;

namespace CipherGeist.Math.Fireworks.Model.Fireworks;

/// <summary>
/// Represents a single firework.
/// </summary>
public class Firework : Solution
{
	/// <summary>
	/// Firework label format: {BirthStepNumber}.{FireworkType}.{BirthOrder}
	/// </summary>
	protected const string LabelFormat = "{0}.{1}.{2}";

	/// <summary>
	/// Initializes a new instance of the <see cref="Firework"/> class.
	/// </summary>
	/// <param name="fireworkType">The type of the firework (or spark this firework has been originated from).</param>
	/// <param name="birthStepNumber">The number of step this firework was created at.</param>
	/// <param name="birthOrder">The number of firework in the collection of fireworks born by the same generator within one step.</param>
	/// <param name="parentFirework">The parent firework that spawned the current firework.</param>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="birthStepNumber"/> or <paramref name="birthOrder"/> is less than zero.</exception>
	public Firework(FireworkType fireworkType, int birthStepNumber, int birthOrder, Firework? parentFirework)
		: base(
			parentFirework == null || parentFirework.Coordinates == null
				? new Dictionary<Dimension, double>() 
				: parentFirework.Coordinates, 
			double.NaN)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(birthStepNumber);
		ArgumentOutOfRangeException.ThrowIfNegative(birthOrder);

		Id = new TId();
		FireworkType = fireworkType;
		BirthStepNumber = birthStepNumber;
		BirthOrder = birthOrder;
		ParentFirework = parentFirework;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Firework"/> class.
	/// </summary>
	/// <param name="fireworkType">The type of the firework (or spark this firework has been originated from).</param>
	/// <param name="birthStepNumber">The number of step this firework was created at.</param>
	/// <param name="birthOrder">The number of firework in the collection of fireworks born by
	/// the same generator within one step.</param>
	public Firework(FireworkType fireworkType, int birthStepNumber, int birthOrder)
		: this(fireworkType, birthStepNumber, birthOrder, default(Firework))
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Firework"/> class.
	/// </summary>
	/// <param name="fireworkType">The type of the firework (or spark this firework has been originated from).</param>
	/// <param name="birthStepNumber">The number of step this firework was created at.</param>
	/// <param name="birthOrder">The number of firework in the collection of fireworks born by the same generator within one step.</param>
	/// <param name="coordinates">The firework coordinates.</param>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="birthStepNumber"/> or <paramref name="birthOrder"/> is less than zero.</exception>
	/// <exception cref="ArgumentNullException"> if <paramref name="coordinates"/> is <c>null</c>.</exception>
	public Firework(FireworkType fireworkType, int birthStepNumber, int birthOrder, IDictionary<Dimension, double> coordinates)
		: base(coordinates, double.NaN)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(birthStepNumber);
		ArgumentOutOfRangeException.ThrowIfNegative(birthOrder);

		Id = new TId();
		FireworkType = fireworkType;
		BirthStepNumber = birthStepNumber;
		BirthOrder = birthOrder;
		ParentFirework = default;
	}

	/// <summary>
	/// Gets a unique identifier of this <see cref="Firework"/>.
	/// </summary>
	public TId Id { get; private set; }

	/// <summary>
	/// Gets or sets the type of the firework (or spark this firework
	/// has been originated from).
	/// </summary>
	public FireworkType FireworkType { get; protected set; }

	/// <summary>
	/// Gets or sets the number of step this firework was created at.
	/// </summary>
	public int BirthStepNumber { get; protected set; }

	/// <summary>
	/// Gets or sets the number of firework in the collection of fireworks born by
	/// the same generator within one step.
	/// </summary>
	public int BirthOrder { get; protected set; }

	/// <summary>
	/// Gets or sets the parent firework that spawned the current firework. 
	/// Note, added to support the Dynamic FWA algorithm types.
	/// </summary>
	public Firework? ParentFirework { get; protected set; }

	/// <summary>
	/// Gets the firework label that can be used to easily distinguish it
	/// from other fireworks: {BirthStepNumber}.{FireworkType}.{BirthOrder}
	/// </summary>
	public virtual string Label
	{
		get
		{
			return string.Format(
				CultureInfo.InvariantCulture, 
				LabelFormat, 
				BirthStepNumber, 
				FireworkType, 
				BirthOrder);
		}
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		var coords = Coordinates == null
			? "<None>"
			: string.Join(", ", Coordinates.Values.Select(v => v.ToString()));

		return $"Firework Info: {Label}. Solution Coordinates: ({coords})";
	}
}