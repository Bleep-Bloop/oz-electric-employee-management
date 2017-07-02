//using System.Web;
//using System.Web.Optimization;

//namespace OzElectric_EmployeeManagement
//{
//    public class BundleConfig
//    {
//        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
//        public static void RegisterBundles(BundleCollection bundles)
//        {
//            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
//                        "~/Scripts/jquery-{version}.js"));

//            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
//                        "~/Scripts/jquery.validate*"));

//            // Use the development version of Modernizr to develop with and learn from. Then, when you're
//            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
//            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
//                        "~/Scripts/modernizr-*"));

//            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
//                      "~/Scripts/bootstrap.js",
//                      "~/Scripts/respond.js"));

//            bundles.Add(new StyleBundle("~/CSS").Include(
//                      "~/Content/CSS/Bootstrap/bootstrap.min.css",
//                      "~/Content/CSS/Custom/site.css",
//                      "~/Content/font-awesome.min.css"));
//        }
//    }
//}

using System.Web;
using System.Web.Optimization;

namespace OzElectric_EmployeeManagement
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
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/template").Include(
                      "~/Content/Plugins/morris/morris.min.js",
                      "~/Content/Plugins/sparkline/jquery.sparkline.min.js",
                      "~/Content/Plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~/Content/Plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                      "~/Content/Plugins/knob/jquery.knob.js",
                      "~/Content/Plugins/daterangepicker/daterangepicker.js",
                      "~/Content/Plugins/datepicker/bootstrap-datepicker.js",
                      "~/Content/Plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                      "~/Content/Plugins/iCheck/icheck.min.js",
                      "~/Content/Plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/Content/Plugins/fastclick/fastclick.min.js",
                      "~/Content/Plugins/datatables/jquery.dataTables.js",
                      "~/Content/Plugins/datatables/dataTables.bootstrap.js",
                      "~/Scripts/app.min.js",
                      "~/Scripts/dashboard.js",
                      "~/Scripts/demo.js"));

            
              bundles.Add(new StyleBundle("~/Content/PublicCSS").Include( 
                        "~/Content/CSS/BootStrap/bootstrap.min.css",
                        "~/Content/CSS/Custom/AdminLTE.min.css",
                        "~/Content/CSS/Custom/skin-blue.min.css",
                        "~/Content/Plugins/iCheck/flat/blue.css",
                        "~/Content/Plugins/morris/morris.css",
                        "~/Content/Plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                        "~/Content/Plugins/datepicker/datepicker3.css",
                        "~/Content/Plugins/daterangepicker/daterangepicker-bs3.css",
                        "~/Content/Plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css",
                        "~/Content/Plugins/ionicons2/ionicons2.min.css",
                        "~/Content/Plugins/datatables/dataTables.bootstrap.css",
                        "~/Content/Plugins/fontawesome/font-awesome.min.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/css/iconssss").Include("~/Content/Plugins/ionicons2/ionicons2.min.css", new CssRewriteUrlTransform()));

            

        }
    }
}

