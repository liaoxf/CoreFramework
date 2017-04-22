using System;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Linq;

namespace CacheFrameWork
{
    public class RedisHelper : IDisposable
    {
        public string Host
        {
            get { return "localhost"; }
        }

        public int Port
        {
            get { return 6379; }
        }

        public string Pwd
        {
            get
            {
                return "123456";
            }
        }

        public int DefaultDb
        {
            get;
            set;
        }

        public string RedisEndPoints
        {
            //IP地址中可以加入auth验证   password@ip:port
            get
            {
                return string.Format("{0}@{1}:{2}", Pwd, Host, Port);
            }
        }

        //初始化Redis,地址跟端口从配置文件中读取
        private RedisClient redis;
        public RedisClient Redis
        {
            get
            {
                return redis ?? (redis = new RedisClient(Host, Port));
            }
            set { redis = value; }
        }

        //初始化缓冲池
        PooledRedisClientManager prcm = null;
        //设置默认缓存过期时间
        public int secondsTimeOut = 30 * 60;

        public static readonly RedisHelper Instance = new RedisHelper();

        public void Install()
        {
            prcm = CreatManager(new string[] { RedisEndPoints }, new string[] { RedisEndPoints }, DefaultDb);
            Redis = prcm.GetClient() as RedisClient;
        }
        /// <summary>
        /// 缓冲池
        /// </summary>
        /// <param name="readWriteHosts"></param>
        /// <param name="readOnlyHosts"></param>
        /// <returns></returns>
        private static PooledRedisClientManager CreatManager(string[] readWriteHosts, string[] readOnlyHosts, int initialDB = 0)
        {
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = readWriteHosts.Length * 5,
                MaxReadPoolSize = readOnlyHosts.Length * 5,
                AutoStart = true,
            }, initialDB, 50, 5);
        }

        public void Dispose()
        {
            if (Redis != null)
            {
                Redis.Dispose();
                Redis = null;
            }
            GC.Collect();
        }

        public bool Set<T>(string key, T t, int timeout = 0)
        {
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                //设置到期时间
                Redis.Expire(key, secondsTimeOut);
            }

            if (Get<T>(key) != null)
            {
                Remove(key);
            }
            return Redis.Add(key, t);
        }

        public T Get<T>(string key)
        {
            return Redis.Get<T>(key);
        }

        public bool Remove(string key)
        {
            return Redis.Remove(key);
        }

        public void AddList<T>(string listId, IEnumerable<T> values, int timeout = 0)
        {
            redis.Expire(listId, 60);
            IRedisTypedClient<T> iRedisClient = Redis.As<T>();
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                //设置到期时间
                Redis.Expire(listId, secondsTimeOut);
            }
            var list = iRedisClient.Lists[listId];
            list.AddRange(values);
            iRedisClient.Save();
        }

        public void AddEntityToList<T>(string listId, T Item, int timeout = 0)
        {
            IRedisTypedClient<T> iRedisClient = Redis.As<T>();
            if (timeout >= 0)
            {
                if (timeout > 0)
                {
                    secondsTimeOut = timeout;
                }
                Redis.Expire(listId, secondsTimeOut);
            }
            var redisList = iRedisClient.Lists[listId];
            redisList.Add(Item);
            iRedisClient.Save();
        }

        public IEnumerable<T> GetList<T>(string listId)
        {
            IRedisTypedClient<T> iRedisClient = Redis.As<T>();
            return iRedisClient.Lists[listId];
        }

        public void RemoveEntityFromList<T>(string listId)
        {
            IRedisTypedClient<T> iRedisClient = Redis.As<T>();
            var redisList = iRedisClient.Lists[listId];
            redisList.RemoveAll();
            iRedisClient.Save();
        }

        public void RemoveEntityFromList<T>(string listId, Func<T, bool> func)
        {
            IRedisTypedClient<T> iRedisClient = Redis.As<T>();
            {
                var redisList = iRedisClient.Lists[listId];

                List<T> delList = redisList.Where(func).ToList();
               
                delList.ForEach(v => redisList.RemoveValue(v));

                iRedisClient.Save();
            }
        }
    }
}
