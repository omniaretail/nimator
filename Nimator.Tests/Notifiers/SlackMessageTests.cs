using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace Nimator.Notifiers
{
    [TestFixture]
    public class SlackMessageTests
    {
        [Test]
        public void Ctor_NotEmptySourceEnvironment_MessageContainsEnvironment()
        {
            // Arrange
            var env = "my existing test environment";
            var result = new NimatorResult(DateTime.Now);
            var checks = new List<ICheckResult>
            {
                new CheckResult("My Check", NotificationLevel.Critical)
            };
            result.LayerResults.Add(new LayerResult("My Test", checks));

            // Act
            var message = new SlackMessage(result, NotificationLevel.Error, env);

            // Assert
            message?.Text.ShouldNotBeNull();
            message.Text.ShouldStartWith($"{SlackMessage.ENV_PREFIX}{env}");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Ctor_NotEmptySourceEnvironment_MessageContainsEnvironment(string sourceEnv)
        {
            // Arrange
            var result = new NimatorResult(DateTime.Now);
            var checks = new List<ICheckResult>
            {
                new CheckResult("My Check", NotificationLevel.Critical)
            };
            result.LayerResults.Add(new LayerResult("My Test", checks));

            // Act
            var message = new SlackMessage(result, NotificationLevel.Error, sourceEnv);

            // Assert
            message?.Text.ShouldNotBeNull();
            message.Text.ShouldNotContain(SlackMessage.ENV_PREFIX);
        }
    }
}
