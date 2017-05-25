namespace Nimator.CouchDb
{
    public class Ram
    {
        public long quotaTotal { get; set; }
        public long quotaTotalPerNode { get; set; }
        public long quotaUsed { get; set; }
        public long quotaUsedPerNode { get; set; }
        public long total { get; set; }
        public long used { get; set; }
        public long usedByData { get; set; }
    }
}