using MySynopsis.BusinessLogic.Models;
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class RecentReadingsViewModel : ViewModelBase
    {
        public RecentReadingsViewModel(IDataReadingService dataReadingService)
        {
            ThisMonth = new Dictionary<string, ObservableCollection<BaseDataReading>>();
            ThisQuarter = new Dictionary<string, ObservableCollection<BaseDataReading>>();
            ThisWeek = new Dictionary<string, ObservableCollection<BaseDataReading>>();
            ThisYear = new Dictionary<string, ObservableCollection<BaseDataReading>>();
            _dataReadingService = dataReadingService;
        }

        private User _user;
        private UsageViewModel _usage;
        private IDataReadingService _dataReadingService;

        public User User
        {
            get { return _user; }
            set
            {
                if (_user == value)
                {
                    return;
                }
                _user = value;
                UpdateRecentReadings();
            }
        }

        public Dictionary<string, ObservableCollection<BaseDataReading>> ThisWeek { get; private set; }
        public Dictionary<string, ObservableCollection<BaseDataReading>> ThisMonth { get; private set; }
        public Dictionary<string, ObservableCollection<BaseDataReading>> ThisQuarter { get; private set; }
        public Dictionary<string, ObservableCollection<BaseDataReading>> ThisYear { get; private set; }

        public UsageViewModel Usage
        {
            get
            {
                return _usage;
            }
            set
            {
                if (_usage == value)
                {
                    return;
                }
                _usage = value;
                NotifyPropertyChanged();
            }
        }

        private void UpdateRecentReadings()
        {
            foreach (var meter in User.MeterConfiguration)
            {
                IEnumerable<double> readings = new double[]{};
                if(meter.Name == "Gas"){
                    readings = new[]{
                        18554.908,18555.567,18556.041,18556.572,18557.110,18557.290,18557.541,18558.001,18558.257,18558.839,18559.219,18559.494,18559.671,18559.800,18559.930,18560.147,18560.347,18560.633             
                    };
                }
                if(meter.Name == "Water"){
                    readings = new[]{
                        13.4,15.3,17.6,19.4,21.2,23.8,26.1,27.9,29.9,31.9,33.1,35.5,37.5,39.3,41.3,43.2,44.9
                    };
                }
                if(meter.Name == "Elec"){
                    readings = new double[]{
                        28488,28738,28987,29233,29503,29793,30041,30336,30538,30743,30912,31086,31275,31449,31553,31682,31775,31864
                    };
                }
                //fake data service
                var data = readings.Select((reading, index) => new BaseDataReading
                {
                    MeterId = meter.Id,
                    Reading = reading,
                    TimeStampUtc = GetDate(index + 1, 2013),
                });

                ThisMonth.Add(meter.Name, new ObservableCollection<BaseDataReading>(data.Take(5)));
                ThisQuarter.Add(meter.Name, new ObservableCollection<BaseDataReading>(data.Take(12)));
                ThisYear.Add(meter.Name, new ObservableCollection<BaseDataReading>(data));
                ThisWeek.Add(meter.Name, new ObservableCollection<BaseDataReading>(data.Take(2)));
                //var vm = new RecentMeterReadingsViewModel(meter.Id, User.Id, _dataReadingService);
                //ThisWeek.Add(meter.Name, vm.WithinAWeek);
                //ThisMonth.Add(meter.Name, vm.WithinAMonth);
                //ThisQuarter.Add(meter.Name, vm.WithinAQuarter);
                //ThisYear.Add(meter.Name, vm.WithinAYear);
            }
        }

        private DateTime GetDate(int weekNumber, int year)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekNumber;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}
