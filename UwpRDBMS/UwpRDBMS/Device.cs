using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpRDBMS
{
    public class Device
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceAction { get; set; }
        public List<DeviceData> DeviceDataList { get; set; } = new List<DeviceData>();
        public List<DeviceSettings> DeviceSettingsList { get; set; } = new List<DeviceSettings>();
    }

    public class DeviceData
    {
        public int DataId { get; set; }
        public string Data { get; set; }
        public string DeviceAttribute { get; set; }
        public string DeviceValue { get; set; }
    }

    public class DeviceSettings
    {
        public int SettingId { get; set; }
        public string Setting { get; set; }
        public string SettingOrder { get; set; }
        public string CameraSetting { get; set; }
        public string ActionSetting { get; set; }
    }
}
