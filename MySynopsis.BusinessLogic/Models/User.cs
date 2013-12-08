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
        public long Id { get; set; }
        /// <summary>
        /// The user id returned by the mobile service
        /// </summary>
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public DateTime SignedUpUtc { get; set; }
        public List<Meter> MeterConfiguration { get; set; }
    }
}
