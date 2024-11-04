using System;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace SYOS.Server.Singleton
{
    public sealed class DatabaseConnection
    {
        private static readonly Lazy<DatabaseConnection> lazyInstance =
            new Lazy<DatabaseConnection>(() => new DatabaseConnection(), LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly string connectionString;

        private DatabaseConnection()
        {
            connectionString = "Data Source=SHAMMIKA-LAPTOP;Initial Catalog=SYOS_DB;Integrated Security=True;Trust Server Certificate=True";
        }

        public static DatabaseConnection Instance => lazyInstance.Value;

        public string ConnectionString => connectionString;

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}