using System.Web.Mvc;
using System.Web.Routing;

namespace Its.Systems.HR.Interface.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Activity", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}