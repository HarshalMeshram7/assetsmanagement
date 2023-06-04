using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class FASSummaryController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: FASSummary
        public ActionResult Index()
        {
            return View();
        }

        [AuthUser]
        public ActionResult FASSummaryDetailed_Index()
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

            ViewBag.Fromdate = new SelectList(fromdatelist.OrderByDescending(e => e.ID), "str_fromdate", "str_fromdate") ;
            ViewBag.Todate = new SelectList(todatelist.OrderByDescending(e => e.ID), "str_todate", "str_todate") ;



            return View();

        }
      
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult FASSummaryDetailedReport(DateTime Fromdate,DateTime Todate)
        {

           // DateTime frmDate = Convert.ToDateTime(Fromdate);
            //DateTime toDate = Convert.ToDateTime(Todate);

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
            List<FASSummaryDetailViewmodel> FARList = new List<FASSummaryDetailViewmodel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.FASSummaryRepository reportRepository = new BusinessLogic.FASSummaryRepository();
            FARList = reportRepository.getFASSummaryDetail(companyid, Fromdate, Todate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow =
                {"Main Group","Sub-MainGroup","Sub- Sub-MainGroup","Sub-sub-subMainGroup","Opening Gross block", "Additions ", "Disposals","Closing Gross block", "Opening Depreciation ", "Depreciation for the period ", "Depreciation on Disposal ",
                    "Total Depreciation ","Net Block"};




                // Determine the header range (e.g. A1:D1)
                //  string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                worksheet.Cells[1, 1].Value = "FAS Summary  Schedule Detail Report";
                worksheet.Cells[2, 2].Value = "FromDate:  " + Fromdate.ToString("dd/MM/yyyy") + "ToDate:  " + Fromdate.ToString("dd/MM/yyyy");
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in FARList)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
                    worksheet.Cells[rowIterator, 2].Value = item.BGroupName;
                    worksheet.Cells[rowIterator, 3].Value = item.CGroupName;
                    worksheet.Cells[rowIterator, 4].Value = item.DGroupName;
                    worksheet.Cells[rowIterator, 5].Value = item.OpGross;
                    worksheet.Cells[rowIterator, 6].Value = item.Addition;
                    worksheet.Cells[rowIterator, 7].Value = item.Disposal;
                    worksheet.Cells[rowIterator, 8].Value = item.ClGross;
                    worksheet.Cells[rowIterator, 9].Value = item.OpDep;
                    worksheet.Cells[rowIterator, 10].Value = item.UpToDep;
                    worksheet.Cells[rowIterator, 11].Value = item.DispoDep;
                    worksheet.Cells[rowIterator, 12].Value = item.TotDep;
                    worksheet.Cells[rowIterator, 13].Value = item.NetBalance;

                    rowIterator = rowIterator + 1;

                }

                string excelName = "FASSUMMARYDETAIL.xlsx";

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


        //public byte[] generateFASSummaryDetailedexcel(int companyid, DateTime frmDate, DateTime toDate)
        //{

        //    List<Assets> alist = new List<Assets>();
        //    List<FASSummaryDetailViewmodel> FARList = new List<FASSummaryDetailViewmodel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.FASSummaryRepository reportRepository = new BusinessLogic.FASSummaryRepository();
        //    FARList =reportRepository.getFASSummaryDetail(companyid, frmDate, toDate);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow =
        //        {"AGroupName", "OpGross", "Addition ", "Disposal","ClGross", "OpDep ", "UpToDep ", "DispoDep ",
        //            "TotDep ","NetBalance"};




        //        // Determine the header range (e.g. A1:D1)
        //      //  string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        worksheet.Cells[1, 1].Value = "FAS Summary Schedule Report";
        //        worksheet.Cells[2, 2].Value = "FromDate:  " + frmDate.ToString("dd/MM/yyyy")  +"ToDate:  "+toDate.ToString("dd/MM/yyyy");
        //        int col = 1;
        //        for (int i = 0; i <= headerRow.Count() - 1; i++)
        //        {

        //            worksheet.Cells[4, col].Value = headerRow[i];
        //            col++;

        //        }
        //        int rowIterator = 5;
        //        foreach (var item in FARList)
        //        {
        //            worksheet.Cells[rowIterator, 1].Value = item.agroupid;
        //            worksheet.Cells[rowIterator, 2].Value = item.OpGross;
        //            worksheet.Cells[rowIterator, 3].Value = item.Addition;
        //            worksheet.Cells[rowIterator, 4].Value = item.Disposal;
        //            worksheet.Cells[rowIterator, 5].Value = item.ClGross;
        //            worksheet.Cells[rowIterator, 6].Value = item.OpDep;
        //            worksheet.Cells[rowIterator, 7].Value = item.UpToDep;
        //            worksheet.Cells[rowIterator, 8].Value = item.DispoDep;
        //            worksheet.Cells[rowIterator, 9].Value = item.TotDep;
        //            worksheet.Cells[rowIterator, 10].Value = item.NetBalance;

        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}
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

        [AuthUser]
        public ActionResult GetFASSummaryReport_Index()
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
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetFASSummaryReport(string FromDate, string ToDate)
        {
            DateTime frmDate = Convert.ToDateTime(FromDate);
            DateTime toDate = Convert.ToDateTime(ToDate);


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
            List<FASSummaryViewmodel> FARList = new List<FASSummaryViewmodel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.FASSummaryRepository reportRepository = new BusinessLogic.FASSummaryRepository();
            FARList = reportRepository.getFASSummary(companyid, frmDate, toDate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow =  {"Main Group", "Opening Gross block", "Additions ", "Disposals","Closing Gross block", "Opening Depreciation ", "Depreciation for the period ", "Depreciation on Disposal ",
                    "Total Depreciation ","Net Block"};





                // Determine the header range (e.g. A1:D1)
                //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                worksheet.Cells[1, 1].Value = "FAS Summary  Report";
                worksheet.Cells[2, 2].Value = "FromDate:  " + frmDate.ToString("dd/MM/yyyy") + "ToDate:  " + toDate.ToString("dd/MM/yyyy");
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in FARList)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
                    worksheet.Cells[rowIterator, 2].Value = item.OpGross;
                    worksheet.Cells[rowIterator, 3].Value = item.Addition;
                    worksheet.Cells[rowIterator, 4].Value = item.Disposal;
                    worksheet.Cells[rowIterator, 5].Value = item.ClGross;
                    worksheet.Cells[rowIterator, 6].Value = item.OpDep;
                    worksheet.Cells[rowIterator, 7].Value = item.UpToDep;
                    worksheet.Cells[rowIterator, 8].Value = item.DispoDep;
                    worksheet.Cells[rowIterator, 9].Value = item.TotDep;
                    worksheet.Cells[rowIterator, 10].Value = item.NetBalance;

                    rowIterator = rowIterator + 1;

                }

                string excelName = "FASSUMMARYReport.xlsx";

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
        //    public byte[] generateFASSummaryexcel(int companyid, DateTime frmDate, DateTime toDate)
        //{

        //    List<Assets> alist = new List<Assets>();
        //    List<FASSummaryViewmodel> FARList = new List<FASSummaryViewmodel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.FASSummaryRepository reportRepository = new BusinessLogic.FASSummaryRepository();
        //    FARList = reportRepository.getFASSummary(companyid, frmDate, toDate);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow =  {"AGroupName", "OpGross", "Addition ", "Disposal","ClGross", "OpDep ", "UpToDep ", "DispoDep ",
        //            "TotDep ","NetBalance"};





        //        // Determine the header range (e.g. A1:D1)
        //        //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        worksheet.Cells[1, 1].Value = "FAS Summary  Report";
        //        worksheet.Cells[2, 2].Value = "FromDate:  " + frmDate.ToString("dd/MM/yyyy") + "ToDate:  " + toDate.ToString("dd/MM/yyyy");
        //        int col = 1;
        //        for (int i = 0; i <= headerRow.Count() - 1; i++)
        //        {

        //            worksheet.Cells[4, col].Value = headerRow[i];
        //            col++;

        //        }
        //        int rowIterator = 5;
        //        foreach (var item in FARList)
        //        {
        //            worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
        //            worksheet.Cells[rowIterator, 2].Value = item.OpGross;
        //            worksheet.Cells[rowIterator, 3].Value = item.Addition;
        //            worksheet.Cells[rowIterator, 4].Value = item.Disposal;
        //            worksheet.Cells[rowIterator, 5].Value = item.ClGross;
        //            worksheet.Cells[rowIterator, 6].Value = item.OpDep;
        //            worksheet.Cells[rowIterator, 7].Value = item.UpToDep;
        //            worksheet.Cells[rowIterator, 8].Value = item.DispoDep;
        //            worksheet.Cells[rowIterator, 9].Value = item.TotDep;
        //            worksheet.Cells[rowIterator, 10].Value = item.NetBalance;

        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}

   
    }
}
