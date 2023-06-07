using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class UOMController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: UOM
        [AuthUser]
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
            List<UOM> lstUOM = new List<UOM>();
            lstUOM = db.UOMs.Where(x=>x.Companyid==companyid).ToList();
            return View(lstUOM);
            
        }
        [AuthUser]
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
            //return View();
            return PartialView();
        }
        [AuthUser]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UOM uom = new UOM();
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
            try
            {
                uom = db.UOMs.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                return PartialView();
            }
            return PartialView(uom);
            //return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Add(UOM uom)
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
            if (ModelState.IsValid)
            {
                try
                {
                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                    var tnow = System.DateTime.Now.ToUniversalTime();
                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                    uom.CreatedUserId = userid;
                    uom.Companyid = companyid;
                    db.UOMs.Add(uom);
                    db.SaveChanges();


                    res.Data = "Success";

                    return res;
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]

        public ActionResult Edit(UOM uom)
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
            try
            {
                TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var tnow = System.DateTime.Now.ToUniversalTime();
                DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
               // uom.CreatedUserId = userid;
                uom.Modified_Userid = userid;
                uom.ModifiedDate = istDate;

                uom.Companyid = companyid; // added by Mandar 17 JUN 2020

                db.Entry(uom).State = System.Data.Entity.EntityState.Modified;
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
            //return View();
        }
        [AuthUser]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Delete(int id)
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

            JsonResult res;
            res = new JsonResult();
            UOM uonobj;

            List<Assets> Asset = new List<Assets>();
            //List<Assets> Asset1 = new List<Assets>();
            //List<Assets> Asset2 = new List<Assets>();

            Asset = db.Assetss.Where(x => x.UOMNo == id && x.Companyid == companyid).ToList();
            //Asset1 = db.Assetss.Where(x => x.AccAccountID == id).ToList();
            //Asset2 = db.Assetss.Where(x => x.DepAccountId == id).ToList();

            try
            {
                if (Asset.Count != 0)
                {
                    res.Data = "Failed";

                }
                else
                {
                    uonobj = db.UOMs.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(uonobj).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    res.Data = "Success";


                }
                return res;
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                res.Data = "Err";
                return res;
            }

        }

    }
}