using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class ItPeriodController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Period
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
            List<ITPeriod> lstitperiod = new List<ITPeriod>();

            try
            {

                lstitperiod = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
                int srno = 1;
                foreach (var item in lstitperiod)
                {
                    item.Srno = srno;
                    item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy"); ;
                    item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                    srno++;

                }

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }


            return View(lstitperiod);
        }

        [AuthUser]
        [HttpGet]
        public ActionResult AddNew()
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


            ITPeriod itperiod = new ITPeriod();
            string str_fromDate = "";
            string str_ToDate = "";
            //  var period = "";
            itperiod = db.ITPeriods.Where(x => x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();//needs to do validation if no period is added all ready


            if (itperiod != null)
            {

                str_fromDate = itperiod.FromDate.AddYears(1).ToString("dd/MM/yyyy");
                str_ToDate = itperiod.ToDate.AddYears(1).ToString("dd/MM/yyyy");
                ViewBag.fromdate = str_fromDate;
                ViewBag.todate = str_ToDate;

            }
            else
            {
                ViewBag.fromdate = "01/04/";
                ViewBag.todate = "31/03/";
            }
            return PartialView();
        }

        // GET: Brand/Create
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult AddNew(ITPeriod itperiod)
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

            // int TenantID = 0;

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);




            if (ModelState.IsValid)
            {
                try
                {
                    // designation.MinHrs = 5;
                    //activity.AssignementID = 0;
                    itperiod.CreatedUserId = userid;
                    itperiod.CreatedDate = istDate;
                    itperiod.Companyid = companyid;
                    itperiod.DepFlag = "N";

                    db.ITPeriods.Add(itperiod);

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
                res.Data = "ERR";
                return res;

            }
        }



        // GET: Brand/Edit/5
        [AuthUser]
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            ITPeriod period = new ITPeriod();
            try
            {
                period = db.ITPeriods.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();

                period.str_fromdate = period.FromDate.ToString("dd/MM/yyyy");
                period.str_todate = period.ToDate.ToString("dd/MM/yyyy");


            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            return PartialView(period);

        }
        // [AuthUser]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Edit(ITPeriod period)
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


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            try
            {
                ITPeriod iTPeriod = new ITPeriod();
                iTPeriod = db.ITPeriods.Where(x => x.ID == period.ID).FirstOrDefault();
                iTPeriod.FromDate = period.FromDate;
                iTPeriod.ToDate = period.ToDate;
                iTPeriod.PeriodlockFlag = period.PeriodlockFlag;
              
                iTPeriod.ModifiedDate = istDate;
                iTPeriod.Modified_Userid = userid;
                db.Entry(iTPeriod).State = System.Data.Entity.EntityState.Modified;
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

        [AuthUser]
        [HttpPost]
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
            ITPeriod periodobj = new ITPeriod();
            //List<ITPeriod> subplist = new List<ITPeriod>();
            periodobj = db.ITPeriods.Where(x => x.Companyid == companyid && (x.DepFlag == "N" || x.PeriodlockFlag == 0)).FirstOrDefault();

            try
            {
                if (periodobj != null)
                {

                    periodobj = db.ITPeriods.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(periodobj).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    res.Data = "Success";

                }
                else
                {

                    res.Data = "Failed";

                }
                return res;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                //logger.Log(LogLevel.Error, strError);
                res.Data = "Err";
                return res;
            }

        }
    }
}
