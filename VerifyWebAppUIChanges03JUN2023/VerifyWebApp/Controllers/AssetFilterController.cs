using OfficeOpenXml;
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
    public class AssetFilterController : Controller
    {
        public VerifyDB db = new VerifyDB();

        //// GET: AssetFilter
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult SearchAsset(string assetNo,string assetname, string srno)
        {
            try
            {
                //return Json(new { foo = "bar", baz = "Blech" });

                List<Assets> lstAssets = new List<Assets>();

                if (assetNo.Length > 0)
                {
                    lstAssets = db.Assetss.Where(x => x.AssetNo.StartsWith(assetNo)).ToList();
                }

                if (assetname.Length > 0)
                {
                    lstAssets = db.Assetss.Where(x => x.AssetName.StartsWith(assetname)).ToList();
                }

                if (srno.Length > 0)
                {
                    lstAssets = db.Assetss.Where(x => x.SrNo.StartsWith(srno)).ToList();
                }



                return Json(new { foo = "bar", baz = "Blech" });


            }
            catch(Exception ex)
            {
                return Json(new { foo = "bar", baz = "Blech" });
            }
        }
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            return View();
        }
    }
}