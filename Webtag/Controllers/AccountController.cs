using System.Web.Mvc;
using System.Linq;
using Webtag.DataAccess;
using Webtag.Models;
using WebMatrix.WebData;
using Postal;

namespace Webtag.Controllers
{
    public class AccountController : BaseController
    {
        private DataContext db = new DataContext();

        public ActionResult Login()
        {
            NavSelected = NavSection.Login;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            NavSelected = NavSection.Login;

            if (ModelState.IsValid)
            {
                if ( WebSecurity.Login(model.Email.Trim().ToLower(), model.Password.Trim()))
                {
                    return RedirectToAction("/", "Home");
                }
                ModelState.AddModelError("ValidationSummary", "That email address and password combination does not exist.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            return RedirectToAction("/", "Home");
        }

        public ActionResult Register()
        {
            NavSelected = NavSection.Login;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginVM model)
        {
            if(!string.IsNullOrEmpty(model.Password))
                model.Password = model.Password.Trim();
            if (!string.IsNullOrEmpty(model.ConfirmPassword))
                model.ConfirmPassword = model.ConfirmPassword.Trim();

            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and password confirmation do not match");
            }

            if(ModelState.IsValid)
            {
                string confirmationToken = WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { UserName = model.Email }, true);
                dynamic email = new Email("RegistrationConfirmation");
                email.To = model.Email;
                email.ConfirmationToken = confirmationToken;
                email.Send();
                return RedirectToAction("CheckEmail", new { email = model.Email });
            }

            return View(model);
        }

        public ActionResult CheckEmail(string email)
        {
            return View(email);
        }

        public ActionResult Confirmation(string id)
        {
            if (WebSecurity.ConfirmAccount(id))
            {
                ViewBag.Confirmed = true;
            }
            else
            {
                ViewBag.Confirmed = false;
            }

            return View();
        }
    }
}
