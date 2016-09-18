using System;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Nimator.ExampleConsoleApp
{
    class Program
    {
        private const int CheckIntervalInSecs = 15;
        private const string EmbeddedResourceConfigFile = "Nimator.ExampleConsoleApp.config.json";
        private static readonly ILog logger = LogManager.GetLogger("Nimator");

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionLogger;

            logger.Info("Creating Nimator.");

            var nimator = CreateNimator();

            logger.Info($"Nimator created. Starting timer for cycle every {CheckIntervalInSecs} seconds.");

            using (new Timer(_ => nimator.TickSafe(logger), null, 0, CheckIntervalInSecs * 1000))
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }

            logger.Info("Stopping Nimator.");
        }

        private static INimator CreateNimator()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(EmbeddedResourceConfigFile))
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return Nimator.FromSettings(json);
            }
        }

        private static void UnhandledExceptionLogger(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            var exc = eventArgs.ExceptionObject as Exception;

            if (exc != null)
            {
                logger.Fatal("Unhandled exception occurred.", exc);
            }
            else
            {
                logger.Fatal("Fatal problem without Excption occurred.");
                logger.Fatal(eventArgs.ExceptionObject);
            }
        }
    }
}
