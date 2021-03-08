using System;
using System.Threading.Tasks;
using DAL.Models;
using BLL.Models;

namespace BLL.Services.Interfaces
{
    public interface IAccountServices
    {
        Task<User> CreateUser(User item);
        
        Task<UserResponseModel> GetUserInfoById(Guid id);
        Task<CommitResult> UpdateProfile(string id, UserProfilesParam model);
        Task<User> FindByEmail(string email);
        Task<bool> IsEmailConfirmed(Guid Id);
        Task<bool> UpdateUser(string Id, User model);
        Task<CommitResult> ActiveInvite(ActiveInviteParam param);
    }
}
