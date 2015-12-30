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
                if (WebSecurity.Login(model.Email.Trim().ToLower(), model.Password.Trim()))
                {
                    return RedirectToAction("/", "Dashboard");
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
            return RedirectToAction("Login");
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

            if(model.Password.Count() < 6)
            {
                ModelState.AddModelError("Password", "Password must be at least 6 characters");
            }
            else if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Password and password confirmation do not match");
            }

            if(ModelState.IsValid)
            {
                // TODO: Make email confirmation work
                //string confirmationToken = WebSecurity.CreateUserAndAccount(model.Email, model.Password, new { UserName = model.Email }, true);
                //dynamic email = new Email("RegistrationConfirmation");
                //email.To = model.Email;
                //email.ConfirmationToken = confirmationToken;
                //email.Send();
                //return RedirectToAction("CheckEmail", new { email = model.Email });

                WebSecurity.CreateUserAndAccount(model.Email, model.Password);
                return RedirectToAction("Confirmation", new { id = "Qaq9oP5z94hArb0GRpbLBiCe0Acj1O06" });
            }

            return View(model);
        }

        public ActionResult CheckEmail(string email)
        {
            return View(email);
        }

        public ActionResult Confirmation(string id)
        {
            // first param is a key to bypass email
            if (id == "Qaq9oP5z94hArb0GRpbLBiCe0Acj1O06" || WebSecurity.ConfirmAccount(id))
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
