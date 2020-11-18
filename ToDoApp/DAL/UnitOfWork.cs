using System;
using System.Threading.Tasks;
using DAL.Repository;
using DAL.Repository.Interface;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        DataContext context;
        IStudentRepository _student;
        public UnitOfWork(DataContext _context)
        {
            context = _context;
        }

        public IStudentRepository studentRepo
        {
            get
            {
                if (_student == null)
                {
                    _student = new StudentRepository(context);
                }
                return _student;
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
