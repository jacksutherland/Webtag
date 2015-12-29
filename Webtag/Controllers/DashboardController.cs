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

        private List<LinkVM> GetLinksVM()
        {
            int userId = WebSecurity.CurrentUserId;
            IEnumerable<Link> links = db.Links.Where(l => l.UserProfileId == userId);
            List<LinkVM> linkVMs = links.Where(l => !l.LinkParentId.HasValue).Select(l => new LinkVM()
            {
                LinkId = l.Id,
                Title = l.Text,
                Href = l.Href,
                IsParent = l.IsParent,
                Order = l.Order
            }).ToList();

            foreach (LinkVM link in linkVMs.Where(lvm => lvm.IsParent))
            {
                link.ChildLinks = links.Where(l => l.LinkParentId == link.LinkId).OrderBy(l => l.Order).Select(l => new LinkVM()
                {
                    LinkId = l.Id,
                    Title = l.Text,
                    Href = l.Href,
                    IsParent = l.IsParent,
                    Order = l.Order
                }).ToList();
            }

            return linkVMs.OrderBy(l => l.Order).ToList();
        }

        private int GetNewLinkOrder()
        {
            IEnumerable<Link> links = db.Links.Where(l => l.UserProfileId == WebSecurity.CurrentUserId);
            return links.Any() ? links.Max(l => l.Order) + 1 : 0;
        }

        public ActionResult Index()
        {
            DashboardVM model = new DashboardVM()
            {
                Links = GetLinksVM()
            };

            return View(model);
        }

        public string SaveLink(string title, string href, int? id = null)
        {
            if(!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(href))
            {
                if (id.HasValue)
                {
                    Link link = db.Links.FirstOrDefault(l => l.Id == id);
                    if(link != null)
                    {
                        link.Text = title;
                        link.Href = href;
                    }
                }
                else
                {
                    db.Links.Add(new Link()
                    {
                        Order = GetNewLinkOrder(),
                        Href = href,
                        Text = title,
                        UserProfileId = WebSecurity.CurrentUserId,
                        IsParent = false
                    });
                }
                db.SaveChanges();
            }

            return SerializePartial("_Links", GetLinksVM());
        }

        public string SaveFolder(string name, int? id = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (id.HasValue)
                {
                    Link link = db.Links.FirstOrDefault(l => l.Id == id);
                    if (link != null)
                    {
                        link.Text = name;
                    }
                }
                else
                {
                    db.Links.Add(new Link()
                    {
                        Order = GetNewLinkOrder(),
                        Text = name,
                        UserProfileId = WebSecurity.CurrentUserId,
                        IsParent = true
                    });
                }
                db.SaveChanges();
            }

            return SerializePartial("_Links", GetLinksVM());
        }

        public string DeleteLink(int id, bool saveChanges = true)
        {
            Link link = db.Links.FirstOrDefault(l => l.Id == id);
            if(link != null)
            {
                db.Links.Remove(link);
                if (saveChanges)
                {
                    db.SaveChanges();
                }
            }

            return SerializePartial("_Links", GetLinksVM());
        }

        public string DeleteFolder(int id)
        {
            DeleteLink(id, false);

            foreach(Link link in db.Links.Where(l => l.LinkParentId == id))
            {
                if (link != null)
                {
                    db.Links.Remove(link);
                }
            }

            db.SaveChanges();

            return SerializePartial("_Links", GetLinksVM());
        }

        public void SortLinks(int parentId, string childIds)
        {
            int userId = WebSecurity.CurrentUserId;
            bool saveChanges = false;
            IEnumerable<Link> links = db.Links.Where(l => l.UserProfileId == userId);
            Link parentLink = links.FirstOrDefault(l => l.Id == parentId);

            string[] stringIds = childIds.Split('&');
            for (int i = 0; i < stringIds.Count(); i++)
            {
                int id;
                if (int.TryParse(stringIds[i].Substring(5), out id))
                {
                    Link link = links.FirstOrDefault(l => l.Id == id);
                    if (link != null && (parentLink == null || !link.IsParent))
                    {
                        link.Order = i;
                        link.LinkParentId = parentLink != null ? parentLink.Id : (int?)null;
                        saveChanges = true;
                    }
                }
            }

            if(saveChanges)
            {
                db.SaveChanges();
            }
        }

    }
}
