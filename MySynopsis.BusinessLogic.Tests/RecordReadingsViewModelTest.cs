using MySynopsis.BusinessLogic.Mocks.Services;
using MySynopsis.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MySynopsis.BusinessLogic.Tests
{
    public class RecordReadingsViewModelTest
    {
        [Fact]
        public void RecordReadingsNotAvailbleWhenOneReadingMissing()
        {
            var user = GetUser();
            var mockService = new MockDataReadingService();
            var vm = new RecordReadingsViewModel(user, mockService);
            vm.MeterReadings.First().Reading = 6780;
            Assert.False(vm.RecordReadings.CanExecute(null));
        }

        private static User GetUser()
        {
            var user = new User
            {
                Id = 7,
                EmailAddress = "test@test.com",
                Name = "Test",
                SignedUpUtc = DateTime.UtcNow,
                UserId = "67868768768768",
                MeterConfiguration =
                {
                    new Meter{
                        Id = Guid.NewGuid(),
                        Name = "Elec 1",
                        Type = MeterType.Electricity
                    },
                    new Meter{
                        Id = Guid.NewGuid(),
                        Name = "Gas 1",
                        Type = MeterType.Gas
                    }
                }
            };
            return user;
        }

        [Fact]
        public void RecordReadingsNotAvailableWhenAllReadingsMissing()
        {
            var user = GetUser();
            var mockService = new MockDataReadingService();
            var vm = new RecordReadingsViewModel(user, mockService);
            Assert.False(vm.RecordReadings.CanExecute(null));
        }

        [Fact]
        public void RecordReadingsAvailableWhenAllReadingsPopulated()
        {
            var user = GetUser();
            var mockService = new MockDataReadingService();
            var vm = new RecordReadingsViewModel(user, mockService);
            vm.MeterReadings.First().Reading = 6780;
            vm.MeterReadings.Last().Reading = 6785;

            Assert.True(vm.RecordReadings.CanExecute(null));
        }

        [Fact]
        public void MeterReadingsPopulated()
        {
            var user = GetUser();
            var mockService = new MockDataReadingService();
            var vm = new RecordReadingsViewModel(user, mockService);
            Assert.Equal(2, vm.MeterReadings.Count);
        }

        [Fact]
        public void PersistReadingsRaisesIsPersistingTrueWhenStarted()
        {
            var mockService = new MockDataReadingService();
            var user = GetUser();
            var vm = new RecordReadingsViewModel(user, mockService);
         
            Assert.PropertyChanged(vm, "IsPersisting", () => vm.RecordReadings.Execute(null));
        }

        [Fact]
        public void PersistReadingsRaisesPostPersistActionWhenSetAndPersistComplete()
        {
            var mockService = new MockDataReadingService();
            var user = GetUser();
            mockService.PersistAction = (readings) => { };

            var vm = new RecordReadingsViewModel(user, mockService);

            var executed = false;
            vm.PostPersistAction = () =>
            {
                executed = true;
            };
            vm.RecordReadings.Execute(null);
            Assert.True(executed);
        }

        [Fact]
        public void PersistReadingsDoesntThrowWhenPostPersistActionWhenNotSetAndPersistComplete()
        {
            var mockService = new MockDataReadingService();
            var user = GetUser();
            var vm = new RecordReadingsViewModel(user, mockService);
      
            var executed = false;
            vm.RecordReadings.Execute(null);
            Assert.False(executed);
        }
    }
}
