using Microsoft.WindowsAzure.MobileServices;
using MySynopsis.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Services
{
    public class UserLoginService : IUserLoginService
    {
        private IUserService _userService;
        private Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> _loginUser;
        public UserLoginService(IUserService userService, Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> loginUser)
        {
            _userService = userService;
            _loginUser = loginUser;
        }

        public IEnumerable<LoginProvider> LoginProviders
        {
            get
            {
                return new[]{
                     new LoginProvider
                        {
                            Name = "Microsoft",
                            Provider = MobileServiceAuthenticationProvider.MicrosoftAccount
                        },
                        new LoginProvider
                        {
                            Name = "Google",
                            Provider = MobileServiceAuthenticationProvider.Google
                        },
                        new LoginProvider
                        {
                            Name = "Twitter",
                            Provider = MobileServiceAuthenticationProvider.Twitter
                        },
                        new LoginProvider
                        {
                            Name = "Facebook",
                            Provider = MobileServiceAuthenticationProvider.Facebook
                        }
                };
            }
        }

        public async Task<UserLoginResult> Login(MobileServiceAuthenticationProvider authenticationProvider)
        {
            UserLoginResult result;
            try
            {
                var serviceUser = await _loginUser(authenticationProvider);
                result = new UserLoginResult(serviceUser);
                var user = await _userService.ByUserId(serviceUser.UserId);
                if (user != null)
                {
                    result.UserDetails = user;
                }
            }
            catch (Exception ex)
            {
                result = new UserLoginResult(ex);
            }
            return result;
        }
    }
}
