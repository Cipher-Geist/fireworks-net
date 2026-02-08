namespace CipherGeist.Math.Fireworks.Tests.Solving;

public class SolverTests
{
	private readonly Solver _solver;
	private readonly Func<double, double> _targetFunc;
	private readonly Interval _range;

	public SolverTests()
	{
		_solver = new Solver();
		_targetFunc = x => x + x;
		_range = new Interval(-5, 5);
	}

	[Test]
	public void SolvePresentAllParamReturnsEqualRoot()
	{
		double expectedRoot = 0;
		double actualRoot = _solver.Solve(_targetFunc, _range);
		Assert.That(actualRoot, Is.EqualTo(expectedRoot));
	}

	[Test]
	public void SolvePresentAllParamReturnsNonEqualRoot()
	{
		double expectedRoot = 1;
		double actualRoot = _solver.Solve(_targetFunc, _range);
		Assert.That(actualRoot, Is.Not.EqualTo(expectedRoot));
	}
}