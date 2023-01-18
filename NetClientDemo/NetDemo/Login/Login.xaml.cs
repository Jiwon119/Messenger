using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NetDemo
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
        }
        private void OnMessage_Recv(string msg)
        {
            JObject json = JObject.Parse(msg);
            string messageType = (string)json["msgType"];

            switch (messageType)
            {
                case "Login success":
                    MessageBox.Show(messageType);
                    NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
                    LoginGrid.Children.Clear();
                    LoginGrid.Children.Add(new UserList());
                    break;
                case "Wrong password":
                    MessageBox.Show("잘못된 패스워드입니다.");
                    break;
                case "Login fail":
                    MessageBox.Show("존재하지 않는 아이디입니다.");
                    break;
                case "connected user":
                    MessageBox.Show("이미 접속중인 유저입니다.");
                    break ;
                default:
                    break;
            }
        }

        private void LoginBtn(object sender, RoutedEventArgs e)
        {
            if(LoginID.Text !=  ""  && LoginPW.Password != "")
            {
                JObject json = new JObject(
                    new JProperty("msgType", "Login"),
                    new JProperty("LoginID", LoginID.Text),
                    new JProperty("LoginPW", LoginPW.Password)
                    );
                string StringJson = JsonConvert.SerializeObject(json);
                NetProxy.Get.SendMessage(StringJson);
            }
            else
            {
                MessageBox.Show("값을 모두 입력해주세요.");
            }
        }

        private void SignUpBtn(object sender, RoutedEventArgs e)
        {
            LoginGrid.Children.Clear();
            LoginGrid.Children.Add(new SignUp());
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
        }

        private void ConnectBtn(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.ConnectToServer("127.0.0.1", 6767);
        }
    }
}