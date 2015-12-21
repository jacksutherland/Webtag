using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webtag.DataAccess;
using Webtag.Models;

namespace Webtag.Controllers
{
    public class HomeController : BaseController
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            UserProfile admin = db.UserProfiles.FirstOrDefault();
            return View(admin);
        }

        public ActionResult About()
        {
            NavSelected = NavSection.About;
            return View();
        }
    }
}
