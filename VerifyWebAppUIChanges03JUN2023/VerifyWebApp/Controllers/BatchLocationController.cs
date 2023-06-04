using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class BatchLocationController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: BatchLocation
        public ActionResult Index(string id)
        {
            List<BatchLocation> lstBLoc = new List<BatchLocation>();
            int int_id = Convert.ToInt32(id);
            try
            {

                lstBLoc = db.BatchLocations.Where(x => x.BatchID == int_id).ToList();
                int srno = 1;
                //foreach (var item in lstBLoc)
                //{
                //    item.ID = srno;
                //    item.LocAID = item.FromDate.ToString("dd/MM/yyyy"); ;
                //    item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                //    srno++;

                //}
                ViewBag.Periodid = id;
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }


            return View(lstBLoc);
        }

        [HttpGet]
        public ActionResult Add()
        {
            // int userid = 0;
            //  ViewBag.addperiodid = perodid;
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddNew(BatchLocation batchLocation)
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


                    db.BatchLocations.Add(batchLocation);

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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            BatchLocation bLocation = new BatchLocation();
            try
            {
                bLocation = db.BatchLocations.Where(x => x.ID == id).FirstOrDefault();

                //period.str_fromdate = period.FromDate.ToString("dd/MM/yyyy");
                //period.str_todate = period.ToDate.ToString("dd/MM/yyyy");


            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            return PartialView(bLocation);
        }

        [HttpPost]
        public ActionResult Edit(BatchLocation batchLocation)
        {

            JsonResult res;
            res = new JsonResult();


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            try
            {


                db.Entry(batchLocation).State = System.Data.Entity.EntityState.Modified;
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
    }
}