using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NetDemo
{
    /// <summary>
    /// ChatRooms.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatRoomsLists : UserControl
    {
        public ChatRoomsLists()
        {
            InitializeComponent();
            MouseDoubleClick += UserControl_MouseDoubleClick;
        }
        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            JObject json = new JObject(
                    new JProperty("msgType", "ChatRoomClick"),
                    new JProperty("chatInfo", RoomID.Text)
                    );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }


        private void DockPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            DockPanel.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#e0e0e0"));
        }

        private void DockPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            DockPanel.Background = Brushes.Transparent;
        }
    }
}
