using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Nimator;

namespace Nimator
{
    public static class DampExtensionsForFramework
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
            if (result == null) throw new ArgumentNullException("result");
            return Task.FromResult<ICheckResult>(result);
        }
    }
}
