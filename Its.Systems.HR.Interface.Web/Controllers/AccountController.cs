using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties {RedirectUri = "/HrKompetensutveckling/Activity"},
                    WsFederationAuthenticationDefaults.AuthenticationType);
        }

        public void SignOut()
        {
            var callbackUrl = Url.Action("SignOutCallback", "Account", null, Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties {RedirectUri = callbackUrl},
                WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Activity");

            return View();
        }
    }
}