using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TaskFrameWork.Test
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public async Task TestGetAsync()
        {
            //{"weatherinfo":{"city":"西安","cityid":"101110101","temp":"20","WD":"西南风","WS":"1级","SD":"14%","WSE":"1","time":"17:00","isRadar":"1","Radar":"JC_RADAR_AZ9290_JB","njd":"暂无实况","qy":"970","rain":"0"}}
            string url = @"http://www.weather.com.cn/data/sk/101020100.html";
            var result = await TaskFactory.DownLoadStringWhithRetriesContinue(url);
            var response = JsonConvert.DeserializeObject<dynamic>(result);
            Trace.WriteLine(result);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task TestPostAsync()
        {
            //{"weatherinfo":{"city":"西安","cityid":"101110101","temp":"20","WD":"西南风","WS":"1级","SD":"14%","WSE":"1","time":"17:00","isRadar":"1","Radar":"JC_RADAR_AZ9290_JB","njd":"暂无实况","qy":"970","rain":"0"}}
            string url = @"http://api.huceo.com/meinv/";

            var parameters = new Dictionary<string, string>
            {
                { "key", "ecd3da75436c11b6a42958408e9cfa85" },
                { "num", "10" }
            };
            var result = await TaskFactory.DownLoadStringWhithRetries(url);
            Trace.WriteLine(result);
            Assert.IsNotNull(result);
        }
    }
}
