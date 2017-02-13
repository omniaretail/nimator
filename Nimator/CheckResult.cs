using System;

namespace Nimator
{
    /// <summary>
    /// Basic implementation of <see cref="ICheckResult"/>, not much more than a DTO for the constructor arguments.
    /// </summary>
    public class CheckResult : ICheckResult
    {
        private readonly string message;

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
            this.message = message;
        }

        /// <inheritdoc/>
        public NotificationLevel Level { get; set; }

        /// <inheritdoc/>
        public string CheckName { get; set; }

        /// <inheritdoc/>
        public string RenderPlainText()
        {
            return $"{Level} in {CheckName}: {message}";
        }
    }
}
