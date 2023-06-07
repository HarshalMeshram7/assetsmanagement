using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using System.Data.Entity;
using VerifyWebApp.Filter;

namespace VerifyWebApp.Controllers
{
    public class AccountController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Account
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

            List<Account> lstAccount = new List<Account>();
            lstAccount = db.Accounts.Where(x => x.Companyid == companyid).ToList();
            return View(lstAccount);
            //return View();
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            List<string> Group = new List<string>();
            Group.Add("Asset");
            Group.Add("Liability");
            Group.Add("Income");
            Group.Add("Expense");

            // ViewBag.Group = Group;
            ViewBag.Group = new SelectList(Group);
            //return View();
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]

        public ActionResult Add(Account account)
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

            Account objAcc = new Account();
            objAcc.GroupName = account.GroupName;
            if (objAcc.GroupName == "1")
            {
                objAcc.GroupName = "Asset";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }
            else if (objAcc.GroupName == "2")
            {
                objAcc.GroupName = "Liability";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }
            else if (objAcc.GroupName == "3")
            {
                objAcc.GroupName = "Income";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }
            else if (objAcc.GroupName == "4")
            {
                objAcc.GroupName = "Expense";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            string res = "";
            if (ModelState.IsValid)
            {
                try
                {
                    objAcc.AccountCode = account.AccountCode;
                    objAcc.AccountName = account.AccountName;
                    objAcc.CreatedUserId = userid;
                    objAcc.CreatedDate = istDate;
                    objAcc.Companyid = companyid;
                    db.Accounts.Add(objAcc);
                    db.SaveChanges();

                    res = "Success";


                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    //  logger.Log(LogLevel.Error, strError);
                    res = "Failed";

                }

            }
            else
            {
                res = "ERR";


            }

            return Content(res);
        }
        [AuthUser]
        [HttpGet]
        public ActionResult Edit(int id)
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

            Account account = new Account();
            try
            {

                account = db.Accounts.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                ViewBag.GroupName = account.GroupName;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                return PartialView();
            }
            return PartialView(account);
            // return View(supplier);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]

        public ActionResult Edit(Account account)
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

            Account objAcc = new Account();
            objAcc.GroupName = account.GroupName;
            if (objAcc.GroupName == "1")
            {
                objAcc.GroupName = "Asset";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }
            else if (objAcc.GroupName == "2")
            {
                objAcc.GroupName = "Liability";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }
            else if (objAcc.GroupName == "3")
            {
                objAcc.GroupName = "Income";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }
            else if (objAcc.GroupName == "4")
            {
                objAcc.GroupName = "Expense";
                objAcc.AccountCode = account.AccountCode;
                objAcc.AccountName = account.AccountName;
            }


            JsonResult res;
            res = new JsonResult();
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            try
            {
                objAcc.ID = account.ID;
                objAcc.AccountName = account.AccountName;
                objAcc.AccountCode = account.AccountCode;
                objAcc.Modified_Userid = userid;
                objAcc.ModifiedDate = istDate;
                objAcc.Companyid = companyid;
                db.Entry(objAcc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                res.Data = "Success";
                return res;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                res.Data = "Failed";
                return res;
            }
            //return View();
        }
        [AuthUser]
        [HttpPost]
        [AllowAnonymous]
        
        public ActionResult Delete(int id)
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

            JsonResult res;
            res = new JsonResult();
            Account account;
            List<Assets> Asset = new List<Assets>();
            List<Assets> Asset1 = new List<Assets>();
            List<Assets> Asset2 = new List<Assets>();

            Asset = db.Assetss.Where(x => x.AccountID == id && x.Companyid == companyid).ToList();
            Asset1 = db.Assetss.Where(x => x.AccAccountID == id && x.Companyid == companyid).ToList();
            Asset2 = db.Assetss.Where(x => x.DepAccountId == id && x.Companyid == companyid).ToList();

            try
            {
                if (Asset.Count != 0 && Asset1.Count != 0 && Asset2.Count != 0)
                {
                    res.Data = "Failed";

                }
                else
                {
                    account = db.Accounts.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(account).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    res.Data = "Success";


                }
                return res;
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                res.Data = "Err";
                return res;
            }

        }
        [AuthUser]
        public ActionResult AccountExport()
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
            Response.BinaryWrite(generateaccountexcel(companyid));
            string excelName = "Account";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Account");

        }
        public byte[] generateaccountexcel(int companyid)
        {
            List<Account> lstins = new List<Account>();
            int srno = 1;


            lstins = db.Accounts.Where(x=>x.Companyid==companyid).ToList();

            foreach (var item in lstins)
            {
                item.Srno = srno;

                srno++;
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
                    new string[] { "Sr No", "AccountCode", "Account Name","Group Name" }

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
                foreach (var item in lstins)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.Srno;
                    worksheet.Cells[rowIterator, 2].Value = item.AccountCode;
                    worksheet.Cells[rowIterator, 3].Value = item.AccountName;
                    worksheet.Cells[rowIterator, 4].Value = item.GroupName;
                   

                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }


        [HttpGet]
        public ActionResult DownloadAccountExcel()
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
            Response.BinaryWrite(generateImportAccountexcel());
            string excelName = "Account";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Account");

        }
        public byte[] generateImportAccountexcel()
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
                   new string[] { "Sr No", "Account Code", "Account Name","Group Name "}
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
        public ActionResult UploadAccount()
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


            Account account = new Account();

            try
            {
                if (Request != null)
                {
                    bool norecordsfound = false;

                    HttpPostedFileBase file;

                    file = null;
                    HttpFileCollectionBase files = Request.Files;

                    if (files.Count > 0)
                    {
                        file = files[0];
                        string fileName = file.FileName;

                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        Stream stream = file.InputStream;
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                               

                                bool srnoflag;
                                bool AccountCodeflag;
                                bool AccountNameflag;
                                bool GroupNameflag;
                               

                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    srnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    srnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    AccountCodeflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    AccountCodeflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    AccountNameflag = false;

                                }
                                else
                                {

                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    AccountNameflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    GroupNameflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    GroupNameflag = true;
                                }

                               


                                //if (srnoflag == true && suppliercodeflag == true && suppliernameflag == true && contactpersonflag == true
                                //    && addressflag == true && cityflag == true && pincodeflag == true && phonenoflag == true
                                //    && mobilenoflag == true && faxnoflag == true && excisenoflag == true && servicetaxnoflag == true &&
                                //    vatnoflag == true && pannoflag == true && tannnoflag == true && othernoflag == true
                                //    && cstnoflag == true && emailflag == true && gstnoflag == true
                                //    && shopactnoflag == true)
                                //{
                                if (srnoflag == true && AccountNameflag == true)
                                {

                                    account.Srno = Convert.ToInt32(a);
                                    account.AccountCode = b;
                                    account.AccountName = c;
                                    account.GroupName = d;

                                    account.CreatedUserId = userid;
                                    account.CreatedDate = istDate;
                                    account.Companyid = companyid;

                                    db.Accounts.Add(account);
                                    db.SaveChanges();

                                }
                                norecordsfound = true;
                            }
                            if (norecordsfound == false)
                            {

                                res.Data = "nodata";
                                return res;

                            }
                            else
                            {
                                res.Data = "Success";
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