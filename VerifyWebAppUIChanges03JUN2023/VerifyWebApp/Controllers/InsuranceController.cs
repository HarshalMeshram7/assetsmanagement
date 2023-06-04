using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;
using System.Data.Entity;
using VerifyWebApp.Filter;

namespace VerifyWebApp.Controllers
{
    public class InsuranceController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Period
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            List<Insurance> lstins = new List<Insurance>();
            int srno = 1;
            try
            {

                lstins = db.Insurances.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstins)
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

            return View(lstins);
        }

        [AuthUser]
        [HttpGet]
        public ActionResult AddNew()
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

            Insurance ins = new Insurance();


            ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.ID), "AssetNo", "AssetNo");
            //  @Html.DropDownList("DesignationID", (SelectList)ViewBag.DesignationList, new { @class = "form-control" })



            //return PartialView();

            return View();
        }

        private DateTime ParseExact(string str_fromdate, string v, CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
        }

        // GET: Brand/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AddNew(InsuranceViewmodel insuranceViewmodel)
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

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);




            if (ModelState.IsValid)
            {
                Insurance insobj = new Insurance();
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {


                        insobj.FromDate = insuranceViewmodel.FromDate;
                        insobj.ToDate = insuranceViewmodel.ToDate;
                        insobj.EMailID = insuranceViewmodel.EMailID;
                        insobj.Senderccemailid = insuranceViewmodel.Senderccemailid;
                        insobj.PolicyDetails = insuranceViewmodel.PolicyDetails;
                        insobj.Remarks = insuranceViewmodel.Remarks;
                        insobj.CreatedUserId = userid;
                        insobj.CreatedDate = istDate;
                        insobj.Companyid = companyid;
                        db.Insurances.Add(insobj);
                        db.SaveChanges();
                        //var insuranceid = db.Insurances.Max(x => x.ID && x.Companyid==companyid);
                        var insuranceid = insobj.ID;
                        SubInsurance subins = new SubInsurance();
                        if (insuranceViewmodel.InsuranceViewModellist.Count() != 0)
                        {
                            foreach (var item in insuranceViewmodel.InsuranceViewModellist)
                            {
                                subins.InsuranceId = insuranceid;
                                if (item.AssetNo != "")
                                {
                                    var assetid = db.Assetss.Where(x => x.AssetNo == item.AssetNo && x.Companyid == companyid).FirstOrDefault().ID;
                                    item.AssetId = assetid;
                                }
                                subins.AssetId = item.AssetId;
                                subins.AssetDescription = item.AssetDescription;
                                subins.CapitalisedAmount = item.CapitalisedAmount;
                                subins.CreatedUserId = userid;
                                subins.CreatedDate = istDate;
                                subins.Companyid = companyid;
                                db.SubInsurances.Add(subins);
                                db.SaveChanges();

                            }
                        }
                        transaction.Commit();
                        return RedirectToAction("Index", "Insurance");
                        // res.Data = "Success";
                        //return res;

                    }
                    catch (Exception ex)
                    {
                        string strError;
                        strError = ex.Message + "|" + ex.InnerException;
                        // logger.Log(LogLevel.Error, strError);
                        transaction.Rollback();
                        return RedirectToAction("Index", "Insurance");
                        // res.Data = "Failed";
                        //return res;

                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Insurance");
                //res.Data = "ERR";
                //return res;

            }
        }



        [AuthUser]
        [HttpGet]
        public ActionResult EditNew(int id)
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


            Insurance insurance = new Insurance();
            List<SubInsurance> splist = new List<SubInsurance>();
            try
            {
                insurance = db.Insurances.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();


                splist = db.SubInsurances.Where(x => x.InsuranceId == id && x.Companyid == companyid).ToList();
                insurance = db.Insurances.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                ViewBag.FromDate = insurance.FromDate.ToString("dd/MM/yyyy");
                ViewBag.ToDate = insurance.ToDate.ToString("dd/MM/yyyy");
                ViewBag.Email = insurance.EMailID;
                ViewBag.Senderccemail = insurance.Senderccemailid;
                ViewBag.policydetails = insurance.PolicyDetails;
                ViewBag.remark = insurance.Remarks;
                ViewBag.ID = id;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.ID), "AssetNo", "AssetNo");
                int srno = 1;
                foreach (SubInsurance item in splist)
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
                    //   item.AssetNo = item.AssetNo;
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
        // [AuthUser]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Edit(InsuranceViewmodel insurance)
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
            JsonResult res;
            res = new JsonResult();


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Insurance insobj = new Insurance();
                    insobj = db.Insurances.Where(x => x.ID == insurance.ID && x.Companyid == companyid).FirstOrDefault();
                    insobj.FromDate = insurance.FromDate;
                    insobj.ToDate = insurance.ToDate;
                    insobj.EMailID = insurance.EMailID;
                    insobj.Senderccemailid = insurance.Senderccemailid;
                    insobj.PolicyDetails = insurance.PolicyDetails;
                    insobj.Remarks = insurance.Remarks;
                    insobj.Modified_Userid = userid;
                    insobj.ModifiedDate = istDate;
                    insobj.Companyid = companyid;
                    db.Entry(insobj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    // SubInsurance subins = new SubInsurance();
                    List<SubInsurance> slist = new List<SubInsurance>();
                    slist = db.SubInsurances.Where(x => x.InsuranceId == insurance.ID && x.Companyid == companyid).ToList();
                    db.SubInsurances.RemoveRange(slist);
                    db.SaveChanges();
                    SubInsurance subins = new SubInsurance();
                    if (insurance.InsuranceViewModellist.Count() != 0)
                    {
                        foreach (var item in insurance.InsuranceViewModellist)
                        {
                            subins.InsuranceId = insurance.ID;
                            if (item.AssetNo != "")
                            {
                                var assetid = db.Assetss.Where(x => x.AssetNo == item.AssetNo && x.Companyid == companyid).FirstOrDefault().ID;
                                item.AssetId = assetid;
                            }
                            subins.AssetId = item.AssetId;
                            subins.AssetDescription = item.AssetDescription;
                            subins.CapitalisedAmount = item.CapitalisedAmount;
                            subins.ModifiedDate = istDate;
                            subins.Modified_Userid = userid;
                            subins.Companyid = companyid;
                            db.SubInsurances.Add(subins);
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
            Insurance amcobj;
            List<SubInsurance> subplist = new List<SubInsurance>();
            subplist = db.SubInsurances.Where(x => x.InsuranceId == id && x.Companyid == companyid).ToList();

            try
            {
                if (subplist.Count == 0)
                {

                    amcobj = db.Insurances.Where(x => x.ID == id).FirstOrDefault();
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
                //ViewBag.LoggedCompany = company.CompanyName;
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
        [HttpGet]

        public ActionResult InsuranceExport()
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



            Response.ClearContent();
            Response.BinaryWrite(generateinsuranceexcel(companyid));
            string excelName = "Insurance";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "Insurance");

        }
        public byte[] generateinsuranceexcel(int companyid)
        {

            int srno = 1;

            List<Insurance> lstins = new List<Insurance>();
            lstins = db.Insurances.Where(x => x.Companyid == companyid).ToList();

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
                    new string[] { "Sr No", "FromDate", "ToDate","Reminder Mail","Reminder Mail CC","Policy Details","Remarks",}
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
                    worksheet.Cells[rowIterator, 4].Value = item.EMailID;
                    worksheet.Cells[rowIterator, 5].Value = item.Senderccemailid;
                    worksheet.Cells[rowIterator, 6].Value = item.PolicyDetails;
                    worksheet.Cells[rowIterator, 7].Value = item.Remarks;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        [HttpGet]
        public ActionResult DownloadInsuranceExcel()
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


            Response.ClearContent();
            Response.BinaryWrite(generateImportinsuranceexcel());
            string excelName = "Insurance";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Insurance");

        }
        public byte[] generateImportinsuranceexcel()
        {
            List<Insurance> lstins = new List<Insurance>();
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
                    new string[] { "Sr No", "FromDate", "ToDate","Reminder Mail", "Reminder Mail CC", "Policy Details","Remarks",}
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
                // ViewBag.LoggedCompany = company.CompanyName;
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
        public ActionResult UploadInsurance()
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
                ViewBag.companyid = company.ID;
                //ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            Insurance insurance = new Insurance();

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

                                bool srnoflag;
                                bool fromdateflag;
                                bool todateflag;
                                bool remindermailflag;
                                bool reminderccmailflag;
                                bool policydetailsflag;
                                bool remarkflag;
                                int designationid = 0;
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
                                    remindermailflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    remindermailflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    e = "";
                                    reminderccmailflag = false;
                                }
                                else
                                {
                                    e = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    reminderccmailflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 6].Text == "")
                                {
                                    f = "";
                                    policydetailsflag = false;
                                }
                                else
                                {
                                    f = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    policydetailsflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 7].Text == "")
                                {
                                    g = "";
                                    remarkflag = false;
                                }
                                else
                                {
                                    g = workSheet.Cells[rowIterator, 7].Value.ToString();
                                    remarkflag = true;
                                }


                                if (srnoflag == true && fromdateflag == true && todateflag == true && remindermailflag == true && reminderccmailflag == true && policydetailsflag == true && remarkflag == true)
                                {

                                    norecordsfound = true;
                                    DateTime fromdate = Convert.ToDateTime(b);
                                    DateTime todate = Convert.ToDateTime(c);
                                    insurance.Srno = Convert.ToInt32(a);
                                    insurance.FromDate = fromdate;
                                    insurance.ToDate = todate;
                                    insurance.EMailID = d;
                                    insurance.Senderccemailid = e;
                                    insurance.PolicyDetails = f;
                                    insurance.Remarks = g;
                                    insurance.CreatedUserId = userid;
                                    insurance.CreatedDate = istDate;
                                    insurance.Companyid = companyid;






                                    db.Insurances.Add(insurance);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    errorlist.Add("Something is missing in row  " + rowIterator);

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
