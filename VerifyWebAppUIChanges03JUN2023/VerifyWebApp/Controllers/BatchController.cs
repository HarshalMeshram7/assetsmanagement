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

            int srno = 1;
            List<Batch> lstbatch = new List<Batch>();
            try
            {

                lstbatch = db.Batchs.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstbatch)
                {
                    item.Srno = srno;
                    item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                    item.str_todate = item.ToDate.ToString("dd/MM/yyyy");
                    item.Min_Value = item.MinimumValue;
                    item.Max_Value = item.MaximumValue;

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
            List<Assets> lstAssets = new List<Assets>();

            ViewBag.Assetlist = new SelectList(lstAssets, "ID", "AssetName");
            // ViewBag.Assestlist = new SelectList(db.Assetss.OrderBy(e => e.ID), "ID", "ID");   
            //  ViewBag.LocationAName = db.ALocations.Where(x => x.ID == id).FirstOrDefault().AGroupName;
            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");

            ViewBag.costalist = new SelectList(db.ACostCenters.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "CCDescription");
            // ViewBag.costblist = new SelectList(db.BCostCenters.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "SCCDescription");
            ViewBag.costblist = new SelectList(db.BCostCenters.OrderBy(e => e.ID), "ID", "SCCDescription");
            // ViewBag.costblist = new SelectList(" ","ID", "SCCDescription");
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
        public ActionResult getcostcenterb(string id)
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
            int int_id = Convert.ToInt32(id);

            List<BCostCenter> blist = new List<BCostCenter>();
            blist = db.BCostCenters.Where(x => x.CCID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(blist, "ID", "SCCDescription", 0);
            return Json(ob);

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
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
                   // batchobj.IsRangeSelect = batchViewmodel.IsRangeSelect;
                    batchobj.MinimumValue = batchViewmodel.MinimumValue.ToString();
                    batchobj.MaximumValue = batchViewmodel.MaximumValue.ToString();
                    batchobj.CreatedUserId = userid;
                    batchobj.CreatedDate = istDate;
                    batchobj.Companyid = companyid;
                    batchobj.ClientID = 1; // Mandar 09 APR 2022




                    db.Batchs.Add(batchobj);
                    db.SaveChanges();

                    String sMinMaxValueQuery = null;

                    if (sMinMaxValueQuery == null)
                    {
  
                        sMinMaxValueQuery = "AmountCapitalised > " + batchViewmodel.MinimumValue + " AND AmountCapitalised < " + batchViewmodel.MaximumValue;
                    }


                    //var batchid = db.Batchs.Max(x => x.ID);
                    var batchid = batchobj.ID;

                    String aLocation = null;
                    String bLocation = null;
                    String cLocation = null;


                    if (batchViewmodel.locationlist.Count() != 0)
                    {

                        foreach (var item in batchViewmodel.locationlist)
                        {
                            Subbatch subbatch = new Subbatch();
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
                                
                                if(aLocation == null)
                                {
                                    aLocation = "( LocAID = "+ item.LocAId + " )";
                                }
                                else
                                {
                                    aLocation = aLocation + " or ( LocAID = "+ item.LocAId + " )";
                                }

                            }

                            if (item.LocBId != 0)
                            {
                                subbatch.LocBName = db.BLocations.Where(x => x.ID == item.LocBId && x.Companyid == companyid).FirstOrDefault().BLocationName;
                                filterLocBID = item.LocBId;
                             
                                if (bLocation == null)
                                {
                                    bLocation = "( LocBID = " + item.LocBId + " )";
                                }
                                else
                                {
                                    bLocation = bLocation + " or ( LocBID = " + item.LocBId + " )";
                                }

                            }

                            if (item.LocCId != 0)
                            {
                                subbatch.LocCName = db.CLocations.Where(x => x.ID == item.LocCId && x.Companyid == companyid).FirstOrDefault().CLocationName;
                                filterLocCID = item.LocCId;
                               
                                if (cLocation == null)
                                {
                                    cLocation = " ( LocCID = " + item.LocCId + " )";
                                }
                                else
                                {
                                    cLocation = cLocation + " or ( LocCID = " + item.LocCId + " )";
                                }
                            }

                            db.SubBatchs.Add(subbatch);

                        }
 
                        db.SaveChanges();
                    }
                    else // alll assets selected 
                    {

                        //string SQL = "select * from tblassets where companyid = " + companyid;
                       // string SQL = "select AssetID,assetno from tblassets where companyid = " + companyid;
                        //List<Assets> lstTempAssets = db.Database.SqlQuery<Assets>(SQL).ToList();

                        //foreach (Assets objAsset in lstTempAssets)
                        //{
                        //    BatchAsset batchAssets = new BatchAsset();

                        //    batchAssets.AssetID = objAsset.ID;
                        //    batchAssets.assetno = objAsset.AssetNo;
                        //    batchAssets.Companyid = companyid;
                        //    batchAssets.BatchID = batchid;
                        //    db.BatchAssets.Add(batchAssets);

                        //}
                        //db.SaveChanges();

                    }

                    //------------------------costcenterlist by mayuri--------------------------
                    String aCostcenterr = null;
                    String bCostcenterr = null;

                    if (batchViewmodel.costcenterlist.Count() != 0)
                    {
                        foreach (var item in batchViewmodel.costcenterlist)
                        {
                            Subbatch subbatch = new Subbatch();
                            subbatch.BatchId = batchid;
                            subbatch.CCId = item.CCId;
                            subbatch.SCCId = item.SCCId;
                            subbatch.CreatedUserId = userid;
                            subbatch.CreatedDate = istDate;
                            subbatch.Companyid = companyid;
                            var filterCCId = 0;
                            var filterSCCId = 0;

                            if (item.CCId != 0)
                            {
                                subbatch.CCDescription = db.ACostCenters.Where(x => x.ID == item.CCId && x.Companyid == companyid).FirstOrDefault().CCDescription;
                                filterCCId = item.CCId;

                                if (aCostcenterr == null)
                                {
                                    aCostcenterr = "( CostCenterAID = " + item.CCId + " )";
                                }
                                else
                                {
                                    aCostcenterr = aCostcenterr + " or ( CostCenterAID = " + item.CCId + " )";
                                }

                            }
                            

                            if (item.SCCId != 0)
                            {
                                //both SCCDescription are not comming from same table.1st from subbatch.cs(tblsubbatch) and 2nd from ACostCenter.cs(tblacostcenter) 
                                subbatch.SCCDescription = db.BCostCenters.Where(x => x.ID == item.SCCId && x.Companyid == companyid).FirstOrDefault().SCCDescription;
                                filterSCCId = item.SCCId;
                                //sCostcenter = sCostcenter + " AND SCCId = " + item.SCCId + ")";
                                if (bCostcenterr == null)
                                {
                                    bCostcenterr = "( CostCenterBID = " + item.SCCId + " )";
                                }
                                else
                                {
                                    bCostcenterr = bCostcenterr + " or ( CostCenterBID = " + item.SCCId + " )";
                                }
                            }
                            else
                            {
                             
                            }

                            db.SubBatchs.Add(subbatch);

                        }
                        db.SaveChanges();
                    }

                  
                    string SQL = "select * from tblassets where companyid = " + companyid;
                    if (sMinMaxValueQuery != null)
                    {
                        SQL = SQL + " AND " + sMinMaxValueQuery;

                    }

                    if (aLocation != null)
                    {
                        SQL = SQL + " AND (" + aLocation + ")";
                    }
                    if (bLocation != null)
                    {
                        SQL = SQL + " AND (" + bLocation + ")";
                    }
                    if (cLocation != null)
                    {
                        SQL = SQL + " AND (" + cLocation + ")";
                    }
                    

                    if (aCostcenterr != null)
                    {
                        SQL = SQL + " AND (" + aCostcenterr + ")";
                    }
                    if (bCostcenterr != null)
                    {
                        SQL = SQL + " AND (" + bCostcenterr + ")";
                    }

                   
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
        //At the time of creation of batch we filter the assets according to range of amountcapitalization ,
        //list of location,list of costcenter and by groups where companyid=1.
        //(range is from tblbatch and locationlist,costcenterlist and group from tblsubbatch)
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
            List<Subbatch> cclist = new List<Subbatch>();
            try
            {
                //batch = db.Batchs.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                BatchViewmodel batchViewmodel = new BatchViewmodel();

                splist = db.SubBatchs.Where(x => x.BatchId == id && x.Companyid == companyid && x.LocAId > 0).ToList();
                cclist = db.SubBatchs.Where(x => x.BatchId == id && x.Companyid == companyid && x.CCId > 0).ToList();
                batch = db.Batchs.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();

                ViewBag.FromDate = batch.FromDate.ToString("dd/MM/yyyy");
                ViewBag.ToDate = batch.ToDate.ToString("dd/MM/yyyy");

                ViewBag.BatchDescription = batch.BatchDescription;
                ViewBag.IsBatchOpen = batch.IsBatchOpen;
                ViewBag.ID = id;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetNo");
                ViewBag.MinmumValue = batch.MinimumValue;
                ViewBag.MaxmumValue = batch.MaximumValue;



                int srno = 1;
                foreach (Subbatch item in splist)
                {

                    item.Srno = srno;
                    item.LocAId = item.LocAId;
                    item.LocBId = item.LocBId;
                    item.LocCId = item.LocCId;

                    //-----------------------------------------
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
                batch.Srno = srno;
                batch.SubbatchTable = splist;

                 int srnocc = 1;

                foreach (Subbatch item in cclist)
                {
                    item.Srnocc = srnocc;
                    item.CCId = item.CCId;
                    item.SCCId = item.SCCId;

                    if (item.CCId == 0)
                    {

                        item.CCDescription = "";
                    }
                    else
                    {
                        item.CCDescription = db.ACostCenters.Where(x => x.ID == item.CCId && x.Companyid == companyid).FirstOrDefault().CCDescription;
                    }
                    if (item.SCCId == 0)
                    {

                        item.SCCDescription = "";
                    }
                    else
                    {
                        item.SCCDescription = db.BCostCenters.Where(x => x.ID == item.SCCId && x.CCID == item.CCId && x.Companyid == companyid).FirstOrDefault().SCCDescription;
                    }
                    srnocc++;

                }

                ViewBag.costalist = new SelectList(db.ACostCenters.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "CCDescription");
                ViewBag.costblist = new SelectList(db.BCostCenters.OrderBy(e => e.ID), "ID", "SCCDescription");

                ViewBag.Srnocc = srnocc;
                batch.Srnocc = srnocc;
                batch.SubbatchTable2 = cclist;

                

            }

            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            // return PartialView(splist);
            return View(batch);
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
        [ValidateJsonXssAttribute] 
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
                    //List<Subbatch> clistt = new List<Subbatch>();
                    
                    slist = db.SubBatchs.Where(x => x.BatchId == batchViewmodel.ID && x.Companyid == companyid).ToList();
                   // clistt= db.SubBatchs.Where(x => x.BatchId == batchViewmodel.ID && x.Companyid == companyid).ToList();
                    db.SubBatchs.RemoveRange(slist);
                    db.SaveChanges();

                    // var batchid = batchobj.ID;
                    String sLocation = null;

                    if (batchViewmodel.locationlist.Count() != 0)
                    {
                        Subbatch subbatch = new Subbatch();
                        foreach (var item in batchViewmodel.locationlist)
                        {

                            subbatch.BatchId = batchViewmodel.ID;
                          //  subbatch.Srno = item.Srno;
                            subbatch.LocAId = item.LocAId;
                            subbatch.LocBId = item.LocBId;
                            subbatch.LocCId = item.LocCId;
                            subbatch.LocAName = item.LocAName;
                            subbatch.LocBName = item.LocBName;
                            subbatch.LocCName = item.LocCName;
                            subbatch.ModifiedDate = istDate;
                            subbatch.Modified_Userid = userid;
                            subbatch.Companyid = companyid;
                            var filterLocAID = 0;
                            var filterLocBID = 0;
                            var filterLocCID = 0;

                            if (item.LocAId != 0)
                            {
                                subbatch.LocAName = db.ALocations.Where(x => x.ID == item.LocAId && x.Companyid == companyid).FirstOrDefault().ALocationName;
                                filterLocAID = item.LocAId;
                                if (sLocation == null)
                                {
                                    sLocation = "( LocAID = " + item.LocAId;
                                }
                                else
                                {
                                    sLocation = sLocation + " OR (LocAID = " + item.LocAId;

                                }
                            }
                            if (item.LocBId != 0)
                            {
                                subbatch.LocBName = db.BLocations.Where(x => x.ID == item.LocBId && x.Companyid == companyid).FirstOrDefault().BLocationName;
                                filterLocBID = item.LocBId;
                                sLocation = sLocation + " AND LocBID = " + item.LocBId;
                            }
                            if (item.LocCId != 0)
                            {
                                subbatch.LocCName = db.CLocations.Where(x => x.ID == item.LocCId && x.Companyid == companyid).FirstOrDefault().CLocationName;
                                filterLocCID = item.LocCId;
                                sLocation = sLocation + " AND LocCID = " + item.LocCId;
                            }
                            sLocation = sLocation + " )";

                            db.SubBatchs.Add(subbatch); 
                        }
                        sLocation = "(" + sLocation + ")";
                        db.SaveChanges();
                    }


                    // ---------------------------------------------edit save by mayuri
                    String sCostcenter = null;

                    if (batchViewmodel.costcenterlist.Count() != 0)
                    {
                        Subbatch subbatch2 = new Subbatch();
                        foreach (var item in batchViewmodel.costcenterlist)
                        {
                            subbatch2.BatchId = batchViewmodel.ID;
                          //  subbatch.Srno = item.Srno;
                            subbatch2.CCId = item.CCId;
                            subbatch2.SCCId = item.SCCId;
                            subbatch2.CCDescription = item.CCDescription;
                            subbatch2.SCCDescription = item.SCCDescription;
                            subbatch2.ModifiedDate = istDate;
                            subbatch2.Modified_Userid = userid;
                            subbatch2.Companyid = companyid;
                            var filterCCId = 0;
                            var filterSCCId = 0;

                            if (item.CCId != 0)
                            {
                                subbatch2.CCDescription = db.ACostCenters.Where(x => x.ID == item.CCId && x.Companyid == companyid).FirstOrDefault().CCDescription;
                                filterCCId = item.CCId;
                                if (sCostcenter == null)
                                {
                                    sCostcenter = "( CCId = " + item.CCId;
                                }
                                else
                                {
                                    sCostcenter = sCostcenter + " OR ( CCId = " + item.CCId;
                                }
                            }
                            if (item.SCCId != 0)
                            {
                                //both SCCDescription are not comming from same table.1st from subbatch.cs(tblsubbatch) and 2nd from ACostCenter.cs(tblacostcenter) 
                                subbatch2.SCCDescription = db.BCostCenters.Where(x => x.ID == item.SCCId && x.Companyid == companyid).FirstOrDefault().SCCDescription;
                                filterSCCId = item.SCCId;
                                sCostcenter = sCostcenter + " AND SCCId = " + item.SCCId + " )";
                            }
                            else
                            {
                                sCostcenter = sCostcenter + " )";
                            }

                            db.SubBatchs.Add(subbatch2);
                        }
                        sCostcenter = "(" + sCostcenter + ")";
                        db.SaveChanges();
                    }
                    string SQL = "select AssetID,assetno from tblassets where companyid = " + companyid;

                    if (sLocation != null)
                    {
                        SQL = SQL + " AND " + sLocation;
                    }

                    if (sCostcenter != null)
                    {
                        SQL = SQL + " AND " + sCostcenter;

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