using Android.App;
using Android.OS;
using MySynopsis.UI.Pages;
using Xamarin.QuickUI;
using Xamarin.QuickUI.Platform.Android;

namespace MySynopsis.Android
{
    [Activity(Label = "MySynopsis.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            QuickUI.Init(this, bundle);
            SetPage(new NavigationPage(new LoginPage(Factory.GetLoginViewModel(this))));
        }
    }
}

