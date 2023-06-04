using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Util;

namespace VerifyWebApp.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ValidateJsonXssAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            try
            {


                var request = filterContext.HttpContext?.Request;
                if (request != null && "application/json".Equals(request.ContentType, StringComparison.OrdinalIgnoreCase))
                {
                    if (request.ContentLength > 0 && request.Form.Count == 0) // 
                    {
                        if (request.InputStream.Position > 0)
                            request.InputStream.Position = 0; // InputStream has already been read once from "ProcessRequest"
                        using (var reader = new StreamReader(request.InputStream))
                        {
                            var postedContent = reader.ReadToEnd(); // Get posted JSON content
                            var isValid = RequestValidator.Current.InvokeIsValidRequestString(HttpContext.Current, postedContent,
                                RequestValidationSource.Form, "postedJson", out var failureIndex); // Invoke XSS validation
                            if (!isValid) // Not valid, so throw request validation exception
                                throw new HttpRequestValidationException("Potentially unsafe input detected");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // LOG Security Exception Here
                filterContext.Result = new RedirectToRouteResult(
                                          new RouteValueDictionary {{ "Controller", "Home" },
                                                                        { "Action", "Error" } });
            }
        }
    }
}