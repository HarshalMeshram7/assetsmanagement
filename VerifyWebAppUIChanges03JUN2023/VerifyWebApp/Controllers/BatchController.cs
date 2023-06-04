using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class BatchController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Batch
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

            List<Batch> lstbatch = new List<Batch>();
            int srno = 1;
            try
            {

                lstbatch = db.Batchs.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstbatch)
                {
                    item.Srno = srno;
                    item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
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

            return View(lstbatch);

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

            // int userid = 0;
            Batch ins = new Batch();
            // ViewBag.Assestlist = new SelectList(db.Assetss.OrderBy(e => e.ID), "ID", "ID");   
            //  ViewBag.LocationAName = db.ALocations.Where(x => x.ID == id).FirstOrDefault().AGroupName;
            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");
            //return PartialView();
            return View();

        }

        [HttpPost]
        public ActionResult getlocationb(string id)
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

            int int_id = Convert.ToInt32(id);

            List<BLocation> blist = new List<BLocation>();
            blist = db.BLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(blist, "ID", "BLocationName", 0);
            return Json(ob);

        }
        [HttpPost]
        public ActionResult getlocationc(string id)
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

            int int_id = Convert.ToInt32(id);

            List<CLocation> clist = new List<CLocation>();
            clist = db.CLocations.Where(x => x.BLocID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(clist, "ID", "CLocationName", 0);
            return Json(ob);


        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Add(BatchViewmodel batchViewmodel)
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

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            Batch batchobj = new Batch();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    batchobj.FromDate = batchViewmodel.FromDate;
                    batchobj.ToDate = batchViewmodel.ToDate;
                    batchobj.BatchDescription = batchViewmodel.BatchDescription;
                    batchobj.IsBatchOpen = batchViewmodel.IsBatchOpen;
                    batchobj.CreatedUserId = userid;
                    batchobj.CreatedDate = istDate;
                    batchobj.Companyid = companyid;
                    batchobj.ClientID = 1; // Mandar 09 APR 2022

                    db.Batchs.Add(batchobj);
                    db.SaveChanges();

                    //var batchid = db.Batchs.Max(x => x.ID);
                    var batchid = batchobj.ID;

                    Subbatch subbatch = new Subbatch();
                    if (batchViewmodel.locationlist.Count() != 0)
                    {
                        foreach (var item in batchViewmodel.locationlist)
                        {
                            subbatch.BatchId = batchid;
                            subbatch.LocAId = item.LocAId;
                            subbatch.LocBId = item.LocBId;
                            subbatch.LocCId = item.LocCId;
                            subbatch.CreatedUserId = userid;
                            subbatch.CreatedDate = istDate;
                            subbatch.Companyid = companyid;
                            var filterLocAID = 0;
                            var filterLocBID = 0;
                            var filterLocCID = 0;
                            if (item.LocAId != 0)
                            {
                                subbatch.LocAName = db.ALocations.Where(x => x.ID == item.LocAId && x.Companyid == companyid).FirstOrDefault().ALocationName;
                                filterLocAID = item.LocAId;
                            }
                            if (item.LocBId != 0)
                            {
                                subbatch.LocBName = db.BLocations.Where(x => x.ID == item.LocBId && x.Companyid == companyid).FirstOrDefault().BLocationName;
                                filterLocBID = item.LocBId;
                            }
                            if (item.LocCId != 0)
                            {
                                subbatch.LocCName = db.CLocations.Where(x => x.ID == item.LocCId && x.Companyid == companyid).FirstOrDefault().CLocationName;
                                filterLocCID = item.LocCId;
                            }


                            db.SubBatchs.Add(subbatch);
                            db.SaveChanges();

                            string SQL = "select * from tblassets where companyid = " + companyid;
                            if (item.LocAId != 0)
                            {
                                SQL = SQL + " and LocAID=" + item.LocAId;
                            }
                            if (item.LocBId != 0)
                            {
                                SQL = SQL + " and LocBID=" + item.LocBId;
                            }
                            if (item.LocCId != 0)
                            {
                                SQL = SQL + " and LocCID=" + item.LocCId;
                            }

                            List<Assets> lstTempAssets=  db.Database.SqlQuery<Assets>(SQL).ToList();

                            foreach (Assets objAsset in lstTempAssets)
                            {
                                BatchAsset batchAssets = new BatchAsset();

                                batchAssets.AssetID = objAsset.ID;
                                batchAssets.assetno = objAsset.AssetNo;
                                batchAssets.Companyid = companyid;
                                batchAssets.BatchID = batchid;
                                db.BatchAssets.Add(batchAssets);

                            }
                            db.SaveChanges();
                        }
                    }else // alll assets selected 
                    {

                        string SQL = "select * from tblassets where companyid = " + companyid;
                        List<Assets> lstTempAssets = db.Database.SqlQuery<Assets>(SQL).ToList();

                        foreach (Assets objAsset in lstTempAssets)
                        {
                            BatchAsset batchAssets = new BatchAsset();

                            batchAssets.AssetID = objAsset.ID;
                            batchAssets.assetno = objAsset.AssetNo;
                            batchAssets.Companyid = companyid;
                            batchAssets.BatchID = batchid;
                            db.BatchAssets.Add(batchAssets);

                        }
                        db.SaveChanges();

                    }
                    transaction.Commit();
                    return RedirectToAction("Index", "Batch");
                    //res.Data = "Success";
                    //return res;

                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    // logger.Log(LogLevel.Error, strError);
                    transaction.Rollback();
                    return RedirectToAction("Index", "Batch");
                    //res.Data = "Failed";
                    //return res;

                }

            }


        }

        private DateTime ParseExact(string str_fromdate, string v, CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
        }

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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            Batch batch = new Batch();
            List<Subbatch> splist = new List<Subbatch>();
            try
            {
                // batch = db.Batchs.Where(x => x.ID == id).FirstOrDefault();


                splist = db.SubBatchs.Where(x => x.BatchId == id && x.Companyid == companyid).ToList();
                batch = db.Batchs.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                ViewBag.FromDate = batch.FromDate.ToString("dd/MM/yyyy");
                ViewBag.ToDate = batch.ToDate.ToString("dd/MM/yyyy");

                ViewBag.BatchDescription = batch.BatchDescription;
                ViewBag.IsBatchOpen = batch.IsBatchOpen;
                ViewBag.ID = id;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetNo");
                int srno = 1;
                foreach (Subbatch item in splist)
                {
                    item.Srno = srno;
                    item.LocAId = item.LocAId;
                    item.LocBId = item.LocBId;
                    item.LocCId = item.LocCId;

                    //if (item.LocAId != 0)
                    //{
                    //    item.LocAName = db.ALocations.Where(x => x.ID == item.LocAId).FirstOrDefault().ALocationName;
                    //item.LocBName = db.BLocations.Where(x => x.ID == item.LocBId && x.ALocID == item.LocAId).FirstOrDefault().BLocationName;
                    //// item.LocCName = db.CLocations.Where(x => x.ALocID == item.LocCId && x.BLocID == item.LocBId && x.ID == item.LocCId).FirstOrDefault().CLocationName;
                    //item.LocCName = db.CLocations.Where(x => x.ALocID == item.LocAId && x.BLocID == item.LocBId && x.ID == item.LocCId).FirstOrDefault().CLocationName;
                    if (item.LocAId == 0)
                    {

                        item.LocAName = "";
                    }
                    else
                    {
                        item.LocAName = db.ALocations.Where(x => x.ID == item.LocAId && x.Companyid == companyid).FirstOrDefault().ALocationName;
                    }
                    if (item.LocBId == 0)
                    {

                        item.LocBName = "";
                    }
                    else
                    {
                        item.LocBName = db.BLocations.Where(x => x.ID == item.LocBId && x.ALocID == item.LocAId && x.Companyid == companyid).FirstOrDefault().BLocationName;
                    }
                    if (item.LocCId == 0)
                    {

                        item.LocCName = "";
                    }
                    else
                    {
                        item.LocCName = db.CLocations.Where(x => x.ALocID == item.LocAId && x.BLocID == item.LocBId && x.ID == item.LocCId && x.Companyid == companyid).FirstOrDefault().CLocationName;
                    }
                    srno++;
                }
                ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
                ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
                ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");
                ViewBag.Srno = srno;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            // return PartialView(splist);
            return View(splist);
        }
        //------------------------------------
        [AuthUser]
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
            Batch amcobj;
            List<Subbatch> subplist = new List<Subbatch>();
            subplist = db.SubBatchs.Where(x => x.BatchId == id && x.Companyid == companyid).ToList();

            try
            {
                if (subplist.Count == 0)
                {

                    amcobj = db.Batchs.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(amcobj).State = System.Data.Entity.EntityState.Deleted;
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
        //---------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Edit(BatchViewmodel batchViewmodel)
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


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Batch batchobj = new Batch();
                    batchobj = db.Batchs.Where(x => x.ID == batchViewmodel.ID).FirstOrDefault();
                    batchobj.FromDate = batchViewmodel.FromDate;
                    batchobj.ToDate = batchViewmodel.ToDate;
                    batchobj.BatchDescription = batchViewmodel.BatchDescription;
                    batchobj.IsBatchOpen = batchViewmodel.IsBatchOpen;
                    batchobj.Modified_Userid = userid;
                    batchobj.ModifiedDate = istDate;
                    batchobj.Companyid = companyid;

                    db.Entry(batchobj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    List<Subbatch> slist = new List<Subbatch>();
                    slist = db.SubBatchs.Where(x => x.BatchId == batchViewmodel.ID && x.Companyid == companyid).ToList();
                    db.SubBatchs.RemoveRange(slist);
                    db.SaveChanges();

                    Subbatch subbatch = new Subbatch();
                    if (batchViewmodel.locationlist.Count() != 0)
                    {
                        foreach (var item in batchViewmodel.locationlist)
                        {
                            subbatch.BatchId = batchViewmodel.ID;
                            subbatch.LocAId = item.LocAId;
                            subbatch.LocBId = item.LocBId;
                            subbatch.LocCId = item.LocCId;
                            subbatch.ModifiedDate = istDate;
                            subbatch.Modified_Userid = userid;
                            subbatch.Companyid = companyid;

                            db.SubBatchs.Add(subbatch);
                            db.SaveChanges();

                        }
                    }
                    transaction.Commit();
                    return RedirectToAction("Index", "Batch");
                    //res.Data = "Success";
                    //return res;

                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    // logger.Log(LogLevel.Error, strError);
                    transaction.Rollback();
                    //res.Data = "Failed";
                    //return res;
                    return RedirectToAction("Index", "Batch");
                }

            }
        }
    }
}