using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DAL
{
    public interface IMongoContext : IDisposable
    {
        void AddCommand(Func<Task> func);
        Task<int> SaveChangesAsync();
        int SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}