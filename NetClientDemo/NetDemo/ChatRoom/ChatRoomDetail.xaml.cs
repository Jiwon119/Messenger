using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetDemo
{
    public partial class ChatRoomDetail : UserControl
    {
        private string _roomId;
        private string _myName;
        private Array _roomUser;
        public ChatRoomDetail(string RoomID)
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;

            _roomId = RoomID;
            ChatRoomInfoRef();
            Init();
            
            ChatScroll.ScrollToBottom();
        }
        public void Init()
        {
            JObject json = new JObject(
                    new JProperty("msgType", "ReqChatInfo"),
                    new JProperty("RoomID", _roomId)
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }

        private void OnMessage_Recv(string msg)
        {
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            byte[] utf8Bytes = utf8.GetBytes(msg);
            string decodedStringByUTF8 = utf8.GetString(utf8Bytes);

            JObject json = JObject.Parse(msg);
            string messageType = (string)json["msgType"];



            switch (messageType)
            {
                case "ChatRoomInfo":
                    _myName = json["UserName"].ToString();
                    _roomUser = json["RoomUser"].ToArray();
                    TopRoomName.Text = json["RoomName"].ToString();
                    int cnt = 0;
                    foreach(var a in json["RoomUser"])
                    {

                        if (cnt > 2)
                        {
                            TextBlock skip = new TextBlock();
                            skip.Text = "...";
                            skip.Margin = new Thickness(5, 0, 5, 0);
                            TopUserList.Children.Add(skip);

                            break;
                        }
                        cnt += 1;

                        TextBlock user = new TextBlock();
                        user.Text = a.ToString();
                        user.Margin = new Thickness(5, 0, 5, 0);
                        TopUserList.Children.Add(user);


                    }

                    TopUserListCount.Text = json["RoomUser"].Count().ToString();
                    if (_myName == json["RoomLeader"].ToString())
                    {
                        ModifyBtn.Visibility = Visibility.Visible;
                    }
                    break;

                case "ChatList":
                    ShowChatList(json);
                    break;

                case "CreateRoom_GuestInfo":
                    MessageBoxResult resultCreateRoom = MessageBox.Show("새로운 채팅방에 초대되었습니다.\n확인하시겠습니까?",
                        "알림", MessageBoxButton.OKCancel);
                    if (resultCreateRoom == MessageBoxResult.OK)
                    {
                        NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                        ChatRoomGrid.Children.Clear();
                        ChatRoomGrid.Children.Add(new ChatRoomDetail(json["RoomID"].ToString()));
                    }
                    else
                    {
                        MessageBox.Show("취소버튼클릭");
                    }
                    break;

                case "Chat_GuestInfo":
                    if (json["ChatRoomId"].ToString() == _roomId)
                    {
                        var ChatUserInfo = json["UserName"].ToString();
                        var ChatContentInfo = json["Content"].ToString();
                        var ChatCreateDate = json["CreDate"].ToString();
                        ChatBasicBubbleSetting(ChatUserInfo, ChatContentInfo, ChatCreateDate);
                        ChatScroll.ScrollToBottom();

                        JObject jsonMsg = new JObject(
                            new JProperty("msgType", "UserInChatRoom"),
                            new JProperty("RoomID", _roomId)
                        );
                        string StringJson = JsonConvert.SerializeObject(jsonMsg);
                        NetProxy.Get.SendMessage(StringJson);
                    }
                    else
                    {
                        Notification notification = new Notification();
                        notification.NoticeChat(json);
                    }
                    break;

                case "success_ChatRoomOut":
                    MessageBox.Show("채팅방을 나갔습니다.");
                    NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                    ChatRoomGrid.Children.Clear();
                    ChatRoomGrid.Children.Add(new ChatRoomList());
                    break;
                case "error_ChangeRoomLeader":
                    MessageBox.Show("방장은 채팅방을 나갈 수 없습니다.\n방장을 변경 후 시도하십시오.");
                    break;
                default:
                    break;
            }
        }
        private void ShowChatList(JObject json)
        {
            Viewer.Children.Clear();
            foreach (var item in json["ChatList"])
            {
                var UserID = item["UserID"].ToString();
                var Content = item["Content"].ToString();
                var CreDate = item["CreateTime"].ToString();
                if(item["UserID"].ToString() == "sys")
                {
                    ChatRoomNotice(Content);
                }
                else if (_myName == item["UserID"].ToString())
                {
                    ChatMyBubbleSetting(UserID, Content, CreDate);
                }
                else
                {
                    ChatBasicBubbleSetting(UserID, Content, CreDate);
                }
            }
        }
        private void ChatRoomInfoRef()
        {
            JObject json = new JObject(
                    new JProperty("msgType", "ReqChatRoomInfo"),
                    new JProperty("RoomID", _roomId)
                    );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }

        #region BtnClick
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            ChatRoomGrid.Children.Clear();
            ChatRoomGrid.Children.Add(new ChatRoomList());
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            string Content = InputTextBox.Text;
            JObject json = new JObject(
                    new JProperty("msgType", "ChatRoomSendMsg"),
                    new JProperty("RoomID", _roomId),
                    new JProperty("Content", Content)
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);

            ChatMyBubbleSetting(_myName, Content, DateTime.Now.ToString("yy-MM-dd HH:mm"));
            ChatScroll.ScrollToBottom();
            InputTextBox.Text = "";
        }

        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            ChatRoomGrid.Children.Clear();
            ChatRoomGrid.Children.Add(new ChatRoomModify(_roomId));
        } 
        
        private void OutRoomBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("채팅방을 나가시겠습니까?", "알림", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                JObject json = new JObject(
                    new JProperty("msgType", "ChatRoomOut"),
                    new JProperty("RoomID", _roomId)
                );
                string StringJson = JsonConvert.SerializeObject(json);
                NetProxy.Get.SendMessage(StringJson);
            }
        }
        #endregion


        #region ChatBubbleSetting
        private void ChatMyBubbleSetting(string UserID, string Content, string CreDate)
        {
            ChatBubbleMy bubble = new ChatBubbleMy();
            bubble.SayUserID.Text = UserID;
            bubble.ChatContent.Text = Content;
            bubble.ChatContent.TextAlignment = TextAlignment.Right;
            bubble.HorizontalAlignment = HorizontalAlignment.Right;
            bubble.TimeTextBlock.Text = CreDate;

            Viewer.Children.Add(bubble);
        }
        private void ChatBasicBubbleSetting(string UserID, string Content, string CreDate)
        {
            ChatBubble bubble = new ChatBubble();
            bubble.SayUserID.Text = UserID;
            bubble.ChatContent.Text = Content;
            bubble.HorizontalAlignment = HorizontalAlignment.Left;
            bubble.TimeTextBlock.Text = CreDate;

            Viewer.Children.Add(bubble);
        }
        private void ChatRoomNotice(string Content)
        {
            ChatRoomNotice notice = new ChatRoomNotice();
            notice.NoticeText.Text = Content;
            Viewer.Children.Add(notice);
        }


        #endregion


        #region userList Event
        private void UserList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string userlist = "";
            foreach(var user in _roomUser)
            {
                userlist += user.ToString() + " ";
            }
            MessageBox.Show(userlist);
        }

        private void UserList_MouseEnter(object sender, MouseEventArgs e)
        {
            UserListBox.Opacity = 0.7;
            UserListBox.Cursor = Cursors.Hand;
        }
        private void UserList_MouseLeave(object sender, MouseEventArgs e)
        {
            UserListBox.Opacity = 1;
        }
        #endregion

    }
}
