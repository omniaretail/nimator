namespace Nimator.Settings
{
    public class LayerSettings
    {
        public LayerSettings()
        {
            Checks = new ICheckSettings[0];
        }

        public string Name { get; set; }

        public ICheckSettings[] Checks { get; set; }
    }
}
