using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SearchHuman",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "SearchHuman", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AddHuman",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "AddHuman", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DelHuman",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "DelHuman", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EditHuman",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "EditHuman", id = UrlParameter.Optional }
            );
        }
    }
}