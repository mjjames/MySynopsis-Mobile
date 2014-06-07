using MySynopsis.BusinessLogic.Models;
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Mocks.Services
{
    public class MockDataReadingService : IDataReadingService
    {
        public Action<IEnumerable<DataReading>> PersistAction { get; set; }

        public Task PersistReadings(IEnumerable<DataReading> readings)
        {
            PersistAction(readings);
            return Task.FromResult(new object());
        }

        public Task<IList<BaseDataReading>> ReadingsForMeterAndDateRange(Guid meterId, TimePeriod period)
        {
            throw new NotImplementedException();
        }
    }
}
