using serlog_SqlServer.Extentions;
using Serilog;
namespace serlog_SqlServer
{
    public class LoggerConfig
    {
        public static void Config()
        {
            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Information()
                .UseSqlServerConfiguration()
                .CreateLogger();


        }
    }
}