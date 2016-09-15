using System.Collections.Generic;

namespace Nimator
{
    public interface INimatorEngine
    {
        INimatorResult Run();

        void AddLayer(ILayer layer);

        void AddLayer(string name, IEnumerable<ICheck> checks);
    }
}
