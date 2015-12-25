using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using Webtag.DataAccess;

namespace Webtag
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            WebSecurity.InitializeDatabaseConnection("SimpleMembership", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            //new DataInitializer().Initialize();
        }

    }

    //public class SimpleMembershipInitializer
    //{
    //    public SimpleMembershipInitializer()
    //    {
    //        using (var context = new DataContext())
    //            context.UserProfiles.Find(1);

    //        if (!WebSecurity.Initialized)
    //            WebSecurity.InitializeDatabaseConnection("SimpleMembership", "UserProfile", "UserId", "UserName", autoCreateTables: true);
    //    }
    //}
}