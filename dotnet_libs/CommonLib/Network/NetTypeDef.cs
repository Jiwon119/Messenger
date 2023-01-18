using System;
using System.Text.Json;
using CommonLib.Loggers;
using CommonLib.Network.IOPacket;
using System.Windows;

namespace CommonLib.Network
{
    public struct ConnectionID
    {
        public ConnectionID(long val) { _value = val; }
        private long _value;
        public long Value { get { return _value; } }
        public bool IsValid { get { return 0 != _value; } }
        public static readonly ConnectionID Nil = new ConnectionID(0);
        public override bool Equals(object obj)
        {
            if (!(obj is ConnectionID))
            {
                return false;
            }

            var iD = (ConnectionID)obj;
            return _value == iD._value;
        }

        public override int GetHashCode()
        {
            return -1939223833 + _value.GetHashCode();
        }

        public static bool operator ==(ConnectionID iD1, ConnectionID iD2)
        {
            return iD1.Equals(iD2);
        }

        public static bool operator !=(ConnectionID iD1, ConnectionID iD2)
        {
            return !(iD1 == iD2);
        }
                
        public void WritePacket(OutPacket oPacket)
        {
            // connection id 은 외부로 노출하지 않는다.
            throw new NotImplementedException();
        }

        public void ReadPacket(InPacket iPacket)
        {
            // connection id 은 외부로 노출하지 않는다.
            throw new NotImplementedException();
        }
    }

    public class ErrorMessage
    {
        public ErrorMessage(int type, string msg)
        {
            ErrorType = type;
            ErrorMsg = msg;
        }
        public ErrorMessage(string json)
        {
            try
            {
                var em = JsonSerializer.Deserialize<ErrorMessage>(json);
                if (null == em)
                {
                    NLogger.Get.Log(NLog.LogLevel.Error, "Parse Failed.");
                    ErrorType = -1;
                    return;
                }

                ErrorType = em.ErrorType;
                ErrorMsg = em.ErrorMsg;
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, "Error Catching Failed : " + e);
            }
        }
        public int ErrorType { get; private set; }
        public string ErrorMsg { get; private set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public bool FromJson(string json)
        {
            var em = JsonSerializer.Deserialize<ErrorMessage>(json);
            if (null == em)
                return false;

            ErrorType = em.ErrorType;
            ErrorMsg = em.ErrorMsg;
            return true;
        }
    }
}