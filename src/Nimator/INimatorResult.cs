using System;
using System.Collections.Generic;

namespace Nimator
{
    /// <summary>
    /// Represents a "Grand Result", i.e. the result of a complete monitoring cycle.
    /// </summary>
    public interface INimatorResult
    {
        /// <summary>
        /// The moment the cycle for this result was completed.
        /// </summary>
        DateTime Finished { get; }

        /// <summary>
        /// The moment the cycle for this result was started.
        /// </summary>
        DateTime Started { get; }

        /// <summary>
        /// Aggregated <see cref="NotificationLevel"/> of the individual parts, typically the "worst" result.
        /// </summary>
        NotificationLevel Level { get; }

        /// <summary>
        /// Individual <see cref="ILayerResult"/>s making up this aggregated, final <see cref="INimatorResult"/>.
        /// </summary>
        IList<ILayerResult> LayerResults { get; }

        /// <summary>
        /// Human readable explanation of this <see cref="INimatorResult"/>.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Get the Name of the first <see cref="ILayer"/> that was considered beyond the <see cref="NotificationLevel"/> threshold.
        /// </summary>
        string GetFirstFailedLayerName();

        /// <summary>
        /// Get the Names of all <see cref="ILayer"/>s that were considered beyond the <see cref="NotificationLevel"/> threshold.
        /// </summary>
        string[] GetFailingLayerNames();

        /// <summary>
        /// Get the Names of all failing <see cref="ICheck"/>s inside the individual <see cref="ILayer"/>s that were checked.
        /// </summary>
        string[] GetFailingCheckNames();

        /// <summary>
        /// Render this result in (possibly mutli-line) plain text, with deatils for a default <see cref="NotificationLevel.Error"/> threshold.
        /// </summary>
        string RenderPlainText();

        /// <summary>
        /// Render this result in (possibly mutli-line) plain text, with details for a chosen <see cref="NotificationLevel"/> threshold.
        /// </summary>
        string RenderPlainText(NotificationLevel minLevelForDetails);
    }
}
