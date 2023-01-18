using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NetDemo
{
    /// <summary>
    /// ChatRoomModify.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatRoomModify : UserControl
    {
        private string _roomId;
        public ChatRoomModify(string RoomId)
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
            _roomId = RoomId;

            // userList 요청
            JObject json = new JObject(
                new JProperty("msgType", "ReqChatRoomModifyUserList"),
                new JProperty("RoomID", _roomId)
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
                case "ChatRoomModifyUserList":
                    Setting(json);
                    break;

                case "ModifySuccess":
                    MessageBox.Show("수정이 완료되었습니다.");
                    NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                    CreateGrid.Children.Clear();
                    CreateGrid.Children.Add(new ChatRoomDetail(_roomId));
                    break;

                case "Chat_GuestInfo":
                    if (json["ChatRoomId"].ToString() != _roomId)
                    {
                        Notification notification = new Notification();
                        notification.NoticeChat(json);
                    }
                    break;

                default:
                    break;
            }
        }

        private void Setting(JObject json)
        {
            List<string> InUser = new List<string>();
            List<string> OutUser = new List<string>();
            LeaderChangeComboBox.Items.Add("- 변경없음 -");

            foreach (var a in json["InUser"])
            {
                InUser.Add(a.ToString());
                LeaderChangeComboBox.Items.Add(a.ToString());
            }
            foreach (var a in json["OutUser"])
            {
                OutUser.Add(a.ToString());
            }
            
            RemoveUserList.ItemsSource = InUser;
            AddUserList.ItemsSource = OutUser;
            ChatRoomName.Text = json["RoomName"].ToString();
        }

        #region event
        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {
            JObject json = new JObject(
                new JProperty("msgType", "ModifyChatRoom"),
                new JProperty("RoomID", _roomId),
                new JProperty("ChatRoomName", ChatRoomName.Text),
                new JProperty("RemoveUserSelect", JArray.FromObject(RemoveUserSelect.Items)),
                new JProperty("AddUserSelect", JArray.FromObject(AddUserSelect.Items))
                );

            if (LeaderChangeComboBox.SelectedIndex != 0)
            {
                json.Add(new JProperty("LeaderChange", LeaderChangeComboBox.SelectedItem.ToString()));
            }
            else
            {
                json.Add(new JProperty("LeaderChange", ""));
            }
            
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            CreateGrid.Children.Clear();
            CreateGrid.Children.Add(new ChatRoomDetail(_roomId));
        }

        private void RemoveUserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                RemoveUserSelect.Items.Add(e.AddedItems[0]);
            }
            else if (e.RemovedItems.Count != 0)
            {
                RemoveUserSelect.Items.Remove(e.RemovedItems[0]);
            }
            else
            {
                return;
            }
        }

        private void AddUserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                AddUserSelect.Items.Add(e.AddedItems[0]);
            }
            else if (e.RemovedItems.Count != 0)
            {
                AddUserSelect.Items.Remove(e.RemovedItems[0]);
            }
            else
            {
                return;
            }
        }

        private void LeaderChangeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LeaderChangeComboBox.SelectedIndex == 0)
            {
                LeaderText.Text = LeaderChangeComboBox.SelectedItem.ToString();
            }
            else
            {
                LeaderText.Text = $" {LeaderChangeComboBox.SelectedItem} 님으로 방장 변경";
            }
            
        }

        #endregion


    }

    class UpdateUserList
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string ConnInfo { get; set; }
    }
}
