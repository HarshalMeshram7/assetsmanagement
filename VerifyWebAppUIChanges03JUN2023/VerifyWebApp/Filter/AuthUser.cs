using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VerifyWebApp.Models;

namespace VerifyWebApp.Filter
{
    public class AuthUser : AuthorizeAttribute
    {
        public VerifyDB db = new VerifyDB();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IEnumerable<VerifyWebApp.Models.Login> filterEmployee;

            var rd = filterContext.HttpContext.Request.RequestContext.RouteData;

            string currentAction = rd.GetRequiredString("action").ToUpper(); ;
            string currentController = rd.GetRequiredString("controller").ToUpper();
            string currentArea = rd.Values["area"] as string;

            bool bPageAccess = false;
          
            int userid = Convert.ToInt32(filterContext.HttpContext.User.Identity.Name);
            var checkuser = db.Logins.Where(x => x.ID == userid).FirstOrDefault();
            if(checkuser.Userlevel=="Admin")
            {
                bPageAccess = true;
            }
            if (checkuser.Userlevel == "User")
            {
                if (filterContext.HttpContext.Request.IsAuthenticated == false)
                {
                    filterContext.Result = new RedirectToRouteResult(
                                                  new RouteValueDictionary {{ "Controller", "Login" },
                                                                        { "Action", "Login" } });
                }
                ////

                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    //The action filter logic
                    //filterEmployee = from staff in db.Logins
                    //                 where staff.UserName == filterContext.HttpContext.User.Identity.Name
                    //                 select staff;

                    //int staffno = (from de in filterEmployee
                    //               select de.ID).SingleOrDefault();

                    //////
                    int staffno = Convert.ToInt32(filterContext.HttpContext.User.Identity.Name);




                    // check access rights for page
                    if (currentController == "FARREPORT")
                    {
                        currentController = "REPORTS";
                    }
                    if(currentController=="EXPORT_GROUPS"|| currentController == "EXPORT_COSTCENTER"|| 
                        currentController == "EXPORTALLASSETS"|| currentController == "EXPORTLOCATIONS")
                    {
                        currentController = "EXPORTDATA";
                    }

                    BusinessLogic.AccessRightsHelper helper = new BusinessLogic.AccessRightsHelper();
                    int code = -1;
                    try
                    {
                        code = helper.controller_codemap[currentController.ToUpper()];
                    }catch(Exception ex)
                    {

                    }

                    

                    var objAccessRights = db.AccessRights.Where(e => e.Userid == staffno && e.ControllerName == code.ToString()).SingleOrDefault();

                    //var objAccessRights = db.tblAccessrights;

                    if (objAccessRights == null)
                    {
                        bPageAccess = false;

                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {

                            filterContext.Result = new RedirectToRouteResult(
                                                 new RouteValueDictionary {{ "Controller", "Home" },
                                                      { "Action", "NoRightsAjax" } });

                            //filterContext.HttpContext.Response.StatusCode = 403;
                            //filterContext.Result = new JsonResult
                            //{
                            //    Data = new
                            //    {
                            //        Error = "NotAuthorized",
                            //        LogOnUrl = urlHelper.Action("LogOn", "Account")
                            //    },
                            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            //};
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                                  new RouteValueDictionary {{ "Controller", "Home" },
                                                      { "Action", "NoRights" } });

                        }


                        //return RedirectToAction("whateverAction", "whateverController");
                    }
                    else
                    {
                        if (objAccessRights != null)
                        {
                            db.Entry(objAccessRights).Reload();
                        }

                        bPageAccess = helper.IsAllowed(objAccessRights, currentController, currentAction);

                        if (bPageAccess == false)
                        {
                            //ViewBag["ErrorMessage"] = "You do not have rights to access this page.";

                            // filterContext.Result = new RedirectResult("/Shared/NoRights.aspx");

                            if (filterContext.HttpContext.Request.IsAjaxRequest())
                            {

                                //filterContext.Result = new RedirectToRouteResult(
                                //                     new RouteValueDictionary {{ "Controller", "Home" },
                                //                      { "Action", "NoRightsAjax" } });


                                APIResponse response = new APIResponse();
                                response.status = "False";
                                response.data = "NotAuthorized";


                                filterContext.Result = new JsonResult
                                {

                                    Data = JsonConvert.SerializeObject(response),
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };

                            }
                            else
                            {

                                filterContext.Result = new RedirectToRouteResult(
                                      new RouteValueDictionary {{ "Controller", "Home" },
                                                                        { "Action", "NoRights" } });

                            }


                        }

                    }

                }

            }
        }
    }
}