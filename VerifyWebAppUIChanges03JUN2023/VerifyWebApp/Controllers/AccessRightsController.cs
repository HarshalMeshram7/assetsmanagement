using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.BusinessLogic;
// comment to create branch
namespace VerifyWebApp.Controllers
{
    public class AccessRightsController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: AccessRights
        public ActionResult Index()
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            if (user.Userlevel == "Admin")
            {
                AccessRightsHelper accessRightsHelper = new AccessRightsHelper();

                List<AccessRights> lstar = new List<AccessRights>();
                lstar = db.AccessRights.ToList();

                foreach (AccessRights item in lstar)
                {
                    item.username = db.Logins.Where(x => x.ID == item.Userid).FirstOrDefault().UserName;
                    item.ControllerName = accessRightsHelper.controller_codemap_reverse[Convert.ToInt32(item.ControllerName)].ToString();


                }
                return View(lstar);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            
        }
        [HttpGet]
        public ActionResult Add()
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            if (user.Userlevel == "Admin")
            {
                //BusinessLogic.GetControllerNames getControllerNames = new BusinessLogic.GetControllerNames();
                VerifyWebApp.BusinessLogic.AccessRightsHelper helper = new BusinessLogic.AccessRightsHelper();

                var cname = helper.getcontrollernames();
                ViewBag.controllername=new SelectList(cname);
                ViewBag.Username = new SelectList(db.Logins.Where( x=>x.CompanyId == companyid).OrderBy(e => e.ID), "ID", "UserName");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Add(AccessRights accessrights)
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            JsonResult res;
            res = new JsonResult();
            if (user.Userlevel == "Admin")
            {
                //return PartialView();
                ///check license count of making company
                ///



                // Mandar 15mar2022

                AccessRightsHelper accessRightsHelper = new AccessRightsHelper();

                int controller_code = -1;
                controller_code = accessRightsHelper.controller_codemap[accessrights.ControllerName.ToUpper()];



                var check_controllerentryexists = db.AccessRights.Where(x => x.Companyid == companyid 
                && x.Userid==accessrights.Userid && x.ControllerName == controller_code.ToString()).FirstOrDefault();



                if (check_controllerentryexists != null)
                {
                    res.Data = "accessalreadygiven";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {

                        TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                        var tnow = System.DateTime.Now.ToUniversalTime();
                        DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                        AccessRights obj = new AccessRights();
                        obj.CreatedDate = istDate;
                        obj.CreatedUserid = userid;
                        obj.Companyid = companyid;
                        obj.Userid = accessrights.Userid;


                        VerifyWebApp.BusinessLogic.AccessRightsHelper helper = new BusinessLogic.AccessRightsHelper();
                        //int code = helper.getcontrollernames();


                        //names.getcontrollernames
                        obj.ControllerName = controller_code.ToString();


                        obj.Add = accessrights.Add;
                        obj.Edit = accessrights.Edit;
                        obj.Delete = accessrights.Delete;
                        obj.Export = accessrights.Export;
                        obj.Import = accessrights.Import;
                        obj.Index = accessrights.Index;
                       
                        db.AccessRights.Add(obj);
                        db.SaveChanges();

                        res.Data = "Success";


                    }
                    catch (Exception ex)
                    {
                        string strError;
                        strError = ex.Message + "|" + ex.InnerException;
                        //  logger.Log(LogLevel.Error, strError);
                        res.Data = "Failed";

                    }


                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            if (user.Userlevel == "Admin")
            {
                //BusinessLogic.GetControllerNames getControllerNames = new BusinessLogic.GetControllerNames();
                BusinessLogic.AccessRightsHelper helper = new BusinessLogic.AccessRightsHelper();
                
                
                var cname = helper.getcontrollernames();
                ViewBag.controllername = new SelectList(cname);
                ViewBag.Username = new SelectList(db.Logins.Where(x => x.CompanyId == companyid).OrderBy(e => e.ID), "ID", "UserName");
                AccessRights access = new AccessRights();
                access = db.AccessRights.Where(x => x.Companyid == companyid && x.Id == id).FirstOrDefault();
                Login login = db.Logins.Where(x => x.ID == access.Userid).FirstOrDefault();
                access.username = login.FullName;


                String _tempPageName = helper.controller_codemap_reverse[Convert.ToInt32(access.ControllerName)].ToString();

                ViewBag.pagename = _tempPageName;

                return View(access);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }



        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Edit(AccessRights accessrights)
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            if (user.Userlevel == "Admin")
            {
                //return PartialView();
                ///check license count of making company
                ///
                string res = "";




                try
                {

                    AccessRightsHelper accessRightsHelper = new AccessRightsHelper();

                    int controller_code = -1;
                    //controller_code = accessRightsHelper.controller_codemap[accessrights.ControllerName.ToUpper()];


                    

                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                    var tnow = System.DateTime.Now.ToUniversalTime();
                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                    AccessRights obj = new AccessRights();
                    obj = db.AccessRights.Where(x => x.Companyid == companyid && x.Id == accessrights.Id).FirstOrDefault();
                    obj.ModifiedDate = istDate;
                    obj.ModifiedUserid = userid;
                    obj.Userid = accessrights.Userid;



                    obj.ControllerName = accessrights.ControllerName;// controller_code.ToString(); //accessrights.ControllerName;
                    obj.Add = accessrights.Add;
                    obj.Edit = accessrights.Edit;
                    obj.Delete = accessrights.Delete;
                    obj.Index = accessrights.Index;
                    obj.Export = accessrights.Export;
                    obj.Import = accessrights.Import;
                    
                    obj.Companyid = companyid;
                  
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    res = "Success";


                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    //  logger.Log(LogLevel.Error, strError);
                    res = "Failed";

                }



                return Content(res);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

    }
    }
