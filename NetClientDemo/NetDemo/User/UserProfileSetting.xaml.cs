using NetDemo.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NetDemo
{
    /// <summary>
    /// UserProfilSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserProfileSetting : UserControl
    {
        private Bitmap _bitmap;
        private BitmapImage _bitmapImage;
        private string _imgPath;
        private string _base64String;

        public UserProfileSetting()
        {
            InitializeComponent();
            NetProxy.Get.OnMessageEvent += OnMessage_Recv;

            // 원래의 유저 프로필사진 받아오기
            JObject json = new JObject(
                new JProperty("msgType", "UserImg")
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }
        private void OnMessage_Recv(string msg)
        {
            JObject json = JObject.Parse(msg);
            string messageType = (string)json["msgType"];
            Bitmap bit;
            switch (messageType)
            {
                case "UserImg":
                    bit = Base64StringToBitmap(json["userimg"].ToString());
                    imgView.Source = BitmapToImageSource(bit);
                    break;
                case "MyImg":
                    MessageBox.Show("수정 완료");

                    break;
                default:
                    break;
            }
        }
        // string -> bitmap
        public static Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;

            byte[] byteBuffer = Convert.FromBase64String(base64String);

            MemoryStream memoryStream = new MemoryStream(byteBuffer);

            memoryStream.Position = 0;

            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);

            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;

            return bmpReturn;
        }

        // bitmap -> string
        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        #region 버튼 이벤트
        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {
            JObject json = new JObject(
                new JProperty("msgType", "ProfilSave"),
                new JProperty("imgSource", _base64String)
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }

        private void SetBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "이미지 파일|*.png;";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                _imgPath = dialog.FileName;
                _bitmap = new Bitmap(_imgPath);

                _bitmapImage = new BitmapImage(new Uri(_imgPath));

                byte[] imageArray;
                using (MemoryStream stream = new MemoryStream())
                {
                    _bitmap.Save(stream, _bitmap.RawFormat);
                    imageArray = stream.ToArray();
                }
                _base64String = Convert.ToBase64String(imageArray);

                imgView.Source = _bitmapImage;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            picGrid.Children.Clear();
            picGrid.Children.Add(new UserList());
        }
        #endregion
    }
}
