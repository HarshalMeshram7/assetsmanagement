using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.ViewModel;
using VerifyWebApp.Models;

using System.IO;
using VerifyWebApp.Filter;

namespace VerifyWebApp.Controllers
{
    public class ITGroupController : Controller
    {
        public VerifyDB db = new VerifyDB();

        // GET: ITGroup
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

            return View();
        }
        [AuthUser]
        [HttpPost]

        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult SaveLocationNode(JsITGroupTreeNode node)
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

            bool bResult = true;
            try
            {
                TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                // var value1 = Request.Form.Get(keys[]);
                var tnow = System.DateTime.Now.ToUniversalTime();

                JsonResult res = new JsonResult();
                DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

                string Level = "";
                Level = node.id.Substring(0, 2);
                int tempLength = node.id.Length;
                string id = node.id.Substring(3, tempLength - 3);

                if (Level == "L0") // selected parent level
                {
                    ITGroup aLocation = new ITGroup();
                    aLocation.ClientID = 1;
                    aLocation.GroupName = node.GroupName;
                    aLocation.OPWDV = node.OPWDV;
                    aLocation.DepRate = node.DepRate;
                    aLocation.DepMethod = node.DepMethod;
                    aLocation.Companyid = companyid;
                    aLocation.CreatedUserId = userid;
                    aLocation.CreatedDate = istDate;

                    db.ITGroups.Add(aLocation);
                    db.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                bResult = false;
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GetLocations()
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

            List<JsTreeModel> list = new List<JsTreeModel>();

            List<ITGroup> lstALocation = new List<ITGroup>();
            //List<BLocation> lstBLocation = new List<BLocation>();
            //List<CLocation> lstCLocation = new List<CLocation>();


            lstALocation = db.ITGroups.Where(x => x.Companyid == companyid).ToList();

            //lstBLocation = db.BLocations.ToList();

            //lstCLocation = db.CLocations.ToList();

            JsTreeModel oNodeL0 = new JsTreeModel();

            oNodeL0.id = "L0-0";
            oNodeL0.text = "IT Group List";
            oNodeL0.parent = "#";
            oNodeL0.children = false;


            list.Add(oNodeL0);

            // Level 1
            foreach (var item in lstALocation)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L1-" + item.ID.ToString();
                oModel.text = item.GroupName;
                oModel.parent = "L0-0";
                oModel.children = false;

                list.Add(oModel);

            }




            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [AuthUser]
        [HttpPost]

        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult EditSaveLocationNode(JsITGroupTreeNode node)
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
            bool bResult = true;
            try
            {
                string Level = "";
                Level = node.id.Substring(0, 2);
                int tempLength = node.id.Length;
                string id = node.id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(id);
                TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                // var value1 = Request.Form.Get(keys[]);
                var tnow = System.DateTime.Now.ToUniversalTime();

                JsonResult res = new JsonResult();
                DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                if (Level == "L1") // selected parent level
                {
                    ITGroup aLocation = new ITGroup();
                    aLocation = db.ITGroups.Where(x => x.ID == int_id).FirstOrDefault();
                    aLocation.ClientID = 1;
                    aLocation.GroupName = node.GroupName;
                    aLocation.OPWDV = node.OPWDV;
                    aLocation.DepRate = node.DepRate;
                    aLocation.DepMethod = node.DepMethod;
                    aLocation.ModifiedDate = istDate;
                    aLocation.Modified_Userid = userid;
                    db.Entry(aLocation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }




            }
            catch (Exception ex)
            {
                bResult = false;
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [ValidateJsonAntiForgeryToken]
        public ActionResult GetITGrpValues(DataTableAjaxPostModel filter, string id)
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
            //int totalResultsCount;
            //int filteredResultsCount;
            JsonResult res;
            res = new JsonResult();
            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                //List<string> strlist = new List<string>();
                string[] Arrey;

                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);
                if (Level == "L1") // selected parent level
                {
                    ITGroup alist = new ITGroup();
                    // alist.AGroup_name = db.AGroups.Where(x => x.ID == int_id).FirstOrDefault().AGroupName;
                    alist = db.ITGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;

                }

            }
            catch (Exception ex)
            {
                res.Data = "Error" + ex;
            }


            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [AuthUser]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteITGroup(string id)
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
            List<Assets> alist = new List<Assets>();
            //List<Childlocation> chllocationlist = new List<Childlocation>();
           
            string res = "";
            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);

                ITGroup ITgrp = new ITGroup();
                 //Childlocation chl = new Childlocation();
                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.ITGroupIDID == int_id && x.Companyid == companyid).ToList();
                    

                    if (alist.Count == 0)
                    {
                        ITgrp = db.ITGroups.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(ITgrp).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        res = "Success";
                    }
                    else
                    {
                        res = "Failed";
                    }
                }

            }

            catch (Exception ex)
            {
                //bResult = false;

            }

            // return res;
            return Content(res);
        }

    }
}