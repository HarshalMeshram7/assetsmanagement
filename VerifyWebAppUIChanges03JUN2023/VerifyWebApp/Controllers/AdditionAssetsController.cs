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

namespace VerifyWebApp.Controllers
{
    public class AdditionAssetsController : Controller
    {
        public VerifyDB db = new VerifyDB();


        // GET: Periostr
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
            List<Addition> lstadd = new List<Addition>();
            int srno = 1;
            try
            {

                lstadd = db.Additions.Where(x=>x.Companyid==companyid).ToList();

                foreach (Addition item in lstadd)
                {
                    item.int_Srno = srno;


                    item.str_BillDate = item.BillDate.ToString("dd/MM/yyyy");
                    item.str_CommissioningDate = item.CommissioningDate.ToString("dd/MM/yyyy"); ;
                    item.str_DtPutToUse = item.DtPutToUse.ToString("dd/MM/yyyy");
                    item.str_DtPutToUseIT = item.DtPutToUseIT.ToString("dd/MM/yyyy"); ;
                    item.str_PODate = item.PODate.ToString("dd/MM/yyyy");
                    item.str_VoucherDate = item.VoucherDate.ToString("dd/MM/yyyy");
                    item.str_ReceiptDate = item.ReceiptDate.ToString("dd/MM/yyyy");
                    if (item.SupplierNo == 0)
                    {
                        item.str_suppliername = "";
                    }
                    else
                    {
                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid==companyid).FirstOrDefault().SupplierName;
                    }
                    if (item.uom == 0)
                    {
                        item.uom_name = "";
                    }
                    else
                    {
                        item.uom_name = db.UOMs.Where(x => x.ID == item.uom && x.Companyid==companyid).FirstOrDefault().Unit;
                    }
                    if (item.DtPutToUse == null)
                    {
                        item.str_DtPutToUse = "";
                    }
                    else
                    {


                        item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                    }
                    if (item.DtPutToUseIT == null)
                    {
                        item.str_DtPutToUseIT = "";
                    }
                    else
                    {

                        item.str_DtPutToUseIT = Convert.ToDateTime(item.DtPutToUseIT).ToString("dd/MM/yyyy");

                    }
                    if (item.CommissioningDate == null)
                    {
                        item.str_CommissioningDate = "";
                    }
                    else
                    {

                        item.str_CommissioningDate = Convert.ToDateTime(item.CommissioningDate).ToString("dd/MM/yyyy");

                    }
                    if (item.ReceiptDate == null)
                    {
                        item.str_ReceiptDate = "";
                    }
                    else
                    {

                        item.str_ReceiptDate = Convert.ToDateTime(item.ReceiptDate).ToString("dd/MM/yyyy");

                    }
                    if (item.BillDate == null)
                    {
                        item.str_BillDate = "";
                    }
                    else
                    {

                        item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                    }
                    if (item.PODate == null)
                    {
                        item.str_PODate = "";
                    }
                    else
                    {

                        item.str_PODate = Convert.ToDateTime(item.PODate).ToString("dd/MM/yyyy");

                    }
                    if (item.VoucherDate == null)
                    {
                        item.str_VoucherDate = "";
                    }
                    else
                    {

                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                    }

                    if (item.str_DtPutToUseIT == "01/01/0001")
                    {
                        item.str_DtPutToUseIT = "";
                    }
                    if (item.str_PODate == "01/01/0001")
                    {
                        item.str_PODate = "";
                    }
                    if (item.str_ReceiptDate == "01/01/0001")
                    {
                        item.str_ReceiptDate = "";
                    }
                    if (item.str_CommissioningDate == "01/01/0001")
                    {
                        item.str_CommissioningDate = "";
                    }
                    if (item.str_BillDate == "01/01/0001")
                    {
                        item.str_BillDate = "";
                    }
                    if (item.AssetId != null)
                    {
                        item.AssetNo = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid==companyid).FirstOrDefault().AssetNo;
                    }

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

            return View(lstadd);
        }


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

            // Addition addition = new Addition();

            var addition = db.Additions.DefaultIfEmpty().Max(x => x.AdditionNo == null && x.Companyid==companyid ? 0 : x.ID);
            //   Addition addition = new Addition();
            //  addition.AdditionNo = db.Additions.OrderByDescending(a => a.AdditionNo).FirstOrDefault().AdditionNo;
            if (addition == 0)
            {
                ViewBag.additionno = 1;
            }
            else
            {
                ViewBag.additionno = addition + 1;
            }
            ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid==companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
            ViewBag.supplierlist = new SelectList(db.Suppliers.Where(x=>x.Companyid==companyid).OrderBy(e => e.ID), "ID", "SupplierName");
            ViewBag.uomlist = new SelectList(db.UOMs.Where(x=>x.Companyid==companyid).OrderBy(e => e.ID), "ID", "Unit");


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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            JsonResult res;
            res = new JsonResult();
            // int int_id = Convert.ToInt32(id);

            List<Assets> list = new List<Assets>();


            list = db.Assetss.Where(x => x.AssetNo == id && x.Companyid==companyid && x.DisposalFlag==0).ToList();
            res.Data = list;
            return Json(res, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public ActionResult validateassetdate(string strvdate, string assetno)
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
            // int int_assetno = Convert.ToInt32(assetno);
            Assets assets = new Assets();

            DateTime vdate = Convert.ToDateTime(strvdate);
            assets = db.Assetss.Where(x => x.AssetNo == assetno && x.Companyid==companyid).FirstOrDefault();
            if (assets != null)
            {
                if (vdate < assets.VoucherDate)
                {
                    res.Data = "AssetYes";
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //return;
                }

            }
            else
            {
                res.Data = "Noassetfound";
            }
            //List<Period> plist = new List<Period>();
            //plist = db.Periods.ToList();
            //List<ITPeriod> itplist = new List<ITPeriod>();
            //itplist = db.ITPeriods.ToList();
            //if (itplist.Count!=0)
            //{
            //    res.Data = "Yesitperiodexits";

            //}
            //else
            //{
            //    res.Data = "Norecordinitperiodfound";
            //   // return Json(res, JsonRequestBehavior.AllowGet);
            //}





            return Json(res, JsonRequestBehavior.AllowGet);

        }
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            JsonResult res;
            res = new JsonResult();
            List<Period> period = new List<Period>();
            period = db.Periods.Where(x=>x.Companyid==companyid).ToList();
            List<ITPeriod> itperiod = new List<ITPeriod>();
            itperiod = db.ITPeriods.Where(x=>x.Companyid==companyid).ToList();
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


        [HttpGet]
        public ActionResult Dateputtousevalidation(string strvdate)
        {
            //i have used res.Data=yess for error showing error and res.data=no for no errors
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
            DateTime itperioddate = Convert.ToDateTime("01/01/0001");
            DateTime perioddate = Convert.ToDateTime("01/01/0001");
            List<ITPeriod> itperiod = new List<ITPeriod>();

            // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
            itperiod = db.ITPeriods.Where(x=>x.Companyid==companyid).ToList();
            // string checkflag = "";
            DateTime vdate = Convert.ToDateTime(strvdate);
            //if (itperiod.Count!=0)
            //{
            List<ITPeriod> itperiodlock = new List<ITPeriod>();
            itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1 && x.Companyid==companyid).ToList();
            if (itperiodlock.Count != 0)
            {
                foreach (ITPeriod item in itperiodlock)
                {
                    if (vdate >= item.FromDate && vdate <= item.ToDate)
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
            slist = db.SubPeriods.Where(x=>x.Companyid==companyid).ToList();
            if (slist != null)
            {
                SubPeriod checkdepflag = new SubPeriod();
                checkdepflag = db.SubPeriods.Where(x => x.DepFlag == "Y" && x.FromDate <= vdate && x.ToDate >= vdate && x.Companyid==companyid).FirstOrDefault();
                if (checkdepflag == null)
                {
                    subperiod = db.SubPeriods.Where(x => x.PeriodLockFlag == "Y" && x.Companyid==companyid).ToList();
                    if (subperiod.Count != 0)
                    {
                        foreach (SubPeriod itemsub in subperiod)
                        {
                            if (vdate >= itemsub.FromDate && vdate <= itemsub.ToDate)
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

            if (vdate <= lockdate)
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
        // GET: Brand/Create
        [HttpPost]
        public ActionResult AddNew(Addition addition)
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
                //amcobj.FromDate = amcViewmodel.FromDate;
                //amcobj.ToDate = amcViewmodel.ToDate;
                //amcobj.ReminderEMail = amcViewmodel.ReminderEMailID;
                //amcobj.AMCDetails = amcViewmodel.AmcDetails;
                //amcobj.Remarks = amcViewmodel.Remarks;
                //db.AMCss.Add(amcobj);
                //db.SaveChanges();
                //var amcid = db.AMCss.Max(x => x.ID);
                //SubAmc subamc = new SubAmc();
                //if (amcViewmodel.AmcViewModellist != null)
                //{
                //    foreach (var item in amcViewmodel.AmcViewModellist)
                //    {
                //        subamc.AmcId = amcid;
                //        subamc.AssetNo = item.AssetNo;
                //        subamc.AssetDescription = item.AssetDescription;
                //        subamc.CapitalisedAmount = item.CapitalisedAmount;

                //        db.SubAmc.Add(subamc);
                //        db.SaveChanges();

                //    }
                //}
                if (addition.AssetNo != null)
                {
                    var assetid = db.Assetss.Where(x => x.AssetNo == addition.AssetNo && x.Companyid==companyid).FirstOrDefault().ID;
                    addition.AssetId = assetid;
                }
                if (addition.IsImported == null)
                {
                    addition.IsImported = "no";
                }
                addition.CreatedUserId = userid;
                addition.CreatedDate = istDate;
                addition.Companyid = companyid;
                db.Additions.Add(addition);

                res.Data = "Success";
                db.SaveChanges();
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
        [HttpGet]
         public ActionResult checkadditionentry(decimal amtcap,string assetno)
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
            var checkamtcap_asset = db.Assetss.Where(x => x.AssetNo == assetno).FirstOrDefault().AmountCapitalisedCompany;
            if(checkamtcap_asset!=null)
            {    
                    decimal? calculatedamt = checkamtcap_asset / 100;
                if (amtcap>calculatedamt)
                {
                    res.Data = "amtisgreater";
                }
                else
                {

                }
            }
            else
            {
                res.Data = "Noamtfound";
            }



            return Json(res, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
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

            Addition addition = new Addition();

            try
            {
                addition = db.Additions.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault();
                addition.str_VoucherDate = Convert.ToDateTime(addition.VoucherDate).ToString("dd/MM/yyyy");
                DateTime checkvoucherdate = Convert.ToDateTime(addition.str_VoucherDate);

                string checklockflag = ImportDatevalidation(checkvoucherdate,companyid);
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
                //amc = db.AMCss.Where(x => x.ID == id).FirstOrDefault();
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid==companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
                ViewBag.supplierlist = new SelectList(db.Suppliers.Where(x=>x.Companyid==companyid).OrderBy(e => e.ID), "ID", "SupplierName");
                ViewBag.uomlist = new SelectList(db.UOMs.Where(x=>x.Companyid==companyid).OrderBy(e => e.ID), "ID", "Unit");
                addition.str_BillDate = addition.BillDate.ToString("dd/MM/yyyy");
                addition.str_CommissioningDate = addition.CommissioningDate.ToString("dd/MM/yyyy"); ;
                addition.str_DtPutToUse = addition.DtPutToUse.ToString("dd/MM/yyyy");
                addition.str_DtPutToUseIT = addition.DtPutToUse.ToString("dd/MM/yyyy");
                addition.str_PODate = addition.PODate.ToString("dd/MM/yyyy");
                addition.str_VoucherDate = addition.VoucherDate.ToString("dd/MM/yyyy");
                addition.str_ReceiptDate = addition.ReceiptDate.ToString("dd/MM/yyyy");
                addition.ID = addition.ID;
                ViewBag.supplierno = addition.SupplierNo;
                ViewBag.uomno = addition.uom;
                var assetno = db.Assetss.Where(x => x.ID == addition.AssetId && x.Companyid==companyid).FirstOrDefault().AssetNo;

                ViewBag.assetno = assetno;
                if (addition.str_DtPutToUseIT == "01/01/0001")
                {
                    addition.str_DtPutToUseIT = "";
                }
                if (addition.str_PODate == "01/01/0001")
                {
                    addition.str_PODate = "";
                }
                if (addition.str_ReceiptDate == "01/01/0001")
                {
                    addition.str_ReceiptDate = "";
                }
                if (addition.str_CommissioningDate == "01/01/0001")
                {
                    addition.str_CommissioningDate = "";
                }
                if (addition.str_BillDate == "01/01/0001")
                {
                    addition.str_BillDate = "";
                }


            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            return View(addition);

        }
        // [AuthUser]
        [HttpPost]
        public ActionResult Edit(Addition addition)
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

                if (addition.AssetNo != "")
                {
                    var assetid = db.Assetss.Where(x => x.AssetNo == addition.AssetNo && x.Companyid==companyid).FirstOrDefault().ID;
                    addition.AssetId = assetid;
                }
                if (addition.IsImported == null)
                {
                    addition.IsImported = "no";
                }
                addition.ModifiedDate = istDate;
                addition.Modified_Userid = userid;
                db.Entry(addition).State = System.Data.Entity.EntityState.Modified;
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
        //public ActionResult Delete(int id)
        //{

        //    JsonResult res;
        //    res = new JsonResult();
        //    Period period;
        //    List<SubPeriod> subplist = new List<SubPeriod>();
        //    subplist = db.SubPeriods.Where(x => x.PeriodID == id).ToList();

        //    try
        //    {
        //        if (subplist.Count == 0)
        //        {

        //            period = db.Periods.Where(x => x.ID == id).FirstOrDefault();
        //            db.Entry(period).State = System.Data.Entity.EntityState.Deleted;
        //            db.SaveChanges();
        //            res.Data = "Success";

        //        }
        //        else
        //        {

        //            res.Data = "Failed";

        //        }
        //        return res;

        //    }
        //    catch (Exception ex)
        //    {
        //        string strError;
        //        strError = ex.Message + "|" + ex.InnerException;
        //        //logger.Log(LogLevel.Error, strError);
        //        res.Data = "Err";
        //        return res;
        //    }

        //}

        [HttpGet]

        public ActionResult AdditionAssetsExport()
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


            Response.ClearContent();
            Response.BinaryWrite(generateAdditionAssetsexcel(companyid));
            string excelName = "AdditionAssets";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "AdditionAssets");

        }
        public byte[] generateAdditionAssetsexcel(int companyid)
        {
            List<Addition> lstins = new List<Addition>();
            int srno = 1;


            lstins = db.Additions.Where(x=>x.Companyid==companyid).ToList();

            foreach (var item in lstins)
            {
                item.int_Srno = srno;
                item.str_BillDate = item.BillDate.ToString("dd/MM/yyyy");
                item.str_CommissioningDate = item.CommissioningDate.ToString("dd/MM/yyyy"); ;
                item.str_DtPutToUse = item.DtPutToUse.ToString("dd/MM/yyyy");
                item.str_DtPutToUseIT = item.DtPutToUse.ToString("dd/MM/yyyy");
                item.str_PODate = item.PODate.ToString("dd/MM/yyyy");
                item.str_VoucherDate = item.VoucherDate.ToString("dd/MM/yyyy");
                item.str_ReceiptDate = item.ReceiptDate.ToString("dd/MM/yyyy");
                srno++;
            }

            var memoryStream = new MemoryStream();
            byte[] data;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[] { "Sr No", "Addition No", "Asset No","Asset Name", "Addition Asset Description", "VoucherNo", "Voucher Date",
                                     "DtPutToUse","DtPutToUseIT","PONo","PODate","BillNo","BillDate","ReceiptDate","CommissioningDate","ResidualVal","Qty","uom",
                                      "SupplierNo","GrossVal","ServiceCharges","OtherExp","CustomDuty","ExciseDuty","ServiceTax","AnyOtherDuty","VAT","CGST","IGST","CST","GST",
                                      "AnyTax","Total Addition","Discount","Rounding off","Total Deduction","Invoice Amount","DutyDrawback","ExciseCredit","ServiceTaxCredit",
                                        "AnyOtherDutyCredit","VATCredit","CSTCredit","GSTCredit","AnyOtherCredit","SGSTCredit","IGSTCredit","TotalCredit","AmountCapitalised",
                                        "AmountCapitalisedCompany","AmountCApitalisedIT","BrandName","SrNo","Model","Remarks","IsImported","Currency","Values" ,"SGST","CGSTCREDIT"

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
                    worksheet.Cells[rowIterator, 2].Value = item.AdditionNo;
                    if (item.AssetNo != null)
                    {
                        var assetno = db.Assetss.Where(x => x.ID == item.AssetId).FirstOrDefault().AssetNo;
                        worksheet.Cells[rowIterator, 3].Value = item.AssetNo;
                    }
                    else
                    {
                        worksheet.Cells[rowIterator, 3].Value = item.AssetNo;
                    }

                    worksheet.Cells[rowIterator, 4].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 5].Value = item.AdditionAssetName;
                    worksheet.Cells[rowIterator, 6].Value = item.VoucherNo;
                    worksheet.Cells[rowIterator, 7].Value = item.str_VoucherDate;
                    worksheet.Cells[rowIterator, 8].Value = item.str_DtPutToUse;
                    worksheet.Cells[rowIterator, 9].Value = item.str_DtPutToUseIT;
                    worksheet.Cells[rowIterator, 10].Value = item.PONo;
                    worksheet.Cells[rowIterator, 11].Value = item.str_PODate;
                    worksheet.Cells[rowIterator, 12].Value = item.BillNo;
                    worksheet.Cells[rowIterator, 13].Value = item.str_BillDate;
                    worksheet.Cells[rowIterator, 14].Value = item.str_ReceiptDate;
                    worksheet.Cells[rowIterator, 15].Value = item.str_CommissioningDate;
                    worksheet.Cells[rowIterator, 16].Value = item.ResidualVal;
                    worksheet.Cells[rowIterator, 17].Value = item.Qty;
                    worksheet.Cells[rowIterator, 18].Value = item.uom;
                    worksheet.Cells[rowIterator, 19].Value = item.SupplierNo;
                    worksheet.Cells[rowIterator, 20].Value = item.GrossVal;
                    worksheet.Cells[rowIterator, 21].Value = item.ServiceCharges;
                    worksheet.Cells[rowIterator, 22].Value = item.OtherExp;
                    worksheet.Cells[rowIterator, 23].Value = item.CustomDuty;
                    worksheet.Cells[rowIterator, 24].Value = item.ExciseDuty;
                    worksheet.Cells[rowIterator, 25].Value = item.ServiceTax;
                    worksheet.Cells[rowIterator, 26].Value = item.AnyOtherDuty;
                    worksheet.Cells[rowIterator, 27].Value = item.VAT;
                    worksheet.Cells[rowIterator, 28].Value = item.CGST;
                    worksheet.Cells[rowIterator, 29].Value = item.IGST;
                    worksheet.Cells[rowIterator, 30].Value = item.CST;
                    worksheet.Cells[rowIterator, 31].Value = item.GST;
                    worksheet.Cells[rowIterator, 32].Value = item.AnyOtherTax;
                    worksheet.Cells[rowIterator, 33].Value = item.TotalAddition;
                    worksheet.Cells[rowIterator, 34].Value = item.Discount;
                    worksheet.Cells[rowIterator, 35].Value = item.Roundingoff;
                    worksheet.Cells[rowIterator, 36].Value = item.TotDeduction;
                    worksheet.Cells[rowIterator, 37].Value = item.InvoiceAmt;
                    worksheet.Cells[rowIterator, 38].Value = item.DutyDrawback;
                    worksheet.Cells[rowIterator, 39].Value = item.ExciseCredit;
                    worksheet.Cells[rowIterator, 40].Value = item.ServiceTaxCredit;
                    worksheet.Cells[rowIterator, 41].Value = item.AnyOtherDutyCredit;
                    worksheet.Cells[rowIterator, 42].Value = item.VATCredit;
                    worksheet.Cells[rowIterator, 43].Value = item.CSTCredit;
                    worksheet.Cells[rowIterator, 44].Value = item.GSTCredit;
                    worksheet.Cells[rowIterator, 45].Value = item.AnyOtherCredit;
                    worksheet.Cells[rowIterator, 46].Value = item.SGSTCredit;
                    worksheet.Cells[rowIterator, 47].Value = item.IGSTCredit;
                    worksheet.Cells[rowIterator, 48].Value = item.TotalCredit;
                    worksheet.Cells[rowIterator, 49].Value = item.AmountCapitalised;
                    worksheet.Cells[rowIterator, 50].Value = item.AmountCapitalisedCompany;
                    worksheet.Cells[rowIterator, 51].Value = item.AmountCApitalisedIT;
                    worksheet.Cells[rowIterator, 52].Value = item.BrandName;
                    worksheet.Cells[rowIterator, 53].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 54].Value = item.Model;
                    worksheet.Cells[rowIterator, 55].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 56].Value = item.IsImported;
                    worksheet.Cells[rowIterator, 57].Value = item.Currency;
                    worksheet.Cells[rowIterator, 58].Value = item.Values;
                    worksheet.Cells[rowIterator, 59].Value = item.SGST;
                    worksheet.Cells[rowIterator, 60].Value = item.CGSTCredit;

                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        [HttpGet]
        public ActionResult DownloadAdditionAssetsExcel()
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

            Response.ClearContent();
            Response.BinaryWrite(generateImportadditionassetexcel(companyid));
            string excelName = "AdditionAssets";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "AdditionAssets");

        }
        public byte[] generateImportadditionassetexcel(int companyid)
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

                   { "Sr No", "Addition No", "Asset No", "Addition Asset Description", "VoucherNo", "Voucher Date",
                                     "DtPutToUse","DtPutToUseIT","PONo","PODate","BillNo","BillDate","ReceiptDate","CommissioningDate","ResidualVal","Qty","uom",
                                      "SupplierNo","GrossVal","ServiceCharges","OtherExp","CustomDuty","ExciseDuty","ServiceTax","AnyOtherDuty","VAT","CGST","SGST","IGST","CST","GST",
                                      "AnyTax","Total Addition","Discount","Rounding off","Total Deduction","Invoice Amount","DutyDrawback","ExciseCredit","ServiceTaxCredit",
                                        "AnyOtherDutyCredit","VATCredit","CSTCredit","GSTCredit","AnyOtherCredit","SGSTCredit","IGSTCredit","TotalCredit","AmountCapitalised",
                                        "AmountCapitalisedCompany","AmountCApitalisedIT","BrandName","SrNo","Model","Remarks","IsImported","Currency","Values","CGSTCREDIT"

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
                //var addition = db.Additions.Max(x => x.AdditionNo == null  && x.Companyid==companyid ? 0 : x.AdditionNo);
                //// var addition = db.Additions.OrderByDescending(x => x.AdditionNo).FirstOrDefault().AdditionNo;
                //if (addition == 0)
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            return PartialView();
        }

        [HttpPost]
        [ValidateAjax]
        public ActionResult UploadAdditionAssets()
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
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            Addition addition = new Addition();
            // using (DbContextTransaction transaction = db.Database.BeginTransaction())
            //{
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
                                string additionno = "";
                                string int_srno = "";
                                string grossvalue = "";
                                string servicecharge = "";
                                string otherexpense = "";
                                string customduty = "";
                                string exciseduty = "";
                                string servicetax = "";
                                string vat = "";
                                string anyotherduty = "";
                                string cst = "";
                                string gst = "";
                                string anyothertax = "";
                                string totaladdition = "";
                                string discount = "";
                                string roundoff = "";
                                string totaldeduction = "";
                                string invoiceamt = "";
                                string dutydrawback = "";
                                string excisecredit = "";
                                string servicetaxcredit = "";

                                string anyotherdutycredit = "";
                                string vatcredit = "";
                                string anyothercredit = "";
                                string cstcredit = "";
                                string gstcredit = "";
                                string totalcredit = "";
                                string amtcap = "";
                                string amtcapcompanylaw = "";
                                string amtcapincometax = "";
                                string AssetNo = "";
                                string AdditionAssetName = "";
                                string VoucherNo = "";
                                string VoucherDate = "";
                                string PODate = "";
                                string ReceiptDate = "";
                                string CommissioningDate = "";
                                string BillDate = "";
                                string DtPutToUse = "";
                                string DtPutToUseIT = "";
                                string PONo = "";
                                string BillNo = "";
                                string Qty = "";
                                string SupplierNo = "";
                                string BrandName = "";
                                string SrNo = "";
                                string Model = "";
                                string Remarks = "";
                                string IsImported = "";
                                string Currency = "";
                                string Values = "";
                                string cgstcredit = "";
                                string igstcredit = "";
                                string sgstcredit = "";
                                string cgst = "";
                                string igst = "";
                                string sgst = "";
                                string uomno = "";
                                string residual = "";

                                bool grossvalueflag = false;
                                bool servicechargeflag = false;
                                bool otherexpenseflag = false;
                                bool customdutyflag = false;
                                bool excisedutyflag = false;
                                bool servicetaxflag = false;
                                bool vatflag = false;
                                bool anyotherdutyflag = false;
                                bool cstflag = false;
                                bool gstflag = false;
                                bool anyothertaxflag = false;
                                bool totaladditionflag = false;
                                bool discountflag = false;
                                bool roundoffflag = false;
                                bool totaldeductionflag = false;
                                bool invoiceamtflag = false;
                                bool dutydrawbackflag = false;
                                bool excisecreditflag = false;
                                bool servicetaxcreditflag = false;
                                bool anyotherdutycreditflag = false;
                                bool vatcreditflag = false;
                                bool anyothercreditflag = false;
                                bool cstcreditflag = false;
                                bool gstcreditflag = false;
                                bool totalcreditflag = false;
                                bool amtcapflag = false;
                                bool amtcapcompanylawflag = false;
                                bool amtcapincometaxflag = false;
                                bool AssetNoflag = false;
                                bool AdditionAssetNameflag = false;
                                bool VoucherNoflag = false;
                                bool VoucherDateflag = false;
                                bool PODateflag = false;
                                bool ReceiptDateflag = false;
                                bool CommissioningDateflag = false;
                                bool BillDateflag = false;
                                bool DtPutToUseflag = false;
                                bool DtPutToUseITflag = false;
                                bool PONoflag = false;
                                bool BillNoflag = false;
                                bool Qtyflag = false;
                                bool SupplierNoflag = false;
                                bool BrandNameflag = false;
                                bool SrNoflag = false;
                                bool Modelflag = false;
                                bool Remarksflag = false;
                                bool IsImportedflag = false;
                                bool Currencyflag = false;
                                bool Valuesflag = false;
                                bool cgstcreditflag = false;
                                bool igstcreditflag = false;
                                bool sgstcreditflag = false;
                                bool cgstflag = false;
                                bool igstflag = false;
                                bool sgstflag = false;
                                bool uomnoflag = false;
                                bool residualflag = false;
                                bool int_srnoflag;
                                bool additionnoflag;
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
                                    additionno = "";
                                    additionnoflag = false;
                                }
                                else
                                {
                                    additionno = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    additionnoflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    AssetNo = "";
                                    AssetNoflag = false;

                                }
                                else
                                {

                                    AssetNo = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    AssetNoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    AdditionAssetName = "";
                                    AdditionAssetNameflag = false;
                                }
                                else
                                {
                                    AdditionAssetName = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    AdditionAssetNameflag = true;
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
                                    DtPutToUse = "";
                                    DtPutToUseflag = false;
                                }
                                else
                                {
                                    DtPutToUse = workSheet.Cells[rowIterator, 7].Value.ToString();
                                    DtPutToUseflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 8].Text == "")
                                {
                                    DtPutToUseIT = "";
                                    DtPutToUseITflag = false;
                                }
                                else
                                {
                                    DtPutToUseIT = workSheet.Cells[rowIterator, 8].Value.ToString();
                                    DtPutToUseITflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 9].Text == "")
                                {
                                    PONo = "";
                                    PONoflag = false;

                                }
                                else
                                {

                                    PONo = workSheet.Cells[rowIterator, 9].Value.ToString();
                                    PONoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 10].Text == "")
                                {
                                    PODate = "";
                                    PODateflag = false;
                                }
                                else
                                {
                                    PODate = workSheet.Cells[rowIterator, 10].Value.ToString();
                                    PODateflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 11].Text == "")
                                {
                                    BillNo = "";
                                    BillNoflag = false;
                                }
                                else
                                {
                                    BillNo = workSheet.Cells[rowIterator, 11].Value.ToString();
                                    BillNoflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 12].Text == "")
                                {
                                    BillDate = "";
                                    BillDateflag = false;
                                }
                                else
                                {
                                    BillDate = workSheet.Cells[rowIterator, 12].Value.ToString();
                                    BillDateflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 13].Text == "")
                                {
                                    ReceiptDate = "";
                                    ReceiptDateflag = false;
                                }
                                else
                                {
                                    ReceiptDate = workSheet.Cells[rowIterator, 13].Value.ToString();
                                    ReceiptDateflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 14].Text == "")
                                {
                                    CommissioningDate = "";
                                    CommissioningDateflag = false;
                                }
                                else
                                {
                                    CommissioningDate = workSheet.Cells[rowIterator, 14].Value.ToString();
                                    CommissioningDateflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 15].Text == "")
                                {
                                    residual = "";
                                    residualflag = false;

                                }
                                else
                                {

                                    residual = workSheet.Cells[rowIterator, 15].Value.ToString();
                                    residualflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 16].Text == "")
                                {
                                    Qty = "";
                                    Qtyflag = false;
                                }
                                else
                                {
                                    Qty = workSheet.Cells[rowIterator, 16].Value.ToString();
                                    Qtyflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 17].Text == "")
                                {
                                    SupplierNo = "";
                                    SupplierNoflag = false;
                                }
                                else
                                {
                                    SupplierNo = workSheet.Cells[rowIterator, 17].Value.ToString();
                                    SupplierNoflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 18].Text == "")
                                {
                                    uomno = "";
                                    uomnoflag = false;
                                }
                                else
                                {
                                    uomno = workSheet.Cells[rowIterator, 18].Value.ToString();
                                    uomnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 19].Text == "")
                                {
                                    grossvalue = "";
                                    grossvalueflag = false;
                                }
                                else
                                {
                                    grossvalue = workSheet.Cells[rowIterator, 19].Value.ToString();
                                    grossvalueflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 20].Text == "")
                                {
                                    servicecharge = "";
                                    servicechargeflag = false;
                                }
                                else
                                {
                                    servicecharge = workSheet.Cells[rowIterator, 20].Value.ToString();
                                    servicechargeflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 21].Text == "")
                                {
                                    otherexpense = "";
                                    otherexpenseflag = false;

                                }
                                else
                                {

                                    otherexpense = workSheet.Cells[rowIterator, 21].Value.ToString();
                                    otherexpenseflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 22].Text == "")
                                {
                                    customduty = "";
                                    customdutyflag = false;
                                }
                                else
                                {
                                    customduty = workSheet.Cells[rowIterator, 22].Value.ToString();
                                    customdutyflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 23].Text == "")
                                {
                                    exciseduty = "";
                                    excisedutyflag = false;
                                }
                                else
                                {
                                    exciseduty = workSheet.Cells[rowIterator, 23].Value.ToString();
                                    excisedutyflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 24].Text == "")
                                {
                                    servicetax = "";
                                    servicetaxflag = false;
                                }
                                else
                                {
                                    servicetax = workSheet.Cells[rowIterator, 24].Value.ToString();
                                    servicetaxflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 25].Text == "")
                                {
                                    anyotherduty = "";
                                    anyotherdutyflag = false;
                                }
                                else
                                {
                                    anyotherduty = workSheet.Cells[rowIterator, 25].Value.ToString();
                                    anyotherdutyflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 26].Text == "")
                                {
                                    vat = "";
                                    vatflag = false;
                                }
                                else
                                {
                                    vat = workSheet.Cells[rowIterator, 26].Value.ToString();
                                    vatflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 27].Text == "")
                                {
                                    cgst = "";
                                    cgstflag = false;

                                }
                                else
                                {

                                    cgst = workSheet.Cells[rowIterator, 27].Value.ToString();
                                    cgstflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 28].Text == "")
                                {
                                    sgst = "";
                                    sgstflag = false;
                                }
                                else
                                {
                                    sgst = workSheet.Cells[rowIterator, 28].Value.ToString();
                                    sgstflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 29].Text == "")
                                {
                                    igst = "";
                                    igstflag = false;
                                }
                                else
                                {
                                    igst = workSheet.Cells[rowIterator, 29].Value.ToString();
                                    igstflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 30].Text == "")
                                {
                                    cst = "";
                                    cstflag = false;
                                }
                                else
                                {
                                    cst = workSheet.Cells[rowIterator, 30].Value.ToString();
                                    cstflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 31].Text == "")
                                {
                                    gst = "";
                                    gstflag = false;
                                }
                                else
                                {
                                    gst = workSheet.Cells[rowIterator, 31].Value.ToString();
                                    gstflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 32].Text == "")
                                {
                                    anyothertax = "";
                                    anyothertaxflag = false;
                                }
                                else
                                {
                                    anyothertax = workSheet.Cells[rowIterator, 32].Value.ToString();
                                    anyothertaxflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 33].Text == "")
                                {
                                    totaladdition = "";
                                    totaladditionflag = false;
                                }
                                else
                                {
                                    totaladdition = workSheet.Cells[rowIterator, 33].Value.ToString();
                                    totaladditionflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 34].Text == "")
                                {
                                    discount = "";
                                    discountflag = false;

                                }
                                else
                                {

                                    discount = workSheet.Cells[rowIterator, 34].Value.ToString();
                                    discountflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 35].Text == "")
                                {
                                    roundoff = "";
                                    roundoffflag = false;
                                }
                                else
                                {
                                    roundoff = workSheet.Cells[rowIterator, 35].Value.ToString();
                                    roundoffflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 36].Text == "")
                                {
                                    totaldeduction = "";
                                    totaldeductionflag = false;
                                }
                                else
                                {
                                    totaldeduction = workSheet.Cells[rowIterator, 36].Value.ToString();
                                    totaldeductionflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 37].Text == "")
                                {
                                    invoiceamt = "";
                                    invoiceamtflag = false;
                                }
                                else
                                {
                                    invoiceamt = workSheet.Cells[rowIterator, 37].Value.ToString();
                                    invoiceamtflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 38].Text == "")
                                {
                                    dutydrawback = "";
                                    dutydrawbackflag = false;
                                }
                                else
                                {
                                    dutydrawback = workSheet.Cells[rowIterator, 38].Value.ToString();
                                    dutydrawbackflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 39].Text == "")
                                {
                                    excisecredit = "";
                                    excisecreditflag = false;
                                }
                                else
                                {
                                    excisecredit = workSheet.Cells[rowIterator, 39].Value.ToString();
                                    excisecreditflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 40].Text == "")
                                {
                                    servicetaxcredit = "";
                                    servicetaxcreditflag = false;

                                }
                                else
                                {

                                    servicetaxcredit = workSheet.Cells[rowIterator, 40].Value.ToString();
                                    servicetaxcreditflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 41].Text == "")
                                {
                                    anyotherdutycredit = "";
                                    anyotherdutycreditflag = false;
                                }
                                else
                                {
                                    anyotherdutycredit = workSheet.Cells[rowIterator, 41].Value.ToString();
                                    anyotherdutycreditflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 42].Text == "")
                                {
                                    vatcredit = "";
                                    vatcreditflag = false;
                                }
                                else
                                {
                                    vatcredit = workSheet.Cells[rowIterator, 42].Value.ToString();
                                    vatcreditflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 43].Text == "")
                                {
                                    cstcredit = "";
                                    cstcreditflag = false;
                                }
                                else
                                {
                                    cstcredit = workSheet.Cells[rowIterator, 43].Value.ToString();
                                    cstcreditflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 44].Text == "")
                                {
                                    gstcredit = "";
                                    gstcreditflag = false;
                                }
                                else
                                {
                                    gstcredit = workSheet.Cells[rowIterator, 44].Value.ToString();
                                    gstcreditflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 45].Text == "")
                                {
                                    anyothercredit = "";
                                    anyothercreditflag = false;
                                }

                                else
                                {
                                    anyothercredit = workSheet.Cells[rowIterator, 45].Value.ToString();
                                    anyothercreditflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 46].Text == "")
                                {
                                    sgstcredit = "";
                                    sgstcreditflag = false;
                                }
                                else
                                {
                                    sgstcredit = workSheet.Cells[rowIterator, 46].Value.ToString();
                                    sgstcreditflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 47].Text == "")
                                {
                                    igstcredit = "";
                                    igstcreditflag = false;
                                }
                                else
                                {
                                    igstcredit = workSheet.Cells[rowIterator, 47].Value.ToString();
                                    igstcreditflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 48].Text == "")
                                {
                                    totalcredit = "";
                                    totalcreditflag = false;
                                }
                                else
                                {
                                    totalcredit = workSheet.Cells[rowIterator, 48].Value.ToString();
                                    totalcreditflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 49].Text == "")
                                {
                                    amtcap = "";
                                    amtcapflag = false;
                                }
                                else
                                {
                                    amtcap = workSheet.Cells[rowIterator, 49].Value.ToString();
                                    amtcapflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 50].Text == "")
                                {
                                    amtcapcompanylaw = "";
                                    amtcapcompanylawflag = false;
                                }
                                else
                                {
                                    amtcapcompanylaw = workSheet.Cells[rowIterator, 50].Value.ToString();
                                    amtcapcompanylawflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 51].Text == "")
                                {
                                    amtcapincometax = "";
                                    amtcapincometaxflag = false;
                                }
                                else
                                {
                                    amtcapincometax = workSheet.Cells[rowIterator, 51].Value.ToString();
                                    amtcapincometaxflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 52].Text == "")
                                {
                                    BrandName = "";
                                    BrandNameflag = false;
                                }
                                else
                                {
                                    BrandName = workSheet.Cells[rowIterator, 52].Value.ToString();
                                    BrandNameflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 53].Text == "")
                                {
                                    SrNo = "";
                                    SrNoflag = false;
                                }
                                else
                                {
                                    SrNo = workSheet.Cells[rowIterator, 53].Value.ToString();
                                    SrNoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 54].Text == "")
                                {
                                    Model = "";
                                    Modelflag = false;
                                }
                                else
                                {
                                    Model = workSheet.Cells[rowIterator, 54].Value.ToString();
                                    Modelflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 55].Text == "")
                                {
                                    Remarks = "";
                                    Remarksflag = false;
                                }
                                else
                                {
                                    Remarks = workSheet.Cells[rowIterator, 55].Value.ToString();
                                    Remarksflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 56].Text == "")
                                {
                                    IsImported = "";
                                    IsImportedflag = false;
                                }
                                else
                                {
                                    IsImported = workSheet.Cells[rowIterator, 56].Value.ToString();
                                    IsImportedflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 57].Text == "")
                                {
                                    Currency = "";
                                    Currencyflag = false;
                                }
                                else
                                {
                                    Currency = workSheet.Cells[rowIterator, 57].Value.ToString();
                                    Currencyflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 58].Text == "")
                                {
                                    Values = "";
                                    Valuesflag = false;
                                }
                                else
                                {
                                    Values = workSheet.Cells[rowIterator, 58].Value.ToString();
                                    Valuesflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 59].Text == "")
                                {
                                    cgstcredit = "";
                                    cgstcreditflag = false;
                                }
                                else
                                {
                                    cgstcredit = workSheet.Cells[rowIterator, 59].Value.ToString();
                                    cgstcreditflag = true;
                                }

                                if (additionnoflag == true && AssetNoflag == true && AdditionAssetNameflag == true
                                     && VoucherDateflag == true && Qtyflag == true && grossvalueflag == true && amtcapflag == true &&
                                     amtcapcompanylawflag == true && amtcapincometaxflag == true && DtPutToUseflag == true && DtPutToUseITflag == true)

                                {
                                    norecordsfound = true;
                                    Addition additioncheck = new Addition();
                                    int additionnumbercheck = Convert.ToInt32(additionno);
                                    additioncheck = db.Additions.Where(r => r.AdditionNo == additionnumbercheck && r.Companyid==companyid).FirstOrDefault();
                                    if (additioncheck != null)
                                    {
                                        errorlist.Add("Addition no record already exists in master,i.e of  row " + rowIterator);
                                        res.Data = errorlist;

                                    }
                                    else
                                    {
                                        if (BillDate != "")
                                        {
                                            DateTime billdate = Convert.ToDateTime(BillDate);
                                            addition.BillDate = billdate;
                                        }
                                        if (ReceiptDate != "")
                                        {
                                            DateTime receiptdate = Convert.ToDateTime(ReceiptDate);
                                            addition.ReceiptDate = receiptdate;
                                        }
                                        if (CommissioningDate != "")
                                        {
                                            DateTime commdate = Convert.ToDateTime(CommissioningDate);
                                            addition.CommissioningDate = commdate;
                                        }

                                        if (PODate != "")
                                        {
                                            DateTime podate = Convert.ToDateTime(PODate);
                                            addition.PODate = podate;
                                        }
                                        //if (DtPutToUseIT != "")
                                        //{
                                        //    DateTime dtputtuseit = Convert.ToDateTime(DtPutToUseIT);
                                        //    addition.DtPutToUseIT = dtputtuseit;


                                        //}

                                        DateTime voucherdate = Convert.ToDateTime(VoucherDate);
                                        DateTime dtputtuse = Convert.ToDateTime(DtPutToUse);
                                        DateTime dtputtuseit = Convert.ToDateTime(DtPutToUseIT);
                                        //// validation for additionasset voucher less than asset voucher date

                                        var str_checkvoucherdate = stringreturn_validateassetdate(voucherdate, AssetNo,companyid);
                                        if (str_checkvoucherdate == "AssetYes")
                                        {
                                            errorlist.Add("For voucher date cannot be less than asset voucher date. For row" + rowIterator);
                                            continue;
                                        }
                                        if (str_checkvoucherdate == "Noassetfound")
                                        {
                                            //addition.VoucherDate = voucherdate;
                                            errorlist.Add("NO asset found of this assetno For row" + rowIterator);
                                            continue;

                                        }
                                        ///////////////////////////////
                                        //validation for voucher date and period
                                        var checkvoucherdate = ImportDatevalidation(voucherdate,companyid);
                                        if (checkvoucherdate == "Yes")
                                        {
                                            errorlist.Add("For voucher date period is lock you cannot add asset. For row" + rowIterator);
                                            continue;
                                        }
                                        if (checkvoucherdate == "No")
                                        {
                                            addition.VoucherDate = voucherdate;

                                        }
                                        if (checkvoucherdate == "Nosubperiod")
                                        {
                                            errorlist.Add("For voucher date No Sub period found. For row" + rowIterator);
                                            continue;
                                        }
                                        if (checkvoucherdate == "Depalreadycalculated")
                                        {
                                            errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                            continue;
                                        }
                                        //for dateputtousecompany
                                        var checkdateputtousecomp = ImportDatevalidation(dtputtuse,companyid);
                                        if (checkdateputtousecomp == "Yes")
                                        {
                                            errorlist.Add("For voucher date period is lock you cannot add asset.For row" + rowIterator);
                                            continue;
                                        }
                                        if (checkdateputtousecomp == "No")
                                        {
                                            addition.DtPutToUse = dtputtuse;

                                        }
                                        if (checkvoucherdate == "Nosubperiod")
                                        {
                                            errorlist.Add("For voucher date No Sub period found. For row" + rowIterator);
                                            continue;
                                        }
                                        if (checkvoucherdate == "Depalreadycalculated")
                                        {
                                            errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                            continue;
                                        }
                                        //for dateputtousecompany
                                        var checkdateputtouseit = ImportDatevalidation(dtputtuseit,companyid);
                                        if (checkdateputtouseit == "Yes")
                                        {
                                            errorlist.Add("For voucher date period is lock you cannot add asset.For row" + rowIterator);
                                            continue;
                                        }
                                        if (checkdateputtouseit == "No")
                                        {
                                            addition.DtPutToUseIT = dtputtuseit;

                                        }
                                        if (checkvoucherdate == "Nosubperiod")
                                        {
                                            errorlist.Add("For voucher date No Sub period found. For row" + rowIterator);
                                            continue;
                                        }
                                        if (checkvoucherdate == "Depalreadycalculated")
                                        {
                                            errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                            continue;
                                        }
                                        /////////////////////////////////////////////////////////////////////





                                        addition.int_Srno = Convert.ToInt32(int_srno);
                                        addition.AdditionNo = Convert.ToInt32(additionno);

                                        var assetname = db.Assetss.Where(x => x.AssetNo == AssetNo && x.Companyid==companyid).FirstOrDefault().AssetName;
                                        addition.AssetName = assetname;
                                        addition.AssetId = db.Assetss.Where(x => x.AssetNo == AssetNo && x.Companyid==companyid).FirstOrDefault().ID;
                                        addition.AdditionAssetName = AdditionAssetName;
                                        // addition.VoucherNo = VoucherNo;
                                        //addition.VoucherDate = voucherdate;
                                        //addition.DtPutToUse = dtputtuse;
                                        addition.PONo = PONo;
                                        addition.BillNo = BillNo;
                                        if (SupplierNo != "")
                                        {
                                            addition.SupplierNo = Convert.ToInt32(SupplierNo);
                                        }
                                        if (uomno != "")
                                        {
                                            addition.uom = Convert.ToInt32(uomno);
                                        }

                                        addition.ResidualVal = ToDecimal(residual);
                                        addition.Qty = Convert.ToInt32(Qty);
                                        addition.GrossVal = ToDecimal(grossvalue);
                                        addition.ServiceCharges = ToDecimal(servicecharge);
                                        addition.ExciseDuty = ToDecimal(exciseduty);
                                        addition.CustomDuty = ToDecimal(customduty);
                                        addition.AnyOtherDuty = ToDecimal(anyotherduty);
                                        addition.VAT = ToDecimal(vat);
                                        addition.AnyOtherTax = ToDecimal(anyothertax);
                                        addition.CST = ToDecimal(cst);
                                        addition.GST = ToDecimal(gst);
                                        addition.SGST = ToDecimal(sgst);
                                        addition.IGST = ToDecimal(igst);
                                        addition.CGST = ToDecimal(cgst);
                                        addition.ServiceTax = ToDecimal(servicetax);
                                        addition.OtherExp = ToDecimal(otherexpense);
                                        addition.TotalAddition = ToDecimal(totaladdition);
                                        addition.Discount = ToDecimal(discount);
                                        addition.Roundingoff = ToDecimal(roundoff);
                                        addition.TotDeduction = ToDecimal(totaldeduction);
                                        addition.InvoiceAmt = ToDecimal(invoiceamt);
                                        addition.AnyOtherCredit = ToDecimal(anyothercredit);
                                        addition.DutyDrawback = ToDecimal(dutydrawback);
                                        addition.ExciseCredit = ToDecimal(excisecredit);
                                        addition.AnyOtherCredit = ToDecimal(anyothercredit);
                                        addition.ServiceTaxCredit = ToDecimal(servicetaxcredit);
                                        addition.VATCredit = ToDecimal(vatcredit);
                                        addition.CSTCredit = ToDecimal(cstcredit);
                                        addition.GSTCredit = ToDecimal(gstcredit);
                                        addition.SGSTCredit = ToDecimal(sgstcredit);
                                        addition.IGSTCredit = ToDecimal(igstcredit);
                                        addition.CGSTCredit = ToDecimal(cgstcredit);
                                        addition.TotalCredit = ToDecimal(totalcredit);
                                        addition.AmountCapitalised = ToDecimal(amtcap);
                                        addition.AmountCapitalisedCompany = ToDecimal(amtcapcompanylaw);
                                        addition.AmountCApitalisedIT = ToDecimal(amtcapincometax);
                                        addition.AnyOtherDutyCredit = ToDecimal(anyotherdutycredit);
                                        addition.BrandName = BrandName;
                                        addition.SrNo = SrNo;
                                        addition.Model = Model;
                                        addition.Remarks = Remarks;
                                        addition.IsImported = IsImported;
                                        addition.Currency = Currency;
                                        addition.Values = ToDecimal(Values);

                                        addition.CreatedUserId = userid;
                                        addition.CreatedDate = istDate;
                                        addition.Companyid = companyid;
                                        db.Additions.Add(addition);
                                        db.SaveChanges();
                                        //                              transaction.Commit();
                                    }
                                }
                                else
                                {
                                    errorlist.Add("Something  went wrong or some value missing in the row  " + rowIterator);

                                }


                            }

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

                string strError;
                strError = ex.Message + "|" + ex.InnerException;

                //  logger.Log(LogLevel.Error, strError);
                //transaction.Rollback();
                res.Data = "error";
                return res;
            }
            //}
        }
        public string ImportDatevalidation(DateTime date ,int companyid)
        {
            
            //i have used res.Data=yess for error showing error and res.data=no for no errors
            string checkflag = "";
            JsonResult res;
            res = new JsonResult();
            DateTime itperioddate = Convert.ToDateTime("01/01/0001");
            DateTime perioddate = Convert.ToDateTime("01/01/0001");
            List<ITPeriod> itperiod = new List<ITPeriod>();

            // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
            itperiod = db.ITPeriods.Where(x=>x.Companyid==companyid).ToList();
            // string checkflag = "";
            // DateTime vdate = Convert.ToDateTime(strvdate);
            //if (itperiod.Count!=0)
            //{
            List<ITPeriod> itperiodlock = new List<ITPeriod>();
            itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1 && x.Companyid==companyid).ToList();
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
            slist = db.SubPeriods.Where(x=>x.Companyid==companyid).ToList();
            if (slist != null)
            {
                SubPeriod checkdepflag = new SubPeriod();
                checkdepflag = db.SubPeriods.Where(x => x.DepFlag == "Y" && x.FromDate <= date && x.ToDate >= date && x.Companyid==companyid).FirstOrDefault();
                if (checkdepflag == null)
                {
                    subperiod = db.SubPeriods.Where(x => x.PeriodLockFlag == "Y" && x.Companyid==companyid).ToList();
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

            return checkflag;

        }
        public string stringreturn_validateassetdate(DateTime vdate, string assetno, int companyid)
        {
            JsonResult res;
            res = new JsonResult();
            // int int_assetno = Convert.ToInt32(assetno);
            Assets assets = new Assets();
            string checkflag = "";

            // DateTime vdate = Convert.ToDateTime(strvdate);
            assets = db.Assetss.Where(x => x.AssetNo == assetno && x.Companyid==companyid).FirstOrDefault();
            if (assets != null)
            {
                if (vdate < assets.VoucherDate)
                {
                    checkflag = "AssetYes";
                    //return;
                }

            }
            else
            {
                checkflag = "Noassetfound";
            }
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
    }
}
