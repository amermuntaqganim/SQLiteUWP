using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteTest
{
    public class DeviceDao : IDeviceDao
    {
        // Static instance variable
        private static readonly Lazy<DeviceDao> instance = new Lazy<DeviceDao>(() => new DeviceDao());

        // Public property to access the singleton instance
        public static DeviceDao Instance => instance.Value;
        public void InsertDevice(Device device)
        {
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();

                string sql = "INSERT INTO Device (Name, Description) VALUES (@Name, @Description)";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", device.Name);
                    command.Parameters.AddWithValue("@Description", device.Description);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
