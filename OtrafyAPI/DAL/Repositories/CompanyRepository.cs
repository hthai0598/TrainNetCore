using DAL.Repositories.Interfaces;
using DAL.Models;

namespace DAL.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(IMongoContext context) : base(context)
        {

        }
    }
}
