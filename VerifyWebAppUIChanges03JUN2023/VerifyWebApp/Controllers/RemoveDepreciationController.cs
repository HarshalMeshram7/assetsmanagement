using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class RemoveDepreciationController : Controller
    {

        public VerifyDB db = new VerifyDB();
        // GET: RemoveDepreciation

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
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }


            //  List<SubPeriod> splist = new List<SubPeriod>();
            string checkdepexists = "";



            var splist = db.SubPeriods.Where(x => x.Companyid == companyid && x.DepFlag == "Y").OrderByDescending(x => x.ToDate).FirstOrDefault();
            
            if (splist != null)
            {
                ViewBag.Fromdate = Convert.ToDateTime(splist.FromDate).ToString("dd/MM/yyyy");
                ViewBag.Todate = Convert.ToDateTime(splist.ToDate).ToString("dd/MM/yyyy");
                

                ViewBag.SubperiodId = splist.ID;
                checkdepexists = "yes";

            }
            else
            {
                checkdepexists = "no";
            }
            ViewBag.checkdepexists = checkdepexists;

            return View();
        }
        [AuthUser]
        [HttpPost]
        
        
        public ActionResult RemoveDepreciation(DateTime fromdate,DateTime todate,int subperiodid)
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
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            JsonResult res;
            res = new JsonResult();
            try
            {
                SubPeriod sp = new SubPeriod();
                sp = db.SubPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
                if (sp.PeriodLockFlag == "Y")
                {
                    res.Data = "Periodlock";
                    return res;
                }
                else
                {
                    //List<Depreciation> deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.FromDate == fromdate && x.ToDate == todate
                    //                               && x.DepreciationType == "A").ToList();
                    //db.Depreciations.RemoveRange(deplist);
                    //if (deplist.Count() != 0)
                    //{
                    //    db.SaveChanges();
                    //    SubPeriod modifiedsubperiod = new SubPeriod();
                    //    modifiedsubperiod = db.SubPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
                    //    if (modifiedsubperiod != null)
                    //    {
                    //        modifiedsubperiod.DepFlag = "N";
                    //        db.Entry(modifiedsubperiod).State = System.Data.Entity.EntityState.Modified;
                    //        db.SaveChanges();
                    //    }
                    //    res.Data = "Success";
                    //}
                    //else
                    //{
                    //    res.Data = "Norecordsfound";
                    //}

                    string startdate = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");
                    string enddate = Convert.ToDateTime(todate).ToString("yyyy-MM-dd");

                    string strSQL = "";
                    /*
                    
                    strSQL = "Call removedepreciation(";
                    strSQL = strSQL + companyid + ",";
                    strSQL = strSQL + "'" + startdate + "'," + "'" + enddate + "')";
                    */


                    strSQL =  " delete from tbldepreciation where FromDate >= '"  + startdate  + "' and ToDate <= '" + enddate  + "' and companyid =" + companyid;
                    strSQL = strSQL + " and id > 0 ";
                    strSQL = strSQL + " and DepreciationType ='A' ";

                    // mandar added check for manual deprecaion . 
                    // Should Delete only AUTO calculated depreciation
                    // 01 SEPT 2022

                    int rec = db.Database.ExecuteSqlCommand(strSQL);

                    if (rec > 0)
                    {
                        SubPeriod modifiedsubperiod = new SubPeriod();
                        modifiedsubperiod = db.SubPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
                        if (modifiedsubperiod != null)
                        {
                            modifiedsubperiod.DepFlag = "N";
                            db.Entry(modifiedsubperiod).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }        
                    }
                    res.Data = "Success";

                    //var result = db.Database.SqlQuery<RemoveDepreciationStatus_Viewmodel>(strSQL).FirstOrDefault();
                    //if (result != null)
                    //{
                    //    if (result.delete_status == "Yes")
                    //    {
                    //        SubPeriod modifiedsubperiod = new SubPeriod();
                    //        modifiedsubperiod = db.SubPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
                    //        if (modifiedsubperiod != null)
                    //        {
                    //            modifiedsubperiod.DepFlag = "N";
                    //            db.Entry(modifiedsubperiod).State = System.Data.Entity.EntityState.Modified;
                    //            db.SaveChanges();
                    //        }
                    //        res.Data = "Success";
                    //    }
                    //}
                }
                return res;
            }
            catch(Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                res.Data = "Failed";
                return res;
            }            
        }

        // GET: RemoveDepreciation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RemoveDepreciation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RemoveDepreciation/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RemoveDepreciation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RemoveDepreciation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RemoveDepreciation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RemoveDepreciation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
