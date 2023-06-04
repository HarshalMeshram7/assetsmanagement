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
    public class Export_GroupsController : Controller
    {
        // GET: Export_Groups
        public VerifyDB db = new VerifyDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Agroup_Index()
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
            List<AGroup> alist = new List<AGroup>();
            alist = db.AGroups.Where(x => x.Companyid == companyid).ToList();


            return View(alist);
        }
        [HttpGet]

        public ActionResult exportagroup()
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
            Response.BinaryWrite(generateagroupexcel(companyid));
            string excelName = "Amc";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Agroup_Index", "Export_Groups");

        }
        public byte[] generateagroupexcel(int companyid)
        {


            List<AGroup> alst = new List<AGroup>();



            alst = db.AGroups.Where(x => x.Companyid == companyid).ToList();


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
                    new string[] { "AgroupNo", "AgroupName"}
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
                    worksheet.Cells[rowIterator, 2].Value = item.AGroupName;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        public ActionResult downloadagroup()
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
            Response.BinaryWrite(generateImportagroupexcel());
            string excelName = "Agroup";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            
            return RedirectToAction("Agroup_Index","Export_Groups");
        }
        public byte[] generateImportagroupexcel()
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
                    new string[] { "AgroupNo", "Agroup Name"}
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
        public ActionResult importagroup()
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
        public ActionResult UploadAGroup()
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


            AGroup agroup = new AGroup();

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


                                bool Agroupnoflag;
                                bool Agroupnameflag;
                               
                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    Agroupnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    Agroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    Agroupnameflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    Agroupnameflag = true;
                                }



                              



                                if (Agroupnoflag == true && Agroupnameflag == true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;

                                   
                                   // agroup.Srno = Convert.ToInt32(a);
                                    agroup.AGroupName = b;
                                    agroup.CreatedDate = istDate;
                                    agroup.CreatedUserId = userid;
                                    agroup.Companyid = companyid;
                                    
                                    db.AGroups.Add(agroup);
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
        public ActionResult Bgroup_Index()
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
            List<BGroup> blist = new List<BGroup>();
            blist = db.BGroups.Where(x => x.Companyid == companyid).ToList();


            return View(blist);
        }
        [HttpGet]
        public ActionResult exportbgroup()
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
            Response.BinaryWrite(generatebgroupexcel(companyid));
            string excelName = "bgroup";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Bgroup_Index", "Export_Groups");

        }
        public byte[] generatebgroupexcel(int companyid)
        {


            List<BGroup> blst = new List<BGroup>();



            blst = db.BGroups.Where(x => x.Companyid == companyid).ToList();


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
                    new string[] { "AgroupNo", "BgroupNo", "BgroupName" }
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
                    worksheet.Cells[rowIterator, 1].Value = item.AGrpID;
                    worksheet.Cells[rowIterator, 2].Value = item.ID;
                    worksheet.Cells[rowIterator, 3].Value = item.BGroupName;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        public ActionResult downloadbgroup()
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
            Response.BinaryWrite(generateImportbgroupexcel());
            string excelName = "Bgroup";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Bgroup_Index", "Export_Groups");
        }
        public byte[] generateImportbgroupexcel()
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
                    new string[] { "AgroupNo","BgroupNo", "Bgroup Name"}
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
        public ActionResult importbgroup()
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
        public ActionResult UploadBGroup()
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


            BGroup bgroup = new BGroup();

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
                               

                                bool Agroupnoflag;
                                bool Bgroupnoflag;
                                bool Bgroupnameflag;

                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    Agroupnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    Agroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    Bgroupnoflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    Bgroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    Bgroupnameflag = false;
                                }
                                else
                                {
                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    Bgroupnameflag = true;
                                }






                                if (Agroupnoflag == true && Bgroupnameflag == true && Bgroupnoflag==true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;

                                   
                                    // agroup.Srno = Convert.ToInt32(a);
                                    bgroup.AGrpID = Convert.ToInt32(a);
                                    bgroup.BGroupName = c;
                                    bgroup.CreatedDate = istDate;
                                    bgroup.CreatedUserId = userid;
                                    bgroup.Companyid = companyid;

                                    db.BGroups.Add(bgroup);
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
        public ActionResult Cgroup_Index()
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
            List<CGroup> clist = new List<CGroup>();
            clist = db.CGroups.Where(x => x.Companyid == companyid).ToList();


            return View(clist);
        }
        [HttpGet]
        public ActionResult exportcgroup()
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
            Response.BinaryWrite(generatecgroupexcel(companyid));
            string excelName = "cgroup";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Cgroup_Index", "Export_Groups");

        }
        public byte[] generatecgroupexcel(int companyid)
        {


            List<CGroup> clst = new List<CGroup>();



            clst = db.CGroups.Where(x => x.Companyid == companyid).ToList();


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
                    new string[] { "AgroupNo", "BgroupNo", "CgroupNo", "CgroupName" }
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
                    worksheet.Cells[rowIterator, 1].Value = item.AGrpID;
                    worksheet.Cells[rowIterator, 2].Value = item.BGrpID;
                    worksheet.Cells[rowIterator, 3].Value = item.ID;
                    worksheet.Cells[rowIterator, 4].Value = item.CGroupName;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        public ActionResult downloadcgroup()
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
            Response.BinaryWrite(generateImportcgroupexcel());
            string excelName = "Cgroup";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Cgroup_Index", "Export_Groups");
        }
        public byte[] generateImportcgroupexcel()
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
                    new string[] { "AgroupNo","BgroupNo", "CgroupNo", "Cgroup Name" }
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
        public ActionResult importcgroup()
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
        public ActionResult UploadCGroup()
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


            CGroup cgroup = new CGroup();

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
                                string d;d = "";

                                bool Agroupnoflag;
                                bool Bgroupnoflag;
                                bool Cgroupnoflag;
                                bool Cgroupnameflag;

                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    Agroupnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    Agroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    Bgroupnoflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    Bgroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    Cgroupnoflag = false;
                                }
                                else
                                {
                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    Cgroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    Cgroupnameflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    Cgroupnameflag = true;
                                }






                                if (Agroupnoflag == true && Cgroupnameflag == true && Bgroupnoflag==true && Cgroupnoflag==true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;


                                    // agroup.Srno = Convert.ToInt32(a);
                                    cgroup.AGrpID = Convert.ToInt32(a);
                                    cgroup.BGrpID = Convert.ToInt32(b);
                                    cgroup.CGroupName = d;
                                    cgroup.CreatedDate = istDate;
                                    cgroup.CreatedUserId = userid;
                                    cgroup.Companyid = companyid;

                                    db.CGroups.Add(cgroup);
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

        /////////dgroup
        public ActionResult Dgroup_Index()
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
            List<DGroup> dlist = new List<DGroup>();
            dlist = db.DGroups.Where(x => x.Companyid == companyid).ToList();


            return View(dlist);
        }
        [HttpGet]
        public ActionResult exportdgroup()
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
            Response.BinaryWrite(generatedgroupexcel(companyid));
            string excelName = "dgroup";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Dgroup_Index", "Export_Groups");

        }
        public byte[] generatedgroupexcel(int companyid)
        {


            List<DGroup> dlst = new List<DGroup>();



            dlst = db.DGroups.Where(x => x.Companyid == companyid).ToList();


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
                    new string[] { "AgroupNo", "BgroupNo", "CgroupNo", "DgroupNo", "DgroupName" }
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
                foreach (var item in dlst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.AGrpID;
                    worksheet.Cells[rowIterator, 2].Value = item.BGrpID;
                    worksheet.Cells[rowIterator, 3].Value = item.CGrpID;
                    worksheet.Cells[rowIterator, 4].Value = item.ID;
                    worksheet.Cells[rowIterator, 5].Value = item.DGroupName;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        public ActionResult downloaddgroup()
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
            Response.BinaryWrite(generateImportdgroupexcel());
            string excelName = "dgroup";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Dgroup_Index", "Export_Groups");
        }
        public byte[] generateImportdgroupexcel()
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
                    new string[] { "AgroupNo","BgroupNo", "CgroupNo", "DgroupNo", "Dgroup Name" }
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
        public ActionResult importdgroup()
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
        public ActionResult UploadDGroup()
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


            DGroup dgroup = new DGroup();

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
                                bool Agroupnoflag;
                                bool Bgroupnoflag;
                                bool Cgroupnoflag;
                                bool Dgroupnoflag;
                                bool Dgroupnameflag;

                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    Agroupnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    Agroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    Bgroupnoflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    Bgroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    Cgroupnoflag = false;
                                }
                                else
                                {
                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    Cgroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    Dgroupnoflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    Dgroupnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    e = "";
                                    Dgroupnameflag = false;
                                }
                                else
                                {
                                    e = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    Dgroupnameflag = true;
                                }






                                if (Agroupnoflag == true && Dgroupnameflag == true && Bgroupnoflag == true && Cgroupnoflag == true && Dgroupnoflag == true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;


                                    // agroup.Srno = Convert.ToInt32(a);
                                    dgroup.AGrpID = Convert.ToInt32(a);
                                    dgroup.BGrpID = Convert.ToInt32(b);
                                    dgroup.CGrpID = Convert.ToInt32(c);
                                    dgroup.DGroupName = e;
                                    dgroup.CreatedDate = istDate;
                                    dgroup.CreatedUserId = userid;
                                    dgroup.Companyid = companyid;

                                    db.DGroups.Add(dgroup);
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
