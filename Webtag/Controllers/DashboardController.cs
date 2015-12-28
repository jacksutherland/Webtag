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
    [Authorize]
    public class DashboardController : BaseController
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            int userId = WebSecurity.CurrentUserId;
            IEnumerable<Link> links = db.Links.Where(l => l.UserProfileId == userId);
            IEnumerable<LinkFolder> folders = db.LinkFolders.Where(f => f.UserProfileId == userId);
            List<LinkVM> linkVMs = folders.Select(f => new LinkVM()
            {
                LinkFolder = f,
                Order = f.Order,
                Links = links.Where(l => l.LinkFolderId == f.Id).OrderBy(l => l.Order).Select(l => new LinkVM()
                {
                    Link = l
                }).ToList()
            }).ToList();
            
            foreach (Link link in links.Where(l => !l.LinkFolderId.HasValue))
            {
                linkVMs.Add(new LinkVM()
                {
                    Link = link,
                    Order = link.Order
                });
            }

            DashboardVM model = new DashboardVM()
            {
                Links = linkVMs.OrderBy(l => l.Order).ToList()
            };

            return View(model);
        }

    }
}
