using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using Newtonsoft.Json;
using serlog_SqlServer.Entity;
using serlog_SqlServer.Exceptions;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using SerilogWeb.Classic.Enrichers;

namespace serlog_SqlServer.Sinks.MSSqlServer
{
    public class MsSqlServerAuditSink : PeriodicBatchingSink
    {
        #region Field

        readonly SqlBuffer _sqlbuffer = new SqlBuffer();

        private const int DefaultBatchPostingLimit = 50;

        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(5);

        private string ApplicationName { get; }
        private string[] AddiotionalColumnNames { get; }
        #endregion
        public MsSqlServerAuditSink() : base(DefaultBatchPostingLimit, DefaultPeriod)
        {
            ApplicationName = WebConfigurationManager.AppSettings["Log:ApplicationName"];

            if (string.IsNullOrWhiteSpace(ApplicationName))
                throw new ApplicationNameNotProvidedException();

            AddiotionalColumnNames = new[] { UserNameEnricher.UserNamePropertyName, HttpRequestClientHostIPEnricher.HttpRequestClientHostIPPropertyName };
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {

            try
            {
                foreach (var logEvent in events)
                {

                    var properties = logEvent.Properties.Where(x => !AddiotionalColumnNames.Contains(x.Key)).ToList();
                    string jsonPro = JsonConvert.SerializeObject(properties);
                    var log = new Log
                    {
                        Id = Guid.NewGuid(),
                        UserName = logEvent.Properties.TryGetValue(UserNameEnricher.UserNamePropertyName, out var usernameOut) ? usernameOut.ToString("l", null) : "",
                        Message = logEvent.RenderMessage(),
                        Level = logEvent.Level,
                        CreationDate = DateTime.Now,
                        ApplicationName = ApplicationName,
                        Ip = logEvent.Properties.TryGetValue(HttpRequestClientHostIPEnricher.HttpRequestClientHostIPPropertyName, out var ipOut) ? ipOut.ToString("l", null) : "",
                        Exception = logEvent.Exception?.ToString(),
                        MyProperties = jsonPro
                    };

                    //$ is --> String Interpolation
                    _sqlbuffer.AddQuery($@"
                                         INSERT INTO Logs_Tbl (id, applicationName, creationdate, exception,[ip], [level], [message], username,Properties)
                                         VALUES ('{log.Id}','{log.ApplicationName}','{log.CreationDate}','{log.Exception}','{log.Ip}',{log.Level.GetHashCode()},
                                         '{log.Message}', '{log.UserName}','{log.MyProperties}')");
                }
                await _sqlbuffer.WriteBufferToDb();
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine($"Unable to write log event to the database due to following error: {ex.Message}");
                throw;
            }
        }

    }
}
