using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteTest
{
    public class DbManager : IDbManager
    {

        private readonly SqliteConnection connection;

        private DbManager()
        {
            string databasePath = "MySqliteTest.db"; // Path to SQLite database file
            string connectionString = $"Data Source={databasePath}";
            connection = new SqliteConnection(connectionString);

            connection.Open();

            // Create Device table if it doesn't exist
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Device (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT,
                                    Description TEXT
                                );";
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        
        private static readonly Lazy<DbManager> instance = new Lazy<DbManager>(() => new DbManager());


        public static DbManager Instance => instance.Value;

        public SqliteConnection GetConnection()
        {
            return connection;
        }

        public void CreateConnection()
        {
            throw new NotImplementedException();
        }

        public void InitializeDB()
        {
            throw new NotImplementedException();
        }
    }
}
