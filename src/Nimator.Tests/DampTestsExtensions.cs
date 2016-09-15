using System;
using System.Threading.Tasks;
using Moq;

namespace Nimator
{
    public static class DampTestsExtensions
    {
        public static Mock<ILayer> WithResult(this Mock<ILayer> layer, NotificationLevel level)
        {
            var name = string.IsNullOrWhiteSpace(layer.Object.Name) ? "dummy-layer" : layer.Object.Name;
            var result = new LayerResult(name, new[] { new CheckResult("dummy-check-result", level) });
            layer.Setup(l => l.Run()).Returns(result);
            return layer;
        }

        public static Task<ICheckResult> AsTaskResult(this CheckResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            return Task.FromResult<ICheckResult>(result);
        }
    }
}
