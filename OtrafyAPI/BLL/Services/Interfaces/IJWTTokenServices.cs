using DAL.Models;
using BLL.Models;

namespace BLL.Services.Interfaces
{
    public interface IJWTTokenServices
    {
        TokenResponseModel GeneralJWTToken(string refreshtoken, User model);
    }
}
