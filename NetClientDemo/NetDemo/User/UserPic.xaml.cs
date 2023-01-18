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
    /// UserPci.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserPic : UserControl
    {
        private Bitmap _bitmap;
        private BitmapImage _bitmapImage;
        private string _imgPath;
        private string _base64String;

        public UserPic()
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
                case "UserImg":
                    Bitmap bit = Base64StringToBitmap(json["userimg"].ToString());
                    imgBitmapView.Source = BitmapToImageSource(bit);
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

        // Bitmap 이미지를 png로 저장
        //public static void SaveAsPng(Uri src)
        //{
        //    var appPath = System.Environment.CurrentDirectory + @"\Data";

        //    //if (!appPath.Exists) { di.Create(); }
        //    Uri filePath2 = new Uri("userimg\\a.png", UriKind.Relative);
        //    string filePath = filePath2.AbsolutePath.ToString();

        //    FileStream stream = new FileStream(filePath, FileMode.Create);

        //    PngBitmapEncoder encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(src));

        //    encoder.Save(stream);

        //    stream.Close();
        //}

        // 사진 크기 조정 -> 오류남 이상함....
        //public Bitmap Resize(string importPath)
        //{
        //    System.Drawing.Image originalImage = System.Drawing.Image.FromFile(importPath);

        //    double ratioX = _maxWidth / (double)originalImage.Width;
        //    double ratioY = _maxHeight / (double)originalImage.Height;

        //    double ratio = Math.Min(ratioX, ratioY);

        //    int newWidth = (int)(originalImage.Width * ratio);
        //    int newHeight = (int)(originalImage.Height * ratio);

        //    Bitmap newImage = new Bitmap(_maxWidth, _maxHeight);
        //    using (Graphics g = Graphics.FromImage(newImage))
        //    {
        //        g.FillRectangle(Brushes.Black, 0, 0, newImage.Width, newImage.Height);
        //        g.DrawImage(originalImage, (_maxWidth - newWidth) / 2, (_maxHeight - newHeight) / 2, newWidth, newHeight);
        //    }
        //    originalImage.Dispose();

        //    return newImage;
        //}

        #region BtnClick Event
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NetProxy.Get.OnMessageEvent -= OnMessage_Recv;
            picGrid.Children.Clear();
            picGrid.Children.Add(new UserList());
        }


        // 사진 선택 버튼 클릭
        private void setBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "이미지 파일|*.png;";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                _imgPath = dialog.FileName;
                _bitmap = new Bitmap(_imgPath);
                //if (_bitmap.Width > _maxWidth)
                //{
                //    _bitmap = Resize(_imgPath);
                //    MessageBox.Show(_bitmap.Width.ToString());
                //    MessageBox.Show(_bitmap.Height.ToString());
                //}

                _bitmapImage = new BitmapImage(new Uri(_imgPath));
                
                byte[] imageArray;
                using (MemoryStream stream = new MemoryStream())
                {
                    _bitmap.Save(stream, _bitmap.RawFormat);
                    imageArray = stream.ToArray();
                }
                _base64String = Convert.ToBase64String(imageArray);

                //byte[] imageArray = System.IO.File.ReadAllBytes(_bitmap);
                //MessageBox.Show(imageArray.ToString());


                imgView.Source = _bitmapImage;
            }

        }

        // 사진 등록 버튼 클릭
        // 디비에 저장할 수 있도록
        private void mypic_Click(object sender, RoutedEventArgs e)
        {

            JObject json = new JObject(
                new JProperty("msgType", "UserImg"),
                new JProperty("imgSource", _base64String)
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);


            //Stream imageStreamSource = new FileStream(_imgPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //BitmapSource bitmapSource = decoder.Frames[0];
            
            //System.Windows.Controls.Image myImage = new System.Windows.Controls.Image();
            //myImage.Source = bitmapSource;

            //myImage.Tag = System.IO.Path.GetFullPath(_imgPath);
            //imgBitmapView.Children.Add(myImage);

            //insertImageData();
            //byte[] imageArray = System.IO.File.ReadAllBytes(_imgPath);
            //MessageBox.Show(imageArray.ToString());
            //string base64String = Convert.ToBase64String(imageArray);
            //MessageBox.Show(base64String);

            ////var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64String)));
            //var img = new BitmapImage(new Uri(Convert.FromBase64String(base64String).ToString()));

            ////imgBitmapView.Source = img;
        }

        // 사진 보기 버튼 클릭
        // 등록된 사진 요청 후 화면에 띄우는
        private void mypic_result_Click(object sender, RoutedEventArgs e)
        {
            JObject json = new JObject(
                new JProperty("msgType", "imgRead")
                );
            string StringJson = JsonConvert.SerializeObject(json);
            NetProxy.Get.SendMessage(StringJson);
        }

        #endregion

        //private void insertImageData()
        //{
        //    try
        //    {
        //        if (_imgPath != "")
        //        {

        //            Initialize a file stream to read the image file
        //            FileStream fs = File.OpenRead(_imgPath);

        //            Initialize a byte array with size of stream
        //            byte[] imgByteArr = new byte[fs.Length];

        //            Read data from the file stream and put into the byte array
        //            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

        //            Close a file stream
        //            fs.Close();

        //            using (SqlConnection conn = new SqlConnection(constr))
        //            {
        //                conn.Open();
        //                string sql = "insert into tbl_bitmapImage(id,img) values('" + strName + "',@img)";
        //                using (SqlCommand cmd = new SqlCommand(sql, conn))
        //                {
        //                    //Pass byte array into database
        //                    cmd.Parameters.Add(new SqlParameter("img", imgByteArr));
        //                    int result = cmd.ExecuteNonQuery();
        //                    if (result == 1)
        //                    {
        //                        MessageBox.Show("Image added successfully.");
        //                        BindImageList();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}
    }
}
