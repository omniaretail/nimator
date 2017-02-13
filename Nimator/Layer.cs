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
            var tasks = checks.Select(GetResult).ToArray();
            var checkResults = Task.WhenAll(tasks).Result;
            return new LayerResult(this.Name, checkResults);
        }

        private Task<ICheckResult> GetResult(ICheck check)
        {
            try
            {
                return check.RunAsync();
            }
            catch (Exception ex)
            {
                var result = new CheckResult(
                    check?.ShortName ?? "Unknown",
                    NotificationLevel.Critical,
                    ex.ToString()
                );

                return Task.FromResult<ICheckResult>(result);
            }
        }
    }
}
