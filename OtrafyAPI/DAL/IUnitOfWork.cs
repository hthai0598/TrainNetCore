using System;
using System.Threading.Tasks;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository companyRepository { get; }
        IAuthRepository authRepository { get; }
        IRefreshTokenRepository refreshTokenRepository { get; }
        IAccountRepository accountRepository { get; }
        ITokenRepository tokenRepository { get; }
        IBuyersRepository buyersRepository { get; }
        ISuppliersRepository suppliersRepository { get; }

        Task<bool> CommitAsync();
        bool Commit();
    }
}
