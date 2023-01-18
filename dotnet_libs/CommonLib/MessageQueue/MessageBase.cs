using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using CommonLib.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonLib.MessageQueue
{
    public class SFM_Message
    {
        internal SFM_Message()
        {
            _jsonBody = null;
        }
        public SFM_Message(OpCode opCode, string sender)
        {
            _jsonBody = new JObject();
            Debug.Assert(null != _jsonBody, "SFM_Message - body is null");

            _jsonBody.Add("opCode", (int)opCode);
            _jsonBody.Add("sender", sender);
        }

        private JObject _jsonBody = null;

        public JObject MessageBody => _jsonBody;

        public JToken GetValue(string key)
        {
            if (string.IsNullOrEmpty(key) ||
                null == _jsonBody)
                return null;

            return _jsonBody.GetValue(key);
        }

        public OpCode OperationCode
        {
            get
            {
                int val =(int)_jsonBody.GetValue("opCode");
                return (OpCode)val;
            }
        }
        public string MessageSender => (string)_jsonBody.GetValue("sender");
        public bool IsValid => null != _jsonBody;

        internal string SerializeMessage
        {
            get
            {
                Debug.Assert(null != _jsonBody);
                return _jsonBody.ToString(Formatting.None);
            }
            set
            {
                //_jsonBody = new JObject(value);
                _jsonBody = JObject.Parse(value);
                Debug.Assert(null != _jsonBody);
            }
        }
    }

}
