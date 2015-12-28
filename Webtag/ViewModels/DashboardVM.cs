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
        public Link Link { get; set; }
        public LinkFolder LinkFolder { get; set; }
        public List<LinkVM> Links { get; set; }
        public int Order { get; set; }
    }
}