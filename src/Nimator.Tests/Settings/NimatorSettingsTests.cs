using System;
using System.Linq;
using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture]
    public class NimatorSettingsTests
    {
        [Test]
        public void FromJson_ForBasicJsonString_ReturnsBasicSettings()
        {
            var json = "{}";
            var sut = NimatorSettings.FromJson(json);
            Assert.That(sut, Is.Not.Null);
            CollectionAssert.IsEmpty(sut.Notifiers);
        }

        [TestCase(typeof(ConsoleSettings))]
        [TestCase(typeof(OpsGenieSettings))]
        [TestCase(typeof(SlackSettings))]
        public void GetExample_WhenCalled_HasNotifierOfType(Type expectedType)
        {
            var sut = NimatorSettings.GetExample();
            Assert.That(sut.Notifiers.Any(n => n.GetType() == expectedType));
        }

        [Test]
        public void GetExample_WhenCalled_HasMultipleLayers()
        {
            var sut = NimatorSettings.GetExample();
            Assert.That(sut.Layers.Length, Is.GreaterThan(1));
        }

        [Test]
        public void GetExample_WhenCalled_HasAtLeastOneLayerWithMultipleChecks()
        {
            var sut = NimatorSettings.GetExample();
            Assert.That(sut.Layers.Any(l => l.Checks.Length > 1));
        }

        [Test]
        public void GetExample_WhenCalled_GivesLayersWithDistinctNames()
        {
            var sut = NimatorSettings.GetExample();
            var names = sut.Layers.Select(l => l.Name);
            CollectionAssert.AllItemsAreUnique(names);
        }

        [Test]
        public void JsonDeserialization_WhenGivenBasicJson_CanInstantiateSettings()
        {
            var json = "{ \"Notifiers\": [] }";
            var settings = NimatorSettings.FromJson(json);
            Assert.That(settings, Is.Not.Null);
            Assert.That(settings.Notifiers, Is.Empty);
        }

        [Test]
        public void JsonDeserialization_WhenGivenNameOfEnum_CanInstantiateSettings()
        {
            var json = @"{ ""Notifiers"": [{  
                ""$type"": ""Nimator.Settings.ConsoleSettings, Nimator"",
                ""Threshold"": ""Critical""
            }] }";
            var settings = NimatorSettings.FromJson(json);
            Assert.That(settings.Notifiers, Is.Not.Empty);
            Assert.That(settings.Notifiers.Single().Threshold, Is.EqualTo(NotificationLevel.Critical));
        }

        [TestCase(NotificationLevel.Error, "Nimator.Settings.SlackSettings, Nimator")]
        [TestCase(NotificationLevel.Error, "Nimator.Settings.OpsGenieSettings, Nimator")]
        [TestCase(NotificationLevel.Okay, "Nimator.Settings.ConsoleSettings, Nimator")]
        public void JsonDeserialization_WhenGivenNoThreshold_InitializesToErrorByDefault(NotificationLevel expectedThreshold, string type)
        {
            var json = "{ \"Notifiers\": [{ \"$type\": \" " + type + "\" }] }";
            var settings = NimatorSettings.FromJson(json);
            Assert.That(settings.Notifiers, Is.Not.Empty);
            Assert.That(settings.Notifiers.Single().Threshold, Is.EqualTo(expectedThreshold));
        }

        [Test]
        public void Serialization_WhenGivenDefaultExample_CanDoRoundTrip()
        {
            var settings = NimatorSettings.GetExample();

            var json = settings.ToJson();
            var roundTripSettings = NimatorSettings.FromJson(json);
            var roundTripJson = roundTripSettings.ToJson();

            Assert.That(json, Is.EqualTo(roundTripJson));
        }

        [Test]
        public void Serialization_WhenGivenDataWithEnum_RendersEnumAsString()
        {
            var settings = new NimatorSettings
            {
                Layers = new LayerSettings[]
                {
                    new LayerSettings
                    {
                        Name = "Layer 1",
                        Checks = new ICheckSettings[]
                        {
                            new NoopCheckSettings { LevelToSimulate = NotificationLevel.Warning },
                        },
                    },
                },
            };

            var json = settings.ToJson();

            Assert.That(json, Does.Contain("Warning"), "Expected readable representation of enum value");
        }

        [Test]
        public void Serialization_WhenGivenCustomExample_CanDoRoundTrip()
        {
            var settings = new NimatorSettings
            {
                Notifiers = new NotifierSettings[]
                {
                    new OpsGenieSettings
                    {
                        Threshold = NotificationLevel.Error,
                        ApiKey = "abc-123",
                        HeartbeatName = "heartbeat",
                        TeamName = "A-Team"
                    },
                    new SlackSettings
                    {
                        Threshold = NotificationLevel.Warning,
                        Url = "http://www.cp.nl/dummy/url",
                    },
                    new ConsoleSettings
                    {
                        Threshold = NotificationLevel.Warning,
                    }
                },
                Layers = new LayerSettings[]
                {
                    new LayerSettings {
                        Name = "Layer A",
                        Checks = new ICheckSettings[]
                        {
                            new NoopCheckSettings
                            {
                                DelayResultInMs = 500,
                                LevelToSimulate = NotificationLevel.Error,
                            },
                        }
                    }
                },
            };

            var json = settings.ToJson();
            var roundTripSettings = NimatorSettings.FromJson(json);
            var roundTripJson = roundTripSettings.ToJson();

            Assert.That(json, Is.EqualTo(roundTripJson));
        }
    }
}
