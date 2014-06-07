
using MySynopsis.BusinessLogic.Models;
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class RecordReadingsViewModel : ViewModelBase
    {
        private User _user;
        private DelegateCommand _recordReadings;
        private bool _isPersisting;
        private IDataReadingService _dataReadingService;
        private bool _isFaulted;
        private DelegateCommand _recordTestReadings;
        private ITorchService _torchService;
        private DelegateCommand _toggleTorch;
        public RecordReadingsViewModel(User user, IDataReadingService dataReadingService, ITorchService torchService)
        {
            _user = user;
            MeterReadings = new ObservableCollection<DataReadingViewModel>(_user.MeterConfiguration.Select(m => new DataReadingViewModel(m, _user.Id)));
            MeterReadings.CollectionChanged += (sender, args) =>
            {
                foreach (var newItem in args.NewItems.Cast<DataReadingViewModel>())
                {
                    ReadingPropertyChanged(newItem);
                }
            };
            foreach (var reading in MeterReadings)
            {
                ReadingPropertyChanged(reading);
            }
            _dataReadingService = dataReadingService;
            _torchService = torchService;
        }

        private void ReadingPropertyChanged(DataReadingViewModel reading)
        {
            reading.PropertyChanged += (obj, e) =>
            {
                if (e.PropertyName == "Reading")
                {
                    ((DelegateCommand)RecordReadings).RaiseCanExecuteChanged();
                }
            };
        }

        public ObservableCollection<DataReadingViewModel> MeterReadings { get; private set; }

        public ICommand RecordReadings
        {
            get
            {
                if (_recordReadings == null)
                {
                    _recordReadings = new DelegateCommand(async obj => await PersistReadings(), (obj) =>
                        {
                            return !IsPersisting && MeterReadings.All(m => m.Reading > 0);
                        });
                }
                return _recordReadings;
            }
        }

        public ICommand ToggleTorch
        {
            get
            {
                if (_toggleTorch == null)
                {
                    _toggleTorch = new DelegateCommand(obj => ToggleTorchAction(), (obj) =>
                    {
                        return _torchService.IsTorchAvailable;
                    });
                }
                return _toggleTorch;
            }
        }

        private void ToggleTorchAction()
        {
            if (!_torchService.IsTorchAvailable)
            {
                return;
            }
            var newStatus = _torchService.Status == TorchStatus.Off ? TorchStatus.On : TorchStatus.Off;
            _torchService.TrySetTorchStatus(newStatus);
        }

        public Action PostPersistAction { get; set; }

        public bool IsPersisting
        {
            get
            {
                return _isPersisting;
            }
            set
            {
                if (_isPersisting == value)
                {
                    return;
                }
                _isPersisting = value;
                NotifyPropertyChanged();
                ((DelegateCommand)_recordReadings).RaiseCanExecuteChanged();
            }
        }

        public bool IsFaulted
        {
            get
            {
                return _isFaulted;
            }
            set
            {
                if (_isFaulted == value)
                {
                    return;
                }
                _isFaulted = value;
                NotifyPropertyChanged();
            }
        }
        private async Task PersistReadings()
        {
            IsPersisting = true;
            IsFaulted = false;
            try
            {
                await _dataReadingService.PersistReadings(MeterReadings.Select(m => m.ToDataReading()));
                if (PostPersistAction != null)
                {
                    PostPersistAction();
                }
                ClearReadingValues();
            }
            catch (Exception)
            {
                IsFaulted = true;
            }
            IsPersisting = false;

        }

        private void ClearReadingValues()
        {
            foreach (var reading in MeterReadings)
            {
                reading.Reading = 0;
            }
        }

        public ICommand RecordTestReadings
        {
            get
            {
                if (_recordTestReadings == null)
                {
                    _recordTestReadings = new DelegateCommand(async obj => await PersistTestReadings(), (obj) =>
                    {
                        return !IsPersisting;
                    });
                }
                return _recordTestReadings;
            }
        }

        private async Task PersistTestReadings()
        {
            IsPersisting = true;
            IsFaulted = false;
            try
            {
                await ElecReadings();
                await GasReadings();
                await WaterReadings();
            }
            catch (Exception)
            {
                IsFaulted = true;
            }
            IsPersisting = false;
        }

        private async Task WaterReadings()
        {
            var waterMeterId = Guid.NewGuid();
            //2013
            await _dataReadingService.PersistReadings(new[]{
                    1070.5299,1072.3111,1074.0679,1075.8031,1077.6921,1079.8484,1082.2678,1084.1155,1086.3434,1088.5431,1090.5021,1092.5464,1094.6787,1096.8345,1098.3465,1100.3389,1101.3723,1101.3723,1102.6881,1104.7524,1106.7951,1108.5888,1110.7119,1112.7817,1114.8366,1116.9162,1118.9852,1121.2323,1123.4087,1125.5011,1127.7817,1129.5499,1131.4621,1133.3566,1135.2239,1137.0228,1139.0442,1140.8274,1142.5821,1144.4648,1146.5397,1148.5271,1150.2496,1151.9203,1154.5876,0,3,4.5,6.1,8.1,9.7,11.6                
                }.Select((reading, index) => new DataReading
                {
                    MeterId = waterMeterId,
                    Reading = reading,
                    TimeStampUtc = GetDate(index + 1, 2013),
                    UserId = _user.Id
                }
            ));
            //2014
            await _dataReadingService.PersistReadings(new[]{
                    13.4,15.3,17.6,19.4,21.2,23.8,26.1,27.9,29.9,31.9,33.1,35.5,37.5,39.3,41.3,43.2,44.9
                }.Select((reading, index) => new DataReading
                {
                    MeterId = waterMeterId,
                    Reading = reading,
                    TimeStampUtc = GetDate(index + 1, 2013),
                    UserId = _user.Id
                }
            ));
            System.Diagnostics.Debug.WriteLine("Water Test Readings Persisted, Meter ID: {0}", waterMeterId);
        }

        private async Task GasReadings()
        {
            var gasMeterId = Guid.NewGuid();
            ////2013 readings
            await _dataReadingService.PersistReadings(new[]{
                    18538.012,18538.228,18538.406,18538.702,18538.897,18539.125,18539.496,18539.924,18540.236,18540.745,18541.326,18541.626,18541.873,18542.432,18542.567,18542.827,18542.990,18542.990,18543.180,18543.809,18544.124,18544.328,18544.657,18545.007,18545.648,18546.259,18546.477,18546.608,18547.057,18547.275,18547.648,18548.106,18548.519,18548.871,18549.070,18549.529,18549.987,18550.397,18550.400,18551.075,18551.697,18552.227,18552.493,18552.762,18553.421,18553.468,18553.745,18553.849,18554.034,18554.399,18554.779,18554.908
                }.Select((reading, index) => new DataReading
                {
                    MeterId = gasMeterId,
                    Reading = reading,
                    TimeStampUtc = GetDate(index + 1, 2013),
                    UserId = _user.Id
                }
            ));
            ////2014
            await _dataReadingService.PersistReadings(new[]{
                    18554.908,18555.567,18556.041,18556.572,18557.110,18557.290,18557.541,18558.001,18558.257,18558.839,18559.219,18559.494,18559.671,18559.800,18559.930,18560.147,18560.347,18560.633             
                }.Select((reading, index) => new DataReading
                {
                    MeterId = gasMeterId,
                    Reading = reading,
                    TimeStampUtc = GetDate(index + 1, 2014),
                    UserId = _user.Id
                }
            ));

            System.Diagnostics.Debug.WriteLine("Gas Test Readings Persisted, Meter ID: {0}", gasMeterId);
        }

        private async Task ElecReadings()
        {
            var elecMeterId = Guid.NewGuid();
            //2013 readings
            await _dataReadingService.PersistReadings(new[]{
                    20735,21000,21356,21682,21897,22195,22477,22763,22985,23234,23460,23762,23977,24146,24310,24411,24512,24596,24648,24769,24857,24921,24947,24998,25039,25100,25146,25167,25201,25227,25262,25303,25355,25399,25445,25535,25646,25785,25869,25964,26094,26256,26380,26562,26794,27006,27267,27505,27735,27961,28203,28488
                }.Select((reading, index) => new DataReading
                {
                    MeterId = elecMeterId,
                    Reading = reading,
                    TimeStampUtc = GetDate(index + 1, 2013),
                    UserId = _user.Id
                }
            ));
            //2014 readings
            await _dataReadingService.PersistReadings(new[]{
                    28488,28738,28987,29233,29503,29793,30041,30336,30538,30743,30912,31086,31275,31449,31553,31682,31775,31864
                }.Select((reading, index) => new DataReading
                {
                    MeterId = elecMeterId,
                    Reading = reading,
                    TimeStampUtc = GetDate(index + 1, 2014),
                    UserId = _user.Id
                }
            ));
            System.Diagnostics.Debug.WriteLine("Elec Test Readings Persisted, Meter ID: {0}", elecMeterId);
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
