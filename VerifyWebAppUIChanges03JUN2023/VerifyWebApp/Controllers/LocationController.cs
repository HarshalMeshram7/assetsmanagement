using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.ViewModel;
using VerifyWebApp.Models;
using Newtonsoft.Json;
using System.IO;
using OfficeOpenXml;
using VerifyWebApp.Filter;
using VerifyWebApp.BusinessLogic;
using System.Web.Helpers;


namespace VerifyWebApp.Controllers
{
    public class LocationController : Controller
    {
        public VerifyDB db = new VerifyDB();

        public  ActionResult TestTree()
        {
            return View();
        }

        // GET: Location
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
        [AuthUser]
        [ValidateJsonXssAttribute]
        public ActionResult SaveLocationNode(JsTreeNode node)
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
                    ALocation aLocation = new ALocation();
                    aLocation.ClientID = 1;
                    aLocation.ALocationName = node.location;
                    aLocation.Companyid = companyid;
                    aLocation.CreatedUserId = userid;
                    aLocation.CreatedDate =istDate;
                    db.ALocations.Add(aLocation);
                    db.SaveChanges();

                }
                if (Level == "L1") // selected parent level
                {
                    BLocation bLocation = new BLocation();
                    bLocation.ClientID = 1;
                    bLocation.ALocID = Convert.ToInt32(id);
                    bLocation.BLocationName = node.location;
                    bLocation.Companyid = companyid;
                    bLocation.CreatedDate = istDate;
                    bLocation.CreatedUserId = userid;
                    db.BLocations.Add(bLocation);
                    db.SaveChanges();

                }

                if (Level == "L2") // selected parent level
                {
                    CLocation cLocation = new CLocation();
                    cLocation.ClientID = 1;
                    int tempID = Convert.ToInt32(id);
                    BLocation bloc = db.BLocations.Where(x => x.ID == tempID && x.Companyid==companyid).FirstOrDefault();

                    if (bloc != null)
                    {
                        cLocation.ALocID = bloc.ALocID;
                        cLocation.BLocID = Convert.ToInt32(id);
                        cLocation.CLocationName = node.location;
                        cLocation.Companyid = companyid;
                        cLocation.CreatedDate = istDate;
                        cLocation.CreatedUserId = userid;
                        db.CLocations.Add(cLocation);
                        db.SaveChanges();
                    }


                }

            }
            catch (Exception ex)
            {
                bResult = false;
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [ValidateJsonAntiForgeryToken]
        [AuthUser]
        public string GetLocationsNew()
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
                //return RedirectToAction("Login", "Login");
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
               // return RedirectToAction("CompanySelection", "Company");
            }

            List<JsTreeModel_New> list = new List<JsTreeModel_New>();

            List<ALocation> lstALocation = new List<ALocation>();
            List<BLocation> lstBLocation = new List<BLocation>();
            List<CLocation> lstCLocation = new List<CLocation>();


            lstALocation = db.ALocations.Where(x => x.Companyid == companyid).ToList();

            //  lstBLocation = db.BLocations.Where(x => x.Companyid == companyid).ToList();

            // lstCLocation = db.CLocations.Where(x => x.Companyid == companyid).ToList();

            JsTreeModel_New oNodeL0 = new JsTreeModel_New();

            oNodeL0.id = "L0-0";
            oNodeL0.text = "Location List";
            oNodeL0.parent = "#";
            //  oNodeL0.children = false;


            list.Add(oNodeL0);

            // Level 1
            List<JsTreeModel_New> lst_Tree_Model_A = new List<JsTreeModel_New>();

            foreach (var item in lstALocation)
            {

                JsTreeModel_New oModel_a = new JsTreeModel_New();

                oModel_a.id = "L1-" + item.ID.ToString();
                oModel_a.text = item.ALocationName;
                oModel_a.parent = "L0-0";
                oModel_a.internal_id = item.ID;
                //  oModel.children = false;

                lstBLocation = db.BLocations.Where(x => x.Companyid == companyid 
                && x.ALocID == oModel_a.internal_id).ToList();

                List<JsTreeModel_New> lst_Tree_Model_B = new List<JsTreeModel_New>();

                foreach (var item_blocation in lstBLocation)
                {

                    JsTreeModel_New oModel_b = new JsTreeModel_New();

                    oModel_b.id = "L2-" + item_blocation.ID.ToString();
                    oModel_b.text = item_blocation.BLocationName;
                    oModel_b.parent = "L1-" + item_blocation.ALocID.ToString();

                    lstCLocation = db.CLocations.Where(x => x.Companyid == companyid
                    && x.BLocID == oModel_b.internal_id
                    && x.ALocID == oModel_a.internal_id
                    ).ToList();

                    List<JsTreeModel_New> lst_Tree_Model_C = new List<JsTreeModel_New>();
                    foreach (var item_clocation in lstCLocation)
                    {

                        JsTreeModel_New oModel_c = new JsTreeModel_New();

                        oModel_c.internal_id = item_clocation.ID;
                        oModel_c.id = "L3-" + item_clocation.ID.ToString();
                        oModel_c.text = item_clocation.CLocationName;
                        oModel_c.parent = "L2-" + item_clocation.BLocID.ToString();

                        lst_Tree_Model_C.Add(oModel_c);
                    }

                    oModel_b.children = lst_Tree_Model_C;

                    lst_Tree_Model_B.Add(oModel_b);
                }
                oModel_a.children = lst_Tree_Model_B;

                lst_Tree_Model_A.Add(oModel_a);
            }
            oNodeL0.children = lst_Tree_Model_A;

            List<JsTreeModel_New> lstNewtemp = new List<JsTreeModel_New>();
            lstNewtemp.Add(oNodeL0);

            string  strJSON = JsonConvert.SerializeObject(lstNewtemp, Formatting.Indented);
            return strJSON;
            //return new JsonResult { Data = lstNewtemp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [ValidateJsonAntiForgeryToken]

        [AuthUser]
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

            List<ALocation> lstALocation = new List<ALocation>();
            List<BLocation> lstBLocation = new List<BLocation>();
            List<CLocation> lstCLocation = new List<CLocation>();


            lstALocation = db.ALocations.Where(x=>x.Companyid==companyid).ToList();

            lstBLocation = db.BLocations.Where(x => x.Companyid == companyid).ToList();

            lstCLocation = db.CLocations.Where(x => x.Companyid == companyid).ToList();

            JsTreeModel oNodeL0 = new JsTreeModel();

            oNodeL0.id = "L0-0";
            oNodeL0.text = "Location List";
            oNodeL0.parent = "#";
          //  oNodeL0.children = false;


            list.Add(oNodeL0);

            // Level 1
            foreach (var item in lstALocation)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L1-" + item.ID.ToString();
                oModel.text = item.ALocationName;
                oModel.parent = "L0-0";
              //  oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstBLocation)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L2-" + item.ID.ToString();
                oModel.text = item.BLocationName;
                oModel.parent = "L1-" + item.ALocID.ToString();
             //   oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstCLocation)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L3-" + item.ID.ToString();
                oModel.text = item.CLocationName;
                oModel.parent = "L2-" + item.BLocID.ToString();
               // oModel.children = false;

                list.Add(oModel);

            }
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [ValidateJsonAntiForgeryToken]
        [AuthUser]
        public JsonResult GetLocations_Sample()
        {
            List<JsTreeModel> list = new List<JsTreeModel>();
            JsTreeModel model = new JsTreeModel();

            model.id = "1";
            model.text = "Location list";
            model.parent = "#";
          //  model.children = false;


            list.Add(model);

            JsTreeModel model_L1 = new JsTreeModel();

            model_L1.id = "2";
            model_L1.text = "Pune";
            model_L1.parent = "1";
          //  model_L1.children = false;


            list.Add(model_L1);

            JsTreeModel model_L2 = new JsTreeModel();

            model_L2.id = "3";
            model_L2.text = "Hyd";
            model_L2.parent = "1";
          //  model_L2.children = false;


            list.Add(model_L2);


            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [AuthUser]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult EditSaveLocationNode(JsTreeNode node)
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
                    ALocation aLocation = new ALocation();
                    aLocation = db.ALocations.Where(x => x.ID == int_id && x.Companyid==companyid).FirstOrDefault();
                    aLocation.ClientID = 1;
                    aLocation.ALocationName = node.location;
                    aLocation.ModifiedDate = istDate;
                    aLocation.Modified_Userid = userid;
                    db.Entry(aLocation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
                if (Level == "L2") // selected parent level
                {
                    BLocation bLocation = new BLocation();
                    bLocation = db.BLocations.Where(x => x.ID == int_id && x.Companyid==companyid).FirstOrDefault();
                    bLocation.ClientID = 1;
                    bLocation.ALocID = bLocation.ALocID;
                    bLocation.BLocationName = node.location;
                    bLocation.Modified_Userid = userid;
                    bLocation.ModifiedDate = istDate;
                    db.Entry(bLocation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }

                if (Level == "L3") // selected parent level
                {
                    CLocation cLocation = new CLocation();
                    cLocation = db.CLocations.Where(x => x.ID == int_id && x.Companyid==companyid).FirstOrDefault();
                    //cLocation.ClientID = 1;
                    //int tempID = Convert.ToInt32(id);
                    //BLocation bloc = db.BLocations.Where(x => x.ID == tempID).FirstOrDefault();

                    //if (bloc != null)
                    //{
                        //cLocation.ALocID = bloc.ALocID;
                        //cLocation.BLocID = Convert.ToInt32(id);
                        cLocation.CLocationName = node.location;
                    cLocation.ModifiedDate = istDate;
                    cLocation.Modified_Userid = userid;
                        db.Entry(cLocation).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    //}


                }

            }
            catch (Exception ex)
            {
                bResult = false;
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /* New Method Added to show list of Assets when clicked
         * on location 
         * Mandar 02 JUL 2020
         */
        [AuthUser]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteLocation(string id)
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
            List<Childlocation> chllocationlist = new List<Childlocation>();
            List<BLocation> blist = new List<BLocation>();
            List<CLocation> clist = new List<CLocation>(); //
            List<Subbatch> batchlist = new List<Subbatch>();
            string res = "";
            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);

                ALocation loc = new ALocation();
                BLocation Bloc = new BLocation();
                CLocation Cloc = new CLocation();

               //Childlocation chl = new Childlocation();
                


                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.LocAID == int_id && x.Companyid == companyid).ToList();
                    chllocationlist = db.childlocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
                    blist = db.BLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
                    clist = db.CLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
                    batchlist = db.SubBatchs.Where(x => x.LocAId== int_id && x.Companyid == companyid).ToList();

                    if (alist.Count==0 && chllocationlist.Count==0 && blist.Count == 0 && clist.Count == 0 && batchlist.Count==0)
                    {
                        loc = db.ALocations.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(loc).State = System.Data.Entity.EntityState.Deleted;
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

                    alist = db.Assetss.Where(x => x.LocBID == int_id && x.Companyid == companyid).ToList();
                    chllocationlist = db.childlocations.Where(x => x.BLocID == int_id && x.Companyid == companyid).ToList();
                   // blist = db.BLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
                    clist = db.CLocations.Where(x => x.BLocID == int_id && x.Companyid == companyid).ToList();
                    batchlist = db.SubBatchs.Where(x => x.LocBId == int_id && x.Companyid == companyid).ToList();

                    if (alist.Count == 0 && chllocationlist.Count == 0 && clist.Count == 0 && batchlist.Count == 0)
                    {
                        Bloc = db.BLocations.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(Bloc).State = System.Data.Entity.EntityState.Deleted;
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

                    alist = db.Assetss.Where(x => x.LocCID == int_id && x.Companyid == companyid).ToList();
                    chllocationlist = db.childlocations.Where(x => x.CLocID == int_id && x.Companyid == companyid).ToList();
                    // blist = db.BLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
                    //clist = db.CLocations.Where(x => x.BLocID == int_id && x.Companyid == companyid).ToList();
                    batchlist = db.SubBatchs.Where(x => x.LocCId == int_id && x.Companyid == companyid).ToList();

                    if (alist.Count == 0 && chllocationlist.Count == 0 && batchlist.Count == 0)
                    {
                        Cloc = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(Cloc).State = System.Data.Entity.EntityState.Deleted;
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


        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [AuthUser]
        public ActionResult GetAssetList(DataTableAjaxPostModel filter, string id)
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
            int totalResultsCount;
            int filteredResultsCount;
            List<Assets> alist = new List<Assets>();
            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);

                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.LocAID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {

                        if (item.DtPutToUse == null)
                        {
                            item.str_DtPutToUse = "";
                        }
                        else
                        {


                            item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                        }
                        if (item.DtPutToUseIT == null)
                        {
                            item.str_DtPutToUseIT = "";
                        }
                        else
                        {

                            item.str_DtPutToUseIT = Convert.ToDateTime(item.DtPutToUseIT).ToString("dd/MM/yyyy");

                        }


                        if (item.BillDate == null)
                        {
                            item.str_BillDate = "";
                        }
                        else
                        {

                            item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                        }

                        if (item.VoucherDate == null)
                        {
                            item.str_VoucherDate = "";
                        }
                        else
                        {

                            item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                        }
                        var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                        if (locid != null)
                        {
                            if (locid.Date == null)
                            {
                                item.str_issuedate = "";
                            }
                            else
                            {
                                item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                            }
                        }
                        if (item.SupplierNo == null)
                        {
                            item.str_suppliername = "";
                        }
                        else
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        }
                        item.str_locationname = db.ALocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ALocationName;

                    }
                }
                if (Level == "L2") // selected parent level
                {
                    BLocation bLocation = new BLocation();
                    var alocid = db.BLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ALocID;
                    //alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {

                        if (item.DtPutToUse == null)
                        {
                            item.str_DtPutToUse = "";
                        }
                        else
                        {


                            item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                        }
                        if (item.DtPutToUseIT == null)
                        {
                            item.str_DtPutToUseIT = "";
                        }
                        else
                        {

                            item.str_DtPutToUseIT = Convert.ToDateTime(item.DtPutToUseIT).ToString("dd/MM/yyyy");

                        }


                        if (item.BillDate == null)
                        {
                            item.str_BillDate = "";
                        }
                        else
                        {

                            item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                        }

                        if (item.VoucherDate == null)
                        {
                            item.str_VoucherDate = "";
                        }
                        else
                        {

                            item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                        }

                        var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                        if (locid != null)
                        {
                            if (locid.Date == null)
                            {
                                item.str_issuedate = "";
                            }
                            else
                            {
                                item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                            }
                        }
                        if (item.SupplierNo == null)
                        {
                            item.str_suppliername = "";
                        }
                        else
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        }
                        item.str_locationname = db.BLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BLocationName;

                    }

                }

                if (Level == "L3") // selected parent level
                {


                    var alocid = db.CLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ALocID;
                    var blocid = db.CLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BLocID;
                   // alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == blocid && x.ID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {

                        if (item.DtPutToUse == null)
                        {
                            item.str_DtPutToUse = "";
                        }
                        else
                        {


                            item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                        }





                        if (item.VoucherDate == null)
                        {
                            item.str_VoucherDate = "";
                        }
                        else
                        {

                            item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                        }
                        var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                        if (locid != null)
                        {
                            if (locid.Date == null)
                            {
                                item.str_issuedate = "";
                            }
                            else
                            {
                                item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                            }
                        }
                        if (item.SupplierNo == null)
                        {
                            item.str_suppliername = "";
                        }
                        else
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        }
                        item.str_locationname = db.CLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CLocationName;

                    }
                }

            }
            catch (Exception ex)
            {
                //bResult = false;
                int bError = 1;
            }

            var lstAssets = alist.Select(x => new  { x.AssetNo,x.AssetIdentificationNo,x.AssetName,x.str_VoucherDate,x.AmountCapitalisedCompany,x.BillNo,x.Qty}).ToList();

            totalResultsCount = lstAssets.Count;

            filteredResultsCount = lstAssets.Count;

            return Json(new
            {
                // this is what datatables wants sending back
                draw = filter.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = lstAssets
            });
        }


        [AuthUser]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult GetAssetData(string id)
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



            // validate token 

            //HttpCookie cookie_csrf = null;
            //cookie_csrf = Request.Cookies[AntiForgeryConfig.CookieName] as HttpCookie;
            //string str_value = "";
            //if (cookie_csrf != null)
            //{
            //    str_value = cookie_csrf.Value;
            //}
            // 



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

                alist = assetRepository.GetAssetData_Location(companyid, Level, int_id);
                /*
                foreach (var item in alist)
                {

                    if (item.DtPutToUse == null)
                    {
                        item.str_DtPutToUse = "";
                    }
                    else
                    {
                        item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
                    }


                    if (item.BillDate == null)
                    {
                        item.str_BillDate = "";
                    }
                    else
                    {

                        item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                    }

                    if (item.VoucherDate == null)
                    {
                        item.str_VoucherDate = "";
                    }
                    else
                    {

                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                    }

                    if (item.SupplierNo == null || item.SupplierNo == 0)
                    {
                        item.str_suppliername = "";
                    }
                    else
                    {

                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;

                    }
                    if (item.LocAID == 0 || item.LocAID == null)
                    {
                        item.str_mainlocation = "";
                    }
                    else
                    {

                        item.str_mainlocation = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault().ALocationName;

                    }
                    if (item.LocBID == 0 || item.LocBID == null)
                    {
                        item.str_sublocation = "";
                    }
                    else
                    {

                        item.str_sublocation = db.BLocations.Where(x => x.ID == item.LocBID && x.Companyid == companyid).FirstOrDefault().BLocationName;

                    }
                    if (item.LocCID == 0 || item.LocCID == null)
                    {
                        item.str_sub_sublocation = "";
                    }
                    else
                    {

                        item.str_sub_sublocation = db.CLocations.Where(x => x.ID == item.LocCID && x.Companyid == companyid).FirstOrDefault().CLocationName;

                    }
                    if (item.AccAccountID == null || item.AccAccountID == 0)
                    {
                        item.str_accumulatedname = "";
                    }
                    else
                    {

                        item.str_accumulatedname = db.Accounts.Where(x => x.ID == item.AccAccountID && x.Companyid == companyid).FirstOrDefault().AccountName;

                    }

                    if (item.AccountID == null || item.AccountID == 0)
                    {
                        item.str_purchaseaccountname = "";
                    }
                    else
                    {

                        item.str_purchaseaccountname = db.Accounts.Where(x => x.ID == item.AccountID && x.Companyid == companyid).FirstOrDefault().AccountName;

                    }
                    if (item.DepAccountId == null || item.DepAccountId == 0)
                    {
                        item.str_depricationname = "";
                    }
                    else
                    {

                        item.str_depricationname = db.Accounts.Where(x => x.ID == item.DepAccountId && x.Companyid == companyid).FirstOrDefault().AccountName;

                    }
                    if (item.ITGroupIDID == null || item.ITGroupIDID == 0)
                    {
                        item.str_it_name = "";
                    }
                    else
                    {

                        item.str_it_name = db.ITGroups.Where(x => x.ID == item.ITGroupIDID && x.Companyid == companyid).FirstOrDefault().GroupName;

                    }
                    if (item.CostCenterAID == null || item.CostCenterAID == 0)
                    {
                        item.str_costcenteraname = "";
                    }
                    else
                    {

                        item.str_costcenteraname = db.ACostCenters.Where(x => x.ID == item.CostCenterAID && x.Companyid == companyid).FirstOrDefault().CCDescription;

                    }
                    if (item.CostCenterBID == null || item.CostCenterBID == 0)
                    {
                        item.str_costcenterbname = "";
                    }
                    else
                    {

                        item.str_costcenterbname = db.BCostCenters.Where(x => x.ID == item.CostCenterBID && x.Companyid == companyid).FirstOrDefault().SCCDescription;

                    }

                    var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                    if (locid != null)
                    {
                        if (locid.Date == null)
                        {
                            item.str_issuedate = "";
                        }
                        else
                        {
                            item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                        }
                    }

                }
                */

                int totalRecords = alist.Count;

                if (!string.IsNullOrEmpty(search) &&
                !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search   
                    // provide search for Asset Sr.No , Asset Name, Asset No
                    alist = alist.Where(p => p.AssetIdentificationNo.ToString().ToLower().Contains(search.ToLower())
                            || p.AssetName.ToLower().Contains(search.ToLower())
                            || p.AssetNo.ToLower().Contains(search.ToLower())
                    ).ToList();

                }

                int recFilter = alist.Count;
                // Apply pagination.   
                alist = alist.Skip(startRec).Take(pageSize).ToList();

                var lstAssets = alist.Select(x => new { x.AssetNo, x.AssetIdentificationNo, x.AssetName, x.str_VoucherDate, x.AmountCapitalisedCompany, x.BillNo, x.Qty }).ToList();
                totalResultsCount = lstAssets.Count;

                filteredResultsCount = lstAssets.Count;

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = recFilter,
                    data = alist
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                int test = 0;//ex.Message("");
            }
            return result;
        }

        [AuthUser]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAssetList_old(DataTableAjaxPostModel filter, string id)
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
            int totalResultsCount;
            int filteredResultsCount;
            List<Assets> alist = new List<Assets>();
            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);
                if (Level == "L1") // selected parent level
                {
                    
                    alist = db.Assetss.Where(x => x.LocAID == int_id && x.DisposalFlag==0 && x.Companyid==companyid).ToList();
                    foreach (var item in alist)
                    {

                        if (item.DtPutToUse == null)
                        {
                            item.str_DtPutToUse = "";
                        }
                        else
                        {


                            item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                        }
                        if (item.DtPutToUseIT == null)
                        {
                            item.str_DtPutToUseIT = "";
                        }
                        else
                        {

                            item.str_DtPutToUseIT = Convert.ToDateTime(item.DtPutToUseIT).ToString("dd/MM/yyyy");

                        }


                        if (item.BillDate == null)
                        {
                            item.str_BillDate = "";
                        }
                        else
                        {

                            item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                        }

                        if (item.VoucherDate == null)
                        {
                            item.str_VoucherDate = "";
                        }
                        else
                        {

                            item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                        }
                        var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                        if (locid != null)
                        {
                            if (locid.Date == null)
                            {
                                item.str_issuedate = "";
                            }
                            else
                            {
                                item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                            }
                        }
                        if (item.SupplierNo == null)
                        {
                            item.str_suppliername = "";
                        }
                        else
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        }
                            item.str_locationname = db.ALocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ALocationName;
                        
                    }
                    }
                if (Level == "L2") // selected parent level
                {
                    BLocation bLocation = new BLocation();
                   var  alocid = db.BLocations.Where(x => x.ID == int_id && x.Companyid==companyid).FirstOrDefault().ALocID;
                    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID==int_id && x.DisposalFlag == 0 && x.Companyid==companyid).ToList();
                    foreach (var item in alist)
                    {

                        if (item.DtPutToUse == null)
                        {
                            item.str_DtPutToUse = "";
                        }
                        else
                        {


                            item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                        }
                        if (item.DtPutToUseIT == null)
                        {
                            item.str_DtPutToUseIT = "";
                        }
                        else
                        {

                            item.str_DtPutToUseIT = Convert.ToDateTime(item.DtPutToUseIT).ToString("dd/MM/yyyy");

                        }


                        if (item.BillDate == null)
                        {
                            item.str_BillDate = "";
                        }
                        else
                        {

                            item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                        }

                        if (item.VoucherDate == null)
                        {
                            item.str_VoucherDate = "";
                        }
                        else
                        {

                            item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                        }

                        var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid==companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                        if (locid != null)
                        {
                            if (locid.Date == null)
                            {
                                item.str_issuedate = "";
                            }
                            else
                            {
                                item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                            }
                        }
                        if (item.SupplierNo == null)
                        {
                            item.str_suppliername = "";
                        }
                        else
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid==companyid).FirstOrDefault().SupplierName;
                        }
                        item.str_locationname = db.BLocations.Where(x => x.ID == int_id && x.Companyid==companyid).FirstOrDefault().BLocationName;

                    }

                }

                if (Level == "L3") // selected parent level
                {


                    var alocid = db.CLocations.Where(x => x.ID == int_id && x.Companyid==companyid).FirstOrDefault().ALocID;
                    var blocid=db.CLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BLocID;
                    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == blocid && x.ID==int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {

                        if (item.DtPutToUse == null)
                        {
                            item.str_DtPutToUse = "";
                        }
                        else
                        {


                            item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                        }
                        


                       

                        if (item.VoucherDate == null)
                        {
                            item.str_VoucherDate = "";
                        }
                        else
                        {

                            item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                        }
                        var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                        if (locid != null)
                        {
                            if (locid.Date == null)
                            {
                                item.str_issuedate = "";
                            }
                            else
                            {
                                item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                            }
                        }
                        if (item.SupplierNo == null)
                        {
                            item.str_suppliername = "";
                        }
                        else
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        }
                        item.str_locationname = db.CLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CLocationName;

                    }
                }

            }
            catch (Exception ex)
            {
                //bResult = false;
                int bError = 1;
            }
            var lstAssets = alist.Select(x => new { x.AssetNo, x.AssetIdentificationNo, x.AssetName, x.str_VoucherDate, x.AmountCapitalisedCompany, x.BillNo, x.Qty }).ToList();

            totalResultsCount = lstAssets.Count;

            filteredResultsCount = lstAssets.Count;

            return Json(new
            {
                // this is what datatables wants sending back
                draw = filter.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = lstAssets
            });
        }

        [HttpPost]
        [AuthUser]

        [ValidateJsonAntiForgeryToken]
        public ActionResult LocationassetExport(string id)
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
            if (id == "0")
            {
                alist = db.Assetss.Where(x => x.Companyid == companyid).ToList();
                foreach (var item in alist)
                {
                    if (item.LocAID == 0 || item.LocAID == null)
                    {
                        item.str_locationname = "";
                    }
                    else
                    {
                        item.str_locationname = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault().ALocationName;
                    }
                }
            }
            else
            {

                int srno = 1;
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);

                //lstins = db.AMCss.ToList();
                if (Level == "L0") // selected parent level
                {

                    alist = db.Assetss.Where(x=> x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {
                        if (item.LocAID == 0 || item.LocAID == null)
                        {
                            item.str_locationname = "";
                        }
                        else
                        {
                            item.str_locationname = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault().ALocationName;
                        }
                    }
                }
                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.LocAID == int_id && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {
                        item.str_locationname = db.ALocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ALocationName;
                    }
                }
                if (Level == "L2") // selected parent level
                {
                    BLocation bLocation = new BLocation();
                    var alocid = db.BLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ALocID;
                    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == int_id && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {
                        item.str_locationname = db.BLocations.Where(x => x.ID == int_id && x.ALocID == alocid && x.Companyid == companyid).FirstOrDefault().BLocationName;
                    }
                }
                if (Level == "L3") // selected parent level
                {


                    var alocid = db.CLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ALocID;
                    var blocid = db.CLocations.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BLocID;
                    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == blocid && x.ID == int_id && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {
                        item.str_locationname = db.CLocations.Where(x => x.ID == int_id && x.ALocID == alocid && x.BLocID == blocid && x.Companyid == companyid).FirstOrDefault().CLocationName;
                    }
                }

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
                    new string[] { "AssetNo", "IdentificationNo", "AssetName", "Voucher Date", "Amount Capitalised", "Bill No",
                    "Qty","Location","Supplier","Srno","Model","Remark","System AssetId","Issue Date"}
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
                foreach (var item in alist)
                {



                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    if (item.VoucherDate == null)
                    {
                        item.str_VoucherDate = "";
                        worksheet.Cells[rowIterator, 4].Value = item.str_VoucherDate;
                    }
                    else
                    {

                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                        worksheet.Cells[rowIterator, 4].Value = item.str_VoucherDate;
                    }

                    worksheet.Cells[rowIterator, 5].Value = item.AmountCapitalisedCompany;
                    worksheet.Cells[rowIterator, 6].Value = item.BillNo;
                    worksheet.Cells[rowIterator, 7].Value = item.Qty;
                    if (item.SupplierNo == null | item.SupplierNo == 0)
                    {
                        item.str_suppliername = "";
                    }
                    else
                    {
                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                    }

                    // item.str_locationname = db.BLocations.Where(x => x.ID == int_id).FirstOrDefault().BLocationName;
                    worksheet.Cells[rowIterator, 8].Value = item.str_locationname;
                    worksheet.Cells[rowIterator, 9].Value = item.str_suppliername;
                    worksheet.Cells[rowIterator, 10].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 11].Value = item.Model;
                    worksheet.Cells[rowIterator, 12].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 13].Value = item.MRRNo;
                    //if (item.DtPutToUse == null)
                    //{
                    //    item.str_DtPutToUse = "";
                    //    worksheet.Cells[rowIterator, 14].Value = item.str_DtPutToUse;
                    //}
                    //else
                    //{


                    //    item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
                    //    worksheet.Cells[rowIterator, 11].Value = item.str_DtPutToUse;

                    //}
                    var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                    if (locid != null)
                    {
                        if (locid.Date == null)
                        {
                            item.str_issuedate = "";
                            worksheet.Cells[rowIterator, 14].Value = item.str_issuedate;
                        }
                        else
                        {
                            item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");
                            worksheet.Cells[rowIterator, 14].Value = item.str_issuedate;

                        }
                    }
                    rowIterator = rowIterator + 1;

                }

                string excelName = "LocationExport.xlsx";

                string handle = Guid.NewGuid().ToString();
                excel.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();

                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = excelName }
                };

            }

        }

        [HttpGet]
        [AuthUser]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }


        [HttpPost]
        public ActionResult UploadLocations()
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);
            APIResponse response = new APIResponse();
            
            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                response.status = "False";
                return Content(JsonConvert.SerializeObject(response), "application/json"); 


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
                response.status = "False";
                return Content(JsonConvert.SerializeObject(response), "application/json");
            }


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
                        List<DTO.LocationDTO> lstLocationList = new List<DTO.LocationDTO>();

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new OfficeOpenXml.ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            const byte col_assetno = 1;
                            const byte col_ALocation = 2;
                            const byte col_BLocation = 3;
                            const byte col_CLocation = 4;

                           

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                string assetno = "";
                                string temp_ALocationName = "";
                                string temp_BLocationName = "";
                                string temp_CLocationName = "";


                                if (workSheet.Cells[rowIterator, col_assetno].Text == "")
                                {
                                    assetno = "";
                                }
                                else
                                {
                                    assetno = workSheet.Cells[rowIterator, col_assetno].Value.ToString();
                                }

                                if (workSheet.Cells[rowIterator, col_ALocation].Text == "")
                                {
                                    temp_ALocationName = "";
                                }
                                else
                                {
                                    temp_ALocationName  = workSheet.Cells[rowIterator, col_ALocation].Value.ToString();
                                }


                                if (workSheet.Cells[rowIterator, col_BLocation].Text == "")
                                {
                                    temp_BLocationName = "";
                                }
                                else
                                {
                                    temp_BLocationName = workSheet.Cells[rowIterator, col_BLocation].Value.ToString();
                                }

                                if (workSheet.Cells[rowIterator, col_CLocation].Text == "")
                                {
                                    temp_CLocationName = "";
                                }
                                else
                                {
                                    temp_CLocationName = workSheet.Cells[rowIterator, col_CLocation].Value.ToString();
                                }

                                if (assetno.Length > 0)
                                {
                                    DTO.LocationDTO location = new DTO.LocationDTO();
                                    location.AssetNo = assetno;

                                    if (temp_ALocationName.Trim().Length > 0)
                                    {
                                        location.ALocationName = temp_ALocationName.Trim();
                                    }else
                                    {
                                        location.ALocationName = "";
                                    }
                                    if (temp_BLocationName.Trim().Length > 0)
                                    {
                                        location.BLocationName = temp_BLocationName.Trim();

                                    } else
                                    {
                                        location.BLocationName = "";
                                    }
                                    if (temp_CLocationName.Trim().Length > 0)
                                    {
                                        location.CLocationName = temp_CLocationName.Trim();
                                    }
                                    else
                                    {
                                        location.CLocationName = "";
                                    }
                                    lstLocationList.Add(location);
                                }

                            }
                        }

                        if (lstLocationList.Count  > 0)
                        {
                            LocationRepository repository = new LocationRepository(db);
                            bool temp_result =  repository.ImportLocations(lstLocationList,companyid,userid,true);
                            if (temp_result)
                            {
                                response.status = "True";
                            }

                        }

                    }
                }
                else
                {
                    response.status = "False";
                    return Content(JsonConvert.SerializeObject(response), "application/json");
                }
            }
            catch (Exception ex)
            {
                response.status = "False";
            }
            return Content(JsonConvert.SerializeObject(response), "application/json");

        }

        [HttpGet]
        [AuthUser]
        public ActionResult ImportLocations()
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
    }
}