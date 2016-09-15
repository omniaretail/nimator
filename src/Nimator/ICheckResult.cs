namespace Nimator
{
    /// <summary>
    /// Represents the result of running an <see cref="ICheck"/>.
    /// </summary>
    public interface ICheckResult
    {
        /// <summary>
        /// Repeats the name of the <see cref="ICheck"/>, possibly in a format altered to
        /// work best for display purposes.
        /// </summary>
        string CheckName { get; set; }

        /// <summary>
        /// The <see cref="NotificationLevel"/> for this result (e.g. "Okay", or "Error").
        /// </summary>
        NotificationLevel Level { get; set; }

        /// <summary>
        /// The description of how the <see cref="ICheck"/> came to this result.
        /// </summary>
        string Message { get; set; }
    }
}
