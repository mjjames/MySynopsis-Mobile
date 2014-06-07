using MySynopsis.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Services
{
    public interface IDataReadingService
    {
        Task PersistReadings(IEnumerable<DataReading> readings);
        Task<IList<Models.BaseDataReading>> ReadingsForMeterAndDateRange(Guid meterId, TimePeriod period);
    }

    public enum TimePeriod
    {
        Week,
        Month,
        Quarter,
        Year
    }
}
