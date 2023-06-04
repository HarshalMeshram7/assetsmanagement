using VerifyWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
using System.Data.Entity;
using VerifyWebApp.BusinessLogic;
using VerifyWebApp.Filter;

namespace VerifyWebApp.Controllers
{
    public class AssetLocationController : Controller
    {
        // GET: AssetLocation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeLocation()
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +  Request.ApplicationPath.TrimEnd('/') + "/";


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
                ViewBag.baseUrl = baseUrl;
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            return View();
        }

        public APIResponse UpdateLocation(Childlocation location)
        {

            APIResponse response = new APIResponse();
            response.status = "true";


            return response;


        }
    }
}