namespace CipherGeist.Math.Fireworks.Tests.Model.Fireworks;

public class FireworkTests
{
    [Test]
    public void ConstructorNegativeBirthStepNumberExceptionThrown()
    {
        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            new Firework(FireworkType.Initial, -1, 0, new Dictionary<Dimension, double>());
        });

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo("birthStepNumber"));
    }

    [Test]
    public void ConstructorNegativeBirthOrderExceptionThrown()
    {
        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            new Firework(FireworkType.Initial, 0, -1, new Dictionary<Dimension, double>());
        });

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo("birthOrder"));
    }

    [Test]
    public void ConstructorValidFireworkTypeSetsPassedFireworkType()
    {
        var expectedType = FireworkType.Initial;
        var result = new Firework(expectedType, 0, 0, new Dictionary<Dimension, double>());

        Assert.That(result.FireworkType, Is.EqualTo(expectedType));
    }

    [Test]
    public void ConstructorValidBirthStepNumberSetsPassedBirthStepNumber()
    {
        int expectedBirthStepNumber = 10;
        var result = new Firework(FireworkType.Initial, expectedBirthStepNumber, 0, new Dictionary<Dimension, double>());

        Assert.That(result.BirthStepNumber, Is.EqualTo(expectedBirthStepNumber));
    }

    [Test]
    public void ConstructorValidBirthOrderSetsPassedBirthOrder()
    {
        int expectedBirthOrder = 2;
        var result = new Firework(FireworkType.Initial, 0, expectedBirthOrder, new Dictionary<Dimension, double>());

        Assert.That(result.BirthOrder, Is.EqualTo(expectedBirthOrder));
    }

    [Test]
    public void ConstructorValidCoordinatesSetsPassedCoordinates()
    {
        var expectedCoordinates = new Dictionary<Dimension, double>
        {
            { new Dimension(new Interval(-10.0, 20.0)), 10.0 },
            { new Dimension(new Interval(50.0, 120.0)), 67.85 }
        };

        var result = new Firework(FireworkType.Initial, 0, 0, expectedCoordinates);

        Assert.That(result.Coordinates, Is.EqualTo(expectedCoordinates));
    }

    [Test]
    public void Constructor2NegativeBirthStepNumberExceptionThrown()
    {
        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, -1, 0));

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo("birthStepNumber"));
    }

    [Test]
    public void Constructor2NegativeBirthOrderExceptionThrown()
    {
        var actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new Firework(FireworkType.Initial, 0, -1));

        Assert.That(actualException, Is.Not.Null);
        Assert.That(actualException.ParamName, Is.EqualTo("birthOrder"));
    }

    [Test]
    public void Constructor2ValidFireworkTypeSetsPassedFireworkType()
    {
        var expectedType = FireworkType.Initial;
        var result = new Firework(expectedType, 0, 0);

        Assert.That(result.FireworkType, Is.EqualTo(expectedType));
    }

    [Test]
    public void Constructor2ValidBirthStepNumberSetsPassedBirthStepNumber()
    {
        int expectedBirthStepNumber = 10;
        var result = new Firework(FireworkType.Initial, expectedBirthStepNumber, 0);

        Assert.That(result.BirthStepNumber, Is.EqualTo(expectedBirthStepNumber));
    }

    [Test]
    public void Constructor2ValidBirthOrderSetsPassedBirthOrder()
    {
        int expectedBirthOrder = 2;
        var result = new Firework(FireworkType.Initial, 0, expectedBirthOrder);

        Assert.That(result.BirthOrder, Is.EqualTo(expectedBirthOrder));
    }

    [Test]
    public void Constructor2ValidArgumentsSetsEmptyCoordinates()
    {
        var result = new Firework(FireworkType.Initial, 0, 0);

        Assert.That(result.Coordinates, Is.Not.Null);
        Assert.That(result.Coordinates.Count, Is.EqualTo(0));
    }

    [Test]
    public void LabelValidFireworkGetsValidLabel()
    {
        var result = new Firework(FireworkType.ExplosionSpark, 2, 17);
        string label = result.Label;

        Assert.That(label, Is.EqualTo("2.ExplosionSpark.17"));
    }
}
