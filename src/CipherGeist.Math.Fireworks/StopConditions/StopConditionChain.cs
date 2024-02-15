namespace CipherGeist.Math.Fireworks.StopConditions;

/// <summary>
/// Allows to "chain" stop conditions, making algorithm stop when either
/// all or at least one of the chained conditions are true.
/// </summary>
/// <remarks>Works in short circuit manner. Cannot mix ANDs and ORs.</remarks>
public sealed class ChainStopCondition : IStopCondition
{
	/// <summary>
	/// Chaining operation.
	/// </summary>
	private enum AggregationOperator
	{
		/// <summary>
		/// The default operation that has no effect.
		/// </summary>
		None = 0,

		/// <summary>
		/// The "AND" - algorithm will stop when all of the 
		/// chained conditions are true.
		/// </summary>
		And,

		/// <summary>
		/// The "OR" - algorithm will stop when at least one of
		/// the chained conditions is true.
		/// </summary>
		Or
	}

	private readonly IList<IStopCondition> _stopConditions;
	private ChainStopCondition.AggregationOperator _aggregationMode;

	/// <summary>
	/// Prevents a default instance of the <see cref="ChainStopCondition"/> class from being created.
	/// </summary>
	private ChainStopCondition()
	{
		_stopConditions = new List<IStopCondition>();
		_aggregationMode = AggregationOperator.None;
	}

	/// <summary>
	/// Creates a new instance of <see cref="ChainStopCondition"/> and adds
	/// the specified stop condition as first in the chain.
	/// </summary>
	/// <param name="firstStopCondition">The first stop condition.</param>
	/// <returns>A new instance of <see cref="ChainStopCondition"/> with
	/// <paramref name="firstStopCondition"/> as first in the chain.</returns>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="firstStopCondition"/>
	/// is <c>null</c>.</exception>
	public static ChainStopCondition From(IStopCondition firstStopCondition)
	{
		ArgumentNullException.ThrowIfNull(firstStopCondition, nameof(firstStopCondition));

		var chain = new ChainStopCondition();
		Debug.Assert(chain._stopConditions != null, "Chain stop condition collection is null");

		chain._stopConditions.Add(firstStopCondition);
		return chain;
	}

	/// <summary>
	/// Adds the specified stop condition to the chain with AND operation.
	/// </summary>
	/// <param name="anotherStopCondition">Another stop condition.</param>
	/// <returns>Stop condition chain with added <paramref name="anotherStopCondition"/>.
	/// </returns>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="anotherStopCondition"/>
	/// is <c>null</c>.</exception>
	public ChainStopCondition And(IStopCondition anotherStopCondition)
	{
		ArgumentNullException.ThrowIfNull(anotherStopCondition);

		return AddStopCondition(anotherStopCondition, AggregationOperator.And);
	}

	/// <summary>
	/// Adds the specified stop condition to the chain with OR operation.
	/// </summary>
	/// <param name="anotherStopCondition">Another stop condition.</param>
	/// <returns>Stop condition chain with added <paramref name="anotherStopCondition"/>.</returns>
	/// <exception cref="System.ArgumentNullException"> if <paramref name="anotherStopCondition"/>
	/// is <c>null</c>.</exception>
	public ChainStopCondition Or(IStopCondition anotherStopCondition)
	{
		ArgumentNullException.ThrowIfNull(anotherStopCondition);

		return AddStopCondition(anotherStopCondition, AggregationOperator.Or);
	}

	/// <summary>
	/// Tells if an algorithm that is currently in <paramref name="state"/> state
	/// should stop (and don't make further steps) or not.
	/// </summary>
	/// <param name="state">The current algorithm state.</param>
	/// <returns>
	/// <c>true</c> if an algorithm that is currently in <paramref name="state"/>
	/// state should stop (and don't make further steps). Otherwise <c>false</c>.
	/// </returns>
	/// <exception cref="InvalidOperationException"> if unsupported
	/// <see cref="ChainStopCondition.AggregationOperator"/> is used to chain stop conditions.</exception>
	public bool ShouldStop(IAlgorithmState state)
	{
		ArgumentNullException.ThrowIfNull(state);

		switch (_aggregationMode)
		{
			case AggregationOperator.And:
			{
				foreach (IStopCondition stopCondition in _stopConditions)
				{
					Debug.Assert(stopCondition != null, "Stop condition is null");

					if (!stopCondition.ShouldStop(state))
					{
						return false;
					}
				}

				return true;
			}
			case AggregationOperator.Or:
			{
				foreach (IStopCondition stopCondition in _stopConditions)
				{
					Debug.Assert(stopCondition != null, "Stop condition is null");

					if (stopCondition.ShouldStop(state))
					{
						return true;
					}
				}

				return false;
			}
			default:
				throw new InvalidOperationException();
		}
	}

	/// <summary>
	/// Adds the stop condition to the chain and ties it to the chain with specified
	/// operator.
	/// </summary>
	/// <param name="anotherStopCondition">Another stop condition.</param>
	/// <param name="mode">The mode (operator).</param>
	/// <returns>Stop condition chain with added <paramref name="anotherStopCondition"/>.</returns>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="mode"/>
	/// differs from one of the supported operations or is the default one.</exception>
	/// <exception cref="InvalidOperationException"> if trying to add new stop condition
	/// with the operator that differs from previous ones.</exception>
	private ChainStopCondition AddStopCondition(IStopCondition anotherStopCondition, ChainStopCondition.AggregationOperator mode)
	{
		if (mode == AggregationOperator.None)
		{
			throw new ArgumentOutOfRangeException(nameof(mode));
		}

		if (_aggregationMode == AggregationOperator.None)
		{
			_aggregationMode = mode;
		}

		if (_aggregationMode != mode)
		{
			throw new InvalidOperationException();
		}

		Debug.Assert(_stopConditions != null, "Stop condition collection is null");

		_stopConditions.Add(anotherStopCondition);
		return this;
	}
}