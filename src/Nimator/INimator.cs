using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimator
{
    public interface INimator
    {
        void TickSafe(log4net.ILog logger);
    }
}
