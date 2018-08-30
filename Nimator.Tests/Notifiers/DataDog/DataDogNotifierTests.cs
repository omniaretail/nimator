using Nimator.Settings;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Nimator.Notifiers.DataDog
{
    public class DataDogNotifierTests
    {
        private const string AlertTypeComparisonErrorMessage = "Alert type didn't match expected value."+
                                                               " Alert type has to be lower case, otherwise not interpreted correctly (DataDog agent v.6.x)";
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
        public void Notify_NimatorResultErrorWithOneErrorCheck_DataDogNotifyOneEventWithTags()
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
            sut.Events[0].Title.ShouldBe("C1");
            sut.Events[0].AlertType.ShouldBe(AlertType.Error, AlertTypeComparisonErrorMessage);
            sut.Events[0].Message.ShouldContain(message);
            var tags = sut.Events[0].Tags;
            tags.ShouldNotBeNull();
            tags.ShouldContain("check:c1");
            tags.ShouldContain("layer:l1");
            tags.ShouldContain("level:error");
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
            sut.Events[0].Title.ShouldBe("C1");
            sut.Events[0].AlertType.ShouldBe(AlertType.Error, AlertTypeComparisonErrorMessage);
            sut.Events[0].Message.ShouldContain(messageError);
            sut.Events[1].StatName.ShouldBe(DataDogEventConverter.MetricsName);
            sut.Events[1].Title.ShouldBe("C3");
            sut.Events[1].AlertType.ShouldBe(AlertType.Error, AlertTypeComparisonErrorMessage);
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
            sut.Events[0].AlertType.ShouldBe(AlertType.Error, AlertTypeComparisonErrorMessage);
            sut.Events[1].AlertType.ShouldBe(AlertType.Error, AlertTypeComparisonErrorMessage);
            sut.Events[2].AlertType.ShouldBe(AlertType.Warning, AlertTypeComparisonErrorMessage);
            sut.Events[3].AlertType.ShouldBe(AlertType.Info, AlertTypeComparisonErrorMessage);
        }

        private DataDogNotifierTestDouble GetSut()
        {
            return GetSut(settings);
        }

        private DataDogNotifierTestDouble GetSut(DataDogSettings dataDogSettings)
        {
            var dataDogEventConverter = new DataDogEventConverter(dataDogSettings);
            return new DataDogNotifierTestDouble(dataDogEventConverter, dataDogSettings);
        }

        private class DataDogNotifierTestDouble : DataDogNotifier
        {
            public List<DataDogEvent> Events = new List<DataDogEvent>();

            public DataDogNotifierTestDouble(IDataDogEventConverter dataDogEventConverter, DataDogSettings settings) : base(dataDogEventConverter, settings)
            {
            }

            protected override void NotifyDataDogEvent(DataDogEvent dataDogEvent)
            {
                Events.Add(dataDogEvent);
            }
        }
    }
}
