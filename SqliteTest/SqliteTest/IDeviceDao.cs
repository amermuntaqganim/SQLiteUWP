using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqliteTest
{
    public interface IDeviceDao
    {
        void InsertDevice(Device device);
    }
}
