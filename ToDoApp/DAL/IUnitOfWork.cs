using System;
using System.Threading.Tasks;
using DAL.Repository.Interface;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository studentRepo { get; }
        Task<int> CommitAsync();
        int Commit();
    }
}
