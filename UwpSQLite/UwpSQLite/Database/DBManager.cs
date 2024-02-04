using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSQLite.Database
{
    using Microsoft.Data.Sqlite;
    using System;
    using System.IO;
    using Windows.Storage;

    public class DbManager
    {
        private static readonly Lazy<DbManager> lazyInstance =
            new Lazy<DbManager>(() => new DbManager());

        string connectionString = "Filename=MySample.db";
        string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
        private SqliteConnection connection;

        private DbManager()
        {
            InitializeDatabase();
        }

        public static DbManager Instance => lazyInstance.Value;

        private async void InitializeDatabase()
        {
            // Set up your database connection here
            //await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);

            //Console.WriteLine(dbpath);
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        public SqliteConnection GetSqliteConnection() 
        {
            if (connection != null)
                return connection;
            else
            {
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
            }
        }

        public void DropTable<T>() where T : class
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DROP TABLE IF EXISTS {typeof(T).Name}";
                command.ExecuteNonQuery();
            }
        }

        public void CloseConnection()
        {
            connection?.Close();
        }

        /*public IRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(connection);
        }*/

        private string GetCreateTableSql<T>() where T : class
        {
            // Define your table creation SQL based on the properties of the entity type T
            // Example: For simplicity, assuming each entity has an 'Id' property of type INTEGER
            return $"CREATE TABLE IF NOT EXISTS {typeof(T).Name} (ID INTEGER PRIMARY KEY, Name TEXT, Age INTEGER)";
            //return $"CREATE TABLE IF NOT EXISTS {typeof(T).Name} (Id INTEGER PRIMARY KEY AUTOINCREMENT)";
        }
    }

}
