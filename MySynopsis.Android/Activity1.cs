﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MySynopsis.UI;
using MySynopsis.UI.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace MySynopsis.Android
{
    [Activity(Label = "mySynopsis", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class StartupActivity : AndroidActivity
    {
        private static bool _initialised;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            if (!_initialised)
            {
                Factory.RegisterPages(this);
                _initialised = true;
            }
            //SetPage(new NavigationPage(new TestPage()));

            //SetPage(new NavigationPage(PageLocator.Get<RegistrationPage>(new MySynopsis.BusinessLogic.User
            //{
            //    EmailAddress = "m@j.com",
            //    UserId = "68767868767868"
            //})));
            
            SetPage(PageLocator.Get<TabbedPage>());
        }
    }
}

