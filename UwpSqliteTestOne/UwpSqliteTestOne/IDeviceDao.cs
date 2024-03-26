using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSqliteTestOne
{
    public interface IDeviceDao
    {
        void InsertData(Device device);

        List<Device> GetData();
    }
}
