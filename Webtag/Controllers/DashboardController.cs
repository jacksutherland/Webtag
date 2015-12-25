﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webtag.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
