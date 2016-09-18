using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimator
{
    /// <inheritdoc/>
    public class LayerResult : ILayerResult
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

        /// <inheritdoc/>
        public NotificationLevel Level { get; }

        /// <inheritdoc/>
        public string LayerName { get; }

        /// <inheritdoc/>
        public IEnumerable<ICheckResult> CheckResults { get; }

        /// <inheritdoc/>
        public string RenderPlainText()
        {
            var checkErrors = CheckResults
                .Where(r => r.Level >= NotificationLevel.Error)
                .Select(r => r.RenderPlainText());

            string addendum = "";

            if (checkErrors.Any())
            {
                addendum = "\n - - " + string.Join("\n - - ", checkErrors);
            }

            return $"{Level}: after running {CheckResults.Count()} check(s) in {LayerName}{addendum}";
        }
    }
}
