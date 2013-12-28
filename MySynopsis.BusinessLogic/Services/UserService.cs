using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private IMobileServiceClient _serviceClient;
        public UserService(IMobileServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<User> Persist(User user) {
            var table = _serviceClient.GetTable<User>();
            if (user.Id == 0)
            {
                await table.InsertAsync(user);
                var users = await table.Where(u => u.UserId == _serviceClient.CurrentUser.UserId).ToEnumerableAsync();
                return users.First();
            }
            else
            {
                await table.UpdateAsync(user);
                return user;
            }
        }

        public async Task<User> ByUserId(string userId)
        {
            var matchingUsers = await _serviceClient.GetTable<User>().Where(u => u.UserId == userId).Take(1).ToEnumerableAsync();
            return matchingUsers.FirstOrDefault();
        }
    }
}
