using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.ViewModel;
using VerifyWebApp.Models;

using System.IO;
using VerifyWebApp.Filter;
using VerifyWebApp.BusinessLogic;
using NLog;
using Newtonsoft.Json;

namespace VerifyWebApp.Controllers
{
    public class ITGroupController : Controller
    {
        public VerifyDB db = new VerifyDB();

        private AssetGroupsRepository repository = new AssetGroupsRepository(new VerifyDB());

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    // var value1 = Request.Form.Get(keys[]);
                    var tnow = System.DateTime.Now.ToUniversalTime();

                    JsonResult res = new JsonResult();
                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

                   // string GroupName = "";



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

                        //repository.SaveGroup(aLocation);

                        //AuditLog auditLog = new AuditLog();

                        //auditLog.SaveRecord("GroupName", "", node.GroupName);
                        //auditLog.SaveRecord("OPWDV", "", node.OPWDV.ToString());
                        //auditLog.SaveRecord("DepRate", "", node.DepRate.ToString());
                        //auditLog.SaveRecord("DepMethod", "", node.DepMethod.ToString());
                        //auditLog.SaveRecord("Companyid", "", companyid.ToString());


                        //auditLog.InsertLog(userid, companyid, AuditLog.Event_Insert, AuditLog.Record_Type_CompanyLawGroup, db);

                        //var jsonString = JsonConvert.SerializeObject(agroup);

                        db.ITGroups.Add(aLocation);
                        db.SaveChanges();
                       transaction.Commit();

                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    bResult = false;
                }
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult GetAssetData(string id, string searchby = "", string searchstring = "")
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
            int totalResultsCount;
            int filteredResultsCount;

            List<Assets> alist = new List<Assets>();

            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

            JsonResult result = new JsonResult();

            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);


                AssetRepository assetRepository = new AssetRepository();
                if (search.Length > 0)
                {
                    searchstring = search;
                }


                if (searchstring.Length > 0)
                {
                    alist = assetRepository.GetAssetDataSearchIT(companyid, Level, int_id, startRec, pageSize, searchby, searchstring);

                    int totalRecords = db.Assetss.Count(x => x.Companyid == companyid && x.ITGroupIDID == int_id );
                    int recFilter = totalRecords;

                    var lstAssets = alist.Select(x => new
                    {
                        x.AssetNo,
                        x.AssetIdentificationNo,
                        x.AssetName,
                        x.str_VoucherDate,
                        x.AmountCapitalisedCompany,
                        x.BillNo,
                        x.Qty
                    }).ToList();

                    filteredResultsCount = lstAssets.Count;

                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = totalRecords,
                        recordsFiltered = recFilter,
                        data = alist,

                    }, JsonRequestBehavior.AllowGet);

                    result.MaxJsonLength = int.MaxValue;
                }
                else
                {
                    alist = assetRepository.GetAssetDataIT(companyid, Level, int_id, startRec, pageSize);

                    int totalRecords = db.Assetss.Count(x => x.Companyid == companyid);
                    int recFilter = totalRecords;


                    var lstAssets = alist.Select(x => new
                    {
                        x.AssetNo,
                        x.AssetIdentificationNo,
                        x.AssetName,
                        x.str_VoucherDate,
                        x.AmountCapitalisedCompany,
                        x.BillNo,
                        x.Qty
                    }).ToList();

                    filteredResultsCount = lstAssets.Count;

                    result = this.Json(new
                    {
                        draw = Convert.ToInt32(draw),
                        recordsTotal = totalRecords,
                        recordsFiltered = totalRecords,
                        data = alist,

                    }, JsonRequestBehavior.AllowGet);

                    result.MaxJsonLength = int.MaxValue;
                }



            }
            catch (Exception ex)
            {

                logger.Log(LogLevel.Error, ex);
            }
            return result;
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
        public ActionResult GetAssetGroup()
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
                ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            List<JsTreeModel> list = new List<JsTreeModel>();

            List<AGroup> lstAGroup = new List<AGroup>();
            List<BGroup> lstBGroup = new List<BGroup>();
            List<CGroup> lstCGroup = new List<CGroup>();
            List<DGroup> lstDGroup = new List<DGroup>();

            lstAGroup = db.AGroups.Where(x => x.Companyid == companyid).ToList();

            lstBGroup = db.BGroups.Where(x => x.Companyid == companyid).ToList();

            lstCGroup = db.CGroups.Where(x => x.Companyid == companyid).ToList();

            lstDGroup = db.DGroups.Where(x => x.Companyid == companyid).ToList();

            JsTreeModel oNodeL0 = new JsTreeModel();

            oNodeL0.id = "L0-0";
            oNodeL0.text = "ITGroups List";
            oNodeL0.parent = "#";
            oNodeL0.children = false;


            list.Add(oNodeL0);

            //  Level 1
            foreach (var item in lstAGroup)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L1-" + item.ID.ToString();
                oModel.text = item.AGroupName;
                oModel.parent = "L0-0";
                oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstBGroup)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L2-" + item.ID.ToString();
                oModel.text = item.BGroupName;
                oModel.parent = "L1-" + item.AGrpID.ToString();
                oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstCGroup)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L3-" + item.ID.ToString();
                oModel.text = item.CGroupName;
                oModel.parent = "L2-" + item.BGrpID.ToString();
                oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstDGroup)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L4-" + item.ID.ToString();
                oModel.text = item.DGroupName;
                oModel.parent = "L3-" + item.CGrpID.ToString();
                oModel.children = false;

                list.Add(oModel);

            }

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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