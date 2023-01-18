using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CommonLib.WebClient
{
    public class SimpleWebListener
    {
        private static readonly HttpListener _listener = new HttpListener();
        private static SimpleWebListener _inst = null;
        public static SimpleWebListener Get
        {
            get
            {
                if (null == _inst)
                {
                    _inst = new SimpleWebListener();
                }
                return _inst;
            }
        }

        private bool _isRun = false;
        private Dictionary<string, Func<string, string>> _webHandler = new Dictionary<string, Func<string, string>>();

        public void AddWebRequestHandler(string req, Func<string, string> func)
        {
            _webHandler.Add(req, func);
        }

        public async void RunListener(string listenUrl)
        {
            if (!HttpListener.IsSupported)
            {
                return;
            }

            _isRun = true;
            //_listener.Prefixes.Add("http://127.0.0.1:17777/");
            //_listener.Prefixes.Add("http://*:17777/");
            _listener.Prefixes.Add(listenUrl);
            _listener.Start();

            while (_isRun)
            {
                var ctx = await _listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse rsp = ctx.Response;

                ProcessWebRequest(req, rsp);
            }
        }

        public void StopListener()
        {
            _isRun = false;
            _listener.Stop();
        }

        private void ProcessWebRequest(HttpListenerRequest req, HttpListenerResponse rsp)
        {
            string[] seg = req.Url.Segments;
            string q = req.Url.Query;
            string method = req.HttpMethod;

            // http://{host}:{port}/{req}

            string rsp_data = "";
            do
            {
                Func<string, string> handler = null;
                if (!_webHandler.TryGetValue(seg[1], out handler))
                {
                    rsp.StatusCode = (int)HttpStatusCode.InternalServerError;
                    rsp_data = "cannot found handler";
                    break;
                }

                rsp.StatusCode = (int)HttpStatusCode.OK;
                rsp_data = handler.Invoke(q);

            } while (false);

            byte[] buffer = Encoding.UTF8.GetBytes(rsp_data);
            using (var outStream = rsp.OutputStream)
            {
                outStream.Write(buffer, 0, buffer.Length);
                outStream.Close();
            }
        }
    }
}
