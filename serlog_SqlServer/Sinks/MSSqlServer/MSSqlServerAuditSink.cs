using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using serlog_SqlServer.Entity;
using serlog_SqlServer.Exception;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using SerilogWeb.Classic.Enrichers;

namespace serlog_SqlServer.Sinks.MSSqlServer
{
    public class MsSqlServerAuditSink : PeriodicBatchingSink
    {
        #region Field

        readonly SqlBuffer _sqlbuffer=new SqlBuffer();
       
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

            AddiotionalColumnNames = new[]{UserNameEnricher.UserNamePropertyName,HttpRequestClientHostIPEnricher.HttpRequestClientHostIPPropertyName};
        }
        
        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
           
            try
            {
                foreach (var logEvent in events)
                {

                    var properties = logEvent.Properties.Where(x => !AddiotionalColumnNames.Contains(x.Key)).ToList();

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
                        MyProperties = properties.ToDictionary(x => x.Key, x => JsonConvert.SerializeObject(x.Value))
                       // ,MyProperty = properties.ToDictionary(x => x.Key, x => SerializeObject(x.Key,x.Value.ToString()))
                    };
                    //$ is --> String Interpolation
                    _sqlbuffer.AddQuery( $@"
                                         INSERT INTO Logs_Tbl (id, applicationName, creationdate, exception,[ip], [level], [message], username,Properties)
                                         VALUES ('{log.Id}','{log.ApplicationName}','{log.CreationDate}','{log.Exception}','{log.Ip}',{log.Level.GetHashCode()},
                                         '{log.Message}', '{log.UserName}','{log.MyProperties}')");
                }
                await _sqlbuffer.WriteBufferToDb();
            }
            catch (System.Exception ex)
            {
                SelfLog.WriteLine($"Unable to write log event to the database due to following error: {ex.Message}");
                throw;
            }
        }
        public static string SerializeObject( object o)
        {
            MemoryStream ms = new MemoryStream();
           // DataContractSerializer xs = new DataContractSerializer(o.GetType());
            XmlSerializer xs = new XmlSerializer(o.GetType());
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
            // xs.WriteObject(xtw, o);
            xs.Serialize(xtw, o);
            ms = (MemoryStream)xtw.BaseStream;
            return Utf8ByteArrayToString(ms.ToArray());
        }     
        public static String Utf8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        public static string Serialize(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        // https://stackoverflow.com/questions/2658916/serializing-a-list-of-key-value-pairs-to-xml
        [Serializable]
        [XmlType(TypeName = "WhateverNameYouLike")]
        public struct KeyValuePair<TK, TV>
        {
            public TK Key
            { get; set; }

            public TV Value
            { get; set; }
        }

    }
}
