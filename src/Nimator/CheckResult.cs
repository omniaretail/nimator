using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public class CheckResult : ICheckResult
    {
        public CheckResult(string checkName, NotificationLevel level, string Message = "no details provided")
        {
            if (string.IsNullOrWhiteSpace(checkName))
            {
                throw new ArgumentException("CheckName has to be provided to be able to make sense of a check result.", "checkName");
            }

            this.CheckName = checkName;
            this.Level = level;
            this.Message = Message;
        }

        public NotificationLevel Level { get; set; }

        public string Message { get; set; }

        public string CheckName { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0} in {1}: {2}",
                Level.ToString(),
                CheckName,
                Message
            );
        }
    }
}
