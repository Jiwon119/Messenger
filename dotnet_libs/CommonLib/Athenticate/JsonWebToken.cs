using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommonLib.Loggers;
using Newtonsoft.Json.Linq;
using OtpNet;

namespace CommonLib.Athenticate
{
    public class JsonWebToken : IToken
    {
        public string url => @"http://1.218.138.141/authenticate-managing-server/User/authenticate";
        public int ExpiresDate => 7;
        public string Type => "JWT";
        public object Request { get; set; }

        
        private string userProfile = Environment.GetEnvironmentVariable("LOCALAPPDATA");

        // Env:LOCALAPPDATA 경로에 token을 저장한다.
        public Task SaveToken(string token)
        {
            /// Set JWT Path
            string folderPath = userProfile + "\\.jwt";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string file = DateTime.Now.ToString("yyyyMMdd hhmmss") + ".txt";
            string path = string.Join("\\", folderPath, file);

            /// File Write
            File.WriteAllText(path, token);

            return Task.CompletedTask;
        }

        // JWT는 만료기간이 있기 때문에, 요청한다면 기존에 있던 것을 사용하지 않고 무조건 생성한다.
        public async Task<string> RetrivedToken(string company, string department, string tokenType, string userName)
        {
            try
            {
                /// Create Request
                OTP otp = new OTP(Environment.GetEnvironmentVariable("SECRET_KEY"));

                Request = new JwtRequest()
                {
                    OTP = otp.GetOtp(),
                    Company = company,
                    Department = department,
                    TokenType = tokenType,
                    UserName = userName
                };

                var body = JsonSerializer.Serialize(Request as JwtRequest);

                /// Http Post including body
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                string token;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        JObject json = JObject.Parse(result);
                        token = json["token"].ToString();
                    }
                }

                /// Save Token
                await SaveToken(token);
                NLogger.Get.Info("Token Created!");
                return token;
            }
            catch (Exception ex)
            {
                NLogger.Get.Error(ex);
                return null;
            }
            
        }

        // 이미 발급한 토큰을 사용한다.
        public string GetRecentToken()
        {
            try
            {
                // Read Files
                string path = string.Join("\\", userProfile, ".jwt");

                string[] files = Directory.GetFiles(path, "*.txt");

                if (files.Length <= 0)
                    throw new Exception("File Not Exist");

                // Most Recently Token Select
                DateTime recentDate = DateTime.MinValue;
                int index = 0;
                for (int i = 0; i < files.Length; i++)
                {
                    string dateStr = Path.GetFileName(files[i]).Split('.')[0];
                    DateTime.TryParseExact(dateStr, "yyyyMMdd HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                    if (recentDate <= date)
                    {
                        index = i;
                        recentDate = date;
                    }
                }

                // Exception if Expired Token
                if (recentDate.AddDays(ExpiresDate) <= DateTime.Now)
                    throw new Exception("Expired Token");

                string token = File.ReadAllText(files[index]);

                return token;

            }
            catch (Exception ex)
            {
                NLogger.Get.Info(ex.Message);
                return null;
            }
            
        }
    }
}
