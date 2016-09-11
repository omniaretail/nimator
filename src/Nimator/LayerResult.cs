using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public class LayerResult
    {
        public LayerResult(string layerName, IEnumerable<ICheckResult> checkResults)
        {
            if (string.IsNullOrWhiteSpace(layerName))
            {
                throw new ArgumentException("Providing a LayerName is required for a LayerResult to be meaningful.", "layerName");
            }

            this.LayerName = layerName;
            
            this.CheckResults = checkResults == null ? new ICheckResult[0] : checkResults.ToArray();

            if (this.CheckResults.Any(r => r == null))
            {
                throw new ArgumentException("One or more check result objects were NULL.", "checkResults");
            }

            this.Level = CheckResults.Any() 
                ? (NotificationLevel)CheckResults.Max(r => (int)r.Level)
                : NotificationLevel.Warning;
        }

        public NotificationLevel Level { get; private set; }

        public string LayerName { get; private set; }

        public IEnumerable<ICheckResult> CheckResults { get; private set; }

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

            return string.Format(
                "{0}: after running {1} check(s) in {2}{3}",
                Level.ToString(),
                CheckResults.Count(),
                LayerName,
                addendum
            );
        }
    }
}
