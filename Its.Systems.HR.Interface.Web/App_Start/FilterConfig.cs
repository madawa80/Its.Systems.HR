using System.Diagnostics;
using System.Web.Mvc;

namespace Its.Systems.HR.Interface.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            #if Debug
            
            #else
                //TODO: build a global authorize filer
            #endif
            filters.Add(new HandleErrorAttribute());
        }
    }
}