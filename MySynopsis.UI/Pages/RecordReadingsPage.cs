using MySynopsis.BusinessLogic.ViewModels;
using MySynopsis.UI.ValueConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.QuickUI;

namespace MySynopsis.UI.Pages
{
    public class RecordReadingsPage : ContentPage
    {
        private RecordReadingsViewModel _viewModel;
        public RecordReadingsPage(RecordReadingsViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Title = PageResources.RecordReadingsTitle;
            BindingContext = _viewModel;

            SetBinding(TitleProperty, new Binding("Title", BindingMode.OneWay));
            SetBinding(IsBusyProperty, new Binding("IsPersisting", BindingMode.OneWay));
            _viewModel.PropertyChanged += async (sender, args) =>{
                if(args.PropertyName == "IsFaulted" && _viewModel.IsFaulted){
                    await DisplayAlert("Perist Reading Failure", "Sorry, We were unable to persist your readings at this time. Please try again.", "OK", "");
                }
            };

            var layout = new GridLayout
            {
                DefaultColumnSpacing = 10,
                DefaultRowSpacing = 20,
                Padding = new Thickness(5, 10)
            };
            
            for (var i = 0; i < _viewModel.MeterReadings.Count; i++)
            {
                var reading = _viewModel.MeterReadings[i];
                layout.AddVertical(new Label
                {
                    Text = reading.MeterName,
                    VerticalOptions = LayoutOptions.Center
                });
                var readingEntry = new Entry
                {
                    BindingContext = reading,
                    Placeholder = "000000",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                readingEntry.SetBinding<DataReadingViewModel>(Entry.TextProperty, vm => vm.Reading, BindingMode.TwoWay, new LongConverter());
                layout.Add(readingEntry, 1, i);
            }

            var persist = new Button
            {
                Text = PageResources.RecordReading,
            };
            persist.SetBinding<RecordReadingsViewModel>(Button.CommandProperty, vm => vm.RecordReadings);
            
            layout.AddVertical(persist);
            Content = layout;
        }
    }
}
