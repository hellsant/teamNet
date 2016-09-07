using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MVCWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MVCWebsite.Controllers
{
    public class AuthenticationController : Controller
    {
        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ActionResult Show()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel input)
        {
            if (ModelState.IsValid)
            {
                if (input.UsernameAndPasswordValidation)
                {
                    var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, input.Username),
                        },
                        DefaultAuthenticationTypes.ApplicationCookie,
                        ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));

                    Authentication.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = input.RememberCondition
                    }, identity);

                    return RedirectToAction("index", "home");
                }
            }

            return View("show", input);
        }

        public ActionResult Logout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("login");
        }
    }
}