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
        string CheckName { get; }

        /// <summary>
        /// The <see cref="NotificationLevel"/> for this result (e.g. "Okay", or "Error").
        /// </summary>
        NotificationLevel Level { get; }

        /// <summary>
        /// Joins <see cref="Level"/>, <see cref="CheckName"/>, and any other details, in 
        /// a readable fashion.
        /// </summary>
        string RenderPlainText();
    }
}
