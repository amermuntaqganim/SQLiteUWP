using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UwpSqliteTestOne
{
    public class DbManager:IDbManager
    {
        public static IDbManager Instance = Singleton<DbManager>.Instance;
    

        private SqliteConnection connection;
        private string dbpath;

        public void Close()
        {
            throw new NotImplementedException();
        }

        public SqliteConnection GetConnection()
        {
            if(connection!=null)
                return connection;
            else
                return connection = new SqliteConnection($"Filename={dbpath}");
            
        }

        private DbManager()
        {
            Debug.WriteLine("Constructor called");
        }

        public async Task InitializeDatabase()
        {
            Debug.WriteLine("Initialize Database");

            await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSampleOne.db", CreationCollisionOption.OpenIfExists);
            dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSampleOne.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                connection = db;

                db.Open();

                Debug.WriteLine("Connection Open");
                SqliteCommand tableCommand = new SqliteCommand();
                tableCommand.Connection = connection;

                // Use parameterized query to prevent SQL injection attacks
                tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Device (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT,
                                    Description TEXT
                                );";
 
                tableCommand.ExecuteReader();

               /* using (var cmd = connection.CreateCommand())
                {
                    Debug.WriteLine("Create Command");
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Device (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT,
                                    Description TEXT
                                );";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }*/
            }

           /* await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            Debug.WriteLine(dbpath);
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Text_Entry NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }*/
        }

        public void Open()
        {
            throw new NotImplementedException();
        }
    }
}
