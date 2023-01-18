using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace CommonLib.Redis
{
    public interface IRedisCommand
    {
        Task<bool> DoRedisCommand(IDatabaseAsync redis);
    }

    // sample
    //class RedisCmd_AuthSession : IRedisCommand
    //{
    //    public async Task<bool> DoRedisCommand(IDatabaseAsync redis)
    //    {
    //        if (null == redis)
    //            return false;
    //        if (!InSession.IsValid)
    //            return false;

    //        string key = RedisProxy.Get.PrefixedKey(InSession.Value);
    //        OutValid = await redis.KeyExistsAsync(key);
    //        return true;
    //    }

    //    public SessionID InSession;

    //    public bool OutValid = false;
    //}
}
