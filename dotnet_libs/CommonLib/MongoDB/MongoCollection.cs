using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.MongoDB
{

    public abstract class MongoCollection
    {
        public MongoCollection(MongoDatabase db, string colName)
        {
            _database = db;
            _colName = colName;
        }

        // 다른 db에 접속이라면 새로운 인스턴스를 만들어주자
        private readonly MongoDatabase _database = null;
        protected string _colName;
        protected IMongoCollection<BsonDocument> _collection = null;

        protected MongoDatabase Database { get => _database; }
        //protected IMongoCollection<BsonDocument> Collection { get => _collection; }

        public async Task<IMongoCollection<BsonDocument>> MakeCollection()
        {
            if (null == _database || !_database.IsValid)
                return null;

            if (null == _collection)
            {
                bool onConf = await InitMongoCollection();
                if (!onConf) return null;
            }

            return _collection;
        }

        protected async Task<bool> InitMongoCollection()
        {
            if (null == _database || !_database.IsValid || !_database.IsInitialized)
                return false;

            var names = await _database.DB.ListCollectionNamesAsync();
            var list = names.ToList();
            if (list.Any(e => e == _colName))
                _collection = _database.DB.GetCollection<BsonDocument>(_colName);
            else
            {
                //var opt = new CreateCollectionOptions();
                //await Database.DB.CreateCollectionAsync(_colName, opt);
                //names = await _database.DB.ListCollectionNamesAsync();
                //list = names.ToList();
                //if (list.Any(e => e == _colName))
                //    _collection = _database.DB.GetCollection<BsonDocument>(_colName);
                //else
                //    return false;

                return await OnConfiguring();
            }

            return true;
        }

        protected abstract Task<bool> OnConfiguring();
    }
}
