using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using MySynopsis.BusinessLogic.ViewModels;
using MySynopsis.BusinessLogic.Services;

namespace MySynopsis.Android
{
    public static class Factory
    {
        static Factory()
        {
            ServiceClient = new MobileServiceClient("https://synopsis.azure-mobile.net/");
        }
        public static IMobileServiceClient ServiceClient { get; private set; }

        public static LoginViewModel GetLoginViewModel(Context context)
        {
            return new LoginViewModel(GetLoginService(context));
        }

        public static IUserLoginService GetLoginService(Context context)
        {
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> authenticate = async (provider) => await ServiceClient.LoginAsync(context, provider);
            return new UserLoginService(GetUserService(), authenticate);
        }

        private static IUserService GetUserService()
        {
            return new UserService(ServiceClient);
        }


        public static void Dispose()
        {
            //see if service client is disposible and clean up as required
            var disposibleClient = ServiceClient as IDisposable;
            if (disposibleClient != null)
            {
                disposibleClient.Dispose();
            }
        }
    }
}