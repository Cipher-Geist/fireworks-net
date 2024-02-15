namespace CipherGeist.Math.Fireworks.Exceptions;

/// <summary>
/// Exception thrown when two coordinates for the same solution have differing lengths.
/// </summary>
internal class CoordinateLengthMismatchException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CoordinateLengthMismatchException"/> class.
	/// </summary>
	public CoordinateLengthMismatchException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CoordinateLengthMismatchException"/> class.
	/// </summary>
	/// <param name="message">The exception message.</param>
	public CoordinateLengthMismatchException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CoordinateLengthMismatchException"/> class.
	/// </summary>
	/// <param name="message">The exception message.</param>
	/// <param name="inner">The inner exception.</param>
	public CoordinateLengthMismatchException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
