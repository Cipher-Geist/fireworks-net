namespace CipherGeist.Math.Fireworks.Selection.Extremum;

/// <summary>
/// Contains logic for selecting the best and the worst <see cref="Firework"/>s from a given set according to some rule.
/// </summary>
public class ExtremumFireworkSelector : IExtremumFireworkSelector
{
	private readonly ProblemTarget _problemTarget;

	/// <summary>
	/// Initializes a new instance of <see cref="ExtremumFireworkSelector"/> class.
	/// </summary>
	/// <param name="problemTarget">Target of the problem under investigation.</param>
	public ExtremumFireworkSelector(ProblemTarget problemTarget)
	{
		_problemTarget = problemTarget;
	}

	/// <summary>
	/// Selects the best of <see cref="Firework"/>s from the <paramref name="from"/> parameters (in terms of firework quality).
	/// </summary>
	/// <param name="from"><see cref="Firework"/>s to select the best one from.</param>
	/// <returns>The best <see cref="Firework"/>.</returns>
	public Firework SelectBest(params Firework[] from)
	{
		Debug.Assert(from != null, "Firework parameters is null");
		return SelectBest((IEnumerable<Firework>)from);
	}

	/// <summary>
	/// Selects the best of <see cref="Firework"/>s from the <paramref name="from"/> collection (in terms of firework quality).
	/// </summary>
	/// <param name="from">A collection to select <see cref="Firework"/>s from.</param>
	/// <returns>The best <see cref="Firework"/>.</returns>
	/// <exception cref="ArgumentNullException">if <paramref name="from"/> is <c>null</c>.</exception>
	public Firework SelectBest(IEnumerable<Firework> from)
	{
		ArgumentNullException.ThrowIfNull(from);

		OnBestFireworkFinding(new ExtremumFireworkFindingEventArgs(from));

		var bestFirework = _problemTarget == ProblemTarget.Minimum
			? from.Aggregate(GetLessQualityFirework)
			: from.Aggregate(GetGreaterQualityFirework);

		OnBestFireworkFound(new ExtremumFireworkFoundEventArgs(from, bestFirework));

		return bestFirework;
	}

	/// <summary>
	/// Selects the worst of <see cref="Firework"/>s from the <paramref name="from"/> parameters (in terms of firework quality).
	/// </summary>
	/// <param name="from"><see cref="Firework"/>s to select the worst one from.</param>
	/// <returns>The worst <see cref="Firework"/>.</returns>
	public Firework SelectWorst(params Firework[] from)
	{
		Debug.Assert(from != null, "Firework parameters is null");
		return SelectWorst((IEnumerable<Firework>)from);
	}

	/// <summary>
	/// Selects the worst of <see cref="Firework"/>s from the <paramref name="from"/> collection (in terms of firework quality).
	/// </summary>
	/// <param name="from">A collection to select <see cref="Firework"/>s from.</param>
	/// <returns>The worst <see cref="Firework"/>.</returns>
	/// <exception cref="ArgumentNullException">if <paramref name="from"/> is <c>null</c>.</exception>
	public Firework SelectWorst(IEnumerable<Firework> from)
	{
		ArgumentNullException.ThrowIfNull(from);

		OnWorstFireworkFinding(new ExtremumFireworkFindingEventArgs(from));

		var worstFirework = _problemTarget == ProblemTarget.Minimum 
			? from.Aggregate(GetGreaterQualityFirework) 
			: from.Aggregate(GetLessQualityFirework);

		OnWorstFireworkFound(new ExtremumFireworkFoundEventArgs(from, worstFirework));

		return worstFirework;
	}

	/// <summary>
	/// Gets <see cref="Firework"/> with minimum quality.
	/// </summary>
	/// <param name="currentMinimum">Current minimum quality <see cref="Firework"/>.</param>
	/// <param name="candidate">The <see cref="Firework"/> to be compared with
	/// <paramref name="currentMinimum"/>.</param>
	/// <returns>The <see cref="Firework"/> with minimum quality.</returns>
	protected virtual Firework GetLessQualityFirework(Firework currentMinimum, Firework candidate)
	{
		return candidate.Quality.IsLess(currentMinimum.Quality) 
			? candidate 
			: currentMinimum;
	}

	/// <summary>
	/// Gets <see cref="Firework"/> with maximum quality.
	/// </summary>
	/// <param name="currentMaximum">Current maximum quality <see cref="Firework"/>.</param>
	/// <param name="candidate">The <see cref="Firework"/> to be compared with
	/// <paramref name="currentMaximum"/>.</param>
	/// <returns>The <see cref="Firework"/> with maximum quality.</returns>
	protected virtual Firework GetGreaterQualityFirework(Firework currentMaximum, Firework candidate)
	{
		return candidate.Quality.IsGreater(currentMaximum.Quality) 
			? candidate 
			: currentMaximum;
	}

	/// <summary>
	/// Firing an event before searching for the best firework.
	/// </summary>
	/// <param name="eventArgs">Event arguments.</param>
	protected virtual void OnBestFireworkFinding(ExtremumFireworkFindingEventArgs eventArgs)
	{
		BestFireworkFinding?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Firing an event after finding the best firework.
	/// </summary>
	/// <param name="eventArgs">Event arguments.</param>
	protected virtual void OnBestFireworkFound(ExtremumFireworkFoundEventArgs eventArgs)
	{
		BestFireworkFound?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Firing an event before searching for the worst firework.
	/// </summary>
	/// <param name="eventArgs">Event arguments.</param>
	protected virtual void OnWorstFireworkFinding(ExtremumFireworkFindingEventArgs eventArgs)
	{
		WorstFireworkFinding?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Firing an event after finding the worst firework.
	/// </summary>
	/// <param name="eventArgs">Event arguments.</param>
	protected virtual void OnWorstFireworkFound(ExtremumFireworkFoundEventArgs eventArgs)
	{
		WorstFireworkFound?.Invoke(this, eventArgs);
	}

	/// <summary>
	/// Fired before looking for the best firework.
	/// </summary>
	public event EventHandler<ExtremumFireworkFindingEventArgs>? BestFireworkFinding;

	/// <summary>
	/// Fired after the best firework is found.
	/// </summary>
	public event EventHandler<ExtremumFireworkFoundEventArgs>? BestFireworkFound;

	/// <summary>
	/// Fired before looking for the worst firework.
	/// </summary>
	public event EventHandler<ExtremumFireworkFindingEventArgs>? WorstFireworkFinding;

	/// <summary>
	/// Fired after the worst firework is found.
	/// </summary>
	public event EventHandler<ExtremumFireworkFoundEventArgs>? WorstFireworkFound;
}