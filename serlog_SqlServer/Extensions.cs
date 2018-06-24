using System;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using serlog_SqlServer.Sinks.MSSqlServer;
using Serilog;
using Serilog.Exceptions;
using SerilogWeb.Classic;

namespace serlog_SqlServer
{
    public static class Extensions
    {
        public static LoggerConfiguration UseSqlServerConfiguration(this LoggerConfiguration loggerConfiguration)
        {
            var appDataPath = HostingEnvironment.MapPath(@"~\App_Data");
            Directory.CreateDirectory(appDataPath ?? throw new InvalidOperationException());

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                var path = $@"{appDataPath}\SerilogInternals-{DateTime.Now:yyyy-MM-dd}.txt";
                File.AppendAllText(path, msg + Environment.NewLine);
            });
            SerilogWebClassic.Configure(config => config.Disable());////Install-Package SerilogWeb.Classic

            loggerConfiguration
          .Enrich.WithExceptionDetails() //Install-Package Serilog.Exceptions 
          .Enrich.WithHttpRequestClientHostIP()
          .Enrich.WithUserName()
          .Enrich.WithHttpRequestUrl()
          .Enrich.With(new ActionParametersEnricher());

            loggerConfiguration.WriteTo.Sink<MsSqlServerAuditSink>();

            return loggerConfiguration;
        }

        public static void UseExceptionLoggerImpl(this HttpConfiguration configuration, bool includeActionParameters = true)
        {
            configuration.Services.Replace(typeof(IExceptionLogger), new ExceptionLogger());

            if (includeActionParameters)
                configuration.Filters.Add(new IncludeActionParametersInRequestFilter());
        }
    }
}
