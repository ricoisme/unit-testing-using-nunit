using Microsoft.Extensions.Logging;
using MyAPI.Infarstructure;
using MyAPI.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyAPI.Repositorys
{
    public interface ICoreProfilerRepository
    {
        void Insert(CoreProfilerModulecs entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<int> InsertAsync(CoreProfilerModulecs entity, IDbTransaction transaction = null, int? commandTimeout = null);
        long BulkInsert(IList<CoreProfilerModulecs> entities, IDbTransaction transaction = null,
           int? commandTimeout = null);
        Task<long> BulkInsertAsync(IList<CoreProfilerModulecs> entities, IDbTransaction transaction = null, int? commandTimeout = null);
    }

    public class CoreProfilerRepository : Repository<CoreProfilerModulecs>, ICoreProfilerRepository
    {
        protected readonly IDbConnection _dbConnection;
        private readonly ILogger<CoreProfilerRepository> _logger;

        public CoreProfilerRepository(ILogger<CoreProfilerRepository> logger, IDapperContext dapperContext) : base(dapperContext)
        {
            _dbConnection = dapperContext.Connection;
            _logger = logger;
        }

        public void Insert(CoreProfilerModulecs entity, IDbTransaction transaction = null, int? commandTimeout = null)
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

        public Task<int> InsertAsync(CoreProfilerModulecs entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            try
            {
                return base.AddAsync(entity, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}:{ex.StackTrace}");
                return Task.FromResult(0);
            }
            finally
            {
                if (_dbConnection?.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        public long BulkInsert(IList<CoreProfilerModulecs> entities, IDbTransaction transaction = null,
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

        public async Task<long> BulkInsertAsync(IList<CoreProfilerModulecs> entities, IDbTransaction transaction = null,
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
    }
}
