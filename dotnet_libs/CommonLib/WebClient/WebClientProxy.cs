using CommonLib.Loggers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CommonLib.WebClient
{
    public struct WebResult
    {
        public readonly static WebResult InvalidResult = new WebResult
        {
            Body = "",
            StatusCode = HttpStatusCode.InternalServerError
        };

        public bool OK => StatusCode == HttpStatusCode.OK;
        public bool Failed => !OK;
        public bool Invalid => string.IsNullOrEmpty(Body) && StatusCode == HttpStatusCode.InternalServerError;
        public bool IsValid => !Invalid;

        public string Body;
        public HttpStatusCode StatusCode;
    }

    public class WebClientProxy
    {
        private static readonly HttpClient _client = new HttpClient();
        private static WebClientProxy _inst = null;

        public static WebClientProxy Get
        {
            get
            {
                if (null == _inst)
                {
                    _inst = new WebClientProxy();
                }
                return _inst;
            }
        }
        public static HttpClient Client => _client;


        //public async Task<WebResult> GetAsync(string url, CancellationToken tok)
        //{
        //    if (string.IsNullOrWhiteSpace(url))
        //        return WebResult.InvalidResult;

        //    try
        //    {
        //        HttpResponseMessage rsp = await _client.GetAsync(url, tok);
        //        rsp.EnsureSuccessStatusCode();

        //        WebResult res = new WebResult
        //        {
        //            StatusCode = rsp.StatusCode,
        //            Body = await rsp.Content.ReadAsStringAsync()
        //        };

        //        return res;
        //    }
        //    catch (Exception e)
        //    {
        //        NLogger.GetInstance.Error(e.Message);
        //    }

        //    return WebResult.InvalidResult;
        //}

        public static async Task<HttpResponseMessage> GetHTTP(string url, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            try
            {
                HttpResponseMessage msg = await _client.GetAsync(url, token);
                if (null == msg || !msg.IsSuccessStatusCode)
                    return null;

                return msg;
            }
            catch (Exception e)
            {
                NLogger.Get.Error($"failed. url: {url}, exp: {e.Message}");
            }
            return null;
        }

        public static async Task<T> GetAsync<T>(string url, CancellationToken token)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            try
            {
                HttpResponseMessage msg = await _client.GetAsync(url, token);
                if (null == msg || !msg.IsSuccessStatusCode)
                    return null;

                T data = JsonSerializer.Deserialize<T>(await msg.Content.ReadAsStringAsync());
                if (null == data)
                    return null;

                return data;
            }
            catch (Exception e)
            {
                NLogger.Get.Error($"failed. url: {url}, exp: {e.Message}");
            }
            return null;
        }

		public static async Task<XmlDocument> GetAsync(string url, CancellationToken token)
		{
			if (string.IsNullOrWhiteSpace(url))
				return null;

			try
			{
				HttpResponseMessage msg = await _client.GetAsync(url, token);
				if (null == msg || !msg.IsSuccessStatusCode)
					return null;

                if(msg.Content.Headers.ContentType.CharSet == "utf8")
                    msg.Content.Headers.ContentType.CharSet = "utf-8";  

				string xmlData = await msg.Content.ReadAsStringAsync();
				if (string.IsNullOrWhiteSpace(xmlData))
					return null;

				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xmlData);
				return doc;
			}
			catch (Exception e)
			{
				NLogger.Get.Error($"XmlDocument failed. url: {url}, exp: {e.Message}");
			}

			return null;
		}
	}
}
