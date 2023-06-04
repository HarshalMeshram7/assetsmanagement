using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace VerifyWebApp.Controllers
{
    public class CostCenterScheduleController : Controller
    {
        // GET: CostCenterSchedule
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCCScheduleReport_Index()
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

            var fromdatelist = db.SubPeriods.Where(x => x.Companyid == companyid).ToList();
            foreach (SubPeriod item in fromdatelist)
            {
                item.str_fromdate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
            }
            var todatelist = db.SubPeriods.Where(x => x.Companyid == companyid).ToList();
            foreach (SubPeriod item in todatelist)
            {
                item.str_todate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy");
            }

            ViewBag.Fromdate = new SelectList(fromdatelist.OrderByDescending(e => e.ID), "str_fromdate", "str_fromdate");

            ViewBag.Todate = new SelectList(todatelist.OrderByDescending(e => e.ID), "str_todate", "str_todate");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public ActionResult GetCCScheduleReportData(DateTime FromDate, DateTime ToDate)
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

            List<Assets> alist = new List<Assets>();
            List<CostCenterScheduleViewmodel> CCList = new List<CostCenterScheduleViewmodel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.CostCenterScheduleRepository reportRepository = new BusinessLogic.CostCenterScheduleRepository();
            CCList = reportRepository.getCCSchedule(companyid, FromDate, ToDate);

            return Content(JsonConvert.SerializeObject(CCList), "application/json");

        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetCCScheduleReport(DateTime FromDate, DateTime ToDate)
        {
           // DateTime frmDate = Convert.ToDateTime(FromDate);
            //DateTime toDate = Convert.ToDateTime(ToDate);


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

            //Response.ClearContent();
            //Response.BinaryWrite(generateCCScheduledexcel(companyid, frmDate, toDate));
            //string excelName = "Cost Center Scheduled Report";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();



            //return RedirectToAction("GetCCScheduleReport", "GetCCSchedule");
            List<Assets> alist = new List<Assets>();
            List<CostCenterScheduleViewmodel> CCList = new List<CostCenterScheduleViewmodel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.CostCenterScheduleRepository reportRepository = new BusinessLogic.CostCenterScheduleRepository();
            CCList = reportRepository.getCCSchedule(companyid, FromDate, ToDate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");


                string[] headerRow = {"AGroupName", "BGroupName", "CGroupName ", "DGroupName","AssetNo", "AssetIdentificationNo ", "AssetName ", "VoucherDate ",
                    "OpGross","Addition ","Disposal","ClGross","OpDep","UpToDep","DispoDep","TotDep","NetBalance",
                    "VoucherNo ","BillNo","PONo","BillDate","DtPutUse(Company)","DepRate","DepMethod","Qty","Product Serial No","Model","Remarks",
                    "ALocName ","BLocName","CLocName","SupplierName","CCCode","CCDescription",};




                // Determine the header range (e.g. A1:D1)
                //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[1, 1].Value = "Cost Center Schedule Report";
                worksheet.Cells[2, 2].Value = "FromDate:  " + FromDate.ToString("dd/MM/yyyy") + " ToDate:" + ToDate.ToString("dd/MM/yyyy");

                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in CCList)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
                    worksheet.Cells[rowIterator, 2].Value = item.BGroupName;
                    worksheet.Cells[rowIterator, 3].Value = item.CGroupName;
                    worksheet.Cells[rowIterator, 4].Value = item.DGroupName;

                    worksheet.Cells[rowIterator, 5].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 6].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 7].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 8].Value = item.str_voucherdate;
                    worksheet.Cells[rowIterator, 9].Value = item.OpGross;
                    worksheet.Cells[rowIterator, 10].Value = item.Addition;
                    worksheet.Cells[rowIterator, 11].Value = item.Disposal;
                    worksheet.Cells[rowIterator, 12].Value = item.ClGross;
                    worksheet.Cells[rowIterator, 13].Value = item.OpDep;

                    worksheet.Cells[rowIterator, 14].Value = item.UpToDep;
                    worksheet.Cells[rowIterator, 15].Value = item.DispoDep;
                    worksheet.Cells[rowIterator, 16].Value = item.TotDep;
                    worksheet.Cells[rowIterator, 17].Value = item.NetBalance;
                    worksheet.Cells[rowIterator, 18].Value = item.VoucherNo;

                    worksheet.Cells[rowIterator, 19].Value = item.BillNo;
                    worksheet.Cells[rowIterator, 20].Value = item.PONo;
                    worksheet.Cells[rowIterator, 21].Value = item.str_billdate;
                    worksheet.Cells[rowIterator, 22].Value = item.str_dateputtousedate;
                    // worksheet.Cells[rowIterator, 23].Value = item.VoucherNo;

                    worksheet.Cells[rowIterator, 23].Value = item.DepRate;
                    worksheet.Cells[rowIterator, 24].Value = item.DepMethod;
                    worksheet.Cells[rowIterator, 25].Value = item.Qty;
                    worksheet.Cells[rowIterator, 26].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 27].Value = item.Model;

                    worksheet.Cells[rowIterator, 28].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 29].Value = item.ALocName;
                    worksheet.Cells[rowIterator, 30].Value = item.BLocName;
                    worksheet.Cells[rowIterator, 31].Value = item.CLocName;
                    worksheet.Cells[rowIterator, 32].Value = item.SupplierName;

                    worksheet.Cells[rowIterator, 33].Value = item.CCCode;
                    worksheet.Cells[rowIterator, 34].Value = item.CCDescription;



                    rowIterator = rowIterator + 1;

                }

                string excelName = "Costcenterschedulereport.xlsx";

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

        //public byte[] generateCCScheduledexcel(int companyid, DateTime frmDate, DateTime toDate)
        //{

        //    List<Assets> alist = new List<Assets>();
        //    List<CostCenterScheduleViewmodel> CCList = new List<CostCenterScheduleViewmodel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.CostCenterScheduleRepository reportRepository = new BusinessLogic.CostCenterScheduleRepository();
        //    CCList = reportRepository.getCCSchedule(companyid, frmDate, toDate);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");


        //        string[] headerRow = {"AGroupName", "BGroupName", "CGroupName ", "DGroupName","AssetNo", "AssetIdentificationNo ", "AssetName ", "VoucherDate ",
        //            "OpGross","Addition ","Disposal","ClGross","OpDep","UpToDep","DispoDep","TotDep","NetBalance",
        //            "VoucherNo ","BillNo","PONo","BillDate","DtPutUse(Company)","DepRate","DepMethod","Qty","Product Serial No","Model","Remarks",
        //            "ALocName ","BLocName","CLocName","SupplierName","CCCode","CCDescription",};




        //        // Determine the header range (e.g. A1:D1)
        //     //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        // Popular header row data
        //        worksheet.Cells[1, 1].Value = "Cost Center Schedule Report";
        //        worksheet.Cells[2, 2].Value = "FromDate:  " +frmDate.ToString("dd/MM/yyyy")+" ToDate:"+toDate.ToString("dd/MM/yyyy");

        //        int col = 1;
        //        for (int i = 0; i <= headerRow.Count() - 1; i++)
        //        {

        //            worksheet.Cells[4, col].Value = headerRow[i];
        //            col++;

        //        }
        //        int rowIterator = 5;
        //        foreach (var item in CCList)
        //        {
        //            worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
        //            worksheet.Cells[rowIterator, 2].Value = item.BGroupName;
        //            worksheet.Cells[rowIterator, 3].Value = item.CGroupName;
        //            worksheet.Cells[rowIterator, 4].Value = item.DGroupName;

        //            worksheet.Cells[rowIterator, 5].Value = item.AssetNo;
        //            worksheet.Cells[rowIterator, 6].Value = item.AssetIdentificationNo;
        //            worksheet.Cells[rowIterator, 7].Value = item.AssetName;
        //            worksheet.Cells[rowIterator, 8].Value = item.str_voucherdate;
        //            worksheet.Cells[rowIterator, 9].Value = item.OpGross;
        //            worksheet.Cells[rowIterator, 10].Value = item.Addition;
        //            worksheet.Cells[rowIterator, 11].Value = item.Disposal;
        //            worksheet.Cells[rowIterator, 12].Value = item.ClGross;
        //            worksheet.Cells[rowIterator, 13].Value = item.OpDep;

        //            worksheet.Cells[rowIterator, 14].Value = item.UpToDep;
        //            worksheet.Cells[rowIterator, 15].Value = item.DispoDep;
        //            worksheet.Cells[rowIterator, 16].Value = item.TotDep;
        //            worksheet.Cells[rowIterator, 17].Value = item.NetBalance;
        //            worksheet.Cells[rowIterator, 18].Value = item.VoucherNo;

        //            worksheet.Cells[rowIterator, 19].Value = item.BillNo;
        //            worksheet.Cells[rowIterator, 20].Value = item.PONo;
        //            worksheet.Cells[rowIterator, 21].Value = item.str_billdate;
        //            worksheet.Cells[rowIterator, 22].Value = item.str_dateputtousedate;
        //           // worksheet.Cells[rowIterator, 23].Value = item.VoucherNo;

        //            worksheet.Cells[rowIterator, 23].Value = item.DepRate;
        //            worksheet.Cells[rowIterator, 24].Value = item.DepMethod;
        //            worksheet.Cells[rowIterator, 25].Value = item.Qty;
        //            worksheet.Cells[rowIterator, 26].Value = item.SrNo;
        //            worksheet.Cells[rowIterator, 27].Value = item.Model;

        //            worksheet.Cells[rowIterator, 28].Value = item.Remarks;
        //            worksheet.Cells[rowIterator, 29].Value = item.ALocName;
        //            worksheet.Cells[rowIterator, 30].Value = item.BLocName;
        //            worksheet.Cells[rowIterator, 31].Value = item.CLocName;
        //            worksheet.Cells[rowIterator, 32].Value = item.SupplierName;

        //            worksheet.Cells[rowIterator, 33].Value = item.CCCode;
        //            worksheet.Cells[rowIterator, 34].Value = item.CCDescription;



        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}

    }
}