using System.Collections.Generic;

namespace Nimator
{
    /// <summary>
    /// Represents the engine running actual checks through its <see cref="ILayer"/>s.
    /// </summary>
    public interface INimatorEngine
    {
        /// <summary>
        /// Runs checks in all internal <see cref="ILayer"/>s, promising not to throw any exceptions.
        /// </summary>
        /// <remarks>
        /// Implementations should catch all exceptions and return an appropriate <see cref="INimatorResult"/>
        /// (e.g. with a <see cref="NotificationLevel.Critical"/> level).
        /// </remarks>
        INimatorResult RunSafe();

        /// <summary>
        /// Add a <see cref="ILayer"/> to the engine.
        /// </summary>
        /// <param name="layer"></param>
        void AddLayer(ILayer layer);

        /// <summary>
        /// Add a new layer with specific name and checks to the engine.
        /// </summary>
        void AddLayer(string name, IEnumerable<ICheck> checks);
    }
}
