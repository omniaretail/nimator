using System.Collections.Generic;

namespace Nimator
{
    /// <summary>
    /// Represents a combination of <see cref="ICheckResult"/> items from running <see cref="ICheck"/>s 
    /// in a <see cref="ILayer"/>. 
    /// </summary>
    public interface ILayerResult
    {
        /// <summary>
        /// The aggregated <see cref="NotificationLevel"/> of the composed <see cref="ICheckResult"/> subresults. This 
        /// will typically be the worst result of the batch. To get the individual levels, see the <see cref="CheckResults"/>.
        /// </summary>
        NotificationLevel Level { get; }

        /// <summary>
        /// Name of the layer that generated this <see cref="ILayerResult"/>.
        /// </summary>
        string LayerName { get; }

        /// <summary>
        /// Individual <see cref="ICheckResult"/> sub results making up this <see cref="ILayerResult"/>.
        /// </summary>
        IEnumerable<ICheckResult> CheckResults { get; }

        /// <summary>
        /// Creates human-readable representation of this result, with details for <see cref="NotificationLevel.Error"/>.
        /// </summary>
        string RenderPlainText();

        /// <summary>
        /// Creates human-readable representation of this result, with details for a given <see cref="NotificationLevel"/>.
        /// </summary>
        string RenderPlainText(NotificationLevel minLevelForDetails);
    }
}