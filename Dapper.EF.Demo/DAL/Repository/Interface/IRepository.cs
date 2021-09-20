using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Add(T obj);
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        void Update(T obj);
        void Remove(Guid id);

        T GetSingle(Expression<Func<T, bool>> criteria);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> criteria);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> criteria);
        List<T> GetList(Expression<Func<T, bool>> criteria);
        IQueryable<T> QueryAll();
        IQueryable<T> GetWhere(Expression<Func<T, bool>> criteria);
    }
}
