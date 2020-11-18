using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        DataContext context;
        protected DbSet<T> DbSet;
        public Repository(DataContext _context)
        {
            this.context = _context;
            this.DbSet = context.Set<T>();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void Add(T obj)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var all = await DbSet.ToListAsync();
            return all.ToList();
        }

        public void Update(T obj)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public T GetSingle(Expression<Func<T, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetSingleAsync(Expression<Func<T, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetListAsync(Expression<Func<T, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public List<T> GetList(Expression<Func<T, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> QueryAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> criteria)
        {
            throw new NotImplementedException();
        }
    }
}
