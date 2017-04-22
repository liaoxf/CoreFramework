using System;
using AutofacFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using Autofac;
using System.Linq;

namespace AutofacFramework.Test
{
    [TestClass]
    public class AutofacTest : TestBase
    {

        [TestMethod]
        public void IocTest()
        {
            //支持两种
            //反转

            IMenuRepository repostory = AutofacService.Resolve<IMenuRepository>();

            repostory.Add(new Menu { M_Name = "A" });

            repostory.Add(new Menu { M_Name = "B" });

            var result = repostory.AllItems.ToList();



            IAuthService authService = AutofacService.Resolve<IAuthService>();
            authService.Auth();

            //IMenuService service = AutofacService.Resolve<IMenuService>();

            var result2 = authService.Add(new AuthItem { });

            var result3 = authService.GetAuthItems();


            Assert.IsTrue(result.Count >= result2);
        }
    }
}
