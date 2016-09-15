using System.Threading.Tasks;

namespace Nimator
{
    /// <summary>
    /// Represents a check that will be called upon each time Nimator "Ticks" to check the system state.
    /// </summary>
    /// <remarks>
    /// The lifetime of an ICheck is linked to the <see cref="NimatorEngine"/>. Everytime the engine is 
    /// started, instances of all (per configuration) requested ICheck classesare constructed. This means
    /// an ICheck might live for quite some time, but nonetheless it might be recreated (e.g. if the 
    /// settings changed and the engine was restarted.
    /// 
    /// <see cref="ICheck"/> instances are typically created from <see cref="ICheckSettings.ToCheck"/>.
    /// </remarks>
    public interface ICheck
    {
        /// <summary>
        /// A simple human-readable way to identify the Check.
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// Every time the <see cref="NimatorEngine"/> "Ticks" the check will be run.
        /// </summary>
        /// <returns>A task representing the check calculating the current result.</returns>
        /// <remarks>
        /// You can safely assume this method will not be called concurrently on one and the same instance 
        /// of your implementation (though it can be called concurrently on seperate different instances).
        /// 
        /// Implementations are allowed to throw exceptions, although it is encouraged to deal with the
        /// exception types that are specific to the check (e.g. a WebException for clients that go over
        /// a WebClient to check stuff), crafting an informative <see cref="ICheckResult"/> for those
        /// situations. If an exception flows from this method, the engine will keep running but consider
        /// the result to be a <see cref="NotificationLevel.Critical"/> failure.
        /// </remarks>
        Task<ICheckResult> RunAsync();
    }
}
