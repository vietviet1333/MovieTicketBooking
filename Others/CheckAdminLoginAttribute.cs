using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MovieTicketBooking.Bcrypt
{
    public class CheckAdminLoginAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check if the session variable indicating user authentication is present
            if (HttpContext.Current.Session["LoggedInAdminID"] == null)
            {
               
                // If the session variable is not present, redirect to the login page
                filterContext.Result = new RedirectResult("/Admin/AdminLoginPage");
            }

            // Continue with the action execution
            base.OnActionExecuting(filterContext);
        }
    }
}