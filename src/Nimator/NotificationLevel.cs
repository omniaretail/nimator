namespace Nimator
{
    public enum NotificationLevel
    {
        /// <summary>
        /// For mere status reports of regular situations, e.g. "Couchbase returned 'STORE: OKAY'." and similar.
        /// </summary>
        Okay = 10,

        /// <summary>
        /// For when there's something worth checking, though not necessarily right now, e.g. "Diskspace running low" or "Many Workers queued".
        /// </summary>
        Warning = 20,

        /// <summary>
        /// For things requiring immediate attention, e.g. services being entirely unreachable.
        /// </summary>
        Error = 30,

        /// <summary>
        /// For unrecoverable monitoring problems, e.g. the fact that monitoring itself cannot run.
        /// </summary>
        Critical = 40,
    }
}
