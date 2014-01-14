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
            var layout = new GridLayout
            {
                DefaultColumnSpacing = 10,
                DefaultRowSpacing = 5
            };
            foreach(var reading in _viewModel.MeterReadings){
                layout.AddVertical(new Label
                {
                    Text = reading.MeterName
                });
                var readingEntry = new Entry{
                    BindingContext = reading
                };
                readingEntry.SetBinding<DataReadingViewModel>(Entry.TextProperty, vm => vm.Reading, BindingMode.TwoWay, new LongConverter());
                layout.Add(readingEntry);
                GridLayout.SetColumn(readingEntry, 1);
            }

            var persist = new Button{
                Text = PageResources.RecordReading,
            };
            persist.SetBinding<RecordReadingsViewModel>(Button.CommandProperty, vm => vm.RecordReadings);
            layout.AddVertical(persist);
         }
    }
}
