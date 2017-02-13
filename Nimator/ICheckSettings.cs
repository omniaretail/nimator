namespace Nimator
{
    /// <summary>
    /// Represents the configuration of an <see cref="ICheck"/>. There are no required properties
    /// for this interface, as each implementation is meant for exactly that: to represent the 
    /// configuration needed for an <see cref="ICheck"/>.
    /// </summary>
    public interface ICheckSettings
    {
        /// <summary>
        /// When called, the settings converts itself to an <see cref="ICheck"/> instance. This
        /// effectively means each <see cref="ICheckSettings"/> is a mini-composition-root that
        /// can construct concrete dependencies for an <see cref="ICheck"/>.
        /// </summary>
        /// <returns></returns>
        ICheck ToCheck();
    }
}
