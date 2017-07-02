using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OzElectric_EmployeeManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //Code that runs on applciation startup
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            /*
                System.Data.Entity.Database.SetInitializer(new BlackMarketAuctionPart3.Models.AuctionSampleData());
            */
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
