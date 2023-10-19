using iTextSharp.text;
using iTextSharp.text.pdf;
using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class EmployeeAssetController : Controller
    {
        // GET: EmployeeAsset
        public VerifyDB db = new VerifyDB();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [AuthUser]
        public ActionResult Index()
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

            List<EmployeeAsset> emplist = new List<EmployeeAsset>();
            List<Assets> lstAssets = db.Assetss.Where(x => x.Companyid == companyid).ToList();
            List<Employee> lstEmployee = db.Employee.Where(x => x.Companyid == companyid).ToList();

            emplist = db.EmployeeAsset.Where(x => x.Companyid == companyid).ToList();
            foreach (EmployeeAsset item in emplist)
            {
                item.str_IssueDate = item.IssueDate.ToString("dd/MM/yyyy");
                if (item.RecievedDate != null)
                {
                    item.str_RecievedDate = Convert.ToDateTime(item.RecievedDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_RecievedDate = "";
                }

                Employee obj_emp = lstEmployee.Where(x => x.ID == item.EmpId).FirstOrDefault();
                if (obj_emp != null)
                {
                    item.str_empname = obj_emp.FullName;
                    item.empno = obj_emp.EmpId;
                }


                Assets obj_assets = lstAssets.Where(x => x.ID == item.AssetId).FirstOrDefault();
                if (obj_assets != null)
                {
                    item.str_assetname = obj_assets.AssetName;
                    item.str_assetno = obj_assets.AssetNo;
                    item.AssetIdentificationNo = obj_assets.AssetIdentificationNo;
                }




                //item.str_empname = db.Employee.Where(x => x.Companyid == companyid && x.ID == item.EmpId).FirstOrDefault().FullName;
                //item.str_assetname = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetName;
                //item.str_assetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;


            }

            return View(emplist);
        }


        [AuthUser]
        [HttpGet]
        public ActionResult IssueAsset()
        {

            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

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
                ViewBag.baseUrl = baseUrl;
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            return View();


        }



        [AuthUser]
        [HttpGet]
        public ActionResult Add()
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
            ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.ID), "ID", "assetcode");
            ViewBag.Emplist = new SelectList(db.Employee.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "FullName");
            return PartialView();

        }


        [HttpGet]
        public ActionResult CheckAssetalreadyassigned(string assetid)
        {
            int int_assetid = Convert.ToInt32(assetid);
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

            string res = "";
            EmployeeAsset empasset = new EmployeeAsset();
            try
            {
                empasset = db.EmployeeAsset.Where(x => x.Companyid == companyid && x.AssetId == int_assetid && x.AssetRecievedFlag == "N").FirstOrDefault();
                if (empasset == null)
                {
                    res = "assetnotallocated";
                }
                else
                {
                    var empname = db.Employee.Where(x => x.Companyid == companyid && x.ID == empasset.EmpId).FirstOrDefault().FullName;
                    res = empname;
                }
            }
            catch (Exception ex)
            {
                res = "Failed";
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);
            }
            return Content(res);
        }


        [AuthUser]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Add(EmployeeAsset employee)
        {
            int userid = 0;
            string res = "";
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


            if (ModelState.IsValid)
            {
                try
                {


                    // validation added by Mandar 20 mar 2022
                    // check if asset already issued

                    EmployeeAsset asset;
                    asset = db.EmployeeAsset.Where(x => x.Companyid == companyid &&
                    x.AssetId == employee.AssetId && x.AssetRecievedFlag == "N").FirstOrDefault();

                    if (asset != null)
                    {
                        res = "Failed";
                        return Content(res);
                    }




                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                    var tnow = System.DateTime.Now.ToUniversalTime();
                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                    // employee.EmpId = db.Employee.Where(x => x.Companyid == companyid && x.EmpId == employee.str_employeeid).FirstOrDefault().ID;
                    employee.EmpId = Convert.ToInt32(employee.str_employeeid);
                    employee.Companyid = companyid;
                    employee.CreatedDate = istDate;
                    employee.CreatedUserId = userid;

                    db.EmployeeAsset.Add(employee);
                    db.SaveChanges();

                    res = "Success";


                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    logger.Log(LogLevel.Error, strError);
                    res = "Failed";

                }

            }


            else
            {
                return RedirectToAction("Login", "Login");
            }
            return Content(res);
        }


        [AuthUser]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            int userid = 0;
            string res = "";
            EmployeeAsset employee = new EmployeeAsset();
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


            try
            {
                employee = db.EmployeeAsset.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                Assets obj_assets = null;
                obj_assets = db.Assetss.Where(x => x.Companyid == companyid && x.ID == employee.AssetId).FirstOrDefault();

                if (obj_assets != null)
                {
                    ViewBag.assetname = obj_assets.assetcode;
                    ViewBag.assetidentificationno = obj_assets.AssetIdentificationNo;
                }
                else
                {
                    ViewBag.assetname = "";
                    ViewBag.assetidentificationno = "";

                }


                ViewBag.employeename = db.Employee.Where(x => x.Companyid == companyid && x.ID == employee.EmpId).FirstOrDefault().FullName;

                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.ID), "ID", "assetcode");
                ViewBag.Emplist = new SelectList(db.Employee.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "FullName");
                employee.str_IssueDate = employee.IssueDate.ToString("dd/MM/yyyy");
                if (employee.RecievedDate != null)
                {
                    employee.str_RecievedDate = Convert.ToDateTime(employee.RecievedDate).ToString("dd/MM/yyyy");

                }
                else
                {
                    employee.str_RecievedDate = "";
                }

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);
                return PartialView();
            }



            return PartialView(employee);
        }


        [AuthUser]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Edit(EmployeeAsset employee)
        {
            JsonResult res;
            res = new JsonResult();
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

            try
            {
                TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                var tnow = System.DateTime.Now.ToUniversalTime();
                DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                EmployeeAsset empobj = new EmployeeAsset();
                empobj = db.EmployeeAsset.Where(x => x.ID == employee.ID && x.Companyid == companyid).FirstOrDefault();
                empobj.AssetId = employee.AssetId;

                empobj.ModifiedDate = istDate;
                empobj.ModifiedUserId = userid;
                empobj.RecievedDate = employee.RecievedDate;
                empobj.EmpId = Convert.ToInt32(employee.str_employeeid);
                //  empobj.EmpId = db.Employee.Where(x => x.Companyid == companyid && x.EmpId == employee.str_employeeid).FirstOrDefault().ID;
                empobj.IssueDate = employee.IssueDate;
                empobj.AssetRecievedFlag = employee.AssetRecievedFlag;

                db.Entry(empobj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                res.Data = "Success";
                return res;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);
                res.Data = "Failed";
                return res;
            }
        }


        // GET: Employee/Delete/5
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Delete(int id)
        {
            JsonResult res;
            res = new JsonResult();
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
            try
            {

                EmployeeAsset emp = new EmployeeAsset();


                emp = db.EmployeeAsset.Where(x => x.Companyid == companyid && x.ID == id).FirstOrDefault();
                db.Entry(emp).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                res.Data = "Success";


            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);
                res.Data = "Failed";
                return res;
            }
            return res;
            // POST: Employee/Delete/5

        }


        [HttpGet]
        [AuthUser]
        public ActionResult EmployeeAssetExport()            //...1
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
            Response.BinaryWrite(generateemployeeassetexcel(companyid));
            string excelName = "EmployeeAsset";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Supplier");

        }
        public byte[] generateemployeeassetexcel(int companyid)        //...2
        {
            List<EmployeeAsset> lstemp = new List<EmployeeAsset>();
            int srno = 1;


            lstemp = db.EmployeeAsset.Where(x => x.Companyid == companyid).ToList();
            foreach (EmployeeAsset item in lstemp)
            {
                item.str_assetname = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetName;
                item.str_assetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;
                item.str_empname = db.Employee.Where(x => x.Companyid == companyid && x.ID == item.EmpId).FirstOrDefault().FullName;
                item.empno = db.Employee.Where(x => x.Companyid == companyid && x.ID == item.EmpId).FirstOrDefault().EmpId;
                item.str_IssueDate = item.IssueDate.ToString("dd/MM/yyyy");
                if (item.RecievedDate != null)
                {
                    item.str_RecievedDate = Convert.ToDateTime(item.RecievedDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_RecievedDate = "";
                }
            }


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
                    new string[] {"EmployeeId", "Employee Name", "AssetNo", "AssetName","IssueDate ","Asset return(Y/N) ","Asset return date"
                        }
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
                foreach (var item in lstemp)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.empno;
                    worksheet.Cells[rowIterator, 2].Value = item.str_empname;
                    worksheet.Cells[rowIterator, 3].Value = item.str_assetno;
                    worksheet.Cells[rowIterator, 4].Value = item.str_assetname;
                    worksheet.Cells[rowIterator, 5].Value = item.str_IssueDate;
                    worksheet.Cells[rowIterator, 6].Value = item.AssetRecievedFlag;
                    worksheet.Cells[rowIterator, 7].Value = item.str_RecievedDate;

                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }

        [HttpGet]
        [AuthUser]
        public ActionResult EmployeeAssetPDF()
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
            Response.BinaryWrite(generateemployeeassetPDF(companyid));
            string pdflName = "EmployeeAssetIssue";
            Response.AppendHeader("content-disposition", "attachment; filename=" + pdflName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "EmployeeAsset");

        }

        public byte[] generateemployeeassetPDF(int companyid)
        {

            List<EmployeeAsset> Elist = new List<EmployeeAsset>();
            int srno = 1;

            Elist = db.EmployeeAsset.Where(x => x.Companyid == companyid).ToList();

            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document(new Rectangle(PageSize.A4.Width * 6, PageSize.A4.Height));
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            document.Add(new Paragraph("Employee Asset Issue"));
            document.Add(new Paragraph(" "));

            string[] headerRow = { "EmployeeId", "Employee Name", "AssetNo", "AssetName", "IssueDate ", "Asset return(Y/N) ", "Asset return date", };

            float[] columnWidths = { 28f, 28f, 28f, 28f, 28f, 28f, 28f,};

            PdfPTable table = new PdfPTable(columnWidths);
            table.WidthPercentage = 100;

            foreach (string header in headerRow)
            {
                PdfPCell cell = new PdfPCell(new Phrase(header));
                table.AddCell(cell);
            }
            table.CompleteRow();
            foreach (var item in Elist)
            {
                table.AddCell(item.Empid);
                table.AddCell(db.Employee.Where(x => x.Companyid == companyid && x.ID == item.EmpId).FirstOrDefault().FullName);
                table.AddCell(db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo);
                table.AddCell(db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetName);           
                table.AddCell(item.IssueDate.ToString("dd/MM/yyyy"));
                table.AddCell(item.AssetRecievedFlag);
                //table.AddCell(Convert.ToDateTime(item.RecievedDate).ToString("dd/MM/yyyy"));
                table.AddCell(item.RecievedDate?.ToString("dd/MM/yyyy"));

                table.CompleteRow();
            }

            document.Add(table);
            document.Close();
   
            byte[] pdfData = memoryStream.ToArray();

            return pdfData;

        }

        // -----------------------------------------------------

        [HttpGet]
        public ActionResult DownloadEmployeeAssetExcel()
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
            Response.BinaryWrite(generateImportemployeeassetexcel());
            string excelName = "employeeasset";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Supplier");

        }
        public byte[] generateImportemployeeassetexcel()
        {
            //List<Insurance> lstins = new List<Insurance>();
            int srno = 1;



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
                   new string[] { "EmpId", "AssetNo","IssueDate ",
                        "AssetReturnFlag(Y/N)","Asset Return Date"}
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




        [AuthUser]
        [HttpGet]
        public ActionResult ImportExcel()
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
        [AllowAnonymous]
        public ActionResult UploadEmployeeAsset()
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

            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            EmployeeAsset emp = new EmployeeAsset();

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
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        Stream stream = file.InputStream;
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));


                        using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                            var tnow = System.DateTime.Now.ToUniversalTime();
                            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);


                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                string a; a = "";
                                string b; b = "";
                                string c; c = "";
                                string d; d = "";
                                string e; e = "";
                                string f; f = "";


                                bool empidflag;
                                bool assetnoflag;
                                bool issuedateflag;
                                bool assetrecievedflag;
                                bool assetrecieveddateflag;



                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    empidflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    empidflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    assetnoflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    assetnoflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    issuedateflag = false;

                                }
                                else
                                {

                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    issuedateflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    assetrecievedflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    assetrecievedflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    e = "";
                                    assetrecieveddateflag = false;
                                }
                                else
                                {
                                    e = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    assetrecieveddateflag = true;
                                }





                                if (empidflag == true && assetnoflag == true && issuedateflag == true && assetrecievedflag == true
                                    )
                                {
                                    norecordsfound = true;
                                    var empid = db.Employee.Where(x => x.Companyid == companyid && x.EmpId == a).FirstOrDefault().ID;
                                    emp.EmpId = empid;
                                    var assetid = db.Assetss.Where(x => x.Companyid == companyid && x.AssetNo == b).FirstOrDefault().ID;
                                    var checkemployeeasset = Importemployeeassetexists(assetid, companyid);
                                    if (checkemployeeasset == "assetalreadyallocated")
                                    {
                                        errorlist.Add("Asset is already assigned  .For row " + rowIterator);
                                        continue;
                                    }
                                    emp.AssetId = assetid;
                                    DateTime dateissuedate;
                                    string Str_issudate = Convert.ToDateTime(c).ToString("yyyy-MM-dd");
                                    if (DateTime.TryParseExact(Str_issudate, "yyyy-MM-dd",
                                                                 System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateissuedate))
                                    {
                                        emp.IssueDate = dateissuedate;
                                    }

                                    if (e != "")
                                    {
                                        DateTime RecievedDate;
                                        string Str_RecievedDate = Convert.ToDateTime(e).ToString("yyyy-MM-dd");
                                        if (DateTime.TryParseExact(Str_RecievedDate, "yyyy-MM-dd",
                                                                     System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out RecievedDate))
                                        {
                                            emp.RecievedDate = RecievedDate;
                                        }
                                        if (emp.IssueDate > emp.RecievedDate)
                                        {
                                            errorlist.Add("Issue Date is greater then Recieved Date .For row " + rowIterator);
                                            continue;
                                        }
                                        // emp.RecievedDate = Convert.ToDateTime(e);


                                    }
                                    else
                                    {

                                    }
                                    emp.AssetRecievedFlag = d;
                                    if (e != "")
                                    {

                                    }
                                    emp.CreatedUserId = userid;
                                    emp.CreatedDate = istDate;
                                    emp.Companyid = companyid;

                                    db.EmployeeAsset.Add(emp);
                                    db.SaveChanges();


                                }
                                else
                                {
                                    errorlist.Add("Something  went wrong or some value missing in the row  " + rowIterator);

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
                logger.Log(LogLevel.Error, strError);
                res.Data = "error";
                return res;
            }
        }
        public string Importemployeeassetexists(int assetid, int companyid)
        {
            string res = "";

            EmployeeAsset empasset = new EmployeeAsset();

            empasset = db.EmployeeAsset.Where(x => x.Companyid == companyid && x.AssetId == assetid).FirstOrDefault();
            if (empasset != null)
            {
                res = "assetalreadyallocated";
            }
            else
            {
                res = "assetnotassigned";
            }


            return res;

        }


        [AuthUser]
        [HttpGet]
        public ActionResult ImportExcelRecievedDate()
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
        [AllowAnonymous]
        public ActionResult UploadEmployeeAssetRecievedDate()
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

            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            EmployeeAsset emp = new EmployeeAsset();

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
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        Stream stream = file.InputStream;
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));


                        using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                            var tnow = System.DateTime.Now.ToUniversalTime();
                            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);


                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                string a; a = "";
                                string b; b = "";
                                string c; c = "";
                                string d; d = "";
                                string e; e = "";
                                string f; f = "";

                                string g; g = "";

                                bool empidflag;
                                bool assetnoflag;
                                bool issuedateflag;
                                bool assetrecievedflag;
                                bool assetrecieveddateflag;
                                bool assetnameflag;
                                bool empnameflag;


                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    empidflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    empidflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    empnameflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    empnameflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    assetnoflag = false;

                                }
                                else
                                {

                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    assetnoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    assetnameflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    assetnameflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    e = "";
                                    issuedateflag = false;
                                }
                                else
                                {
                                    e = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    issuedateflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 6].Text == "")
                                {
                                    f = "";
                                    assetrecievedflag = false;
                                }
                                else
                                {
                                    f = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    assetrecievedflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 7].Text == "")
                                {
                                    g = "";
                                    assetrecieveddateflag = false;
                                }
                                else
                                {
                                    g = workSheet.Cells[rowIterator, 7].Value.ToString();
                                    assetrecieveddateflag = true;
                                }




                                if (empidflag == true && assetnoflag == true && issuedateflag == true && assetrecievedflag == true
                                    && assetrecieveddateflag == true
                                    )
                                {
                                    norecordsfound = true;
                                    var empid = db.Employee.Where(x => x.Companyid == companyid && x.EmpId == a).FirstOrDefault().ID;

                                    var assetid = db.Assetss.Where(x => x.Companyid == companyid && x.AssetNo == c).FirstOrDefault().ID;
                                    var issuedate = Convert.ToDateTime(e);
                                    var recieveddate = Convert.ToDateTime(g);
                                    if (issuedate > recieveddate)
                                    {
                                        errorlist.Add("Issue date cannot be greater than recieveddate  of the row  " + rowIterator);
                                    }
                                    emp = db.EmployeeAsset.Where(x => x.Companyid == companyid && x.EmpId == empid && x.AssetId == assetid && x.IssueDate == issuedate && x.RecievedDate == null).FirstOrDefault();
                                    if (emp != null)
                                    {
                                        emp.ModifiedUserId = userid;
                                        emp.RecievedDate = Convert.ToDateTime(g);
                                        emp.ModifiedDate = istDate;
                                        emp.Companyid = companyid;

                                        db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        errorlist.Add("This record doesn't exist in Employee Asset  of the row  " + rowIterator);
                                    }

                                }
                                else
                                {
                                    errorlist.Add("Something  went wrong or some value missing in the row  " + rowIterator);

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
                logger.Log(LogLevel.Error, strError);
                res.Data = "error";
                return res;
            }
        }

       
        [HttpPost]
        public ActionResult DownloadAndSendEmail()
        {
           
            //byte[] pdfBytes = GeneratePDF(); 

             
            string pdfFileName = "EmployeeAssetIssue.pdf";
            string pdfFilePath = Server.MapPath("~/PDFs/" + pdfFileName);
         //   System.IO.File.WriteAllBytes(pdfFilePath, pdfBytes);

            
            SendEmailWithAttachment(pdfFilePath); 

            return Json(new { success = true });
        }

        
        //private byte[] GeneratePDF()
        //{
            
        //}

        private ActionResult SendEmailWithAttachment(string filePath)
        {
            try
            {
                // Sender's email address and password
                string senderEmail = "Mayuri.shendre@pixonix.tech";
                string senderPassword = "your_password";

                // Recipient's email address
                string recipientEmail = "mayurishendre.sfdc@gmail.com";

              
                var message = new MailMessage(senderEmail, recipientEmail);


                message.Subject = "Hello from Turbotrack";
                message.Body = "This is a test email sent from an application.";

                // Configure the SMTP client
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                };

                // Send the email
                smtpClient.Send(message);

                ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Email sending failed: " + ex.Message;
            }
            return View();

        }
    }


    }
    

