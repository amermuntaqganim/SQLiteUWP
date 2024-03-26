using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void InsertData(Device device)
        {
            using (var connection = dbmanager.GetConnection())
            { 
                
                connection.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = connection;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = @"INSERT INTO Device (Name, Description) VALUES (@Name, @Description)";

                insertCommand.Parameters.AddWithValue("@Name", device.Name);
                insertCommand.Parameters.AddWithValue("@Description", device.Description);
                insertCommand.ExecuteNonQuery();
                

            }
        }

        public List<Device> GetData()
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
                    dev.Id =Convert.ToInt32(query.GetString(0));
                    dev.Name = query.GetString(1);
                    dev.Description = query.GetString(2);

                    list.Add(dev);
                }
                


            }

            return list;
        }
    }
}
