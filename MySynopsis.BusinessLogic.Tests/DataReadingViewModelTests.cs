using MySynopsis.BusinessLogic.Models;
using MySynopsis.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MySynopsis.BusinessLogic.Tests
{
    public class DataReadingViewModelTests
    {
        [Fact]
        public void MeterNamePopulated()
        {
            var meter = new Meter
            {
                Id = Guid.NewGuid(),
                Name = "Test Meter",
                Type = MeterType.Electricity
            };

            var vm = new DataReadingViewModel(meter, 678);
            Assert.Equal(meter.Name, vm.MeterName);
        }

        [Fact]
        public void ReadingRaisesNotifiesValueChanged()
        {
            var meter = new Meter
            {
                Id = Guid.NewGuid(),
                Name = "Test Meter",
                Type = MeterType.Electricity
            };

            var vm = new DataReadingViewModel(meter, 678);
            Assert.PropertyChanged(vm, "Reading", () => vm.Reading = 567);
        }

        [Fact]
        public void ToDataReadingMeterIdPopulated()
        {
            var meterId = Guid.NewGuid();
            var reading = new DataReading
                        {
                            Id = 5,
                            MeterId = meterId,
                            Reading = 654321,
                            TimeStampUtc = DateTime.UtcNow,
                            UserId = 678
                        };
            var meter = new Meter
            {
                Id = meterId,
                Name = "Test Meter",
                Type = MeterType.Electricity
            };

            var vm = new DataReadingViewModel(meter, 678)
            {
                Reading = 6789
            };

            var result = vm.ToDataReading();
            Assert.Equal(meterId, result.MeterId);
        }

        [Fact]
        public void ToDataReadingReadingPopulated()
        {
            var meterId = Guid.NewGuid();
            var reading = new DataReading
            {
                Id = 5,
                MeterId = meterId,
                Reading = 654321,
                TimeStampUtc = DateTime.UtcNow,
                UserId = 678
            };
            var meter = new Meter
            {
                Id = meterId,
                Name = "Test Meter",
                Type = MeterType.Electricity
            };

            var vm = new DataReadingViewModel(meter, 678)
            {
                Reading = 6789
            };

            var result = vm.ToDataReading();
            Assert.Equal(6789, result.Reading);
        }

        [Fact]
        public void ToDataReadingUserIdPopulated()
        {
            var meterId = Guid.NewGuid();
            var reading = new DataReading
            {
                Id = 5,
                MeterId = meterId,
                Reading = 654321,
                TimeStampUtc = DateTime.UtcNow,
                UserId = 678
            };
            var meter = new Meter
            {
                Id = meterId,
                Name = "Test Meter",
                Type = MeterType.Electricity
            };

            var vm = new DataReadingViewModel(meter, 678)
            {
                Reading = 6789
            };

            var result = vm.ToDataReading();
            Assert.Equal(678, result.UserId);
        }

        [Fact]
        public void ToDataReadingDateTimePopulated()
        {
            var meterId = Guid.NewGuid();
            var reading = new DataReading
            {
                Id = 5,
                MeterId = meterId,
                Reading = 654321,
                TimeStampUtc = DateTime.UtcNow,
                UserId = 678
            };
            var meter = new Meter
            {
                Id = meterId,
                Name = "Test Meter",
                Type = MeterType.Electricity
            };

            var vm = new DataReadingViewModel(meter, 678)
            {
                Reading = 6789
            };

            var result = vm.ToDataReading();
            Assert.NotEqual(DateTime.MinValue, result.TimeStampUtc);
        }
    }
}


