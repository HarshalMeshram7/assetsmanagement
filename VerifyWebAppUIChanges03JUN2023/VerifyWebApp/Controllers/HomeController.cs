using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class HomeController : Controller
    {
        public VerifyDB db = new VerifyDB();
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
               // return RedirectToAction("Login", "Login");
            }
           // Company companyobj = db.Companys.Where(x => x.ID == id).FirstOrDefault();
            Company companyid = (Company)(Session["CId"]);
            if (companyid == null)
            {
                Session.Add("CId", companyid);

            }
            else
            {
            }
            if(companyid==null )
            {
                ViewBag.companyid = null;
                ViewBag.LoggedCompany = null;
            }
            else
            {
                ViewBag.companyid = companyid;
                ViewBag.LoggedCompany = companyid.CompanyName;
                ViewBag.CompanyName = companyid.CompanyName;
            }
           
         
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult NoRights()
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
                // return RedirectToAction("Login", "Login");
            }
            // Company companyobj = db.Companys.Where(x => x.ID == id).FirstOrDefault();
            Company companyid = (Company)(Session["CId"]);
            if (companyid == null)
            {
                Session.Add("CId", companyid);

            }
            else
            {
            }
            if (companyid == null)
            {
                ViewBag.companyid = null;
                ViewBag.LoggedCompany = null;
            }
            else
            {
                ViewBag.companyid = companyid;
                ViewBag.LoggedCompany = companyid.CompanyName;
                
            }

            return View();
        }

        public ActionResult NoRightsAjax()
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
                // return RedirectToAction("Login", "Login");
            }
            // Company companyobj = db.Companys.Where(x => x.ID == id).FirstOrDefault();
            Company companyid = (Company)(Session["CId"]);
            if (companyid == null)
            {
                Session.Add("CId", companyid);

            }
            else
            {
            }
            if (companyid == null)
            {
                ViewBag.companyid = null;
                ViewBag.LoggedCompany = null;
            }
            else
            {
                ViewBag.companyid = companyid;
                ViewBag.LoggedCompany = companyid.CompanyName;
            }

            return PartialView();
        }

        /// <summary>
        ///  used with CSRF error
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}