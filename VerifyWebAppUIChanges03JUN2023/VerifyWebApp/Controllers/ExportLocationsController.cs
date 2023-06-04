using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class ExportLocationsController : Controller
    {
        // GET: ExportLocations
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Alocation_Index()
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
            List<ALocation> alist = new List<ALocation>();
            alist = db.ALocations.Where(x => x.Companyid == companyid).ToList();


            return View(alist);
        }

        [HttpGet]
        public ActionResult exportalocation()
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }


            Response.ClearContent();
            Response.BinaryWrite(generatealocationexcel(companyid));
            string excelName = "ALocation";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Alocation_Index", "ExportLocations");

        }
        public byte[] generatealocationexcel(int companyid)
        {


            List<ALocation> alst = new List<ALocation>();



            alst = db.ALocations.Where(x => x.Companyid == companyid).ToList();


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "AlocationNo", "AlocationName"}
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                int rowIterator = 2;
                foreach (var item in alst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.ALocationName;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        public ActionResult downloadalocation()
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
            Response.ClearContent();
            Response.BinaryWrite(generateImportalocationexcel());
            string excelName = "Alocation";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("AlocationIndex", "ExportLocations");
        }
        public byte[] generateImportalocationexcel()
        {
            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "AlocationNo", "Alocation Name"}
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);


                return excel.GetAsByteArray();
            }
        }
        public ActionResult importalocation()
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
            return PartialView();
        }

        [HttpPost]
        [ValidateAjax]
        public ActionResult UploadALocation()
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            ALocation alocation = new ALocation();

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


                        using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;


                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                string a; a = "";
                                string b; b = "";
                                string c; c = "";
                                string d; d = "";
                                string e; e = "";
                                string f; f = "";


                                bool Alocationnoflag;
                                bool Alocationnameflag;

                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    Alocationnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    Alocationnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    Alocationnameflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    Alocationnameflag = true;
                                }







                                if (Alocationnoflag == true && Alocationnameflag == true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;


                                    // agroup.Srno = Convert.ToInt32(a);
                                    alocation.ALocationName = b;
                                    alocation.CreatedDate = istDate;
                                    alocation.CreatedUserId = userid;
                                    alocation.Companyid = companyid;

                                    db.ALocations.Add(alocation);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    errorlist.Add("Something missing in row  " + rowIterator);

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
                res.Data = "error";
                return res;
            }
        }

        /////////
        public ActionResult Blocation_Index()
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
            List<BLocation> blist = new List<BLocation>();
            blist = db.BLocations.Where(x => x.Companyid == companyid).ToList();


            return View(blist);
        }
        [HttpGet]
        public ActionResult exportblocation()
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }


            Response.ClearContent();
            Response.BinaryWrite(generateblocationexcel(companyid));
            string excelName = "blocation";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Blocation_Index", "Export_Groups");

        }
        public byte[] generateblocationexcel(int companyid)
        {


            List<BLocation> blst = new List<BLocation>();



            blst = db.BLocations.Where(x => x.Companyid == companyid).ToList();


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "AlocationNo", "BlocationNo", "BlocationName" }
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                int rowIterator = 2;
                foreach (var item in blst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ALocID;
                    worksheet.Cells[rowIterator, 2].Value = item.ID;
                    worksheet.Cells[rowIterator, 3].Value = item.BLocationName;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        public ActionResult downloadblocation()
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
            Response.ClearContent();
            Response.BinaryWrite(generateImportblocationexcel());
            string excelName = "Blocation";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Blocation_Index", "ExportLocations");
        }
        public byte[] generateImportblocationexcel()
        {
            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "AlocationNo","BlocationNo", "Blocation Name"}
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);


                return excel.GetAsByteArray();
            }
        }
        public ActionResult importblocation()
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
            return PartialView();
        }

        [HttpPost]
        [ValidateAjax]
        public ActionResult UploadBLocation()
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            BLocation blocation = new BLocation();

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


                        using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;


                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                string a; a = "";
                                string b; b = "";
                                string c; c = "";


                                bool Alocationnoflag;
                                bool Blocationnoflag;
                                bool Blocationnameflag;

                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    Alocationnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    Alocationnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    Blocationnoflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    Blocationnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    Blocationnameflag = false;
                                }
                                else
                                {
                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    Blocationnameflag = true;
                                }






                                if (Alocationnoflag == true && Blocationnameflag == true && Blocationnoflag == true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;


                                    // agroup.Srno = Convert.ToInt32(a);
                                    blocation.ALocID = Convert.ToInt32(a);
                                    blocation.BLocationName = c;
                                    blocation.CreatedDate = istDate;
                                    blocation.CreatedUserId = userid;
                                    blocation.Companyid = companyid;

                                    db.BLocations.Add(blocation);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    errorlist.Add("Something missing in row  " + rowIterator);

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
                res.Data = "error";
                return res;
            }
        }

        /////////
        public ActionResult Clocation_Index()
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
            List<CLocation> clist = new List<CLocation>();
            clist = db.CLocations.Where(x => x.Companyid == companyid).ToList();


            return View(clist);
        }
        [HttpGet]
        public ActionResult exportclocation()
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }


            Response.ClearContent();
            Response.BinaryWrite(generateclocationexcel(companyid));
            string excelName = "clocation";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Clocation_Index", "ExportLocations");

        }
        public byte[] generateclocationexcel(int companyid)
        {


            List<CLocation> clst = new List<CLocation>();



            clst = db.CLocations.Where(x => x.Companyid == companyid).ToList();


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "AlocationNo", "BlocationNo", "ClocationNo", "ClocationName" }
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                int rowIterator = 2;
                foreach (var item in clst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ALocID;
                    worksheet.Cells[rowIterator, 2].Value = item.BLocID;
                    worksheet.Cells[rowIterator, 3].Value = item.ID;
                    worksheet.Cells[rowIterator, 4].Value = item.CLocationName;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        public ActionResult downloadclocation()
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
            Response.ClearContent();
            Response.BinaryWrite(generateImportclocationexcel());
            string excelName = "Clocation";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Cgroup_Index", "Export_Groups");
        }
        public byte[] generateImportclocationexcel()
        {
            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                var headerRow = new List<string[]>()
                  {
                    new string[] { "AlocationNo","BlocationNo", "ClocationNo", "Clocation Name" }
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
                worksheet.Cells[headerRange].LoadFromArrays(headerRow);


                return excel.GetAsByteArray();
            }
        }
        public ActionResult importclocation()
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
            return PartialView();
        }

        [HttpPost]
        [ValidateAjax]
        public ActionResult UploadCLocation()
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            CLocation clocation = new CLocation();

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


                        using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;


                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                string a; a = "";
                                string b; b = "";
                                string c; c = "";
                                string d; d = "";

                                bool Alocationnoflag;
                                bool Blocationnoflag;
                                bool Clocationnoflag;
                                bool Clocationnameflag;

                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    Alocationnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    Alocationnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    Blocationnoflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    Blocationnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    Clocationnoflag = false;
                                }
                                else
                                {
                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    Clocationnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    Clocationnameflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    Clocationnameflag = true;
                                }






                                if (Alocationnoflag == true && Clocationnameflag == true && Blocationnoflag == true && Clocationnoflag == true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;


                                    // agroup.Srno = Convert.ToInt32(a);
                                    clocation.ALocID = Convert.ToInt32(a);
                                    clocation.BLocID = Convert.ToInt32(b);
                                    clocation.CLocationName = d;
                                    clocation.CreatedDate = istDate;
                                    clocation.CreatedUserId = userid;
                                    clocation.Companyid = companyid;

                                    db.CLocations.Add(clocation);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    errorlist.Add("Something missing in row  " + rowIterator);

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
                res.Data = "error";
                return res;
            }
        }

    }
}