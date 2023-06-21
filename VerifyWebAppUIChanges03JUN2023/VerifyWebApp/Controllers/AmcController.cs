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
    public class AmcController : Controller
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
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            List<AMC> lstins = new List<AMC>();
            int srno = 1;
            try
            {

                lstins = db.AMCss.Where(x=>x.Companyid==companyid).ToList();

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
            // int userid = 0;
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
            // Insurance ins = new Insurance();
            AMC amc = new AMC();


            ViewBag.Assestlist = new SelectList(db.Assetss.Where(x=>x.DisposalFlag==0 && x.Companyid==companyid).OrderBy(e => e.ID), "AssetNo", "AssetNo");
            //  @Html.DropDownList("DesignationID", (SelectList)ViewBag.DesignationList, new { @class = "form-control" })


            return View();
            //return PartialView();

        }

        private DateTime ParseExact(string str_fromdate, string v, CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
        }

        // GET: Brand/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]                             
        public ActionResult AddNew(AmcViewmodel amcViewmodel)
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
                AMC amcobj = new AMC();
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                       //TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                       // var tnow = System.DateTime.Now.ToUniversalTime();
                       // DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                        amcobj.CreatedDate = istDate;
                        amcobj.Companyid = companyid; 
                        amcobj.CreatedUserId = userid;
                        amcobj.FromDate = amcViewmodel.FromDate;
                        amcobj.ToDate = amcViewmodel.ToDate;
                        amcobj.ReminderEMail = amcViewmodel.ReminderEMailID;
                        amcobj.Senderccemailid= amcViewmodel.Senderccemailid;
                        amcobj.AMCDetails = amcViewmodel.AmcDetails;
                        amcobj.Remarks = amcViewmodel.Remarks;
                        db.AMCss.Add(amcobj);
                        db.SaveChanges();
                        var amcid = amcobj.ID;
                        // var amcid = db.AMCss.Max(x => x.ID && x.Companyid==companyid);
                        SubAmc subamc = new SubAmc();
                        if (amcViewmodel.AmcViewModellist.Count() != 0)
                        {
                            foreach (var item in amcViewmodel.AmcViewModellist)
                            {

                                subamc.AmcId = amcid;
                                if (item.AssetNo != "")
                                {
                                    var assetid = db.Assetss.Where(x => x.AssetNo == item.AssetNo && x.Companyid==companyid).FirstOrDefault().ID;

                                    subamc.AssetId = assetid;
                                }
                              
                                subamc.AssetDescription = item.AssetDescription;
                                subamc.CapitalisedAmount = item.CapitalisedAmount;
                                subamc.CreatedUserId = userid;
                                subamc.Companyid = companyid;
                                subamc.CreatedDate = istDate;
                                db.SubAmc.Add(subamc);
                                db.SaveChanges();

                            }
                        }
                        transaction.Commit();
                       // return RedirectToAction("Index", "Amc");


                    }
                    catch (Exception ex)
                    {
                        string strError;
                        strError = ex.Message + "|" + ex.InnerException;
                        // logger.Log(LogLevel.Error, strError);
                        transaction.Rollback();
                        //return RedirectToAction("Index", "Amc");
                        bResult = false;
                    }

                }
            }
            else
            {
                // return RedirectToAction("Index", "Amc");
                /// todo redirect to common error page
                bResult = false;

            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            AMC amc = new AMC();
            List<SubAmc> splist = new List<SubAmc>();
            try
            {
                amc = db.AMCss.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault();
                splist = db.SubAmc.Where(x => x.AmcId == id && x.Companyid==companyid).ToList();
                
                amc = db.AMCss.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
                ViewBag.FromDate = amc.FromDate.ToString("dd/MM/yyyy");
                ViewBag.ToDate = amc.ToDate.ToString("dd/MM/yyyy");
                ViewBag.Email = amc.ReminderEMail;
                ViewBag.Senderccemail = amc.Senderccemailid;
                ViewBag.amcdetails = amc.AMCDetails;
                ViewBag.remark = amc.Remarks;
                ViewBag.ID = id;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid ).OrderBy(e => e.ID), "AssetNo", "AssetNo");
                int srno = 1;
                foreach (SubAmc item in splist)
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
            //return PartialView(splist);
            return View(splist);
        }
        [AuthUser]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Edit(AmcViewmodel amcViewmodel)
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
                    AMC amcobj = new AMC();
                    amcobj = db.AMCss.Where(x => x.ID == amcViewmodel.ID && x.Companyid == companyid).FirstOrDefault();
                    amcobj.FromDate = amcViewmodel.FromDate;
                    amcobj.ToDate = amcViewmodel.ToDate;
                    amcobj.ReminderEMail = amcViewmodel.ReminderEMailID;
                    amcobj.Senderccemailid = amcViewmodel.Senderccemailid;
                    amcobj.AMCDetails = amcViewmodel.AmcDetails;
                    amcobj.Remarks = amcViewmodel.Remarks;
                    amcobj.Companyid = companyid;
                    amcobj.ModifiedDate = istDate;
                    amcobj.Modified_Userid = userid;
                    db.Entry(amcobj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    // SubInsurance subins = new SubInsurance();
                    List<SubAmc> slist = new List<SubAmc>();
                    slist = db.SubAmc.Where(x => x.AmcId == amcViewmodel.ID && x.Companyid==companyid).ToList();
                    db.SubAmc.RemoveRange(slist);
                    db.SaveChanges();
                    //SubAmc subins=new SubAmc();
                    SubAmc subamc = new SubAmc();
                    if (amcViewmodel.AmcViewModellist.Count() != 0)
                    {
                        foreach (var item in amcViewmodel.AmcViewModellist)
                        {
                            subamc.AmcId = amcViewmodel.ID;
                            if (item.AssetNo != "")
                            {
                                var assetid = db.Assetss.Where(x => x.AssetNo == item.AssetNo && x.Companyid==companyid).FirstOrDefault().ID;
                                item.AssetId = assetid;
                            }
                            subamc.AssetId = item.AssetId;
                            subamc.AssetDescription = item.AssetDescription;
                            subamc.CapitalisedAmount = item.CapitalisedAmount;
                            subamc.Modified_Userid = userid;
                            subamc.ModifiedDate = istDate;
                            subamc.Companyid = amcobj.Companyid;
                            db.SubAmc.Add(subamc);
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
            AMC amcobj;
            List<SubAmc> subplist = new List<SubAmc>();
            subplist = db.SubAmc.Where(x => x.AmcId == id && x.Companyid == companyid).ToList();

            try
            {
                if (subplist.Count == 0)
                {

                    amcobj = db.AMCss.Where(x => x.ID == id).FirstOrDefault();
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            JsonResult res;
            res = new JsonResult();
            int int_id = Convert.ToInt32(id);
            List<Assets> list = new List<Assets>();


            list = db.Assetss.Where(x => x.AssetNo == id && x.DisposalFlag==0 && x.Companyid==companyid).ToList();
            res.Data = list;
            return Json(res, JsonRequestBehavior.AllowGet);


        }

        [AuthUser]
        [HttpGet]

        public ActionResult AmcExport()
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
            string excelName = "Amc";
            Response.AddHeader("content-dispostion", "attachment;filename="+excelName+".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Amc");

        }
        public byte[] generateinsuranceexcel(int companyid)
        {
            

            List<AMC> lstins = new List<AMC>();
            int srno = 1;


            lstins = db.AMCss.Where(x=>x.Companyid==companyid).ToList();

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
                    new string[] { "Sr No", "FromDate", "ToDate","Reminder Mail","Send Reminder mail to CC","Amc Details","Remarks",}
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
                    worksheet.Cells[rowIterator, 4].Value = item.ReminderEMail;
                    worksheet.Cells[rowIterator, 5].Value = item.Senderccemailid;
                    worksheet.Cells[rowIterator, 6].Value = item.AMCDetails;
                    worksheet.Cells[rowIterator, 7].Value = item.Remarks;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }

        [HttpGet]
        public ActionResult DownloadAmcExcel()
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
            string excelName = "Amc";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

           return  RedirectToAction("Index", "Amc");
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
                    new string[] { "Sr No", "FromDate", "ToDate","Reminder Mail","Send Reminder Mail to CC","Amc Details","Remarks",}
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
        public ActionResult UploadAmc()
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


            AMC amc = new AMC();

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
                                bool remindermailflag;
                                bool reminderccmailflag;
                                bool amcdetailsflag;
                                bool remarkflag;
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
                                    amcdetailsflag = false;
                                }
                                else
                                {
                                    f = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    amcdetailsflag = true;

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



                                if (srnoflag == true && fromdateflag == true && todateflag == true && remindermailflag == true && reminderccmailflag == true)
                                {
                                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                    var tnow = System.DateTime.Now.ToUniversalTime();
                                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                                    norecordsfound = true;

                                    DateTime fromdate = Convert.ToDateTime(b);
                                    DateTime todate = Convert.ToDateTime(c);
                                    amc.Srno = Convert.ToInt32(a);
                                    amc.FromDate = fromdate;
                                    amc.ToDate = todate;
                                    amc.ReminderEMail = d;
                                    amc.Senderccemailid = e;
                                    amc.AMCDetails = f;
                                    amc.Remarks = g;
                                    amc.Companyid = companyid;
                                    amc.CreatedDate = istDate;
                                    amc.CreatedUserId = userid;
                                    db.AMCss.Add(amc);
                                    db.SaveChanges();

                                }
                                else {
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
        public ActionResult Assetexport()
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
            Response.BinaryWrite(generateassetexcel(companyid));
            string excelName = "assetexport";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Amc");



            
        }

        public byte[] generateassetexcel(int companyid)
        {


            List<Assets> lstins = new List<Assets>();
            int srno = 1;


            lstins = db.Assetss.Where(x => x.Companyid == companyid).ToList();

            foreach (var item in lstins)
            {
                item.Srno = srno;
                item.AssetNo = item.AssetNo;
                item.AssetName = item.AssetName;
                item.AmountCapitalisedCompany=item.AmountCapitalisedCompany;
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
                    new string[] { "Sr No", "AssetNo", "AssetName","AmountCapitalisedCompany"}
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
                    worksheet.Cells[rowIterator, 2].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.AmountCapitalisedCompany;
                   
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        //public ActionResult ImportAssetExcel()
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
        //    }
        //    else
        //    {
        //        return RedirectToAction("CompanySelection", "Company");
        //    }
        //    return PartialView();
        //}
        [HttpPost]
        [ValidateAjax]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult UploadAsset()
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


            List<Asset_Import_Amc> alist = new List<Asset_Import_Amc>();
            Asset_Import_Amc assetamc = new Asset_Import_Amc();

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

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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


                                bool assetnoflag;
                                bool assetnameflag;
                                bool amtflag;
                               
                                //  int designationid = 0;
                                if (workSheet.Cells[rowIterator, 1].Text == "")
                                {
                                    a = "";
                                    assetnoflag = false;
                                }
                                else
                                {
                                    a = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    assetnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 2].Text == "")
                                {
                                    b = "";
                                    assetnameflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    assetnameflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    amtflag = false;

                                }
                                else
                                {

                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    amtflag = true;
                                }

                             



                                if (assetnoflag == true && assetnameflag == true && amtflag == true)
                                {

                                    assetamc = new Asset_Import_Amc();
                                        assetamc.assetno = a;
                                        assetamc.assetname = b;
                                        assetamc.amountcapcompany = Convert.ToDecimal(c);
                                        alist.Add(assetamc);
                                        norecordsfound = true;
                                    
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
                                res.Data = this.Json(new
                                {
                                    data = alist
                                }, JsonRequestBehavior.AllowGet);

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
