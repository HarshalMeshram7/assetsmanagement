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
    public class BatchVerificationController : Controller
    {
        // GET: BatchVerification
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetVerifiedAssets_Index()
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

            var Batchlist = db.Batchs.Where(x => x.Companyid == companyid).ToList();
            ViewBag.BatchName = new SelectList(db.Batchs.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "batchcode");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetVerifiedAssetsData(int BatchId)
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

            //Response.ClearContent();
            //Response.BinaryWrite(generateVerifiedAssetsexcel(companyid, BatchId));
            //string excelName = "Verified Assets Report";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();



            List<Assets> alist = new List<Assets>();
            List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
            FARList = reportRepository.getVerifiedAssets(companyid, BatchId);

            return Content(JsonConvert.SerializeObject(FARList), "application/json");

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetVerifiedAssets(int BatchId)
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

            //Response.ClearContent();
            //Response.BinaryWrite(generateVerifiedAssetsexcel(companyid, BatchId));
            //string excelName = "Verified Assets Report";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();



            List<Assets> alist = new List<Assets>();
            List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
            FARList = reportRepository.getVerifiedAssets(companyid, BatchId);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name",
                    "VerificationStatus", "SrNo", "Model ", "Location ","Sub Location","Sub-SubLocation", "Remarks ","GeoLocation","Datetime"};




                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
                worksheet.Cells[1, 1].Value = "Verified Assets";
                worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    // worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
                    worksheet.Cells[rowIterator, 4].Value = item.VerificationStatus;
                    worksheet.Cells[rowIterator, 5].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 6].Value = item.Model;
                    worksheet.Cells[rowIterator, 7].Value = item.Location;
                    worksheet.Cells[rowIterator, 8].Value = item.SubLocation;
                    worksheet.Cells[rowIterator, 9].Value = item.Sub_SubLocation;
                    worksheet.Cells[rowIterator, 10].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 11].Value = item.GeoLocation;


                    //if (item.Lastupdatetimestamp != null)
                    //{

                    //    string lastdate = Convert.ToString(item.Lastupdatetimestamp );



                    //    worksheet.Cells[rowIterator, 12].Value = lastdate;


                    //}
                    if (item.Lastupdatetimestamp != null)
                    {
                        DateTime? lastUpdate = item.Lastupdatetimestamp;
                        //string lastDate = lastUpdate.Value.ToString("yyyy-MM-dd");
                        string lastDate = lastUpdate.Value.ToString("dd-MM-yyyy");
                        //string lastdate = Convert.ToString(item.Lastupdatetimestamp);



                        //worksheet.Cells[rowIterator, 12].Value = lastdate;
                        worksheet.Cells[rowIterator, 12].Value = lastDate;


                    }
                    //worksheet.Cells[rowIterator, 13].Value = item.nameofperson;




                    rowIterator = rowIterator + 1;

                }

                string excelName = "Verifiedassets.xlsx";

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

        //public byte[] generateVerifiedAssetsexcel(int companyid, int BatchId)
        //{

        //    List<Assets> alist = new List<Assets>();
        //    List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
        //    FARList = reportRepository.getVerifiedAssets(companyid, BatchId);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "System Identification No", "SrNo", "Model ", "Location ", "Remarks " };

                  


        //        // Determine the header range (e.g. A1:D1)
        //       // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
        //        worksheet.Cells[1, 1].Value = "Verified Assets";
        //        worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
        //            worksheet.Cells[rowIterator, 3].Value = item.AssetName;
        //            worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
        //            worksheet.Cells[rowIterator, 5].Value = item.SrNo;
        //            worksheet.Cells[rowIterator, 6].Value = item.Model;
        //            worksheet.Cells[rowIterator, 7].Value = item.Location;
        //            worksheet.Cells[rowIterator, 8].Value = item.Location;
        //            worksheet.Cells[rowIterator, 9].Value = item.Location;
        //            worksheet.Cells[rowIterator, 10].Value = item.Remarks;


        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}

        //-------------Asset Found ------------
        public ActionResult GetAssetsFound_Index()
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

            var Batchlist = db.Batchs.Where(x => x.Companyid == companyid).ToList();
            ViewBag.BatchName = new SelectList(db.Batchs.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "batchcode");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetAssetsFound(int BatchId)
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

            //Response.ClearContent();
            //Response.BinaryWrite(generateAssetFoundsexcel(companyid, BatchId));
            //string excelName = " Assets Found Report";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();



            //return RedirectToAction("GetAssetsFound", "BatchVerification");
            List<Assets> alist = new List<Assets>();
            List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
            FARList = reportRepository.getAssetsfound(companyid, BatchId);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "System Identification No", "SrNo", "Model ", "Location ", "Remarks " };




                // Determine the header range (e.g. A1:D1)
                // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
                worksheet.Cells[1, 1].Value = " Assets Found";
                worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
                    worksheet.Cells[rowIterator, 5].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 6].Value = item.Model;
                    worksheet.Cells[rowIterator, 7].Value = item.Location;
                    worksheet.Cells[rowIterator, 8].Value = item.Location;
                    worksheet.Cells[rowIterator, 9].Value = item.Location;
                    worksheet.Cells[rowIterator, 10].Value = item.Remarks;


                    rowIterator = rowIterator + 1;

                }

                string excelName = "AssetsFoundReport.xlsx";

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

        //public byte[] generateAssetFoundsexcel(int companyid, int BatchId)
        //{

        //    List<Assets> alist = new List<Assets>();
        //    List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
        //    FARList = reportRepository.getAssetsfound(companyid, BatchId);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "System Identification No", "SrNo", "Model ", "Location ", "Remarks " };

                 


        //        // Determine the header range (e.g. A1:D1)
        //       // string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
        //        worksheet.Cells[1, 1].Value = " Assets Found";
        //        worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
        //            worksheet.Cells[rowIterator, 3].Value = item.AssetName;
        //            worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
        //            worksheet.Cells[rowIterator, 5].Value = item.SrNo;
        //            worksheet.Cells[rowIterator, 6].Value = item.Model;
        //            worksheet.Cells[rowIterator, 7].Value = item.Location;
        //            worksheet.Cells[rowIterator, 8].Value = item.Location;
        //            worksheet.Cells[rowIterator, 9].Value = item.Location;
        //            worksheet.Cells[rowIterator, 10].Value = item.Remarks;


        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}
        //-------------------Asset Extra Found--------------------
        public ActionResult GetAssetsExtraFound_Index()
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

            var Batchlist = db.Batchs.Where(x => x.Companyid == companyid).ToList();
            ViewBag.BatchName = new SelectList(db.Batchs.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "batchcode");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetAssetsExtraFoundData(int BatchId)
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
            List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
            FARList = reportRepository.getAssetsextrafound(companyid, BatchId);
            return Content(JsonConvert.SerializeObject(FARList), "application/json");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetAssetsExtraFound(int BatchId)
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

            //Response.ClearContent();
            //Response.BinaryWrite(generateAssetExtraFoundsexcel(companyid, BatchId));
            //string excelName = " Assets Extra Found Report";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();

            //return RedirectToAction("GetAssetsExtraFound", "BatchVerification");
            List<Assets> alist = new List<Assets>();
            List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
            FARList = reportRepository.getAssetsextrafound(companyid, BatchId);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "System Identification No", "SrNo", "Model ", "Location ", "Remarks " };




                // Determine the header range (e.g. A1:D1)
                //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
                worksheet.Cells[1, 1].Value = " Assets extra found";
                worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
                    worksheet.Cells[rowIterator, 5].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 6].Value = item.Model;
                    worksheet.Cells[rowIterator, 7].Value = item.Location;
                    worksheet.Cells[rowIterator, 8].Value = item.Location;
                    worksheet.Cells[rowIterator, 9].Value = item.Location;
                    worksheet.Cells[rowIterator, 10].Value = item.Remarks;


                    rowIterator = rowIterator + 1;

                }
                string excelName = "AssetsExtraFoundReport.xlsx";

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

        //public byte[] generateAssetExtraFoundsexcel(int companyid, int BatchId)
        //{

        //    List<Assets> alist = new List<Assets>();
        //    List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
        //    //  alist=BusinessLogic.ReportRepository.
        //    BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
        //    FARList = reportRepository.getAssetsextrafound(companyid, BatchId);


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "System Identification No", "SrNo", "Model ", "Location ", "Remarks " };

                  


        //        // Determine the header range (e.g. A1:D1)
        //        //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
        //        worksheet.Cells[1, 1].Value = " Assets extra found";
        //        worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
        //            worksheet.Cells[rowIterator, 3].Value = item.AssetName;
        //            worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
        //            worksheet.Cells[rowIterator, 5].Value = item.SrNo;
        //            worksheet.Cells[rowIterator, 6].Value = item.Model;
        //            worksheet.Cells[rowIterator, 7].Value = item.Location;
        //            worksheet.Cells[rowIterator, 8].Value = item.Location;
        //            worksheet.Cells[rowIterator, 9].Value = item.Location;
        //            worksheet.Cells[rowIterator, 10].Value = item.Remarks;


        //            rowIterator = rowIterator + 1;

        //        }

        //        return excel.GetAsByteArray();

        //    }

        //}
        //-----------------------Asset Not Found--------------------------------
        public ActionResult GetAssetsNotFound_Index()
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

            var Batchlist = db.Batchs.Where(x => x.Companyid == companyid).ToList();
            ViewBag.BatchName = new SelectList(db.Batchs.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "batchcode");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetAssetsNotFound(int BatchId)
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

            //Response.ClearContent();
            //Response.BinaryWrite(generateAssetNotFoundsexcel(companyid, BatchId));
            //string excelName = " Assets Not Found Report";
            //Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Flush();
            //Response.End();



            //return RedirectToAction("GetAssetsNotFound", "BatchVerification");
            List<Assets> alist = new List<Assets>();
            List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
            FARList = reportRepository.getAssetsnotfound(companyid, BatchId);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "System Identification No", "SrNo", "Model ", "Location ", "Remarks " };




                // Determine the header range (e.g. A1:D1)
                //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
                worksheet.Cells[1, 1].Value = " Assets not found";
                worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
                    worksheet.Cells[rowIterator, 5].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 6].Value = item.Model;
                    worksheet.Cells[rowIterator, 7].Value = item.Location;
                    worksheet.Cells[rowIterator, 8].Value = item.Location;
                    worksheet.Cells[rowIterator, 9].Value = item.Location;
                    worksheet.Cells[rowIterator, 10].Value = item.Remarks;


                    rowIterator = rowIterator + 1;

                }

                string excelName = "AssetsNotFounReport.xlsx";

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

        public byte[] generateAssetNotFoundsexcel(int companyid, int BatchId)
        {

            List<Assets> alist = new List<Assets>();
            List<BatchVerificationViewModel> FARList = new List<BatchVerificationViewModel>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.BatchVerificationRepository reportRepository = new BusinessLogic.BatchVerificationRepository();
            FARList = reportRepository.getAssetsnotfound(companyid, BatchId);


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = { "AssetNo", "AssetIdentification No", "Asset Name ", "System Identification No", "SrNo", "Model ", "Location ", "Remarks " };

                  


                // Determine the header range (e.g. A1:D1)
                //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                string batchcode = db.Batchs.Where(x => x.Companyid == companyid && x.ID == BatchId).FirstOrDefault().batchcode;
                worksheet.Cells[1, 1].Value = " Assets not found";
                worksheet.Cells[2, 2].Value = "Batch:  " + batchcode;
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
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationno;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.systemassetid;
                    worksheet.Cells[rowIterator, 5].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 6].Value = item.Model;
                    worksheet.Cells[rowIterator, 7].Value = item.Location;
                    worksheet.Cells[rowIterator, 8].Value = item.Location;
                    worksheet.Cells[rowIterator, 9].Value = item.Location;
                    worksheet.Cells[rowIterator, 10].Value = item.Remarks;


                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }

        }
        //---------------------------------------------------

    }
}