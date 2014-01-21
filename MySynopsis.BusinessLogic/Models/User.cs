using MySynopsis.BusinessLogic.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySynopsis.BusinessLogic
{
    public class User
    {
        public User()
        {
            MeterConfiguration = new List<Meter>();
        }
        /// <summary>
        /// The storage service's identifier
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The user id returned by the mobile service
        /// </summary>
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public DateTime SignedUpUtc { get; set; }

        [JsonConverter(typeof(ListConverter<Meter>))]
        public List<Meter> MeterConfiguration { get; set; }

        public bool IsValid
        {
            get
            {
                return  !String.IsNullOrWhiteSpace(UserId)
                        && !String.IsNullOrWhiteSpace(EmailAddress)
                        && !String.IsNullOrWhiteSpace(Name)
                        && MeterConfiguration.Any();
            }
        }

        public bool IsRegistered
        {
            get
            {
                return Id != null && SignedUpUtc > DateTime.MinValue;
            }
        }
    }
}
