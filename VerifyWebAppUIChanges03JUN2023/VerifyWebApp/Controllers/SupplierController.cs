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
    public class SupplierController : Controller
    {
        public VerifyDB db = new VerifyDB();
        //GET: Supplier
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


            List<Supplier> lstSupplier = new List<Supplier>();
            lstSupplier = db.Suppliers.Where(x => x.Companyid == companyid).ToList();
            return View(lstSupplier);
        }

        private List<Supplier> SortByColumnWithOrder(string order, string orderDir, List<Supplier> data)
        {
            // Initialization.   
            List<Supplier> lst = new List<Supplier>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SupplierCode).ToList() : data.OrderBy(p => p.SupplierCode).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SupplierName).ToList() : data.OrderBy(p => p.SupplierName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmailID).ToList() : data.OrderBy(p => p.EmailID).ToList();
                        break;

                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SupplierName).ToList() : data.OrderBy(p => p.SupplierName).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
            }
            // info.   
            return lst;
        }
        [HttpPost]
        public ActionResult GetData()
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

            JsonResult result = new JsonResult();
            try
            {
                // Initialization.   
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.   
                List<Supplier> data = db.Suppliers.ToList();
                // Total record count.   
                int totalRecords = data.Count;
                // Verification.   
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    data = data.Where(p => p.SupplierName.ToString().ToLower().Contains(search.ToLower())).ToList();

                }
                // Sorting.   
                data = this.SortByColumnWithOrder(order, orderDir, data); //TODO Mandar
                // Filter record count.   
                int recFilter = data.Count;
                // Apply pagination.   
                data = data.Skip(startRec).Take(pageSize).ToList();
                // Loading drop down lists.   
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info   
                //Console.Write(ex);
                // log error
            }
            // Return info.   
            return result;

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

            //return View();
            return PartialView();
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


            Supplier supplier = new Supplier();
            try
            {
                supplier = db.Suppliers.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                return PartialView();
            }
            return PartialView(supplier);
            // return View(supplier);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]

        public ActionResult Add(Supplier supplier)
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

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            string res = "";
            if (ModelState.IsValid)
            {
                //Supplier Objsupplier = new Supplier();
                try
                {
                    supplier.CreatedUserId = userid;
                    supplier.CreatedDate = istDate;
                    supplier.Companyid = companyid;

                    db.Suppliers.Add(supplier);
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
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]

        public ActionResult Edit(Supplier supplier)
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
            try
            {
                //Supplier sobj = new Supplier();
                supplier.Modified_Userid = userid;
                supplier.ModifiedDate = istDate;
                supplier.Companyid = companyid;

                db.Entry(supplier).State = System.Data.Entity.EntityState.Modified;
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
            Supplier supplier;
            List<Assets> Asset = new List<Assets>();
            Asset = db.Assetss.Where(x => x.SupplierNo == id && x.Companyid == companyid).ToList();
            try
            {
                if (Asset.Count != 0)
                {
                    res.Data = "Failed";

                }
                else
                {
                    supplier = db.Suppliers.Where(x => x.ID == id).FirstOrDefault();
                    db.Entry(supplier).State = System.Data.Entity.EntityState.Deleted;
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
        [HttpGet]
        [AuthUser]
        public ActionResult SupplierExport()
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
            Response.BinaryWrite(generatesupplierexcel(companyid));
            string excelName = "Supplier";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Supplier");

        }
        public byte[] generatesupplierexcel(int companyid)
        {
            List<Supplier> lstins = new List<Supplier>();
            int srno = 1;


            lstins = db.Suppliers.Where(x=>x.Companyid==companyid).ToList();

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
                    new string[] { "Sr No","ID", "SupplierCode", "Supplier Name","Contact Person ","Address ",
                        "City","Pincode","Phone No","Mobile No","Fax No","Excise No",
                        "Service No","Vat No","Cst No","Other No","Pan No","Tan No","Emailid","Gst No","Shop Act License","Mseme No"}
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
                    worksheet.Cells[rowIterator, 2].Value = item.ID;
                    worksheet.Cells[rowIterator, 3].Value = item.SupplierCode;
                    worksheet.Cells[rowIterator, 4].Value = item.SupplierName;
                    worksheet.Cells[rowIterator, 5].Value = item.ContactPerson;
                    worksheet.Cells[rowIterator, 6].Value = item.Address;
                    worksheet.Cells[rowIterator, 7].Value = item.City;
                    worksheet.Cells[rowIterator, 8].Value = item.Pincode;
                    worksheet.Cells[rowIterator, 9].Value = item.PhoneNo;
                    worksheet.Cells[rowIterator, 10].Value = item.MobileNo;
                    worksheet.Cells[rowIterator, 11].Value = item.FaxNo;
                    worksheet.Cells[rowIterator, 12].Value = item.ExciseRegNo;
                    worksheet.Cells[rowIterator, 13].Value = item.ServiceTaxRegNo;
                    worksheet.Cells[rowIterator, 14].Value = item.VATRegNo;
                    worksheet.Cells[rowIterator, 15].Value = item.CSTRegNo;
                    worksheet.Cells[rowIterator, 16].Value = item.AnyOtherRegNo;
                    worksheet.Cells[rowIterator, 17].Value = item.PANNo;
                    worksheet.Cells[rowIterator, 18].Value = item.TANNo;
                    worksheet.Cells[rowIterator, 19].Value = item.EmailID;
                    worksheet.Cells[rowIterator, 20].Value = item.GSTNo;
                    worksheet.Cells[rowIterator, 21].Value = item.ShopActLicence;
                    worksheet.Cells[rowIterator, 22].Value = item.Msemeno;
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        [HttpGet]

        public ActionResult DownloadSupplierExcel()
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
            Response.BinaryWrite(generateImportsupplierexcel());
            string excelName = "Supplier";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Supplier");

        }
        public byte[] generateImportsupplierexcel()
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
                   new string[] { "Sr No", "SupplierCode", "Supplier Name","Contact Person ","Address ",
                        "City","Pincode","Phone No","Mobile No","Fax No","Excise No",
                        "Service No","Vat No","Cst No","Other No","Pan No","Tan No","Emailid","Gst No","Shop Act License","Mseme No"}
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

        public ActionResult UploadSupplier()
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


            Supplier supplier = new Supplier();

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
                                string h; h = "";
                                string i; i = "";
                                string j; j = "";
                                string k; k = "";
                                string l; l = "";

                                string m; m = "";
                                string n; n = "";
                                string o; o = "";
                                string p; p = "";
                                string q; q = "";

                                string r; r = "";
                                string s; s = "";
                                string t; t = "";
                                string u; u = "";

                                bool srnoflag;
                                bool suppliercodeflag;
                                bool suppliernameflag;
                                bool contactpersonflag;
                                bool addressflag;
                                bool cityflag;

                                bool phonenoflag;
                                bool mobilenoflag;
                                bool faxnoflag;
                                bool excisenoflag;
                                bool servicetaxnoflag;
                                bool cstnoflag;

                                bool vatnoflag;
                                bool othernoflag;
                                bool pannoflag;
                                bool pincodeflag;
                                bool tannnoflag;
                                bool gstnoflag;
                                bool shopactnoflag;
                                bool msemenoflag;
                                bool emailflag;
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
                                    suppliercodeflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    suppliercodeflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    suppliernameflag = false;

                                }
                                else
                                {

                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    suppliernameflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    contactpersonflag = false;
                                }
                                else
                                {
                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    contactpersonflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    e = "";
                                    addressflag = false;
                                }
                                else
                                {
                                    e = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    addressflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 6].Text == "")
                                {
                                    f = "";
                                    cityflag = false;
                                }
                                else
                                {
                                    f = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    cityflag = true;
                                }

                                ////////////////////

                                if (workSheet.Cells[rowIterator, 7].Text == "")
                                {
                                    g = "";
                                    pincodeflag = false;
                                }
                                else
                                {
                                    g = workSheet.Cells[rowIterator, 7].Value.ToString();
                                    pincodeflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 8].Text == "")
                                {
                                    h = "";
                                    phonenoflag = false;
                                }
                                else
                                {
                                    h = workSheet.Cells[rowIterator, 8].Value.ToString();
                                    phonenoflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 9].Text == "")
                                {
                                    i = "";
                                    mobilenoflag = false;

                                }
                                else
                                {

                                    i = workSheet.Cells[rowIterator, 9].Value.ToString();
                                    mobilenoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 10].Text == "")
                                {
                                    j = "";
                                    faxnoflag = false;
                                }
                                else
                                {
                                    j = workSheet.Cells[rowIterator, 10].Value.ToString();
                                    faxnoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 11].Text == "")
                                {
                                    k = "";
                                    excisenoflag = false;
                                }
                                else
                                {
                                    k = workSheet.Cells[rowIterator, 11].Value.ToString();
                                    excisenoflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 12].Text == "")
                                {
                                    l = "";
                                    servicetaxnoflag = false;
                                }
                                else
                                {
                                    l = workSheet.Cells[rowIterator, 12].Value.ToString();
                                    servicetaxnoflag = true;
                                }

                                //////
                                if (workSheet.Cells[rowIterator, 13].Text == "")
                                {
                                    m = "";
                                    vatnoflag = false;
                                }
                                else
                                {
                                    m = workSheet.Cells[rowIterator, 13].Value.ToString();
                                    vatnoflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 14].Text == "")
                                {
                                    n = "";
                                    cstnoflag = false;
                                }
                                else
                                {
                                    n = workSheet.Cells[rowIterator, 14].Value.ToString();
                                    cstnoflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 15].Text == "")
                                {
                                    o = "";
                                    othernoflag = false;

                                }
                                else
                                {

                                    o = workSheet.Cells[rowIterator, 15].Value.ToString();
                                    othernoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 16].Text == "")
                                {
                                    p = "";
                                    pannoflag = false;
                                }
                                else
                                {
                                    p = workSheet.Cells[rowIterator, 16].Value.ToString();
                                    pannoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 17].Text == "")
                                {
                                    q = "";
                                    tannnoflag = false;
                                }
                                else
                                {
                                    q = workSheet.Cells[rowIterator, 17].Value.ToString();
                                    tannnoflag = true;

                                }
                                ////
                                if (workSheet.Cells[rowIterator, 18].Text == "")
                                {
                                    r = "";
                                    emailflag = false;

                                }
                                else
                                {

                                    r = workSheet.Cells[rowIterator, 18].Value.ToString();
                                    emailflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 19].Text == "")
                                {
                                    s = "";
                                    gstnoflag = false;
                                }
                                else
                                {
                                    s = workSheet.Cells[rowIterator, 19].Value.ToString();
                                    gstnoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 20].Text == "")
                                {
                                    t = "";
                                    shopactnoflag = false;
                                }
                                else
                                {
                                    t = workSheet.Cells[rowIterator, 20].Value.ToString();
                                    shopactnoflag = true;

                                }
                                if (workSheet.Cells[rowIterator, 21].Text == "")
                                {
                                    u = "";
                                    msemenoflag = false;
                                }
                                else
                                {
                                    u = workSheet.Cells[rowIterator, 21].Value.ToString();
                                    msemenoflag = true;

                                }




                                //if (srnoflag == true && suppliercodeflag == true && suppliernameflag == true && contactpersonflag == true
                                //    && addressflag == true && cityflag == true && pincodeflag == true && phonenoflag == true
                                //    && mobilenoflag == true && faxnoflag == true && excisenoflag == true && servicetaxnoflag == true &&
                                //    vatnoflag == true && pannoflag == true && tannnoflag == true && othernoflag == true
                                //    && cstnoflag == true && emailflag == true && gstnoflag == true
                                //    && shopactnoflag == true)
                                //{
                                    if (srnoflag == true && suppliercodeflag == true && suppliernameflag == true )/*&& contactpersonflag == true*/
                                    //&& addressflag == true && cityflag == true && pincodeflag == true && phonenoflag == true
                                    //&& mobilenoflag == true && faxnoflag == true && excisenoflag == true && servicetaxnoflag == true &&
                                    //vatnoflag == true && pannoflag == true && tannnoflag == true && othernoflag == true
                                    //&& cstnoflag == true && emailflag == true && gstnoflag == true
                                    //&& shopactnoflag == true)
                                    {

                                        supplier.Srno = Convert.ToInt32(a);
                                    supplier.SupplierCode = b;
                                    supplier.SupplierName = c;
                                    supplier.ContactPerson = d;
                                    supplier.Address = e;
                                    supplier.City = f;

                                    // todo change name of variables from a b c to actual names 
                                    if (g.Length > 0) // mandar added this on 23 nov 2021
                                    {
                                        supplier.Pincode = Convert.ToInt32(g);
                                    }
                                    
                                    supplier.PhoneNo = h;
                                    supplier.MobileNo = i;
                                    supplier.FaxNo = j;
                                    supplier.ExciseRegNo = k;
                                    supplier.ServiceTaxRegNo = l;


                                    supplier.VATRegNo = m;
                                    supplier.CSTRegNo = n;

                                    supplier.AnyOtherRegNo = o;
                                    supplier.PANNo = p;
                                    supplier.TANNo = q;
                                    supplier.EmailID = r;
                                    supplier.GSTNo = s;
                                    supplier.ShopActLicence = t;
                                    supplier.Msemeno = u;
                                    supplier.CreatedUserId = userid;
                                    supplier.CreatedDate = istDate;
                                    supplier.Companyid = companyid;

                                    db.Suppliers.Add(supplier);
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