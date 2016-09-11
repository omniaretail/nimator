using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public interface ILayer
    {
        string Name { get; set; }

        LayerResult Run();
    }
}
