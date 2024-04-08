using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace MovieTicketBooking.Bcrypt
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            // Check if the session variable indicating user authentication is present
            if (HttpContext.Current.Session["LoggedInUserID"] == null )
            {
                // If the session variable is not present, redirect to the login page
                filterContext.Result = new RedirectResult("/Client/ViewLogin");
            }

            // Continue with the action execution
            base.OnActionExecuting(filterContext);
        }
    }
}