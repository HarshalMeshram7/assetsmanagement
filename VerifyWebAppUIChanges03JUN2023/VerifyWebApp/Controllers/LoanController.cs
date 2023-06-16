using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class LoanController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Loan
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

            List<Loan> lstloan = new List<Loan>();
            int srno = 1;
            try
            {

                lstloan = db.Loans.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstloan)
                {
                    item.Srno = srno;
                    item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                    item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                    srno++;
                }
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            ViewBag.Srno = srno;

            return View(lstloan);
            // return View();
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

            // int userid = 0;
            Loan ins = new Loan();
            //List<Assets> alist = new List<Assets>();
            //alist = db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).ToList();

            ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(z => z.ID), "AssetNo", "AssetNo");
            //  @Html.DropDownList("DesignationID", (SelectList)ViewBag.DesignationList, new { @class = "form-control" })



            // return PartialView();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]                    
        public ActionResult Add(LoanViewmodel loanViewmodel)
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

            JsonResult res;
            res = new JsonResult();
            bool bResult = true;

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            if (ModelState.IsValid)
            {
                Loan loanobj = new Loan();
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        loanobj.FromDate = loanViewmodel.FromDate;
                        loanobj.ToDate = loanViewmodel.ToDate;
                        loanobj.BankName = loanViewmodel.BankName;
                        loanobj.Year = loanViewmodel.Year;
                        loanobj.Amount = loanViewmodel.Amount;
                        loanobj.Percent = loanViewmodel.Percent;
                        loanobj.CreatedUserId = userid;
                        loanobj.CreatedDate = istDate;
                        loanobj.Companyid = companyid;

                        db.Loans.Add(loanobj);
                        db.SaveChanges();

                        // var loanid = db.Loans.Max(x => x.ID);
                        var loanid = loanobj.ID;
                        SubLoan subloan = new SubLoan();
                        if (loanViewmodel.LoanViewModellist.Count() != 0)
                        {
                            foreach (var item in loanViewmodel.LoanViewModellist)
                            {
                                subloan.LoanId = loanid;
                                if (item.AssetNo != "")
                                {
                                    var assetid = db.Assetss.Where(x => x.AssetNo == item.AssetNo && x.Companyid == companyid).FirstOrDefault().ID;
                                    item.AssetId = assetid;
                                }
                                subloan.AssetId = item.AssetId;
                                subloan.AssetDescription = item.AssetDescription;
                                subloan.CapitalisedAmount = item.CapitalisedAmount;
                                subloan.CreatedUserId = userid;
                                subloan.CreatedDate = istDate;
                                subloan.Companyid = loanobj.Companyid;
                                db.SubLoans.Add(subloan);
                                db.SaveChanges();

                            }
                        }
                        transaction.Commit();
                        //return RedirectToAction("Index", "Loan");
                        //res.Data = "Success";
                        //return res;

                    }
                    catch (Exception ex)
                    {
                        string strError;
                        strError = ex.Message + "|" + ex.InnerException;
                        // logger.Log(LogLevel.Error, strError);
                        transaction.Rollback();
                        //return RedirectToAction("Index", "Loan");
                        //res.Data = "Failed";
                        //return res;
                        bResult = false;

                    }

                }
            }
            else
            {
                //return RedirectToAction("Index", "Loan");
                //res.Data = "ERR";
                //return res;
                bResult = false;

            }

            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private DateTime ParseExact(string str_fromdate, string v, CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
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

            Loan loan = new Loan();
            List<SubLoan> splist = new List<SubLoan>();
            try
            {
                loan = db.Loans.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();


                splist = db.SubLoans.Where(x => x.LoanId == id && x.Companyid == companyid).ToList();
                loan = db.Loans.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                ViewBag.FromDate = loan.FromDate.ToString("dd/MM/yyyy");
                ViewBag.ToDate = loan.ToDate.ToString("dd/MM/yyyy");

                ViewBag.BankName = loan.BankName;
                ViewBag.Year = loan.Year;
                ViewBag.Percent = loan.Percent;
                ViewBag.Amount = loan.Amount;
                ViewBag.ID = id;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.ID), "AssetNo", "AssetNo");
                int srno = 1;
                foreach (SubLoan item in splist)
                {
                    item.Srno = srno;
                    if (item.AssetId != 0)
                    {
                        var assetno = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                        item.AssetNo = assetno;
                    }
                    else
                    {
                        item.AssetNo = "";
                    }
                    //  item.AssetNo = item.AssetNo;
                    item.AssetDescription = item.AssetDescription;
                    item.CapitalisedAmount = item.CapitalisedAmount;
                    srno++;
                }
                ViewBag.Srno = srno;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            // return PartialView(splist);
            return View(splist);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Edit(LoanViewmodel loanViewmodel)
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

            JsonResult res;
            res = new JsonResult();


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Loan loanobj = new Loan();
                    loanobj = db.Loans.Where(x => x.ID == loanViewmodel.ID && x.Companyid == companyid).FirstOrDefault();
                    loanobj.FromDate = loanViewmodel.FromDate;
                    loanobj.ToDate = loanViewmodel.ToDate;
                    loanobj.BankName = loanViewmodel.BankName;
                    loanobj.Year = loanViewmodel.Year;
                    loanobj.Amount = loanViewmodel.Amount;
                    loanobj.Percent = loanViewmodel.Percent;
                    loanobj.Modified_Userid = userid;
                    loanobj.ModifiedDate = istDate;
                    loanobj.Companyid = companyid;

                    db.Entry(loanobj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    // SubInsurance subins = new SubInsurance();
                    List<SubLoan> slist = new List<SubLoan>();
                    slist = db.SubLoans.Where(x => x.LoanId == loanViewmodel.ID && x.Companyid == companyid).ToList();
                    db.SubLoans.RemoveRange(slist);
                    db.SaveChanges();
                    SubLoan subloan = new SubLoan();
                    if (loanViewmodel.LoanViewModellist.Count() != 0)
                    {
                        foreach (var item in loanViewmodel.LoanViewModellist)
                        {
                            subloan.LoanId = loanViewmodel.ID;
                            if (item.AssetNo != "")
                            {
                                var assetid = db.Assetss.Where(x => x.AssetNo == item.AssetNo && x.Companyid == companyid).FirstOrDefault().ID;
                                item.AssetId = assetid;
                            }
                            subloan.AssetId = item.AssetId;
                            subloan.AssetDescription = item.AssetDescription;
                            subloan.CapitalisedAmount = item.CapitalisedAmount;
                            subloan.Modified_Userid = userid;
                            subloan.ModifiedDate = istDate;
                            subloan.Companyid = loanobj.Companyid;

                            db.SubLoans.Add(subloan);
                            db.SaveChanges();

                        }
                    }
                    transaction.Commit();
                    res.Data = "Success";
                    return res;

                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    // logger.Log(LogLevel.Error, strError);
                    transaction.Rollback();
                    res.Data = "Failed";
                    return res;
                }
            }
        }

        [HttpGet]
        public ActionResult getassetinfo(string id)
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

            JsonResult res;
            res = new JsonResult();
            // int int_id = Convert.ToInt32(id);
            List<Assets> list = new List<Assets>();


            list = db.Assetss.Where(x => x.AssetNo == id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
            res.Data = list;
            return Json(res, JsonRequestBehavior.AllowGet);


        }
       
        [AuthUser]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
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
            Loan amcobj;
            List<SubLoan> subplist = new List<SubLoan>();
            subplist = db.SubLoans.Where(x => x.LoanId == id && x.Companyid == companyid).ToList();

            try
            {
                if (subplist.Count == 0)
                {

                    amcobj = db.Loans.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(amcobj).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    res.Data = "Success";

                }
                else
                {

                    res.Data = "Failed";

                }
                return res;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                //logger.Log(LogLevel.Error, strError);
                res.Data = "Err";
                return res;
            }

        }
        
        [AuthUser]
        [HttpGet]
        public ActionResult LoanExport()
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
            Response.BinaryWrite(generateinsuranceexcel(companyid));
            string excelName = "Loan";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Loan");

        }
        public byte[] generateinsuranceexcel(int companyid)
        {


            List<Loan> lstins = new List<Loan>();
            int srno = 1;


            lstins = db.Loans.Where(x => x.Companyid == companyid).ToList();

            foreach (var item in lstins)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
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
                    new string[] { "Sr No", "FromDate", "ToDate","Bank Name","Year","Percent", "Amount"}
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
                    worksheet.Cells[rowIterator, 2].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 3].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 4].Value = item.BankName;
                    worksheet.Cells[rowIterator, 5].Value = item.Year;
                    worksheet.Cells[rowIterator, 6].Value = item.Percent;
                    worksheet.Cells[rowIterator, 7].Value = item.Amount;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }

        [HttpGet]
        public ActionResult DownloadLoanExcel()
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
            Response.BinaryWrite(generateImportamcexcel());
            string excelName = "LoanExcel";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "Loan");
        }
        public byte[] generateImportamcexcel()
        {

            int srno = 1;
            Login user = (Login)(Session["PUser"]);
            Company company = (Company)(Session["Cid"]);

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
                    new string[] { "Sr No", "FromDate", "ToDate","Bank Name","Year","Percent","Amount"}
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
        [ValidateJsonAntiForgeryToken]
        public ActionResult UploadLoan()
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


            Loan loan = new Loan();

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
                                string g; g = "";


                                bool srnoflag;
                                bool fromdateflag;
                                bool todateflag;
                                bool banknameflag;
                                bool yearflag;
                                bool percentflag;
                                bool amountflag;


                                //  int designationid = 0;
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
                                    fromdateflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    fromdateflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    todateflag = false;

                                }
                                else
                                {

                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    todateflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    banknameflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    banknameflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    e = "";
                                    yearflag = false;
                                }
                                else
                                {
                                    e = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    yearflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 6].Text == "")
                                {
                                    f = "";
                                    percentflag = false;
                                }
                                else
                                {
                                    f = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    percentflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 6].Text == "")
                                {
                                    g = "";
                                    amountflag = false;
                                }
                                else
                                {
                                    g = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    amountflag = true;
                                }


                                if (srnoflag == true && fromdateflag == true && todateflag == true && banknameflag == true && amountflag == true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;

                                    DateTime fromdate = Convert.ToDateTime(b);
                                    DateTime todate = Convert.ToDateTime(c);
                                    loan.Srno = Convert.ToInt32(a);
                                    loan.FromDate = fromdate;
                                    loan.ToDate = todate;
                                    loan.BankName = d;
                                    loan.Year = Convert.ToInt32(e);
                                    loan.Percent = Convert.ToInt32(f);
                                    loan.Amount = Convert.ToInt32(g);
                                    loan.Companyid = companyid;
                                    loan.CreatedDate = istDate;
                                    loan.CreatedUserId = userid;
                                    db.Loans.Add(loan);
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