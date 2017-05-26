namespace Nimator.CouchDb.Models
{
    public class BasicStats
    {
        public int dataUsed { get; set; }
        public int diskFetches { get; set; }
        public int diskUsed { get; set; }
        public long itemCount { get; set; }
        public int memUsed { get; set; }
        public int opsPerSec { get; set; }
        public double quotaPercentUsed { get; set; }
    }
}