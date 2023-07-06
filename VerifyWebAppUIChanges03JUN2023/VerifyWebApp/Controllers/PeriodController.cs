using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
namespace VerifyWebApp.Controllers
{
    public class PeriodController : Controller
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            List<Period> lstperiod = new List<Period>();
            int srno = 1;
            try
            {

                lstperiod = db.Periods.Where(x => x.Companyid == companyid).OrderByDescending(x=>x.ToDate).ToList();

                foreach (var item in lstperiod)
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
            ViewBag.Srno = srno;

            return View(lstperiod);
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            /////////*****************
            //do validations
            //**********************/

            Period period = new Period();
            string str_fromDate = "";
            //  var period = "";
            period = db.Periods.Where(x => x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();//needs to do validation if no period is added all ready

            //DateTime fromdate;
            //if (period != null)
            //{

            //    str_fromDate = period.ToDate.AddDays(1).ToString("dd/MM/yyyy");
            //    ViewBag.fromdate = str_fromDate;

            //}
            //else
            //{
            //    ViewBag.fromdate = "";
            //}

           // DateTime fromdate;
            if (period != null)
            {

                str_fromDate = period.ToDate.AddDays(1).ToString("yyyy/MM/dd");
                ViewBag.fromdate = str_fromDate;

            }
            else
            {
                ViewBag.fromdate = "";
            }
            return PartialView();
        }

        private DateTime ParseExact(string str_fromdate, string v, CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
        }

        // GET: Brand/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult AddNew(PeriodViewModel period)
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




            if (ModelState.IsValid)
            {
                Period periodobj = new Period();
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        periodobj.FromDate = period.FromDate;
                        periodobj.ToDate = period.ToDate;
                        periodobj.Companyid = companyid;
                        periodobj.CreatedUserId = userid;
                        periodobj.CreatedDate = istDate;
                        db.Periods.Add(periodobj);
                        db.SaveChanges();
                        var periodid = periodobj.ID;// db.Periods.Max(x => x.ID);
                        SubPeriod subperiod = new SubPeriod();
                        if (period.PeriodViewModellist.Count() != 0)
                        {
                            foreach (var item in period.PeriodViewModellist)
                            {
                                subperiod.PeriodID = periodid;
                                subperiod.FromDate = item.FromDate;
                                subperiod.ToDate = item.ToDate;
                                subperiod.CreatedDate = istDate;
                                subperiod.Companyid = companyid;
                                subperiod.CreatedUserId = userid;
                                // subperiod.PeriodLockFlag = item.PeriodLockFlag;
                                if (item.PeriodLockFlag == "" || item.PeriodLockFlag == null)
                                {
                                    subperiod.PeriodLockFlag = "N";
                                }
                                else
                                {
                                    subperiod.PeriodLockFlag = item.PeriodLockFlag;
                                }
                                if (item.DepFlag == "" || item.DepFlag == null)
                                {
                                    subperiod.DepFlag = "N";
                                }
                                else
                                {
                                    subperiod.DepFlag = item.DepFlag;
                                }

                                db.SubPeriods.Add(subperiod);
                                db.SaveChanges();

                            }
                        }
                        else
                        {
                            subperiod.PeriodID = periodid;
                            subperiod.FromDate = period.FromDate;
                            subperiod.ToDate = period.ToDate;
                            subperiod.CreatedDate = istDate;
                            subperiod.Companyid = companyid;
                            subperiod.CreatedUserId = userid;
                            subperiod.PeriodLockFlag = "N";
                            subperiod.DepFlag = "N";
                            db.SubPeriods.Add(subperiod);
                            db.SaveChanges();
                        }
                        transaction.Commit();
                        res.Data = "Success";
                        return res;

                    }
                    catch (Exception ex)
                    {
                        string strError;
                        strError = ex.Message + "|" + ex.InnerException;
                        // logger.Log(LogLevel.Error, strError);
                        transaction.Rollback();
                        res.Data = "Failed";
                        return res;

                    }

                }
            }
            else
            {
                res.Data = "ERR";
                return res;

            }
        }



        // GET: Brand/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{


        //    Period period = new Period();
        //    List<SubPeriod> splist = new List<SubPeriod>();
        //    try
        //    {
        //        period = db.Periods.Where(x => x.ID == id).FirstOrDefault();

        //        ViewBag.StartDate = period.FromDate.ToString("dd/MM/yyyy"); 
        //        ViewBag.EndDate = period.ToDate.ToString("dd/MM/yyyy");
        //      splist = db.SubPeriods.Where(x => x.PeriodID == id).ToList();
        //        int srno = 1;
        //        foreach (SubPeriod item in splist)
        //        {
        //            item.Srno = srno;
        //            item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy"); ;
        //            item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
        //            srno++;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        string strError;
        //        strError = ex.Message + "|" + ex.InnerException;
        //        // logger.Log(LogLevel.Error, strError);

        //    }
        //    return PartialView(splist);

        //}
        [HttpGet]
        [AuthUser]
        public ActionResult EditNew(int id)
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
        
            Period period = new Period();
            List<SubPeriod> splist = new List<SubPeriod>();
            try
            {
                period = db.Periods.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();

                ViewBag.StartDate = period.FromDate.ToString("dd/MM/yyyy");
                ViewBag.EndDate = period.ToDate.ToString("dd/MM/yyyy");
                ViewBag.id = period.ID;
                
                splist = db.SubPeriods.Where(x => x.PeriodID == id && x.Companyid == companyid).ToList();
                int srno = 1;
                int depcnt = 0;
                foreach (SubPeriod item in splist)
                {
                    item.Srno = srno;
                    item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy"); 
                    item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); 
                    if(item.DepFlag=="Y" && item.PeriodLockFlag == "Y")
                    {
                        depcnt = 1+depcnt;
                    }
                    srno++;
                }
                if (depcnt > 0)
                {
                    ViewBag.depflag = "Y";
                }
                else
                {
                    ViewBag.depflag = "N";
                }
                
                ViewBag.Srno = srno;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            return PartialView(splist);

        }
        // [AuthUser]
        [HttpPost]
        [AllowAnonymousAttribute]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Edit(PeriodViewModel period)
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
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    Period editperiod = new Period();
                    editperiod = db.Periods.Where(x => x.ID == period.ID && x.Companyid == companyid).FirstOrDefault();
                    editperiod.FromDate = period.FromDate;
                    editperiod.ToDate = period.ToDate;
                    editperiod.ModifiedDate = istDate;
                    editperiod.Modified_Userid = userid;
                    db.Entry(editperiod).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    List<SubPeriod> slist = new List<SubPeriod>();
                    slist = db.SubPeriods.Where(x => x.PeriodID == period.ID && x.Companyid == companyid).ToList();
                    db.SubPeriods.RemoveRange(slist);
                    db.SaveChanges();
                    SubPeriod subperiod = new SubPeriod();
                    if (period.PeriodViewModellist.Count() != 0)
                    {
                        foreach (var item in period.PeriodViewModellist)
                        {
                            subperiod.PeriodID = period.ID;
                            subperiod.FromDate = item.FromDate;
                            subperiod.ToDate = item.ToDate;
                            subperiod.Companyid = companyid;
                            subperiod.Modified_Userid = userid;
                            subperiod.ModifiedDate = istDate;
                            if (item.PeriodLockFlag == "" || item.PeriodLockFlag == null)
                            {
                                subperiod.PeriodLockFlag = "N";
                            }
                            else
                            {
                                subperiod.PeriodLockFlag = item.PeriodLockFlag;
                            }
                            if (item.DepFlag == "" || item.DepFlag == null)
                            {
                                subperiod.DepFlag = "N";
                            }
                            else
                            {
                                subperiod.DepFlag = item.DepFlag;
                            }

                            db.SubPeriods.Add(subperiod);
                            db.SaveChanges();

                        }
                    }
                    else
                    {
                        //subperiod.PeriodID = period.ID;
                        //subperiod.FromDate = period.FromDate;
                        //subperiod.ToDate = period.ToDate;
                        //subperiod.ModifiedDate = istDate;
                        //subperiod.Modified_Userid = userid;
                        //subperiod.Companyid = companyid;
                        //subperiod.PeriodLockFlag = "N";
                        //subperiod.DepFlag = "N";
                        //db.SubPeriods.Add(subperiod);
                        //db.SaveChanges();
                    }
                    transaction.Commit();
                    res.Data = "Success";
                    return res;

                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    // logger.Log(LogLevel.Error, strError);
                    transaction.Rollback();
                    res.Data = "Failed";
                    return res;
                }
            }
        }

        [AuthUser]
        [HttpPost]
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
            Period periodobj;
            List<SubPeriod> subplist = new List<SubPeriod>();
            subplist = db.SubPeriods.Where(x => x.PeriodID == id && x.Companyid == companyid && (x.DepFlag == "N" || x.PeriodLockFlag == "N")).ToList();  // 

            try
            {
                if (subplist.Count == 0)
                {

                    periodobj = db.Periods.Where(x => x.ID == id).FirstOrDefault();
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
