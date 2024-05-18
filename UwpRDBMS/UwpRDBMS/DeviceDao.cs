using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpRDBMS
{
    public class DeviceDao
    {
        public static DeviceDao Instance = Singleton<DeviceDao>.Instance;

        private DeviceDao() { Debug.WriteLine("Device dao Constructor called"); }

        public void InsertDevice(string deviceId)
        {
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                REPLACE INTO Device (DeviceId)
                VALUES ($deviceId)
                ON CONFLICT(DeviceId) DO UPDATE SET DeviceId = $deviceId;
                ";

                command.Parameters.AddWithValue("$deviceId", deviceId);
                command.ExecuteNonQuery();
            }



        }

        public void InsertAllDeviceInfo(string deviceId, string deviceName , string deviceAction)
        {
            using (var connection = DbManager.Instance.GetConnection())
            {
                
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                REPLACE INTO Device (DeviceId, DeviceName, DeviceAction)
                VALUES ($deviceId, $deviceName, $deviceAction)
                ON CONFLICT(DeviceId) DO UPDATE SET DeviceName = $deviceName, DeviceAction = $deviceAction WHERE DeviceId = $deviceId;
                ";

                command.Parameters.AddWithValue("$deviceId", deviceId);
                command.Parameters.AddWithValue("$deviceName", deviceName);
                command.Parameters.AddWithValue("$deviceAction", deviceAction);

                command.ExecuteNonQuery();
            }



        }

        public void DeleteDevice(string deviceId)
        {
            using (var connection = DbManager.Instance.GetConnection())
            {

                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                DELETE FROM Device WHERE DeviceId = $deviceId;
                ";

                command.Parameters.AddWithValue("$deviceId", deviceId);

                command.ExecuteNonQuery();
            }



        }

        public void InsertDeviceData(string deviceId, string data, string deviceAttribute, string deviceValue)
        {
            //ON CONFLICT(DataId) DO UPDATE SET Data = $data;
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                REPLACE INTO DeviceData (DeviceId, Data, DeviceAttribute, DeviceValue) VALUES ((SELECT Id FROM Device WHERE DeviceId = $deviceId), $data, $deviceAttribute, $deviceValue);

            ";
                command.Parameters.AddWithValue("$deviceId", deviceId);
                command.Parameters.AddWithValue("$data", data);
                command.Parameters.AddWithValue("$deviceAttribute", deviceAttribute);
                command.Parameters.AddWithValue("$deviceValue", deviceValue);
                command.ExecuteNonQuery();
            }
        }

        /*public void InsertDeviceData( string deviceId, string data)
        {
            //ON CONFLICT(DataId) DO UPDATE SET Data = $data;
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                REPLACE INTO DeviceData (DeviceId, Data)
                SELECT Id, $data FROM Device WHERE DeviceId = $deviceId
                ON CONFLICT(DeviceId) DO UPDATE SET Data = $data;
            ";
                command.Parameters.AddWithValue("$deviceId", deviceId);
                command.Parameters.AddWithValue("$data", data);
                command.ExecuteNonQuery();
            }
        }*/

        public void InsertDeviceSettings(string deviceId, string setting)
        {
            //ON CONFLICT(SettingId) DO UPDATE SET Setting = $setting;
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                REPLACE INTO DeviceSettings (DeviceId, Setting)
                SELECT Id, $setting FROM Device WHERE DeviceId = $deviceId
                ON CONFLICT(DeviceId) DO UPDATE SET Setting = $setting;
            ";
                command.Parameters.AddWithValue("$deviceId", deviceId);
                command.Parameters.AddWithValue("$setting", setting);
                command.ExecuteNonQuery();
            }
        }

        //For Updating
        public void UpdateDeviceData(string deviceId, string data, string deviceAttribute, string deviceValue)
        {
            
            using (var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                UPDATE DeviceData SET Data = $data, DeviceAttribute = $deviceAttribute, DeviceValue = $deviceValue WHERE DeviceId =
                (SELECT Id FROM Device WHERE DeviceId = $deviceId);
                
            ";
                command.Parameters.AddWithValue("$deviceId", deviceId);
                command.Parameters.AddWithValue("$data", data);
                command.Parameters.AddWithValue("$deviceAttribute", deviceAttribute);
                command.Parameters.AddWithValue("$deviceValue", deviceValue);
                command.ExecuteNonQuery();
            }
        }

        //For Select Query

        public List<Device> GetDevicesWithData()
        {
            var devices = new Dictionary<int, Device>();

            using(var connection = DbManager.Instance.GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
            SELECT 
                d.Id, 
                d.DeviceId, 
                d.DeviceName,
                d.DeviceAction,
                dd.DataId, 
                dd.Data, 
                dd.DeviceAttribute,
                dd.DeviceValue,
                ds.SettingId, 
                ds.Setting,
                ds.SettingOrder,
                ds.CameraSetting,
                ds.ActionSetting
            FROM Device d
            LEFT JOIN DeviceData dd ON d.Id = dd.DeviceId
            LEFT JOIN DeviceSettings ds ON d.Id = ds.DeviceId;
        ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        Debug.WriteLine("Ganim: "+ Id);

                        if (!devices.TryGetValue(Id, out var device))
                        {
                            device = new Device
                            {
                                Id = Id,
                                DeviceId = reader.GetString(reader.GetOrdinal("DeviceId")),
                                DeviceAction = reader.IsDBNull(reader.GetOrdinal("DeviceAction")) ? null : reader.GetString(reader.GetOrdinal("DeviceAction")),
                                DeviceName = reader.IsDBNull(reader.GetOrdinal("DeviceName")) ? null : reader.GetString(reader.GetOrdinal("DeviceName"))
                            };
                            devices[Id] = device;
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("DataId")))
                        {
                            var deviceData = new DeviceData
                            {
                                DataId = reader.GetInt32(reader.GetOrdinal("DataId")),
                                Data = reader.IsDBNull(reader.GetOrdinal("Data")) ? null : reader.GetString(reader.GetOrdinal("Data")),
                                DeviceAttribute = reader.IsDBNull(reader.GetOrdinal("DeviceAttribute")) ? null : reader.GetString(reader.GetOrdinal("DeviceAttribute")),
                                DeviceValue = reader.IsDBNull(reader.GetOrdinal("DeviceValue")) ? null : reader.GetString(reader.GetOrdinal("DeviceValue"))
                            };
                            device.DeviceDataList.Add(deviceData);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("SettingId")))
                        {
                            var deviceSetting = new DeviceSettings
                            {
                                SettingId = reader.GetInt32(reader.GetOrdinal("SettingId")),
                                Setting = reader.IsDBNull(reader.GetOrdinal("Setting")) ? null : reader.GetString(reader.GetOrdinal("Setting")),
                                SettingOrder = reader.IsDBNull(reader.GetOrdinal("SettingOrder")) ? null : reader.GetString(reader.GetOrdinal("SettingOrder")),
                                CameraSetting = reader.IsDBNull(reader.GetOrdinal("CameraSetting")) ? null : reader.GetString(reader.GetOrdinal("CameraSetting")),
                                ActionSetting = reader.IsDBNull(reader.GetOrdinal("ActionSetting")) ? null : reader.GetString(reader.GetOrdinal("ActionSetting"))
                            };
                            device.DeviceSettingsList.Add(deviceSetting);
                        }
                    }
                }
            }


            return new List<Device>(devices.Values);
        }

    }
}
