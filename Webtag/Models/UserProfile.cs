using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webtag.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }
    }
}