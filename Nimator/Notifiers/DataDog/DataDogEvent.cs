using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers.DataDog
{
    class DataDogEvent
    {
        public string StatName { get; set; }
        public string LayerName { get; set; }
        public string CheckName { get; set; }
        public string Message { get; set; }

        public AlertType AlertType { get; internal set; }
    }
}
