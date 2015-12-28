using System.Web;
using System.Web.Optimization;

namespace Webtag
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // *.min.* files will not load locally
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/js/webtag").Include(
                        "~/js/jquery-{version}.js",
                        "~/js/webtag.js"));

            bundles.Add(new StyleBundle("~/css/webtag").Include(
                        "~/css/bootstrap.css", // custom: only includes bootstrap grid and responsive utilities
                        "~/css/font-awesome.css",
                        "~/css/webtag.css"));
        }
    }
}