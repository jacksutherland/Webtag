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

        protected string SerializePartial(string partial, object model)
        {
            ViewData.Model = model;

            string html;
            System.IO.StringWriter sw = new System.IO.StringWriter();
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, partial);
            ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
            viewResult.View.Render(viewContext, sw);

            html = sw.GetStringBuilder().ToString();

            return html;
        }

    }
}
