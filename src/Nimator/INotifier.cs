using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public interface INotifier
    {
        void Notify(INimatorResult result);
    }
}
