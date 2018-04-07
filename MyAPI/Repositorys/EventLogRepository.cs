using Dapper;
using Microsoft.Extensions.Logging;
using MyAPI.Infarstructure;
using MyAPI.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyAPI.Repositorys
{
    public interface IEventLogRepository
    {
        void Insert(EventLogModule entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<int> InsertAsync(EventLogModule entity, IDbTransaction transaction = null, int? commandTimeout = null);
        long BulkInsert(IList<EventLogModule> entities, IDbTransaction transaction = null,
            int? commandTimeout = null);
        Task<long> BulkInsertAsync(IList<EventLogModule> entities, IDbTransaction transaction = null, int? commandTimeout = null);
        IEnumerable<EventLogModule> ExecSql<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null);
    }
    public class EventLogRepository : Repository<EventLogModule>, IEventLogRepository
    {
        protected readonly IDbConnection _dbConnection;
        private readonly ILogger<EventLogRepository> _logger;
        public EventLogRepository(ILogger<EventLogRepository> logger, IDapperContext dapperContext) : base(dapperContext)
        {
            _dbConnection = dapperContext.Connection;
            _logger = logger;
        }

        public void Insert(EventLogModule entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            try
            {
                base.Add(entity, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}:{ex.StackTrace}");
            }
            finally
            {
                if (_dbConnection?.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        public async Task<int> InsertAsync(EventLogModule entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            try
            {
                return await base.AddAsync(entity, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}:{ex.StackTrace}");
                return await Task.FromResult(0);
            }
            finally
            {
                if (_dbConnection?.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        public long BulkInsert(IList<EventLogModule> entities, IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            try
            {
                return base.BulkAdd(entities, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}:{ex.StackTrace}");
                return 0;
            }
            finally
            {
                if (_dbConnection?.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        public async Task<long> BulkInsertAsync(IList<EventLogModule> entities, IDbTransaction transaction = null,
            int? commandTimeout = null)
        {
            try
            {
                return await base.BulkAddAsync(entities, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}:{ex.StackTrace}");
                return await Task.FromResult((Int64)0);
            }
            finally
            {
                if (_dbConnection?.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        public IEnumerable<EventLogModule> ExecSql<Tsp>(string sql, CommandType commandType, DynamicParameters param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            try
            {
                return base.Exec<EventLogModule>(sql, commandType, param, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}:{ex.StackTrace}");
                return null;
            }
            finally
            {
                if (_dbConnection?.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }
    }
}
