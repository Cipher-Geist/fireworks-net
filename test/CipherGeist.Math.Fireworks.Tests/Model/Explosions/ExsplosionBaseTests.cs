namespace CipherGeist.Math.Fireworks.Tests.Model.Explosions;

public class ExplosionBaseTests
{
    private class TestExplosionBase : ExplosionBase
    {
        public TestExplosionBase(int stepNumber, IDictionary<FireworkType, int> sparkCounts)
            : base(stepNumber, sparkCounts)
        {
        }
    }

    [Test]
    public void ExplosionBaseNegativeAs1stParamExceptionThrown()
    {
        int stepNumber = -1;
        var sparkCounts = new Dictionary<FireworkType, int>();
        var expectedParamName = "stepNumber";

        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new TestExplosionBase(stepNumber, sparkCounts));

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
    }
}