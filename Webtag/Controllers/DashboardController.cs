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

        private Dashboard GetOrCreateDashboard()
        {
            int userId = WebSecurity.CurrentUserId;

            Dashboard dashboard = db.Dashboards.FirstOrDefault(d => d.UserProfileId == userId);
            if(dashboard == null)
            {
                dashboard = db.Dashboards.Add(new Dashboard(userId));
                db.SaveChanges();
            }
            return dashboard;
        }

        public ActionResult Index()
        {
            NavSelected = NavSection.Dashboard;

            Dashboard dashboard = GetOrCreateDashboard();
            IEnumerable<Widget> widgets = db.Widgets.Where(w => w.DashboardId == dashboard.Id);
            Widget links = widgets.FirstOrDefault(w => w.Type == WidgetType.Links);
            Widget search = widgets.FirstOrDefault(w => w.Type == WidgetType.Search);
            Widget weather = widgets.FirstOrDefault(w => w.Type == WidgetType.Weather);

            DashboardVM model = new DashboardVM()
            {
                Name = dashboard.Name,
                Links = GetLinksVM(),
                ShowLinks = links != null,
                SearchType = search == null ? SearchType.None : (SearchType)search.ObjectId,
                WeatherType = weather == null ? WeatherType.None : (WeatherType)weather.ObjectId,
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

        public ActionResult Widgets()
        {
            NavSelected = NavSection.AddWidget;

            Dashboard dashboard = GetOrCreateDashboard();
            IEnumerable<Widget> widgets = db.Widgets.Where(w => w.DashboardId == dashboard.Id);
            
            SearchType searchType = SearchType.None;
            Widget search = widgets.FirstOrDefault(w => w.Type == WidgetType.Search);
            if (search != null)
            {
                searchType = (SearchType)search.ObjectId;
            }

            WeatherType weatherType = WeatherType.None;
            Widget weather = widgets.FirstOrDefault(w => w.Type == WidgetType.Weather);
            if (weather != null)
            {
                weatherType = (WeatherType)weather.ObjectId;
            }

            AddWidgetVM model = new AddWidgetVM()
            {
                Dashboard = dashboard,
                Widgets = widgets.ToList(),
                ShowLinks = widgets.Where(w => w.Type == WidgetType.Links).Any(),
                SearchType = searchType,
                SearchTypes = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "No Search", Value = ((int)SearchType.None).ToString(), Selected = searchType == SearchType.None },
                    new SelectListItem() { Text = "Google", Value = ((int)SearchType.Google).ToString(), Selected = searchType == SearchType.Google },
                    new SelectListItem() { Text = "Bing", Value = ((int)SearchType.Bing).ToString(), Selected = searchType == SearchType.Bing },
                    new SelectListItem() { Text = "Yahoo", Value = ((int)SearchType.Yahoo).ToString(), Selected = searchType == SearchType.Yahoo }
                },
                WeatherType = weatherType,
                WeatherTypes = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "No Weather", Value = ((int)WeatherType.None).ToString(), Selected = weatherType == WeatherType.None },
                    new SelectListItem() { Text = "AccuWeather", Value = ((int)WeatherType.AccuWeather).ToString(), Selected = weatherType == WeatherType.AccuWeather },
                    new SelectListItem() { Text = "Willy Weather", Value = ((int)WeatherType.WillyWeather).ToString(), Selected = weatherType == WeatherType.WillyWeather }
                }
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Widgets(AddWidgetVM model)
        {
            bool updated = false;
            Dashboard dashboard = GetOrCreateDashboard();
            Widget links = db.Widgets.FirstOrDefault(w => w.DashboardId == dashboard.Id && w.Type == WidgetType.Links);
            Widget search = db.Widgets.FirstOrDefault(w => w.DashboardId == dashboard.Id && w.Type == WidgetType.Search);
            Widget weather = db.Widgets.FirstOrDefault(w => w.DashboardId == dashboard.Id && w.Type == WidgetType.Weather);

            /*** Links ***/

            if(model.ShowLinks && links == null)
            {
                updated = true;
                db.Widgets.Add(new Widget()
                {
                    DashboardId = dashboard.Id,
                    Order = 1,
                    Type = WidgetType.Links
                });
            }
            else if(!model.ShowLinks && links != null)
            {
                updated = true;
                db.Widgets.Remove(links);
            }

            /*** Search ***/

            if(search == null)
            {
                updated = true;
                search = db.Widgets.Add(new Widget()
                {
                    DashboardId = dashboard.Id,
                    Order = 1,
                    Type = WidgetType.Search,
                    ObjectId = (int)model.SearchType
                });
            }
            else if(search.ObjectId != (int)model.SearchType)
            {
                updated = true;
                search.ObjectId = (int)model.SearchType;
            }

            /*** Weather ***/

            if (weather == null)
            {
                updated = true;
                weather = db.Widgets.Add(new Widget()
                {
                    DashboardId = dashboard.Id,
                    Order = 1,
                    Type = WidgetType.Weather,
                    ObjectId = (int)model.WeatherType
                });
            }
            else if (weather.ObjectId != (int)model.WeatherType)
            {
                updated = true;
                weather.ObjectId = (int)model.WeatherType;
            }

            if(updated)
            {
                db.SaveChanges();
            }


            return RedirectToAction("/");
        }
    }
}
