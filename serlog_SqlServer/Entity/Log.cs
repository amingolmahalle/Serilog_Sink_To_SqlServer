using System;
using Serilog.Events;

namespace serlog_SqlServer.Entity
{
    public class Log
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime CreationDate { get; set; }

        public LogEventLevel Level { get; set; }

        public string Ip { get; set; }

        public string ApplicationName { get; set; }

        public string Exception { get; set; }

         public string MyProperties { get; set; }

    }
}