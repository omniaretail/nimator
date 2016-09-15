using System;

namespace Nimator
{
    public class CheckResult : ICheckResult
    {
        public CheckResult(string checkName, NotificationLevel level, string message = "no details provided")
        {
            if (string.IsNullOrWhiteSpace(checkName))
            {
                throw new ArgumentException("CheckName has to be provided to be able to make sense of a check result.", nameof(checkName));
            }

            this.CheckName = checkName;
            this.Level = level;
            this.Message = message;
        }

        public NotificationLevel Level { get; set; }

        public string Message { get; set; }

        public string CheckName { get; set; }

        public override string ToString()
        {
            return $"{Level} in {CheckName}: {Message}";
        }
    }
}
