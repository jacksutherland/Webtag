using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webtag.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is  required")]
        [EmailAddress(ErrorMessage="Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage="Password is  required")]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}