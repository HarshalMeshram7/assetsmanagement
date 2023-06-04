using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class CompanyController : Controller
    {
        public VerifyDB db = new VerifyDB();

        // GET: Company
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

            if (user.Userlevel == "Admin")
            {
                List<Company> lstcompany = new List<Company>();
                lstcompany = db.Companys.ToList();
                return View(lstcompany);
            }
            else
            {
                 return RedirectToAction("Login", "Login");
            }
          //  return View();
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

            if(user.Userlevel=="Admin")
            {
                return PartialView();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            //return View();
            
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Add(Company company)
        {
            int userid = 0;
            JsonResult res = new JsonResult();
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
            if (user.Userlevel == "Admin")
            {
                //return PartialView();
                ///check license count of making company
                ///
              
                var companyobj = db.Companys.ToList();
                var companycount = companyobj.Count;
                var licenseobj = db.Licenses.FirstOrDefault();
                
                if(companycount >= licenseobj.Company_Creation_Count)
                {
                    res.Data = "cannotaddcompany";
                   return res; 
                }

              
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Companys.Add(company);
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
                else
                {
                    res.Data = "ERR";
                }

                return res;
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
            if (user.Userlevel == "Admin")
            {
                // return PartialView();
                Company company = new Company();
                try
                {
                    company = db.Companys.Where(x => x.ID == id).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    // logger.Log(LogLevel.Error, strError);
                    return PartialView();
                }
                return PartialView(company);

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

                
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Edit(Company company)
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
            if (user.Userlevel == "Admin")
            {
                //      return PartialView();
                JsonResult res;
                res = new JsonResult();
                try
                {

                    db.Entry(company).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    res.Data = "Success";
                    return res;

                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    // logger.Log(LogLevel.Error, strError);
                    res.Data = "Failed";
                    return res;
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
          
            //return View();
        }

        public ActionResult CompanySelection()
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

            List<Company> clist = new List<Company>();

            if (user.Userlevel == "Admin")
            {
                clist = db.Companys.ToList();
                ViewBag.userlevel = "Admin";
            }
            else
            {
                // var companypermision = db.CompanyPermissions.Where(x => x.UserId == userid).ToList();
                var getcompanyid = db.Logins.Where(x => x.ID == userid).FirstOrDefault().CompanyId;
                clist = db.Companys.Where(x => x.ID == getcompanyid).ToList();
                ViewBag.userlevel = "User";
            }

            return View(clist);
        }
        public ActionResult Selectcompany(int id)
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
            Company companyobj = db.Companys.Where(x => x.ID == id).FirstOrDefault();
           // Company companyid = (Company)(Session["CId"]);
            if (companyobj != null)
            {
                Session.Add("CId", companyobj);
            }
            else
            {
            }
            ViewBag.companyid = companyobj.CompanyName;
            return RedirectToAction("Index", "Home");

        }

    }
}