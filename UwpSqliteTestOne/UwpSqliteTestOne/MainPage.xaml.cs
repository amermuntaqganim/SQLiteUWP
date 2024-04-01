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
using Windows.UI.Xaml.Media.Imaging;
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

            List<DeviceAction> actions = new List<DeviceAction>();
            List<DeviceUrl> urls = new List<DeviceUrl>();

            await Task.Run(async () =>
            {



                for (int i = 0; i < 100; i++)
                {
                    Device dev = new Device()
                    {
                        Name = "Hello " + i,
                        Description = "World " + i
                    };

                    await DeviceDao.Instance.InsertData(dev);

                    for (int j = 0; j < 10; j++)
                    {
                        DeviceAction devaction = new DeviceAction()
                        {
                            DeviceId = i + 1,
                            Name = "Action with hello " + j + 1
                        };

                        actions.Add(devaction);

                        for (int k = 0; k < 10; k++)
                        {
                            DeviceUrl deviceUrl = new DeviceUrl()
                            {
                                //ActionId = j + 1,
                                // Link = k+"_"+"https//url/?/dfd"
                                ActionId = j+1,
                                Link = null
                            };
                           
                            urls.Add(deviceUrl);
                        }
                    }



                }



               // handler?.Invoke(sender, new EventArgs());
                
            });

            await Task.Run(() => {
                DeviceDao.Instance.InsertActions(actions);
                
            });

            await Task.Run(() => {
                 DeviceDao.Instance.InsertUrls(urls);

            });

            
            await Task.Run(() => {

                var list = DeviceDao.Instance.GetData().Result;
                
                foreach(var dev in list)
                {
                    Debug.WriteLine(dev.Name + " " + dev.Description);
                }
            });


            await Task.Run(() => {

                //var list = DeviceDao.Instance.GetUrlLinks().Result;

                var list = DeviceDao.Instance.GetUrlsForAction(5).Result;
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


        private async void CacheImagesButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Chace Images Button Clicked");

            await DbManager.Instance.InitializeDatabase();

            List<string> urls = new List<string>();
            urls.Add("https://via.placeholder.com/150");
            urls.Add("https://via.placeholder.com/200");
            urls.Add("https://via.placeholder.com/300");

            await ImageManager.Instance.DownloadAndSaveImages(urls);
        }

        private async void LoadCachedImagesButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] imageData = await ImageManager.Instance.GetImageFromDatabase("https://via.placeholder.com/300");

            if (imageData != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    _ = bitmapImage.SetSourceAsync(stream.AsRandomAccessStream());
                }
                ImageView.Source = bitmapImage;
            }
            else
            {
                // Handle case where image not found
            }
        }
    }
}
