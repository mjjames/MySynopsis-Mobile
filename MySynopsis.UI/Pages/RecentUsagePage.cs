using MySynopsis.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MySynopsis.UI.Pages
{
    public class RecentUsagePage : CarouselPage
    {
        public RecentUsagePage(string rootPath, User user)
        {
            
            this.Children.Add(GetPage("This Week", rootPath, user.Id));
            this.Children.Add(GetPage("This Month", rootPath, user.Id));
            this.Children.Add(GetPage("This Quarter", rootPath, user.Id));
            this.Children.Add(GetPage("This Year", rootPath, user.Id));

        }
        private ContentPage GetPage(string title, string rootPath, Guid id){
            return new ContentPage
            {
                Content = new StackLayout
                {
                    Children = {
                        new Label{
                            Text = title ,
                             Font = Font.OfSize("sans-serif-light", 40),
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new WebView
                        {

                            Source = new UrlWebViewSource
                            {
                                Url = System.IO.Path.Combine(rootPath, string.Format("RecentUsage.html#{0}/{1}", title.Replace(" ", "-"), id))
                            },
                            VerticalOptions = LayoutOptions.FillAndExpand
                        } 
                    }

                }
            };
        }
    }
}
