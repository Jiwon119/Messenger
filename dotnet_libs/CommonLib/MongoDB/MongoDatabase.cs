using CommonLib.Loggers;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CommonLib.MongoDB
{
    public class MongoDatabase
    {
        public MongoDatabase(string alias, string connStr, string dBName)
        {
            Alias = alias;
            ConnectionStr = connStr;
            DBName = dBName;
        }

        public string Alias { get; private set; }
        public string ConnectionStr { get; private set; }
        public string DBName { get; private set; }

        internal MongoClient Client { get; private set; } = null;
        public IMongoDatabase DB { get; private set; } = null;
        //internal IMongoCollection<BsonDocument> Docs { get; private set; }

        public bool IsValid { get => null != Client && null != DB; }
        public bool IsInitialized { get; private set; } = false;

        public async Task<bool> ConnectAsync()
        {
            if (IsInitialized)
            {
                NLogger.Get.Log(NLog.LogLevel.Warn, "already connected");
                return true;
            }

            if (string.IsNullOrEmpty(ConnectionStr))
                return false;

            var setting = MongoClientSettings.FromConnectionString(ConnectionStr);
            Client = new MongoClient(setting); // lazy init

            if (null == Client)
            {
                return false;
            }

            try
            {
                // connection test
                var dbNames = await Client.ListDatabaseNamesAsync();
                var list = dbNames.ToList();
                if (list.Any(e => e == DBName))
                {
                    DB = Client.GetDatabase(DBName);
                }
                //else
                //{
                //    DB = Client.
                //}
            }
            catch (Exception e)
            {
                NLogger.Get.Log(NLog.LogLevel.Error, e.Message);
                Client = null;
                DB = null;
                return false;
            }

            DB = Client.GetDatabase(DBName); // lazy init
            if (null == DB)
            {
                return false;
            }

            IsInitialized = true;
            return true;
        }
    }

}
