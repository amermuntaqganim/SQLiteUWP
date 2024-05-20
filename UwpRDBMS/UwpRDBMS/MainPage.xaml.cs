using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace UwpRDBMS
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

        private void Insert_Click(object sender, RoutedEventArgs e)
        {

            DeviceDao.Instance.InsertAllDeviceInfo("device-123", "name_one", "action_one");
            DeviceDao.Instance.InsertDeviceData("device-123", "Sample Data", "Sample data atrribute one", "sample value one");
            DeviceDao.Instance.InsertDeviceSettings("device-123", "Sample Settings");

            DeviceDao.Instance.InsertAllDeviceInfo("device-123", "name_one", "action_two");
            DeviceDao.Instance.InsertDeviceData("device-123", "Sample Data Two", "Sample data atrribute two", "sample value two");
            DeviceDao.Instance.InsertDeviceSettings("device-123", "Sample Settings TWO");

            DeviceDao.Instance.InsertAllDeviceInfo("device-234", "name_three", "action_three");
            DeviceDao.Instance.InsertDeviceData("device-234", "Sample Data Three", "Sample data atrribute three", "sample value three");
            DeviceDao.Instance.InsertDeviceSettings("device-234", "Sample Settings Three");

            //DeviceDao.Instance.DeleteDevice("device-123");


            // For Nested Child

            DeviceDao.Instance.InsertDeviceState("device-123", "New State", DateTime.Now);
            DeviceDao.Instance.UpdateDeviceState("device-123", "Updated State", DateTime.Now);


            List<Device> devices = DeviceDao.Instance.GetDevicesWithDataAndStates();

            // Example usage: Print the devices and their data
            foreach (var device in devices)
            {
                Debug.WriteLine($"Device: {device.Id}, Name: {device.DeviceName}, Device Id: {device.DeviceId}");
                foreach (var data in device.DeviceDataList)
                {
                    Debug.WriteLine($"\tData: {data.Data}, Additional Info: {data.DeviceAttribute}");

                    foreach (var state in data.DeviceStatesList)
                    {
                        Debug.WriteLine($"\tState: {state.StateId}, Status: {state.State}, TimeStamp: { state.Timestamp }");
                    }
                }
                foreach (var setting in device.DeviceSettingsList)
                {
                    Debug.WriteLine($"\tSetting: {setting.Setting}");
                }
            }



        }
    }
}
