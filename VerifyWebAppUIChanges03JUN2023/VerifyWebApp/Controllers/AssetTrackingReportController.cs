using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class AssetTrackingReportController : Controller
    {
        // GET: AssetTrackingReport
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }
        [AuthUser]
        public ActionResult AssetTrackinReport_Index()
        {
            string BetaVer = ConfigurationManager.AppSettings["BetaVersion"];

            if (BetaVer == "true")
            {
                ModelState.AddModelError(string.Empty, "Report download is restricted in the beta version.");
                return RedirectToAction("Index", "Home");
            }

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

            ViewBag.location = new SelectList(db.ALocations.Where(x => x.Companyid == companyid ).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.fromassetno = new SelectList(db.Assetss.Where(x => x.Companyid == companyid && x.DisposalFlag==0).OrderBy(e => e.ID), "AssetNo", "AssetNo");
            ViewBag.toassetno = new SelectList(db.Assetss.Where(x => x.Companyid == companyid && x.DisposalFlag == 0).OrderBy(e => e.ID), "AssetNo", "AssetNo");
            return View();
        }

        //[HttpGet]
        //public ActionResult GetAssetsTrackingReport(string startdate, string enddate, string fromassetno, string toassetno, int alocid)
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
        //        //ViewBag.LoggedCompany = company.CompanyName;
        //    }
        //    else
        //    {
        //        return RedirectToAction("CompanySelection", "Company");
        //    }

        //    Response.ClearContent();
        //    Response.BinaryWrite(generateAssettrackingexcel(companyid, startdate,enddate,fromassetno,toassetno,alocid));
        //    string excelName = " Assets Tracking Report";
        //    Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Flush();
        //    Response.End();



        //    return RedirectToAction("AssetTrackinReport_Index", "AssetTrackingReport");

        //}

        //public byte[] generateAssettrackingexcel(int companyid,string startdate,string enddate,string fromassetno,string toassetno, int alocid)
        //{

           
        //    List<AssetTrackingViewmodel> FARList = new List<AssetTrackingViewmodel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.AssetTrackinReportRepository reportRepository = new BusinessLogic.AssetTrackinReportRepository();
        //    FARList = reportRepository.getAssettracking(companyid, startdate,enddate,fromassetno,toassetno,alocid);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "Issue Date","Location ","SystemAssetid","SrNo", "Model ", "Remarks " };




        //        // Determine the header range (e.g. A1:D1)
        //        // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        string str_startdate = startdate;
        //        string str_enddate = enddate;
                
        //        string alocname = db.ALocations.Where(x => x.Companyid == companyid && x.ID == alocid).FirstOrDefault().ALocationName;
        //        worksheet.Cells[1, 1].Value = "Assets Tracking Report";

        //        worksheet.Cells[2, 2].Value = "StartDate:"+str_startdate+"  Enddate:"+str_enddate+"   Fromassetno:"+fromassetno
        //                                      +"Toassetno:"+toassetno +" location:  " + alocname;
        //        int col = 1;
        //        for (int i = 0; i <= headerRow.Count() - 1; i++)
        //        {

        //            worksheet.Cells[4, col].Value = headerRow[i];
        //            col++;

        //        }
        //        int rowIterator = 5;
        //        foreach (var item in FARList)
        //        {
        //            worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationNo;
        //            worksheet.Cells[rowIterator, 3].Value = item.AssetName;
        //            worksheet.Cells[rowIterator, 4].Value = item.str_IssueDate;
        //            worksheet.Cells[rowIterator, 5].Value = item.ALocName;
        //            worksheet.Cells[rowIterator, 6].Value = item.SystemAssetId;
        //            worksheet.Cells[rowIterator, 7].Value = item.SrNo;
        //            worksheet.Cells[rowIterator, 8].Value = item.Model;
        //            worksheet.Cells[rowIterator, 9].Value = item.Remarks;
                   


        //            rowIterator = rowIterator + 1;

        //        }
        //        string excelName = " Assets Tracking Report";
        //        Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        return excel.GetAsByteArray();

        //    }

        //}
        [HttpPost]
        public ActionResult GetAssetsTrackingReport(DateTime? startdate, DateTime? enddate, string fromassetno, string toassetno, int alocid)
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



            List<AssetTrackingViewmodel> FARList = new List<AssetTrackingViewmodel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.AssetTrackinReportRepository reportRepository = new BusinessLogic.AssetTrackinReportRepository();
            FARList = reportRepository.getAssettracking(companyid, startdate, enddate, fromassetno, toassetno, alocid);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "Issue Date", "Location ", "SystemAssetid", "Product Serial No", "Model ", "Remarks " };


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string str_startdate = Convert.ToDateTime(startdate).ToString("dd/MM/yyyy");
                string str_enddate = Convert.ToDateTime(enddate).ToString("dd/MM/yyyy");
                string alocname = "";
                if (alocid == 0)
                {
                    alocname = "";
                }
                else
                {
                    alocname = db.ALocations.Where(x => x.Companyid == companyid && x.ID == alocid).FirstOrDefault().ALocationName;
                }
                worksheet.Cells[1, 1].Value = "Assets Tracking Report";

                worksheet.Cells[2, 2].Value = "StartDate:" + str_startdate + "  Enddate:" + str_enddate + "   Fromassetno:" + fromassetno
                                              + "Toassetno:" + toassetno + " location:  " + alocname;
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in FARList)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.str_IssueDate;
                    worksheet.Cells[rowIterator, 5].Value = item.ALocName;
                    worksheet.Cells[rowIterator, 6].Value = item.SystemAssetId;
                    worksheet.Cells[rowIterator, 7].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 8].Value = item.Model;
                    worksheet.Cells[rowIterator, 9].Value = item.Remarks;



                    rowIterator = rowIterator + 1;

                }
                string excelName = "AssetTrackingReport.xlsx";

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
        public ActionResult EmployeeAssetTrackinReport_Index()
        {

            string BetaVer = ConfigurationManager.AppSettings["BetaVersion"];

            if (BetaVer == "true")
            {
                ModelState.AddModelError(string.Empty, "Report download is restricted in the beta version.");
                return RedirectToAction("Index", "Home");
            }

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

            ViewBag.employee = new SelectList(db.Employee.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "EmpId", "FullName");
            ViewBag.fromassetno = new SelectList(db.Assetss.Where(x => x.Companyid == companyid && x.DisposalFlag == 0).OrderBy(e => e.ID), "AssetNo", "AssetNo");
            ViewBag.toassetno = new SelectList(db.Assetss.Where(x => x.Companyid == companyid && x.DisposalFlag == 0).OrderBy(e => e.ID), "AssetNo", "AssetNo");
            return View();
        }

        [HttpPost]
        public ActionResult GetEmployeeAssetsTrackingReport(DateTime? startdate, DateTime? enddate, string fromassetno, string toassetno, string strempid)
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

            List<EmployeeAssetReport> FARList = new List<EmployeeAssetReport>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.AssetTrackinReportRepository reportRepository = new BusinessLogic.AssetTrackinReportRepository();
            FARList = reportRepository.getEmployeeAssettracking(companyid, startdate, enddate, fromassetno, toassetno, strempid);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "Issue Date", "Employee ", "SystemAssetid", "Product SerialNo", "Model ", "Remarks " };




                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string str_startdate = Convert.ToDateTime(startdate).ToString("dd/MM/yyyy");
                string str_enddate = Convert.ToDateTime(enddate).ToString("dd/MM/yyyy");
                string empname = "";
                if (strempid != "")
                {
                    Employee emp = db.Employee.Where(x => x.Companyid == companyid && x.EmpId == strempid).FirstOrDefault();
                    if (emp!= null)
                    {
                        empname = emp.FullName;
                    }
                    
                }
               
                worksheet.Cells[1, 1].Value = "Employee Assets Tracking Report";

                worksheet.Cells[2, 2].Value = "StartDate:" + str_startdate + "  Enddate:" + str_enddate + "   Fromassetno:" + fromassetno
                                              + "Toassetno:" + toassetno + " Employee:  " + empname;
                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;
                foreach (var item in FARList)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.str_IssueDate;
                    worksheet.Cells[rowIterator, 5].Value = item.EmpName;
                    worksheet.Cells[rowIterator, 6].Value = item.SystemAssetId;
                    worksheet.Cells[rowIterator, 7].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 8].Value = item.Model;
                    worksheet.Cells[rowIterator, 9].Value = item.Remarks;



                    rowIterator = rowIterator + 1;

                }
                string excelName = "EmployeeAssetTrackingReport.xlsx";

                string handle = Guid.NewGuid().ToString();
                excel.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();








                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = excelName }
                };

                // Note we are returning a filename as well as the handle


            }
            

        }

    }
}
