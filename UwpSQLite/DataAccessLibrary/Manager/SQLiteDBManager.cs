using Microsoft.Data.Sqlite;
using System;
using System.IO;
using Windows.Storage;


namespace DataAccessLibrary.Manager
{


    public class SQLiteDBManager
    {
        private static readonly Lazy<SQLiteDBManager> lazyInstance =
            new Lazy<SQLiteDBManager>(() => new SQLiteDBManager());

        string connectionString = "Filename=MySQLiteTest.db";
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "MySQLiteTest.db");
        private SqliteConnection connection;

        private SQLiteDBManager()
        {
            InitializeDatabase();
        }

        public static SQLiteDBManager Instance => lazyInstance.Value;

        private void InitializeDatabase()
        {
            LogManager.WriteLogs("Initialize Database and Open Connection");
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        public SqliteConnection GetSqliteConnection()
        {
            
            if (connection != null)
            {

                LogManager.WriteLogs("GetSqliteConnection: return exisiting connection");
                return connection;
            }
            else
            {
                LogManager.WriteLogs("GetSqliteConnection: Need to create new connection and open connection");
                connection = new SqliteConnection(connectionString);
                connection.Open();
                return connection;
            }

        }

        public void CreateTable<T>() where T : class
        {

            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = GetCreateTableSql<T>();
                command.ExecuteNonQuery();

                LogManager.WriteLogs("Table Creation Successful");
            }
        }

        public void DropTable<T>() where T : class
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DROP TABLE IF EXISTS {typeof(T).Name}";
                command.ExecuteNonQuery();

                LogManager.WriteLogs("Drop Table Successful");
            }
        }

        public void CloseConnection()
        {
            LogManager.WriteLogs("Close Connection");
            connection?.Close();
        }


        private string GetCreateTableSql<T>() where T : class
        {
            // Define your table creation SQL based on the properties of the entity type T
            if (typeof(T).Name.Equals("Users")) {
                LogManager.WriteLogs("GetCreateTableSQL: "+ typeof(T).Name+ " Table ");

                return $"CREATE TABLE IF NOT EXISTS {typeof(T).Name} (ID INTEGER PRIMARY KEY, Name TEXT, Age INTEGER)";
            }
               
            
            return $"CREATE TABLE IF NOT EXISTS SampleTable (Id INTEGER PRIMARY KEY AUTOINCREMENT)";
        }
    }
}
