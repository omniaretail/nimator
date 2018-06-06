using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers.DataDog
{
    interface IDataDogEventConverter
    {
        IEnumerable<DataDogEvent> Convert(INimatorResult result);
    }
}
