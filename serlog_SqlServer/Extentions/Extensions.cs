using System;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using serlog_SqlServer.Entity.Enrichers;
using serlog_SqlServer.Filters;
using serlog_SqlServer.Sinks.MSSqlServer;
using Serilog;
using SerilogWeb.Classic;
using Serilog.Exceptions;

namespace serlog_SqlServer.Extentions
{
    public static class Extensions
    {
        public static LoggerConfiguration UseSqlServerConfiguration(this LoggerConfiguration loggerConfiguration)
        {
            var appDataPath = HostingEnvironment.MapPath(@"~\App_Data");

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Directory.CreateDirectory(appDataPath ?? throw new InvalidOperationException());
                var path = $@"{appDataPath}\SerilogInternals-{DateTime.Now:yyyy-MM-dd}.txt";
                File.AppendAllText(path, msg + Environment.NewLine);
            });
            SerilogWebClassic.Configure(config => config.Disable());//Install-Package SerilogWeb.Classic

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
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); //Apply Filters In Asp.Net MVC

            configuration.Services.Replace(typeof(IExceptionLogger), new Filters.ExceptionLogger()); //Apply ExceptionLogger Filter In Asp.Net WebApi

            if (includeActionParameters)
                configuration.Filters.Add(new IncludeActionParametersInRequestFilter()); //Apply IncludeActionParametersInRequest Filter In Asp.Net WebApi
        }
    }
}
