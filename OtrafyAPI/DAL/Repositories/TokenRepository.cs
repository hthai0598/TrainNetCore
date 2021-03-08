using DAL.Repositories.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class TokenRepository : Repository<Token>, ITokenRepository
    {
        public TokenRepository(IMongoContext context) : base(context)
        {
        }        
    }
}
