using MongoDB.Driver;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace DAL.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        //protected readonly IMongoContext _context;
        //protected readonly IMongoCollection<TEntity> _collection;

        protected readonly IMongoContext _context;
        protected IMongoCollection<TEntity> DbSet;

        protected Repository(IMongoContext context)
        {
            _context = context;
        }

        private void ConfigDbSet()
        {
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        public virtual void Add(TEntity obj)
        {
            ConfigDbSet();
            _context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            ConfigDbSet();
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            ConfigDbSet();
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            ConfigDbSet();
            _context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        }

        public virtual void Remove(Guid id)
        {
            ConfigDbSet();
            _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> criteria)
        {
            ConfigDbSet();
            return DbSet.Find(criteria).FirstOrDefault();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> criteria)
        {
            ConfigDbSet();
            var result = await DbSet.FindSync(criteria).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> criteria)
        {
            ConfigDbSet();
            var result = await DbSet.FindSync(criteria).ToListAsync();
            return result;
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> criteria)
        {
            ConfigDbSet();
            var result = DbSet.Find(criteria).ToList();
            return result;
        }

        public IQueryable<TEntity> QueryAll()
        {
            ConfigDbSet();
            return DbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> criteria)
        {
            ConfigDbSet();
            return DbSet.AsQueryable().Where(criteria);
        }

        public long Count(FilterDefinition<TEntity> filter)
        {
            ConfigDbSet();
            var result = DbSet.CountDocuments(filter);
            return result;
        }

        public async Task<long> CountAsync(FilterDefinition<TEntity> filter)
        {
            ConfigDbSet();
            var result = await DbSet.CountDocumentsAsync(filter);
            return result;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
