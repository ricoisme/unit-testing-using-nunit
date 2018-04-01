using CoreProfiler;
using CoreProfiler.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MyAPI.Infarstructure
{

    public interface IDapperContext : IDisposable
    {
        IDbConnection Connection { get; }
    }

    public class DapperContext : IDapperContext
    {
        private readonly string _connectionStringName;
        private readonly string _connectionString;
        private bool _useCoreProfiler;
        private IDbConnection _connection;

        public DapperContext(IConfiguration configuration, string connectionStringKey, string coreProfilerKey = "EnableCoreProfiler")
        {
            var configurationSection = configuration.GetSection("ConnectionStrings");
            if (configurationSection == null)
                throw new ArgumentNullException("configurationSection is null");
            var connectionString = configurationSection[connectionStringKey];
            if (connectionString == null)
                throw new ArgumentNullException("ConnectionStrings is null");
            var useCoreProfilerForSql = configurationSection[coreProfilerKey];
            if (string.IsNullOrEmpty(useCoreProfilerForSql))
                _useCoreProfiler = false;
            else
                _useCoreProfiler = bool.Parse(useCoreProfilerForSql);

            _connectionStringName = connectionStringKey;
            _connectionString = connectionString;
        }

        IDbConnection IDapperContext.Connection
        {
            get
            {
                if (_connection == null)
                {
                    if (_useCoreProfiler)
                    {
                        DbProfiler dbProfiler = null;
                        if (ProfilingSession.Current != null)
                            dbProfiler = new DbProfiler(ProfilingSession.Current.Profiler);

                        _connection = new ProfiledDbConnection(new SqlConnection(_connectionString), dbProfiler);
                    }
                    else
                        _connection = new SqlConnection(_connectionString);
                }
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();
                return _connection;
            }
        }

        void IDisposable.Dispose()
        {
            if (_connection?.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
