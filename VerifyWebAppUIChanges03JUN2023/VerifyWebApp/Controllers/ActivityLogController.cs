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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace VerifyWebApp.Controllers
{
    public class ActivityLogController : Controller
    {
        // GET: FARReport
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ActivityLog_Index(DateTime FromDate, DateTime ToDate)
        {

            try
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
                List<ActivityLogViewmodel> ALogList = new List<ActivityLogViewmodel>();


                // BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();

                //  FARList = reportRepository.getFAR_New(companyid, FromDate, ToDate);

                string strSQL = null;
                string str_comid = companyid.ToString();

                string fromDate = FromDate.ToString("yyyy-MM-dd");
                string todate = ToDate.ToString("yyyy-MM-dd");

                
                strSQL = " SELECT `id`, `userid`, `eventid`, `recordtype`, `column`, `oldvalue`, `newvalue` FROM tblauditlog WHERE ";
                strSQL = strSQL + " trandate >= " + "'" + fromDate + "'" + " AND trandate <= " + "'" + todate + "'";

                db.Database.CommandTimeout = 180;

                ALogList = db.Database.SqlQuery<ActivityLogViewmodel>(strSQL).ToList();




                var memoryStream = new MemoryStream();
                byte[] data;
                //ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excel = new ExcelPackage(memoryStream))
                {

                    excel.Workbook.Worksheets.Add("Worksheet1");
                    //excel.Workbook.Worksheets.Add("Worksheet2");


                    string[] headerRow = {"UserId", "EventId", "RecordType", "TranDate", "Column", "Oldvalue", "Newvalue",};




                    // Determine the header range (e.g. A1:D1)
                    //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                    // Target a worksheet




                    var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                 //   var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                    var currentSheet = excel.Workbook.Worksheets;
                    // Popular header row data
                    worksheet.Cells[1, 1].Value = "ActivityLog Report";
                    worksheet.Cells[2, 2].Value = "FromDate:  " + FromDate.ToString("dd/MM/yyyy") + " ToDate:" + ToDate.ToString("dd/MM/yyyy");

                    int col = 1;
                    for (int i = 0; i <= headerRow.Count() - 1; i++)
                    {

                        worksheet.Cells[4, col].Value = headerRow[i];
                        col++;

                    }
                    int rowIterator = 5;




                    foreach (var item in ALogList)
                    {
                        worksheet.Cells[rowIterator, 1].Value = item.UserId;
                        worksheet.Cells[rowIterator, 2].Value = item.RecordType;
                        worksheet.Cells[rowIterator, 3].Value = item.EventId;
                        worksheet.Cells[rowIterator, 4].Value = item.TranDate;

                        worksheet.Cells[rowIterator, 5].Value = item.Column;
                        worksheet.Cells[rowIterator, 6].Value = item.Oldvalue;
                        worksheet.Cells[rowIterator, 7].Value = item.Newvalue;
                      

                        rowIterator = rowIterator + 1;

                    }


                    string excelName = "ActivityLogReport.xlsx";

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
            catch (Exception ex)
            {
                // log error to database 
                int i = 0;

                //return View("Error");
                return View("~/Views/Shared/Error.cshtm");


            }

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetPdfReport(DateTime FromDate, DateTime ToDate)
        {

            try
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


                BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();

                FARList = reportRepository.getFAR_New(companyid, FromDate, ToDate);


                MemoryStream memoryStream = new MemoryStream();
                Document document = new Document(new Rectangle(PageSize.A4.Width * 6, PageSize.A4.Height));
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                document.Add(new Paragraph("Fixed Asset Register Report"));
                document.Add(new Paragraph("FromDate: " + FromDate.ToString("dd/MM/yyyy") + " ToDate: " + ToDate.ToString("dd/MM/yyyy")));
                document.Add(new Paragraph(" "));

                string[] headerRow = { "AGroupName", "BGroupName", "CGroupName ", "DGroupName","AssetNo", "AssetIdentificationNo ", "AssetName ", "VoucherDate ",
                    "OpGross","Addition ","Disposal","ClGross","OpDep","DepForPeriod","DispoDep","TotDep","NetBalance",
                    "ResidualVal",
                    "VoucherNo","VoucherDate", "BillNo","PONo","BillDate","DtPutUse(Company)","DepRate","DepMethod",
                    "Opening Qty","DisposedQtyTillFromDate", "Disposed Qty", "Closing Qty", "Product Serial No","Model","Remarks",
                    "ALocName ","BLocName","CLocName","SupplierName",};

                float[] columnWidths = { 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f, 28f };

                PdfPTable table = new PdfPTable(columnWidths);
                table.WidthPercentage = 100;

                foreach (string header in headerRow)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header));
                    table.AddCell(cell);
                }

                foreach (var item in FARList)
                {
                    table.AddCell(item.AGroupName);
                    table.AddCell(item.BGroupName);
                    table.AddCell(item.CGroupName);
                    table.AddCell(item.DGroupName);
                    table.AddCell(item.AssetNo);
                    table.AddCell(item.AssetIdentificationNo);
                    table.AddCell(item.AssetName);
                    table.AddCell(item.str_voucherdate);
                    table.AddCell(item.OpGross.ToString());
                    table.AddCell(item.Addition.ToString());
                    table.AddCell(item.Disposal.ToString());
                    table.AddCell(item.ClGross.ToString());
                    table.AddCell(item.OpDep.ToString());
                    table.AddCell(item.UpToDep.ToString());
                    table.AddCell(item.DispoDep.ToString());
                    table.AddCell(item.TotDep.ToString());
                    table.AddCell(item.NetBalance.ToString());
                    table.AddCell(item.ResidualVal.ToString());
                    table.AddCell(item.VoucherNo);
                    table.AddCell(item.voucherDate.ToString());
                    table.AddCell(item.BillNo);
                    table.AddCell(item.PONo);
                    table.AddCell(item.BillDate.ToString());
                    table.AddCell(item.DTPutUse.ToString());
                    table.AddCell(item.DepRate.ToString());
                    table.AddCell(item.DepMethod);
                    table.AddCell(item.OpeningQty.ToString());
                    table.AddCell(item.OpeningQty.ToString());
                    table.AddCell(item.DisposedQty.ToString());
                    table.AddCell(item.ClosingQty.ToString());
                    table.AddCell(item.SrNo);
                    table.AddCell(item.Model);
                    table.AddCell(item.Remarks);
                    table.AddCell(item.ALocName);
                    table.AddCell(item.BLocName);
                    table.AddCell(item.CLocName);
                    table.AddCell(item.SupplierName);

                    table.CompleteRow();
                }

                document.Add(table);
                document.Close();

                string pdfName = "FARReport.pdf";

                string handle = Guid.NewGuid().ToString();
                TempData[handle] = memoryStream.ToArray();

                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = pdfName }
                };
            }
            catch (Exception ex)
            {
                int i = 0;

                return View("~/Views/Shared/Error.cshtm");

            }

        }


        [HttpGet]
        [AllowAnonymous]
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

  
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
                // how can i return pdf and excel here
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
    }

}