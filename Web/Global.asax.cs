using Autofac;
using Autofac.Integration.Mvc;
using AutofacFramework;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var _builder = AutofacService.RegisterInterface("AutofacFramework");

            _builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var _container = _builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
        }
    }
}
