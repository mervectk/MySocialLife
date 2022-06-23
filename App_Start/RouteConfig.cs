using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MySocialLife
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Profile","Profile/{action}/{id}", new { controller = "Profile", action = "Index", id = UrlParameter.Optional });
            //optional->id route(rota-link)'da opsiyonel olarak gösterilecek parametre




            routes.MapRoute("Login", "Account/Login", new { controller = "Account", action = "Login" });

            routes.MapRoute("LoginPartial", "Account/LoginPartial", new { controller = "Account", action = "LoginPartial" });

            routes.MapRoute("Logout", "Account/Logout", new { controller = "Account", action = "Logout" });


            routes.MapRoute("Account", "{kullaniciadi}", new { controller = "Account", action = "KullaniciAdi" });

            routes.MapRoute("CreateAccount", "Account/CreateAccount", new { controller = "Account", action = "CreateAccount" });
            routes.MapRoute("Default", "", new { controller = "Account", action = "Index" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
