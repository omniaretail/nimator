using System;
using System.Collections.Generic;
using Nimator.Notifiers.DataDog;
using Nimator.Settings;
using StatsdClient;

namespace Nimator.Notifiers.Integration.Tests
{
    internal class DataDogNotifierScenarios
    {
        private static DataDogNotifier _notifier;

        public static void Run()
        {
            ConfigureNotifier();

            SendOkayResultForTestCheck();
            SendMultipleChecksInLayer();
        }

        private static void ConfigureNotifier()
        {
            DataDogSettings settings = new DataDogSettings()
            {
                Prefix = "nimatorIntegrationTests",
                StatsdServerName = "127.0.0.1",
                Threshold = NotificationLevel.Warning
            };

            _notifier = (DataDogNotifier)settings.ToNotifier();
        }

        private static void SendMultipleChecksInLayer()
        {
            NimatorResult nimatorResult = new NimatorResult(DateTime.Now);

            List<ICheckResult> checks = new List<ICheckResult>();
            checks.Add(new CheckResult("testCheck1", NotificationLevel.Okay));
            checks.Add(new CheckResult("testCheck2", NotificationLevel.Warning));
            checks.Add(new CheckResult("testCheck3", NotificationLevel.Error));
            checks.Add(new CheckResult("testCheck4", NotificationLevel.Critical));

            ILayerResult layerResult = new LayerResult("secondLayer", checks);
            nimatorResult.LayerResults.Add(layerResult);

            _notifier.Notify(nimatorResult);
        }

        private static void SendOkayResultForTestCheck()
        {
            NimatorResult nimatorResult = new NimatorResult(DateTime.Now);
            List<ICheckResult> checks = new List<ICheckResult>();
            checks.Add(new CheckResult("testCheck", NotificationLevel.Okay));
            ILayerResult layerResult = new LayerResult("firstLayer", checks);
            nimatorResult.LayerResults.Add(layerResult);

            _notifier.Notify(nimatorResult);
        }

    }
}