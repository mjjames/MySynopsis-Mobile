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
            var vm = new RecordReadingsViewModel(user);
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
            var vm = new RecordReadingsViewModel(user);
            Assert.False(vm.RecordReadings.CanExecute(null));
        }

        [Fact]
        public void RecordReadingsAvailableWhenAllReadingsPopulated()
        {
            var user = GetUser();
            var vm = new RecordReadingsViewModel(user);
            vm.MeterReadings.First().Reading = 6780;
            vm.MeterReadings.Last().Reading = 6785;

            Assert.True(vm.RecordReadings.CanExecute(null));
        }

        [Fact]
        public void MeterReadingsPopulated()
        {
            var user = GetUser();
            var vm = new RecordReadingsViewModel(user);
            Assert.Equal(2, vm.MeterReadings.Count);
        }
    }
}
