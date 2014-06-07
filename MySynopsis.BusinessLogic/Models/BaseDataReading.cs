using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySynopsis.BusinessLogic.Models
{
    public class BaseDataReading
    {
        public DateTime TimeStampUtc { get; set; }
        public double Reading { get; set; }
        public Guid MeterId { get; set; }
    }
}
