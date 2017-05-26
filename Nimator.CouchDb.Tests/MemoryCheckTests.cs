using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Nimator.CouchDb.Models;
using NSubstitute;
using NUnit.Framework;

namespace Nimator.CouchDb.Tests
{
    [TestFixture]
    public class MemoryCheckTests
    {
        [Test]
        public void Constructing_WhenSettingsIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var couchDbService = Substitute.For<ICouchDbService>();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => new MemoryCheck(null, couchDbService));
        }

        [Test]
        public void Constructing_WhenCouchDbServiceIsNull_ThrowArgumentNullException()
        {
            //Arrange
            var checkSettings = new MemoryCheckSettings(string.Empty, string.Empty);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => new MemoryCheck(checkSettings, null));
        }

        [Test]
        public void RunAsync_WhenNodeInformationIsNull_NotificationLevelError()
        {
            //Arrange
            var couchDbService = Substitute.For<ICouchDbService>();
            var settings = new MemoryCheckSettings(string.Empty, string.Empty);
            var SUT = new MemoryCheck(settings, couchDbService);

            //Act
            var result = SUT.RunAsync().Result;

            //Assert
            result.Level.Should().Be(NotificationLevel.Error);
        }

        [TestCase(100, 100, NotificationLevel.Warning)]
        [TestCase(115000, 99999, NotificationLevel.Okay)]
        [TestCase(20000000000, 10000000000, NotificationLevel.Okay)]
        public void RunAsync_WhenNodeInformationWithTotalAndUsedMemory_ReturnsNotificationLevel(long total, long used, NotificationLevel notification)
        {
            //Arrange
            var couchDbService = Substitute.For<ICouchDbService>();
            couchDbService.GetNodeInformation()
                .Returns(x => Task.FromResult(new NodeInformation
                {
                    storageTotals = new StorageTotals
                    {
                        ram = new Ram
                        {
                            total = total,
                            used = used
                        }
                    }

                }));

            var settings = new MemoryCheckSettings(string.Empty, string.Empty);
            var SUT = new MemoryCheck(settings, couchDbService);

            //Act
            var result = SUT.RunAsync().Result;

            //Assert
            result.Level.Should().Be(notification);
        }
    }
}
