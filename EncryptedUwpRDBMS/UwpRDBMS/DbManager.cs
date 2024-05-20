using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Input.Inking;
using Windows.UI.ViewManagement;

namespace UwpRDBMS
{
    public class DbManager
    {

        public static DbManager Instance = Singleton<DbManager>.Instance;
        string dbpath;
        SqliteConnection connection;
        string password = "my_password";

        private DbManager()
        {
            
            Debug.WriteLine("Constructor called");
            SQLitePCL.Batteries_V2.Init();
            dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "SQLiteRDBMS.db");
            password = "my_password";

            // Create and encrypt the database
            using (var connection = GetEncryptedConnection())
            {

                var cacheSizeCmd = connection.CreateCommand();
                cacheSizeCmd.CommandText = "PRAGMA cache_size = 10000;";
                cacheSizeCmd.ExecuteNonQuery();

                var pageSizeCmd = connection.CreateCommand();
                pageSizeCmd.CommandText = "PRAGMA page_size = 4096;";
                pageSizeCmd.ExecuteNonQuery();
            }
        }

        public SqliteConnection GetConnection()
        {

            return connection = new SqliteConnection($"Filename={dbpath}");

        }

        public SqliteConnection GetEncryptedConnection()
        {
            dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "SQLiteRDBMS.db");
            password = "my_password";

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = dbpath,
                Password = password,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Default
            }.ToString();

            var connection = new SqliteConnection(connectionString);
            connection.Open();
            Debug.WriteLine("Passowrd: " + password);

           
            /*var command = connection.CreateCommand();
            command.CommandText = $"PRAGMA key = '{password}';";
            command.ExecuteNonQuery();*/

            return connection;
        }

        public void CreateTables()
        {

            //var connection = new SqliteConnection($"Filename={dbpath}");
            var connection = GetEncryptedConnection();
            //connection.Open();
            

            var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Device (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DeviceId TEXT UNIQUE NOT NULL,
                    DeviceName TEXT,
                    DeviceAction TEXT
                );

                CREATE TABLE IF NOT EXISTS DeviceData (
                    DataId INTEGER PRIMARY KEY AUTOINCREMENT,
                    DeviceId TEXT UNIQUE NOT NULL,
                    Data TEXT,
                    DeviceAttribute TEXT,
                    DeviceValue TEXT,
                    FOREIGN KEY (DeviceId) REFERENCES Device(Id)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE
                );

                CREATE TABLE IF NOT EXISTS DeviceSettings (
                    SettingId INTEGER PRIMARY KEY AUTOINCREMENT,
                    DeviceId TEXT UNIQUE NOT NULL,
                    Setting TEXT,
                    SettingOrder TEXT,
                    CameraSetting TEXT,
                    ActionSetting TEXT,
                    FOREIGN KEY (DeviceId) REFERENCES Device(Id)
                    ON DELETE CASCADE
                    ON UPDATE CASCADE
                );
            ";
            command.ExecuteNonQuery();
        }

    }
}
