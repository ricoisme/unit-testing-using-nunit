using Microsoft.Extensions.Configuration;
using NLog.Config;
using NLog.Targets;
using System;
using System.Linq;

namespace MyAPI.Infarstructure
{
    public interface INlogger
    {
        void Debug(string message, Exception exception = null);
        void Error(string message, Exception exception = null);
        void Info(string message, Exception exception = null);
        void Trace(string message, Exception exception = null);
        void Warn(string message, Exception exception = null);
        void Fatal(string message, Exception exception = null);
    }

    public class NloggerWrapper : INlogger
    {
        private readonly NLog.ILogger _logger;
        private readonly IConfiguration _configuration;
        public NloggerWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
            var name = _configuration["Logging:NlogFile:Name"];
            var fileName = _configuration["Logging:NlogFile:FileName"];
            var layout = _configuration["Logging:NlogFile:layout"];
            var logLevel = _configuration["Logging:NlogFile:LogLevel:Default"];
            var maxArchiveFiles = int.Parse(_configuration["Logging:NlogFile:MaxArchiveFiles"]);
            var archiveFileName = _configuration["Logging:NlogFile:ArchiveFileName"];
            var archiveNumbering = _configuration["Logging:NlogFile:ArchiveNumbering"];
            var archiveEvery = _configuration["Logging:NlogFile:ArchiveEvery"];
            var archiveDateFormat = _configuration["Logging:NlogFile:ArchiveDateFormat"];

            var nlogConfig = new NLog.Config.LoggingConfiguration();
            var logfileTarget = new NLog.Targets.FileTarget()
            {
                FileName = fileName,
                Name = name,
                Layout = layout,
                MaxArchiveFiles = maxArchiveFiles,
                ArchiveFileName = archiveFileName,
                ArchiveDateFormat = archiveDateFormat,
                ArchiveEvery = (NLog.Targets.FileArchivePeriod)Enum.Parse(typeof(NLog.Targets.FileArchivePeriod), archiveEvery),
                ArchiveNumbering = (NLog.Targets.ArchiveNumberingMode)
                Enum.Parse(typeof(NLog.Targets.ArchiveNumberingMode), archiveNumbering),
                EnableFileDelete = true
            };
            var miniLogLevel = NLog.LogLevel.AllLevels.Where(s => s.Name.ToLower().Equals(logLevel.ToLower())).FirstOrDefault();
            var fileRule = new LoggingRule("*", miniLogLevel, logfileTarget);
            nlogConfig.LoggingRules.Add(fileRule);

            var dbname = _configuration["Logging:NlogDB:Name"];
            var dbconnectionstring = _configuration["Logging:NlogDB:connectionStringName"];
            var dbcommandtype = _configuration["Logging:NlogDB:commandType"];
            var dbcommandtext = _configuration["Logging:NlogDB:commandText"];

            var dbTarget = new DatabaseTarget
            {
                Name = dbname,
                ConnectionString = dbconnectionstring,
                CommandType = (System.Data.CommandType)Enum.Parse(typeof(System.Data.CommandType), dbcommandtype),
                CommandText = dbcommandtext
            };
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@machineName", "${machinename}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@siteName", "${iis-site-name}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logged", "${date}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", "${level}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@username", "${aspnet-user-identity}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", "${message}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", "${logger}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@properties", "${all-event-properties:separator=|}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@serverName", "${aspnet-request-host}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@port", "${aspnet-request:serverVariable=SERVER_PORT}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@url", "${aspnet-request-url:IncludeHost=true:IncludePort=true:IncludeQueryString=true}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@https", "${when:inner=1:when='${aspnet-request:serverVariable=HTTPS}' == 'on'}${when:inner=0:when='${aspnet-request:serverVariable=HTTPS}' != 'on'}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@serverAddress", "${aspnet-request-host}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@remoteAddress", "${aspnet-request-ip}}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@callSite", "${callsite}"));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", "${exception:tostring}"));

            var dbRule = new LoggingRule("*", miniLogLevel, dbTarget);
            nlogConfig.LoggingRules.Add(dbRule);

            NLog.LogManager.Configuration = nlogConfig;

            _logger = NLog.LogManager.GetLogger(dbname);
        }

        void INlogger.Debug(string message, Exception exception)
        {
            _logger.Debug(exception, message);
        }

        void INlogger.Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        void INlogger.Fatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }

        void INlogger.Info(string message, Exception exception)
        {
            _logger.Info(exception, message);
        }

        void INlogger.Trace(string message, Exception exception)
        {
            _logger.Trace(exception, message);
        }

        void INlogger.Warn(string message, Exception exception)
        {
            _logger.Warn(exception, message);
        }
    }
}
