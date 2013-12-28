using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.QuickUI;

namespace MySynopsis.UI.Pages
{
    public class HomePage : ContentPage
    {
        public HomePage()
        {
            Title = "Home";
            Navigation.PushModal(PageLocator.Get<LoginPage>(this));
        }
    }
}
