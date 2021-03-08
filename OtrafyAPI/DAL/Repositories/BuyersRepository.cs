using DAL.Repositories.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class BuyersRepository : Repository<Buyers >, IBuyersRepository
    {
        public BuyersRepository(IMongoContext context) : base(context)
        {

        }
    }
}
