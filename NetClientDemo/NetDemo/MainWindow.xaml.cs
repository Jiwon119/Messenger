using Microsoft.Toolkit.Uwp.Notifications;
using NetDemo.Network;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows;

namespace NetDemo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // note
            //NetProxy.Get.OnMessageEvent += OnMessage_Recv;
            //NetProxy.Get.OnMessageEvent -= OnMessage_Recv;



            mainGrid.Children.Add(new Login());

            Connect();
        }

        

        private void Connect()
        {
            NetProxy.Get.ConnectToServer("127.0.0.1", 6767);
        }

        public class Message
        {
            public Dictionary<string, string> Msg { get; set; }
            public Dictionary<string, string> Login { get; set; }
            public Dictionary<string, string> SignUp { get; set; }
        }
    }
}
