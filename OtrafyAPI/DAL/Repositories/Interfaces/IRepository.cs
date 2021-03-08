using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        void Remove(Guid id);

        TEntity GetSingle(Expression<Func<TEntity, bool>> criteria);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> criteria);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> criteria);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> criteria);
        IQueryable<TEntity> QueryAll();
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> criteria);

        long Count(FilterDefinition<TEntity> filter);
        Task<long> CountAsync(FilterDefinition<TEntity> filter);
    }
}
