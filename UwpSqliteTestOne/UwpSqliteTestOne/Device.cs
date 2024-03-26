using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSqliteTestOne
{
    public class Device
    {
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class DeviceAction
    {
        public int ActionId { get; set; }
        public int DeviceId { get; set; }
        public string Name { get; set; }
    }

    public class DeviceUrl
    {
        public int ActionId { get; set; }
        public string Link { get; set; }
    }
}
