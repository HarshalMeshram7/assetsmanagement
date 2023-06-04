using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class DepreciationController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Depreciation

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        [HttpPost]
        public ActionResult GetData(string subpid)
        {

            int userid = 0;
            int subperiodid = 0;
            if (subpid == "")
            {
                subpid = "0";

            }
            else
            {
                subperiodid = Convert.ToInt32(subpid);
            }



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

            //List<Depreciation> lstDepriciation = new List<Depreciation>();
            List<DepreciationViewModel> lstDepriciation = new List<DepreciationViewModel>();

            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

            JsonResult result = new JsonResult();
            int totalRecords = 0;
            try
            {


                List<SubPeriod> subperiod = new List<SubPeriod>();
                subperiod = db.SubPeriods.Where(x => x.Companyid == companyid && x.DepFlag == "Y").ToList();
                var lastsubperiod = subperiod.LastOrDefault();
                if (lastsubperiod == null)
                {
                    ViewBag.sublastperiod = 0;
                }
                else
                {
                    ViewBag.sublastperiod = lastsubperiod.ID;
                }
                if (subperiodid == 0)
                {

                    lstDepriciation = null;
                }
                else
                {
                    SubPeriod subperiods = new SubPeriod();
                    subperiods = db.SubPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
                    if (subperiods != null)
                    {
                        /*
                        lstDepriciation = db.Depreciations.Where(x => x.Companyid == companyid 
                        && x.FromDate == subperiods.FromDate && x.ToDate == subperiods.ToDate).ToList();
                        */

                        if (!string.IsNullOrEmpty(search) &&
                        !string.IsNullOrWhiteSpace(search))
                        {
                            lstDepriciation = (from dep in db.Depreciations
                                               join assetmaster in db.Assetss
                                               on dep.AssetId equals assetmaster.ID
                                               where dep.Companyid == companyid
                                               && dep.FromDate >= subperiods.FromDate
                                               && dep.ToDate <= subperiods.ToDate
                                               && (assetmaster.AssetName.ToString().ToLower().Contains(search.ToLower())
                                               || assetmaster.AssetNo.ToLower().Contains(search.ToLower())
                                               )
                                               select new { dep, assetmaster }).AsEnumerable()
                                                .Select(e => new DepreciationViewModel
                                                {
                                                    ID = e.dep.ID,
                                                    AssetID = e.dep.AssetId,
                                                    AssetNo = e.assetmaster.AssetNo,
                                                    AssetName = e.assetmaster.AssetName,
                                                    FromDate = e.dep.FromDate,
                                                    ToDate = e.dep.ToDate,
                                                    TotalRate = e.dep.TotalRate,
                                                    Amount = e.dep.Amount,
                                                    DepreciationMethod = e.dep.DepreciationMethod,
                                                    DepreciationDays = e.dep.DepreciationDays
                                                }).OrderBy(z => z.AssetNo).Skip(startRec).Take(pageSize).ToList();


                            totalRecords = db.Depreciations.Where(x => x.Companyid == companyid
                            && x.FromDate == subperiods.FromDate && x.ToDate == subperiods.ToDate).Count();



                        }
                        else {

                            //lstDepriciation = db.Depreciations.Where(x => x.Companyid == companyid
                            //&& x.FromDate == subperiods.FromDate && x.ToDate == subperiods.ToDate)
                            //.Select(x => new DepreciationViewModel
                            //{
                            //    ID = x.ID,
                            //    AssetID = x.AssetId,
                            //    AssetNo = string.Empty,
                            //    AssetName = x.AssetName,
                            //    FromDate = x.FromDate,
                            //    ToDate = x.ToDate,
                            //    TotalRate = x.TotalRate,
                            //    Amount =x.Amount,
                            //    DepreciationMethod = x.DepreciationMethod,
                            //    DepreciationDays= x.DepreciationDays
                            //}).OrderBy(z=>z.ID).Skip(startRec).Take(pageSize).ToList();

                            lstDepriciation = (from dep in db.Depreciations
                                               join assetmaster in db.Assetss
                                               on dep.AssetId equals assetmaster.ID
                                               where dep.Companyid == companyid
                                               && dep.FromDate >= subperiods.FromDate
                                               && dep.ToDate <= subperiods.ToDate

                                               select new { dep, assetmaster }).AsEnumerable()
                                                .Select(e => new DepreciationViewModel
                                                {
                                                    ID = e.dep.ID,
                                                    AssetID = e.dep.AssetId,
                                                    AssetNo = e.assetmaster.AssetNo,
                                                    AssetName = e.assetmaster.AssetName,
                                                    FromDate = e.dep.FromDate,
                                                    ToDate = e.dep.ToDate,
                                                    TotalRate = e.dep.TotalRate,
                                                    Amount = e.dep.Amount,
                                                    DepreciationMethod = e.dep.DepreciationMethod,
                                                    DepreciationDays = e.dep.DepreciationDays
                                                }).OrderBy(z => z.ID).Skip(startRec).Take(pageSize).ToList();

                            totalRecords = db.Depreciations.Where(x => x.Companyid == companyid
                            && x.FromDate == subperiods.FromDate && x.ToDate == subperiods.ToDate).Count();


                        }

                    }
                }


                List <Assets> lstAssetList  = db.Assetss.ToList();

                
                

                if (lstDepriciation != null)
                {

                    foreach (DepreciationViewModel item in lstDepriciation)
                    {
                        if (item.FromDate != null)
                        {
                            item.str_FromDate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
                        }
                        if (item.ToDate != null)
                        {
                            item.str_ToDate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy");
                        }

                        Assets obAssets = lstAssetList.Where(x => x.ID == item.AssetID).FirstOrDefault();

                        if (obAssets != null)
                        {
                            item.AssetNo = obAssets.AssetNo;
                        }

                    }


                    
                    int recFilter = lstDepriciation.Count;

                    totalResultsCount = lstDepriciation.Count;

                    filteredResultsCount = lstDepriciation.Count;

                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = totalRecords,
                        recordsFiltered = totalRecords,
                        data = lstDepriciation
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.Data = lstDepriciation;
                }



            }
            catch (Exception ex)
            {
                int test = 0;//ex.Message("");
            }
            return result;
        }
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

            List<Depreciation> lstadd = new List<Depreciation>();
            int srno = 1;
            try
            {

                //  lstadd = db.Depreciations.Where(x => x.Companyid == companyid).ToList();
                var subperiodlistlist = db.SubPeriods.Where(x => x.Companyid == companyid && x.DepFlag == "Y").ToList();
                foreach (SubPeriod item in subperiodlistlist)
                {
                    item.str_fromdate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
                    item.str_todate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy");
                }



                List<SubPeriod> subperiod = new List<SubPeriod>();
                subperiod = db.SubPeriods.Where(x => x.Companyid == companyid && x.DepFlag == "Y").ToList();
                var lastsubperiod = subperiod.LastOrDefault();
                if (subperiodlistlist == null)
                {
                    ViewBag.Subperiodist = new SelectList("0", "No Data");
                }
                else
                {
                    ViewBag.Subperiodist = new SelectList(subperiodlistlist.OrderByDescending(e => e.ToDate), "ID", "Fromdatetodate");
                }
                ///for page ready select last subperiod dropdown from dropdown
                ViewBag.sublastperiod = lastsubperiod.ID;


                //foreach (Depreciation item in lstadd)
                //{
                //    item.int_Srno = srno;
                //    item.str_FromDate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
                //    item.str_ToDate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy"); ;

                //    //if (item.AssetId != 0)
                //    //{
                //    //    item.AssetNo = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //    //}

                //    if (item.FromDate == null)
                //    {
                //        item.str_FromDate = "";
                //    }
                //    else
                //    {

                //        item.str_FromDate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");

                //    }

                //    if (item.ToDate == null)
                //    {
                //        item.str_ToDate = "";
                //    }
                //    else
                //    {

                //        item.str_ToDate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy");

                //    }

                //    srno++;
                //}
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


        [AuthUser]
        [HttpGet]
        public ActionResult Add()
        {

            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";


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
                ViewBag.baseUrl = baseUrl;

            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            // int userid = 0;

            Depreciation depreciation = new Depreciation();



            // ViewBag.Assestlist = new SelectList(db.Assetss.OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
         //   ViewBag.Assestlist = new SelectList(db.Assetss.Where(e => e.Companyid == companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
            //ViewBag.supplierlist = new SelectList(db.Suppliers.OrderBy(e => e.ID), "ID", "SupplierName");
            //ViewBag.uomlist = new SelectList(db.UOMs.OrderBy(e => e.ID), "ID", "Unit");


            return View("AddNew");
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
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Add(Depreciation depreciation)
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


                depreciation.CreatedUserId = userid;
                depreciation.CreatedDate = istDate;
                depreciation.Companyid = companyid;
                depreciation.DepreciationType = "M";
                //depreciation.AssetId = db.Assetss.Where(x => x.Companyid == companyid && x.AssetNo == depreciation.AssetNo).FirstOrDefault().ID;
                db.Depreciations.Add(depreciation);

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
        [AuthUser]
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

            Depreciation depreciation = new Depreciation();

            try
            {
                depreciation = db.Depreciations.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                depreciation.str_FromDate = Convert.ToDateTime(depreciation.FromDate).ToString("dd/MM/yyyy");
                depreciation.str_FromDate = Convert.ToDateTime(depreciation.ToDate).ToString("dd/MM/yyyy");

                //DateTime checkvoucherdate = Convert.ToDateTime(addition.str_VoucherDate);
                //string checklockflag = ImportDatevalidation(checkvoucherdate, companyid);
                //if (checklockflag == "Depalreadycalculated")
                //{
                //    ViewBag.Lock = "Depalreadycalculated";
                //}
                //if (checklockflag == "Yes")
                //{
                //    ViewBag.Lock = "Periodlock";
                //}
                //if (checklockflag == "No")
                //{
                //    ViewBag.Lock = "Nolock";
                //}
                //if (checklockflag == "Nomainperiod")
                //{
                //    ViewBag.Lock = "Nomainperiod";
                //}
                

                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.Companyid == companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
                depreciation.str_FromDate = Convert.ToDateTime(depreciation.FromDate).ToString("dd/MM/yyyy");
                depreciation.str_ToDate = Convert.ToDateTime(depreciation.ToDate).ToString("dd/MM/yyyy"); ;

                depreciation.ID = depreciation.ID;

                Assets obj_assets = db.Assetss.Where(x => x.ID == depreciation.AssetId && x.Companyid == companyid).FirstOrDefault();

                var assetno = obj_assets.AssetNo;
                ViewBag.assetno = assetno;
                ViewBag.assetname = obj_assets.AssetName;

                
                if (depreciation.str_FromDate == "01/01/0001")
                {
                    depreciation.str_FromDate = "";
                }
                if (depreciation.str_ToDate == "01/01/0001")
                {
                    depreciation.str_ToDate = "";
                }



            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            return View(depreciation);

        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Edit(Depreciation depreciation)
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


                depreciation.ModifiedDate = istDate;
                depreciation.Modified_Userid = userid;

                depreciation.DepreciationType = "M";
                depreciation.Companyid = companyid;

                Assets obj_assets;
                obj_assets = db.Assetss.Where(x => x.Companyid == companyid && x.AssetNo == depreciation.AssetNo).FirstOrDefault();
                if (obj_assets!= null)
                {
                    depreciation.AssetId = obj_assets.ID;
                    depreciation.AssetNo = obj_assets.AssetNo;
                }
                

                db.Entry(depreciation).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult DepreciationExport(int subperiodid)
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
            //Response.BinaryWrite(generateDepreciationsexcel(companyid));
            //string excelName = "DepreciationCompanyLaw";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();
            //return RedirectToAction("Index", "Depreciation");
            List<Depreciation> lstins = new List<Depreciation>();
            int srno = 1;

            SubPeriod subperiods = new SubPeriod();
            subperiods = db.SubPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
            if (subperiods != null)
            {
                lstins = db.Depreciations.Where(x => x.Companyid == companyid && x.FromDate == subperiods.FromDate && x.ToDate == subperiods.ToDate).ToList();
            }
            else
            {
                lstins = db.Depreciations.Where(x => x.Companyid == companyid).ToList();
            }
            if (lstins != null)
            {
                foreach (var item in lstins)
                {
                    item.int_Srno = srno;
                    item.AssetNo = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;
                    item.str_FromDate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
                    item.str_ToDate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy"); ;

                    srno++;
                }
            }
            var memoryStream = new MemoryStream();
            byte[] data;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[] { "Sr No","AssetNo ","Asset Name","From Date ","To Date",
                    "Amount", "Total Rate","Depreciation Method","Depreciation Days"

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
                    worksheet.Cells[rowIterator, 2].Value = item.AssetNo;

                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.str_FromDate;
                    worksheet.Cells[rowIterator, 5].Value = item.str_ToDate;
                    worksheet.Cells[rowIterator, 6].Value = item.Amount;
                    worksheet.Cells[rowIterator, 7].Value = item.TotalRate;
                    worksheet.Cells[rowIterator, 8].Value = item.DepreciationMethod;
                    worksheet.Cells[rowIterator, 9].Value = item.DepreciationDays;



                    rowIterator = rowIterator + 1;


                }

                string excelName = "Depreciationexport.xlsx";

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

        //public byte[] generateDepreciationsexcel(int companyid)
        //{
        //    List<Depreciation> lstins = new List<Depreciation>();
        //    int srno = 1;


        //    lstins = db.Depreciations.Where(x => x.Companyid == companyid).ToList();

        //    foreach (var item in lstins)
        //    {
        //        item.int_Srno = srno;
        //        item.AssetNo = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;
        //        item.str_FromDate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
        //        item.str_ToDate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy"); ;

        //        srno++;
        //    }

        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow = new string[] { "Sr No","AssetNo ","Asset Name","From Date ","To Date",
        //            "Amount", "Total Rate","Depreciation Method","Depreciation Days"

        //        };



        //        // Determine the header range (e.g. A1:D1)
        //        // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        // Popular header row data
        //        //  worksheet.Cells[headerRange].LoadFromArrays(headerRow);
        //        var excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
        //        // Popular header row data
        //        for (var i = 0; i < headerRow.Length; i++)
        //        {
        //            worksheet.Cells[1, i + 1].Value = headerRow[i];
        //        }
        //        int rowIterator = 2;
        //        foreach (var item in lstins)
        //        {
        //            worksheet.Cells[rowIterator, 1].Value = item.int_Srno;
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetNo;

        //            worksheet.Cells[rowIterator, 3].Value = item.AssetName;
        //            worksheet.Cells[rowIterator, 4].Value = item.str_FromDate;
        //            worksheet.Cells[rowIterator, 5].Value = item.str_ToDate;
        //            worksheet.Cells[rowIterator, 6].Value = item.Amount;
        //            worksheet.Cells[rowIterator, 7].Value = item.TotalRate;
        //            worksheet.Cells[rowIterator, 8].Value = item.DepreciationMethod;
        //            worksheet.Cells[rowIterator, 9].Value = item.DepreciationDays;



        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }
        //}


        //[HttpGet]
        //public ActionResult DownloadDepreciationExcel()
        //{
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

        //    Response.ClearContent();
        //    Response.BinaryWrite(generateImportDepreciationexcel());
        //    string excelName = "DepreciationImport";
        //    Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Flush();
        //    Response.End();
        //    return RedirectToAction("Index", "Disposal");

        //}

        //public byte[] generateImportDepreciationexcel()
        //{

        //    int srno = 1;

        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow = new string[]
        //        { "Sr No","AssetNo ","Asset Name","From Date ","To Date",
        //            "Amount", "Total Rate","Depreciation Method","Depreciation Days","Auto or Manual"

        //        };


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        var excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
        //        // Popular header row data
        //        for (var i = 0; i < headerRow.Length; i++)
        //        {
        //            worksheet.Cells[1, i + 1].Value = headerRow[i];
        //        }


        //        return excel.GetAsByteArray();

        //    }
        //}
        //[AuthUser]
        //[HttpGet]
        //public ActionResult ImportExcel()
        //{
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
        //    return PartialView();
        //}

        //[HttpPost]
        //[ValidateAjax]
        //public ActionResult UploadDepreciation()
        //{
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

        //    APIResponse response = new APIResponse();

        //    JsonResult res;
        //    res = new JsonResult();


        //    Depreciation depreciation = new Depreciation();

        //    try
        //    {
        //        if (Request != null)
        //        {
        //            bool norecordsfound = false;

        //            HttpPostedFileBase file;

        //            file = null;
        //            HttpFileCollectionBase files = Request.Files;

        //            if (files.Count > 0)
        //            {
        //                file = files[0];
        //                string fileName = file.FileName;

        //                string fileContentType = file.ContentType;
        //                byte[] fileBytes = new byte[file.ContentLength];
        //                Stream stream = file.InputStream;
        //                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

        //                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //                using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
        //                {
        //                    var currentSheet = package.Workbook.Worksheets;
        //                    var workSheet = currentSheet.First();
        //                    var noOfCol = workSheet.Dimension.End.Column;
        //                    var noOfRow = workSheet.Dimension.End.Row;
        //                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        //                    var tnow = System.DateTime.Now.ToUniversalTime();
        //                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);


        //                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
        //                    {
        //                        string a; a = ""; //SrNo
        //                        string b; b = ""; //AssetNo
        //                        string c; c = ""; //AssetNamr
        //                        string d; d = ""; //FromDate
        //                        string e; e = "";//ToDAte
        //                        string f; f = ""; //Amount

        //                        string g; g = ""; //TotalRate
        //                        string h; h = ""; //SLM/WDV
        //                        string i; i = ""; //days
        //                        string j; j = ""; //A/ M



        //                        bool srnoflag;
        //                        bool assetnoflag;
        //                        bool assetnameflag;
        //                        bool fromdateflag;
        //                        bool todateflag;
        //                        bool totalrateflag;
        //                        bool depreciationmethodflag;
        //                        bool amountflag;
        //                        bool depreciationdays;
        //                        bool autoormanual;

        //                        if (workSheet.Cells[rowIterator, 1].Text == "")
        //                        {
        //                            a = "";
        //                            srnoflag = false;
        //                        }
        //                        else
        //                        {
        //                            a = workSheet.Cells[rowIterator, 1].Value.ToString();
        //                            srnoflag = true;
        //                        }
        //                        if (workSheet.Cells[rowIterator, 2].Text == "")
        //                        {
        //                            b = "";
        //                            assetnoflag = false;
        //                        }
        //                        else
        //                        {
        //                            b = workSheet.Cells[rowIterator, 2].Value.ToString();
        //                            assetnoflag = true;
        //                        }



        //                        if (workSheet.Cells[rowIterator, 3].Text == "")
        //                        {
        //                            c = "";
        //                            assetnameflag = false;

        //                        }
        //                        else
        //                        {

        //                            c = workSheet.Cells[rowIterator, 3].Value.ToString();
        //                            assetnameflag = true;
        //                        }

        //                        if (workSheet.Cells[rowIterator, 4].Text == "")
        //                        {
        //                            d = "";
        //                            fromdateflag = false;
        //                        }
        //                        else
        //                        {
        //                            d = workSheet.Cells[rowIterator, 4].Value.ToString();
        //                            fromdateflag = true;
        //                        }

        //                        if (workSheet.Cells[rowIterator, 5].Text == "")
        //                        {
        //                            e = "";
        //                            todateflag = false;
        //                        }
        //                        else
        //                        {
        //                            e = workSheet.Cells[rowIterator, 5].Value.ToString();
        //                            todateflag = true;

        //                        }

        //                        if (workSheet.Cells[rowIterator, 6].Text == "")
        //                        {
        //                            f = "";
        //                            amountflag = false;
        //                        }
        //                        else
        //                        {
        //                            f = workSheet.Cells[rowIterator, 6].Value.ToString();
        //                            amountflag = true;
        //                        }

        //                        ////////////////////

        //                        if (workSheet.Cells[rowIterator, 7].Text == "")
        //                        {
        //                            g = "";
        //                            totalrateflag = false;
        //                        }
        //                        else
        //                        {
        //                            g = workSheet.Cells[rowIterator, 7].Value.ToString();
        //                            totalrateflag = true;
        //                        }
        //                        if (workSheet.Cells[rowIterator, 8].Text == "")
        //                        {
        //                            h = "";
        //                            depreciationmethodflag = false;
        //                        }
        //                        else
        //                        {
        //                            h = workSheet.Cells[rowIterator, 8].Value.ToString();
        //                            depreciationmethodflag = true;
        //                        }



        //                        if (workSheet.Cells[rowIterator, 9].Text == "")
        //                        {
        //                            i = "";
        //                            depreciationdays = false;

        //                        }
        //                        else
        //                        {

        //                            i = workSheet.Cells[rowIterator, 9].Value.ToString();
        //                            depreciationdays = true;
        //                        }

        //                        if (workSheet.Cells[rowIterator, 10].Text == "")
        //                        {
        //                            j = "";
        //                            autoormanual = false;
        //                        }
        //                        else
        //                        {
        //                            j = workSheet.Cells[rowIterator, 10].Value.ToString();
        //                            autoormanual = true;
        //                        }






        //                        if (srnoflag == true && assetnoflag == true && assetnameflag == true && fromdateflag == true
        //                            && todateflag == true && totalrateflag == true && depreciationmethodflag == true && amountflag == true
        //                            && depreciationdays == true && autoormanual == true)

        //                        {

        //                            if (d != "")
        //                            {
        //                                DateTime fromdate1 = Convert.ToDateTime(d);
        //                                depreciation.FromDate = fromdate1;
        //                            }
        //                            if (e != "")
        //                            {
        //                                DateTime todate1 = Convert.ToDateTime(e);
        //                                depreciation.ToDate = todate1;
        //                            }
        //                            depreciation.int_Srno = Convert.ToInt32(a);
        //                            depreciation.AssetNo = b;
        //                            depreciation.AssetName = c;
        //                            depreciation.TotalRate = Convert.ToInt32(g);
        //                            depreciation.DepreciationMethod = (h);
        //                            depreciation.Amount = Convert.ToInt32(f);

        //                            depreciation.DepreciationDays = Convert.ToInt32(i);
        //                            depreciation.DepreciationType = j;


        //                            depreciation.CreatedUserId = userid;
        //                            depreciation.CreatedDate = istDate;
        //                            depreciation.Companyid = companyid;

        //                            db.Depreciations.Add(depreciation);
        //                            db.SaveChanges();

        //                        }
        //                        norecordsfound = true;
        //                    }
        //                    if (norecordsfound == false)
        //                    {

        //                        res.Data = "nodata";
        //                        return res;

        //                    }
        //                    else
        //                    {
        //                        res.Data = "Success";
        //                        return res;


        //                    }

        //                }



        //            }
        //            else
        //            {
        //                res.Data = "error";
        //                return res;


        //            }
        //        }


        //        else
        //        {
        //            res.Data = "error";
        //            return res;


        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        string strError;
        //        strError = ex.Message + "|" + ex.InnerException;
        //        //  logger.Log(LogLevel.Error, strError);
        //        res.Data = "error";
        //        return res;
        //    }
        //}


        [HttpGet]
        public ActionResult CalculateDepreciation()
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

            //-------------------------------------------------
            SubPeriod lstpobjperiod = new SubPeriod();
            lstpobjperiod = db.SubPeriods.Where(x => x.DepFlag == "N" && x.Companyid == companyid).FirstOrDefault();
            if (lstpobjperiod != null)
            {
                ViewBag.fromdate = lstpobjperiod.FromDate.ToString("dd/MM/yyyy");
                ViewBag.todate = lstpobjperiod.ToDate.ToString("dd/MM/yyyy");
            }
            else
            {
                ViewBag.fromdate = "";
                ViewBag.todate = "";

                // lstpobjperiod.str_fromdate = "";
                //lstpobjperiod.str_todate = "";
            }
            ViewBag.userid = userid;
            //---------------------------------------------------

            return PartialView(lstpobjperiod);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult startcalculation(DateTime startdate, DateTime enddate)
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

            bool bresult=false;


            Task taskDep = Task.Run(() =>
           {
               BusinessLogic.DepreciationCalculationRepository reportRepository = new BusinessLogic.DepreciationCalculationRepository();
               bresult = reportRepository.StartCalculationRequest(companyid, startdate, enddate, userid);

           });

            

            return new JsonResult()
            {

                Data = new { result = bresult }
            };



        }


        [HttpGet]
        public ActionResult ShowProgress(DateTime fromdate, DateTime todate)
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

            ViewBag.fromdate = fromdate.ToString("dd/MM/yyyy");
            ViewBag.todate = todate.ToString("dd/MM/yyyy");

            List<DepreciationLog> depreciationLog = new List<DepreciationLog>();
            depreciationLog = db.DepreciationLog.ToList();
            if (depreciationLog.Count > 0)
            {
                ViewBag.count = depreciationLog.Count;
            }
            else
            {
                ViewBag.count = 0;
            }
            return View();
        }

        /*
        [HttpPost]
        public ActionResult Getdepreciation(string startdate, string enddate)
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


            List<Depreciation> DepList = new List<Depreciation>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.DepreciationCalculationRepository reportRepository = new BusinessLogic.DepreciationCalculationRepository();
            DepList = reportRepository.Getdepcal(companyid, startdate, enddate,userid);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetName", "FromDate ", "To Date", "Amount ", "DepreciationMethod", "DepreciationType", "DepreciationRate ", "DepreciationDays " };




                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string str_startdate = Convert.ToDateTime(startdate).ToString("dd/MM/yyyy");
                string str_enddate = Convert.ToDateTime(enddate).ToString("dd/MM/yyyy");


                worksheet.Cells[1, 1].Value = "Depreciation Report";

                worksheet.Cells[2, 2].Value = "StartDate:" + str_startdate + "  Enddate:" + str_enddate;
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in DepList)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 3].Value = item.str_FromDate;
                    worksheet.Cells[rowIterator, 4].Value = item.str_ToDate;
                    worksheet.Cells[rowIterator, 5].Value = item.Amount;
                    worksheet.Cells[rowIterator, 6].Value = item.DepreciationMethod;
                    worksheet.Cells[rowIterator, 7].Value = item.DepreciationType;
                    worksheet.Cells[rowIterator, 8].Value = item.TotalRate;
                    worksheet.Cells[rowIterator, 9].Value = item.DepreciationDays;



                    rowIterator = rowIterator + 1;

                }
                string excelName = "DepreciationReport.xlsx";

                string handle = Guid.NewGuid().ToString();
                excel.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();










                // Note we are returning a filename as well as the handle
                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = excelName }
                };
            }
        }
        */



        [HttpGet]
        public ActionResult checkdepreciationiscalculated(DateTime fromdate, DateTime todate)
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);
            JsonResult result = new JsonResult();
            //DateTime Fromdate = Convert.ToDateTime(fromdate);
            //DateTime Todate = Convert.ToDateTime(todate);
            var checkdepcal = "";


            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

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


            DepreciationRequest depreciationRequest = new DepreciationRequest();
            //            depreciationRequest = db.DepreciationRequest.Where(x => x.CompanyID == companyid && x.StartDate == Fromdate && x.EndDate == Todate).FirstOrDefault();


            int v_TotalRecords = 0;
            int v_TotalProcessed = 0;

            v_TotalRecords = db.DepCalculation.Count();
            v_TotalProcessed = db.DepreciationLog.Count();


            depreciationRequest = db.DepreciationRequest.Where(x => x.CompanyID == companyid
                    && x.StartDate == fromdate
                    && x.EndDate == todate).OrderByDescending(x => x.ID).FirstOrDefault();
            if (depreciationRequest != null)
            {

                if (depreciationRequest.InProcess == 2)
                {

                    checkdepcal = "Y";
                }
                else
                {
                    checkdepcal = "N";
                }

            }
            result = this.Json(new
            {
                TotalRecords = v_TotalRecords,
                TotalProcessed = v_TotalProcessed,
                IsComplete = checkdepcal
            }, JsonRequestBehavior.AllowGet);

            return result;

        }

    }
}