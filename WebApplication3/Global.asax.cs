using System.Data;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FastReport;
using InformixConnector;
using Newtonsoft.Json;

namespace WebApplication3
{
    public class MvcApplication : HttpApplication
    {
        public static SQLRepository repository;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Thread.Sleep(50000);
            repository = new SQLRepository(Settings.host, Settings.service, Settings.serverName, Settings.DBName, Settings.userName, Settings.password);
            // string json = repository.searchHuman("", "", "", "", "");
            // DataTable table = JsonConvert.DeserializeObject<DataTable>(json);
            // Report report = new Report();
            // report.RegisterData(table, "table");
        }
    }
}