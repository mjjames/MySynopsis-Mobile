using MySynopsis.BusinessLogic.JsonConverters;
using Newtonsoft.Json;
using System;
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
            Id = Guid.NewGuid();
        }
        public string Name { get; set; }
        public MeterType Type { get; set; }
        //[JsonConverter(typeof(ListConverter<MeterRate>))]
        public List<MeterRate> Rates { get; set; }
        public Guid Id { get; set; }
    }
}
