using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheFrameWork
{
    public interface IRedis
    {
        #region Key/Value 键值操作
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeout">过期时间，单位秒,-1：不过期，0：默认过期时间</param>
        /// <returns></returns>
        bool Set<T>(string key, T t, int timeout = 0);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        T Get<T>(string key);


        /// <summary>
        /// 摧毁缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);

        #endregion

        #region 链表操作
        /// <summary>
        /// IEnumerable泛型集合添加到链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="values"></param>
        /// <param name="timeout"></param>
        void AddList<T>(string listId, IEnumerable<T> values, int timeout = 0);


        /// <summary>
        /// 单个实体添加到链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="Item"></param>
        /// <param name="timeout"></param>
        void AddEntityToList<T>(string listId, T Item, int timeout = 0);

        /// <summary>
        /// 获取链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <returns></returns>
        IEnumerable<T> GetList<T>(string listId);

        /// <summary>
        /// 链表中摧毁单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="t"></param>
        void RemoveEntityFromList<T>(string listId, T t);


        /// <summary>
        /// 根据Lambada过滤摧毁实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listId"></param>
        /// <param name="func"></param>
        void RemoveEntityFromList<T>(string listId, Func<T, bool> func);
        #endregion
    }
}
