using Interval = CipherGeist.Math.Fireworks.Model.Interval;

namespace CipherGeist.Math.Fireworks.Extensions
{
	/// <summary>
	/// Extension methods that support randomness.
	/// </summary>
	public static class RandomExtensions
	{
		/// <summary>
		/// Gets the next random double.
		/// </summary>
		/// <param name="random">The random generator.</param>
		/// <param name="minInclusive">The minimum value inclusive.</param>
		/// <param name="maxExclusive">The maximum value inclusive.</param>
		/// <returns>A random double in the required range.</returns>
		public static double NextDouble(this System.Random random, double minInclusive, double maxExclusive)
		{
			return NextDoubleInternal(random, minInclusive, maxExclusive - minInclusive);
		}

		/// <summary>
		/// Gets the next random double.
		/// </summary>
		/// <param name="random">The random generator.</param>
		/// <param name="allowedRange">The range/interval for the generated double.</param>
		/// <returns>A random double in the required range.</returns>
		public static double NextDouble(this System.Random random, Interval allowedRange)
		{
			double correctValue;
			bool gotCorrectValue;
			do
			{
				correctValue = NextDoubleInternal(random, allowedRange.Minimum, allowedRange.Length);

				// 1. 'Is generated value within a range' check is missed intentionally.
				// 2. Even though upper bound is always exclusive, second check should stay here.
				gotCorrectValue =
					!((allowedRange.IsMinimumOpen && allowedRange.Minimum.IsEqual(correctValue)) ||
					  (allowedRange.IsMaximumOpen && allowedRange.Maximum.IsEqual(correctValue)));
			}
			while (!gotCorrectValue);

			return correctValue;
		}

		/// <summary>
		/// Generates an enumerable of uniform random integer numbers within specified range.
		/// </summary>
		/// <param name="random">The random number generator.</param>
		/// <param name="neededValuesNumber">The amount of values to be generated.</param>
		/// <param name="minInclusive">Lower bound, inclusive.</param>
		/// <param name="maxExclusive">Upper bound, exclusive.</param>
		/// <returns>An enumerable of random integer numbers.</returns>
		/// <exception cref="ArgumentNullException">if <paramref name="random"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="neededValuesNumber"/>
		/// is less than zero. Or if <paramref name="neededValuesNumber"/> &gt; <paramref name="maxExclusive"/> 
		/// - <paramref name="minInclusive"/>. Or if <paramref name="maxExclusive"/> is less or equal to
		/// <paramref name="minInclusive"/>.</exception>
		public static IEnumerable<int> NextInt32s(this System.Random random, int neededValuesNumber, int minInclusive, int maxExclusive)
		{
			ArgumentNullException.ThrowIfNull(random);
			ArgumentOutOfRangeException.ThrowIfNegative(neededValuesNumber);

			// maxExclusive - minInclusive > 0 is required to avoid overflow.
			if (neededValuesNumber > maxExclusive - minInclusive && maxExclusive - minInclusive > 0) 
			{
				throw new ArgumentOutOfRangeException(nameof(neededValuesNumber));
			}

			ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, minInclusive);

			int[] result = new int[neededValuesNumber];
			MathNet.Numerics.Random.RandomExtensions.NextInt32s(random, result, minInclusive, maxExclusive);

			return result;
		}

		/// <summary>
		/// Generates an enumerable of uniform random integer numbers within specified range, which are unique within this enumerable.
		/// </summary>
		/// <param name="random">The random number generator.</param>
		/// <param name="neededValuesNumber">The amount of values to be generated.</param>
		/// <param name="minInclusive">Lower bound, inclusive.</param>
		/// <param name="maxExclusive">Upper bound, exclusive.</param>
		/// <returns>An enumerable of random integer numbers, unique within this enumerable.</returns>
		/// <exception cref="ArgumentNullException">if <paramref name="random"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="neededValuesNumber"/>
		/// is less than zero. Or if <paramref name="neededValuesNumber"/> &gt; <paramref name="maxExclusive"/> 
		/// - <paramref name="minInclusive"/>. Or if <paramref name="maxExclusive"/> is less or equal to
		/// <paramref name="minInclusive"/>.</exception>
		/// <remarks>http://codereview.stackexchange.com/questions/61338/generate-random-numbers-without-repetitions</remarks>
		public static IEnumerable<int> NextUniqueInt32s(this System.Random random, int neededValuesNumber, int minInclusive, int maxExclusive)
		{
			ArgumentNullException.ThrowIfNull(random);
			ArgumentOutOfRangeException.ThrowIfNegative(neededValuesNumber);

			// maxExclusive - minInclusive > 0 is required to avoid overflow
			if (neededValuesNumber > maxExclusive - minInclusive && maxExclusive - minInclusive > 0) 
			{
				throw new ArgumentOutOfRangeException(nameof(neededValuesNumber));
			}

			ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxExclusive, minInclusive);

			ISet<int> uniqueNumbers = new HashSet<int>();
			for (int top = maxExclusive - neededValuesNumber; top < maxExclusive; ++top)
			{
				if (!uniqueNumbers.Add(random.Next(minInclusive, top + 1)))
				{
					uniqueNumbers.Add(top);
				}
			}

			IList<int> result = uniqueNumbers.ToList();
			for (int i = result.Count - 1; i > 0; i--)
			{
				int k = random.Next(i + 1);
				int temp = result[k];
				result[k] = result[i];
				result[i] = temp;
			}

			return result;
		}

		/// <summary>
		/// Returns a random boolean.
		/// </summary>
		/// <param name="random">The random number generator.</param>
		/// <returns>The next random boolean.</returns>
		/// <exception cref="System.ArgumentNullException"> if <paramref name="random"/>
		/// is <c>null</c>.</exception>
		public static bool NextBoolean(this System.Random random)
		{
			ArgumentNullException.ThrowIfNull(random);
			return MathNet.Numerics.Random.RandomExtensions.NextBoolean(random);
		}

		private static double NextDoubleInternal(System.Random random, double minInclusive, double intervalLength)
		{
			ArgumentNullException.ThrowIfNull(random);

			if (double.IsNaN(minInclusive) || double.IsInfinity(minInclusive))
			{
				throw new ArgumentOutOfRangeException(nameof(minInclusive));
			}

			if (double.IsNaN(intervalLength) || double.IsInfinity(intervalLength))
			{
				throw new ArgumentOutOfRangeException(nameof(intervalLength));
			}

			return minInclusive + (random.NextDouble() * intervalLength);
		}
	}
}