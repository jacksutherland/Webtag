using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webtag.Models
{
    public class DashboardVM
    {
        public List<LinkVM> Links { get; set; }
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
}