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
        // This would probably be a bit higher (e.g. 60 secs or even more) and in
        // the App.config for production scenarios:
        private const int CheckIntervalInSecs = 15;

        // For ease of demo this is an embedded resource, but it could also be in a
        // seperate file or whatever persistence you'd prefer. It might be good not
        // to persist it in a database system, since your monitoring app should pro-
        // bably have as few dependencies as possible...
        private const string ConfigResource = "Nimator.ExampleConsoleApp.config.json";

        // See app.config for logging setup.
        private static readonly ILog logger = LogManager.GetLogger("Nimator");

        static void Main()
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

            logger.Info("Shutting down.");
        }

        private static INimator CreateNimator()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ConfigResource))
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
