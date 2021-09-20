using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.Context
{
    public interface IApplicationDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        public DbSet<D_Customer> D_Customers { get; set; }
        public DbSet<D_CustomerGroup> D_CustomersGroups { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
