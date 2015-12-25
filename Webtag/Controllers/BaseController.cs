using System.Web.Mvc;
using WebMatrix.WebData;
using Webtag.Models;

namespace Webtag.Controllers
{
    public class BaseController : Controller
    {
        public NavSection NavSelected { get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.NavSelected = NavSelected;
            if(WebSecurity.IsAuthenticated)
            {
                ViewBag.UserName = WebSecurity.CurrentUserName;
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
