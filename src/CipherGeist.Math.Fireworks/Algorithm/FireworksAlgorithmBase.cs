using Microsoft.Extensions.Logging;

namespace CipherGeist.Math.Fireworks.Algorithm;

/// <summary>
/// Base class for Fireworks Algorithm implementation.
/// </summary>
/// <typeparam name="TSettings">Algorithm settings type.</typeparam>
public abstract class FireworksAlgorithmBase<TSettings> : IFireworksAlgorithm
	where TSettings : class
{
	/// <summary>
	/// The logger.
	/// </summary>
	protected ILogger _logger;

	/// <summary>
	/// Initializes the algorith.
	/// </summary>
	/// <param name="problem">The problem to be solved by the algorithm.</param>
	/// <param name="stopCondition">The stop condition for the algorithm.</param>
	/// <param name="settings">The algorithm settings.</param>
	/// <param name="logger">A logger.</param>
	/// <exception cref="ArgumentNullException">If <paramref name="problem"/> or <paramref name="stopCondition"/> 
	/// or <paramref name="settings"/> is <c>null</c>.</exception>
	public FireworksAlgorithmBase(Problem problem, IStopCondition stopCondition, TSettings settings, ILogger logger)
	{
		ProblemToSolve = problem ?? throw new ArgumentNullException(nameof(problem));
		StopCondition = stopCondition ?? throw new ArgumentNullException(nameof(stopCondition));
		Settings = settings ?? throw new ArgumentNullException(nameof(settings));
		
		_logger = logger;

		BestWorstFireworkSelector = new ExtremumFireworkSelector(problem.Target);
	}

	/// <summary>
	/// Solves the specified problem by running the algorithm.
	/// </summary>
	/// <returns><see cref="Solution"/> instance that represents
	/// best solution found during the algorithm run.</returns>
	public abstract Solution? Solve();

	/// <summary>
	/// Calculates the quality for the given <paramref name="firework"/>.
	/// </summary>
	/// <param name="firework">The firework to calculate quality for.</param>
	/// <remarks>It is expected that <paramref name="firework"/> hasn't got its quality calculated before.</remarks>
	public virtual void CalculateQuality(Firework firework)
	{
		// If quality is not NaN, it most likely has been already calculated.
		Debug.Assert(double.IsNaN(firework.Quality), "Excessive quality calculation");

		ArgumentNullException.ThrowIfNull(firework, nameof(firework));
		ArgumentNullException.ThrowIfNull(ProblemToSolve, nameof(ProblemToSolve));

		firework.Quality = ProblemToSolve.CalculateQuality(firework.Coordinates);
	}

	/// <summary>
	/// Calculates the qualities for each <see cref="Firework"/> in <paramref name="fireworks"/> collection.
	/// </summary>
	/// <param name="fireworks">The fireworks to calculate qualities for.</param>
	/// <remarks>It is expected that none of the <paramref name="fireworks"/>
	/// has its quality calculated before.</remarks>
	public virtual void CalculateQualities(IEnumerable<Firework> fireworks)
	{
		ArgumentNullException.ThrowIfNull(fireworks, nameof(fireworks));
		ArgumentNullException.ThrowIfNull(ProblemToSolve, nameof(ProblemToSolve));

		foreach (Firework firework in fireworks)
		{
			CalculateQuality(firework);
		}
	}

	/// <summary>
	/// Gets the extremum firework selector.
	/// </summary>
	public IExtremumFireworkSelector BestWorstFireworkSelector { get; }

	/// <summary>
	/// Gets the problem to be solved by the algorithm.
	/// </summary>
	public Problem ProblemToSolve { get; }

	/// <summary>
	/// Gets the stop condition for the algorithm.
	/// </summary>
	public IStopCondition StopCondition { get; }

	/// <summary>
	/// Gets the algorithm settings.
	/// </summary>
	public TSettings Settings { get; }
}