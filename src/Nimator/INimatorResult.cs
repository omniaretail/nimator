using System;
using System.Collections.Generic;

namespace Nimator
{
    public interface INimatorResult
    {
        DateTime Finished { get; }

        DateTime Started { get; }

        NotificationLevel Level { get; }

        IList<LayerResult> LayerResults { get; }

        string Message { get; }

        string GetFirstFailedLayerName();

        string[] GetFailingLayerNames();

        string[] GetFailingCheckNames();

        string RenderPlainText();
    }
}
