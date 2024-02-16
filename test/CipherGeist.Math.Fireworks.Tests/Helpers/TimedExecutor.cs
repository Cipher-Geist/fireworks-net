using System.Diagnostics;

namespace CipherGeist.Math.Fireworks.Tests.Helpers;

public class TimedExecutor
{
	public T Execute<T>(Func<T> func)
	{
		Stopwatch = Stopwatch.StartNew();
		try
		{
			return func();
		}
		finally
		{
			Stopwatch.Stop();
			Console.WriteLine(
				$"Elapsed = {Stopwatch.Elapsed.TotalMinutes:00}:" +
				$"{Stopwatch.Elapsed.Seconds:00}:{Stopwatch.Elapsed.Milliseconds:000}");
		}
	}

	public Stopwatch? Stopwatch { get; private set; }
}
