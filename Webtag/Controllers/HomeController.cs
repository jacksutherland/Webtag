using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Webtag.DataAccess;
using Webtag.Models;

namespace Webtag.Controllers
{
    public class HomeController : BaseController
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            if(WebSecurity.IsAuthenticated)
            {
                return RedirectToAction("/", "Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            NavSelected = NavSection.About;
            return View();
        }
    }
}
