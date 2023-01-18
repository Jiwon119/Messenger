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
    /// ChatRoomList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatRoomList : UserControl
    {
        public ChatRoomList()
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
            JObject json = new JObject(
                new JProperty("msgType", "showRoomList")
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
                case "ChatRoomList":
                    ShowRoomList(json);
                    break;
                case "RoomEnter":
                    NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                    ChatRoomGrid.Children.Clear();
                    ChatRoomGrid.Children.Add(
                        new ChatRoomDetail(json["RoomNum"].ToString()));
                    break;
                case "Chat_GuestInfo":
                    JObject jsonMsg = new JObject(
                        new JProperty("msgType", "showRoomList")
                        );
                    string StringJson = JsonConvert.SerializeObject(jsonMsg);
                    NetProxy.Get.SendMessage(StringJson);
                    break;
                case "CreateRoom_GuestInfo":
                    JObject reqJson = new JObject(
                        new JProperty("msgType", "showRoomList")
                        );
                    string StringReqJson = JsonConvert.SerializeObject(reqJson);
                    NetProxy.Get.SendMessage(StringReqJson);
                    break;
                default:
                    break;
            }
        }

        private void ShowRoomList(JObject json)
        {
            RoomListStack.Children.Clear();
            foreach (var item in json["RoomList"])
            {
                var RoomID = item["RoomID"].ToString();
                var RoomName = item["RoomName"].ToString();
                var userList = item["RoomUser"];
                var CreationTime = item["CreationTime"].ToString();
                var newChatCount = item["NewChat"].ToString();

                string userListStr = "";
                foreach(var a in userList)
                {
                    userListStr += a + "   ";
                }

                ChatRoomsLists u = new ChatRoomsLists();
                u.RoomID.Text = RoomID;
                u.RoomName.Text = RoomName;
                if(newChatCount != "0")
                {
                    u.newChatCount.Text = newChatCount;
                    u.NewChatNum.Visibility = Visibility.Visible;
                }

                foreach (var a in userList)
                {
                    TextBlock block = new TextBlock();
                    block.Text = a.ToString();
                    block.Margin = new Thickness(5,0,5,0);
                    u.RoomUsers.Children.Add(block);
                }

                u.IDCountText.Text = userList.Count().ToString();
                u.CreDate.Text = CreationTime;

                RoomListStack.Children.Add(u);
            }
        }

        #region BtnClick
        private void UserListBtnClick(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            ChatRoomGrid.Children.Clear();
            ChatRoomGrid.Children.Add(new UserList());
        }

        private void CreateBtnClick(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            ChatRoomGrid.Children.Clear();
            ChatRoomGrid.Children.Add(new CreateRoom());
        }
        private void ResetBtnClick(object sender, RoutedEventArgs e)
        {
            JObject json = new JObject(
                new JProperty("msgType", "showRoomList")
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }
        #endregion
    }
}
