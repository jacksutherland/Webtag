using System.Web.Mvc;
using System.Linq;
using Webtag.DataAccess;
using Webtag.Models;

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

            if(string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("ValidationSummary", "You must enter a valid email address and password");
            }

            if(ModelState.IsValid)
            {
                if (db.UserProfiles.Where(u => u.Email.ToLower() == model.Email.Trim().ToLower() && u.Password == model.Password.Trim()).Any())
                {
                    return RedirectToAction("/", "Home");
                }
                else
                {
                    ModelState.AddModelError("ValidationSummary", "That email address and password combination does not exist.");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            NavSelected = NavSection.Login;
            return View();
        }
    }
}
