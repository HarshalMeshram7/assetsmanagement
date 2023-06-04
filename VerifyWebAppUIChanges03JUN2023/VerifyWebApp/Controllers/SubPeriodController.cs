using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class SubPeriodController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Period

        public ActionResult Index(string id)
        {
            List<SubPeriod> lstsperiod = new List<SubPeriod>();
           int int_id = Convert.ToInt32(id);
            try
            {

                lstsperiod = db.SubPeriods.Where(x=>x.PeriodID==int_id).ToList();
                int srno = 1;
                foreach (var item in lstsperiod)
                {
                    item.Srno = srno;
                    item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy"); ;
                    item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                    srno++;

                }
                ViewBag.Periodid = id;
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }


            return View(lstsperiod);
        }


        [HttpGet]
        public ActionResult AddNew()
        {
           // int userid = 0;
          //  ViewBag.addperiodid = perodid;
            return PartialView();
        }

        // GET: Brand/Create
        [HttpPost]
        public ActionResult AddNew(SubPeriod speriod)
        {

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


                    db.SubPeriods.Add(speriod);

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
        [HttpGet]
        public ActionResult Edit(int id)
        {


            SubPeriod period = new SubPeriod();
            try
            {
                period = db.SubPeriods.Where(x => x.ID == id).FirstOrDefault();

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
        public ActionResult Edit(SubPeriod period)
        {

            JsonResult res;
            res = new JsonResult();


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            try
            {


                db.Entry(period).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult Delete(int id)
        {

            JsonResult res;
            res = new JsonResult();
            Period period;
            List<SubPeriod> subplist = new List<SubPeriod>();
            subplist = db.SubPeriods.Where(x => x.PeriodID == id).ToList();

            try
            {
                if (subplist.Count == 0)
                {

                    period = db.Periods.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(period).State = System.Data.Entity.EntityState.Deleted;
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
