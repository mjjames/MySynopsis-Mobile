using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySynopsis.BusinessLogic.Models
{
    public class LoginProvider
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public MobileServiceAuthenticationProvider Provider { get; set; }
    }
}
