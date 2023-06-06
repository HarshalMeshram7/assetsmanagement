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

namespace VerifyWebApp.Controllers
{
    public class CostCenterController : Controller
    {
        public VerifyDB db = new VerifyDB();

        // GET: CostCenter
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
        public ActionResult SaveCostCenterNode(JsCostCenterTreeNode node)
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
                    ACostCenter acostcenter = new ACostCenter();
                    acostcenter.ClientID = 1;
                    acostcenter.CCCode = node.CCCode;
                    acostcenter.CCDescription = node.CCDescription;
                    acostcenter.Companyid = companyid;
                    acostcenter.CreatedUserId = userid;
                    acostcenter.CreatedDate = istDate;
                    db.ACostCenters.Add(acostcenter);
                    db.SaveChanges();

                }
                if (Level == "L1") // selected parent level
                {
                    BCostCenter bcostcenter = new BCostCenter();
                    bcostcenter.ClientID = 1;
                    bcostcenter.CCID = Convert.ToInt32(id);
                    bcostcenter.SCCCode = node.CCCode;
                    bcostcenter.SCCDescription = node.CCDescription;
                    bcostcenter.Companyid = companyid;
                    bcostcenter.CreatedUserId = userid;
                    bcostcenter.CreatedDate = istDate;
                    db.BCostCenters.Add(bcostcenter);
                    db.SaveChanges();

                }

                //if (Level == "L2") // selected parent level
                //{
                //    CLocation cLocation = new CLocation();
                //    cLocation.ClientID = 1;
                //    int tempID = Convert.ToInt32(id);
                //    BLocation bloc = db.BLocations.Where(x => x.ID == tempID).FirstOrDefault();

                //    if (bloc != null)
                //    {
                //        cLocation.ALocID = bloc.ALocID;
                //        cLocation.BLocID = Convert.ToInt32(id);
                //        cLocation.CLocationName = node.location;

                //        db.CLocations.Add(cLocation);
                //        db.SaveChanges();
                //    }


                //}

            }
            catch (Exception ex)
            {
                bResult = false;
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [AuthUser]
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public ActionResult GetCostCenter()
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

            List<ACostCenter> lstACostCenter = new List<ACostCenter>();
            List<BCostCenter> lstBCostCenter = new List<BCostCenter>();
            //List<CLocation> lstCLocation = new List<CLocation>();


            lstACostCenter = db.ACostCenters.Where(x => x.Companyid == companyid).ToList();

            lstBCostCenter = db.BCostCenters.Where(x => x.Companyid == companyid).ToList();

            //lstCLocation = db.CLocations.ToList();

            JsTreeModel oNodeL0 = new JsTreeModel();

            oNodeL0.id = "L0-0";
            oNodeL0.text = "Cost Center List";
            oNodeL0.parent = "#";
            oNodeL0.children = false;


            list.Add(oNodeL0);

            // Level 1
            foreach (var item in lstACostCenter)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L1-" + item.ID.ToString();
                oModel.text = item.CCDescription;
                oModel.parent = "L0-0";
                oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstBCostCenter)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L2-" + item.ID.ToString();
                oModel.text = item.SCCDescription;
                oModel.parent = "L1-" + item.CCID.ToString();
                oModel.children = false;

                list.Add(oModel);

            }

            //foreach (var item in lstCLocation)
            //{

            //    JsTreeModel oModel = new JsTreeModel();

            //    oModel.id = "L3-" + item.ID.ToString();
            //    oModel.text = item.CLocationName;
            //    oModel.parent = "L2-" + item.BLocID.ToString();
            //    oModel.children = false;

            //    list.Add(oModel);

            //}
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetLocations_Sample()
        {
            List<JsTreeModel> list = new List<JsTreeModel>();
            JsTreeModel model = new JsTreeModel();

            model.id = "1";
            model.text = "Cost Center list";
            model.parent = "#";
            model.children = false;


            list.Add(model);

            JsTreeModel model_L1 = new JsTreeModel();

            model_L1.id = "2";
            model_L1.text = "Cost Center 1";
            model_L1.parent = "1";
            model_L1.children = false;


            list.Add(model_L1);

            JsTreeModel model_L2 = new JsTreeModel();

            model_L2.id = "3";
            model_L2.text = "Sub cost Center 1";
            model_L2.parent = "1";
            model_L2.children = false;


            list.Add(model_L2);

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [AuthUser]
        [HttpPost]

        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult EditSaveCostCenterNode(JsCostCenterTreeNode node)
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
                    ACostCenter aCostCenter = new ACostCenter();
                    aCostCenter = db.ACostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    aCostCenter.ClientID = 1;
                    aCostCenter.CCDescription = node.CCDescription;
                    aCostCenter.CCCode = node.CCCode;
                    aCostCenter.ModifiedDate = istDate;
                    aCostCenter.Modified_Userid = userid;
                    db.Entry(aCostCenter).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
                if (Level == "L2") // selected parent level
                {
                    BCostCenter bCostCenter = new BCostCenter();
                    bCostCenter = db.BCostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    bCostCenter.ClientID = 1;
                    // bCostCenter.CCID = Convert.ToInt32(id);
                    bCostCenter.SCCDescription = node.CCDescription;
                    bCostCenter.SCCCode = node.CCCode;
                    bCostCenter.ModifiedDate = istDate;
                    bCostCenter.Modified_Userid = userid;
                    db.Entry(bCostCenter).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }

                //if (Level == "L3") // selected parent level
                //{
                //    CLocation cLocation = new CLocation();
                //    cLocation = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault();
                //    cLocation.ClientID = 1;
                //    int tempID = Convert.ToInt32(id);
                //    BLocation bloc = db.BLocations.Where(x => x.ID == tempID).FirstOrDefault();

                //    if (bloc != null)
                //    {
                //        cLocation.ALocID = bloc.ALocID;
                //        cLocation.BLocID = Convert.ToInt32(id);
                //        cLocation.CLocationName = node.location;

                //        db.Entry(cLocation).State = System.Data.Entity.EntityState.Modified;
                //        db.SaveChanges();
                //    }


                //}

            }
            catch (Exception ex)
            {
                bResult = false;
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

                    alist = db.Assetss.Where(x => x.CostCenterAID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
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
                        if (item.SupplierNo == null)
                        {
                            item.str_suppliername = "";
                        }
                        else
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        }

                       
                        item.str_CCDescription = db.ACostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCDescription;

                    }
                }
                if (Level == "L2") // selected parent level
                {
                    BCostCenter bCostCenter = new BCostCenter();
                    var aCCId = db.BCostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCID;
                    alist = db.Assetss.Where(x => x.CostCenterAID == aCCId && x.ID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
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


                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        item.str_BCCDescription = db.BCostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().SCCDescription;

                    }

                }

                //if (Level == "L3") // selected parent level
                //{


                //    var alocid = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().ALocID;
                //    var blocid=db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().BLocID;
                //    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == blocid && x.ID==int_id).ToList();
                //    foreach (var item in alist)
                //    {

                //        if (item.DtPutToUse == null)
                //        {
                //            item.str_DtPutToUse = "";
                //        }
                //        else
                //        {


                //            item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                //        }





                //        if (item.VoucherDate == null)
                //        {
                //            item.str_VoucherDate = "";
                //        }
                //        else
                //        {

                //            item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                //        }


                //        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                //        item.str_locationname = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().CLocationName;

                //    }
                //}
                
            }
             
            catch (Exception ex)
            {
                //bResult = false;
            }
            totalResultsCount = alist.Count;

            filteredResultsCount = alist.Count;
            var lstAssets = alist.Select(x => new { x.AssetNo, x.AssetIdentificationNo, x.AssetName, x.str_VoucherDate, x.AmountCapitalisedCompany, x.BillNo, x.Qty }).ToList();
            return Json(new
            {
                // this is what datatables wants sending back
                draw = filter.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = lstAssets
                //data = alist
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

                alist = assetRepository.GetAssetData_Costcenter(companyid, Level, int_id);
              
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
        [ValidateJsonAntiForgeryToken]
        public ActionResult Deletecostcenter(string id)
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
            List<Childcostcenter> chlcostcenterlist = new List<Childcostcenter>();
            List<BCostCenter> blist = new List<BCostCenter>();
            
            string res = "";
            try
            {
                string Level = "";
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);

                ACostCenter ACost = new ACostCenter();
                BCostCenter BCost = new BCostCenter();
               

                //Childlocation chl = new Childlocation();



                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.CostCenterAID == int_id && x.Companyid == companyid).ToList();
                    chlcostcenterlist = db.childcostcenters.Where(x => x.AcostcenterID == int_id && x.Companyid == companyid).ToList();
                    blist = db.BCostCenters.Where(x => x.CCID == int_id && x.Companyid == companyid).ToList();
                    //clist = db.CLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
                   // batchlist = db.SubBatchs.Where(x => x.LocAId == int_id && x.Companyid == companyid).ToList();

                    if (alist.Count == 0 && chlcostcenterlist.Count == 0 && blist.Count == 0)
                    {
                        ACost = db.ACostCenters.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(ACost).State = System.Data.Entity.EntityState.Deleted;
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

                    alist = db.Assetss.Where(x => x.CostCenterBID == int_id && x.Companyid == companyid).ToList();
                    chlcostcenterlist = db.childcostcenters.Where(x => x.BcostcenterID == int_id && x.Companyid == companyid).ToList();
                    // blist = db.BLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
                   // clist = db.CLocations.Where(x => x.BLocID == int_id && x.Companyid == companyid).ToList();
                    //batchlist = db.SubBatchs.Where(x => x.LocBId == int_id && x.Companyid == companyid).ToList();

                    if (alist.Count == 0 && chlcostcenterlist.Count == 0)
                    {
                        BCost = db.BCostCenters.Where(x => x.ID == int_id).FirstOrDefault();
                        db.Entry(BCost).State = System.Data.Entity.EntityState.Deleted;
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

        [AuthUser]
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult CostCenterassetExport(string id)
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
                    if (item.CostCenterAID == 0 || item.CostCenterAID == null)
                    {
                        item.str_CCDescription = "";
                    }
                    else
                    {
                        item.str_CCDescription = db.ACostCenters.Where(x => x.ID == item.CostCenterAID && x.Companyid == companyid).FirstOrDefault().CCDescription;
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

                if (Level == "L0")
                {
                    alist = db.Assetss.Where(x => x.Companyid == companyid ).ToList();
                    foreach (var item in alist)
                    {
                        if (item.CostCenterAID == 0 || item.CostCenterAID == null)
                        {
                            item.str_CCDescription = "";
                        }
                        else
                        {
                            item.str_CCDescription = db.ACostCenters.Where(x => x.ID == item.CostCenterAID && x.Companyid == companyid).FirstOrDefault().CCDescription;
                        }
                    }

                }

                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.CostCenterAID == int_id && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {

                        item.str_CCDescription = db.ACostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCDescription;
                    }
                }
                if (Level == "L2") // selected parent level
                {
                    BCostCenter bCostCenter = new BCostCenter();
                    var accid = db.BCostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCID;
                    alist = db.Assetss.Where(x => x.CostCenterAID == accid && x.CostCenterBID == int_id && x.Companyid == companyid).ToList();
                    foreach (var item in alist)
                    {
                        item.str_CCDescription = db.BCostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCDescription;
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
                    "Qty","Costcenter","Supplier","Srno","Model","Remark","System AssetId"}
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
                    if (item.SupplierNo == null||item.SupplierNo == 0  )
                    {
                        item.str_suppliername = "";

                    }
                    else
                    {
                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                        
                    }
                    // item.str_CCDescription = db.ACostCenters.Where(x => x.ID == int_id).FirstOrDefault().CCDescription;
                    worksheet.Cells[rowIterator, 8].Value = item.str_CCDescription;
                    worksheet.Cells[rowIterator, 9].Value = item.str_suppliername;
                    worksheet.Cells[rowIterator, 10].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 8].Value = item.Model;
                    worksheet.Cells[rowIterator, 9].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 10].Value = item.MRRNo;
                    //if (item.DtPutToUse == null)
                    //{
                    //    item.str_DtPutToUse = "";
                    //    worksheet.Cells[rowIterator, 11].Value = item.str_DtPutToUse;
                    //}
                    //else
                    //{


                    //    item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
                    //    worksheet.Cells[rowIterator, 11].Value = item.str_DtPutToUse;

                    //}
                    rowIterator = rowIterator + 1;


                }

                string excelName = "Costcenterexport.xlsx";

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

        //public byte[] generatecostcenterexcel(string id, int companyid)
        //{

        //    List<Assets> alist = new List<Assets>();

        //    if (id == "0")
        //    {
        //        alist = db.Assetss.Where(x => x.Companyid == companyid).ToList();
        //    }
        //    else
        //    {
        //        int srno = 1;
        //    string Level = "";
        //    Level = id.Substring(0, 2);
        //    int tempLength = id.Length;
        //    string strr_id = id.Substring(3, tempLength - 3);
        //    int int_id = Convert.ToInt32(strr_id);

        //    //lstins = db.AMCss.ToList();

        //        if (Level == "L1") // selected parent level
        //        {

        //            alist = db.Assetss.Where(x => x.CostCenterAID == int_id && x.Companyid == companyid).ToList();
        //            foreach (var item in alist)
        //            {
        //                item.str_CCDescription = db.ACostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCDescription;
        //            }
        //        }
        //        if (Level == "L2") // selected parent level
        //        {
        //            BCostCenter bCostCenter = new BCostCenter();
        //            var accid = db.BCostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCID;
        //            alist = db.Assetss.Where(x => x.CostCenterAID == accid && x.CostCenterBID == int_id && x.Companyid == companyid).ToList();
        //        }
        //    }
        //    //if (Level == "L3") // selected parent level
        //    //{


        //    //    var alocid = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().ALocID;
        //    //    var blocid = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().BLocID;
        //    //    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == blocid && x.ID == int_id).ToList();
        //    //}


        //    var memoryStream = new MemoryStream();
        //    byte[] data;
        //    //ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage excel = new ExcelPackage(memoryStream))
        //    {

        //        excel.Workbook.Worksheets.Add("Worksheet1");
        //        excel.Workbook.Worksheets.Add("Worksheet2");

        //        var headerRow = new List<string[]>()
        //          {
        //            new string[] { "AssetNo", "IdentificationNo", "AssetName", "Voucher Date", "Amount Capitalised", "Bill No",
        //            "Qty","Location","Supplier","Srno","Model","Remark","System AssetId"}
        //          };


        //        // Determine the header range (e.g. A1:D1)
        //        string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


        //        // Target a worksheet
        //        var worksheet = excel.Workbook.Worksheets["Worksheet1"];
        //        var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
        //        var currentSheet = excel.Workbook.Worksheets;
        //        // Popular header row data
        //        worksheet.Cells[headerRange].LoadFromArrays(headerRow);
        //        int rowIterator = 2;
        //        foreach (var item in alist)
        //        {



        //            worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationNo;
        //            worksheet.Cells[rowIterator, 3].Value = item.AssetName;
        //            if (item.VoucherDate == null)
        //            {
        //                item.str_VoucherDate = "";
        //                worksheet.Cells[rowIterator, 4].Value = item.str_VoucherDate;
        //            }
        //            else
        //            {

        //                item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
        //                worksheet.Cells[rowIterator, 4].Value = item.str_VoucherDate;
        //            }

        //            worksheet.Cells[rowIterator, 5].Value = item.AmountCapitalisedCompany;
        //            worksheet.Cells[rowIterator, 6].Value = item.BillNo;
        //            worksheet.Cells[rowIterator, 7].Value = item.Qty;
        //            if (item.SupplierNo != 0 || item.SupplierNo != null)
        //            {
        //                item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
        //            }
        //            else
        //            {
        //                item.str_suppliername = "";
        //            }
        //            // item.str_CCDescription = db.ACostCenters.Where(x => x.ID == int_id).FirstOrDefault().CCDescription;
        //            worksheet.Cells[rowIterator, 8].Value = item.str_CCDescription;
        //            worksheet.Cells[rowIterator, 9].Value = item.str_suppliername;
        //            worksheet.Cells[rowIterator, 10].Value = item.SrNo;
        //            worksheet.Cells[rowIterator, 8].Value = item.Model;
        //            worksheet.Cells[rowIterator, 9].Value = item.Remarks;
        //            worksheet.Cells[rowIterator, 10].Value = item.MRRNo;
        //            //if (item.DtPutToUse == null)
        //            //{
        //            //    item.str_DtPutToUse = "";
        //            //    worksheet.Cells[rowIterator, 11].Value = item.str_DtPutToUse;
        //            //}
        //            //else
        //            //{


        //            //    item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
        //            //    worksheet.Cells[rowIterator, 11].Value = item.str_DtPutToUse;

        //            //}
        //            rowIterator = rowIterator + 1;


        //        }

        //        return excel.GetAsByteArray();

        //    }
        //}

        [HttpGet]
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

        [ValidateJsonAntiForgeryToken]
        public ActionResult GetCostCenterGrpValues(DataTableAjaxPostModel filter, string id)
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
                    ACostCenter alist = new ACostCenter();
                    //alist.AGroup_name = db.AGroups.Where(x => x.ID == int_id).FirstOrDefault().AGroupName;
                    alist = db.ACostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    res.Data = alist;

                }
                if (Level == "L2") // selected parent level
                {
                    BCostCenter alist = new BCostCenter();
                    var agrpid = db.BCostCenters.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CCID;
                    //  alist.BGroup_name = db.BGroups.Where(x => x.AGrpID == agrpid && x.ID == int_id).FirstOrDefault().BGroupName;
                    alist = db.BCostCenters.Where(x => x.CCID == agrpid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                    alist.CCCode = alist.SCCCode;
                    alist.CCDescription = alist.SCCDescription;
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