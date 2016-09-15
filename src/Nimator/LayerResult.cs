using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimator
{
    public class LayerResult
    {
        public LayerResult(string layerName, IEnumerable<ICheckResult> checkResults)
        {
            if (string.IsNullOrWhiteSpace(layerName))
            {
                throw new ArgumentException("Providing a LayerName is required for a LayerResult to be meaningful.", nameof(layerName));
            }

            this.LayerName = layerName;
            
            this.CheckResults = checkResults?.ToArray() ?? new ICheckResult[0];

            if (this.CheckResults.Any(r => r == null))
            {
                throw new ArgumentException("One or more check result objects were NULL.", nameof(checkResults));
            }

            this.Level = CheckResults.Any() 
                ? (NotificationLevel)CheckResults.Max(r => (int)r.Level)
                : NotificationLevel.Warning;
        }

        public NotificationLevel Level { get; }

        public string LayerName { get; }

        public IEnumerable<ICheckResult> CheckResults { get; }

        public override string ToString()
        {
            var checkErrors = CheckResults
                .Where(r => r.Level >= NotificationLevel.Error)
                .Select(r => r.ToString());

            string addendum = "";

            if (checkErrors.Any())
            {
                addendum = "\n - - " + string.Join("\n - - ", checkErrors);
            }

            return $"{Level}: after running {CheckResults.Count()} check(s) in {LayerName}{addendum}";
        }
    }
}
