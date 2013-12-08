﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySynopsis.BusinessLogic
{
    public class Meter
    {
        public Meter()
        {
            Rates = new List<MeterRate>();
        }
        public string Name { get; set; }
        public MeterType Type { get; set; }
        public List<MeterRate> Rates { get; set; }
    }
}
