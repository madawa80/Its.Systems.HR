using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Its.Systems.HR.Infrastructure;
using Its.Systems.HR.Interface.Web.App_Start;

namespace Its.Systems.HR.Interface.Web
{
    public class MvcApplication : HttpApplication
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

        void Application_PostAuthenticateRequest()
        {
            if (Request.IsAuthenticated)
            {
                var userIdentity = (ClaimsIdentity)User.Identity;
                var userClaims = userIdentity.Claims.ToArray();
                var upn = userClaims.SingleOrDefault(n => n.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").Value;
                var loggedInCasId = upn.Split('@')[0];

                // Always add the username
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, upn));
                
                // Now see if the logged in user is an HrPerson (Admin).
                IQueryable<string> adminCasIds;
                using (HRContext db = new HRContext())
                {
                    var isHrPerson = false;
                    adminCasIds = db.Participants.Where(n => n.IsHrPerson).Select(n => n.CasId);

                    foreach (var adminCasId in adminCasIds)
                    {
                        if (adminCasId == loggedInCasId)
                        {
                            userIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                            isHrPerson = true;
                            break;
                        }
                    }

                    if (!isHrPerson)
                        userIdentity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                }
            }
        }

    }
}