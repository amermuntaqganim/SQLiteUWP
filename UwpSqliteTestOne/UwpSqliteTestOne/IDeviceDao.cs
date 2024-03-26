using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpSqliteTestOne
{
    public interface IDeviceDao
    {
        Task InsertData(Device device);

        Task InsertActions(List<DeviceAction> devaction);

        Task InsertUrls(List<DeviceUrl> devurls);

        Task<List<Device>> GetData();

        Task<List<string>> GetUrlLinks();
    }
}
