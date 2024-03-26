using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpSqliteTestOne
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        EventHandler<EventArgs> handler;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            handler += OnUpdateDB;

            Debug.WriteLine("Button clicked");

            await DbManager.Instance.InitializeDatabase();

            await Task.Run(() =>
            {

                List<DeviceAction> actions = new List<DeviceAction>();
                List<DeviceUrl> urls = new List<DeviceUrl>();

                for (int i = 0; i < 100; i++)
                {
                    Device dev = new Device()
                    {
                        Name = "Hello " + i,
                        Description = "World " + i
                    };

                    DeviceDao.Instance.InsertData(dev);

                    DeviceAction devaction = new DeviceAction()
                    {
                        DeviceId = i + 1,
                        Name = "Action with hello " + i + 1
                    };

                    actions.Add(devaction);

                    DeviceUrl deviceUrl = new DeviceUrl()
                    {
                        ActionId = i + 1,
                        Link = "https//url/?/dfd"
                    };
                    urls.Add(deviceUrl);
                }

                DeviceDao.Instance.InsertActions(actions);
                DeviceDao.Instance.InsertUrls(urls);

                handler?.Invoke(sender, new EventArgs());
                return Task.CompletedTask;
            });

            await Task.Run(() => {

                var list = DeviceDao.Instance.GetData().Result;
                
                foreach(var dev in list)
                {
                    Debug.WriteLine(dev.Name + " " + dev.Description);
                }
            });


            await Task.Run(() => {

                var list = DeviceDao.Instance.GetUrlLinks().Result;

                foreach (var url in list)
                {
                    Debug.WriteLine(url);
                }
            });

            //DeviceDao.Instance.InsertData(new Device() { Name = "Hello", Description = "World" });
        }

        private void OnUpdateDB(object sender, EventArgs e)
        {
            Debug.WriteLine("Data Inserted");
        }
    }
}
