﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ngo_One
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Admin",
            url: "Admin/{action}/{id}",
            defaults: new { controller = "Admin", action = "Sign_In", id = UrlParameter.Optional }
        );
            routes.MapRoute(
                name: "Default",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Payment",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Payment", action = "Payment", id = UrlParameter.Optional }
            );
        }
    }
}
