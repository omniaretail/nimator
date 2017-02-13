namespace Nimator.Settings
{
    /// <summary>
    /// Settings for a <see cref="Layer"/>.
    /// </summary>
    public class LayerSettings
    {
        /// <summary>
        /// Constructs default, empty <see cref="LayerSettings"/>.
        /// </summary>
        public LayerSettings()
        {
            Checks = new ICheckSettings[0];
        }

        /// <summary>
        /// Human readable name for the <see cref="Layer"/> to be generated based on these settings.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of <see cref="ICheckSettings"/> instances to be generated.
        /// </summary>
        public ICheckSettings[] Checks { get; set; }
    }
}
