using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public interface INimatorEngine
    {
        INimatorResult Run();

        void AddLayer(ILayer layer);

        void AddLayer(string name, IEnumerable<ICheck> checks);
    }
}
