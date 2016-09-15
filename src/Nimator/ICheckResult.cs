namespace Nimator
{
    public interface ICheckResult
    {
        string CheckName { get; set; }

        NotificationLevel Level { get; set; }

        string Message { get; set; }
    }
}
