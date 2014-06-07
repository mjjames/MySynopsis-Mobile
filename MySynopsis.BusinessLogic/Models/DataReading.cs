using System;

namespace MySynopsis.BusinessLogic.Models
{
    public class DataReading : BaseDataReading
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
    }
}
