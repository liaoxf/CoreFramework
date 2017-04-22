using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TaskFrameWork
{
    public class TaskFactory
    {
        /// <summary>
        /// 第一次1s,一次累加，直到请求完成
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DownLoadStringWhithRetriesContinue(string url)
        {
            using (var client = new HttpClient())
            {
                var nextDelay = TimeSpan.FromSeconds(1);
                for (int i = 0; i != 3; i++)
                {
                    try
                    {
                        return await client.GetStringAsync(url);
                    }
                    catch
                    {
                    }

                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }
                return await client.GetStringAsync(url);
            }
        }

        public static async Task<string> DownLoadStringWhithRetries(string url)
        {
            using (var client = new HttpClient())
            {
                var downloadTask = client.GetStringAsync(url);
                var timeoutTask = Task.Delay(3000);
                var completedTask = await Task.WhenAny(downloadTask, timeoutTask);
                if (completedTask == timeoutTask)
                    return null;
                return await downloadTask;
            }
        }

    }

    public enum REQUESTTYPE {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE,
        COPY,
        HEAD,
        OPTIONS,
        LINK,
        UNLINK,
        PURGE
    }
}
