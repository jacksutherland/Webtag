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

            bundles.Add(new ScriptBundle("~/js").Include(
                        "~/js/jquery-{version}.js",
                        "~/js/webtag.js"));

            bundles.Add(new StyleBundle("~/css").Include(
                        "~/css/bootstrap.css", // custom: only includes bootstrap grid and responsive utilities
                        "~/css/webtag.css"));
        }
    }
}