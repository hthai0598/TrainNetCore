using System;
using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext()
        {

        }

        public DbSet<Student> students { get; set; }
    }
}
