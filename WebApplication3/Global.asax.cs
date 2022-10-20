using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using InformixConnector;

namespace WebApplication3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static SQLRepository repository;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            repository = new SQLRepository(Settings.host, Settings.service, Settings.serverName, Settings.DBName, Settings.userName, Settings.password);
        }
    }
}