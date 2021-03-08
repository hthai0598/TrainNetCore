using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshJWTToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IMongoContext context) : base(context)
        {
        }
    }
}
