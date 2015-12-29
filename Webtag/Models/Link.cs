using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Webtag.Models
{
    public class Link
    {
        public int Id { get; set; }

        public int UserProfileId { get; set; }

        public string Text { get; set; }

        public string Href { get; set; }

        public int Order { get; set; }

        public bool IsParent { get; set; }

        public int? LinkParentId { get; set; }
    }
}