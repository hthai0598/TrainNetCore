using System;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(Token item);
        bool VerifyIsValidToken(string Id);
        Task<Token> GetToken(Guid id);
    }
}
