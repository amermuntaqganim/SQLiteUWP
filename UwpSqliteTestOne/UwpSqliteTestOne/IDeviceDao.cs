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

        void InsertActions(List<DeviceAction> devaction);

        void InsertUrls(List<DeviceUrl> devurls);

        Task<List<Device>> GetData();

        Task<List<string>> GetUrlLinks();
        Task<List<string>> GetUrlsForAction(int actionId);
    }
}
