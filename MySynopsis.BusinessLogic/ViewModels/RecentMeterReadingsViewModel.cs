using MySynopsis.BusinessLogic.Models;
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class RecentMeterReadingsViewModel : ViewModelBase
    {
        private Guid _userId;
        private Guid _meterReadingId;
        private IDataReadingService _dataReadingService;
        public RecentMeterReadingsViewModel(Guid meterReadingId, Guid userId, IDataReadingService dataReadingService)
        {
            _userId = userId;
            _meterReadingId = meterReadingId;
            _dataReadingService = dataReadingService;
            WithinAMonth = new ObservableCollection<BaseDataReading>();
            WithinAQuarter = new ObservableCollection<BaseDataReading>();
            WithinAWeek = new ObservableCollection<BaseDataReading>();
            WithinAYear = new ObservableCollection<BaseDataReading>();
            GetReadings();

        }

        private void GetReadings()
        {
            _dataReadingService.ReadingsForMeterAndDateRange(_meterReadingId, TimePeriod.Week).ContinueWith(r =>
            {
                if (!r.IsCompleted)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to Retrieve Readings");
                    return;
                }
                foreach (var item in r.Result)
                {
                    WithinAWeek.Add(item);
                }
            });
            _dataReadingService.ReadingsForMeterAndDateRange(_meterReadingId, TimePeriod.Month).ContinueWith(r =>
            {
                if (!r.IsCompleted)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to Retrieve Readings");
                    return;
                }
                foreach (var item in r.Result)
                {
                    WithinAMonth.Add(item);
                }
            });
            _dataReadingService.ReadingsForMeterAndDateRange(_meterReadingId, TimePeriod.Quarter).ContinueWith(r =>
            {
                if (!r.IsCompleted)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to Retrieve Readings");
                    return;
                }
                foreach (var item in r.Result)
                {
                    WithinAQuarter.Add(item);
                }
            });
            _dataReadingService.ReadingsForMeterAndDateRange(_meterReadingId, TimePeriod.Year).ContinueWith(r =>
            {
                if (!r.IsCompleted)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to Retrieve Readings");
                    return;
                }
                foreach (var item in r.Result)
                {
                    WithinAYear.Add(item);
                }
            });
        }

        public ObservableCollection<BaseDataReading> WithinAWeek { get; private set; }
        public ObservableCollection<BaseDataReading> WithinAMonth { get; private set; }
        public ObservableCollection<BaseDataReading> WithinAQuarter { get; private set; }
        public ObservableCollection<BaseDataReading> WithinAYear { get; private set; }

    }
}
