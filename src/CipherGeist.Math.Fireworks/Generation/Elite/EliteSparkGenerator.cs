using Interval = CipherGeist.Math.Fireworks.Model.Interval;

namespace CipherGeist.Math.Fireworks.Generation.Elite;

/// <summary>
/// Elite strategy spark generator, as described in 2012 paper.
/// </summary>
public abstract class EliteSparkGenerator : SparkGeneratorBase<EliteExplosion>
{
	private readonly IEnumerable<Dimension> _dimensions;
	private readonly IFit _polynomialFit;

	/// <summary>
	/// Initializes a new instance of the <see cref="EliteSparkGenerator"/> class.
	/// </summary>
	/// <param name="dimensions">The dimensions to fit generated sparks into.</param>
	/// <param name="polynomialFit">The polynomial fit.</param>
	protected EliteSparkGenerator(IEnumerable<Dimension> dimensions, IFit polynomialFit)
	{
		_dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
		_polynomialFit = polynomialFit ?? throw new ArgumentNullException(nameof(polynomialFit));
	}

	/// <summary>
	/// Calculates elite point.
	/// </summary>
	/// <param name="func">The function to calculate elite point.</param>
	/// <param name="variationRange">Represents an interval to calculate 
	/// elite point.</param>
	/// <returns>The coordinate of elite point.</returns>
	protected abstract double CalculateElitePoint(Func<double, double> func, Interval variationRange);

	/// <summary>
	/// Creates the spark from the explosion.
	/// </summary>
	/// <param name="explosion">The explosion that contains the collection of sparks.</param>
	/// <param name="birthOrder">The number of spark in the collection of sparks born by
	/// this generator within one step.</param>
	/// <returns>A spark for the specified explosion.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="explosion"/> is <c>null</c>.</exception>
	/// <exception cref="ArgumentOutOfRangeException"> if <paramref name="birthOrder"/> is less than zero.</exception>
	public override Firework CreateSpark(EliteExplosion explosion, int birthOrder)
	{
		ArgumentNullException.ThrowIfNull(explosion);
		ArgumentOutOfRangeException.ThrowIfNegative(birthOrder);

		var fitnessLandscapes = ApproximateFitnessLandscapes(explosion.Fireworks);
		var elitePointCoordinates = new Dictionary<Dimension, double>();

		foreach (KeyValuePair<Dimension, Func<double, double>> fitnessLandscape in fitnessLandscapes)
		{
			double coordinate = CalculateElitePoint(fitnessLandscape.Value, fitnessLandscape.Key.Range);
			elitePointCoordinates[fitnessLandscape.Key] = coordinate;
		}

		return new Firework(GeneratedSparkType, explosion.StepNumber, birthOrder, elitePointCoordinates);
	}

	/// <summary>
	/// Approximates fitness landscape in each one dimensional search space.
	/// </summary>
	/// <param name="fireworks">The collection of <see cref="Firework"/>s to obtain
	/// coordinates in each one dimensional search space.</param>
	/// <returns>A map. Key is a <see cref="Dimension"/>. Value is a approximated
	/// curve by coordinates and qualities of <see cref="Firework"/>s.</returns>
	/// <exception cref="ArgumentNullException"> if <paramref name="fireworks"/> is <c>null</c>.</exception>
	protected virtual IDictionary<Dimension, Func<double, double>> ApproximateFitnessLandscapes(IEnumerable<Firework> fireworks)
	{
		ArgumentNullException.ThrowIfNull(fireworks);

		var currentFireworks = new List<Firework>(fireworks);
		double[] qualities = new double[currentFireworks.Count];

		int current = 0;
		foreach (Firework firework in currentFireworks)
		{
			qualities[current] = firework.Quality;
			current++;
		}

		var fitnessLandscapes = new Dictionary<Dimension, Func<double, double>>();
		foreach (var dimension in _dimensions)
		{
			current = 0;
			double[] coordinates = new double[currentFireworks.Count];

			foreach (var firework in currentFireworks)
			{
				coordinates[current] = firework.Coordinates[dimension];
				current++;
			}

			fitnessLandscapes[dimension] = _polynomialFit.Approximate(coordinates, qualities);
		}

		return fitnessLandscapes;
	}

	/// <summary>
	/// Gets the type of the generated spark.
	/// </summary>
	public override FireworkType GeneratedSparkType
	{
		get { return FireworkType.EliteFirework; }
	}
}