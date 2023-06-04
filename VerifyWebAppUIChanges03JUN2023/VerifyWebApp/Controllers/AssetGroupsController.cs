using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.ViewModel;
using VerifyWebApp.Models;

using System.IO;
using OfficeOpenXml;
using VerifyWebApp.Filter;
using VerifyWebApp.BusinessLogic;
using Newtonsoft.Json;

namespace VerifyWebApp.Controllers
{
    public class AssetGroupsController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: AssetGroups

        private AssetGroupsRepository repository = new AssetGroupsRepository(new VerifyDB());

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
        public ActionResult SaveAssetGroupNode(JsAssetGroupTreeNode node)
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

                    string GroupName = "";

                    GroupName = node.AGroupName;


                    JsonResult res = new JsonResult();
                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                    DateTime istDateTime = TimeZoneInfo.ConvertTimeFromUtc(tnow, istZone);
                    string Level = "";
                    Level = node.id.Substring(0, 2);
                    int tempLength = node.id.Length;
                    string id = node.id.Substring(3, tempLength - 3);

                    if (Level == "L0") // selected parent level
                    {


                        AGroup agroup = new AGroup();
                        agroup.ClientID = 1;
                        agroup.AGroupName = node.AGroupName;
                        agroup.NormalRate = node.NormalRate;
                        agroup.AdditionalRate = node.AdditionalRate;
                        agroup.TotalRate = node.TotalRate;
                        agroup.DepMethod = node.DepMethod;
                        agroup.Companyid = companyid;
                        agroup.CreatedUserId = userid;
                        agroup.CreatedDate = istDate;


                        repository.SaveAGroup(agroup);

                        AuditLog auditLog = new AuditLog();

                        auditLog.SaveRecord("AGroupName", "", node.AGroupName);
                        auditLog.SaveRecord("NormalRate", "",node.NormalRate.ToString());
                        auditLog.SaveRecord("AdditionalRate", "",node.AdditionalRate.ToString());
                        auditLog.SaveRecord("TotalRate", "", node.TotalRate.ToString());
                        auditLog.SaveRecord("DepMethod", "", node.DepMethod.ToString());
                        auditLog.SaveRecord("Companyid", "", companyid.ToString());


                        auditLog.InsertLog(userid, companyid, AuditLog.Event_Insert, AuditLog.Record_Type_CompanyLawGroup, db);

                        //var jsonString = JsonConvert.SerializeObject(agroup);



                    }
                    if (Level == "L1") // selected parent level
                    {

                            BGroup bgroup = new BGroup();
                            bgroup.ClientID = 1;
                            bgroup.AGrpID = Convert.ToInt32(id);
                            bgroup.BGroupName = node.AGroupName;
                            bgroup.NormalRate = node.NormalRate;
                            bgroup.AdditionalRate = node.AdditionalRate;
                            bgroup.TotalRate = node.TotalRate;
                            bgroup.DepMethod = node.DepMethod;
                            bgroup.Companyid = companyid;
                            bgroup.CreatedUserId = userid;
                            bgroup.CreatedDate = istDate;

                            repository.SaveBGroup(bgroup);

                            AuditLog auditLog = new AuditLog();

                            auditLog.SaveRecord("BGroupName", "", node.AGroupName);
                            auditLog.SaveRecord("NormalRate", "", node.NormalRate.ToString());
                            auditLog.SaveRecord("AdditionalRate", "", node.AdditionalRate.ToString());
                            auditLog.SaveRecord("TotalRate", "", node.TotalRate.ToString());
                            auditLog.SaveRecord("DepMethod", "", node.DepMethod.ToString());
                            auditLog.SaveRecord("Companyid", "", companyid.ToString());


                            auditLog.InsertLog(userid, companyid, AuditLog.Event_Insert, AuditLog.Record_Type_CompanyLawGroup, db);


                    }

                    if (Level == "L2") // selected parent level
                    {
                        CGroup cgroup = new CGroup();
                        cgroup.ClientID = 1;
                        int tempID = Convert.ToInt32(id);
                        BGroup bgrp = db.BGroups.Where(x => x.ID == tempID && x.Companyid == companyid).FirstOrDefault();

                        if (bgrp != null)
                        {
                            cgroup.AGrpID = bgrp.AGrpID;
                            cgroup.BGrpID = Convert.ToInt32(id);
                            cgroup.CGroupName = node.AGroupName;
                            cgroup.NormalRate = node.NormalRate;
                            cgroup.AdditionalRate = node.AdditionalRate;
                            cgroup.TotalRate = node.TotalRate;
                            cgroup.DepMethod = node.DepMethod;
                            cgroup.Companyid = companyid;
                            cgroup.CreatedUserId = userid;
                            cgroup.CreatedDate = istDate;

                            repository.SaveCGroup(cgroup);

                            AuditLog auditLog = new AuditLog();

                            auditLog.SaveRecord("CGroupName", "", node.AGroupName);
                            auditLog.SaveRecord("NormalRate", "", node.NormalRate.ToString());
                            auditLog.SaveRecord("AdditionalRate", "", node.AdditionalRate.ToString());
                            auditLog.SaveRecord("TotalRate", "", node.TotalRate.ToString());
                            auditLog.SaveRecord("DepMethod", "", node.DepMethod.ToString());
                            auditLog.SaveRecord("Companyid", "", companyid.ToString());


                            auditLog.InsertLog(userid, companyid, AuditLog.Event_Insert, AuditLog.Record_Type_CompanyLawGroup, db);


                            //db.CGroups.Add(cgroup);
                            //db.SaveChanges();
                        }

                    }

                    if (Level == "L3") // selected parent level
                    {
                        DGroup dgroup = new DGroup();
                        dgroup.ClientID = 1;
                        int tempID = Convert.ToInt32(id);
                        
                        CGroup cgrp = db.CGroups.Where(x => x.ID == tempID && x.Companyid == companyid).FirstOrDefault();

                        if (cgrp != null)
                        {
                            dgroup.AGrpID = cgrp.AGrpID;
                            dgroup.BGrpID = cgrp.BGrpID; //Convert.ToInt32(id);
                            dgroup.CGrpID = Convert.ToInt32(id);
                            dgroup.DGroupName = node.AGroupName;
                            dgroup.NormalRate = node.NormalRate;
                            dgroup.AdditionalRate = node.AdditionalRate;
                            dgroup.TotalRate = node.TotalRate;
                            dgroup.DepMethod = node.DepMethod;
                            dgroup.Companyid = companyid;
                            dgroup.CreatedUserId = userid;
                            dgroup.CreatedDate = istDate;

                            repository.SaveDGroup(dgroup);

                            AuditLog auditLog = new AuditLog();

                            auditLog.SaveRecord("DGroupName", "", node.AGroupName);
                            auditLog.SaveRecord("NormalRate", "", node.NormalRate.ToString());
                            auditLog.SaveRecord("AdditionalRate", "", node.AdditionalRate.ToString());
                            auditLog.SaveRecord("TotalRate", "", node.TotalRate.ToString());
                            auditLog.SaveRecord("DepMethod", "", node.DepMethod.ToString());
                            auditLog.SaveRecord("Companyid", "", companyid.ToString());


                            auditLog.InsertLog(userid, companyid, AuditLog.Event_Insert, AuditLog.Record_Type_CompanyLawGroup, db);

                            //db.DGroups.Add(dgroup);
                            //db.SaveChanges();

                        }


                    }


                    db.SaveChanges();

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    bResult = false;
                }
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
            oNodeL0.text = "Asset Groups List";
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


        [AuthUser]
        [HttpPost]

        [ValidateJsonAntiForgeryToken]
        public ActionResult EditSaveAssetGroupNode(JsAssetGroupTreeNode node)
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

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string Level = "";
                    Level = node.id.Substring(0, 2);
                    int tempLength = node.id.Length;
                    string id = node.id.Substring(3, tempLength - 3);
                    int int_id = Convert.ToInt32(id);
                    string Level_DESC = ""; 

                    TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    // var value1 = Request.Form.Get(keys[]);
                    var tnow = System.DateTime.Now.ToUniversalTime();

                    JsonResult res = new JsonResult();
                    DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
                    DateTime istDateTime = TimeZoneInfo.ConvertTimeFromUtc(tnow, istZone);

                    if (Level == "L1") // selected parent level
                    {
                        Level_DESC = "AGroup";

                        AGroup agroup = new AGroup();
                        agroup = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();

                        // AUDIT LOG CODE
                        AuditLog auditLog = new AuditLog();

                        auditLog.SaveRecord("AGroupID", agroup.ID.ToString() , agroup.ID.ToString());
                        auditLog.SaveRecord("AGroupName", agroup.AGroupName, node.AGroupName);
                        auditLog.SaveRecord("NormalRate", agroup.NormalRate.ToString(), node.NormalRate.ToString());
                        auditLog.SaveRecord("AdditionalRate", agroup.AdditionalRate.ToString() , node.AdditionalRate.ToString());
                        auditLog.SaveRecord("TotalRate", agroup.TotalRate.ToString(), node.TotalRate.ToString());
                        auditLog.SaveRecord("DepMethod", agroup.DepMethod.ToString(), node.DepMethod.ToString());
                        auditLog.SaveRecord("Companyid", agroup.Companyid.ToString(), companyid.ToString());

                        auditLog.InsertLog(userid, companyid, AuditLog.Event_Update, AuditLog.Record_Type_CompanyLawGroup, db);

                        agroup.ClientID = 1;
                        agroup.AGroupName = node.AGroupName;
                        agroup.NormalRate = node.NormalRate;
                        agroup.AdditionalRate = node.AdditionalRate;
                        agroup.TotalRate = node.TotalRate;
                        agroup.DepMethod = node.DepMethod;
                        agroup.ModifiedDate = istDate;
                        agroup.Modified_Userid = userid;
                        db.Entry(agroup).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                       

                    }
                    if (Level == "L2") // selected parent level
                    {
                        Level_DESC = "BGroup";

                        BGroup bgroup = new BGroup();
                        bgroup = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();


                        // AUDIT LOG CODE
                        AuditLog auditLog = new AuditLog();

                        auditLog.SaveRecord("AGroupID", bgroup.ID.ToString(), bgroup.ID.ToString());
                        auditLog.SaveRecord("AGroupName", bgroup.BGroupName, node.AGroupName);
                        auditLog.SaveRecord("NormalRate", bgroup.NormalRate.ToString(), node.NormalRate.ToString());
                        auditLog.SaveRecord("AdditionalRate", bgroup.AdditionalRate.ToString(), node.AdditionalRate.ToString());
                        auditLog.SaveRecord("TotalRate", bgroup.TotalRate.ToString(), node.TotalRate.ToString());
                        auditLog.SaveRecord("DepMethod", bgroup.DepMethod.ToString(), node.DepMethod.ToString());
                        auditLog.SaveRecord("Companyid", bgroup.Companyid.ToString(), companyid.ToString());

                        auditLog.InsertLog(userid, companyid, AuditLog.Event_Update, AuditLog.Record_Type_CompanyLawGroup, db);

                        bgroup.ClientID = 1;
                        //bgroup.AGrpID = bgroup.AGrpID;
                        bgroup.BGroupName = node.AGroupName;
                        bgroup.NormalRate = node.NormalRate;
                        bgroup.AdditionalRate = node.AdditionalRate;
                        bgroup.TotalRate = node.TotalRate;
                        bgroup.DepMethod = node.DepMethod;
                        bgroup.ModifiedDate = istDate;
                        bgroup.Modified_Userid = userid;
                        db.Entry(bgroup).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();


                    

                    }

                    if (Level == "L3") // selected parent level
                    {
                        Level_DESC = "CGroup";

                        CGroup cgroup = new CGroup();
                        cgroup = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                        cgroup.ClientID = 1;
                        int tempID = Convert.ToInt32(id);
                        BGroup bloc = db.BGroups.Where(x => x.ID == tempID && x.Companyid == companyid).FirstOrDefault();

                        //if (bloc != null)
                        //{
                        //cgroup.AGrpID = cgroup.AGrpID;
                        //cgroup.BGrpID = cgroup.BGrpID;


                        // AUDIT LOG CODE
                        AuditLog auditLog = new AuditLog();

                        auditLog.SaveRecord("AGroupID", cgroup.ID.ToString(), cgroup.ID.ToString());
                        auditLog.SaveRecord("AGroupName", cgroup.CGroupName, node.AGroupName);
                        auditLog.SaveRecord("NormalRate", cgroup.NormalRate.ToString(), node.NormalRate.ToString());
                        auditLog.SaveRecord("AdditionalRate", cgroup.AdditionalRate.ToString(), node.AdditionalRate.ToString());
                        auditLog.SaveRecord("TotalRate", cgroup.TotalRate.ToString(), node.TotalRate.ToString());
                        auditLog.SaveRecord("DepMethod", cgroup.DepMethod.ToString(), node.DepMethod.ToString());
                        auditLog.SaveRecord("Companyid", cgroup.Companyid.ToString(), companyid.ToString());

                        auditLog.InsertLog(userid, companyid, AuditLog.Event_Update, AuditLog.Record_Type_CompanyLawGroup, db);

                        cgroup.CGroupName = node.AGroupName;
                        cgroup.NormalRate = node.NormalRate;
                        cgroup.AdditionalRate = node.AdditionalRate;
                        cgroup.TotalRate = node.TotalRate;
                        cgroup.DepMethod = node.DepMethod;
                        cgroup.ModifiedDate = istDate;
                        cgroup.Modified_Userid = userid;
                        db.Entry(cgroup).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        //}



                    }

                    if (Level == "L4") // selected parent level
                    {
                        Level_DESC = "DGroup";

                        DGroup dgroup = new DGroup();
                        dgroup = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                        dgroup.ClientID = 1;
                        int tempID = Convert.ToInt32(id);
                        // BGroup bloc = db.BGroups.Where(x => x.ID == tempID).FirstOrDefault();
                        CGroup cloc = db.CGroups.Where(x => x.ID == tempID && x.Companyid == companyid).FirstOrDefault();
                        //if (cloc != null)
                        //{
                        //dgroup.AGrpID = cloc.AGrpID;
                        //dgroup.BGrpID = cloc.BGrpID;//Convert.ToInt32(id);
                        //dgroup.CGrpID = Convert.ToInt32(id);
                        dgroup.DGroupName = node.AGroupName;
                        dgroup.NormalRate = node.NormalRate;
                        dgroup.AdditionalRate = node.AdditionalRate;
                        dgroup.TotalRate = node.TotalRate;
                        dgroup.DepMethod = node.DepMethod;
                        dgroup.ModifiedDate = istDate;
                        dgroup.Modified_Userid = userid;
                        db.Entry(dgroup).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        //}

                        AuditLog auditLog = new AuditLog();

                        auditLog.SaveRecord("DGroupName", "", node.AGroupName);
                        auditLog.SaveRecord("NormalRate", "", node.NormalRate.ToString());
                        auditLog.SaveRecord("AdditionalRate", "", node.AdditionalRate.ToString());
                        auditLog.SaveRecord("TotalRate", "", node.TotalRate.ToString());
                        auditLog.SaveRecord("DepMethod", "", node.DepMethod.ToString());
                        auditLog.SaveRecord("Companyid", "", companyid.ToString());

                        auditLog.InsertLog(userid, companyid, AuditLog.Event_Update, AuditLog.Record_Type_CompanyLawGroup, db);


                    }

                    db.SaveChanges();

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    bResult = false;
                }
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [AuthUser]
        [HttpPost]

        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteGroups(string id)
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
            // List<Childlocation> chllocationlist = new List<Childlocation>();
            List<BGroup> blist = new List<BGroup>();
            List<CGroup> clist = new List<CGroup>(); //
            List<DGroup> dlist = new List<DGroup>();
            string res = "";
            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);

                AGroup Aobj = new AGroup();
                BGroup Bobj = new BGroup();
                CGroup Cobj = new CGroup();
                DGroup Dobj = new DGroup();

                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.AGroupID == int_id && x.Companyid == companyid).ToList();
                    blist = db.BGroups.Where(x => x.AGrpID == int_id && x.Companyid == companyid).ToList();
                    clist = db.CGroups.Where(x => x.AGrpID == int_id && x.Companyid == companyid).ToList();
                    dlist = db.DGroups.Where(x => x.AGrpID == int_id && x.Companyid == companyid).ToList();

                    if (alist.Count == 0 && blist.Count == 0 && blist.Count == 0 && clist.Count == 0 && dlist.Count == 0)
                    {
                        Aobj = db.AGroups.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(Aobj).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        res = "Success";
                    }
                    else
                    {
                        res = "Failed";
                    }
                }


                if (Level == "L2") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.BGroupID == int_id && x.Companyid == companyid).ToList();
                    clist = db.CGroups.Where(x => x.BGrpID == int_id && x.Companyid == companyid).ToList();
                    dlist = db.DGroups.Where(x => x.BGrpID == int_id && x.Companyid == companyid).ToList();


                    if (alist.Count == 0 && clist.Count == 0 && clist.Count == 0 && dlist.Count == 0)
                    {
                        Bobj = db.BGroups.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(Bobj).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        res = "Success";
                    }
                    else
                    {
                        res = "Failed";
                    }
                }

                if (Level == "L3") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.CGroupID == int_id && x.Companyid == companyid).ToList();
                    dlist = db.DGroups.Where(x => x.CGrpID == int_id && x.Companyid == companyid).ToList();

                    if (alist.Count == 0 && dlist.Count == 0)
                    {
                        Cobj = db.CGroups.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(Cobj).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        res = "Success";
                    }
                    else
                    {
                        res = "Failed";
                    }
                }

                if (Level == "L4") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.DGroupID == int_id && x.Companyid == companyid).ToList();
                    //  dlist = db.DGroups.Where(x => x.CGrpID == int_id && x.Companyid == companyid).ToList();

                    if (alist.Count == 0)
                    {
                        Dobj = db.DGroups.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(Dobj).State = System.Data.Entity.EntityState.Deleted;
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

        public ActionResult GetAssetGrpValues_old(DataTableAjaxPostModel filter, string id)
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
                    AGroup alist = new AGroup();
                    alist.AGroup_name = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGroupName;
                    alist = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;

                }
                if (Level == "L2") // selected parent level
                {
                    BGroup alist = new BGroup();
                    var agrpid = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    alist.BGroup_name = db.BGroups.Where(x => x.AGrpID == agrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGroupName;
                    alist = db.BGroups.Where(x => x.AGrpID == agrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();

                    res.Data = alist;

                }

                if (Level == "L3") // selected parent level
                {
                    CGroup alist = new CGroup();

                    var aGrpid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bGrpid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    alist.CGroup_name = db.CGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGroupName;
                    alist = db.CGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;
                }

                if (Level == "L4") // selected parent level
                {
                    DGroup alist = new DGroup();

                    var aGrpid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bGrpid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    // alist.CGroup_name = db.CGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id).FirstOrDefault().CGroupName;
                    alist = db.DGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;
                }

            }
            catch (Exception ex)
            {
                res.Data = "Error" + ex;
            }


            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAssetGrpValues(DataTableAjaxPostModel filter, string id)
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
                    AGroup alist = new AGroup();

                    alist.AGroup_name = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGroupName;
                    alist = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;

                }
                if (Level == "L2") // selected parent level
                {
                    BGroup alist = new BGroup();
                    var agrpid = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    alist.BGroup_name = db.BGroups.Where(x => x.AGrpID == agrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGroupName;
                    alist = db.BGroups.Where(x => x.AGrpID == agrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();

                    res.Data = alist;

                }

                if (Level == "L3") // selected parent level
                {
                    CGroup alist = new CGroup();

                    var aGrpid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bGrpid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    alist.CGroup_name = db.CGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGroupName;
                    alist = db.CGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;
                }

                if (Level == "L4") // selected parent level
                {
                    DGroup alist = new DGroup();

                    var aGrpid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bGrpid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    // alist.CGroup_name = db.CGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id).FirstOrDefault().CGroupName;
                    alist = db.DGroups.Where(x => x.AGrpID == aGrpid && x.BGrpID == bGrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;
                }

            }
            catch (Exception ex)
            {
                res.Data = "Error" + ex;
            }


            return Json(res, JsonRequestBehavior.AllowGet);

        }
    }
}