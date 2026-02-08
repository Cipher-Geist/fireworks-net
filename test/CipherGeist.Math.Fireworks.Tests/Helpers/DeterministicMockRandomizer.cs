namespace CipherGeist.Math.Fireworks.Tests.Helpers;

public class DeterministicMockRandomizer : IRandomizer
{
	private int seed;
	private System.Random deterministicGenerator;

	public DeterministicMockRandomizer(int seed = 0)
	{
		this.seed = seed;
		deterministicGenerator = new System.Random(seed);
	}

	public int Next()
	{
		return deterministicGenerator.Next();
	}

	public int Next(int maxValue)
	{
		return deterministicGenerator.Next(maxValue);
	}

	public int Next(int minValue, int maxValue)
	{
		return deterministicGenerator.Next(minValue, maxValue);
	}

	public void NextBytes(byte[] buffer)
	{
		deterministicGenerator.NextBytes(buffer);
	}

	public double NextDouble()
	{
		return deterministicGenerator.NextDouble();
	}

	public RandomizerType RandomizerType
	{
		get { return RandomizerType.Default; }
	}
}
