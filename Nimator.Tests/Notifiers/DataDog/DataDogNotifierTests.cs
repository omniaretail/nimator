using Nimator.Settings;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Nimator.Notifiers.DataDog
{
    public class DataDogNotifierTests
    {
        private DataDogSettings settings;

        [SetUp]
        public void Setup()
        {
            settings = new DataDogSettings
            {
                Threshold = NotificationLevel.Error
            };
        }

        [Test]
        public void Notify_NimatorResultIsNull_ThrowsException()
        {
            Should.Throw<ArgumentNullException>(() => GetSut().Notify(result: null));
        }

        [Test]
        public void Notify_NimatorResultIsEmpty_NoDataDogNotify()
        {
            // Arrange
            var sut = GetSut();

            // Act
            sut.Notify(result: new NimatorResult(DateTime.Now));

            // Assert
            sut.Events.ShouldBeEmpty();
        }

        [Test]
        public void Notify_CriticalNimatorResult_DataDogNotifyOneEvent()
        {
            // Arrange
            var msg = "result message";
            var result = new CriticalNimatorResult(message: msg, fullText: null);
            var sut = GetSut();

            // Act
            sut.Notify(result);

            // Assert
            sut.Events.Count.ShouldBe(1);
            sut.Events[0].Message.ShouldContain(msg);
        }

        [Test]
        public void Notify_NimatorResultOk_NoDataDogNotify()
        {
            // Arrange
            var result = new NimatorResult(DateTime.Now);
            var sut = GetSut();

            // Act
            sut.Notify(result);

            // Assert
            sut.Events.ShouldBeEmpty();
        }

        [Test]
        public void Notify_NimatorResultErrorWithOneErrorCheck_DataDogNotifyOneEvent()
        {
            // Arrange
            var result = new NimatorResult(DateTime.Now);
            var message = "error message";
            var checkResults = new List<CheckResult>
            {
                new CheckResult("C1", NotificationLevel.Error, message)
            };
            result.LayerResults.Add(new LayerResult("L1", checkResults));
            var sut = GetSut();

            // Act
            sut.Notify(result);

            // Assert
            sut.Events.Count.ShouldBe(1);
            sut.Events[0].StatName.ShouldBe(DataDogEventConverter.MetricsName);
            sut.Events[0].CheckName.ShouldBe("C1");
            sut.Events[0].LayerName.ShouldBe("L1");
            sut.Events[0].AlertType.ShouldBe("Error");
            sut.Events[0].Level.ShouldBe("Error");
            sut.Events[0].Message.ShouldContain(message);
        }

        [Test]
        public void Notify_NimatorResultErrorWithSeveralErrorChecks_DataDogNotifyErrorsOnly()
        {
            // Arrange
            var result = new NimatorResult(DateTime.Now);
            var messageError = "error message";
            var messageCritical = "critical message";
            var checkResults = new List<CheckResult>
            {
                new CheckResult("C1", NotificationLevel.Error, messageError),
                new CheckResult("C2", NotificationLevel.Okay),
                new CheckResult("C3", NotificationLevel.Critical, messageCritical)
            };
            result.LayerResults.Add(new LayerResult("L1", checkResults));
            var sut = GetSut();

            // Act
            sut.Notify(result);

            // Assert
            sut.Events.Count.ShouldBe(2);
            sut.Events[0].StatName.ShouldBe(DataDogEventConverter.MetricsName);
            sut.Events[0].CheckName.ShouldBe("C1");
            sut.Events[0].LayerName.ShouldBe("L1");
            sut.Events[0].AlertType.ShouldBe("Error");
            sut.Events[0].Level.ShouldBe("Error");
            sut.Events[0].Message.ShouldContain(messageError);
            sut.Events[1].StatName.ShouldBe(DataDogEventConverter.MetricsName);
            sut.Events[1].CheckName.ShouldBe("C3");
            sut.Events[1].LayerName.ShouldBe("L1");
            sut.Events[1].Level.ShouldBe("Critical");
            sut.Events[1].Message.ShouldContain(messageCritical);
        }

        [Test]
        public void Notify_NimatorResultErrorWithAllTypeOfChecksAndMinThreshold_DataDogNotifyAll()
        {
            // Arrange
            var result = new NimatorResult(DateTime.Now);
            var message = "error message";
            var checkResults = new List<CheckResult>
            {
                new CheckResult("C1", NotificationLevel.Critical, message),
                new CheckResult("C2", NotificationLevel.Error, message),
                new CheckResult("C3", NotificationLevel.Warning, message),
                new CheckResult("C4", NotificationLevel.Okay, message)
            };
            result.LayerResults.Add(new LayerResult("L1", checkResults));

            var settings = new DataDogSettings
            {
                Threshold = NotificationLevel.Okay
            };
            var sut = GetSut(settings);

            // Act
            sut.Notify(result);

            // Assert
            sut.Events.Count.ShouldBe(4);
            sut.Events[0].AlertType.ShouldBe("Error");
            sut.Events[1].AlertType.ShouldBe("Error");
            sut.Events[2].AlertType.ShouldBe("Warning");
            sut.Events[3].AlertType.ShouldBe("Info");
        }

        private DataDogNotifierTestDouble GetSut()
        {
            return GetSut(settings);
        }

        private DataDogNotifierTestDouble GetSut(DataDogSettings dataDogSettings)
        {
            var dataDogEventConverter = new DataDogEventConverter(dataDogSettings);
            return new DataDogNotifierTestDouble(dataDogEventConverter);
        }

        private class DataDogNotifierTestDouble : DataDogNotifier
        {
            public List<DataDogEvent> Events = new List<DataDogEvent>();

            public DataDogNotifierTestDouble(IDataDogEventConverter dataDogEventConverter) : base(dataDogEventConverter)
            {
            }

            protected override void NotifyDataDog(DataDogEvent dataDogEvent)
            {
                Events.Add(dataDogEvent);
            }
        }
    }
}
