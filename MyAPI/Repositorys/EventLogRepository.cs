using Microsoft.Extensions.Logging;
using MyAPI.Infarstructure;
using MyAPI.Modules;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MyAPI.Repositorys
{
    public interface IEventLogRepository
    {
        void Insert(EventLogModule entity, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<int> InsertAsync(EventLogModule entity, IDbTransaction transaction = null, int? commandTimeout = null);
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

        public Task<int> InsertAsync(EventLogModule entity, IDbTransaction transaction = null, int? commandTimeout = null)
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
    }
}
