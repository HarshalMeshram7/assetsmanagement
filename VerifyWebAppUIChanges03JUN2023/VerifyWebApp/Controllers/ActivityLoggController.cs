using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using VerifyWebApp.Filter;
using System.IO;

namespace VerifyWebApp.Controllers
{
    public class ActivityLoggController : Controller
    {
        // GET: ActivityLogg
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult ActivityLog_Index()
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

            var fromdatelist = db.AuditLogs.Where(x => x.userid == userid).ToList();
            foreach (AuditLogRecord item in fromdatelist)
            {
                item.str_fromdate = Convert.ToDateTime(item.trandate).ToString("dd/MM/yyyy");
            }
            var todatelist = db.AuditLogs.Where(x => x.userid == userid).ToList();
            foreach (AuditLogRecord item in todatelist)
            {
                item.str_todate = Convert.ToDateTime(item.trandate).ToString("dd/MM/yyyy");
            }

            ViewBag.Fromdate = new SelectList(fromdatelist.OrderByDescending(e => e.trandate), "str_fromdate", "str_fromdate");

            ViewBag.Todate = new SelectList(todatelist.OrderByDescending(e => e.trandate), "str_todate", "str_todate");


            return View();
        }

       

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult ActivityLog_Index(DateTime FromDate, DateTime ToDate)
        //{

        //    try
        //    {
        //        DateTime asondate = DateTime.Now;


        //        // DateTime Asondate = Convert.ToDateTime(asondate);

        //        int userid = 0;
        //        Login user = (Login)(Session["PUser"]);

        //        if (user != null)
        //        {
        //            ViewBag.LogonUser = user.UserName;
        //            userid = user.ID;
        //        }
        //        else
        //        {
        //            return RedirectToAction("Login", "Login");
        //        }
        //        int companyid = 0;
        //        Company company = (Company)(Session["Cid"]);

        //        if (company != null)
        //        {
        //            ViewBag.LoggedCompany = company.CompanyName;
        //            companyid = company.ID;
        //            ViewBag.companyid = companyid;
        //            //ViewBag.LoggedCompany = company.CompanyName;
        //        }
        //        else
        //        {
        //            return RedirectToAction("CompanySelection", "Company");
        //        }

        //        List<Assets> alist = new List<Assets>();
        //        List<FARReportViewmodel> FARList = new List<FARReportViewmodel>();


        //        BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();

        //        FARList = reportRepository.getFAR_New(companyid, FromDate, ToDate);


        //        var memoryStream = new MemoryStream();
        //        byte[] data;
        //        //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //        using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //        {

        //            excel.Workbook.Worksheets.Add("Worksheet1");
        //            excel.Workbook.Worksheets.Add("Worksheet2");


        //            string[] headerRow = {"AGroupName", "BGroupName", "CGroupName ", "DGroupName","AssetNo", "AssetIdentificationNo ", "AssetName ", "VoucherDate ",
        //            "OpGross","Addition ","Disposal","ClGross","OpDep","DepForPeriod","DispoDep","TotDep","NetBalance",
        //            "ResidualVal",
        //            "VoucherNo","VoucherDate", "BillNo","PONo","BillDate","DtPutUse(Company)","DepRate","DepMethod",
        //            "Opening Qty","DisposedQtyTillFromDate", "Disposed Qty", "Closing Qty", "Product Serial No","Model","Remarks",
        //            "ALocName ","BLocName","CLocName","SupplierName",};




        //            // Determine the header range (e.g. A1:D1)
        //            //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //            // Target a worksheet




        //            var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //            var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //            var currentSheet = excel.Workbook.Worksheets;
        //            // Popular header row data
        //            worksheet.Cells[1, 1].Value = "Fixed Asset Register Report";
        //            worksheet.Cells[2, 2].Value = "FromDate:  " + FromDate.ToString("dd/MM/yyyy") + " ToDate:" + ToDate.ToString("dd/MM/yyyy");

        //            int col = 1;
        //            for (int i = 0; i <= headerRow.Count() - 1; i++)
        //            {

        //                worksheet.Cells[4, col].Value = headerRow[i];
        //                col++;

        //            }
        //            int rowIterator = 5;




        //            foreach (var item in FARList)
        //            {
        //                worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
        //                worksheet.Cells[rowIterator, 2].Value = item.BGroupName;
        //                worksheet.Cells[rowIterator, 3].Value = item.CGroupName;
        //                worksheet.Cells[rowIterator, 4].Value = item.DGroupName;

        //                worksheet.Cells[rowIterator, 5].Value = item.AssetNo;
        //                worksheet.Cells[rowIterator, 6].Value = item.AssetIdentificationNo;
        //                worksheet.Cells[rowIterator, 7].Value = item.AssetName;
        //                worksheet.Cells[rowIterator, 8].Value = item.str_voucherdate;
        //                worksheet.Cells[rowIterator, 9].Value = item.OpGross;
        //                worksheet.Cells[rowIterator, 10].Value = item.Addition;
        //                worksheet.Cells[rowIterator, 11].Value = item.Disposal;
        //                worksheet.Cells[rowIterator, 12].Value = item.ClGross;
        //                worksheet.Cells[rowIterator, 13].Value = item.OpDep;

        //                worksheet.Cells[rowIterator, 14].Value = item.UpToDep;
        //                worksheet.Cells[rowIterator, 15].Value = item.DispoDep;
        //                worksheet.Cells[rowIterator, 16].Value = item.TotDep;
        //                worksheet.Cells[rowIterator, 17].Value = item.NetBalance;



        //                worksheet.Cells[rowIterator, 18].Value = item.ResidualVal;


        //                worksheet.Cells[rowIterator, 19].Value = item.VoucherNo;



        //                if (item.voucherDate.HasValue)
        //                {
        //                    DateTime dtVouucherDate = item.voucherDate.Value.Date;

        //                    worksheet.Cells[rowIterator, 20].Value = dtVouucherDate.ToString("dd/MM/yyyy");
        //                    // worksheet.Cells[rowIterator, 19].Style.Numberformat.Format = "dd/mm/yyyy";

        //                }
        //                else
        //                {
        //                    worksheet.Cells[rowIterator, 20].Value = "";
        //                }


        //                worksheet.Cells[rowIterator, 21].Value = item.BillNo;
        //                worksheet.Cells[rowIterator, 22].Value = item.PONo;


        //                if (item.BillDate.HasValue)
        //                {
        //                    DateTime dtBillDate = item.BillDate.Value.Date;
        //                    worksheet.Cells[rowIterator, 23].Value = dtBillDate.Date.ToString("dd/MM/yyyy");

        //                }
        //                else
        //                {
        //                    worksheet.Cells[rowIterator, 23].Value = "";
        //                }





        //                if (item.DTPutUse.HasValue)
        //                {
        //                    DateTime dtDTPutUse = item.DTPutUse.Value.Date;
        //                    worksheet.Cells[rowIterator, 24].Value = dtDTPutUse.ToString("dd/MM/yyyy");
        //                    // worksheet.Cells[rowIterator, 23].Style.Numberformat.Format = "dd/mm/yyyy";
        //                }
        //                else
        //                {
        //                    worksheet.Cells[rowIterator, 24].Value = "";
        //                }

        //                worksheet.Cells[rowIterator, 25].Value = item.DepRate;
        //                worksheet.Cells[rowIterator, 26].Value = item.DepMethod;

        //                worksheet.Cells[rowIterator, 27].Value = item.OpeningQty;
        //                worksheet.Cells[rowIterator, 28].Value = item.DisposedQtyTillFromDate;
        //                worksheet.Cells[rowIterator, 29].Value = item.DisposedQty;
        //                worksheet.Cells[rowIterator, 30].Value = item.ClosingQty;
        //                worksheet.Cells[rowIterator, 31].Value = item.SrNo;


        //                //worksheet.Cells[rowIterator, 32].Value = item.SrNo;

        //                worksheet.Cells[rowIterator, 32].Value = item.Model;

        //                worksheet.Cells[rowIterator, 33].Value = item.Remarks;
        //                worksheet.Cells[rowIterator, 34].Value = item.ALocName;
        //                worksheet.Cells[rowIterator, 35].Value = item.BLocName;
        //                worksheet.Cells[rowIterator, 36].Value = item.CLocName;
        //                worksheet.Cells[rowIterator, 37].Value = item.SupplierName;




        //                rowIterator = rowIterator + 1;

        //            }


        //            string excelName = "activitylog.xlsx";

        //            string handle = Guid.NewGuid().ToString();
        //            excel.SaveAs(memoryStream);
        //            memoryStream.Position = 0;
        //            TempData[handle] = memoryStream.ToArray();


        //            return new JsonResult()
        //            {
        //                Data = new { FileGuid = handle, FileName = excelName }
        //            };

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // log error to database 
        //        int i = 0;

        //        //return View("Error");
        //        return View("~/Views/Shared/Error.cshtm");


        //    }

        //}




    }
}