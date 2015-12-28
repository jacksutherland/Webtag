using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Webtag.Models;

namespace Webtag.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name=DataContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<LinkFolder> LinkFolders { get; set; }
    }
}