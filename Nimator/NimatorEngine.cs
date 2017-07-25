using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimator
{
    /// <summary>
    /// Core <see cref="INimatorEngine"/> that will run layers sequentially.
    /// </summary>
    public class NimatorEngine : INimatorEngine
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(NimatorEngine));
        private const NotificationLevel StopProcessingAtThreshold = NotificationLevel.Error;

        private readonly IList<ILayer> layers;

        /// <summary>
        /// Constructs default engine without any <see cref="ILayer"/>s.
        /// </summary>
        public NimatorEngine()
        {
            this.layers = new List<ILayer>();
        }

        /// <summary>
        /// Constructs engine with specifica <see cref="ILayer"/>s.
        /// </summary>
        /// <param name="layers"></param>
        public NimatorEngine(IEnumerable<ILayer> layers)
        {
            if (layers == null) throw new ArgumentNullException(nameof(layers));
            this.layers = layers.ToList();
        }

        /// <inheritDoc/>
        public INimatorResult RunSafe()
        {
            try
            {
                return RunUnsafe();
            }
            catch (AggregateException ex)
            {
                var error = GetAggregateExceptionMessageAndStackTrace(ex);
                var fullText = $"Nimator itself failed: {error.Item1}";

                _logger.Error(fullText, ex);
                _logger.Error(error.Item2, ex);

                return new CriticalNimatorResult("Nimator (or one of its layers) itself failed.", fullText);
            }
            catch (Exception ex)
            {
                var error = GetInnerExceptionMessageAndStackTrace(ex);
                var fullText = $"Nimator itself failed: {error.Item1}";

                _logger.Error(fullText, ex);
                _logger.Error(error.Item2, ex);

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

        private Tuple<string, string> GetInnerExceptionMessageAndStackTrace(Exception ex)
        {
            string message = String.Empty;
            string stackTrace = String.Empty;

            if (ex == null)
            {
                return new Tuple<string, string>(message, stackTrace);
            }

            message = ex.Message + Environment.NewLine;

            if (ex.InnerException != null)
            {
                var innerResult = GetInnerExceptionMessageAndStackTrace(ex.InnerException);

                message += '\t' + innerResult.Item1;
                stackTrace = innerResult.Item2;
            }

            return new Tuple<string, string>(message, stackTrace); ;
        }

        private Tuple<string, string> GetAggregateExceptionMessageAndStackTrace(AggregateException ex)
        {
            string message = String.Empty;
            string stackTrace = String.Empty;

            if (ex == null)
            {
                return new Tuple<string, string>(message, stackTrace);
            }

            foreach (var innerEx in ex.InnerExceptions)
            {
                message += '\t' + innerEx.Message;
                stackTrace = innerEx.StackTrace;
            }

            return new Tuple<string, string>(message, stackTrace); ;
        }

        /// <inheritDoc/>
        public void AddLayer(string name, IEnumerable<ICheck> checks)
        {
            this.layers.Add(new Layer(name, checks));
        }

        /// <inheritDoc/>
        public void AddLayer(ILayer layer)
        {
            this.layers.Add(layer);
        }
    }
}
