using Microsoft.WindowsAzure.MobileServices;
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MySynopsis.BusinessLogic.Tests
{
    public class UserLoginServiceTests
    {
        [Fact]
        public async Task ExceptionDuringAuthReturnsAuthenticationFailedResult()
        {
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> login =  (provider) =>
            {
                throw new HttpRequestException();
            };
            var service = new UserLoginService(new Mocks.MockUserService(), login);
            var result = await service.Login(MobileServiceAuthenticationProvider.MicrosoftAccount);
            
            Assert.True(result.AuthenticationFailed);
            Assert.IsType<HttpRequestException>(result.AuthenticationException);
        }

        [Fact]
        public async Task AuthenticationExceptionDuringUserLookupReturnsAuthenticationFailedResult()
        {
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> login = (provider) =>
            {
                return Task.FromResult(new MobileServiceUser("56565467546757"));
            };
            var mockService = new Mocks.MockUserService();
            mockService.SetExpectedUserId(Mocks.MockUserService.ExpectedUserStatus.AuthenticationException);
            var service = new UserLoginService(mockService, login);
            var result = await service.Login(MobileServiceAuthenticationProvider.MicrosoftAccount);

            Assert.True(result.AuthenticationFailed);
            Assert.IsType<HttpRequestException>(result.AuthenticationException);
        }

        [Fact]
        public async Task NetworkExceptionDuringUserLookupReturnsAuthenticationFailedResult()
        {
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> login = (provider) =>
            {
                return Task.FromResult(new MobileServiceUser("56565467546757"));
            };
            var mockService = new Mocks.MockUserService();
            mockService.SetExpectedUserId(Mocks.MockUserService.ExpectedUserStatus.NetworkException);
            var service = new UserLoginService(mockService, login);
            var result = await service.Login(MobileServiceAuthenticationProvider.MicrosoftAccount);

            Assert.True(result.AuthenticationFailed);
            Assert.IsType<HttpRequestException>(result.AuthenticationException);
        }

        [Fact]
        public async Task UserLookupReturnsNotRegisteredResult()
        {
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> login = (provider) =>
            {
                return Task.FromResult(new MobileServiceUser("56565467546757"));
            };
            var mockService = new Mocks.MockUserService();
            mockService.SetExpectedUserId(Mocks.MockUserService.ExpectedUserStatus.UnregisteredUser);
            var service = new UserLoginService(mockService, login);
            var result = await service.Login(MobileServiceAuthenticationProvider.MicrosoftAccount);

            Assert.True(result.RequiresRegistration);
            Assert.NotNull(result.UserDetails);
            Assert.Equal(0, result.UserDetails.Id);
        }

        [Fact]
        public async Task UserLookupReturnsRegisteredResult()
        {
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> login = (provider) =>
            {
                return Task.FromResult(new MobileServiceUser("56565467546757"));
            };
            var mockService = new Mocks.MockUserService();
            mockService.SetExpectedUserId(Mocks.MockUserService.ExpectedUserStatus.RegisteredUser1Elec1Gas1Water);
            var service = new UserLoginService(mockService, login);
            var result = await service.Login(MobileServiceAuthenticationProvider.MicrosoftAccount);

            Assert.False(result.RequiresRegistration);
            Assert.NotNull(result.UserDetails);
            Assert.NotNull(result.User);
        }
    }
}
