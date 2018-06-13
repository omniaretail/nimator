namespace Nimator.Notifiers.DataDog
{
    /// <summary>
    /// Represents possible values for DataDog alert types.
    /// Alert type has to be lower case, otherwise not interpreted correctly (DataDog agent v.6.x)
    /// </summary>
    internal static class AlertType
    {
        public const string Error = "error";
        public const string Warning = "warning";
        public const string Success = "success";
        public const string Info = "info";
    }
}