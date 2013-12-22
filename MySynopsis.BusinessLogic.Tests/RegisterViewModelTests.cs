using MySynopsis.BusinessLogic.Mocks;
using MySynopsis.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MySynopsis.BusinessLogic.Tests
{
    public class RegisterViewModelTests
    {
        [Fact]
        public void ConfirmSetupNotAvaliableIfNameNotSet()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            user.EmailAddress = "test@test.com";
            user.MeterConfiguration.Add(new Meter{
                Name = "test",
                Type = MeterType.Electricity
            });
            var vm = new RegisterViewModel(mockService, user);
            Assert.False(vm.ConfirmSetup.CanExecute(null));
        }

        private static User GetUser()
        {
            return new User
            {
                Id = 1,
                SignedUpUtc = DateTime.Now,
                UserId = "4567897897987"
            };
        }

        [Fact]
        public void ConfirmSetupNotAvailableIfUserEmailNotSet()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            user.MeterConfiguration.Add(new Meter
            {
                Name = "test",
                Type = MeterType.Electricity
            });
            var vm = new RegisterViewModel(mockService, user);
            vm.Name = "test";
            Assert.False(vm.ConfirmSetup.CanExecute(null));
        }

        [Fact]
        public void ConfirmSetupNotAvailableIfUserMetersEmpty()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            user.EmailAddress = "test@test.com";
            var vm = new RegisterViewModel(mockService, user);
            vm.Name = "Test";
            Assert.False(vm.ConfirmSetup.CanExecute(null));
        }

        [Fact]
        public void ConfirmSetupAvailableWhenNameEmailAndMetersSet()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            user.EmailAddress = "test@test.com";
            user.MeterConfiguration.Add(new Meter
            {
                Name = "test",
                Type = MeterType.Electricity
            });
            var vm = new RegisterViewModel(mockService, user);
            vm.Name = "Test User";
            Assert.True(vm.ConfirmSetup.CanExecute(null));
        }

        [Fact]
        public void QuickSetup1ActionPopulatesMeters()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            var vm = new RegisterViewModel(mockService, user);
            Assert.Empty(user.MeterConfiguration);
            vm.ConfigureOptionOne.Execute(null);
            Assert.NotEmpty(user.MeterConfiguration);
        }

        [Fact]
        public void QuickSetup2ActionPopulatesMeters()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            var vm = new RegisterViewModel(mockService, user);
            Assert.Empty(user.MeterConfiguration);
            vm.ConfigureOptionTwo.Execute(null);
            Assert.NotEmpty(user.MeterConfiguration);
        }

        [Fact]
        public void QuickSetup3ActionPopulatesMeters()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            var vm = new RegisterViewModel(mockService, user);
            Assert.Empty(user.MeterConfiguration);
            vm.ConfigureOptionThree.Execute(null);
            Assert.NotEmpty(user.MeterConfiguration);
        }

        [Fact]
        public void ConfirmSetupRaisesIsPersistingTrueWhenStarted()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            var vm = new RegisterViewModel(mockService, user);
            vm.Name = "Test";
            user.EmailAddress = "test@test.com";
            vm.ConfigureOptionOne.Execute(null);
            Assert.PropertyChanged(vm, "IsPersisting", () => vm.ConfirmSetup.Execute(null));
        }

        //[Fact]
        //public void ConfirmSetupRaisesIsPersistingFalseWhenComplete()
        //{

        //}

        [Fact]
        public void ConfirmSetupRaisesPostPersistActionWhenSetAndPersistComplete()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            mockService.PersistAction = () => user;

            var vm = new RegisterViewModel(mockService, user);
            vm.Name = "Test";
            user.EmailAddress = "test@test.com";
            vm.ConfigureOptionOne.Execute(null);

            var executed = false;
            vm.PostPersistAction = (persistedUser) =>
            {
                executed = true;
            };
            vm.ConfirmSetup.Execute(null);
            Assert.True(executed);
        }

        [Fact]
        public void ConfirmSetupDoesntThrowWhenPostPersistActionWhenNotSetAndPersistComplete()
        {
            var mockService = new MockUserService();
            var user = GetUser();
            var vm = new RegisterViewModel(mockService, user);
            vm.Name = "Test";
            user.EmailAddress = "test@test.com";
            vm.ConfigureOptionOne.Execute(null);

            var executed = false;
            vm.ConfirmSetup.Execute(null);
            Assert.False(executed);
        }
    }
}
