using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Nimator.CouchDb.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace Nimator.CouchDb.Tests
{
    [TestFixture]
    public class BucketRecordsCheckTests
    {
        [Test]
        public void Constructing_WhenBucketRecordsCheckSettingsIsNull_ArgumentNullExceptionThrown()
        {
            //Arrange
            var couchDbService = Substitute.For<ICouchDbService>();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => new BucketRecordsCheck(null, couchDbService));
        }

        [Test]
        public void Constructing_WhenICouchDbServiceIsNull_ArgumentNullExceptionThrown()
        {
            //Arrange
            var settings = new BucketRecordsCheckSettings(string.Empty ,string.Empty, string.Empty);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => new BucketRecordsCheck(settings, null));
        }

        [Test]
        public void RunAsync_WhenBucketInformationIsNull_NotificationLevelError()
        {
            //Arrange
            var couchDbService = Substitute.For<ICouchDbService>();
            var settings = new BucketRecordsCheckSettings(string.Empty, string.Empty, string.Empty);
            var SUT = new BucketRecordsCheck(settings, couchDbService);

            //Act
            var result = SUT.RunAsync().Result;

            //Assert
            result.Level.Should().Be(NotificationLevel.Error);
        }

        [TestCase(0, NotificationLevel.Okay)]
        [TestCase(20000, NotificationLevel.Okay)]
        [TestCase(120000, NotificationLevel.Warning)]
        public void RunAsync_WhenBucketInformationWithRecords_ReturnsNotificationLevel(int records, NotificationLevel notification)
        {
            //Arrange
            var couchDbService = Substitute.For<ICouchDbService>();
            couchDbService.GetBucketInformation(string.Empty)
                .Returns(x => Task.FromResult(new BucketInformation
                {
                    basicStats = new BasicStats
                    {
                        itemCount = records
                    }
                }));

            var settings = new BucketRecordsCheckSettings(string.Empty, string.Empty, string.Empty);
            var SUT = new BucketRecordsCheck(settings, couchDbService);

            //Act
            var result = SUT.RunAsync().Result;

            //Assert
            result.Level.Should().Be(notification);
        }
    }
}
