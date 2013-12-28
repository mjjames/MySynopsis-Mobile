using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Services
{
    public class DataReadingService : IDataReadingService
    {

        private IMobileServiceClient _serviceClient;
        public DataReadingService(IMobileServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task PersistReadings(IEnumerable<Models.DataReading> readings)
        {
            var table = _serviceClient.GetTable<Models.DataReading>();
            var tasks = new List<Task>();
            foreach (var reading in readings)
            {
                tasks.Add(table.InsertAsync(reading));
            }
            await Task.WhenAll(tasks);
        }
    }
}
