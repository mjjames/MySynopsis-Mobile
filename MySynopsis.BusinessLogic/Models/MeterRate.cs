using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySynopsis.BusinessLogic
{
    public class MeterRate
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public decimal UnitRate { get; set; }
        public decimal StandingCharge { get; set; }
        public RateType Type { get; set; }
    }


}
