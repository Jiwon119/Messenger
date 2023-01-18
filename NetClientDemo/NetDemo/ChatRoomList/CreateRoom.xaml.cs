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
    /// CreateRoom.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CreateRoom : UserControl
    {
        public CreateRoom()
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
            RequestList();
        }

        private void OnMessage_Recv(string msg)
        {
            JObject json = JObject.Parse(msg);
            string messageType = (string)json["msgType"];

            switch (messageType)
            {
                case "UserListReq":
                    UserListSetting(json);
                    break;

                case "CreateRoom":
                    MessageBox.Show("Create Room Success");
                    NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                    CreateGrid.Children.Clear();
                    CreateGrid.Children.Add(new ChatRoomDetail(json["RoomID"].ToString()));
                    break;

                case "Chat_GuestInfo":
                    Notification notification = new Notification();
                    notification.NoticeChat(json);
                    break;

                default:
                    break;
            }
        }

        #region Btn
        private void CreBtnClick(object sender, RoutedEventArgs e)
        {

            var userList = selectedUserListBox.Items;


            JObject json = new JObject(
                new JProperty("msgType", "CreateChatRoom"),
                new JProperty("chatRoomName", ChatRoomName.Text),
                new JProperty("chatRoomUser", JArray.FromObject(userList))
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }
        private void BackBtnClick(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            CreateGrid.Children.Clear();
            CreateGrid.Children.Add(new ChatRoomList());
        }

        #endregion

        #region ListSetting
        private void RequestList()
        {
            JObject json = new JObject(
                new JProperty("msgType", "ReqUserList")
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }
        private void UserListSetting(JObject json)
        {
            List<CreUserList> items = new List<CreUserList>();

            foreach (var a in json["UserList"])
            {
                string con;
                if (a["ConnectInfo"].ToString() == "true")
                {
                    con = "#93d3c6";
                }
                else
                {
                    con = "#7f7f7f";
                }

                items.Add(new CreUserList()
                {
                    UserName = a["UserName"].ToString(),
                    UserID = a["UserID"].ToString(),
                    ConnInfo = con
                });
            }
            userList.ItemsSource = items;
        }
        private void Change(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                CreUserList user = e.AddedItems[0] as CreUserList;
                selectedUserListBox.Items.Add(user.UserID);
            }
            else if (e.RemovedItems.Count != 0) {
                CreUserList user = e.RemovedItems[0] as CreUserList;
                selectedUserListBox.Items.Remove(user.UserID);
            }
            else
            {
                return;
            }
        }
        class CreUserList
        {
            public string UserName { get; set; }
            public string UserID { get; set; }
            public string ConnInfo { get; set; }
        }
        #endregion


    }
}