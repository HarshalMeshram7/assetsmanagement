using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace VerifyWebApp.Filter
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,AllowMultiple =false)]
    public class ValidateJsonAntiForgeryTokenAttribute : System.Web.Mvc.ActionFilterAttribute
    {

        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext actionContext)
        {
        
            if (actionContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            string token = "";
         
            var headers = actionContext.RequestContext.HttpContext.Request.Headers;
            var cookies = actionContext.RequestContext.HttpContext.Request.Cookies;

           

            if (!String.IsNullOrEmpty(headers["__RequestVerificationToken"]))
            {

                token = headers["__RequestVerificationToken"];
            }

            HttpCookie cookie_csrf = null;
            cookie_csrf = actionContext.RequestContext.HttpContext.Request.Cookies[AntiForgeryConfig.CookieName] as HttpCookie;
            string str_value = ""; 
            if (cookie_csrf != null)
            {
                str_value = cookie_csrf.Value;
            }


            try
            {
                AntiForgery.Validate(str_value, token);
            }
            catch(Exception ex)
            {
                int i = 0;

                
                actionContext.Result = new RedirectToRouteResult(
                                          new RouteValueDictionary {{ "Controller", "Home" },
                                                                        { "Action", "Error" } });
                // Log Exception here 
            }


        }
      
    }

 


}