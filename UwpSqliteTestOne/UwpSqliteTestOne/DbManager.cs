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
/*            if(connection!=null)
                return connection;
            else*/
                return connection = new SqliteConnection($"Filename={dbpath}");
            
        }

        private DbManager()
        {
            Debug.WriteLine("Constructor called");
            dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSampleOne.db");
        }

        public async Task InitializeDatabase()
        {
            Debug.WriteLine("Initialize Database");

            await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSampleOne.db", CreationCollisionOption.OpenIfExists);
            dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSampleOne.db");
            Debug.WriteLine(dbpath);
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
                                    Description TEXT,
                                    Machine TEXT,
                                    AGE  TEXT,
                                    DATE TEXT
                                );";

 
                tableCommand.ExecuteReader();


                var createActionTableCommand = connection.CreateCommand();

                createActionTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Action (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        DeviceId INTEGER,
                        Name TEXT,
                        FOREIGN KEY(DeviceId) REFERENCES Device(Id)
                    );";


                createActionTableCommand.ExecuteReader();


                var createUrlTableCommand = connection.CreateCommand();
                createUrlTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Url (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ActionId INTEGER,
                        Link TEXT,
                        AGE TEXT,
                        ULTRA TEXT,
                        FOREIGN KEY(ActionId) REFERENCES Action(Id)
                    );";


                createUrlTableCommand.ExecuteReader();


                var createImageTableCommand = connection.CreateCommand();
                createImageTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Images (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Url TEXT,
                        ImageData BLOB
                    );";


                createImageTableCommand.ExecuteReader();

            }

        }

        public void Open()
        {
            throw new NotImplementedException();
        }
    }
}
