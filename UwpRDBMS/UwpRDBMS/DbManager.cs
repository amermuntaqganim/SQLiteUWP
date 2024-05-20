﻿using Microsoft.Data.Sqlite;
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
        string password;
        SqliteConnection connection;

        private DbManager()
        {
            Debug.WriteLine("Constructor called");
            dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "SQLiteRDBMS.db");
        }

        public SqliteConnection GetConnection() 
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

            //return connection = new SqliteConnection($"Filename={dbpath}");
            return connection = new SqliteConnection(connectionString);

        }
        public void CreateTables()
        {

            //var connection = new SqliteConnection($"Filename={dbpath}");
            var connection = GetConnection();
            connection.Open();
            

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

                CREATE TABLE IF NOT EXISTS DeviceStates (
                StateId INTEGER PRIMARY KEY AUTOINCREMENT,
                DataId INTEGER NOT NULL,
                State TEXT,
                Timestamp DATETIME,
                FOREIGN KEY (DataId) REFERENCES DeviceData(DataId)
                ON DELETE CASCADE
                ON UPDATE CASCADE
            );
            ";
            command.ExecuteNonQuery();
        }

    }
}
