using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace NetDemo
{
    /// <summary>
    /// UserList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserList : UserControl
    {
        private string _myName;
        public UserList()
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
            RequestMyName();
            RequestList();
            ReqestMyImg();
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

                case "NameReq":
                    _myName = json["MyName"].ToString();
                    myName.Text = _myName;
                    break;

                case "CreateRoom_GuestInfo":
                    MessageBoxResult resultCreateRoom = MessageBox.Show("새로운 채팅방에 초대되었습니다.\n확인하시겠습니까?",
                        "알림", MessageBoxButton.OKCancel);
                    if (resultCreateRoom == MessageBoxResult.OK)
                    {
                        NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                        UserListGrid.Children.Clear();
                        UserListGrid.Children.Add(new ChatRoomDetail(json["RoomID"].ToString()));
                    }
                    else
                    {
                        MessageBox.Show("취소버튼클릭");
                    }
                    break;

                case "Chat_GuestInfo":
                    Notification notification = new Notification();
                    notification.NoticeChat(json);
                    break;
                case "Logout Success":
                    NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                    UserListGrid.Children.Clear();
                    UserListGrid.Children.Add(new Login());
                    break;

                case "UserImg":
                    if(json["userimg"].ToString() != "")
                    {
                        Bitmap bit = UserProfileSetting.Base64StringToBitmap(json["userimg"].ToString());
                        myImg.Source = UserProfileSetting.BitmapToImageSource(bit);
                    }
                    break;
                default:
                    break;
            }
        }
        private void UserListSetting(JObject json)
        {
            UserListStack.Children.Clear();

            foreach (var a in json["UserList"])
            {
                var userName = a["UserName"].ToString();
                var userID = a["UserID"].ToString();
                var conInfo = a["ConnectInfo"].ToString();
                var userImg = a["UserImg"].ToString();
                Users u = new Users();
                if(userImg != "")
                {
                    Bitmap bit = UserProfileSetting.Base64StringToBitmap(userImg);
                    u.userImg.Source = UserProfileSetting.BitmapToImageSource(bit);
                }
                u.NameText.Text = userName;
                u.IDText.Text = userID;
                if (conInfo == "true")
                {
                    u.UserConnectTrue();
                }
                UserListStack.Children.Add(u);
            }
        }

        private void RequestList()
        {
            JObject json = new JObject(
                new JProperty("msgType", "ReqUserList")
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }

        private void RequestMyName()
        {
            JObject json = new JObject(
                new JProperty("msgType", "ReqName")
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }

        private void ReqestMyImg()
        {
            JObject json = new JObject(
                new JProperty("msgType", "UserImg")
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }

        #region Btn_Click
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            RequestList();
        }

        private void ChatRoomBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            UserListGrid.Children.Clear();
            UserListGrid.Children.Add(new ChatRoomList());
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultChat = MessageBox.Show("로그아웃 하시겠습니까?",
                        "로그아웃", MessageBoxButton.OKCancel);
            if (resultChat == MessageBoxResult.OK)
            {
                JObject json = new JObject(
                    new JProperty("msgType", "Logout")
                    );
                string StringJson = JsonConvert.SerializeObject(json);
                NetProxy.Get.SendMessage(StringJson);
            }
        }

        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            UserListGrid.Children.Clear();
            UserListGrid.Children.Add(new UserInfoModify(_myName));
        }
        private void PicBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            UserListGrid.Children.Clear();
            UserListGrid.Children.Add(new UserProfileSetting());
        }
        #endregion
    }
}
