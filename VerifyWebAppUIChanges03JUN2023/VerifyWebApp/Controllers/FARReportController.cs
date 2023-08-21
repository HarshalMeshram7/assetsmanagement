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
    public class FARReportController : Controller
    {
        // GET: FARReport
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetFAReport_Index()
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

        //[HttpGet]
        //public ActionResult GetFAReport(string asondate)
        //{
        //    DateTime Asondate = Convert.ToDateTime(asondate);
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
        //    Response.BinaryWrite(generateFARexcel(companyid, Asondate));
        //    string excelName = "FAR Report";
        //    Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Flush();
        //    Response.End();



        //    return RedirectToAction("GetFAReport", "FARReport");

        //}
        /*

  [HttpPost]
  [AllowAnonymous]
  [ValidateJsonAntiForgeryToken]
  public ActionResult GetFAReport(DateTime asondate)
  {

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
      //  alist=BusinessLogic.ReportRepository.
      BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();
      FARList = reportRepository.getAllassetForFAR(companyid, asondate);


      var memoryStream = new MemoryStream();
      byte[] data;
      //ExcelPackage.LicenseContext = LicenseContext.Commercial;
      ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
      using (ExcelPackage excel = new ExcelPackage(memoryStream))
      {

          excel.Workbook.Worksheets.Add("Worksheet1");
          excel.Workbook.Worksheets.Add("Worksheet2");

          //var headerRow = new List<string[]>()
          //  {
          //    new string[] {"AGroupName", "BGroupName", "CGroupName", "DGroupName", "AssetNo", "AssetIdentificationNo", "AssetName","VoucherNo", "VoucherDate", "DTPutUseCompany", "Qty", "SupplierName",
          //    "DepreciationRate","DepreciationMethod","AmountCapitalisedCompany","AmountCapitalisedIT",
          //    "DepreciationAmount","NetBalance","TotalCedit","InvoiceAmount","TransactionType"}
          //  };
          string[] headerRow = {"AGroupName", "BGroupName", "CGroupName", "DGroupName", "AssetNo", "AssetIdentificationNo", "AssetName","VoucherNo", "VoucherDate", "DTPutUseCompany", "Qty", "SupplierName",
              "DepreciationRate","DepreciationMethod","AmountCapitalisedCompany","AmountCapitalisedIT",
              "DepreciationAmount","NetBalance","TotalCedit","InvoiceAmount","TransactionType"};


          string headerRange = "A3:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


          // Target a worksheet
          var worksheet = excel.Workbook.Worksheets["Worksheet1"];
          var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
          var currentSheet = excel.Workbook.Worksheets;
          // Popular header row data
          worksheet.Cells[1,1].Value = "FAR Report";
          worksheet.Cells[2,2].Value = "AsonDate:  "+asondate.ToString("dd/MM/yyyy");
          int col = 1;
          for (int i = 0; i <= headerRow.Count()-1; i++)
          {

              worksheet.Cells[4,col].Value = headerRow[i];
              col++;

          }
         // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
          int rowIterator = 5;
          foreach (var item in FARList)
          {
              worksheet.Cells[rowIterator, 1].Value = item.AGroupName;
              worksheet.Cells[rowIterator, 2].Value = item.BGroupName;
              worksheet.Cells[rowIterator, 3].Value = item.CGroupName;
              worksheet.Cells[rowIterator, 4].Value = item.DGroupName;
              worksheet.Cells[rowIterator, 5].Value = item.AssetNo;
              worksheet.Cells[rowIterator, 6].Value = item.AssetIdentificationNo;
              worksheet.Cells[rowIterator, 7].Value = item.AssetName;
              worksheet.Cells[rowIterator, 8].Value = item.VoucherNo;
              worksheet.Cells[rowIterator, 9].Value = item.str_VoucherDate;
              worksheet.Cells[rowIterator, 10].Value = item.str_DTPutUseCompany;
              worksheet.Cells[rowIterator, 11].Value = item.Qty;
              worksheet.Cells[rowIterator, 12].Value = item.SupplierName;
              worksheet.Cells[rowIterator, 13].Value = item.DepRate;
              worksheet.Cells[rowIterator, 14].Value = item.DepMethod;
              worksheet.Cells[rowIterator, 15].Value = item.AmountCapitalisedCompany;
              worksheet.Cells[rowIterator, 16].Value = item.AmountCapitalisedIT;
              worksheet.Cells[rowIterator, 17].Value = item.DepreciationAmount;
              worksheet.Cells[rowIterator, 18].Value = item.NetBalance;
              worksheet.Cells[rowIterator, 19].Value = item.TotalCedit;
              worksheet.Cells[rowIterator, 20].Value = item.InvoiceAmount;
              worksheet.Cells[rowIterator, 21].Value = item.TransactionType;
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
  */

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetFAReport_New(DateTime FromDate, DateTime ToDate)
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


                var memoryStream = new MemoryStream();
                byte[] data;
                //ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excel = new ExcelPackage(memoryStream))
                {

                    excel.Workbook.Worksheets.Add("Worksheet1");
                    excel.Workbook.Worksheets.Add("Worksheet2");


                    string[] headerRow = {"AGroupName", "BGroupName", "CGroupName ", "DGroupName","AssetNo", "AssetIdentificationNo ", "AssetName ", "VoucherDate ",
                    "OpGross","Addition ","Disposal","ClGross","OpDep","DepForPeriod","DispoDep","TotDep","NetBalance",
                    "ResidualVal",
                    "VoucherNo","VoucherDate", "BillNo","PONo","BillDate","DtPutUse(Company)","DepRate","DepMethod",
                    "Opening Qty","DisposedQtyTillFromDate", "Disposed Qty", "Closing Qty", "Product Serial No","Model","Remarks",
                    "ALocName ","BLocName","CLocName","SupplierName",};




                    // Determine the header range (e.g. A1:D1)
                    //   string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                    // Target a worksheet




                    var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                    var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                    var currentSheet = excel.Workbook.Worksheets;
                    // Popular header row data
                    worksheet.Cells[1, 1].Value = "Fixed Asset Register Report";
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



                        worksheet.Cells[rowIterator, 18].Value = item.ResidualVal;


                        worksheet.Cells[rowIterator, 19].Value = item.VoucherNo;



                        if (item.voucherDate.HasValue)
                        {
                            DateTime dtVouucherDate = item.voucherDate.Value.Date;

                            worksheet.Cells[rowIterator, 20].Value = dtVouucherDate.ToString("dd/MM/yyyy");
                            // worksheet.Cells[rowIterator, 19].Style.Numberformat.Format = "dd/mm/yyyy";

                        }
                        else
                        {
                            worksheet.Cells[rowIterator, 20].Value = "";
                        }


                        worksheet.Cells[rowIterator, 21].Value = item.BillNo;
                        worksheet.Cells[rowIterator, 22].Value = item.PONo;


                        if (item.BillDate.HasValue)
                        {
                            DateTime dtBillDate = item.BillDate.Value.Date;
                            worksheet.Cells[rowIterator, 23].Value = dtBillDate.Date.ToString("dd/MM/yyyy");

                        }
                        else
                        {
                            worksheet.Cells[rowIterator, 23].Value = "";
                        }





                        if (item.DTPutUse.HasValue)
                        {
                            DateTime dtDTPutUse = item.DTPutUse.Value.Date;
                            worksheet.Cells[rowIterator, 24].Value = dtDTPutUse.ToString("dd/MM/yyyy");
                            // worksheet.Cells[rowIterator, 23].Style.Numberformat.Format = "dd/mm/yyyy";
                        }
                        else
                        {
                            worksheet.Cells[rowIterator, 24].Value = "";
                        }

                        worksheet.Cells[rowIterator, 25].Value = item.DepRate;
                        worksheet.Cells[rowIterator, 26].Value = item.DepMethod;

                        worksheet.Cells[rowIterator, 27].Value = item.OpeningQty;
                        worksheet.Cells[rowIterator, 28].Value = item.DisposedQtyTillFromDate;
                        worksheet.Cells[rowIterator, 29].Value = item.DisposedQty;
                        worksheet.Cells[rowIterator, 30].Value = item.ClosingQty;
                        worksheet.Cells[rowIterator, 31].Value = item.SrNo;


                        //worksheet.Cells[rowIterator, 32].Value = item.SrNo;

                        worksheet.Cells[rowIterator, 32].Value = item.Model;

                        worksheet.Cells[rowIterator, 33].Value = item.Remarks;
                        worksheet.Cells[rowIterator, 34].Value = item.ALocName;
                        worksheet.Cells[rowIterator, 35].Value = item.BLocName;
                        worksheet.Cells[rowIterator, 36].Value = item.CLocName;
                        worksheet.Cells[rowIterator, 37].Value = item.SupplierName;




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
        public ActionResult FARReport_New() 
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

            ViewBag.Fromdate = new SelectList(fromdatelist.OrderByDescending(e => e.FromDate), "str_fromdate", "str_fromdate");

            ViewBag.Todate = new SelectList(todatelist.OrderByDescending(e => e.ToDate), "str_todate", "str_todate");


            return View("FARReport");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult FARReportV1()
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

            ViewBag.Fromdate = new SelectList(fromdatelist.OrderByDescending(e => e.FromDate), "str_fromdate", "str_fromdate");

            ViewBag.Todate = new SelectList(todatelist.OrderByDescending(e => e.ToDate), "str_todate", "str_todate");

            // view for show report in data grid
            return View("FARReportV1");
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

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetFARReport_NewGrid(DateTime FromDate, DateTime ToDate)
        {
            JsonResult result = new JsonResult();
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

                //string draw = Request.Form.GetValues("draw")[0];



                int totalRecords = 0;

                BusinessLogic.FARReportRepository reportRepository = new BusinessLogic.FARReportRepository();

                FARList = reportRepository.getFAR_New(companyid, FromDate, ToDate);



                // string json = JsonConvert.SerializeObject(videogames);

               // JObject o = JObject.FromObject(new
               // {
               //     draw = Convert.ToInt32(draw),
               //     recordsTotal = totalRecords,
               //     recordsFiltered = totalRecords,
               //     data = FARList,
               // }
               //);

                return Content(JsonConvert.SerializeObject(FARList), "application/json");

               // return o.ToString(Formatting.None);
                /*
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = FARList,
                }, JsonRequestBehavior.AllowGet);
                */

            }
            catch (Exception ex)
            {

            }

            return result;




        }

    }

}