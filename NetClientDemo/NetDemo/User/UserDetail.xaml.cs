using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetDemo
{
    /// <summary>
    /// UserDetail.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserDetail : Window
    {
        private string _userId;
        public UserDetail(string userId)
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
            _userId = userId;


            JObject json = new JObject(
                new JProperty("msgType", "RefUserInfo"),
                new JProperty("userId", _userId)
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);

        }
        private void OnMessage_Recv(string msg)
        {
            JObject json = JObject.Parse(msg);
            string messageType = (string)json["msgType"];

            switch (messageType)
            {
                case "UserInfo":
                    userID.Text = _userId;
                    userName.Text = json["userName"].ToString();
                    userPhon.Text = json["userPhon"].ToString();
                    break;
                default:
                    break;
            }
        }
    }
}
