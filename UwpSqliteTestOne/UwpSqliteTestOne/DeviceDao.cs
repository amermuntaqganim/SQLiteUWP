using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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

                await connection.OpenAsync();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = connection;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = @"INSERT INTO Device (Name, Description) VALUES (@Name, @Description)";

                insertCommand.Parameters.AddWithValue("@Name", device.Name);
                insertCommand.Parameters.AddWithValue("@Description", device.Description);
                await insertCommand.ExecuteNonQueryAsync();
                

            }
        }

        public async Task<List<Device>> GetData()
        {

            List<Device> list = new List<Device>();
            using (var connection = dbmanager.GetConnection())
            {

                await connection.OpenAsync();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = connection;
                selectCommand.CommandText = @"SELECT * From Device";
                    
                SqliteDataReader query = selectCommand.ExecuteReader();

                
                while (await query.ReadAsync())
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

        /*public async Task InsertActions(List<DeviceAction> devaction)
        {

            foreach (var action in devaction)
            {
                using(var connection = dbmanager.GetConnection())
                {
                    await connection.OpenAsync();
                    var insertActionCommand = connection.CreateCommand();
                    insertActionCommand.CommandText = @"INSERT INTO Action (DeviceId, Name) VALUES (@DeviceId, @Name);";
                    insertActionCommand.Parameters.AddWithValue("@DeviceId", action.DeviceId); // Assuming device IDs start from 1
                    insertActionCommand.Parameters.AddWithValue("@Name", action.Name);
                    await insertActionCommand.ExecuteNonQueryAsync();

                }

            }
        }*/

        public void InsertActions(List<DeviceAction> devaction)
        {
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {


                        // Create command for inserting into Table1
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = @"INSERT INTO Action (DeviceId, Name) VALUES (@DeviceId, @Name);";

                            // Add parameters
                            var param1 = command.CreateParameter();
                            param1.ParameterName = "@DeviceId";
                            var param2 = command.CreateParameter();
                            param2.ParameterName = "@Name";
                            command.Parameters.Add(param1);
                            command.Parameters.Add(param2);

                            // Execute the insert command for each data object
                            foreach (var data in devaction)
                            {
                                param1.Value = data.DeviceId;
                                param2.Value = data.Name;
                                command.ExecuteNonQuery();
                            }
                        }

                        // Repeat the above process for other tables if necessary

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

           
        }

        /*public async Task InsertUrls(List<DeviceUrl> devurls)
        {

            foreach (var devurl in devurls)
            {
                using (var connection = dbmanager.GetConnection())
                {
                    await connection.OpenAsync();
                    var insertUrlCommand = connection.CreateCommand();
                    insertUrlCommand.CommandText = @"INSERT INTO Url (ActionId, Link) VALUES (@ActionId, @Link);";
                    insertUrlCommand.Parameters.AddWithValue("@ActionId", devurl.ActionId); // Assuming action IDs start from 1
                    insertUrlCommand.Parameters.AddWithValue("@Link", devurl.Link);
                    await insertUrlCommand.ExecuteNonQueryAsync();

                }

            }
  
        }*/

        public void InsertUrls(List<DeviceUrl> devurls)
        {
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Create command for inserting into Table1
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = @"INSERT INTO Url (ActionId, Link) VALUES (@ActionId, @Link);";

                            // Add parameters
                            var param1 = command.CreateParameter();
                            param1.ParameterName = "@ActionId";
                            var param2 = command.CreateParameter();
                            param2.ParameterName = "@Link";
                            command.Parameters.Add(param1);
                            command.Parameters.Add(param2);

                            // Execute the insert command for each data object
                            foreach (var data in devurls)
                            {
                                param1.Value = data.ActionId;
                                param2.Value = data.Link;
                                command.ExecuteNonQuery();
                            }
                        }

                        // Repeat the above process for other tables if necessary

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception
                        transaction.Rollback();
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        public async Task<List<string>> GetUrlLinks()
        {

            var urls = new List<string>();

            using (var connection = DbManager.Instance.GetConnection())
            {
                await connection.OpenAsync();

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

        public async Task<List<string>> GetUrlsForAction(int actionId)
        {
            var urls = new List<string>();

            using (var connection = DbManager.Instance.GetConnection())
            {
                await connection.OpenAsync();

                var queryCommand = connection.CreateCommand();
                queryCommand.CommandText = @"
            SELECT Url.Link
            FROM Url
            WHERE Url.ActionId = @ActionId;";
                queryCommand.Parameters.AddWithValue("@ActionId", actionId);

                using (var reader = await queryCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var url = reader.GetString(0);
                        urls.Add(url);
                    }
                }
            }

            return urls;
        }

    }
}
