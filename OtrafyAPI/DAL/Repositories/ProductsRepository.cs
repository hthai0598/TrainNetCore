using DAL.Repositories.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class ProductsRepository : Repository<Products>, IProductsRepository
    {
        public ProductsRepository(IMongoContext context) : base(context)
        {
        }
    }
}