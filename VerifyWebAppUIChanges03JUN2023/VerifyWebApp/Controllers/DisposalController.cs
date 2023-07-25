using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
using VerifyWebApp.BusinessLogic;
using VerifyWebApp.Filter;

namespace VerifyWebApp.Controllers
{
    public class DisposalController : Controller
    {
        public VerifyDB db = new VerifyDB();



        // GET: Disposal
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

            // List<Disposal> lstadd = new List<Disposal>();
            List<Disposal_ViewModel> lstdisp = new List<Disposal_ViewModel>();
            int srno = 1;
            try
            {

                //lstadd = db.Disposals.Where(x => x.Companyid == companyid).ToList();

                //foreach (Disposal item in lstadd)
                //{
                //    item.int_Srno = srno;


                //    item.str_billDate = item.BillDate.ToString("dd/MM/yyyy");
                //    item.str_disposalDate = item.DisposalDate.ToString("dd/MM/yyyy"); ;
                //    item.str_voucherDate = item.VoucherDate.ToString("dd/MM/yyyy");
                //    if (item.AssetId != 0)
                //    {
                //        item.AssetNo = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //    }




                //    if (item.VoucherDate == null)
                //    {
                //        item.str_voucherDate = "";
                //    }
                //    else
                //    {

                //        item.str_voucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                //    }

                //    if (item.DisposalDate == null)
                //    {
                //        item.str_disposalDate = "";
                //    }
                //    else
                //    {

                //        item.str_disposalDate = Convert.ToDateTime(item.DisposalDate).ToString("dd/MM/yyyy");

                //    }

                //    if (item.BillDate == null)
                //    {
                //        item.str_billDate = "";
                //    }
                //    else
                //    {

                //        item.str_billDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                //    }

                //    if (item.str_billDate == "01/01/0001")
                //    {
                //        item.str_billDate = "";
                //    }

                //    srno++;
                //}
                string strSQL = "";
                strSQL = "Call Disposal(";
                strSQL = strSQL + companyid + ")";


                lstdisp = new List<Disposal_ViewModel>();


             //   lstdisp = db.Database.SqlQuery<Disposal_ViewModel>(strSQL).ToList();

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            //  ViewBag.Srno = srno;

            return View(lstdisp);
            // return View();
        }

        [HttpPost]
        public ActionResult GetDisposalData()
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
            int totalResultsCount;
            int filteredResultsCount;

            List<Disposal> dispList = new List<Disposal>();

            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

            JsonResult result = new JsonResult();

            try
            {
                
                dispList = db.Disposals.Where(x => x.Companyid == companyid).ToList();

                int totalRecords = dispList.Count;

                if (!string.IsNullOrEmpty(search) &&
                !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    // provide search for Asset Sr.No , Asset Name, Asset No
                    dispList = dispList.Where(p => p.AssetName.ToLower().Contains(search.ToLower())
                    ).ToList();

                }

                int recFilter = dispList.Count;
                // Apply pagination.   
                dispList = dispList.Skip(startRec).Take(pageSize).ToList();

                foreach (var item in dispList)
                {
                    item.str_disposalDate = item.DisposalDate.ToString("dd/MM/yyyy");
                }



                var lstAssets = dispList.Select(x => new { x.ID,
                                    x.AssetNo, x.AssetName,
                    x.DisposalType,x.str_disposalDate,
                     x.Qty,x.DisposalAmount,x.Remarks }).ToList();

              
                totalResultsCount = lstAssets.Count;

                filteredResultsCount = lstAssets.Count;

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = dispList
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                int test = 0;//ex.Message("");
            }
            return result;
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

            Disposal disposal = new Disposal();



            // ViewBag.Assestlist = new SelectList(db.Assetss.OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
            ViewBag.Assestlist = new SelectList(db.Assetss.Where(e => e.DisposalFlag == 0 && e.Companyid == companyid).
                OrderBy(e => e.AssetNo), "AssetNo", "AssetName");

            //ViewBag.supplierlist = new SelectList(db.Suppliers.OrderBy(e => e.ID), "ID", "SupplierName");
            //ViewBag.uomlist = new SelectList(db.UOMs.OrderBy(e => e.ID), "ID", "Unit");

            List<string> AssetCategory = new List<string>();
            AssetCategory.Add("Sold");
            AssetCategory.Add("Scraped");
            AssetCategory.Add("Stolen");
            AssetCategory.Add("lost");
            AssetCategory.Add("Replace");
            AssetCategory.Add("Others");

            // ViewBag.Group = Group;
            ViewBag.AssetCategory = new SelectList(AssetCategory);

            return View();
        }

        private DateTime ParseExact(string str_fromdate, string v, CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult getassetinfo(string id)
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
            int int_id = Convert.ToInt32(id);

            List<Assets> list = new List<Assets>();


            list = db.Assetss.Where(x => x.AssetNo == id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
            res.Data = list;
            return Json(res, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Add(Disposal disposal)
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
            //--------------------------------------------------------------
            Disposal objAcc = new Disposal();
            objAcc.DisposalCategory = disposal.DisposalCategory;
            if (objAcc.DisposalCategory == "1")
            {
                objAcc.DisposalCategory = "Sold";

            }
            else if (objAcc.DisposalCategory == "2")
            {
                objAcc.DisposalCategory = "Scraped";

            }
            else if (objAcc.DisposalCategory == "3")
            {
                objAcc.DisposalCategory = "Stolen";

            }
            else if (objAcc.DisposalCategory == "4")
            {
                objAcc.DisposalCategory = "Lost";

            }
            else if (objAcc.DisposalCategory == "5")
            {
                objAcc.DisposalCategory = "Replace";

            }
            else if (objAcc.DisposalCategory == "6")
            {
                objAcc.DisposalCategory = "Others";

            }
            //----------------------------------------------------------

            JsonResult res;
            res = new JsonResult();

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Assets objAsset = new Assets();
                    DateTime Dispodate;

                    //--------------------------------------------------------------
                    if (disposal.AssetNo != null)
                    {
                        var assetid = db.Assetss.Where(x => x.AssetNo == disposal.AssetNo && x.Companyid == companyid).FirstOrDefault().ID;
                        objAsset = db.Assetss.Where(x => x.ID == assetid && x.Companyid == companyid).FirstOrDefault();
                        disposal.AssetId = assetid;
                        //Dispodate = disposal.DisposalDate;
                    }
                    //---------------------------------------------------

                    disposal.CreatedUserId = userid;
                    disposal.CreatedDate = istDate;
                    disposal.Companyid = companyid;
                    db.Disposals.Add(disposal);
                    db.SaveChanges();
                    //----------------Add Disposal flag 1 in asset table---------------------------------------------
                    if (objAsset.ID == disposal.AssetId && disposal.DisposalType == "Full")
                    {
                        objAsset.DisposalFlag = 1;
                        db.Entry(objAsset).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                    }


                    ////-------Add Entry in depreciation-----------------------------
                    //int PeriodId;
                    //DateTime subFromDt;
                    //DateTime subToDt;

                    //List<Period> period = new List<Period>();
                    //period = db.Periods.ToList();
                    //if (period.Count != 0)
                    //{
                    //    foreach (Period item in period)
                    //    {
                    //        if (disposal.DisposalDate >= item.FromDate && disposal.DisposalDate <= item.ToDate)
                    //        {
                    //            PeriodId = item.ID;
                    //            List<SubPeriod> subperiod = new List<SubPeriod>();
                    //            subperiod = db.SubPeriods.Where(x => x.PeriodID == PeriodId && x.Companyid == companyid).ToList();
                    //            if (subperiod.Count != 0)
                    //            {
                    //                foreach (SubPeriod item1 in subperiod)
                    //                {
                    //                    if (disposal.DisposalDate >= item1.FromDate && disposal.DisposalDate <= item1.ToDate)
                    //                    {
                    //                        subFromDt = item1.FromDate;
                    //                        subToDt = item1.ToDate;

                    //                        Depreciation objDep = new Depreciation();
                    //                        //var DepId = db.Depreciations.Where(x => x.AssetId == disposal.AssetId && x.FromDate == subFromDt && x.ToDate == subToDt).FirstOrDefault().ID;
                    //                        objDep = db.Depreciations.Where(x => x.AssetId == disposal.AssetId && x.FromDate == subFromDt && x.ToDate == subToDt && x.Companyid == companyid).FirstOrDefault();
                    //                        //objDep = db.Depreciations.Where(x => x.ID == DepId).FirstOrDefault();
                    //                        if (objDep != null)
                    //                        {

                    //                            // var DepId = db.Depreciations.Where(x => x.AssetId == disposal.AssetId && x.FromDate == subFromDt && x.ToDate == subToDt).FirstOrDefault().ID;
                    //                            objDep.Amount = objDep.Amount + disposal.OpAccumulatedDepTill;
                    //                            db.Entry(objDep).State = System.Data.Entity.EntityState.Modified;
                    //                            db.SaveChanges();
                    //                        }
                    //                        else
                    //                        {
                    //                            Depreciation objDep2 = new Depreciation();
                    //                            //int SrNo = objDep.ID; //get previous record from depreciatio file
                    //                            // objDep.ID = 1;
                    //                            objDep2.AssetId = disposal.AssetId;
                    //                            objDep2.AssetName = objAsset.AssetName;
                    //                            objDep2.DepreciationType = "D";
                    //                            objDep2.DepreciationMethod = objAsset.DepreciationMethod;
                    //                            objDep2.TotalRate = Convert.ToDecimal(objAsset.TotalRate);
                    //                            objDep2.Amount = disposal.dep;
                    //                            objDep2.ToDate = disposal.DisposalDate;
                    //                            objDep2.FromDate = item1.FromDate;
                    //                            objDep2.NormalRate = objAsset.NormalRatae;
                    //                            objDep2.AdditionRate = objAsset.AdditionalRate;
                    //                            double noofdays= (disposal.DisposalDate - item1.FromDate).TotalDays;
                    //                            objDep2.DepreciationDays = Convert.ToInt32(noofdays);
                    //                            objDep2.CreatedUserId = userid;
                    //                            objDep2.CreatedDate = istDate;
                    //                            objDep2.Companyid = companyid;
                    //                            db.Depreciations.Add(objDep2);
                    //                            db.SaveChanges();

                    //                        }

                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}



                    //-----------------------------------
                    transaction.Commit();
                    res.Data = "Success";
                    // db.SaveChanges();
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

        [HttpGet]
        public ActionResult validateassetdate(DateTime strvdate, DateTime strddate, string assetno)
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
            // int int_assetno = Convert.ToInt32(assetno);
            Assets assets = new Assets();

            //// DateTime vdate = DateTime.Parse(strvdate);
            // DateTime ddate = DateTime.Parse(strddate);

            assets = db.Assetss.Where(x => x.AssetNo == assetno && x.Companyid == companyid).FirstOrDefault();
            if (assets != null)
            {

                if (strvdate < assets.VoucherDate)
                {
                    res.Data = "AssetYes";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                if (strddate < assets.VoucherDate)
                {
                    res.Data = "ChkDate";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                if (strddate < strvdate)
                {
                    res.Data = "ChkDDate";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                res.Data = "Noassetfound";
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        //-------------------------------------------------------------------
        [HttpGet]
        public ActionResult checkperiod_itperiod_exits()
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
            List<Period> period = new List<Period>();
            period = db.Periods.ToList();
            List<ITPeriod> itperiod = new List<ITPeriod>();
            itperiod = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
            if (period.Count != 0)
            {
                res.Data = "Yesperiodexists";

            }
            else
            {
                res.Data = "Noperiodexists";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            if (itperiod.Count != 0)
            {
                res.Data = "Yesitperiodexists";

            }
            else
            {
                res.Data = "Noitperiodexists";
                return Json(res, JsonRequestBehavior.AllowGet);
            }


            return Json(res, JsonRequestBehavior.AllowGet);

        }

        //----------------------------------------------------------------------
        //[HttpGet]
        //public ActionResult Dateputtousevalidation(string strvdate)
        //{
        //    //i have used res.Data=yess for error showing error and res.data=no for no errors
        //    int userid = 0;
        //    Login user = (Login)(Session["PUser"]);

        //    if (user != null)
        //    {
        //        ViewBag.LogonUser = user.UserName;
        //        userid = user.ID;
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Login");
        //    }
        //    int companyid = 0;
        //    Company company = (Company)(Session["Cid"]);

        //    if (company != null)
        //    {
        //        ViewBag.LoggedCompany = company.CompanyName;
        //        companyid = company.ID;
        //        ViewBag.companyid = companyid;
        //    }
        //    else
        //    {
        //        return RedirectToAction("CompanySelection", "Company");
        //    }

        //    JsonResult res;
        //    res = new JsonResult();
        //    DateTime itperioddate = Convert.ToDateTime("01/01/0001");
        //    DateTime perioddate = Convert.ToDateTime("01/01/0001");
        //    List<ITPeriod> itperiod = new List<ITPeriod>();

        //    // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
        //    itperiod = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
        //    // string checkflag = "";
        //    DateTime vdate = Convert.ToDateTime(strvdate);
        //    //if (itperiod.Count!=0)
        //    //{
        //    List<ITPeriod> itperiodlock = new List<ITPeriod>();
        //    itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1 && x.Companyid == companyid).ToList();
        //    if (itperiodlock.Count != 0)
        //    {
        //        foreach (ITPeriod item in itperiodlock)
        //        {
        //            if (vdate >= item.FromDate && vdate <= item.ToDate)
        //            {
        //                // checkflag = "Yes";
        //                // res.Data = checkflag;
        //                itperioddate = item.ToDate;

        //                break;
        //            }
        //            //else
        //            //{
        //            //   itperioddate = Convert.ToDateTime("00/00/0000");
        //            //    // checkflag = "No";
        //            //    //res.Data = checkflag;
        //            //}
        //        }

        //    }
        //    else
        //    {
        //        // perioddate = Convert.ToDateTime("00/00/0000");
        //        // res.Data = "NoLock";
        //    }
        //    //}
        //    //else
        //    //{
        //    //    res.Data = "Noitperiod";
        //    //}

        //    List<Period> period = new List<Period>();
        //    int periodid;
        //    List<SubPeriod> subperiod = new List<SubPeriod>();
        //    // subperiod = db.SubPeriods.ToList();
        //    period = db.Periods.Where(x => x.Companyid == companyid).ToList();
        //    //if(period.Count!=0)
        //    //{
        //    foreach (Period item in period)
        //    {
        //        if (vdate >= item.FromDate && vdate <= item.ToDate)
        //        {
        //            periodid = item.ID;
        //            SubPeriod checkdepflag = new SubPeriod();
        //            checkdepflag = db.SubPeriods.Where(x => x.PeriodID == periodid && x.DepFlag == "Y" && x.FromDate <= vdate && x.ToDate >= vdate && x.Companyid == companyid).FirstOrDefault();
        //            if (checkdepflag == null)
        //            {
        //                subperiod = db.SubPeriods.Where(x => x.PeriodID == periodid && x.PeriodLockFlag == "Y" && x.Companyid == companyid).ToList();
        //                if (subperiod.Count != 0)
        //                {
        //                    foreach (SubPeriod itemsub in subperiod)
        //                    {
        //                        if (vdate >= item.FromDate && vdate <= item.ToDate)
        //                        {
        //                            perioddate = item.ToDate;
        //                            break;
        //                        }
        //                        //else
        //                        //{
        //                        //    perioddate = Convert.ToDateTime("00/00/0000");
        //                        //}
        //                    }
        //                }
        //                else
        //                {
        //                    //perioddate = Convert.ToDateTime("00/00/0000");
        //                }
        //            }

        //            else
        //            {
        //                res.Data = "Depalreadycalculated";
        //                return Json(res, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            res.Data = "Nomainperiod";
        //            return Json(res, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    //}
        //    //else
        //    //{
        //    //    //noperiod
        //    //    perioddate = Convert.ToDateTime("00/00/0000");
        //    //}
        //    DateTime lockdate;
        //    int value = DateTime.Compare(perioddate, itperioddate);

        //    // checking 
        //    if (value > 0)
        //    {
        //        lockdate = perioddate;
        //        // Console.Write("date1 is later than date2. ");
        //    }
        //    else if (value < 0)
        //    {
        //        lockdate = itperioddate;
        //        //Console.Write("date1 is earlier than date2. ");
        //    }

        //    else
        //    {
        //        lockdate = perioddate;
        //        //Console.Write("date1 is the same as date2. ");
        //    }

        //    if (vdate <= lockdate)
        //    {
        //        //error
        //        res.Data = "Yes";
        //    }
        //    else
        //    {
        //        //nothing
        //        res.Data = "No";
        //    }

        //    return Json(res, JsonRequestBehavior.AllowGet);

        //}
        [HttpGet]
        public ActionResult Dateputtousevalidation(DateTime strvdate)
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
            //i have used res.Data=yess for error showing error and res.data=no for no errors

            JsonResult res;
            res = new JsonResult();
            DateTime itperioddate = Convert.ToDateTime("01/01/0001");
            DateTime perioddate = Convert.ToDateTime("01/01/0001");
            List<ITPeriod> itperiod = new List<ITPeriod>();

            // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
            itperiod = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
            // string checkflag = "";
            // DateTime vdate = Convert.ToDateTime(strvdate);
            //if (itperiod.Count!=0)
            //{
            List<ITPeriod> itperiodlock = new List<ITPeriod>();
            itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1 && x.Companyid == companyid).ToList();
            if (itperiodlock.Count != 0)
            {
                foreach (ITPeriod item in itperiodlock)
                {
                    if (strvdate >= item.FromDate && strvdate <= item.ToDate)
                    {
                        // checkflag = "Yes";
                        // res.Data = checkflag;
                        itperioddate = item.ToDate;

                        break;
                    }
                    //else
                    //{
                    //   itperioddate = Convert.ToDateTime("00/00/0000");
                    //    // checkflag = "No";
                    //    //res.Data = checkflag;
                    //}
                }

            }
            else
            {
                // perioddate = Convert.ToDateTime("00/00/0000");
                // res.Data = "NoLock";
            }
            //}
            //else
            //{
            //    res.Data = "Noitperiod";
            //}

            // List<Period> period = new List<Period>();
            //int periodid;
            List<SubPeriod> subperiod = new List<SubPeriod>();
            // subperiod = db.SubPeriods.ToList();
            //period = db.Periods.ToList();
            //if(period.Count!=0)
            //{
            //  foreach (Period item in period)
            // {
            //if (vdate >= item.FromDate && vdate <= item.ToDate)
            //{
            //  periodid = item.ID;
            List<SubPeriod> slist = new List<SubPeriod>();
            slist = db.SubPeriods.Where(x => x.Companyid == companyid).ToList();
            if (slist != null)
            {
                SubPeriod checkdepflag = new SubPeriod();
                checkdepflag = db.SubPeriods.Where(x => x.DepFlag == "Y" && x.FromDate <= strvdate && x.ToDate >= strvdate && x.Companyid == companyid).FirstOrDefault();
                if (checkdepflag == null)
                {
                    subperiod = db.SubPeriods.Where(x => x.PeriodLockFlag == "Y" && x.Companyid == companyid).ToList();
                    if (subperiod.Count != 0)
                    {
                        foreach (SubPeriod itemsub in subperiod)
                        {
                            if (strvdate >= itemsub.FromDate && strvdate <= itemsub.ToDate)
                            {
                                perioddate = itemsub.ToDate;
                                break;
                            }
                            //else
                            //{
                            //    perioddate = Convert.ToDateTime("00/00/0000");
                            //}
                        }
                    }
                    else
                    {
                        //perioddate = Convert.ToDateTime("00/00/0000");
                    }
                }

                else
                {
                    res.Data = "Depalreadycalculated";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                res.Data = "Nosubperiod";
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            //}
            //else
            //{
            //    //noperiod
            //    perioddate = Convert.ToDateTime("00/00/0000");
            //}
            DateTime lockdate;
            int value = DateTime.Compare(perioddate, itperioddate);

            // checking 
            if (value > 0)
            {
                lockdate = perioddate;
                // Console.Write("date1 is later than date2. ");
            }
            else if (value < 0)
            {
                lockdate = itperioddate;
                //Console.Write("date1 is earlier than date2. ");
            }

            else
            {
                lockdate = perioddate;
                //Console.Write("date1 is the same as date2. ");
            }

            if (strvdate <= lockdate)
            {
                //error
                res.Data = "Yes";
            }
            else
            {
                //nothing
                res.Data = "No";
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        //[HttpGet]
        //public DisposalViewModelWrapper GetFieldsfromAssetNew(string strddate, string disposaltype, string assetno)
        //{

        //    DisposalBL disposalBL = new DisposalBL();

        //    DisposalViewModelWrapper wrapper = new DisposalViewModelWrapper();

        //   if  (disposalBL.IsValidateDisposalEntry(strddate, disposaltype, assetno))
        //   {
        //        wrapper.IsValid = true;
        //        wrapper.model = disposalBL.GetDisposalGrossValue(assetno, strddate);
        //    }else
        //    {
        //        wrapper.IsValid = false;
        //    }
        //    return wrapper;

        //}
        [HttpGet]
        public JsonResult GetdisplayFields(DateTime strddate, string assetno)
        {

            DisposalBL disposalBL = new DisposalBL();

            DisposalViewModelWrapper wrapper = new DisposalViewModelWrapper();

            if (disposalBL.IsValidateDisposalEntry(strddate, assetno))
            {
                wrapper.IsValid = true;
                wrapper.model = disposalBL.GetDisplayValue(assetno, strddate);
            }
            else
            {
                wrapper.IsValid = false;
            }
            return Json(wrapper, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult Getdepreciationreversed(DateTime strddate, string assetno, string disposaltype)
        {

            DisposalBL disposalBL = new DisposalBL();

            DisposalViewModelWrapper wrapper = new DisposalViewModelWrapper();

            if (disposalBL.IsValidateDisposalEntry(strddate, assetno))
            {
                wrapper.IsValid = true;
                wrapper.model = disposalBL.GetDepreciationreversed(assetno, strddate, disposaltype);
            }
            else
            {
                wrapper.IsValid = false;
            }
            return Json(wrapper, JsonRequestBehavior.AllowGet);

        }

        //-------------------------Dispossl date and voucher date validation----

        //------------------------Get Gross Value( using VoucherDate & Disposal Type)---------------------------------------------
        //[HttpGet]
        //public JsonResult GetFieldsfromAsset(string strddate, string disposaltype, string assetno)
        //    {
        //    JsonResult res;
        //    res = new JsonResult();
        //    APIResponse response = new APIResponse();

        //    DisposalViewModel disposalviewmodel = new DisposalViewModel();
        //    decimal? GrossValue = 0;
        //    decimal? OpAccumulatedTillDDate = 0;
        //    decimal? OpAccumulated = 0;
        //    decimal? DepChargeFromDDateToDepToDate = 0;
        //    decimal? TotalDepreciation = 0;
        //    decimal? NetAmount = 0;
        //    decimal? DepTillDate = 0;
        //    string DepMethod;
        //    decimal? TotalRate = 0;
        //    Assets assets = new Assets();
        //    DateTime ddate = Convert.ToDateTime(strddate);

        //        if (ddate < assets.VoucherDate && ddate < assets.DtPutToUse && disposaltype != "Full")
        //        {
        //              res.Data = "Yes";


        //        //return;
        //        }
        //      else
        //        {
        //            var AssNo = db.Assetss.Where(x => x.AssetNo == assetno).FirstOrDefault().ID;
        //            assets = db.Assetss.Where(x => x.ID == AssNo).FirstOrDefault();
        //            //assets.ID = AssNo;
        //            GrossValue = assets.AmountCapitalisedCompany;
        //            OpAccumulatedTillDDate = assets.OPAccDepreciation;
        //            OpAccumulated = assets.OPAccDepreciation;
        //            DepMethod = assets.DepreciationMethod;
        //            TotalRate = assets.TotalRate;
        //            List<Addition> lstaddition = new List<Addition>();
        //             lstaddition = db.Additions.Where(x => x.AssetId == assets.ID && x.VoucherDate <= ddate).ToList();

        //           //lstaddition = db.Additions.Where(x => x.AssetId == 5).ToList();
        //            if (lstaddition.Count != 0)
        //            {

        //                foreach (var item in lstaddition)
        //                {
        //                    GrossValue += item.AmountCapitalisedCompany;
        //                   // GrossValue++;
        //                }

        //            }
        //        //----depreciatinligon------------------------
        //        //DateTime vdate = Convert.ToDateTime(strddate);
        //        List<Depreciation> lstdepreciation = new List<Depreciation>();

        //        //lstdepreciation = db.Depreciations.Where(x => x.AssetId == assets.ID && x.ToDate <= ddate).ToList();
        //        //if (lstdepreciation.Count != 0)
        //        //{

        //        //    foreach (var item in lstdepreciation)
        //        //    {

        //        //        OpAccumulatedTillDDate += item.Amount; //Asset + Depreciation (OpAccDep)                      
        //        //        OpAccumulated += item.Amount;//Only Depreciation (OpAccDep)

        //        //    }
        //        //    // OpDepreciation2 = DepOpt1 - OpDepreciation1

        //        //}
        //        //-----------------------Calculate Total days-------------------------------------------
        //        SubPeriod subperiod = new SubPeriod();
        //        var SubId = db.SubPeriods.Where(x => x.FromDate <= ddate && ddate <= x.ToDate).FirstOrDefault().ID;
        //        subperiod= db.SubPeriods.Where(x => x.ID == SubId).FirstOrDefault();
        //        DateTime DepFrom;
        //        DateTime PurDisDt;
        //        int count=0;
        //        DateTime assetDate = Convert.ToDateTime(assets.VoucherDate);

        //        if(assets.VoucherDate > subperiod.FromDate)
        //        {
        //            DepFrom = assetDate;
        //            PurDisDt = assetDate;
        //        }
        //        else
        //        {
        //            DepFrom = subperiod.FromDate;
        //            PurDisDt = subperiod.FromDate;
        //        }

        //        //count = Convert.ToInt32(ddate - DepFrom);
        //         count = Convert.ToInt32((ddate - DepFrom).TotalDays);

        //        //---------------------------------------------------------------------------
        //          DepChargeFromDDateToDepToDate = OpAccumulated - OpAccumulatedTillDDate;
        //          if(DepMethod=="SLM")
        //          {
        //            DepTillDate= GrossValue * TotalRate / 100 / 365 * count;
        //          }
        //         if (DepMethod == "WDV")
        //         {
        //            DepTillDate = (GrossValue - OpAccumulatedTillDDate) * TotalRate / 100 / 365 * count;
        //            //(BookVal1 - Dis:OpDepreciation1) *Ass:TotRate / 100 / 365 * Count
        //         }
        //           //TotalDepreciation = OpAccumulated + DepTillDate;
        //           NetAmount = GrossValue - TotalDepreciation;
        //        //------------------------------------------------------------------------
        //            disposalviewmodel.AssetAmtCapitalised = GrossValue;
        //            disposalviewmodel.OpAccDepTillDipodate = OpAccumulatedTillDDate;
        //            disposalviewmodel.OpAccDepreciation = OpAccumulated;
        //            disposalviewmodel.DepChargeFromDDateToDepToDate = DepChargeFromDDateToDepToDate;
        //            disposalviewmodel.DepMethod = DepMethod;
        //            disposalviewmodel.TotalRate = TotalRate;
        //            //disposalviewmodel.TotalDepreciation = TotalDepreciation;
        //            //disposalviewmodel.NetAmount = NetAmount;
        //            //disposalviewmodel.DepTillDate = DepTillDate;

        //        res.Data = disposalviewmodel;
        //        }

        //    // return Json(res, JsonRequestBehavior.AllowGet);
        //    return res;

        //}
        //----------------------------------------------------------------------
        //public string GetFieldsfromAsset(string strddate, string disposaltype, string assetno)
        //{
        //    string checkflag = "";
        //    JsonResult res;
        //    res = new JsonResult();
        //    DisposalViewModel disposalviewmodel = new DisposalViewModel();
        //    decimal? GrossValue = 0;
        //    decimal? OpAccumulatedTillDDate = 0;
        //    decimal? OpAccumulated = 0;
        //    decimal? DepChargeFromDDateToDepToDate = 0;
        //    decimal? TotalDepreciation = 0;
        //    decimal? NetAmount = 0;
        //    decimal? DepTillDate = 0;
        //    string DepMethod;
        //    decimal? TotalRate = 0;
        //    Assets assets = new Assets();
        //    DateTime ddate = Convert.ToDateTime(strddate);

        //    if (ddate < assets.VoucherDate && ddate < assets.DtPutToUse && disposaltype != "Full")
        //    {
        //        checkflag = "Yes";
        //        //return;
        //    }
        //    else
        //    {
        //        var AssNo = db.Assetss.Where(x => x.AssetNo == assetno).FirstOrDefault().ID;
        //        assets = db.Assetss.Where(x => x.ID == AssNo).FirstOrDefault();
        //        //assets.ID = AssNo;
        //        GrossValue = assets.AmountCapitalisedCompany;
        //        OpAccumulatedTillDDate = assets.OPAccDepreciation;
        //        OpAccumulated = assets.OPAccDepreciation;
        //        DepMethod = assets.DepreciationMethod;
        //        TotalRate = assets.TotalRate;
        //        List<Addition> lstaddition = new List<Addition>();
        //        lstaddition = db.Additions.Where(x => x.AssetId == assets.ID && x.VoucherDate <= ddate).ToList();

        //        //lstaddition = db.Additions.Where(x => x.AssetId == 5).ToList();
        //        if (lstaddition.Count != 0)
        //        {

        //            foreach (var item in lstaddition)
        //            {
        //                GrossValue += item.AmountCapitalisedCompany;
        //                // GrossValue++;
        //            }

        //        }
        //        //----depreciatinligon------------------------
        //        //DateTime vdate = Convert.ToDateTime(strddate);
        //        List<Depreciation> lstdepreciation = new List<Depreciation>();

        //        lstdepreciation = db.Depreciations.Where(x => x.AssetId == assets.ID && x.ToDate <= ddate).ToList();
        //        if (lstdepreciation.Count != 0)
        //        {

        //            foreach (var item in lstdepreciation)
        //            {

        //                OpAccumulatedTillDDate += item.Amount; //Asset + Depreciation (OpAccDep)                      
        //                OpAccumulated += item.Amount;//Only Depreciation (OpAccDep)

        //            }
        //            // OpDepreciation2 = DepOpt1 - OpDepreciation1

        //        }
        //        //-----------------------Calculate Total days-------------------------------------------
        //        SubPeriod subperiod = new SubPeriod();
        //        var SubId = db.SubPeriods.Where(x => x.FromDate <= ddate && ddate <= x.ToDate).FirstOrDefault().ID;
        //        subperiod = db.SubPeriods.Where(x => x.ID == SubId).FirstOrDefault();
        //        DateTime DepFrom;
        //        DateTime PurDisDt;
        //        int count = 0;
        //        DateTime assetDate = Convert.ToDateTime(assets.VoucherDate);

        //        if (assets.VoucherDate > subperiod.FromDate)
        //        {
        //            DepFrom = assetDate;
        //            PurDisDt = assetDate;
        //        }
        //        else
        //        {
        //            DepFrom = subperiod.FromDate;
        //            PurDisDt = subperiod.FromDate;
        //        }

        //        //count = Convert.ToInt32(ddate - DepFrom);
        //        count = Convert.ToInt32((ddate - DepFrom).TotalDays);

        //        //---------------------------------------------------------------------------
        //        DepChargeFromDDateToDepToDate = OpAccumulated - OpAccumulatedTillDDate;
        //        if (DepMethod == "SLM")
        //        {
        //            DepTillDate = GrossValue * TotalRate / 100 / 365 * count;
        //        }
        //        if (DepMethod == "WDV")
        //        {
        //            DepTillDate = (GrossValue - OpAccumulatedTillDDate) * TotalRate / 100 / 365 * count;
        //            //(BookVal1 - Dis:OpDepreciation1) *Ass:TotRate / 100 / 365 * Count
        //        }
        //        //TotalDepreciation = OpAccumulated + DepTillDate;
        //        NetAmount = GrossValue - TotalDepreciation;
        //        //------------------------------------------------------------------------
        //        disposalviewmodel.AssetAmtCapitalised = GrossValue;
        //        disposalviewmodel.OpAccDepTillDipodate = OpAccumulatedTillDDate;
        //        disposalviewmodel.OpAccDepreciation = OpAccumulated;
        //        disposalviewmodel.DepChargeFromDDateToDepToDate = DepChargeFromDDateToDepToDate;
        //        disposalviewmodel.DepMethod = DepMethod;
        //        disposalviewmodel.TotalRate = TotalRate;
        //        //disposalviewmodel.TotalDepreciation = TotalDepreciation;
        //        //disposalviewmodel.NetAmount = NetAmount;
        //        //disposalviewmodel.DepTillDate = DepTillDate;

        //        res.Data = disposalviewmodel;
        //    }

        //    //return Json(res, JsonRequestBehavior.AllowGet);
        //    return checkflag;
        //}
        //----------------------------------------------------------------------
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
            Disposal disposal = new Disposal();

            try
            {
                disposal = db.Disposals.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();



                disposal.str_voucherDate = Convert.ToDateTime(disposal.VoucherDate).ToString("yyyy-MM-dd");
                DateTime checkvoucherdate;// = Convert.ToDateTime(disposal.str_voucherDate);
                string checklockflag = "";
                if (DateTime.TryParseExact(disposal.str_voucherDate, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out checkvoucherdate))
                {
                    checklockflag = ImportDatevalidation(checkvoucherdate, companyid);

                    if (checklockflag == "Depalreadycalculated")
                    {
                        ViewBag.Lock = "Depalreadycalculated";
                    }
                    if (checklockflag == "Yes")
                    {
                        ViewBag.Lock = "Periodlock";
                    }
                    if (checklockflag == "No")
                    {
                        ViewBag.Lock = "Nolock";
                    }
                    if (checklockflag == "Nomainperiod")
                    {
                        ViewBag.Lock = "Nomainperiod";
                    }
                }
                List<Depreciation> deplist = new List<Depreciation>();
                deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == disposal.AssetId).ToList();
                if (deplist.Count() != 0)
                {
                    ViewBag.Lock = "Depalreadycalculated";
                }




                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.Companyid == companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
                //ViewBag.supplierlist = new SelectList(db.Suppliers.OrderBy(e => e.ID), "ID", "SupplierName");
                //ViewBag.uomlist = new SelectList(db.UOMs.OrderBy(e => e.ID), "ID", "Unit");
                disposal.str_billDate = disposal.BillDate.ToString("dd/MM/yyyy");
                disposal.str_voucherDate = disposal.VoucherDate.ToString("dd/MM/yyyy"); ;
                disposal.str_disposalDate = disposal.DisposalDate.ToString("dd/MM/yyyy");
                disposal.ID = disposal.ID;
                var assetno = db.Assetss.Where(x => x.ID == disposal.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                ViewBag.assetno = assetno;
                Assets asset = new Assets();
                asset = db.Assetss.Where(x => x.ID == disposal.AssetId && x.Companyid == companyid).FirstOrDefault();
                // disposal.AssetName = asset.AssetName;
                disposal.DepMethod = asset.DepreciationMethod;
                disposal.DepRate = asset.TotalRate;

                if (disposal.str_billDate == "01/01/0001")
                {
                    disposal.str_billDate = "";
                }
                if (disposal.str_voucherDate == "01/01/0001")
                {
                    disposal.str_voucherDate = "";
                }
                if (disposal.str_disposalDate == "01/01/0001")
                {
                    disposal.str_disposalDate = "";
                }

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            return View(disposal);

        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Edit(Disposal disposal)
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

            //-----------------------------------------------------------------------
            Disposal objAcc = new Disposal();
            objAcc.DisposalCategory = disposal.DisposalCategory;
            if (objAcc.DisposalCategory == "1")
            {
                disposal.DisposalCategory = "Sold";

            }
            else if (objAcc.DisposalCategory == "2")
            {
                disposal.DisposalCategory = "Scraped";

            }
            else if (objAcc.DisposalCategory == "3")
            {
                disposal.DisposalCategory = "Stolen";

            }
            else if (objAcc.DisposalCategory == "4")
            {
                disposal.DisposalCategory = "Lost";

            }
            else if (objAcc.DisposalCategory == "5")
            {
                disposal.DisposalCategory = "Replace";

            }
            else if (objAcc.DisposalCategory == "6")
            {
                disposal.DisposalCategory = "Others";

            }
            //---------------------------------------------------------------------


            JsonResult res;
            res = new JsonResult();


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            try
            {
                if (disposal.AssetNo != "")
                {
                    var assetid = db.Assetss.Where(x => x.AssetNo == disposal.AssetNo).FirstOrDefault().ID;
                    disposal.AssetId = assetid;
                }
                disposal.Modified_Userid = userid;
                disposal.ModifiedDate = istDate;
                disposal.Companyid = companyid;
                db.Entry(disposal).State = System.Data.Entity.EntityState.Modified;
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

        [HttpPost]
        public ActionResult DisposalExport()
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

            //Response.ClearContent();
            //Response.BinaryWrite(generateDisposalsexcel(companyid));
            //string excelName = "DisposalAssets";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();
            //return RedirectToAction("Index", "Disposal");  List<Disposal> lstins = new List<Disposal>();
            int srno = 1;

            List<Disposal> lstins = new List<Disposal>();
            lstins = db.Disposals.Where(x => x.Companyid == companyid).ToList();

            foreach (var item in lstins)
            {
                item.int_Srno = srno;
                item.str_billDate = item.BillDate.ToString("dd/MM/yyyy");
                item.str_voucherDate = item.VoucherDate.ToString("dd/MM/yyyy"); ;
                item.str_disposalDate = item.DisposalDate.ToString("dd/MM/yyyy");

                srno++;
            }

            var memoryStream = new MemoryStream();
            byte[] data;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[] { "Sr No","ID ","Asset No","Asset Name", "DisposalDate", "VoucherNo", "Voucher Date","Bill Date ",
                                     "Disposal Type","Qty","Disposal Amount","Asset Category"


                };



                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                //  worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                var excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                int rowIterator = 2;
                foreach (var item in lstins)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.int_Srno;
                    worksheet.Cells[rowIterator, 2].Value = item.ID;
                    item.AssetNo = db.Assetss.Where(x => x.ID == item.AssetId).FirstOrDefault().AssetNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 4].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 5].Value = item.str_disposalDate;
                    worksheet.Cells[rowIterator, 6].Value = item.VoucherNo;
                    worksheet.Cells[rowIterator, 7].Value = item.str_voucherDate;
                    worksheet.Cells[rowIterator, 8].Value = item.str_billDate;
                    worksheet.Cells[rowIterator, 9].Value = item.DisposalType;
                    worksheet.Cells[rowIterator, 10].Value = item.Qty;
                    worksheet.Cells[rowIterator, 11].Value = item.DisposalAmount;
                    worksheet.Cells[rowIterator, 12].Value = item.DisposalCategory;
                    //worksheet.Cells[rowIterator, 12].Value = item.GrossAmount;
                    //worksheet.Cells[rowIterator, 13].Value = item.OpAccumulatedDep;
                    //worksheet.Cells[rowIterator, 14].Value = item.TotalDepreciation;
                    //worksheet.Cells[rowIterator, 15].Value = item.WDvDisposedOff;
                    //worksheet.Cells[rowIterator, 16].Value = item.ProfitLoss;


                    rowIterator = rowIterator + 1;

                }
                string excelName = "DisposalExport.xlsx";

                string handle = Guid.NewGuid().ToString();
                excel.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();

                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = excelName }
                };



            }



        }
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        public byte[] generateDisposalsexcel(int companyid)
        {
            List<Disposal> lstins = new List<Disposal>();
            int srno = 1;


            lstins = db.Disposals.Where(x => x.Companyid == companyid).ToList();

            foreach (var item in lstins)
            {
                item.int_Srno = srno;
                item.str_billDate = item.BillDate.ToString("dd/MM/yyyy");
                item.str_voucherDate = item.VoucherDate.ToString("dd/MM/yyyy"); ;
                item.str_disposalDate = item.DisposalDate.ToString("dd/MM/yyyy");

                srno++;
            }

            var memoryStream = new MemoryStream();
            byte[] data;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[] { "Sr No","ID ","Asset No","Asset Name", "DisposalDate", "VoucherNo", "Voucher Date","Bill Date ",
                                     "Disposal Type","Qty","Disposal Amount","Asset Category"


                };



                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                //  worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                var excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                int rowIterator = 2;
                foreach (var item in lstins)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.int_Srno;
                    worksheet.Cells[rowIterator, 2].Value = item.ID;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 4].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 5].Value = item.str_disposalDate;
                    worksheet.Cells[rowIterator, 6].Value = item.VoucherNo;
                    worksheet.Cells[rowIterator, 7].Value = item.str_voucherDate;
                    worksheet.Cells[rowIterator, 8].Value = item.str_billDate;
                    worksheet.Cells[rowIterator, 9].Value = item.DisposalType;
                    worksheet.Cells[rowIterator, 10].Value = item.Qty;
                    worksheet.Cells[rowIterator, 11].Value = item.DisposalAmount;
                    worksheet.Cells[rowIterator, 12].Value = item.DisposalCategory;
                    //worksheet.Cells[rowIterator, 12].Value = item.GrossAmount;
                    //worksheet.Cells[rowIterator, 13].Value = item.OpAccumulatedDep;
                    //worksheet.Cells[rowIterator, 14].Value = item.TotalDepreciation;
                    //worksheet.Cells[rowIterator, 15].Value = item.WDvDisposedOff;
                    //worksheet.Cells[rowIterator, 16].Value = item.ProfitLoss;


                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        [AuthUser]
        [HttpGet]
        public ActionResult DownloadDisposalExcel()
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

            Response.ClearContent();
            Response.BinaryWrite(generateImportdisposalexcel());
            string excelName = "DisposalAssets";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Disposal");

        }

        public byte[] generateImportdisposalexcel()
        {

            int srno = 1;



            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[]

                   { "Sr No","Asset No","Asset Name", "DisposalDate", "VoucherNo", "Voucher Date","Bill Date ",
                                     "Disposal Type","Qty","Disposal Amount","Disposal Depreciation","Asset Category"

            };


                // Determine the header range (e.g. A1:D1)


                //  string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                // string headerRange1 = "B2:"+ addition;

                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                var excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                // var addition = db.Additions.Max(x => x.AdditionNo == 0 ? 0 : x.AdditionNo);
                //if (addition == null)
                //{
                //    addition = 1;
                //}
                //else
                //{
                //    addition = addition + 1;
                //}

                //worksheet.Cells[2, 2].Value = addition;

                return excel.GetAsByteArray();

            }
        }

        [AuthUser]
        [HttpGet]
        public ActionResult ImportExcel()
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
            return PartialView();
        }

        [HttpPost]
        [ValidateAjax]
        public ActionResult UploadDisposal()
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

            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            Disposal disposal = new Disposal();
            DbContextTransaction transaction = db.Database.BeginTransaction();
            try
            {
                if (Request != null)
                {
                    bool norecordsfound = false;

                    HttpPostedFileBase file;


                    file = null;
                    HttpFileCollectionBase files = Request.Files;
                    List<string> errorlist = new List<string>();



                    if (files.Count > 0)
                    {
                        file = files[0];
                        string fileName = file.FileName;

                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        Stream stream = file.InputStream;
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                            var tnow = System.DateTime.Now.ToUniversalTime();
                            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                // string additionno = "";
                                string int_srno = "";
                                string AssetNo = "";
                                string AssetName = "";
                                string DisposalDate = "";
                                string VoucherNo = "";
                                string VoucherDate = "";
                                string BillDate = "";
                                string DisposalType = "";
                                string Qty = "";
                                string DisposalAmount = "";
                                string GrossValue = "";
                                string OPAccDepreciation = "";
                                string AssetCategory = "";
                                string DisposalDepreciation = "";




                                bool int_srnoflag;
                                bool AssetNoflag = false;
                                bool AssetNameflag = false;
                                bool DisposalDateflag = false;
                                bool VoucherNoflag = false;
                                bool VoucherDateflag = false;
                                bool BillDateflag = false;
                                bool DisposalTypeflag = false;
                                bool Qtyflag = false;
                                bool DisposalAmountflag = false;
                                bool GrossValueflag = false;
                                bool OPAccDepreciationflag = false;
                                bool AssetCategoyflag = false;
                                bool DisposalDepreciationflag = false;

                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    int_srno = "";
                                    int_srnoflag = false;
                                }
                                else
                                {
                                    int_srno = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    int_srnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    AssetNo = "";
                                    AssetNoflag = false;
                                }
                                else
                                {
                                    AssetNo = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    AssetNoflag = true;
                                }





                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    AssetName = "";
                                    AssetNameflag = false;
                                }
                                else
                                {
                                    AssetName = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    AssetNameflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    DisposalDate = "";
                                    DisposalDateflag = false;
                                }
                                else
                                {
                                    DisposalDate = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    DisposalDateflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    VoucherNo = "";
                                    VoucherNoflag = false;
                                }
                                else
                                {
                                    VoucherNo = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    VoucherNoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 6].Text == "")
                                {
                                    VoucherDate = "";
                                    VoucherDateflag = false;
                                }
                                else
                                {
                                    VoucherDate = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    VoucherDateflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 7].Text == "")
                                {
                                    BillDate = "";
                                    BillDateflag = false;
                                }
                                else
                                {
                                    BillDate = workSheet.Cells[rowIterator, 7].Value.ToString();
                                    BillDateflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 8].Text == "")
                                {
                                    DisposalType = "";
                                    DisposalTypeflag = false;

                                }
                                else
                                {

                                    DisposalType = workSheet.Cells[rowIterator, 8].Value.ToString();
                                    DisposalTypeflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 9].Text == "0" || workSheet.Cells[rowIterator, 9].Text.Trim().Length <=0)
                                {
                                    Qty = "";
                                    Qtyflag = false;
                                }
                                else
                                {
                                    Qty = workSheet.Cells[rowIterator, 9].Value.ToString();
                                    Qtyflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 10].Text == "")
                                {
                                    DisposalAmount = "";
                                    DisposalAmountflag = false;
                                }
                                else
                                {
                                    DisposalAmount = workSheet.Cells[rowIterator, 10].Value.ToString();
                                    DisposalAmountflag = true;

                                }



                                if (workSheet.Cells[rowIterator, 11].Text == "")
                                {
                                    DisposalDepreciation = "";
                                    DisposalDepreciationflag = false;
                                }
                                else
                                {
                                    DisposalDepreciation = workSheet.Cells[rowIterator, 11].Value.ToString();
                                    DisposalDepreciationflag = true;

                                }

                                

                                if (workSheet.Cells[rowIterator, 12].Text == "")
                                {
                                    AssetCategory = "";
                                    AssetCategoyflag = false;
                                }
                                else
                                {
                                    AssetCategory = workSheet.Cells[rowIterator, 12].Value.ToString();
                                    AssetCategoyflag = true;
                                }

                                //if (workSheet.Cells[rowIterator, 12].Text == "")
                                //{
                                //    OPAccDepreciation = "";
                                //    OPAccDepreciationflag = false;
                                //}
                                //else
                                //{
                                //    OPAccDepreciation = workSheet.Cells[rowIterator, 12].Value.ToString();
                                //    OPAccDepreciationflag = true;
                                //}


                                if (AssetNoflag == true && VoucherDateflag == true && Qtyflag == true && DisposalDateflag == true 
                                    && DisposalTypeflag == true && AssetCategoyflag == true)
                                {

                                   // System.Diagnostics.Debug.WriteLine("Asset No = " + AssetNo);
                                    norecordsfound = true;
                                    //Addition additioncheck = new Addition();
                                    //int additionnumbercheck = Convert.ToInt32(additionno);
                                    //additioncheck = db.Additions.Where(r => r.AdditionNo == additionnumbercheck).FirstOrDefault();
                                    //if (disposalcheck != null)
                                    //{
                                    //    errorlist.Add("Addition no record already exists in master,i.e of  row " + rowIterator);
                                    //    res.Data = errorlist;

                                    //}
                                    //else
                                    //{
                                    if (BillDate != "")
                                    {
                                        DateTime billdate1;//= Convert.ToDateTime(BillDate);
                                                           //  disposal.BillDate = billdate1;
                                        string Str_billdate1 = Convert.ToDateTime(VoucherDate).ToString("yyyy-MM-dd");
                                        if (DateTime.TryParseExact(Str_billdate1, "yyyy-MM-dd",
                                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out billdate1))
                                        {
                                            disposal.BillDate = billdate1;
                                        }
                                    }
                                    DateTime disposaldate;
                                    if (DisposalDate != "")
                                    {
                                        //= Convert.ToDateTime(DisposalDate);
                                        // disposal.DisposalDate = disposaldate;
                                        string Str_disposaldate = Convert.ToDateTime(DisposalDate).ToString("yyyy-MM-dd");
                                        if (DateTime.TryParseExact(Str_disposaldate, "yyyy-MM-dd",
                                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out disposaldate))
                                        {
                                            disposal.DisposalDate = disposaldate;
                                        }
                                    }
                                    if (VoucherDate != "")
                                    {
                                        DateTime vDate;// = Convert.ToDateTime(VoucherDate);
                                                       // disposal.VoucherDate = vDate;
                                        string Str_VoucherDate = Convert.ToDateTime(VoucherDate).ToString("yyyy-MM-dd");
                                        if (DateTime.TryParseExact(Str_VoucherDate, "yyyy-MM-dd",
                                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out vDate))
                                        {
                                            disposal.VoucherDate = vDate;
                                        }
                                    }

                                    //  DateTime voucherdate = Convert.ToDateTime(VoucherDate);
                                    //DateTime disosaldate = Convert.ToDateTime(DisposalDate);
                                    //DateTime billdate = Convert.ToDateTime(BillDate);
                                    //disposal.int_Srno = Convert.ToInt32(int_srno);
                                    // addition.AdditionNo = Convert.ToInt32(additionno);
                                    //// validation for additionasset voucher less than asset voucher date----

                                    var str_checkvoucherdate = stringreturn_validateassetdate(disposal.VoucherDate, AssetNo, disposal.DisposalDate);
                                    if (str_checkvoucherdate == "AssetYes")
                                    {
                                        //errorlist.Add("For voucher date cannot be less than asset voucher date. For row" + rowIterator);
                                        continue;
                                    }
                                    if (str_checkvoucherdate == "ChkDate")
                                    {
                                        //errorlist.Add("For Disposal date cannot be less than asset voucher date. For row" + rowIterator);
                                        continue;
                                    }
                                    if (str_checkvoucherdate == "ChkDDate")
                                    {
                                        //errorlist.Add(" Disposal date cannot be less than Disposal voucher date. For row" + rowIterator);
                                        continue;
                                    }
                                    //validation for voucher date and period----
                                    var checkvoucherdate = ImportDatevalidation(disposal.VoucherDate);
                                    if (checkvoucherdate == "Yes")
                                    {
                                        // errorlist.Add("For voucher date period is lock you cannot add asset. For row" + rowIterator);
                                        continue;
                                    }
                                    if (checkvoucherdate == "No")
                                    {
                                        disposal.VoucherDate = disposal.VoucherDate;

                                    }
                                    if (checkvoucherdate == "Nomainperiod")
                                    {
                                        //errorlist.Add("For voucher date No current  financial period found. For row" + rowIterator);
                                        continue;
                                    }
                                    if (checkvoucherdate == "Depalreadycalculated")
                                    {
                                        //errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                        continue;
                                    }
                                    //--------------------------------chk disposal date validation----------
                                    var checkdisposaldate = ImportDatevalidation(disposal.DisposalDate);
                                    if (checkdisposaldate == "Yes")
                                    {
                                        //errorlist.Add("For disposal date period is lock you cannot add asset. For row" + rowIterator);
                                        continue;
                                    }
                                    if (checkdisposaldate == "No")
                                    {
                                        disposal.DisposalDate = disposal.DisposalDate;

                                    }
                                    if (checkdisposaldate == "Nomainperiod")
                                    {
                                        ///errorlist.Add("For disposal date No current  financial period found. For row" + rowIterator);
                                        continue;
                                    }
                                    if (checkdisposaldate == "Depalreadycalculated")
                                    {
                                       // errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                        continue;
                                    }
                                    //----------------------------------------------------------------------
                                    JsonResult res1;
                                    res1 = new JsonResult();

                                    //   var res2 = GetFieldsfromAsset(DisposalDate, DisposalType, AssetNo);
                                    DisposalBL disposalBL = new DisposalBL();
                                    DisposalViewModelWrapper wrapper = new DisposalViewModelWrapper();
                                    DateTime Ddate = DateTime.MinValue;
                                    //if (DateTime.TryParseExact(DisposalDate, "yyyy-MM-dd",
                                    //                               System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Ddate))
                                    //{

                                    //}

                                    ////--new code shubhangi----
                                    Ddate = Convert.ToDateTime(DisposalDate);

                                    //if (DisposalDate!=null)
                                    //{
                                    //    DateTime.TryParseExact(DisposalDate, "dd/MM/yyyy", new CultureInfo("en-IN"), DateTimeStyles.None, out Ddate);
                                    //}
                                    ////--------------------------
                                    //if (disposalBL.IsValidateDisposalEntry(Ddate, AssetNo) == true)
                                    //{
                                    //    wrapper.model = disposalBL.GetDisplayValue(AssetNo, Ddate);
                                    //}
                                    //else
                                    //{
                                    //    // this is invalid
                                    //    // errorlist.Add("Disposal date cannot be less than asset voucher date and DateputtoUse " + rowIterator);
                                    //    continue;
                                    //}

                                    // above block commented by Mandar 31 DEC 2021 wrapper.model wheere is this used?




                                    //--------------------------------------------------------------------------
                                    disposal.int_Srno = Convert.ToInt32(int_srno);

                                    var assetname = db.Assetss.Where(x => x.AssetNo == AssetNo).FirstOrDefault().AssetName;
                                    disposal.AssetName = assetname;
                                    disposal.AssetId = db.Assetss.Where(x => x.AssetNo == AssetNo).FirstOrDefault().ID;

                                    disposal.VoucherNo = VoucherNo;
                                    disposal.VoucherDate = disposal.VoucherDate;
                                    disposal.DisposalDate = disposal.DisposalDate;
                                    disposal.DisposalType = DisposalType;
                                    //disposal amount logic
                                    //var disposalamount=disposalBL.GetDepreciationreversed(AssetNo, disposal.DisposalDate, disposal.DisposalType);
                                    //disposal.DisposalAmount =Convert.ToDecimal(disposalamount.depreciationreversed);
                                    disposal.DisposalAmount = Convert.ToDecimal(DisposalAmount);
                                    disposal.DisposalCategory = disposal.DisposalCategory;
                                    /////
                                    //GrossValue = (wrapper.model.AssetAmtCapitalised).ToString();
                                    //disposal.GrossAmount = ToDecimal(GrossValue);
                                    //OPAccDepreciation = (wrapper.model.depreciationtilldate).ToString();
                                    //disposal.OpAccumulatedDep = ToDecimal(OPAccDepreciation);
                                    //checking if disposal amount is greater or less

                                    //if (wrapper.model.disposaltilldate == 0)
                                    //{
                                    //    if (disposal.GrossAmount < disposal.DisposalAmount)
                                    //    {
                                    //        errorlist.Add("Disposal gross block cannot be greater than asset gross block" + rowIterator);
                                    //        continue;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    if (wrapper.model.disposaltilldate < disposal.DisposalAmount)
                                    //    {
                                    //        errorlist.Add("Disposal gross block cannot be greater than disposal till date block" + rowIterator);
                                    //        continue;
                                    //    }
                                    //}
                                    ///

                                    disposal.BillDate = disposal.BillDate;
                                    //  disposal.BillNo = BillNo;
                                    // disposal.DisposalType = DisposalType;
                                    disposal.Qty = Convert.ToInt32(Qty);
                                    //  disposal.DisposalAmount = ToDecimal(DisposalAmount);

                                    if (DisposalDepreciation.Length > 0)
                                    {
                                        try
                                        {
                                            disposal.OpAccumulatedDep = Convert.ToDecimal(DisposalDepreciation);
                                        }
                                        catch(Exception ex)
                                        {
                                            disposal.OpAccumulatedDep = 0;
                                        }

                                    }






                                    disposal.CreatedUserId = userid;
                                    disposal.CreatedDate = istDate;
                                    disposal.Companyid = companyid;
                                    db.Disposals.Add(disposal);
                                    db.SaveChanges();
                                    //------------------update Asset Table--------------------------------------
                                    Assets objAsset = new Assets();
                                    var AssetId = db.Assetss.Where(x => x.AssetNo == AssetNo).FirstOrDefault().ID;
                                    objAsset = db.Assetss.Where(x => x.ID == AssetId).FirstOrDefault();
                                    //objAsset.ID = AssetId;
                                    if (objAsset.ID == disposal.AssetId && disposal.DisposalType == "Full")
                                    {
                                        objAsset.DisposalFlag = 1;
                                        db.Entry(objAsset).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();

                                    }
                                    ////------------------Update Depreciation table------------------------------------
                                    //Depreciation objDep = new Depreciation();

                                    ////------------------------------------------------------

                                    ////}
                                }
                                else
                                {
                                    errorlist.Add("Something  went wrong or some value missing in the row  " + rowIterator);

                                }


                            }
                            transaction.Commit();

                            if (norecordsfound == false)
                            {

                                res.Data = "nodata";
                                return res;
                             

                            }
                            if (errorlist.Count == 0)
                            {
                                res.Data = "Success";
                                return res;

                            }
                            else
                            {
                                res.Data = errorlist;
                                return res;



                            }

                        }


                        
                    }
                    else
                    {
                        res.Data = "error";
                        return res;

                    }
                }
                else
                {
                    res.Data = "error";
                    return res;


                }
                

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                //  logger.Log(LogLevel.Error, strError);
                res.Data = "error";
                return res;
            }
        }

        //----------------------------------------------------
        public string ImportDatevalidation(DateTime date)
        {

            //i have used res.Data=yess for error showing error and res.data=no for no errors
            string checkflag = "";
            /* Commnted bY Mandar 15 FEB 2021
           JsonResult res;
           res = new JsonResult();
           DateTime itperioddate = Convert.ToDateTime("01/01/0001");
           DateTime perioddate = Convert.ToDateTime("01/01/0001");
           List<ITPeriod> itperiod = new List<ITPeriod>();

           // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
           itperiod = db.ITPeriods.ToList();
           // string checkflag = "";
           // DateTime vdate = Convert.ToDateTime(strvdate);
           //if (itperiod.Count!=0)
           //{
           List<ITPeriod> itperiodlock = new List<ITPeriod>();
           itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1).ToList();
           if (itperiodlock.Count != 0)
           {
               foreach (ITPeriod item in itperiodlock)
               {
                   if (date >= item.FromDate && date <= item.ToDate)
                   {
                       // checkflag = "Yes";
                       // res.Data = checkflag;
                       itperioddate = item.ToDate;

                       break;
                   }
                   //else
                   //{
                   //   itperioddate = Convert.ToDateTime("00/00/0000");
                   //    // checkflag = "No";
                   //    //res.Data = checkflag;
                   //}
               }

           }
           else
           {
               // perioddate = Convert.ToDateTime("00/00/0000");
               // res.Data = "NoLock";
           }
           //}
           //else
           //{
           //    res.Data = "Noitperiod";
           //}

           List<Period> period = new List<Period>();
           int periodid;
           List<SubPeriod> subperiod = new List<SubPeriod>();
           // subperiod = db.SubPeriods.ToList();
           period = db.Periods.ToList();
           //if(period.Count!=0)
           //{
           foreach (Period item in period)
           {
               if (date >= item.FromDate && date <= item.ToDate)
               {
                   periodid = item.ID;
                   SubPeriod checkdepflag = new SubPeriod();
                   checkdepflag = db.SubPeriods.Where(x => x.PeriodID == periodid && x.DepFlag == "Y" && x.FromDate <= date && x.ToDate >= date).FirstOrDefault();
                   if (checkdepflag == null)
                   {
                       subperiod = db.SubPeriods.Where(x => x.PeriodID == periodid && x.PeriodLockFlag == "Y").ToList();
                       if (subperiod.Count != 0)
                       {
                           foreach (SubPeriod itemsub in subperiod)
                           {
                               if (date >= item.FromDate && date <= item.ToDate)
                               {
                                   perioddate = item.ToDate;
                                   break;
                               }
                               //else
                               //{
                               //    perioddate = Convert.ToDateTime("00/00/0000");
                               //}
                           }
                       }
                       else
                       {
                           //perioddate = Convert.ToDateTime("00/00/0000");
                       }
                   }
                   else
                   {
                       checkflag = "Depalreadycalculated";
                       return checkflag;
                   }
               }
               else
               {
                   checkflag = "Nomainperiod";
                   return checkflag;
               }
           }
           //}
           //else
           //{
           //    //noperiod
           //    perioddate = Convert.ToDateTime("00/00/0000");
           //}
           DateTime lockdate;
           int value = DateTime.Compare(perioddate, itperioddate);

           // checking 
           if (value > 0)
           {
               lockdate = perioddate;
               // Console.Write("date1 is later than date2. ");
           }
           else if (value < 0)
           {
               lockdate = itperioddate;
               //Console.Write("date1 is earlier than date2. ");
           }

           else
           {
               lockdate = perioddate;
               //Console.Write("date1 is the same as date2. ");
           }

           if (date <= lockdate)
           {
               //error
               checkflag = "Yes";
           }
           else
           {
               //nothing
               checkflag = "No";
           }
           */

           return checkflag;

       }
       //-------------------------------------------------------
       public string stringreturn_validateassetdate(DateTime vdate, string assetno, DateTime ddate)
       {
            string checkflag = "";
            /*
            JsonResult res;
           res = new JsonResult();
           // int int_assetno = Convert.ToInt32(assetno);
           Assets assets = new Assets();
           

           // DateTime vdate = Convert.ToDateTime(strvdate);
           assets = db.Assetss.Where(x => x.AssetNo == assetno).FirstOrDefault();
           if (assets != null)
           {
               if (vdate < assets.VoucherDate)
               {
                   checkflag = "AssetYes";
                   //return;
               }
               if (ddate < assets.VoucherDate)
               {
                   checkflag = "ChkDate";

               }
               if (ddate < vdate)
               {
                   checkflag = "ChkDDate";

               }


           }
           else
           {
               checkflag = "Noassetfound";
           }
           */
           return checkflag;
       }

       public static Decimal ToDecimal(string number)
       {
           if (number.Length > 0)
           {
               return Convert.ToDecimal(number);
           }
           else
           {
               return 0;
           }
       }
       public string ImportDatevalidation(DateTime date, int companyid)
       {
           string checkflag = "";

           /* Commnted bY Mandar 15 FEB 2021

           //i have used res.Data=yess for error showing error and res.data=no for no errors

           JsonResult res;
           res = new JsonResult();
           DateTime itperioddate = Convert.ToDateTime("01/01/0001");
           DateTime perioddate = Convert.ToDateTime("01/01/0001");
           List<ITPeriod> itperiod = new List<ITPeriod>();

           // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
           itperiod = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
           // string checkflag = "";
           // DateTime vdate = Convert.ToDateTime(strvdate);
           //if (itperiod.Count!=0)
           //{
           List<ITPeriod> itperiodlock = new List<ITPeriod>();
           itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1 && x.Companyid == companyid).ToList();
           if (itperiodlock.Count != 0)
           {
               foreach (ITPeriod item in itperiodlock)
               {
                   if (date >= item.FromDate && date <= item.ToDate)
                   {
                       // checkflag = "Yes";
                       // res.Data = checkflag;
                       itperioddate = item.ToDate;

                       break;
                   }
                   //else
                   //{
                   //   itperioddate = Convert.ToDateTime("00/00/0000");
                   //    // checkflag = "No";
                   //    //res.Data = checkflag;
                   //}
               }

           }
           else
           {
               // perioddate = Convert.ToDateTime("00/00/0000");
               // res.Data = "NoLock";
           }
           //}
           //else
           //{
           //    res.Data = "Noitperiod";
           //}
           List<SubPeriod> subperiod = new List<SubPeriod>();
           // subperiod = db.SubPeriods.ToList();
           //period = db.Periods.ToList();
           //if(period.Count!=0)
           //{
           //  foreach (Period item in period)
           // {
           //if (vdate >= item.FromDate && vdate <= item.ToDate)
           //{
           //  periodid = item.ID;
           List<SubPeriod> slist = new List<SubPeriod>();
           slist = db.SubPeriods.Where(x => x.Companyid == companyid).ToList();
           if (slist != null)
           {
               SubPeriod checkdepflag = new SubPeriod();
               checkdepflag = db.SubPeriods.Where(x => x.DepFlag == "Y" && x.FromDate <= date && x.ToDate >= date && x.Companyid == companyid).FirstOrDefault();
               if (checkdepflag == null)
               {
                   subperiod = db.SubPeriods.Where(x => x.PeriodLockFlag == "Y" && x.Companyid == companyid).ToList();
                   if (subperiod.Count != 0)
                   {
                       foreach (SubPeriod itemsub in subperiod)
                       {
                           if (date >= itemsub.FromDate && date <= itemsub.ToDate)
                           {
                               perioddate = itemsub.ToDate;
                               break;
                           }
                           //else
                           //{
                           //    perioddate = Convert.ToDateTime("00/00/0000");
                           //}
                       }
                   }
                   else
                   {
                       //perioddate = Convert.ToDateTime("00/00/0000");
                   }
               }

               else
               {
                   checkflag = "Depalreadycalculated";
                   return checkflag;
               }
           }
           else
           {
               checkflag = "Nosubperiod";
               return checkflag;
           }


           //}
           //else
           //{
           //    //noperiod
           //    perioddate = Convert.ToDateTime("00/00/0000");
           //}
           DateTime lockdate;
           int value = DateTime.Compare(perioddate, itperioddate);

           // checking 
           if (value > 0)
           {
               lockdate = perioddate;
               // Console.Write("date1 is later than date2. ");
           }
           else if (value < 0)
           {
               lockdate = itperioddate;
               //Console.Write("date1 is earlier than date2. ");
           }

           else
           {
               lockdate = perioddate;
               //Console.Write("date1 is the same as date2. ");
           }

           if (date <= lockdate)
           {
               //error
               checkflag = "Yes";
           }
           else
           {
               //nothing
               checkflag = "No";
           }
           */
            checkflag = "";
            return checkflag;

        }
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
            Disposal disobj;
            List<Disposal> dispo = new List<Disposal>();
            dispo = db.Disposals.Where(x => x.ID == id && x.Companyid == companyid).ToList();

            try
            {
                if (dispo.Count != 0)
                {

                    disobj = db.Disposals.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(disobj).State = System.Data.Entity.EntityState.Deleted;
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