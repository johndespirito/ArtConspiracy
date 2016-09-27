using ArtConspiracy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ArtConspiracy.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private Models.ArtConspiracyEntities db = new Models.ArtConspiracyEntities();

        public AccountController()
        : this (Startup.UserManagerFactory.Invoke())
        {
        }

        public AccountController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Account

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userManager.FindAsync(model.Email, model.Password);

            if (user != null)
            {
                var identity = await userManager.CreateIdentityAsync(
                    user, DefaultAuthenticationTypes.ApplicationCookie);

                GetAuthenticationManager().SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            RegisterModel defaultDate = new RegisterModel();
            defaultDate.JoinDate = DateTime.Now;
            return View(defaultDate);
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new AppUser
            {
                UserName = model.Email,
                city = model.City,
                state = model.State,
                fullName = model.FullName,
                joinDate = DateTime.Now
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignIn(user);
                return RedirectToAction("index", "account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        private async Task SignIn(AppUser user)
        {
            var identity = await userManager.CreateIdentityAsync(
                user, DefaultAuthenticationTypes.ApplicationCookie);
            
            identity.AddClaim(new Claim("FullName", user.fullName));
            identity.AddClaim(new Claim("City", user.city));
            identity.AddClaim(new Claim("State", user.state));

            GetAuthenticationManager().SignIn(identity);
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditProfile()
        {
            var id = User.Identity.GetUserId();
            var profileData = db.AspNetUsers.Find(id);
            return View(profileData);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(AspNetUser userprofile)
        {

            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                // Get the userprofile
                AspNetUser user = db.AspNetUsers.FirstOrDefault(u => u.UserName.Equals(username));

                // Update fields
                user.fullName = userprofile.fullName;
                user.city = userprofile.city;
                user.state = userprofile.state;

                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index", "Account"); 
            }

            return RedirectToAction("EditProfile", "Account");

        }

    }
}