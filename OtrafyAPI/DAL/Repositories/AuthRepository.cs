using DAL.Repositories.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        public AuthRepository(IMongoContext context) : base(context)
        {
        }
    }
}