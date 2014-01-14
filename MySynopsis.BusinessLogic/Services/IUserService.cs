using System;
using System.Threading.Tasks;
namespace MySynopsis.BusinessLogic.Services
{
    public interface IUserService
    {
        Task<User> ByUserId(string userId);
        Task<User> Persist(User user);
        User CurrentUser { get; }
    }
}
