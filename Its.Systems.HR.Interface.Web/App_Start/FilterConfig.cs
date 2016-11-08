using System.Diagnostics;
using System.Web.Mvc;

namespace Its.Systems.HR.Interface.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
#if DEBUG
            // TODO: DOESNT WORK
            //filters.Add(new System.Web.Mvc.AllowAnonymousAttribute());
#else
            filters.Add(new AuthorizeAttribute());
#endif

            filters.Add(new HandleErrorAttribute());
        }
    }
}