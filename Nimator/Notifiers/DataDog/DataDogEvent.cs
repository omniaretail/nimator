using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers.DataDog
{
    internal class DataDogEvent
    {
        private readonly IDictionary<string, string> tags = new Dictionary<string, string>();

        public string StatName { get; set; }

        public string Title { get; set; }

        public string AlertType { get; set; }

        public string Message { get; set; }

        public string[] Tags
        {
            get
            {
                if (!tags.Any()) return null;

                return tags.Select(t => $"{t.Key}:{t.Value}").ToArray();
            }
        }

        public void AddTag(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            tags[name.ToLower()] = value.ToLower();
        }
    }
}
