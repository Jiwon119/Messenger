using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NetDemo
{
    /// <summary>
    /// Users.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Users : UserControl
    {
        public Users()
        {
            InitializeComponent();
            MouseDoubleClick += User_MouseDoubleClick;
        }
        private void User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NetDemo.UserDetail detail = new NetDemo.UserDetail(IDText.Text);
            detail.Show();
        }

        public void UserConnectTrue()
        {
            ConnectInfo.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#93d3c6"));
        }
    }
}
