using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Settings
{
    public class LayerSettings
    {
        public LayerSettings()
        {
            Checks = new ICheckSettings[0];
        }

        public string Name { get; set; }

        public ICheckSettings[] Checks { get; set; }
    }
}
