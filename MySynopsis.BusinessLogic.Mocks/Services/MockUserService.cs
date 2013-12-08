using Microsoft.WindowsAzure.MobileServices;
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Mocks
{
    public class MockUserService : IUserService
    {
        private ExpectedUserStatus _expectedUserStatus;

        private Task<User> NetworkException
        {
            get
            {
                var result = new TaskCompletionSource<User>();
                result.SetException(new HttpRequestException());
                return result.Task;
            }
        }

        private Task<User> AuthenticationException
        {
            get
            {
                var result = new TaskCompletionSource<User>();
                result.SetException(new HttpRequestException());
                return result.Task;
            }
        }

        private Task<User> UnregisteredUser
        {
            get
            {
                return Task.FromResult<User>(null);
            }
        }

        private Task<User> RegisteredUser1Elec1Gas1Water
        {
            get
            {
                return Task.FromResult(new User
                {
                    EmailAddress = "test@test.com",
                    Id = 4,
                    MeterConfiguration ={
                        new Meter{
                            Name = "Office Elec",
                            Type = MeterType.Electricity,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.10M,
                                    UnitRate = 0.12M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        },
                         new Meter{
                            Name = "Office Gas",
                            Type = MeterType.Gas,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.25M,
                                    UnitRate = 0.15M,
                                    Type = RateType.AllDate    
                                }    
                            }
                         },
                        new Meter{
                            Name = "Office Water",
                            Type = MeterType.Water,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.15M,
                                    UnitRate = 0.05M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        }
                    },
                    Name = "Test User",
                    SignedUpUtc = DateTime.UtcNow,
                    UserId = "tytryutyutyutyutryu"
                });
            }
        }


        private Task<User> RegisteredUser1Elec1Gas0Water
        {
            get
            {
                return Task.FromResult(new User
                {
                    EmailAddress = "test@test.com",
                    Id = 4,
                    MeterConfiguration ={
                        new Meter{
                            Name = "Office Elec",
                            Type = MeterType.Electricity,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.10M,
                                    UnitRate = 0.12M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        },
                         new Meter{
                            Name = "Office Gas",
                            Type = MeterType.Gas,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.25M,
                                    UnitRate = 0.15M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        }
                    },
                    Name = "Test User",
                    SignedUpUtc = DateTime.UtcNow,
                    UserId = "tytryutyutyutyutryu"
                });
            }
        }
        private Task<User> RegisteredUser1Elec0Gas1Water
        {
            get
            {
                return Task.FromResult(new User
                {
                    EmailAddress = "test@test.com",
                    Id = 4,
                    MeterConfiguration ={
                        new Meter{
                            Name = "Office Elec",
                            Type = MeterType.Electricity,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.10M,
                                    UnitRate = 0.12M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        },
                        new Meter{
                            Name = "Office Water",
                            Type = MeterType.Water,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.15M,
                                    UnitRate = 0.05M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        }
                    },
                    Name = "Test User",
                    SignedUpUtc = DateTime.UtcNow,
                    UserId = "tytryutyutyutyutryu"
                });
            }
        }
        private Task<User> RegisteredUser2Elec1Gas1Water
        {
            get
            {
                return Task.FromResult(new User
                {
                    EmailAddress = "test@test.com",
                    Id = 4,
                    MeterConfiguration ={
                        new Meter{
                            Name = "Office Elec",
                            Type = MeterType.Electricity,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.10M,
                                    UnitRate = 0.12M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        },
                         new Meter{
                            Name = "Workshop Elec",
                            Type = MeterType.Electricity,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.10M,
                                    UnitRate = 0.12M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        },
                         new Meter{
                            Name = "Office Gas",
                            Type = MeterType.Gas,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.25M,
                                    UnitRate = 0.15M,
                                    Type = RateType.AllDate    
                                }    
                            }
                         },
                        new Meter{
                            Name = "Office Water",
                            Type = MeterType.Water,
                            Rates = {
                                new MeterRate{
                                    Start = DateTime.UtcNow,
                                    StandingCharge = 0.15M,
                                    UnitRate = 0.05M,
                                    Type = RateType.AllDate    
                                }    
                            }
                        }
                    },
                    Name = "Test User",
                    SignedUpUtc = DateTime.UtcNow,
                    UserId = "tytryutyutyutyutryu"
                });
            }
        }


        public void SetExpectedUserId(ExpectedUserStatus status)
        {
            _expectedUserStatus = status;
        }

        public Task<User> ByUserId(string userId)
        {
            switch (_expectedUserStatus)
            {
                case ExpectedUserStatus.AuthenticationException:
                    return AuthenticationException;
                case ExpectedUserStatus.NetworkException:
                    return NetworkException;
                case ExpectedUserStatus.RegisteredUser1Elec0Gas1Water:
                    return RegisteredUser1Elec0Gas1Water;
                case ExpectedUserStatus.RegisteredUser1Elec1Gas0Water:
                    return RegisteredUser1Elec1Gas0Water;
                case ExpectedUserStatus.RegisteredUser1Elec1Gas1Water:
                    return RegisteredUser1Elec1Gas1Water;
                case ExpectedUserStatus.RegisteredUser2Elec1Gas1Water:
                    return RegisteredUser2Elec1Gas1Water;
                default:
                    return UnregisteredUser;
            }
        }

        public Task<User> Persist(User user)
        {
            throw new NotImplementedException();
        }

        public enum ExpectedUserStatus
        {
            NetworkException,
            AuthenticationException,
            UnregisteredUser,
            RegisteredUser1Elec1Gas1Water,
            RegisteredUser1Elec1Gas0Water,
            RegisteredUser1Elec0Gas1Water,
            RegisteredUser2Elec1Gas1Water
        }
    }
}
