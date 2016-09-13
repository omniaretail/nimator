using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;
using Nimator.Settings;

namespace Nimator
{
    public class Nimator : INimator
    {
        private readonly IEnumerable<INotifier> notifiers;
        private readonly INimatorEngine nimator;

        private Nimator(NimatorSettings settings)
        {
            this.notifiers = settings.Notifiers.Select(s => s.ToNotifier()).ToList();

            nimator = new NimatorEngine();

            foreach (var layerSettings in settings.Layers)
            {
                var checks = layerSettings.Checks.Select(c => c.ToCheck()).ToList();
                nimator.AddLayer(layerSettings.Name, checks);
            }
        }

        /// <summary>
        /// Run all checks and distribute results, as safely as possible (meaning 
        /// that we try to trap as many types of exceptions as reasonably possible
        /// without crashing).
        /// </summary>
        /// <param name="logger"></param>
        public void TickSafe(ILog logger)
        {
            try
            {
                this.Tick();
            }
            catch (NotificationException ex)
            {
                Console.WriteLine("App tick caused a NotificationException.");
                logger.Error("App tick caused a NotificationException.", ex);

                // This type of exception could be common and is no reason to exit
                // immediately, so we just continue.
            }
            catch (Exception ex)
            {
                Console.WriteLine("App tick caused an unexpected exception.");
                logger.Error("App tick caused an unexpected exception.", ex);

                // TODO: Pull the plug, exit the application. To do so gracefully
                // we need a proper way to post back to the main thread's context,
                // because this code will be running on a seperate thread.
            }
        }

        private void Tick()
        {
            var result = nimator.Run();

            var exceptions = new ConcurrentBag<Exception>();

            Parallel.ForEach(notifiers, notifier =>
            {
                try
                {
                    notifier.Notify(result);
                }
                catch (Exception ex) 
                {
                    exceptions.Add(ex);
                }
            });

            if (exceptions.Any())
            {
                throw new NotificationException("One or more notifiers could not notify.", exceptions);
            }
        }
        
        public static INimator FromSettings(string json)
        {
            return new Nimator(NimatorSettings.FromJson(json));
        }
    }
}
