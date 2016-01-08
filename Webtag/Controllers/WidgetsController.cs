using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webtag.DataAccess;
using Webtag.Models;

namespace Webtag.Controllers
{
    public class WidgetsController : BaseController
    {
        public ActionResult Add()
        {
            NavSelected = NavSection.AddWidget;

            Dashboard dashboard = GetOrCreateDashboard();
            IEnumerable<Widget> widgets = db.Widgets.Where(w => w.DashboardId == dashboard.Id);

            AddWidgetVM model = new AddWidgetVM()
            {
                Dashboard = dashboard,
                WidgetTypes = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Select a widget", Value = "" }
                },
                SearchTypes = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Google", Value = ((int)SearchType.Google).ToString() },
                    new SelectListItem() { Text = "Bing", Value = ((int)SearchType.Bing).ToString() },
                    new SelectListItem() { Text = "Yahoo", Value = ((int)SearchType.Yahoo).ToString() }
                },
                WeatherTypes = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "Willy Weather", Value = ((int)WeatherType.WillyWeather).ToString() },
                    new SelectListItem() { Text = "AccuWeather", Value = ((int)WeatherType.AccuWeather).ToString() }
                }
            };

            if (!widgets.Where(w => w.Type == WidgetType.Links).Any())
            {
                model.WidgetTypes.Add(new SelectListItem() { Text = "Links", Value = ((int)WidgetType.Links).ToString() });
            }

            if (!widgets.Where(w => w.Type == WidgetType.Search).Any())
            {
                model.WidgetTypes.Add(new SelectListItem() { Text = "Search engine", Value = ((int)WidgetType.Search).ToString() });
            }

            if (!widgets.Where(w => w.Type == WidgetType.Weather).Any())
            {
                model.WidgetTypes.Add(new SelectListItem() { Text = "Weather", Value = ((int)WidgetType.Weather).ToString() });
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(AddWidgetVM model)
        {
            bool updated = false;
            Dashboard dashboard = GetOrCreateDashboard();

            switch(model.WidgetType)
            {
                case WidgetType.Links:
                    if (!db.Widgets.Where(w => w.DashboardId == dashboard.Id && w.Type == WidgetType.Links).Any())
                    {
                        updated = true;
                        db.Widgets.Add(new Widget()
                        {
                            DashboardId = dashboard.Id,
                            Order = 1,
                            Type = WidgetType.Links
                        });
                    }
                    break;
                case WidgetType.Search:
                    if (!db.Widgets.Where(w => w.DashboardId == dashboard.Id && w.Type == WidgetType.Search).Any())
                    {
                        updated = true;
                        db.Widgets.Add(new Widget()
                        {
                            DashboardId = dashboard.Id,
                            Order = 1,
                            Type = WidgetType.Search,
                            ObjectId = (int)model.SearchType
                        });
                    }
                    break;
                case WidgetType.Weather:
                    if (!db.Widgets.Where(w => w.DashboardId == dashboard.Id && w.Type == WidgetType.Weather).Any())
                    {
                        updated = true;
                        db.Widgets.Add(new Widget()
                        {
                            DashboardId = dashboard.Id,
                            Order = 1,
                            Type = WidgetType.Weather,
                            ObjectId = (int)model.WeatherType
                        });
                    }
                    break;
            }

            if (updated)
            {
                db.SaveChanges();
            }

            return RedirectToAction("/", "Dashboard");
        }

        [HttpPost]
        public string Delete(WidgetType type)
        {
            bool deleted = false;
            Dashboard dashboard = GetOrCreateDashboard();
            foreach(Widget widget in db.Widgets.Where(w => w.DashboardId == dashboard.Id && w.Type == type))
            {
                deleted = true;
                db.Widgets.Remove(widget);
            }
            if (deleted)
            {
                db.SaveChanges();
            }
            return "success";
        }
    }
}
