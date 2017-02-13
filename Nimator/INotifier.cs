namespace Nimator
{
    /// <summary>
    /// Represents something that can distribute a <see cref="INimatorResult"/> to receivers.
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Notifies receivers via a channel about a <see cref="INimatorResult"/>.
        /// </summary>
        /// <remarks>
        /// Implementations are allowed to throw any type of exception that is warranted. The 
        /// <see cref="INimator"/> distributing a <see cref="INimatorResult"/> to various
        /// notifiers should decide how to handle exceptions thrown by Notify.
        /// </remarks>
        void Notify(INimatorResult result);
    }
}
