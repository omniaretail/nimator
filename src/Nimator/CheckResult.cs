using System;

namespace Nimator
{
    /// <summary>
    /// Basic implementation of <see cref="ICheckResult"/>, not much more than a DTO for the constructor arguments.
    /// </summary>
    public class CheckResult : ICheckResult
    {
        /// <summary>
        /// Constructs new <see cref="CheckResult"/> for specific values.
        /// </summary>
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

        /// <inheritdoc/>
        public NotificationLevel Level { get; set; }

        /// <inheritdoc/>
        public string Message { get; set; }

        /// <inheritdoc/>
        public string CheckName { get; set; }

        /// <summary>
        /// Joins <see cref="Level"/>, <see cref="CheckName"/>, and <see cref="Message"/> in a readable fashion.
        /// </summary>
        public override string ToString()
        {
            return $"{Level} in {CheckName}: {Message}";
        }
    }
}
