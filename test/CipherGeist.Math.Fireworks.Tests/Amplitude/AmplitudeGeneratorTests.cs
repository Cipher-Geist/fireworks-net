using Moq;
using CipherGeist.Math.Fireworks.Amplitude;

namespace CipherGeist.Math.Fireworks.Tests.Amplitude;

[TestFixture]
public class AmplitudeGeneratorTests
{
	private Mock<IExtremumFireworkSelector> _mockSelector;
	private ExploderSettings _settings;

	[SetUp]
	public void Setup()
	{
		_mockSelector = new Mock<IExtremumFireworkSelector>();
		_settings = new ExploderSettings
		{
			ExplosionSparksMaximumAmplitude = 10.0
		};
	}

	[Test]
	public void CalculateAmplitudeBase_WithValidInput_ReturnsExpectedValue()
	{
		var fireworks = new List<Firework>
		{
			new (FireworkType.Initial, 0, 1) { Quality = 5 },
			new (FireworkType.Initial, 0, 2) { Quality = 7 },
			new (FireworkType.Initial, 0, 3) { Quality = 9 },
		};
		var focusFirework = new Firework(FireworkType.Initial, 0, 4) { Quality = 8 };
		_mockSelector.Setup(s => s.SelectBest(It.IsAny<IEnumerable<Firework>>())).Returns(fireworks[2]); // Best quality is 9

		var generator = new AmplitudeGenerator(_mockSelector.Object, _settings);

		var amplitude = generator.CalculateAmplitudeBase(focusFirework, fireworks, 1);

		Assert.That(amplitude, Is.EqualTo(1.0 + (2.0 / 3.0)).Within(0.0000001));
	}

	[TestCase(double.NaN)]
	[TestCase(double.PositiveInfinity)]
	public void CalculateAmplitudeBase_WhenBestQualityIsInfinite_ThrowsArgumentOutOfRangeException(double bestFireworkQuality)
	{
		var fireworks = new List<Firework>
		{
			new (FireworkType.Initial, 0, 1) { Quality = double.PositiveInfinity }
		};
		var focusFirework = new Firework(FireworkType.Initial, 0, 2) { Quality = 8 };

		_mockSelector.Setup(s => s.SelectBest(It.IsAny<IEnumerable<Firework>>())).Returns(fireworks[0]);

		var generator = new AmplitudeGenerator(_mockSelector.Object, _settings);

		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			generator.CalculateAmplitudeBase(focusFirework, fireworks, 1);
		});
	}
}
