using MySynopsis.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class DataReadingViewModel : ViewModelBase
    {
        private Meter _meter;
        private Guid _userId;
        private long _reading;
        public DataReadingViewModel(Meter meter, Guid userId)
        {
            _meter = meter;
            _userId = userId;
        }

        public string MeterName { get { return _meter.Name; } }
        public long Reading
        {
            get { return _reading; }
            set
            {
                if (value == _reading)
                {
                    return;
                }
                _reading = value;
                NotifyPropertyChanged();
            }
        }
        
        public DataReading ToDataReading()
        {
            return new DataReading
            {
                MeterId = _meter.Id,
                Reading = Reading,
                TimeStampUtc = DateTime.UtcNow,
                UserId = _userId
            };
        }
    }
}
