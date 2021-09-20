using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.Dapper
{
    public class DapperRepository : IDapperRepository, IDisposable
    {
        private readonly ApplicationDbContext context;
        public readonly IConfiguration _config;

        public DapperRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            await context.Connection.ExecuteAsync(sp, parms, commandType: commandType);
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return context.Connection.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return await context.Connection.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType);
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            var item = context.Connection.Query<T>(sp, parms, commandType: commandType).ToList();
            return item;
        }

        public IEnumerable<T> GetAllItem<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            var item = context.Connection.Query<T>(sp, parms, commandType: commandType);
            return item;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return await context.Connection.QueryAsync<T>(sp, parms, commandType: commandType);
        }

        public async Task<int> Execute_int(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return await context.Connection.ExecuteAsync(sp, parms, commandType: commandType);
        }

        public void Dispose()
        {
            context.Connection.Dispose();
        }
    }
}
