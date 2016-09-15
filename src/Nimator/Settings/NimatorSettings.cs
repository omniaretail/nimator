using Newtonsoft.Json;

namespace Nimator.Settings
{
    /// <summary>
    /// Top level object for settings to bootstrap <see cref="INimator"/> instances.
    /// </summary>
    public class NimatorSettings
    {
        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented,
            Converters = new JsonConverter[] { new Newtonsoft.Json.Converters.StringEnumConverter() },
        };

        internal NimatorSettings()
        {
            this.Layers = new LayerSettings[0];
            this.Notifiers = new NotifierSettings[] { new ConsoleSettings() };
        }

        /// <summary>
        /// List of <see cref="NotifierSettings"/> instances to determine how recepients get notified.
        /// </summary>
        public NotifierSettings[] Notifiers { get; set; }

        /// <summary>
        /// List of <see cref="LayerSettings"/> to determine the actual (layered) checks that will be run.
        /// </summary>
        public LayerSettings[] Layers { get; set; }

        /// <summary>
        /// Convert this <see cref="NimatorSettings"/> instance to a json string so it can be persisted.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, jsonSettings);
        }

        /// <summary>
        /// Create <see cref="NimatorSettings"/> instance from json string.
        /// </summary>
        /// <param name="json"></param>
        public static NimatorSettings FromJson(string json)
        {
            return JsonConvert.DeserializeObject<NimatorSettings>(json, jsonSettings);
        }

        /// <summary>
        /// Get typical example <see cref="NimatorSettings"/> instance with dummy data.
        /// </summary>
        public static NimatorSettings GetExample()
        {
            return new NimatorSettings
            {
                Notifiers = new NotifierSettings[] 
                { 
                    ConsoleSettings.GetExample(),
                    OpsGenieSettings.GetExample(),
                    SlackSettings.GetExample(),
                },
                Layers = new LayerSettings[]
                {
                    new LayerSettings
                    {
                        Name = "Layer 1",
                        Checks = new ICheckSettings[]
                        {
                            new NoopCheckSettings(),
                            new NoopCheckSettings(),
                        },
                    },
                    new LayerSettings
                    {
                        Name = "Layer 2",
                        Checks = new ICheckSettings[]
                        {
                            new NoopCheckSettings(),
                            new NoopCheckSettings(),
                        },
                    }
                },
            };
        }
    }
}
