using DAL.Repositories.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class SuppliersRepository : Repository<Suppliers>, ISuppliersRepository
    {
        public SuppliersRepository(IMongoContext context) : base(context)
        {

        }
    }
}
