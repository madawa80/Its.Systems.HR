using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Its.Systems.HR.Interface.Web.App_Start;

namespace Its.Systems.HR.Interface.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityWebActivator.Start();
            //UnityConfig.RegisterTypes();
            
        }
    }
}
