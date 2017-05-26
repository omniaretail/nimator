using System;
using FluentAssertions;
using log4net;
using NSubstitute;
using NUnit.Framework;

namespace Nimator.CouchDb.Tests
{
    [TestFixture]
    public class CouchNimatorTests
    {
        [Test]
        public void FromSetting_WhenLoggerIsNull_ArgumentNullException()
        {
            //Arrange
            var monitorSettings = new CouchDbMonitoringSettings();
            
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => CouchNimator.FromSettings(null, monitorSettings));
        }

        [Test]
        public void FromSetting_WhenCouchDbMonitoringSettingsIsNull_ArgumentNullException()
        {
            //Arrange
            var logger = Substitute.For<ILog>();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => CouchNimator.FromSettings(logger, null));
        }

        [Test]
        public void FromSetting_WhenWithEmptySettings_LogsWarning()
        {
            //Arrange
            var monitorSettings = new CouchDbMonitoringSettings
            {
                ConnectionString = string.Empty,
                Bucket = string.Empty,
                Credentials = string.Empty
            };
            var logger = Substitute.For<ILog>();

            //Act
            CouchNimator.FromSettings(logger, monitorSettings);

            //Assert
            logger.Received().Warn(Arg.Is<string>(s => s != null && s.ToLowerInvariant().Contains("notifiers")));
        }
    }
}
