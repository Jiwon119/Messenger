using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NetDemo
{
    /// <summary>
    /// UserInfoModify.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInfoModify : UserControl
    {
        private string _myName;
        public UserInfoModify(string myName)
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
            _myName = myName;
            UserId.Text = _myName;
        }
        private void OnMessage_Recv(string msg)
        {
            JObject json = JObject.Parse(msg);
            string messageType = (string)json["msgType"];

            switch (messageType)
            {
                case "Identification success":
                    MessageBox.Show("본인확인에 성공하셨습니다.");
                    ModifySetting(json);
                    break;

                case "Identification fail":
                    MessageBox.Show("비밀번호를 다시 확인해주세요.");
                    break;

                case "modify completed":
                    MessageBox.Show("수정이 완료되었습니다.");

                    NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                    UserInfoModifyGrid.Children.Clear();
                    UserInfoModifyGrid.Children.Add(new UserList());
                    break;

                case "Chat_GuestInfo":
                    Notification notification = new Notification();
                    notification.NoticeChat(json);
                    break;

                default:
                    break;
            }
        }

        private void ModifySetting(JObject json)
        {
            IdentificationBtn.Visibility = Visibility.Collapsed;
            ModifyBox.Visibility = Visibility.Visible;

            modifyName.Text = json["userName"].ToString();
            modifyPhon.Text = json["userPhon"].ToString();

        }
        
        private void IdentificationBtn_Click(object sender, RoutedEventArgs e)
        {
            JObject Msg = new JObject(
                new JProperty("msgType", "Identification"),
                new JProperty("password", verifyPwBox.Password)
                );
            string SendMsg = JsonConvert.SerializeObject(Msg);
            NetProxy.Get.SendMessage(SendMsg);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            UserInfoModifyGrid.Children.Clear();
            UserInfoModifyGrid.Children.Add(new UserList());
        }

        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {
            if(PasswordBtn.Visibility == Visibility.Visible)
            {
                JObject Msg = new JObject(
                    new JProperty("msgType", "ModifyUserInfo"),
                    new JProperty("userName", modifyName.Text),
                    new JProperty("usrPhon", modifyPhon.Text)
                    );
                string SendMsg = JsonConvert.SerializeObject(Msg);
                NetProxy.Get.SendMessage(SendMsg);
            }
            else
            {
                // 비밀번호 수정 버튼을 클릭한 경우
                // 수정한 비밀번호까지 서버에 보내줌
                if (modifyPw.Password != verifyPw.Password)
                {
                    MessageBox.Show("비밀번호를 다시 확인해주세요.");
                }
                else
                {
                    JObject Msg = new JObject(
                        new JProperty("msgType", "ModifyUserInfo"),
                        new JProperty("userName", modifyName.Text),
                        new JProperty("usrPhon", modifyPhon.Text),
                        new JProperty("userPw", modifyPw.Password)
                        );
                    string SendMsg = JsonConvert.SerializeObject(Msg);
                    NetProxy.Get.SendMessage(SendMsg);
                }
            }
        }

        private void PasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            PasswordBtn.Visibility = Visibility.Collapsed;
            modifyPw.Visibility = Visibility.Visible;
            verifyPwTextBlock.Visibility = Visibility.Visible;
            verifyPw.Visibility = Visibility.Visible;
        }
    }
}
