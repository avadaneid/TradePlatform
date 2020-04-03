using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TradingPlatform
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
              name: "SignUp",
              url: "{Home}/{SignUp}",
              defaults: new { controller = "Home", action = "SignUp", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "LogIn",
            url: "{Home}/{LogIn}",
            defaults: new { controller = "Home", action = "LogIn", id = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "SesionCompany",
            url: "{User}/{SesionCompany}",
            defaults: new { controller = "User", action = "SesionCompany" }
            );


            routes.MapRoute(
            name: "SesionIndividual",
            url: "{User}/{SesionIndividual}",
            defaults: new { controller = "User", action = "SesionIndividual" }
            );

             routes.MapRoute(
             name: "UploadFile",
             url: "{User}/{UploadCompanyDetails}",
             defaults: new { controller = "User", action = "UploadCompanyDetails" }
             );

            routes.MapRoute(
            name: "Transaction",
            url: "{User}/{Order}",
            defaults: new { controller = "User", action = "Order", id = UrlParameter.Optional }
            );

        }
    }
}
