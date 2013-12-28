using System;

namespace MySynopsis.BusinessLogic.Models
{
    public class DataReading
    {
        public long Id { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public long Reading { get; set; }
        public Guid MeterId { get; set; }
        public long UserId { get; set; }
    }
}
