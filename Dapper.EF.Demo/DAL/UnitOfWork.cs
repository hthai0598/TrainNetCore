using System;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Repository;
using DAL.Repository.Interface;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        ApplicationDbContext context;
        ID_CustomerRepository D_CustomerRepo;
        public static object obj { get; set; }
        public UnitOfWork(ApplicationDbContext _context)
        {
            context = _context;
        }

        public ID_CustomerRepository studentRepo
        {
            get
            {
                if (D_CustomerRepo == null)
                {
                    D_CustomerRepo = new D_CustomerRepository(context);
                }
                return D_CustomerRepo;
            }
        }


        public int Commit()
        {
            return context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
