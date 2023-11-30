using NetDemo.Network;
using NetDemo.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace NetDemo
{
    public partial class SignUp : UserControl
    {
        public SignUp()
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;
        }
        private void OnMessage_Recv(string msg)
        {
            MessageBox.Show(msg);
        }

        private void SignUpBtn(object sender, RoutedEventArgs e)
        {
            if( newID.Text != "" && newPW.Text != "" &&
                newName.Text != "" && newPhon.Text != "")
            {
                JObject json = new JObject(
                    new JProperty("msgType", "SignUp"),
                    new JProperty("NewUserID", newID.Text),
                    new JProperty("NewUserPW", newPW.Text),
                    new JProperty("NewUserName", newName.Text),
                    new JProperty("NewUserPhon", newPhon.Text)
                    );
                string StringJson = JsonConvert.SerializeObject(json);
                NetProxy.Get.SendMessage(StringJson);
            }
            else
            {
                MessageBox.Show("모든 값을 입력해주세요.");
            }


        }

        private void backBtnClick(object sender, RoutedEventArgs e)
        {
            SignUpGrid.Children.Clear();
            SignUpGrid.Children.Add(new Login());
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
        }

    }
}
