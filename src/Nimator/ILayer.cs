namespace Nimator
{
    /// <summary>
    /// Represents a logical group of <see cref="ICheck"/> instances.
    /// </summary>
    public interface ILayer
    {
        /// <summary>
        /// The human-readable identifier for a layer, e.g. "Hardware" or "API's".
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Runs all components to get to an aggregated result.
        /// </summary>
        /// <returns></returns>
        ILayerResult Run();
    }
}
