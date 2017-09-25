using System.Web;
using System.Web.Optimization;

namespace Green
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                        "~/Scripts/agency.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                     "~/Scripts/knockout-3.4.2.js",
                     "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/viewModels").Include(
                      "~/Scripts/ViewModels/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/overlay").Include(
                      "~/Scripts/overlay.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                      "~/Scripts/moment-with-locales.js",
                      "~/Scripts/moment-with-locales.min.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/moment.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/rateit").Include(
                      "~/Scripts/jquery.rateit.js",
                      "~/Scripts/jquery.rateit.min.js",
                      "~/Scripts/jquery.rateit.min.js.map"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/charts").Include(
                      "~/Scripts/Chart.bundle.js",
                      "~/Scripts/Chart.bundle.min.js",
                      "~/Scripts/Chart.js",
                      "~/Scripts/Chart.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                      "~/Scripts/jquery.signalR-2.2.2.js",
                      "~/Scripts/jquery.signalR-2.2.2.min.js",
                      "~/signalr/hubs"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-white.css",
                      "~/Content/bootstrap-social.css",
                       "~/Content/agency.css",
                      "~/Content/site.css",
                      "~/Content/rateit.css",
                      "~/Content/stilizations.css",
                      "~/Content/delete.gif",
                      "~/Content/star.gif"
                      ));
        }
    }
}
