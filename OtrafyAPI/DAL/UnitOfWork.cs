using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Repositories.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoContext _context;
        ICompanyRepository _companies;
        IAuthRepository _auth;
        IRefreshTokenRepository _refreshToken;
        IAccountRepository _account;
        ITokenRepository _token;
        IBuyersRepository _buyers;
        ISuppliersRepository _supplier;

        public UnitOfWork(IMongoContext context)
        {
            _context = context;
        }

        public ICompanyRepository companyRepository
        {
            get
            {
                if (_companies == null)
                    _companies = new CompanyRepository(_context);

                return _companies;
            }
        }

        public IAuthRepository authRepository
        {
            get
            {
                if (_auth == null)
                    _auth = new AuthRepository(_context);

                return _auth;
            }
        }

        public IRefreshTokenRepository refreshTokenRepository
        {
            get
            {
                if (_refreshToken == null)
                    _refreshToken = new RefreshTokenRepository(_context);

                return _refreshToken;
            }
        }

        public IAccountRepository accountRepository
        {
            get
            {
                if (_account == null)
                    _account = new AccountRepository(_context);

                return _account;
            }
        }

        public ITokenRepository tokenRepository
        {
            get
            {
                if (_token == null)
                    _token = new TokenRepository(_context);

                return _token;
            }
        }

        public IBuyersRepository buyersRepository
        {
            get
            {
                if (_buyers == null)
                    _buyers = new BuyersRepository(_context);

                return _buyers;
            }
        }

        public ISuppliersRepository suppliersRepository
        {
            get
            {
                if (_supplier == null)
                    _supplier = new SuppliersRepository(_context);

                return _supplier;
            }
        }              

        public async Task<bool> CommitAsync()
        {
            //var changeAmount = await _context.SaveChangesAsync();
            return _context.SaveChanges() > 0;
            //return changeAmount > 0;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
