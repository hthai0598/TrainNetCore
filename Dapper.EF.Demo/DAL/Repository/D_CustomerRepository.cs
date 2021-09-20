using System;
using DAL.Context;
using DAL.Model;
using DAL.Repository.Interface;

namespace DAL.Repository
{
    public class D_CustomerRepository : Repository<D_Customer>, ID_CustomerRepository
    {
        public D_CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
