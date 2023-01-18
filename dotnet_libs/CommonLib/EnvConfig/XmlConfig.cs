using CommonLib.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace CommonLib
{    
    public class XmlConfig
    {
        protected XmlDocument XmlDoc = null;

        public XmlConfig() { }

        public bool IsValid { get => null != XmlDoc; }

        public bool LoadXml<R>(string filePath) where R : XmlStringReader, new()
        {
            try
            {
                if (IsValid)
                    return true;

                R reader = new R();
                if (!reader.LoadTextData(filePath))
                    return false;

                XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(reader.GetXmlString());
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                return false;
            }

            return true;
        }

        public XmlNode QueryXmlData(string xmlPath)
        {
            try
            {
                do
                {
                    if (null == XmlDoc)
                        break;

                    return XmlDoc.SelectSingleNode(xmlPath);

                } while (false);
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                return null;
            }

            return null;
        }

        public XmlNodeList QueryXmlListData(string xmlPath)
        {
            try
            {
                do
                {
                    if (null == XmlDoc)
                        break;

                    return XmlDoc.SelectNodes(xmlPath);

                } while (false);
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                return null;
            }

            return null;
        }

        public bool GetServerBindInfo(out string ip, out int port)
        {
            ip = "";
            port = 0;

            XmlNode node = QueryXmlData("ServerConfig/BindInfo");
            if (null == node)
                return false;

            XmlAttributeCollection attrs = node.Attributes;
            ip = attrs["ip"].Value;
            port = int.Parse(attrs["port"].Value);
            return true;
        }

    }

    public class XmlStringReader
    {
        protected string TextData = null;

        public bool IsValid { get => !string.IsNullOrEmpty(TextData); }

        public virtual bool LoadTextData(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            try
            {
                TextData = File.ReadAllText(path);
                if (!IsValid)
                    return false;
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                return false;
            }

            return true;
        }
        public string GetXmlString()
        {
            return TextData;
        }
    }
}
