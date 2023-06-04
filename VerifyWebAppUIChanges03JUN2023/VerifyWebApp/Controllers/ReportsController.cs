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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VerifyWebApp.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports  public VerifyDB db = new VerifyDB();
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reports/Details/5
        public ActionResult AllLocationWiseReport_Index()
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

            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");


            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Alllocationreport(DateTime asondate)
        {
            //  DateTime Asondate = Convert.ToDateTime(asondate);
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
            //Response.BinaryWrite(generateallocationasseteexcel(companyid, Asondate));
            //string excelName = "getallocationasset";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();



            //return RedirectToAction("AllLocationWiseReport_Index", "Reports");


            // DateTime AsonDate;
            //= Convert.ToDateTime(asondate);
            //  string strasondate = Convert.ToDateTime(asondate).ToString("yyyy-MM-dd");
            //  if (DateTime.TryParseExact(strasondate, "yyyy-MM-dd",
            //                                                         System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out AsonDate))
            //{
            List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
            alist = reportRepository.getalllocationwiseasset(companyid, asondate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "Asset Identification NO","AssetName", "VoucherDate", "Date Put To Use ",
                    "SupplierName", "Qty", "Location", "SubLocation",
                    "Sub_SubLocation", "IssueDate", "Amount Capitalised" };



                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                worksheet.Cells[1, 1].Value = "All locations  Report";
                worksheet.Cells[2, 2].Value = "AsonDate:  " + asondate.ToString("dd/MM/yyyy");
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in alist)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;

                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentification;

                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;


                    worksheet.Cells[rowIterator, 4].Value = item.str_voucherdate;
                    worksheet.Cells[rowIterator, 5].Value = item.str_DtPutToUse;
                    worksheet.Cells[rowIterator, 6].Value = item.SupplierName;
                    worksheet.Cells[rowIterator, 7].Value = item.Qty;
                    worksheet.Cells[rowIterator, 8].Value = item.Location;
                    worksheet.Cells[rowIterator, 9].Value = item.Sublocation;
                    worksheet.Cells[rowIterator, 10].Value = item.Sub_Sublocation;
                    worksheet.Cells[rowIterator, 11].Value = item.str_issuedate;
                    worksheet.Cells[rowIterator, 12].Value = item.AmountCapitalised;
                    rowIterator = rowIterator + 1;

                }

                string excelName = "Alllocationreport.xlsx";

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
        //public byte[] generateallocationasseteexcel(int companyid, DateTime AsonDate)
        //{

        //    List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
        //    alist = reportRepository.getalllocationwiseasset(companyid, AsonDate);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow = { "AssetNo", "AssetName", "VoucherDate", "Date Put To Use ",
        //            "SupplierName", "Qty", "Location", "SubLocation",
        //            "Sub_SubLocation", "IssueDate", "Amount Capitalised" };



        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        worksheet.Cells[1, 1].Value = "All locations  Report";
        //        worksheet.Cells[2, 2].Value = "AsonDate:  " + AsonDate.ToString("dd/MM/yyyy");
        //        int col = 1;
        //        for (int i = 0; i <= headerRow.Count() - 1; i++)
        //        {

        //            worksheet.Cells[4, col].Value = headerRow[i];
        //            col++;

        //        }
        //        int rowIterator = 5;
        //        foreach (var item in alist)
        //        {
        //            worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetName;

        //            worksheet.Cells[rowIterator, 3].Value = item.str_voucherdate;
        //            worksheet.Cells[rowIterator, 4].Value = item.str_DtPutToUse;
        //            worksheet.Cells[rowIterator, 5].Value = item.SupplierName;
        //            worksheet.Cells[rowIterator, 6].Value = item.Qty;
        //            worksheet.Cells[rowIterator, 7].Value = item.Location;
        //            worksheet.Cells[rowIterator, 8].Value = item.Sublocation;
        //            worksheet.Cells[rowIterator, 9].Value = item.Sub_Sublocation;
        //            worksheet.Cells[rowIterator, 10].Value = item.str_issuedate;
        //            worksheet.Cells[rowIterator, 11].Value = item.AmountCapitalised;
        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}
        ///////////////////////////
        public ActionResult SingleLocationWiseReport_Index()
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

            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");
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
                // ViewBag.LoggedCompany = company.CompanyName;
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
                // ViewBag.LoggedCompany = company.CompanyName;
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
        public ActionResult Singlelocationreport(SingleLocationParameter singleLocationParameter)
        {
            // DateTime asondate = Convert.ToDateTime(singleLocationParameter.AsonDate);
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
            //Response.BinaryWrite(generatesinglelocationasseteexcel(companyid,singleLocationParameter));
            //string excelName = "getsinglelocationasset";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();


            //return RedirectToAction("SingleLocationWiseReport_Index","Reports");
            List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
            alist = reportRepository.getsinglelocationwiseasset(companyid, singleLocationParameter);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow =
                   { "AssetNo", "Asset Identification No","AssetName", "VoucherDate","Date Put To Use ","SupplierName","Qty",
                    "Location","SubLocation","Sub_SubLocation","IssueDate","Amount Capitalised" };

                string alocation = db.ALocations.Where(x => x.Companyid == companyid &&
                                    x.ID == singleLocationParameter.locationid).FirstOrDefault().ALocationName;
                string blocation = "";
                if (singleLocationParameter.sublocationid != 0)
                {
                    blocation = db.BLocations.Where(x => x.Companyid == companyid &&
                                   x.ID == singleLocationParameter.sublocationid).FirstOrDefault().BLocationName;
                }
                string clocation = "";
                if (singleLocationParameter.sub_sublocationid != 0)
                {
                    clocation = db.CLocations.Where(x => x.Companyid == companyid &&
                                   x.ID == singleLocationParameter.sub_sublocationid).FirstOrDefault().CLocationName;
                }


                // Determine the header range (e.g. A1:D1)
                //  string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                worksheet.Cells[1, 1].Value = "Single Location  Report";
                worksheet.Cells[2, 2].Value = "AsonDate:  " + singleLocationParameter.AsonDate;
                worksheet.Cells[3, 3].Value = "Location:" + alocation + "   Sub_location:" + blocation + "  Sub_Sublocation:" + clocation;
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in alist)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentification;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;

                    worksheet.Cells[rowIterator, 4].Value = item.str_voucherdate;
                    worksheet.Cells[rowIterator, 5].Value = item.str_DtPutToUse;
                    worksheet.Cells[rowIterator, 6].Value = item.SupplierName;
                    worksheet.Cells[rowIterator, 7].Value = item.Qty;
                    worksheet.Cells[rowIterator, 8].Value = item.Location;
                    worksheet.Cells[rowIterator, 9].Value = item.Sublocation;
                    worksheet.Cells[rowIterator, 10].Value = item.Sub_Sublocation;
                    worksheet.Cells[rowIterator, 11].Value = item.str_issuedate;
                    worksheet.Cells[rowIterator, 12].Value = item.AmountCapitalised;
                    rowIterator = rowIterator + 1;

                }

                string excelName = "Singlelocationreport.xlsx";

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
        //public byte[] generatesinglelocationasseteexcel(int companyid, SingleLocationParameter singleLocationParameter)
        //{

        //    List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
        //    alist = reportRepository.getsinglelocationwiseasset(companyid, singleLocationParameter);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow =
        //           { "AssetNo", "AssetName", "VoucherDate","Date Put To Use ","SupplierName","Qty",
        //            "Location","SubLocation","Sub_SubLocation","IssueDate","Amount Capitalised" };

        //        string alocation = db.ALocations.Where(x => x.Companyid == companyid &&
        //                            x.ID == singleLocationParameter.locationid).FirstOrDefault().ALocationName;
        //        string blocation = "";
        //        if (singleLocationParameter.sublocationid != 0)
        //        {
        //            blocation = db.BLocations.Where(x => x.Companyid == companyid &&
        //                           x.ID == singleLocationParameter.sublocationid).FirstOrDefault().BLocationName;
        //        }
        //        string clocation = "";
        //        if (singleLocationParameter.sub_sublocationid != 0)
        //        {
        //            clocation = db.CLocations.Where(x => x.Companyid == companyid &&
        //                           x.ID == singleLocationParameter.sub_sublocationid).FirstOrDefault().CLocationName;
        //        }


        //        // Determine the header range (e.g. A1:D1)
        //      //  string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        worksheet.Cells[1, 1].Value = "Single Location  Report";
        //        worksheet.Cells[2, 2].Value = "AsonDate:  " + singleLocationParameter.AsonDate;
        //        worksheet.Cells[3, 3].Value = "Location:" + alocation + "   Sub_location:" + blocation + "  Sub_Sublocation:" + clocation;
        //        int col = 1;
        //        for (int i = 0; i <= headerRow.Count() - 1; i++)
        //        {

        //            worksheet.Cells[4, col].Value = headerRow[i];
        //            col++;

        //        }
        //        int rowIterator = 5;
        //        foreach (var item in alist)
        //        {
        //            worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetName;

        //            worksheet.Cells[rowIterator, 3].Value = item.str_voucherdate;
        //            worksheet.Cells[rowIterator, 4].Value = item.str_DtPutToUse;
        //            worksheet.Cells[rowIterator, 5].Value = item.SupplierName;
        //            worksheet.Cells[rowIterator, 6].Value = item.Qty;
        //            worksheet.Cells[rowIterator, 7].Value = item.Location;
        //            worksheet.Cells[rowIterator, 8].Value = item.Sublocation;
        //            worksheet.Cells[rowIterator, 9].Value = item.Sub_Sublocation;
        //            worksheet.Cells[rowIterator, 10].Value = item.str_issuedate;
        //            worksheet.Cells[rowIterator, 11].Value = item.AmountCapitalised;
        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}

        ///////////////////////////////////


        public ActionResult Componential_Report_Index()
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

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Componentialreport(DateTime fromdate, DateTime todate)
        {
            // DateTime Fromdate = Convert.ToDateTime(fromdate);
            //DateTime Todate = Convert.ToDateTime(todate);
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
            //Response.BinaryWrite(generatecomponentialreportexcel(companyid,Fromdate,Todate ));
            //string excelName = "getcomponentialasset";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();



            //return RedirectToAction("Componential_Report_Index", "Reports");
            List<ComponentialReport_Viewmodel> clist = new List<ComponentialReport_Viewmodel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
            clist = reportRepository.getcomponentialasset(companyid, fromdate, todate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow =
                  { "Asset Date", "AssetNo"," Description", "Asset Usefullife", "Parent Asset Date",
                    "Parent AssetNo ", "Parent Asset Description", "Parent Asset Usefullife",
                    "Component Amt Capitalised", "Parent Asset Amt Capitalised" };



                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                worksheet.Cells[1, 1].Value = "Componential  Report";
                worksheet.Cells[2, 2].Value = "FromDate:  " + fromdate.ToString("dd/MM/yyyy") + "  ToDate: " + todate.ToString("dd/MM/yyyy");
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in clist)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.str_componentdate;

                    worksheet.Cells[rowIterator, 2].Value = item.Assetno;

                    worksheet.Cells[rowIterator, 3].Value = item.componentname;
                    worksheet.Cells[rowIterator, 4].Value = item.componentusefullife;
                    worksheet.Cells[rowIterator, 5].Value = item.str_parent_assetdate;
                    worksheet.Cells[rowIterator, 6].Value = item.ParentAssetno;
                    worksheet.Cells[rowIterator, 7].Value = item.parentassetname;
                    worksheet.Cells[rowIterator, 8].Value = item.parent_asset_usefullife;
                    worksheet.Cells[rowIterator, 9].Value = item.componentamtcapcomp;

                    worksheet.Cells[rowIterator, 10].Value = item.parent_asset_amtcapcomp;
                    rowIterator = rowIterator + 1;

                }

                string excelName = "Componentialreport.xlsx";

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
        public byte[] generatecomponentialreportexcel(int companyid, DateTime fromdate, DateTime todate)
        {

            List<ComponentialReport_Viewmodel> clist = new List<ComponentialReport_Viewmodel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
            clist = reportRepository.getcomponentialasset(companyid, fromdate, todate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow =
                  { "Asset Date", "AssetNo"," Description", "Asset Usefullife", "Parent Asset Date",
                    "Parent AssetNo ", "Parent Asset Description", "Parent Asset Usefullife",
                    "Component Amt Capitalised", "Parent Asset Amt Capitalised" };



                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                worksheet.Cells[1, 1].Value = "Componential  Report";
                worksheet.Cells[2, 2].Value = "FromDate:  " + fromdate.ToString("dd/MM/yyyy") + "  ToDate: " + todate.ToString("dd/MM/yyyy");
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in clist)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.str_componentdate;

                    worksheet.Cells[rowIterator, 2].Value = item.Assetno;

                    worksheet.Cells[rowIterator, 3].Value = item.componentname;
                    worksheet.Cells[rowIterator, 4].Value = item.componentusefullife;
                    worksheet.Cells[rowIterator, 5].Value = item.str_parent_assetdate;
                    worksheet.Cells[rowIterator, 6].Value = item.ParentAssetno;
                    worksheet.Cells[rowIterator, 7].Value = item.parentassetname;
                    worksheet.Cells[rowIterator, 8].Value = item.parent_asset_usefullife;
                    worksheet.Cells[rowIterator, 9].Value = item.componentamtcapcomp;

                    worksheet.Cells[rowIterator, 10].Value = item.parent_asset_amtcapcomp;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }

        }
        public ActionResult UsefullifeReport_Index()
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

            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");


            return View();
        }

        [HttpPost]

        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Usefulllifereport(string usefullifetype)
        {
            //  DateTime Asondate = Convert.ToDateTime(asondate);
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


            // List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
            List<UsefullifeReportViewModel> alist = new List<UsefullifeReportViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
            alist = reportRepository.getusefullifereport(companyid, usefullifetype);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetName","Date Put To Use ","ExpiryDate",
                    "AmountCapitalised",
                     "Depreciation"
                    };



                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;

                worksheet.Cells[1, 1].Value = "Usefullife Report";

                //worksheet.Cells[2, 2].Value = " " + asondate.ToString("dd/MM/yyyy");

                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in alist)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetName;

                    worksheet.Cells[rowIterator, 3].Value = item.str_Dateputtouse;
                    worksheet.Cells[rowIterator, 4].Value = item.str_ExpiryDate;
                    worksheet.Cells[rowIterator, 5].Value = item.AmountCapitalised;
                    worksheet.Cells[rowIterator, 6].Value = item.UpToDep;
                    /*   worksheet.Cells[rowIterator, 7].Value = item.Addition;
                       worksheet.Cells[rowIterator, 8].Value = item.Disposal;
                       worksheet.Cells[rowIterator, 9].Value = item.ClGross;
                       worksheet.Cells[rowIterator, 10].Value = item.OpDep;
                       worksheet.Cells[rowIterator, 11].Value = item.UpToDep;
                       worksheet.Cells[rowIterator, 12].Value = item.DispoDep;
                       worksheet.Cells[rowIterator, 13].Value = item.TotDep;
                       worksheet.Cells[rowIterator, 14].Value = item.NetBalance;*/
                    rowIterator = rowIterator + 1;

                }

                string excelName = "Usefulllifereport.xlsx";

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

        [HttpPost]
        public ActionResult Usefulllifereportdata(string usefullifetype)
        {
            //  DateTime Asondate = Convert.ToDateTime(asondate);
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


            // List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
            List<UsefullifeReportViewModel> alist = new List<UsefullifeReportViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();
            alist = reportRepository.getusefullifereport(companyid, usefullifetype);

            return Content(JsonConvert.SerializeObject(alist), "application/json");

        }

        public ActionResult AssetAdditionReport()
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


            return View("AssetAdditionReport");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAssetAdditionReport(DateTime FromDate, DateTime ToDate)
        {
            DateTime asondate = DateTime.Now;


            // DateTime Asondate = Convert.ToDateTime(asondate);

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
            List<FARReportViewmodel> FARList = new List<FARReportViewmodel>();


            //   BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();
            BusinessLogic.ReportRepository  reportRepository = new BusinessLogic.ReportRepository();
            
            FARList = reportRepository.getAssetAdditionsReport(companyid, FromDate, ToDate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                // Amount  = Addition

                string[] headerRow = {"AGroupName", "BGroupName", "CGroupName ", "DGroupName","AssetNo",  "AssetName ", "VoucherDate ",
                    "Amount","DepForPeriod","BillNo","PONo","BillDate","DtPutUse(Company)",
                     "Qty","Product Serial No","Model","Remarks",
                    "ALocName ","BLocName","CLocName","SupplierName",};




                // Determine the header range (e.g. A1:D1)
                //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet




                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[1, 1].Value = "Asset Additions Report";
                worksheet.Cells[2, 2].Value = "FromDate:  " + FromDate.ToString("dd/MM/yyyy") + " ToDate:" + ToDate.ToString("dd/MM/yyyy");

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

                    worksheet.Cells[rowIterator, 5].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 6].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 7].Value = item.str_voucherdate;
                    
                    worksheet.Cells[rowIterator, 8].Value = item.Addition;
                    worksheet.Cells[rowIterator, 9].Value = item.TotDep; // Dep For Period

                    


                    worksheet.Cells[rowIterator, 10].Value = item.BillNo;
                    worksheet.Cells[rowIterator, 11].Value = item.PONo;


                    if (item.BillDate.HasValue)
                    {
                        DateTime dtBillDate = item.BillDate.Value.Date;
                        worksheet.Cells[rowIterator, 12].Value = dtBillDate.Date.ToString("dd/MM/yyyy");
                        // worksheet.Cells[rowIterator, 22].Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    else
                    {
                        worksheet.Cells[rowIterator, 12].Value = "";
                    }

                    // worksheet.Cells[rowIterator, 22].Value = item.BillDate;



                    if (item.DTPutUse.HasValue)
                    {
                        DateTime dtDTPutUse = item.DTPutUse.Value.Date;
                        worksheet.Cells[rowIterator, 13].Value = dtDTPutUse.ToString("dd/MM/yyyy");
                        // worksheet.Cells[rowIterator, 23].Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    else
                    {
                        worksheet.Cells[rowIterator, 13].Value = "";
                    }




               
                    worksheet.Cells[rowIterator, 14].Value = item.Qty;


                    worksheet.Cells[rowIterator, 15].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 16].Value = item.Model;

                    worksheet.Cells[rowIterator, 17].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 18].Value = item.ALocName;
                    worksheet.Cells[rowIterator, 19].Value = item.BLocName;
                    worksheet.Cells[rowIterator, 20].Value = item.CLocName;
                    worksheet.Cells[rowIterator, 21].Value = item.SupplierName;




                    rowIterator = rowIterator + 1;

                }

                string excelName = "FARReport.xlsx";

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

        [HttpPost]
        public ActionResult GetAssetAdditionReportData(DateTime FromDate, DateTime ToDate)
        {
            DateTime asondate = DateTime.Now;


            // DateTime Asondate = Convert.ToDateTime(asondate);

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
            List<FARReportViewmodel> FARList = new List<FARReportViewmodel>();


            //   BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();

            FARList = reportRepository.getAssetAdditionsReport(companyid, FromDate, ToDate);

            return Content(JsonConvert.SerializeObject(FARList), "application/json");

        }


        public ActionResult AssetDisposalReport()
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


            return View("AssetDisposalReport");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetAssetDispoalReport(DateTime FromDate, DateTime ToDate)
        {
            DateTime asondate = DateTime.Now;


            // DateTime Asondate = Convert.ToDateTime(asondate);

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
            // List<FARReportViewmodel> FARList = new List<FARReportViewmodel>();
            List<DisposalReportViewModel> list = new List<DisposalReportViewModel>();


            //   BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();

            list = reportRepository.getAssetDisposalReport(companyid, FromDate, ToDate);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                // Amount  = Addition
                /*
                string[] headerRow = {"AGroupName", "BGroupName", "CGroupName ", "DGroupName","AssetNo",  "AssetName ", "VoucherDate ",
                    "Amount","BillNo","PONo","BillDate","DtPutUse(Company)",
                     "Qty","Product Serial No","Model","Remarks",
                    "ALocName ","BLocName","CLocName","SupplierName",};
                    */

                string[] headerRow = {"AGroupName", "BGroupName", "CGroupName", "DGroupName", "AssetNo",  
                        "AssetIdentificationNo", "AssetName", "Disposal", "DispoDep", "Qty", "SrNo", "Model", "ALocName",
                      "BLocName", "CLocName","DisposalDate" };

                // Determine the header range (e.g. A1:D1)
                //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet




                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[1, 1].Value = "Asset Disposal  Report";
                worksheet.Cells[2, 2].Value = "FromDate:  " + FromDate.ToString("dd/MM/yyyy") + " ToDate:" + ToDate.ToString("dd/MM/yyyy");

                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;




                foreach (var item in list)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
                    worksheet.Cells[rowIterator, 2].Value = item.BGroupName;
                    worksheet.Cells[rowIterator, 3].Value = item.CGroupName;
                    worksheet.Cells[rowIterator, 4].Value = item.DGroupName;

                    worksheet.Cells[rowIterator, 5].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 6].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 7].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 8].Value = item.Disposal;
                    worksheet.Cells[rowIterator, 9].Value = item.DispoDep;
                    worksheet.Cells[rowIterator, 10].Value = item.Qty;

                    worksheet.Cells[rowIterator, 11].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 12].Value = item.Model;

                    worksheet.Cells[rowIterator, 13].Value = item.ALocName;
                    worksheet.Cells[rowIterator, 14].Value = item.BLocName;
                    worksheet.Cells[rowIterator, 15].Value = item.CLocName;


                    if (item.DisposalDate.HasValue)
                    {
                        DateTime dtDTPutUse = item.DisposalDate.Value.Date;
                        worksheet.Cells[rowIterator, 16].Value = dtDTPutUse.ToString("dd/MM/yyyy");
                        // worksheet.Cells[rowIterator, 23].Style.Numberformat.Format = "dd/mm/yyyy";
                    }
                    else
                    {
                        worksheet.Cells[rowIterator, 16].Value = "";
                    }



                    rowIterator = rowIterator + 1;

                }

                string excelName = "AssetDisposalReport.xlsx";

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

        [HttpPost]
        public ActionResult GetAssetDisposalReportData(DateTime FromDate, DateTime ToDate)
        {
            DateTime asondate = DateTime.Now;


            // DateTime Asondate = Convert.ToDateTime(asondate);

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
            // List<FARReportViewmodel> FARList = new List<FARReportViewmodel>();
            List<DisposalReportViewModel> list = new List<DisposalReportViewModel>();


            //   BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();
            BusinessLogic.ReportRepository reportRepository = new BusinessLogic.ReportRepository();

            list = reportRepository.getAssetDisposalReport(companyid, FromDate, ToDate);


            return Content(JsonConvert.SerializeObject(list), "application/json");
        }
    }













}
