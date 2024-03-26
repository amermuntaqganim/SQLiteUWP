using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace UwpSqliteTestOne
{
    public class DeviceDao : IDeviceDao
    {
        IDbManager dbmanager = DbManager.Instance;

        public static IDeviceDao Instance = Singleton<DeviceDao>.Instance;

        private DeviceDao()
        { 
        }
        public async Task InsertData(Device device)
        {
            using (var connection = dbmanager.GetConnection())
            { 
                
                connection.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = connection;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = @"REPLACE INTO Device (Name, Description) VALUES (@Name, @Description)";

                insertCommand.Parameters.AddWithValue("@Name", device.Name);
                insertCommand.Parameters.AddWithValue("@Description", device.Description);
                insertCommand.ExecuteNonQuery();
                

            }
        }

        public async Task<List<Device>> GetData()
        {

            List<Device> list = new List<Device>();
            using (var connection = dbmanager.GetConnection())
            {

                connection.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = connection;
                selectCommand.CommandText = @"SELECT * From Device";
                    
                SqliteDataReader query = selectCommand.ExecuteReader();

                
                while (query.Read())
                {
                    Device dev = new Device();
                    dev.DeviceId =Convert.ToInt32(query.GetString(0));
                    dev.Name = query.GetString(1);
                    dev.Description = query.GetString(2);

                    list.Add(dev);
                }
                


            }

            return list;
        }

        public async Task InsertActions(List<DeviceAction> devaction)
        {

            foreach (var action in devaction)
            {
                using(var connection = dbmanager.GetConnection())
                {
                    connection.Open();
                    var insertActionCommand = connection.CreateCommand();
                    insertActionCommand.CommandText = "REPLACE INTO Action (DeviceId, Name) VALUES (@DeviceId, @Name);";
                    insertActionCommand.Parameters.AddWithValue("@DeviceId", action.DeviceId); // Assuming device IDs start from 1
                    insertActionCommand.Parameters.AddWithValue("@Name", action.Name);
                    insertActionCommand.ExecuteNonQuery();

                }

            }
        }

        public async Task InsertUrls(List<DeviceUrl> devurls)
        {

            foreach (var devurl in devurls)
            {
                using (var connection = dbmanager.GetConnection())
                {
                    connection.Open();
                    var insertUrlCommand = connection.CreateCommand();
                    insertUrlCommand.CommandText = "REPLACE INTO Url (ActionId, Link) VALUES (@ActionId, @Link);";
                    insertUrlCommand.Parameters.AddWithValue("@ActionId", devurl.ActionId); // Assuming action IDs start from 1
                    insertUrlCommand.Parameters.AddWithValue("@Link", devurl.Link);
                    insertUrlCommand.ExecuteNonQuery();

                }

            }
  
        }

        public async Task<List<string>> GetUrlLinks()
        {

            var urls = new List<string>();

            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();

                var queryCommand = connection.CreateCommand();
                queryCommand.CommandText = @"
            SELECT Url.Link
            FROM Action
            INNER JOIN Url ON Action.Id = Url.ActionId;";

                using (var reader = await queryCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var url = reader.GetString(0);
                        Debug.WriteLine("URL LINK: "+ url);
                        urls.Add(url);
                    }
                }
            }

            return urls;


        }
    }
}
