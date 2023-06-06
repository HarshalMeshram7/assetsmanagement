using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
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

            List<Employee> emplist = new List<Employee>();
            emplist = db.Employee.Where(x => x.Companyid == companyid).ToList();

            return View(emplist);
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
            return PartialView();

        }

        [AuthUser]
        [HttpPost]
        [ValidateJsonXssAttribute]
        public ActionResult Add(Employee employee)
        {
            int userid = 0;
            //string res = "";

            JsonResult res;

            res = new JsonResult();

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
                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                    var tnow = System.DateTime.Now.ToUniversalTime();
                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

                    employee.Companyid = companyid;
                    employee.CreatedDate = istDate;
                    employee.CreatedUserId = userid;

                    db.Employee.Add(employee);
                    db.SaveChanges();

                    res.Data = "Success";

                    //res = "Success";


                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    //  logger.Log(LogLevel.Error, strError);
                    res.Data =  "Failed";

                }

            }


            else
            {
                return RedirectToAction("Login", "Login");
            }
            // return Content(res);
            return res;
        }

        [AuthUser]
        [HttpGet]

        public ActionResult Edit(int id)
        {
            int userid = 0;
            string res = "";
            Employee employee = new Employee();
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
                employee = db.Employee.Where(x => x.ID == id && x.Companyid == companyid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                return PartialView();
            }



            return PartialView(employee);
        }
        [AuthUser]
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonXssAttribute]
        
        public ActionResult Edit(Employee employee)
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
                Employee empobj = new Employee();
                empobj = db.Employee.Where(x => x.ID == employee.ID && x.Companyid == companyid).FirstOrDefault();
                empobj.MiddleName = employee.MiddleName;

                empobj.ModifiedDate = istDate;
                empobj.ModifiedUserId = userid;
                empobj.EmpId = employee.EmpId;
                empobj.FirstName = employee.FirstName;
                empobj.LastName = employee.LastName;
                empobj.Address1 = employee.Address1;
                empobj.Mobileno = employee.Mobileno;
                empobj.Emailid = employee.Emailid;
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
                List<EmployeeAsset> empasset = new List<EmployeeAsset>();
                Employee emp = new Employee();
                
                empasset = db.EmployeeAsset.Where(x => x.Companyid == companyid && x.EmpId == id).ToList();
                if (empasset.Count() == 0)
                {
                    emp = db.Employee.Where(x => x.Companyid == companyid && x.ID == id).FirstOrDefault();
                    db.Entry(emp).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    res.Data = "Success";
                }
                else
                {
                    res.Data = "Failed";
                }

            }
            catch(Exception ex)
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
        public ActionResult EmployeeExport()
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
            Response.BinaryWrite(generateemployeeexcel(companyid));
            string excelName = "Employee";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Supplier");

        }
        public byte[] generateemployeeexcel(int companyid)
        {
            List<Employee> lstemp = new List<Employee>();
            int srno = 1;


            lstemp = db.Employee.Where(x=>x.Companyid==companyid).ToList();

            

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
                    new string[] {"EmployeeId","FirstName", "Middle Name", "Last Name", "MobileNo","EmailId ","Address "
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
                    worksheet.Cells[rowIterator, 1].Value = item.EmpId;
                    worksheet.Cells[rowIterator, 2].Value = item.FirstName;
                    worksheet.Cells[rowIterator, 3].Value = item.MiddleName;
                    worksheet.Cells[rowIterator, 4].Value = item.LastName;
                    worksheet.Cells[rowIterator, 5].Value = item.Mobileno;
                    worksheet.Cells[rowIterator, 6].Value = item.Emailid;
                    worksheet.Cells[rowIterator, 7].Value = item.Address1;
                  
                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }
        [HttpGet]

        public ActionResult DownloadEmployeeExcel()
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
            Response.BinaryWrite(generateImportemployeeexcel());
            string excelName = "employee";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Supplier");

        }
        public byte[] generateImportemployeeexcel()
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
                   new string[] {"EmployeeId", "FirstName", "Middle Name", "Last Name","Mobile No ","Emailid ",
                        "Address"}
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
        
        [AllowAnonymous]
        public ActionResult UploadEmployee()
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


            Employee emp = new Employee();

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
                                bool empidflag;
                                bool firstnameflag;
                                bool middlenameflag;
                                bool lastnameflag;
                                bool mobilenoflag;
                                bool emialidflag;

                                bool addressflag;



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
                                    firstnameflag = false;
                                }
                                else
                                {
                                    b = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    firstnameflag = true;
                                }
                                if (workSheet.Cells[rowIterator, 3].Text == "")
                                {
                                    c = "";
                                    middlenameflag = false;
                                }
                                else
                                {
                                    c = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    middlenameflag = true;
                                }



                                if (workSheet.Cells[rowIterator, 4].Text == "")
                                {
                                    d = "";
                                    lastnameflag = false;

                                }
                                else
                                {

                                    d = workSheet.Cells[rowIterator, 4].Value.ToString();
                                    lastnameflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 5].Text == "")
                                {
                                    e = "";
                                    mobilenoflag = false;
                                }
                                else
                                {
                                    e = workSheet.Cells[rowIterator, 5].Value.ToString();
                                    mobilenoflag = true;
                                }

                                if (workSheet.Cells[rowIterator, 6].Text == "")
                                {
                                    f = "";
                                    emialidflag = false;
                                }
                                else
                                {
                                    f = workSheet.Cells[rowIterator, 6].Value.ToString();
                                    emialidflag = true;

                                }

                                if (workSheet.Cells[rowIterator, 7].Text == "")
                                {
                                    g = "";
                                    addressflag = false;
                                }
                                else
                                {
                                    g = workSheet.Cells[rowIterator, 7].Value.ToString();
                                    addressflag = true;
                                }

                             


                                if (empidflag == true && firstnameflag == true && lastnameflag == true && mobilenoflag == true && emialidflag == true
                                    )
                                {

                                    emp.EmpId = a;
                                    emp.FirstName = b;
                                    emp.MiddleName = c;
                                    emp.LastName = d;
                                    emp.Mobileno = e;
                                    emp.Emailid = f;
                                    emp.Address1 = g;

                                    emp.CreatedUserId = userid;
                                    emp.CreatedDate = istDate;
                                    emp.Companyid = companyid;

                                    db.Employee.Add(emp);
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
                 logger.Log(LogLevel.Error, strError);
                res.Data = "error";
                return res;
            }
        }

        private List<Employee> SortByColumnWithOrder(string order, string orderDir, List<Employee> data)
        {
            // Initialization.   
            List<Employee> lst = new List<Employee>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FirstName).ToList() : data.OrderBy(p => p.FirstName).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LastName).ToList() : data.OrderBy(p => p.LastName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.EmpId).ToList() : data.OrderBy(p => p.EmpId).ToList();
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
                List<Employee> data = db.Employee.ToList();
                // Total record count.   
                int totalRecords = data.Count;
                // Verification.   
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    data = data.Where(p => p.FirstName.ToString().ToLower().Contains(search.ToLower())).ToList();

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

    }
}
