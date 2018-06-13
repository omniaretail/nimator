using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers.DataDog
{
    public class DataDogEventTests
    {
        [TestCase(null, "value")]
        [TestCase("name", null)]
        public void AddTag_ParamNull_Exception(string name, string value)
        {
            Should.Throw<ArgumentNullException>(() => GetSut().AddTag(name, value));
        }

        [Test]
        public void AddTag_NoTagAdded_TagsNull()
        {
            // Arrange, Act
            var tags = GetSut().Tags;

            // Assert
            tags.ShouldBeNull();
        }

        [Test]
        public void Tags_TagAdded_ContainsTagFormattedAndLowercase()
        {
            // Arrange
            var sut = GetSut();
            sut.AddTag("Name", "VALUE");

            // Act
            var tags = sut.Tags;

            // Assert
            tags.ShouldNotBeNull();
            tags.Length.ShouldBe(1);
            tags[0].ShouldBe("name:value");
        }

        [Test]
        public void Tags_DuplicateTagsAdded_ContainsTagsOverwritten()
        {
            // Arrange
            var sut = GetSut();
            sut.AddTag("Name", "VALUE");
            sut.AddTag("Name", "VALUE2");
            sut.AddTag("Name2", "VALUE3");

            // Act
            var tags = sut.Tags;

            // Assert
            tags.ShouldNotBeNull();
            tags.Length.ShouldBe(2);
            tags[0].ShouldBe("name:value2");
            tags[1].ShouldBe("name2:value3");
        }

        private DataDogEvent GetSut()
        {
            return new DataDogEvent();
        }
    }
}
