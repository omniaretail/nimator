using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimator
{
    /// <summary>
    /// Represents a combination of <see cref="ICheckResult"/> items from running <see cref="ICheck"/>s 
    /// in a <see cref="ILayer"/>.
    /// </summary>
    public class LayerResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LayerResult"/>.
        /// </summary>
        /// <param name="layerName">Human readable identifier for the layer that generated this <see cref="LayerResult"/></param>
        /// <param name="checkResults">The individual results that make up this <see cref="LayerResult"/></param>
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

        /// <summary>
        /// The aggregated <see cref="NotificationLevel"/> of the composed <see cref="ICheckResult"/> subresults. This 
        /// will typically be the worst result of the batch. To get the individual levels, see the <see cref="CheckResults"/>.
        /// </summary>
        public NotificationLevel Level { get; }

        /// <summary>
        /// Name of the layer that generated this <see cref="LayerResult"/>.
        /// </summary>
        public string LayerName { get; }

        /// <summary>
        /// Individual <see cref="ICheckResult"/> sub results making up this <see cref="LayerResult"/>.
        /// </summary>
        public IEnumerable<ICheckResult> CheckResults { get; }

        /// <summary>
        /// Creates human-readable representation of this result.
        /// </summary>
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
