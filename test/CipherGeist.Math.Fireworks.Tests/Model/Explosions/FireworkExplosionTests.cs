namespace CipherGeist.Math.Fireworks.Tests.Model.Explosions;

public class FireworkExplosionTests
{
    [Test]
    public void FirewordExplosionNaNAs3tdParamExceptionThrown()
    {
        var parent = new Firework(FireworkType.Initial, 1, 0);
        int stepNumber = 1;
        var amplidute = double.NaN;
        var sparkCounts = new Dictionary<FireworkType, int>();
        var expectedParamName = "amplitude";

        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new FireworkExplosion(parent, stepNumber, amplidute, sparkCounts));

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
    }

    [Test]
    public void FirewordExplosionInfinityAs3tdParamExceptionThrownS()
    {
        var parent = new Firework(FireworkType.Initial, 1, 0);

        int stepNumber = 1;
        var amplidute = double.PositiveInfinity;

        var sparkCounts = new Dictionary<FireworkType, int>();
        var expectedParamName = "amplitude";

        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new FireworkExplosion(parent, stepNumber, amplidute, sparkCounts));

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
    }
}