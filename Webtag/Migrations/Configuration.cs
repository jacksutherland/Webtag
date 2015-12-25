namespace Webtag.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<Webtag.DataAccess.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            
        }

        protected override void Seed(Webtag.DataAccess.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            WebSecurity.InitializeDatabaseConnection("SimpleMembership", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            //context.UserProfiles.AddOrUpdate(up => up.Email,
            //new Models.UserProfile()
            //{
            //    Active = true,
            //    DateCreated = DateTime.UtcNow,
            //    Email = "jacksutherl@gmail.com",
            //    Username = "Admin",
            //    Password = "123"
            //});
            //context.SaveChanges();

            if (!WebSecurity.UserExists("jacksutherl"))
            {
                WebSecurity.CreateUserAndAccount("jacksutherl", "514178");
            }
        }
    }
}
