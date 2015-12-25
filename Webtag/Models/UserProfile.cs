using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Webtag.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [KeyAttribute]
        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}