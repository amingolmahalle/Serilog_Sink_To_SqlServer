using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace serlog_SqlServer.Sinks.MSSqlServer
{
    public class SqlBuffer
    {
        private StringBuilder _sb = new StringBuilder();

        private readonly SqlConnection _conn = new SqlConnection();

        private int _commandCount;

        public int CommandCount => _commandCount;

        public void AddQuery(string query)
        {
            _sb.Append(query);
            _commandCount++;
        }
        private void Clear()
        {
            _commandCount = 0;
            _sb = new StringBuilder
            {
                Length = 0
            };
            GC.Collect();

        }
        public async Task<int> WriteBufferToDb()
        {
            Task<int> k = ExecuteNonQueryAsync(_sb.ToString());
            Clear();
            return await k;
        }
        private async Task<int> ExecuteNonQueryAsync(string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr))
                return 0; //Task.FromResult(0);
            _conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                _conn.Open();
                var cm = new SqlCommand
                {
                    CommandType = System.Data.CommandType.Text,
                    CommandTimeout = 30000000,
                    CommandText = queryStr,
                    Connection = _conn
                };
                return await cm.ExecuteNonQueryAsync();
            }
            finally
            {
                _conn.Close();
            }
        }
        public string GetQuery()
        {
            return _sb.ToString();
        }
    }
}