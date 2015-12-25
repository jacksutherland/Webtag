using System.ComponentModel.DataAnnotations.Schema;

namespace Webtag.Models
{
    public class Dashboard
    {
        public int Id { get; set; }

        //[ForeignKey("UserProfile")]
        //public int UserProfileId { get; set; }
        //public virtual UserProfile UserProfile { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }
    }
}