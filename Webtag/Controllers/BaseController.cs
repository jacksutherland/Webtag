using System.Web.Mvc;
using Webtag.Models;

namespace Webtag.Controllers
{
    public class BaseController : Controller
    {
        public NavSection NavSelected { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.NavSelected = NavSelected;
            base.OnActionExecuted(filterContext);
        }
    }
}
