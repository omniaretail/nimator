using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimator
{
    public class NimatorEngine : INimatorEngine
    {
        private const NotificationLevel StopProcessingAtThreshold = NotificationLevel.Error;

        private readonly IList<ILayer> layers;
        
        public NimatorEngine()
        {
            this.layers = new List<ILayer>();
        }

        public NimatorEngine(IEnumerable<ILayer> layers)
        {
            if (layers == null) throw new ArgumentNullException(nameof(layers));
            this.layers = layers.ToList();
        }

        public INimatorResult Run()
        {
            try
            {
                return RunUnsafe();
            }
            catch (Exception ex)
            {
                var fullText = $"Nimator (or one of its layers) itself failed. Exception '{ex.GetType().Name}' with message: {ex.Message}";

                return new CriticalNimatorResult("Nimator (or one of its layers) itself failed.", fullText);
            }
        }

        private NimatorResult RunUnsafe()
        {
            var nimatorResult = new NimatorResult(AmbientTimeProvider.GetNow());
            
            foreach (var layer in this.layers)
            {
                var layerResult = layer.Run();

                if (layerResult == null)
                {
                    throw new InvalidOperationException("Layer " + layer.Name + " returned no result. Cannot continue because we now cannot determine error level of that layer.");
                }

                nimatorResult.LayerResults.Add(layerResult);

                if (layerResult.Level >= StopProcessingAtThreshold)
                {
                    break;
                }
            }

            nimatorResult.Finished = AmbientTimeProvider.GetNow();

            return nimatorResult;
        }

        public void AddLayer(string name, IEnumerable<ICheck> checks)
        {
            this.layers.Add(new Layer(name, checks));
        }

        public void AddLayer(ILayer layer)
        {
            this.layers.Add(layer);
        }
    }
}
