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

        public async Task<IList<Models.BaseDataReading>> ReadingsForMeterAndDateRange(Guid meterId, TimePeriod period)
        {
            var table = _serviceClient.GetTable<Models.DataReading>();
            var query = table.Where(m => m.MeterId == meterId);
            switch (period)
            {
                case TimePeriod.Week:
                    query = query.Where(m => m.TimeStampUtc >= DateTime.Today.AddDays(-7));
                    break;
                case TimePeriod.Month:
                    query = query.Where(m => m.TimeStampUtc >= new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
                    break;
                case TimePeriod.Quarter:
                    query = query.Where(m => m.TimeStampUtc >= new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-3));
                    break;
                case TimePeriod.Year:
                    query = query.Where(m => m.TimeStampUtc >= new DateTime(DateTime.Today.Year, 1, 1));
                    break;
                default:
                    break;
            }
            var results = await query.OrderBy(o => o.TimeStampUtc).ToListAsync();
            switch (period)
            {
                case TimePeriod.Quarter:
                case TimePeriod.Year:
                    var groupedItems = results.GroupBy(r => r.TimeStampUtc.Month);
                    return groupedItems.Select(g => new Models.BaseDataReading
                    {
                        MeterId = meterId,
                        Reading = g.Max(d => d.Reading) - g.Min(d => d.Reading),
                        TimeStampUtc = new DateTime(g.Min(d => d.TimeStampUtc).Year, g.Min(d => d.TimeStampUtc).Month, 1)
                    }).ToList();
                case TimePeriod.Month:
                case TimePeriod.Week:
                default:
                    return results.Cast<Models.BaseDataReading>().ToList();
            }
        }

      
    }
}
