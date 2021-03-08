using DAL.Repositories.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        public AccountRepository(IMongoContext context) : base(context)
        {
        }   
    }
}