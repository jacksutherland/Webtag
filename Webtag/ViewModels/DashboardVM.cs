using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Webtag.Models
{
    public class DashboardVM
    {
        public string Name { get; set; }
        public List<LinkVM> Links { get; set; }
        public SearchType SearchType { get; set; }
        public WeatherType WeatherType { get; set; }
        public bool ShowLinks { get; set; }
        public bool HasWidgets { get; set; }
    }

    public class LinkVM
    {
        public int LinkId { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public bool IsParent { get; set; }
        public List<LinkVM> ChildLinks { get; set; }
        public int Order { get; set; }
    }

    public class AddWidgetVM
    {
        public Dashboard Dashboard { get; set; }
        public bool ShowLinks { get; set; }
        public SearchType SearchType { get; set; }
        public List<SelectListItem> SearchTypes { get; set; }
        public WeatherType WeatherType { get; set; }
        public List<SelectListItem> WeatherTypes { get; set; }
        public WidgetType WidgetType { get; set; }
        public List<SelectListItem> WidgetTypes { get; set; }
        public List<Widget> Widgets { get; set; }
    }
}