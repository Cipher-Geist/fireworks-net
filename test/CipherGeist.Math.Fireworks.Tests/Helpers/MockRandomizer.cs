namespace CipherGeist.Math.Fireworks.Tests.Helpers;

public class MockRandomizer : IRandomizer
{
	private Queue<int> _numbers;
	private Queue<double> _doubles;
	private RandomizerType _randomizerType;

	public MockRandomizer(
		IEnumerable<int> numbers, 
		IEnumerable<double> doubles, 
		RandomizerType randomizerType = RandomizerType.MersenneTwister)
	{
		_numbers = new Queue<int>(numbers);
		_doubles = new Queue<double>(doubles);
		_randomizerType = randomizerType;
	}

	public int Next()
	{
		return _numbers.Dequeue();
	}

	public int Next(int maxValue)
	{
		int next = _numbers.Dequeue();
		return next % maxValue;
	}

	public int Next(int minValue, int maxValue)
	{
		int next = _numbers.Dequeue();
		return minValue + (next % (maxValue - minValue));
	}

	public void NextBytes(byte[] buffer)
	{
		for (int i = 0; i < buffer.Length; i++)
		{
			if (_numbers.Count > 0)
			{
				buffer[i] = (byte)(_numbers.Dequeue() % 256);
			}
			else
			{
				throw new InvalidOperationException("Not enough numbers in queue to fill the byte array.");
			}
		}
	}

	public double NextDouble()
	{
		return _doubles.Dequeue();
	}

	public RandomizerType RandomizerType
	{
		get { return _randomizerType; }
	}
}
