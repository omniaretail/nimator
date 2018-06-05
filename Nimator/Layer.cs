using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nimator
{
    internal class Layer : ILayer
    {
        private readonly List<ICheck> checks;

        public Layer(string name, IEnumerable<ICheck> checks)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required so that a layer is recognizable.", nameof(name));
            }

            this.Name = name;
            this.checks = checks.ToList();
        }

        public string Name { get; set; }

        public ILayerResult Run()
        {
            return Task.Run(async () => await RunAsync()).Result;
        }

        private async Task<LayerResult> RunAsync()
        {
            var tasks = checks.Select(GetResult);
            var checkResults = await Task.WhenAll(tasks);
            return new LayerResult(this.Name, checkResults);
        }

        private async Task<ICheckResult> GetResult(ICheck check)
        {
            try
            {
                return await check.RunAsync();
            }
            catch (Exception ex)
            {
                var result = new CheckResult(
                    check?.ShortName ?? "Unknown",
                    NotificationLevel.Critical,
                    ex.ToString()
                );

                return await Task.FromResult<ICheckResult>(result);
            }
        }
    }
}
