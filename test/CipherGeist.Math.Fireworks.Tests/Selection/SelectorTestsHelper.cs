namespace CipherGeist.Math.Fireworks.Tests.Selection;

public static class SelectorTestsHelper
{
	public static IEnumerable<Firework> Fireworks { get; set; } = [];

	public static IEnumerable<Firework> NearBestFireworks
	{
		get
		{
			return Fireworks.Skip(1).Take(SamplingNumber);
		}
	}

	public static IEnumerable<Firework> NonNearBestFirework
	{
		get
		{
			return Fireworks.Reverse().Take(SamplingNumber);
		}
	}

	public static IEnumerable<Firework> BestFireworks
	{
		get
		{
			return Fireworks.OrderBy(fr => fr.Quality).Take(SamplingNumber);
		}
	}

	public static IEnumerable<Firework> NonBestFireworks
	{
		get
		{
			return Fireworks.OrderByDescending(fr => fr.Quality).Take(SamplingNumber);
		}
	}

	public static IEnumerable<Firework> RandomFireworks
	{
		get
		{
			return Fireworks.Take(SamplingNumber);
		}
	}

	public static Firework FirstBestFirework
	{
		get
		{
			return Fireworks.OrderBy(fr => fr.Quality).First();
		}
	}

	public static int SamplingNumber { get; set; }

	public static int CountFireworks { get; set; }

	static SelectorTestsHelper()
	{
		SamplingNumber = 3;
		CountFireworks = 10;
		FormFireworks();
	}

	public static Firework GetBest(IEnumerable<Firework> fireworks)
	{
		return fireworks.OrderBy(fr => fr.Quality).First();
	}

	// #TODO: lazy initialization collection of fireworks.
	private static void FormFireworks()
	{
		var range = new Interval(0, 10.0);
		var fireworks = new List<Firework>();
		IDictionary<Dimension, double> coordinates;

		for (int i = 1; i < CountFireworks + 1; i++)
		{
			coordinates = new Dictionary<Dimension, double>
			{
				{ new Dimension(range), i },
				{ new Dimension(range), i }
			};

			var firework = new Firework(FireworkType.Initial, 0, 0, coordinates)
			{
				Quality = i
			};

			fireworks.Add(firework);
		}
		Fireworks = fireworks;
	}
}