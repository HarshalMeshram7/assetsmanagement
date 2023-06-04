using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using System.Net;
using System.Security.Cryptography;

using VerifyWebApp.Filter;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class LoginController : Controller
    {
        public VerifyDB db = new VerifyDB();
        RNGCryptoServiceProvider rProvider = new RNGCryptoServiceProvider();

        enum Password_Options
        {
            ALPHANUM,
            ALL
        }

        // GET: Login
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
            if (user.Userlevel == "Admin")
            {
                List<Login> lstlogin = new List<Login>();
                lstlogin = db.Logins.Where(x => x.CompanyId == companyid).ToList();
                return View(lstlogin);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }


            //return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {


            string strPath = "";
            strPath = Server.MapPath("~");

            //strPath = HttpRunTime.AppDomainAppPath;
            try
            {
                strPath = System.Configuration.ConfigurationManager.AppSettings["VerifyLicenseFile"];
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("IsAppAccess", "License Configuration Setup is Missing !!!");
                return View(login);
            }

            if (strPath == null || strPath.Trim().Length ==0)
            {

                ModelState.AddModelError("IsAppAccess", "License Configuration Setup is Missing !!!");
                return View(login);
            }

            VerifyLicenseV2.VLicense license = new VerifyLicenseV2.VLicense();


            License dbLicense = db.Licenses.FirstOrDefault();
            if (dbLicense == null)
            {
                // redirect to License Error Page
                ModelState.AddModelError("IsAppAccess", "License Expired!");
                return View(login);
            }


          //  bool bCheckLicense = license.IsValidatLicense(strPath, dbLicense.CompanyCode); ;
            // TEMP FIX 
           //bool bCheckLicense = true;

            //if (bCheckLicense == false)
            //{
            //    // redirect to License Error Page
            //    ModelState.AddModelError("IsAppAccess", "License Expired!");
            //    return View(login);
            //}


            if (isValid(login.UserName, login.Password))
            {
                //FormsAuthentication.SetAuthCookie(tenant.Email, true);

                Login loggedInUser = db.Logins.Where(x => x.UserName == login.UserName).FirstOrDefault();

                if (loggedInUser.IsTwoFactor == "yes")
                {
                    
                    TempData["OTPUser"] = loggedInUser.ID;
                    Random random = new Random();
                    int number = random.Next(1001, 9999);
                    TempData["OTP"] = number;

                    loggedInUser.OTP = number.ToString();

                    db.Entry(loggedInUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    string url = "https://2factor.in/API/V1/3c3dd735-0800-11e8-a996-0200cd936042/SMS/" + loggedInUser.MobileNo  + "/" + number.ToString();

                    WebRequest request = WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    response.Close();



                    return RedirectToAction("OTP", "Login");
                    
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(loggedInUser.ID.ToString(), true);
                    ViewBag.LogonUser = loggedInUser.UserName;


                    Login userid = (Login)(Session["PUser"]);
                    if (userid == null)
                    {
                        Session.Add("PUser", loggedInUser);

                    }
                    else
                    {

                    }
                    return RedirectToAction("CompanySelection", "Company");
                }
            }
            else
            {
                ModelState.AddModelError("Password", "Emailid or Password is incorrect!");
            }

            return View(login);
        }

        [HttpGet]
        public ActionResult OTP()
        {
            ViewBag.OTPUser = TempData["OTPUser"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult OTP(Login login)
        {

            Login loggedInUser = db.Logins.Where(x => x.ID == login.ID).FirstOrDefault();
            
            if (loggedInUser!= null)
            {

                if (loggedInUser.OTP == login.OTP)
                {
                    FormsAuthentication.SetAuthCookie(loggedInUser.ID.ToString(), true);
                    ViewBag.LogonUser = loggedInUser.UserName;


                    Login userid = (Login)(Session["PUser"]);
                    if (userid == null)
                    {
                        Session.Add("PUser", loggedInUser);

                    }
                    else
                    {

                    }
                    //return Json(Url.Action("CompanySelection", "Company"));

                    APIResponse resp = new APIResponse();
                    resp.status = "True";
                    resp.data = Url.Action("CompanySelection", "Company");
                    return Json(resp);


                    //return RedirectToAction("CompanySelection", "Company",null);

                }
                else
                {
                    //ModelState.AddModelError("OTP", "OTP is incorrect!");
                    //Dictionary<string, string> err = new Dictionary<string, string>();
                    //err.Add("error", "Please input correct OTP");

                    APIResponse response = new APIResponse();
                    response.status = "error";
                    return Json(response);

                    //return View();
                }
            }else
            {
                return View();
            }
            

        }


        private bool isValid(string _username, string _password)
        {


            string hshpassword;
            hshpassword = System.Web.Util.Security.HashSHA1(_password);

            IEnumerable<Login> filteredUsers;

            filteredUsers = from c in db.Logins
                            where c.UserName == _username && c.Password == hshpassword
                            select c;



            //IEnumerable<TenantUser> filteredUsers;

            //filteredUsers = from c in db.TenantUsers
            //                where c.Email== _username && c.pwd == hshpassword
            //                select c;

            if (filteredUsers.Count() == 1)
                return true;
            else
                return false;


        }
        public ActionResult Logout()
        {
            // Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
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
            if (user.Userlevel == "Admin")
            {

                return PartialView();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
   
        [ValidateJsonAntiForgeryToken]
        public ActionResult Add(Login login)
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
            if (user.Userlevel == "Admin")
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                        var tnow = System.DateTime.Now.ToUniversalTime();
                        DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                        login.Password = System.Web.Util.Security.HashSHA1(login.Password);
                        login.CompanyId = companyid;
                        login.CreatedDate = istDate;
                        login.CreatedUserId = userid;
                        login.Userlevel = "User";
                        login.ClientID = 1;

                        db.Logins.Add(login);
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
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            return Content(res);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            int userid = 0;
            string res = "";
            Login login = new Login();
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
            if (user.Userlevel == "Admin")
            {

                try
                {
                    login = db.Logins.Where(x => x.ID == id && x.CompanyId == companyid).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    // logger.Log(LogLevel.Error, strError);
                    return PartialView();
                }
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            return PartialView(login);
            // return View();
        }

        [HttpPost]
  
        [ValidateJsonAntiForgeryToken]
        public ActionResult Edit(Login login)
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
                    Login loginobj = new Login();
                    loginobj = db.Logins.Where(x => x.ID == login.ID && x.CompanyId == companyid).FirstOrDefault();

                    //loginobj.UserName = login.UserName;
                    //    loginobj.Password = System.Web.Util.Security.HashSHA1(login.Password);

                    loginobj.ModifiedDate = istDate;
                    loginobj.ModifiedUserId = userid;
                    //loginobj.Userlevel = "User";
                    loginobj.FirstName = login.FirstName;
                    loginobj.LastName = login.LastName;
                    loginobj.Address = login.Address;
                    loginobj.MobileNo = login.MobileNo;
                    loginobj.EmailId = login.EmailId;
                    loginobj.IsAppAccess = login.IsAppAccess;
                    loginobj.IsTwoFactor = login.IsTwoFactor;

                    db.Entry(loginobj).State = System.Data.Entity.EntityState.Modified;
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
            
            
        }

        [HttpGet]
        [OutputCache(Duration = 0)]

        public ActionResult ChangePassword()
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


            Login loginInfo;
            loginInfo = db.Logins.Where(x => x.CompanyId == companyid && x.ID == userid).FirstOrDefault();
            loginInfo.Password = "";
            return View(loginInfo);
        }
        [HttpPost]
        public ActionResult ChangePassword(Login loginInfo)
        {
            try
            {

                Login ExistingloginInfo;
                ExistingloginInfo = db.Logins.Where(x => x.ID == loginInfo.ID).FirstOrDefault();
                string chkPwd;
                string newpwd;

                chkPwd = System.Web.Util.Security.HashSHA1(loginInfo.Password);
                newpwd = System.Web.Util.Security.HashSHA1(loginInfo.NewPassword);
                if (ExistingloginInfo.Password == chkPwd)
                {
                    if (ExistingloginInfo.Password == newpwd)
                    {
                        ModelState.AddModelError("Password", "Old Password And New Password cannot be same");
                    }
                    else
                    {
                        ExistingloginInfo.Password = System.Web.Util.Security.HashSHA1(loginInfo.NewPassword);
                        ExistingloginInfo.ConfirmPassword = System.Web.Util.Security.HashSHA1(loginInfo.ConfirmPassword);
                        ExistingloginInfo.NewPassword = System.Web.Util.Security.HashSHA1(loginInfo.NewPassword);

                        db.Entry(ExistingloginInfo).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Login", "Login");
                    }
                }
                else
                {

                    ModelState.AddModelError("Password", "Old Password is incorrect");
                }




            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                // return PartialView();
            }

            return View(loginInfo);

        }

        [HttpGet]
        public ActionResult GeneratePassword()
        {
            JsonResult res;
            res = new JsonResult();

            string password = "";
            password = CreatePassword(6, Password_Options.ALL);
            res.Data = password;

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        private string CreatePassword(int length, Password_Options options)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const string valid_all = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-=+{}:;\\<>?|,./`~[]'";
            using (RNGCryptoServiceProvider rProvider = new RNGCryptoServiceProvider())
            {
                var source = options == Password_Options.ALPHANUM ? valid_all : valid;
                byte[] random = new byte[source.Length];
                rProvider.GetBytes(random);
                var pwd = (from k in
                               from c in source select new { c = c, o = random[source.IndexOf(c)] }
                           orderby k.o
                           select k.c).Take(length).ToArray();
                return new string(pwd);

            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult ResetUserPassword(Login login)
        {
            JsonResult res;
            res = new JsonResult();

            try
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

                /*
                TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standad Time");
                var tnow = System.DateTime.Now.ToUniversalTime();
                DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                */

                DateTime istDate = DateTime.Today;

                Login loginobj = new Login();
                loginobj = db.Logins.Where(x => x.ID == login.ID && x.CompanyId == companyid).FirstOrDefault();

                //loginobj.UserName = login.UserName;
                loginobj.Password = System.Web.Util.Security.HashSHA1(login.Password);

                loginobj.ModifiedDate = istDate;
                loginobj.ModifiedUserId = userid;

                db.Entry(loginobj).State = System.Data.Entity.EntityState.Modified;
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

        }
        [HttpGet]
        public ActionResult ResetUserPassword(int id)
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
            if (user.Userlevel == "Admin")
            {
                Login login = db.Logins.Where(x => x.ID == id).FirstOrDefault();
                return PartialView(login);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    }
}