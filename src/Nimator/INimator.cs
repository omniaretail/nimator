namespace Nimator
{
    /// <summary>
    /// Represents something that can "Tick" and get monitoring results to the appropriate receivers. Implementations will
    /// typically use an <see cref="INimatorEngine"/> to do the actual work.
    /// </summary>
    public interface INimator
    {
        /// <summary>
        /// Promises to run a monitoring cycle, probably delegating the work to the inner <see cref="INimatorEngine"/>, and
        /// reports to any <see cref="INotifier"/>s if any. Implementations should be "Safe" in a sense that they trap all
        /// exceptions, only letting the most gruesome failures escape (e.g. <see cref="System.Threading.ThreadAbortException"/>).
        /// </summary>
        /// <param name="logger">The <see cref="log4net.ILog"/> instance to log application exceptiosn to.</param>
        void TickSafe(log4net.ILog logger);
    }
}
