using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySynopsis.BusinessLogic.Models
{
    public class UserLoginResult
    {
        public UserLoginResult(Exception exception)
        {
            AuthenticationException = exception;
            AuthenticationFailed = true;
        }

        public UserLoginResult(MobileServiceUser user)
        {
            User = user;
        }

        public MobileServiceUser User { get; private set; }
        public User UserDetails { get; set; }
        public bool RequiresRegistration
        {
            get
            {
                return UserDetails == null;
            }
        }
        public bool AuthenticationFailed { get; private set; }
        public Exception AuthenticationException { get; private set; }
    }
}
