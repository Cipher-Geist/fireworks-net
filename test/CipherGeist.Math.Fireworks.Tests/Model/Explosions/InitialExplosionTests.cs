namespace CipherGeist.Math.Fireworks.Tests.Model.Explosions;

public class InitialExplosionTests
{
    [Test]
    public void InitialExplosion_NegativeAs1stParam_ExceptionThrown()
    {
        int initialSparksNumber = -1;
        string expectedParamName = "initialSparksNumber";

        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new InitialExplosion(initialSparksNumber));

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo(expectedParamName));
    }
}