using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MovieTicketBooking
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            // Check if it's a 404 error
            if (exception is HttpException httpException && httpException.GetHttpCode() == 404)
            {
                // Redirect to custom 404 error page
                Response.Redirect("/Error/Error");
            }
            else
            {
                // Redirect to general error page
                Response.Redirect("/Error/Error");
            }
        }

    }
}
