using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public IDbConnection Connection => Database.GetDbConnection();
        public DbSet<D_Customer> D_Customers { get; set; }
        public DbSet<D_CustomerGroup> D_CustomersGroups { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
