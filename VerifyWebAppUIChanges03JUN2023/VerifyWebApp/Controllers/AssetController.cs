using OfficeOpenXml;
using VerifyWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
using System.Data.Entity;
using VerifyWebApp.BusinessLogic;
using VerifyWebApp.Filter;

using NLog;
using Newtonsoft.Json;

namespace VerifyWebApp.Controllers
{
    public class AssetController : Controller
    {
        // GET: AssetGroup

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public VerifyDB db = new VerifyDB();

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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            return View();
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public JsonResult SaveGroupNode(JsTreeNodeGroup node)
        {
            bool bResult = true;
            try
            {
                string Level = "";
                Level = node.id.Substring(0, 2);
                int tempLength = node.id.Length;
                string id = node.id.Substring(3, tempLength - 3);

                if (Level == "L0") // selected parent level
                {
                    AGroup agroup = new AGroup();
                    agroup.ClientID = 1;
                    agroup.AGroupName = node.GroupName;

                    db.AGroups.Add(agroup);
                    db.SaveChanges();

                }
                if (Level == "L1") // selected parent level
                {
                    BGroup bgroup = new BGroup();
                    bgroup.ClientID = 1;
                    bgroup.AGrpID = Convert.ToInt32(id);
                    bgroup.BGroupName = node.GroupName;

                    db.BGroups.Add(bgroup);
                    db.SaveChanges();

                }

                if (Level == "L2") // selected parent level
                {
                    CGroup cgroup = new CGroup();
                    cgroup.ClientID = 1;
                    int tempID = Convert.ToInt32(id);
                    BGroup bloc = db.BGroups.Where(x => x.ID == tempID).FirstOrDefault();

                    if (bloc != null)
                    {
                        cgroup.AGrpID = bloc.AGrpID;
                        cgroup.BGrpID = Convert.ToInt32(id);
                        cgroup.CGroupName = node.GroupName;

                        db.CGroups.Add(cgroup);
                        db.SaveChanges();
                    }


                }
                if (Level == "L3") // selected parent level
                {
                    DGroup dgroup = new DGroup();
                    dgroup.ClientID = 1;
                    int tempID = Convert.ToInt32(id);
                    CGroup cloc = db.CGroups.Where(x => x.ID == tempID).FirstOrDefault();

                    if (cloc != null)
                    {
                        dgroup.AGrpID = cloc.AGrpID;
                        dgroup.BGrpID = cloc.BGrpID;
                        dgroup.CGrpID = Convert.ToInt32(id);
                        dgroup.DGroupName = node.GroupName;

                        db.DGroups.Add(dgroup);
                        db.SaveChanges();
                    }


                }

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                bResult = false;
            }
            return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult GetGroups()
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
            List<JsTreeModel> list = new List<JsTreeModel>();

            List<AGroup> lstAgroup = new List<AGroup>();
            List<BGroup> lstBgroup = new List<BGroup>();
            List<CGroup> lstCgroup = new List<CGroup>();
            List<DGroup> lstDgroup = new List<DGroup>();

            lstAgroup = db.AGroups.Where(x => x.Companyid == companyid).ToList();

            lstBgroup = db.BGroups.Where(x => x.Companyid == companyid).ToList();

            lstCgroup = db.CGroups.Where(x => x.Companyid == companyid).ToList();
            lstDgroup = db.DGroups.ToList();

            JsTreeModel oNodeL0 = new JsTreeModel();

            oNodeL0.id = "L0-0";
            oNodeL0.text = "Asset Group List";
            oNodeL0.parent = "#";
            oNodeL0.children = false;


            list.Add(oNodeL0);

            // Level 1
            foreach (var item in lstAgroup)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L1-" + item.ID.ToString();
                oModel.text = item.AGroupName;
                oModel.parent = "L0-0";
                oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstBgroup)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L2-" + item.ID.ToString();
                oModel.text = item.BGroupName;
                oModel.parent = "L1-" + item.AGrpID.ToString();
                oModel.children = false;

                list.Add(oModel);

            }

            foreach (var item in lstCgroup)
            {

                JsTreeModel oModel = new JsTreeModel();

                oModel.id = "L3-" + item.ID.ToString();
                oModel.text = item.CGroupName;
                oModel.parent = "L2-" + item.BGrpID.ToString();
                oModel.children = false;

                list.Add(oModel);

            }
            foreach (var item in lstDgroup)
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

        public JsonResult GetLocations_Sample()
        {
            List<JsTreeModel> list = new List<JsTreeModel>();
            JsTreeModel model = new JsTreeModel();

            model.id = "1";
            model.text = "Asset Group list";
            model.parent = "#";
            // model.children = false;


            list.Add(model);

            JsTreeModel model_L1 = new JsTreeModel();

            model_L1.id = "2";
            model_L1.text = "Pune";
            model_L1.parent = "1";
            //   model_L1.children = false;


            list.Add(model_L1);

            JsTreeModel model_L2 = new JsTreeModel();

            model_L2.id = "3";
            model_L2.text = "Hyd";
            model_L2.parent = "1";
            // model_L2.children = false;


            list.Add(model_L2);

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                    alist = assetRepository.GetAssetDataSearch(companyid, Level, int_id, startRec, pageSize, searchby, searchstring);

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
                        recordsFiltered = recFilter,
                        data = alist,

                    }, JsonRequestBehavior.AllowGet);

                    result.MaxJsonLength = int.MaxValue;
                }
                else
                {
                    alist = assetRepository.GetAssetData(companyid, Level, int_id, startRec, pageSize);

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

        [HttpPost]
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

                    alist = db.Assetss.Where(x => x.AGroupID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();

                }
                else if (Level == "L2") // selected parent level
                {
                    BGroup bgroup = new BGroup();
                    var agroupid = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();


                }

                else if (Level == "L3") // selected parent level
                {


                    var agroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bgroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == bgroupid && x.CGroupID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
                }
                else if (Level == "L4") // selected parent level
                {


                    var agroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    var cgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGrpID;

                    alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == bgroupid && x.CGroupID == cgroupid && x.DGroupID == int_id && x.DisposalFlag == 0 && x.Companyid == companyid).ToList();
                }
                else
                {
                    alist = db.Assetss.Where(x => x.Companyid == companyid && x.DisposalFlag == 0).ToList();
                }
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


            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                //ex.Message("");
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
        [AuthUser]
        [HttpPost]
        [AllowAnonymous]

        public ActionResult GroupassetExport(string id)
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
            int srno = 1;
            string Level = "";
            if (id == "0")
            {
                alist = db.Assetss.Where(x => x.Companyid == companyid).ToList();
            }
            else
            {
                Level = id.Substring(0, 2);
                int tempLength = id.Length;
                string strr_id = id.Substring(3, tempLength - 3);
                int int_id = Convert.ToInt32(strr_id);

                if (Level == "L0") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.Companyid == companyid).ToList();

                }

                if (Level == "L1") // selected parent level
                {

                    alist = db.Assetss.Where(x => x.AGroupID == int_id && x.Companyid == companyid).ToList();

                }
                if (Level == "L2") // selected parent level
                {
                    BGroup bgroup = new BGroup();
                    var agroupid = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == int_id && x.Companyid == companyid).ToList();


                }

                if (Level == "L3") // selected parent level
                {


                    var agroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bgroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == bgroupid && x.CGroupID == int_id && x.Companyid == companyid).ToList();
                }
                if (Level == "L4") // selected parent level
                {


                    var agroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                    var bgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                    var cgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGrpID;

                    alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == bgroupid && x.CGroupID == cgroupid && x.DGroupID == int_id && x.Companyid == companyid).ToList();
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
                    "Qty","MainLocation","SubLocation","Sub-sublocation","Supplier","DtputtoUse","PurchaseAccountname","AccumulatedAccountname",
                        "Depreciation Name","CCA Name","CC Bname","Srno","Model","Remark","System AssetId","Issue Date","Parent_AssetNo","IsComponent"}
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
                        Supplier supplier = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault();
                        if (supplier != null)
                        {
                            item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
                        } else {
                            item.str_suppliername = "";
                        }



                    }
                    if (item.LocAID == null || item.LocAID == 0)
                    {
                        item.str_mainlocation = "";
                    }
                    else
                    {

                        ALocation aLocations = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault();
                        if (aLocations != null)
                        {
                            item.str_mainlocation = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault().ALocationName;
                        } else
                        {
                            item.str_mainlocation = "";
                        }


                    }
                    if (item.LocBID == null || item.LocBID == 0)
                    {
                        item.str_sublocation = "";
                    }
                    else
                    {
                        BLocation bLocation = db.BLocations.Where(x => x.ID == item.LocBID && x.Companyid == companyid).FirstOrDefault();

                        if (bLocation != null)
                        {
                            item.str_sublocation = db.BLocations.Where(x => x.ID == item.LocBID && x.Companyid == companyid).FirstOrDefault().BLocationName;
                        }
                        else
                        {
                            item.str_sublocation = "";
                        }



                    }
                    if (item.LocCID == 0 || item.LocCID == null)
                    {
                        item.str_sub_sublocation = "";
                    }
                    else
                    {

                        CLocation location = db.CLocations.Where(x => x.ID == item.LocCID && x.Companyid == companyid).FirstOrDefault();

                        if (location != null)
                        {
                            item.str_sub_sublocation = db.CLocations.Where(x => x.ID == item.LocCID && x.Companyid == companyid).FirstOrDefault().CLocationName;
                        }
                        else
                        {
                            item.str_sub_sublocation = "";
                        }



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
                    if (item.CostCenterAID == 0 || item.CostCenterAID == null)
                    {
                        item.str_costcenteraname = "";
                    }
                    else
                    {

                        item.str_costcenteraname = db.ACostCenters.Where(x => x.ID == item.CostCenterAID && x.Companyid == companyid).FirstOrDefault().CCDescription;

                    }
                    if (item.CostCenterBID == 0 || item.CostCenterBID == null)
                    {
                        item.str_costcenterbname = "";
                    }
                    else
                    {

                        item.str_costcenterbname = db.BCostCenters.Where(x => x.ID == item.CostCenterBID && x.Companyid == companyid).FirstOrDefault().SCCDescription;

                    }



                    worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 4].Value = item.str_VoucherDate;
                    worksheet.Cells[rowIterator, 5].Value = item.AmountCapitalisedCompany;
                    worksheet.Cells[rowIterator, 6].Value = item.BillNo;
                    worksheet.Cells[rowIterator, 7].Value = item.Qty;
                    worksheet.Cells[rowIterator, 8].Value = item.str_mainlocation;
                    worksheet.Cells[rowIterator, 9].Value = item.str_sublocation;
                    worksheet.Cells[rowIterator, 10].Value = item.str_sub_sublocation;
                    worksheet.Cells[rowIterator, 11].Value = item.SupplierNo;
                    worksheet.Cells[rowIterator, 12].Value = item.str_DtPutToUse;
                    worksheet.Cells[rowIterator, 13].Value = item.str_purchaseaccountname;
                    worksheet.Cells[rowIterator, 14].Value = item.str_accumulatedname;
                    worksheet.Cells[rowIterator, 15].Value = item.str_depricationname;
                    worksheet.Cells[rowIterator, 16].Value = item.str_costcenteraname;
                    worksheet.Cells[rowIterator, 17].Value = item.str_costcenterbname;
                    worksheet.Cells[rowIterator, 18].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 19].Value = item.Model;
                    worksheet.Cells[rowIterator, 20].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 21].Value = item.MRRNo;
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
                    //var locationissuedate = db.childlocations.Where(x => x.AssetID == item.ID).FirstOrDefault().Date.ToString("dd/MM/yyyy");
                    worksheet.Cells[rowIterator, 22].Value = item.str_issuedate;
                    if (item.Parent_AssetId == null || item.Parent_AssetId == 0)
                    {
                        worksheet.Cells[rowIterator, 23].Value = "";
                    }
                    else
                    {
                        var parentassetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.Parent_AssetId).FirstOrDefault().AssetNo;
                        worksheet.Cells[rowIterator, 23].Value = parentassetno;
                    }

                    worksheet.Cells[rowIterator, 24].Value = item.iscomponent;
                    rowIterator = rowIterator + 1;

                }


                string excelName = "Assetexport.xlsx";

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
        //public byte[] generategroupassetexcel(string id, int companyid)
        //{
        //    List<Assets> alist = new List<Assets>();
        //    int srno = 1;
        //    string Level = "";
        //    if (id == "0")
        //    {
        //        alist = db.Assetss.Where(x=>x.Companyid==companyid).ToList();
        //    }
        //    else
        //    {
        //        Level = id.Substring(0, 2);
        //        int tempLength = id.Length;
        //        string strr_id = id.Substring(3, tempLength - 3);
        //        int int_id = Convert.ToInt32(strr_id);
        //        if (Level == "L1") // selected parent level
        //        {

        //            alist = db.Assetss.Where(x => x.AGroupID == int_id && x.Companyid==companyid).ToList();

        //        }
        //        if (Level == "L2") // selected parent level
        //        {
        //            BGroup bgroup = new BGroup();
        //            var agroupid = db.BGroups.Where(x => x.ID == int_id && x.Companyid==companyid).FirstOrDefault().AGrpID;
        //            alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == int_id && x.Companyid==companyid).ToList();


        //        }

        //        if (Level == "L3") // selected parent level
        //        {


        //            var agroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
        //            var bgroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
        //            alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == bgroupid && x.CGroupID == int_id && x.Companyid == companyid).ToList();
        //        }
        //        if (Level == "L4") // selected parent level
        //        {


        //            var agroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
        //            var bgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
        //            var cgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGrpID;

        //            alist = db.Assetss.Where(x => x.AGroupID == agroupid && x.BGroupID == bgroupid && x.CGroupID == cgroupid && x.DGroupID == int_id && x.Companyid == companyid).ToList();
        //        }

        //    }

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
        //            "Qty","MainLocation","SubLocation","Sub-sublocation","Supplier","DtputtoUse","PurchaseAccountname","AccumulatedAccountname",
        //                "Depreciation Name","CCA Name","CC Bname","Srno","Model","Remark","System AssetId","Issue Date","Parent_AssetNo","IsComponent"}
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



        //            if (item.DtPutToUse == null)
        //            {
        //                item.str_DtPutToUse = "";
        //            }
        //            else
        //            {


        //                item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

        //            }



        //            if (item.BillDate == null)
        //            {
        //                item.str_BillDate = "";
        //            }
        //            else
        //            {

        //                item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

        //            }

        //            if (item.VoucherDate == null)
        //            {
        //                item.str_VoucherDate = "";
        //            }
        //            else
        //            {

        //                item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

        //            }

        //            if (item.SupplierNo == null)
        //            {
        //                item.str_suppliername = "";
        //            }
        //            else
        //            {

        //                item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;

        //            }
        //            if (item.LocAID == null || item.LocAID == 0)
        //            {
        //                item.str_mainlocation = "";
        //            }
        //            else
        //            {

        //                item.str_mainlocation = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault().ALocationName;

        //            }
        //            if (item.LocBID == null || item.LocBID == 0)
        //            {
        //                item.str_sublocation = "";
        //            }
        //            else
        //            {

        //                item.str_sublocation = db.BLocations.Where(x => x.ID == item.LocBID && x.Companyid == companyid).FirstOrDefault().BLocationName;

        //            }
        //            if (item.LocCID == 0 || item.LocCID == null )
        //            {
        //                item.str_sub_sublocation = "";
        //            }
        //            else
        //            {

        //                item.str_sub_sublocation = db.CLocations.Where(x => x.ID == item.LocCID && x.Companyid == companyid).FirstOrDefault().CLocationName;

        //            }
        //            if (item.AccAccountID == null || item.AccAccountID == 0)
        //            {
        //                item.str_accumulatedname = "";
        //            }
        //            else
        //            {

        //                item.str_accumulatedname = db.Accounts.Where(x => x.ID == item.AccAccountID && x.Companyid == companyid).FirstOrDefault().AccountName;

        //            }

        //            if (item.AccountID == null || item.AccountID == 0)
        //            {
        //                item.str_purchaseaccountname = "";
        //            }
        //            else
        //            {

        //                item.str_purchaseaccountname = db.Accounts.Where(x => x.ID == item.AccountID && x.Companyid == companyid).FirstOrDefault().AccountName;

        //            }
        //            if (item.DepAccountId == null || item.DepAccountId == 0)
        //            {
        //                item.str_depricationname = "";
        //            }
        //            else
        //            {

        //                item.str_depricationname = db.Accounts.Where(x => x.ID == item.DepAccountId && x.Companyid == companyid).FirstOrDefault().AccountName;

        //            }
        //            if (item.ITGroupIDID == null || item.ITGroupIDID == 0)
        //            {
        //                item.str_it_name = "";
        //            }
        //            else
        //            {

        //                item.str_it_name = db.ITGroups.Where(x => x.ID == item.ITGroupIDID && x.Companyid == companyid).FirstOrDefault().GroupName;

        //            }
        //            if (item.CostCenterAID == 0 || item.CostCenterAID == null)
        //            {
        //                item.str_costcenteraname = "";
        //            }
        //            else
        //            {

        //                item.str_costcenteraname = db.ACostCenters.Where(x => x.ID == item.CostCenterAID && x.Companyid == companyid).FirstOrDefault().CCDescription;

        //            }
        //            if (item.CostCenterBID == 0 || item.CostCenterBID == null)
        //            {
        //                item.str_costcenterbname = "";
        //            }
        //            else
        //            {

        //                item.str_costcenterbname = db.BCostCenters.Where(x => x.ID == item.CostCenterBID && x.Companyid == companyid).FirstOrDefault().SCCDescription;

        //            }



        //            worksheet.Cells[rowIterator, 1].Value = item.AssetNo;
        //            worksheet.Cells[rowIterator, 2].Value = item.AssetIdentificationNo;
        //            worksheet.Cells[rowIterator, 3].Value = item.AssetName;
        //            worksheet.Cells[rowIterator, 4].Value = item.str_VoucherDate;
        //            worksheet.Cells[rowIterator, 5].Value = item.AmountCapitalisedCompany;
        //            worksheet.Cells[rowIterator, 6].Value = item.BillNo;
        //            worksheet.Cells[rowIterator, 7].Value = item.Qty;
        //            worksheet.Cells[rowIterator, 8].Value = item.str_mainlocation;
        //            worksheet.Cells[rowIterator, 9].Value = item.str_sublocation;
        //            worksheet.Cells[rowIterator, 10].Value = item.str_sub_sublocation;
        //            worksheet.Cells[rowIterator, 11].Value = item.SupplierNo;
        //            worksheet.Cells[rowIterator, 12].Value = item.str_DtPutToUse;
        //            worksheet.Cells[rowIterator, 13].Value = item.str_purchaseaccountname;
        //            worksheet.Cells[rowIterator, 14].Value = item.str_accumulatedname;
        //            worksheet.Cells[rowIterator, 15].Value = item.str_depricationname;
        //            worksheet.Cells[rowIterator, 16].Value = item.str_costcenteraname;
        //            worksheet.Cells[rowIterator, 17].Value = item.str_costcenterbname;
        //            worksheet.Cells[rowIterator, 18].Value = item.SrNo;
        //            worksheet.Cells[rowIterator, 19].Value = item.Model;
        //            worksheet.Cells[rowIterator, 20].Value = item.Remarks;
        //            worksheet.Cells[rowIterator, 21].Value = item.MRRNo;
        //            var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
        //            if (locid != null)
        //            {
        //                if (locid.Date == null)
        //                {
        //                    item.str_issuedate = "";
        //                }
        //                else
        //                {
        //                    item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

        //                }
        //            }
        //            //var locationissuedate = db.childlocations.Where(x => x.AssetID == item.ID).FirstOrDefault().Date.ToString("dd/MM/yyyy");
        //            worksheet.Cells[rowIterator, 22].Value = item.str_issuedate;
        //            if(item.Parent_AssetId==null || item.Parent_AssetId == 0)
        //            {
        //                worksheet.Cells[rowIterator, 23].Value = "";
        //            }
        //            else
        //            {
        //                var parentassetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.Parent_AssetId).FirstOrDefault().AssetNo;
        //                worksheet.Cells[rowIterator, 23].Value = parentassetno;
        //            }

        //            worksheet.Cells[rowIterator, 24].Value = item.iscomponent;
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
        [AuthUser]
        [HttpGet]
        public ActionResult Add(string id)
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

            // Addition addition = new Addition();
            //var addition = db.Assetss.Max(x => x.ID == 0 ? 0 : x.ID);
            ////var addition = db.Assetss.OrderBy(x => x.AssetNo).FirstOrDefault().AssetNo;
            // if (addition == null)
            // {
            //     ViewBag.additionno = 1;
            // }
            // else
            // {
            //     ViewBag.additionno = addition + 1;
            // }

            string Level = "";
            Level = id.Substring(0, 2);
            int tempLength = id.Length;
            string strr_id = id.Substring(3, tempLength - 3);
            int int_id = Convert.ToInt32(strr_id);

            string AGroupName = "";
            string BGroupName = "";
            string CGroupName = "";
            string DGroupName = "";

            if (Level == "L1") // selected parent level
            {

                AGroup A_Group = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();

                if (A_Group != null)
                {
                    ViewBag.groupname = A_Group.AGroupName;
                    AGroupName = A_Group.AGroupName;
                    ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0
                    && x.Companyid == companyid && x.AGroupID == int_id).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
                    AGroupName = ViewBag.groupname;

                }

                //ViewBag.groupname = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGroupName;




            }
            if (Level == "L2") // selected parent level
            {
                //BGroup bgroup = new BGroup();
                //var agroupid = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;

                BGroup bgroup = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                if (bgroup != null)
                {

                    int aGrpID = bgroup.AGrpID;

                    ViewBag.groupname = bgroup.BGroupName;
                    BGroupName = bgroup.BGroupName;

                    ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == aGrpID && x.BGroupID == int_id).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");

                }


            }

            if (Level == "L3") // selected parent level
            {

                CGroup c_group = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();


                //var agroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                //var bgroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;

                //ViewBag.groupname = db.CGroups.Where(x => x.AGrpID == agroupid && x.BGrpID == bgroupid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGroupName;

                if (c_group != null)
                {
                    ViewBag.groupname = c_group.CGroupName;
                    CGroupName = c_group.CGroupName;


                    int bGrpID = c_group.BGrpID;
                    int aGrpID = c_group.AGrpID;

                    if (bGrpID > 0)
                    {
                        BGroup b_group = db.BGroups.Where(x => x.Companyid == companyid && x.ID == bGrpID).FirstOrDefault();

                        if (b_group != null)
                        {
                            BGroupName = b_group.BGroupName;
                        }
                    }

                    if (aGrpID > 0)
                    {
                        AGroup A_group = db.AGroups.Where(x => x.Companyid == companyid && x.ID == aGrpID).FirstOrDefault();

                        if (A_group != null)
                        {
                            AGroupName = A_group.AGroupName;
                        }
                    }


                    ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == aGrpID && x.BGroupID == bGrpID && x.CGroupID == int_id).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
                }


            }
            if (Level == "L4") // selected parent level
            {

                DGroup d_group = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault();

                if (d_group != null)
                {
                    ViewBag.groupname = d_group.DGroupName;
                    ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == d_group.AGrpID && x.BGroupID == d_group.BGrpID && x.CGroupID == d_group.CGrpID && x.DGroupID == int_id).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
                    DGroupName = d_group.DGroupName;

                    int cGrpID = d_group.CGrpID;
                    int bGrpID = d_group.BGrpID;
                    int aGrpID = d_group.AGrpID;

                    if (cGrpID > 0)
                    {
                        CGroup c_group = db.CGroups.Where(x => x.Companyid == companyid && x.ID == cGrpID).FirstOrDefault();

                        if (c_group != null)
                        {
                            CGroupName = c_group.CGroupName;
                        }
                    }
                    if (bGrpID > 0)
                    {
                        BGroup b_group = db.BGroups.Where(x => x.Companyid == companyid && x.ID == bGrpID).FirstOrDefault();

                        if (b_group != null)
                        {
                            BGroupName = b_group.BGroupName;
                        }
                    }

                    if (aGrpID > 0)
                    {
                        AGroup A_group = db.AGroups.Where(x => x.Companyid == companyid && x.ID == aGrpID).FirstOrDefault();

                        if (A_group != null)
                        {
                            AGroupName = A_group.AGroupName;
                        }
                    }

                }



                //var agroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                //var bgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                //var cgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGrpID;



                //ViewBag.groupname = db.DGroups.Where(x => x.AGrpID == agroupid && x.BGrpID == bgroupid && x.CGrpID == cgroupid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault().DGroupName;

            }

            List<Assets> lstAssets = new List<Assets>();

            ViewBag.Assestlist = new SelectList(lstAssets, "ID", "AssetName");



            ViewBag.supplierlist = new SelectList(db.Suppliers.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "SupplierName");
            ViewBag.uomlist = new SelectList(db.UOMs.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "Unit");
            ViewBag.itgrouplist = new SelectList(db.ITGroups.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "GroupName");
            ViewBag.purchaselist = new SelectList(db.Accounts.Where(x => x.GroupName == "Asset" && x.Companyid == companyid).OrderBy(e => e.ID), "ID", "AccountName");
            ViewBag.depreciationlist = new SelectList(db.Accounts.Where(x => x.GroupName == "Expense" && x.Companyid == companyid).OrderBy(e => e.ID), "ID", "AccountName");
            ViewBag.accumulatedlist = new SelectList(db.Accounts.Where(x => x.GroupName == "Asset" || x.GroupName == "Liability" && x.Companyid == companyid).OrderBy(e => e.ID), "ID", "AccountName");
            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");
            ViewBag.costalist = new SelectList(db.ACostCenters.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "CCDescription");
            // ViewBag.costblist = new SelectList(db.BCostCenters.OrderBy(e => e.ID), "ID", "SCCDescription");
            ViewBag.levelid = id;

            ViewBag.AGroupName = AGroupName;
            ViewBag.BGroupName = BGroupName;
            ViewBag.CGroupName = CGroupName;
            ViewBag.DGroupName = DGroupName;

            // var LastAssetNo = "";

            //LastAssetNo = db.Assetss.OrderByDescending(x => x.ID).ToList().FirstOrDefault().AssetNo;

            string strsql = "select max(convert(assetno, unsigned)) assetno from tblassets order by convert(assetno, unsigned)";

            var LastAssetNo = db.Database.SqlQuery<int>(strsql).FirstOrDefault();

            var NextAssetNo = LastAssetNo + 1;

            Company companyObj = db.Companys.Where(x => x.ID == companyid).FirstOrDefault();
            if (companyObj != null)
            {
                ViewBag.ResidualValuePercent = companyObj.ResidualValuePercent;
            }


            ViewBag.LastAssetNo = LastAssetNo;
            ViewBag.NextAssetNo = NextAssetNo;
            ViewBag.AutoGenerateAssetNo = company.AutoGenerateAssetNo;

            return View();
        }
        [HttpGet]
        public ActionResult GetUsefulife(string parentassetno, DateTime vdate)
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
            DateTime Vdate;// Convert.ToDateTime(vdate);
            JsonResult res = new JsonResult();
            // DateTime componentdate;

            var assetid = db.Assetss.Where(x => x.AssetNo == parentassetno && x.Companyid == companyid).FirstOrDefault().ID;
            var assetobj = db.Assetss.Where(x => x.ID == assetid && x.Companyid == companyid).FirstOrDefault();
            DateTime assetvdate = Convert.ToDateTime(assetobj.VoucherDate);
            int assetusefullife = Convert.ToInt32(assetobj.Usefullife);

            DateTime assetfinaldate = assetvdate.AddYears(assetusefullife);


            double componentusefullife = Math.Floor(assetfinaldate.Subtract(vdate).TotalDays / 365.0);
            res.Data = componentusefullife.ToString();



            return Json(res, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult getlocationb(string id)
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

            int int_id = Convert.ToInt32(id);

            List<BLocation> blist = new List<BLocation>();
            blist = db.BLocations.Where(x => x.ALocID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(blist, "ID", "BLocationName", 0);
            return Json(ob);

        }
        [HttpPost]
        public ActionResult getlocationc(string id)
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
            int int_id = Convert.ToInt32(id);

            List<CLocation> clist = new List<CLocation>();
            clist = db.CLocations.Where(x => x.BLocID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(clist, "ID", "CLocationName", 0);
            return Json(ob);


        }
        [HttpPost]
        public ActionResult getcostcenterb(string id)
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
            int int_id = Convert.ToInt32(id);

            List<BCostCenter> blist = new List<BCostCenter>();
            blist = db.BCostCenters.Where(x => x.CCID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(blist, "ID", "SCCDescription", 0);
            return Json(ob);

        }
        [HttpGet]
        public ActionResult checkopeningaccumulateddepreciation(DateTime strvdate)
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
            List<Period> period = new List<Period>();

            // period =;
            var firstentry = db.Periods.Where(x => x.Companyid == companyid).FirstOrDefault();
            string checkflag = "";

            if (firstentry != null)
            {

                if (strvdate < firstentry.FromDate)
                {
                    checkflag = "Yes";
                    res.Data = checkflag;
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    checkflag = "No";
                    res.Data = checkflag;
                }

            }
            else
            {
                res.Data = "Noperiod";
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }
        public string str_checkopeningaccumulateddepreciation(DateTime strvdate)
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

            }

            List<Period> period = new List<Period>();
            string res;
            // period =;
            var firstentry = db.Periods.Where(x => x.Companyid == companyid).FirstOrDefault();
            string checkflag = "";

            if (firstentry != null)
            {

                if (strvdate < firstentry.FromDate)
                {
                    checkflag = "Yes";
                    res = checkflag;
                    return res;
                }
                else
                {
                    checkflag = "No";
                    res = checkflag;
                }

            }
            else
            {
                res = "Noperiod";
            }

            return res;

        }
        [HttpGet]
        public ActionResult assetnoexists(string assetno)
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
            List<Assets> assets = new List<Assets>();
            assets = db.Assetss.Where(x => x.Companyid == companyid).ToList();
            if (assets.Count != 0)
            {
                foreach (Assets item in assets)
                {
                    if (item.AssetNo == assetno)
                    {
                        res.Data = "Yes";
                        break;
                    }
                    else
                    {
                        res.Data = "No";
                    }
                }
            }
            else
            {
                res.Data = "Noassetfound";
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// check whwther period or itperiod exits or not 
        /// </summary>
        /// <param name="strvdate"></param>
        /// <returns></returns>
        /// 

        [HttpGet]
        public ActionResult checkperiod_itperiod_exits()
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
            List<Period> period = new List<Period>();
            period = db.Periods.ToList();
            List<ITPeriod> itperiod = new List<ITPeriod>();
            itperiod = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
            if (period.Count != 0)
            {
                res.Data = "Yesperiodexists";

            }
            else
            {
                res.Data = "Noperiodexists";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            if (itperiod.Count != 0)
            {
                res.Data = "Yesitperiodexists";

            }
            else
            {
                res.Data = "Noitperiodexists";
                return Json(res, JsonRequestBehavior.AllowGet);
            }


            return Json(res, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult Dateputtousevalidation(DateTime strvdate)
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
            //i have used res.Data=yess for error showing error and res.data=no for no errors

            JsonResult res;
            res = new JsonResult();
            DateTime itperioddate = Convert.ToDateTime("01/01/0001");
            DateTime perioddate = Convert.ToDateTime("01/01/0001");
            List<ITPeriod> itperiod = new List<ITPeriod>();

            // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
            itperiod = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
            // string checkflag = "";

            //if (itperiod.Count!=0)
            //{
            List<ITPeriod> itperiodlock = new List<ITPeriod>();
            itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1 && x.Companyid == companyid).ToList();
            if (itperiodlock.Count != 0)
            {
                foreach (ITPeriod item in itperiodlock)
                {
                    if (strvdate >= item.FromDate && strvdate <= item.ToDate)
                    {
                        // checkflag = "Yes";
                        // res.Data = checkflag;
                        itperioddate = item.ToDate;

                        break;
                    }
                    //else
                    //{
                    //   itperioddate = Convert.ToDateTime("00/00/0000");
                    //    // checkflag = "No";
                    //    //res.Data = checkflag;
                    //}
                }

            }
            else
            {
                // perioddate = Convert.ToDateTime("00/00/0000");
                // res.Data = "NoLock";
            }
            //}
            //else
            //{
            //    res.Data = "Noitperiod";
            //}

            // List<Period> period = new List<Period>();
            //int periodid;
            List<SubPeriod> subperiod = new List<SubPeriod>();
            // subperiod = db.SubPeriods.ToList();
            //period = db.Periods.ToList();
            //if(period.Count!=0)
            //{
            //  foreach (Period item in period)
            // {
            //if (vdate >= item.FromDate && vdate <= item.ToDate)
            //{
            //  periodid = item.ID;
            List<SubPeriod> slist = new List<SubPeriod>();
            slist = db.SubPeriods.Where(x => x.Companyid == companyid).ToList();
            if (slist != null)
            {
                SubPeriod checkdepflag = new SubPeriod();
                checkdepflag = db.SubPeriods.Where(x => x.DepFlag == "Y" && x.FromDate <= strvdate && x.ToDate >= strvdate && x.Companyid == companyid).FirstOrDefault();
                if (checkdepflag == null)
                {
                    subperiod = db.SubPeriods.Where(x => x.PeriodLockFlag == "Y" && x.Companyid == companyid).ToList();
                    if (subperiod.Count != 0)
                    {
                        foreach (SubPeriod itemsub in subperiod)
                        {
                            if (strvdate >= itemsub.FromDate && strvdate <= itemsub.ToDate)
                            {
                                perioddate = itemsub.ToDate;
                                break;
                            }
                            //else
                            //{
                            //    perioddate = Convert.ToDateTime("00/00/0000");
                            //}
                        }
                    }
                    else
                    {
                        //perioddate = Convert.ToDateTime("00/00/0000");
                    }
                }

                else
                {
                    res.Data = "Depalreadycalculated";
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                res.Data = "Nosubperiod";
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            //}
            //else
            //{
            //    //noperiod
            //    perioddate = Convert.ToDateTime("00/00/0000");
            //}
            DateTime lockdate;
            int value = DateTime.Compare(perioddate, itperioddate);

            // checking 
            if (value > 0)
            {
                lockdate = perioddate;
                // Console.Write("date1 is later than date2. ");
            }
            else if (value < 0)
            {
                lockdate = itperioddate;
                //Console.Write("date1 is earlier than date2. ");
            }

            else
            {
                lockdate = perioddate;
                //Console.Write("date1 is the same as date2. ");
            }

            if (strvdate <= lockdate)
            {
                //error
                res.Data = "Yes";
            }
            else
            {
                //nothing
                res.Data = "No";
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]

        public ActionResult Addassetgroup(AssetGroupViewmodel assetGroup)
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
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Assets asset = new Assets();
                    assetGroup.str_VoucherDate = Convert.ToDateTime(assetGroup.VoucherDate).ToString("yyyy-MM-dd");

                    DateTime checkvoucherdate;// = DateTime.Parse(assets.str_VoucherDate);
                    if (DateTime.TryParseExact(assetGroup.str_VoucherDate, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out checkvoucherdate))
                    {
                        string checklockflag = ImportDatevalidation(checkvoucherdate, companyid);
                        if (checklockflag == "Depalreadycalculated")
                        {
                            res.Data = "Depreciation already calculated";
                            return res;
                        }
                        if (checklockflag == "Yes")
                        {

                            res.Data = "Periodlock";
                            return res;
                        }
                        if (checklockflag == "No")
                        {

                        }
                        if (checklockflag == "Nomainperiod")
                        {
                            res.Data = "No main period please add period";
                            return res;

                        }
                    }

                    List<Depreciation> deplist = new List<Depreciation>();
                    deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == assetGroup.ID).ToList();
                    if (deplist.Count() != 0)
                    {
                        res.Data = "Depreciation already calculated";
                        return res;
                    }

                    if (assetGroup.VoucherDate > assetGroup.DtPutToUse)
                    {
                        res.Data = "Voucher Date cannot be greater than Dateputtouse";
                        return res;
                    }
                    if (assetGroup.VoucherDate > assetGroup.ExpiryDate)
                    {
                        res.Data = "Voucher Date cannot be greater than ExpiryDate";
                        return res;
                    }
                    DateTime vdate = Convert.ToDateTime(assetGroup.VoucherDate);

                    //this is for opening accumalted depreciation
                    var oppaccdep = str_checkopeningaccumulateddepreciation(vdate);
                    if (oppaccdep == "Yes")
                    {
                        asset.OPAccDepreciation = assetGroup.OPAccDepreciation;
                    }
                    if (oppaccdep == "No")
                    {
                        asset.OPAccDepreciation = 0;
                    }
                    if (oppaccdep == "Noperiod")
                    {
                        res.Data = "No main period please add period";
                        return res;
                    }



                    asset.ID = assetGroup.ID;

                    string parentassetno = assetGroup.Parent_AssetId.ToString();
                    if (parentassetno == "")
                    {
                        asset.Parent_AssetId = 0;
                    }
                    else
                    {
                        var parentassetid = db.Assetss.Where(x => x.AssetNo == parentassetno && x.Companyid == companyid).FirstOrDefault().ID;
                        asset.Parent_AssetId = parentassetid;
                    }
                    asset.iscomponent = assetGroup.iscomponent;
                    asset.AssetNo = assetGroup.AssetNo;
                    asset.AssetName = assetGroup.AssetName;
                    asset.VoucherNo = assetGroup.VoucherNo;

                    if (assetGroup.DtPutToUse == null)
                    {
                        asset.DtPutToUse = null;
                    }
                    else
                    {
                        asset.DtPutToUse = assetGroup.DtPutToUse;

                    }
                    if (assetGroup.DtPutToUseIT == null)
                    {
                        asset.DtPutToUseIT = null;
                    }
                    else
                    {


                        asset.DtPutToUseIT = assetGroup.DtPutToUseIT;

                    }


                    if (assetGroup.BillDate == null)
                    {
                        asset.BillDate = null;
                    }
                    else
                    {

                        asset.BillDate = assetGroup.BillDate;

                    }
                    if (assetGroup.ReceiptDate == null)
                    {
                        asset.ReceiptDate = null;
                    }
                    else
                    {

                        asset.ReceiptDate = assetGroup.ReceiptDate;

                    }

                    if (assetGroup.VoucherDate == null)
                    {
                        asset.VoucherDate = null;
                    }
                    else
                    {

                        asset.VoucherDate = assetGroup.VoucherDate;

                    }
                    if (assetGroup.PODate == null)
                    {
                        asset.PODate = null;
                    }
                    else
                    {

                        asset.PODate = assetGroup.PODate;

                    }
                    if (assetGroup.CommissioningDate == null)
                    {
                        asset.CommissioningDate = null;
                    }
                    else
                    {

                        asset.CommissioningDate = assetGroup.CommissioningDate;

                    }
                    if (assetGroup.ExpiryDate == null)
                    {
                        asset.ExpiryDate = null;
                    }
                    else
                    {
                        DateTime caldate = Convert.ToDateTime(assetGroup.VoucherDate).AddYears(Convert.ToInt32(assetGroup.Usefullife));
                        if (caldate == assetGroup.ExpiryDate)
                        {
                            asset.ExpiryDate = assetGroup.ExpiryDate;
                        }
                        else
                        {
                            asset.ExpiryDate = caldate;
                        }



                    }


                    asset.AssetName = assetGroup.AssetName;

                    asset.AssetIdentificationNo = assetGroup.AssetIdentificationNo;
                    asset.PONo = assetGroup.PONo;
                    asset.BillNo = assetGroup.BillNo;
                    asset.Qty = assetGroup.Qty;
                    asset.SupplierNo = assetGroup.SupplierNo;
                    asset.UOMNo = assetGroup.UOMNo;
                    asset.GrossVal = decimalToDecimal(assetGroup.GrossVal);
                    asset.Discount = decimalToDecimal(assetGroup.Discount);
                    asset.DutyDrawback = decimalToDecimal(assetGroup.DutyDrawback);
                    asset.ServiceCharges = decimalToDecimal(assetGroup.ServiceCharges);
                    //  asset.OPAccDepreciation = assetGroup.OPAccDepreciation;
                    asset.Roundingoff = decimalToDecimal(assetGroup.Roundingoff);
                    asset.ExciseCredit = decimalToDecimal(assetGroup.ExciseCredit);
                    asset.OtherExp = decimalToDecimal(assetGroup.OtherExp);
                    asset.TotDeduction = decimalToDecimal(assetGroup.TotDeduction);
                    asset.ServiceTaxCredit = decimalToDecimal(assetGroup.ServiceTaxCredit);
                    asset.CustomDuty = decimalToDecimal(assetGroup.CustomDuty);

                    asset.AnyOtherDutyCredit = decimalToDecimal(assetGroup.AnyOtherDutyCredit);
                    asset.ExciseDuty = decimalToDecimal(assetGroup.ExciseDuty);
                    asset.VATCredit = decimalToDecimal(assetGroup.VATCredit);
                    asset.ServiceTax = decimalToDecimal(assetGroup.ServiceTax);
                    asset.CSTCredit = decimalToDecimal(assetGroup.CSTCredit);
                    asset.AnyOtherDuty = decimalToDecimal(assetGroup.AnyOtherDuty);
                    asset.CGSTCredit = decimalToDecimal(assetGroup.CGSTCredit);
                    asset.VAT = decimalToDecimal(assetGroup.VAT);
                    asset.SGSTCredit = decimalToDecimal(assetGroup.SGSTCredit);
                    asset.CSt = decimalToDecimal(assetGroup.CSt);
                    asset.IGSTCredit = decimalToDecimal(assetGroup.IGSTCredit);
                    asset.CGST = decimalToDecimal(assetGroup.CGST);
                    asset.AnyOtherCredit = decimalToDecimal(assetGroup.AnyOtherCredit);
                    asset.AnyOtherTax = decimalToDecimal(assetGroup.AnyOtherTax);
                    asset.SGST = decimalToDecimal(assetGroup.SGST);
                    asset.IGST = decimalToDecimal(assetGroup.IGST);
                    asset.ResidualVal = decimalToDecimal(assetGroup.ResidualVal);
                    //ajinkya server side total calculation
                    asset.TotalAddition = asset.GrossVal + asset.ServiceCharges + asset.OtherExp + asset.CustomDuty + asset.ExciseDuty + asset.ServiceTax;
                    asset.TotalAddition = asset.TotalAddition + asset.AnyOtherDuty + asset.VAT + asset.CGST + asset.IGST + asset.SGST + asset.CSt + asset.AnyOtherTax;
                    asset.InvoiceAmt = asset.TotalAddition - asset.Discount - asset.Roundingoff - asset.TotDeduction;
                    asset.TotalCredit = asset.DutyDrawback + asset.ExciseCredit + asset.ServiceTaxCredit + asset.AnyOtherDutyCredit + asset.AnyOtherCredit + asset.VATCredit;
                    asset.TotalCredit = asset.TotalCredit + asset.SGSTCredit + asset.CGSTCredit + asset.CSTCredit + asset.IGSTCredit;
                    asset.AmountCapitalised = asset.InvoiceAmt - asset.TotalCredit;
                    asset.AmountCapitalisedCompany = asset.InvoiceAmt - asset.TotalCredit;
                    asset.AmountCApitalisedIT = asset.InvoiceAmt - asset.TotalCredit;
                    ///////////


                    asset.Model = assetGroup.Model;
                    asset.BrandName = assetGroup.BrandName;
                    asset.SrNo = assetGroup.SrNo;
                    asset.Remarks = assetGroup.Remarks;
                    asset.IsImported = assetGroup.IsImported;
                    asset.Currency = assetGroup.Currency;
                    asset.Values = decimalToDecimal(assetGroup.Values);
                    //////////////////////////////////////////////////////////////
                    asset.NormalRatae = decimalToDecimal(assetGroup.NormalRatae);
                    asset.AdditionalRate = decimalToDecimal(assetGroup.AdditionalRate);
                    //-----this is for total rate

                    asset.TotalRate = asset.NormalRatae + asset.AdditionalRate;

                    //
                    if (assetGroup.AccountID == null)
                    {
                        asset.AccountID = 0;
                    }
                    else
                    {
                        asset.AccountID = assetGroup.AccountID;
                    }
                    if (assetGroup.DepAccountId == null)
                    {
                        asset.DepAccountId = 0;
                    }
                    else
                    {
                        asset.DepAccountId = assetGroup.DepAccountId;
                    }
                    if (assetGroup.ITGroupIDID == null)
                    {
                        asset.ITGroupIDID = 0;
                    }
                    else
                    {
                        asset.ITGroupIDID = assetGroup.ITGroupIDID;
                    }

                    asset.Usefullife = assetGroup.Usefullife;
                    asset.YrofManufacturing = assetGroup.YrofManufacturing;
                    asset.DepreciationMethod = assetGroup.DepreciationMethod;
                    /////
                    asset.CreatedDate = istDate;
                    asset.CreatedUserId = userid;
                    asset.Companyid = companyid;
                    //////////////////////////////
                    ////////save groups id based on level
                    ///

                    /* old logic commented by Mandar 23 MAR 2023
                    string Level = "";
                    Level = assetGroup.levelid.Substring(0, 2);
                    int tempLength = assetGroup.levelid.Length;
                    string strr_id = assetGroup.levelid.Substring(3, tempLength - 3);
                    int int_id = Convert.ToInt32(strr_id);
                    if (Level == "L1") // selected parent level
                    {
                        var id = db.AGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().ID;

                        asset.AGroupID = id;
                        asset.BGroupID = 0;
                        asset.CGroupID = 0;
                        asset.DGroupID = 0;


                    }
                    if (Level == "L2") // selected parent level
                    {

                        var agroupid = db.BGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                        var bgroup = db.BGroups.Where(x => x.AGrpID == agroupid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                        asset.AGroupID = bgroup.AGrpID;
                        asset.BGroupID = bgroup.ID;
                        asset.CGroupID = 0;
                        asset.DGroupID = 0;

                    }

                    if (Level == "L3") // selected parent level
                    {


                        var agroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                        var bgroupid = db.CGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                        var cgroup = db.CGroups.Where(x => x.AGrpID == agroupid && x.BGrpID == bgroupid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                        asset.AGroupID = cgroup.AGrpID;
                        asset.BGroupID = cgroup.BGrpID;
                        asset.CGroupID = cgroup.ID;
                        asset.DGroupID = 0;
                    }
                    if (Level == "L4") // selected parent level
                    {


                        var agroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().AGrpID;
                        var bgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().BGrpID;
                        var cgroupid = db.DGroups.Where(x => x.ID == int_id && x.Companyid == companyid).FirstOrDefault().CGrpID;
                        var dgroup = db.DGroups.Where(x => x.AGrpID == agroupid && x.BGrpID == bgroupid && x.CGrpID == cgroupid && x.ID == int_id && x.Companyid == companyid).FirstOrDefault();
                        asset.AGroupID = dgroup.AGrpID;
                        asset.BGroupID = dgroup.BGrpID;
                        asset.CGroupID = dgroup.CGrpID;
                        asset.DGroupID = dgroup.ID;
                    }
                    */

                    asset.AGroupID = assetGroup.AGroupID;
                    asset.BGroupID = assetGroup.BGroupID;
                    asset.CGroupID = assetGroup.CGroupID;
                    asset.DGroupID = assetGroup.DGroupID;

                    ///////////////////////////////////////////////
                    db.Assetss.Add(asset);
                    // db.Entry(asset).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //var assetid = db.Assetss.Max(x => x.AssetNo);
                    var assetid = db.Assetss.Where(x => x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault().ID;
                    var abc = asset.ID;
                    ////////////////////////////////////////////////////
                    //locationtable binding


                    Childlocation childlocations = new Childlocation();
                    if (assetGroup.locationlist.Count() != 0)
                    {
                        foreach (var item in assetGroup.locationlist)
                        {
                            if (item.Date == null)
                            {
                                item.Date = null;
                            }
                            item.AssetID = assetid;
                            childlocations.AssetID = item.AssetID;
                            childlocations.ALocID = item.ALocID;
                            childlocations.BLocID = item.BLocID;
                            childlocations.CLocID = item.CLocID;
                            childlocations.Date = item.Date;
                            childlocations.CreatedDate = istDate;
                            childlocations.Companyid = companyid;
                            childlocations.CreatedUserId = userid;
                            db.childlocations.Add(childlocations);
                            db.SaveChanges();

                        }
                    }

                    Childcostcenter childcostcenter = new Childcostcenter();
                    if (assetGroup.costcenterlist.Count() != 0)
                    {
                        foreach (var item in assetGroup.costcenterlist)
                        {
                            if (item.Date == null)
                            {
                                item.Date = null;
                            }

                            item.Ass_ID = assetid;
                            childcostcenter.Ass_ID = item.Ass_ID;
                            childcostcenter.Date = item.Date;
                            childcostcenter.AcostcenterID = item.AcostcenterID;
                            childcostcenter.BcostcenterID = item.BcostcenterID;
                            childcostcenter.Percentage = item.Percentage;
                            childcostcenter.CreatedDate = istDate;
                            childcostcenter.Companyid = companyid;
                            childcostcenter.CreatedUserId = userid;
                            db.childcostcenters.Add(childcostcenter);
                            db.SaveChanges();

                        }
                    }

                    Assetfreeofcost assetfreeofcost = new Assetfreeofcost();
                    if (assetGroup.assetfreeofcostlist.Count() != 0)
                    {
                        foreach (var item in assetGroup.assetfreeofcostlist)
                        {
                            if (item.Date == null)
                            {
                                item.Date = null;
                            }
                            item.Asset_id = assetid;
                            assetfreeofcost.Asset_id = item.Asset_id;
                            assetfreeofcost.Date = item.Date;
                            assetfreeofcost.Description = item.Description;
                            assetfreeofcost.Uom = item.Uom;
                            assetfreeofcost.CreatedDate = istDate;
                            assetfreeofcost.Companyid = companyid;
                            assetfreeofcost.CreatedUserId = userid;
                            db.Assetfreeofcosts.Add(assetfreeofcost);
                            db.SaveChanges();

                        }
                    }
                    //////update id lastlocation in asset table and costcenter id in asset table
                    var locid = db.childlocations.Where(x => x.AssetID == assetid && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                    var costid = db.childcostcenters.Where(x => x.Ass_ID == assetid && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                    Assets assetidupdate = new Assets();
                    assetidupdate = db.Assetss.Where(x => x.ID == assetid && x.Companyid == companyid).FirstOrDefault();
                    if (locid != null)
                    {
                        assetidupdate.LocAID = locid.ALocID;
                        assetidupdate.LocBID = locid.BLocID;
                        assetidupdate.LocCID = locid.CLocID;

                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        assetidupdate.LocAID = 0;
                        assetidupdate.LocBID = 0;
                        assetidupdate.LocCID = 0;

                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (costid != null)
                    {
                        assetidupdate.CostCenterAID = costid.AcostcenterID;
                        assetidupdate.CostCenterBID = costid.BcostcenterID;
                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    else
                    {
                        assetidupdate.CostCenterAID = 0;
                        assetidupdate.CostCenterBID = 0;
                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
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
                    res.Data = "Something went wrong";
                    return res;
                }

            }


        }
        [HttpGet]
        public ActionResult checkadditionentry(decimal amtcap, string assetno)
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
            var checkamtcap_asset = db.Assetss.Where(x => x.AssetNo == assetno && x.Companyid == companyid).FirstOrDefault().AmountCapitalisedCompany;
            if (checkamtcap_asset != null)
            {
                decimal? calculatedamt = checkamtcap_asset / 100;
                if (amtcap > calculatedamt)
                {
                    res.Data = "amtisgreater";
                }
                else
                {

                }
            }
            else
            {
                res.Data = "Noamtfound";
            }



            return Json(res, JsonRequestBehavior.AllowGet);
        }



        public ActionResult EditAsset(string id)
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





            AssetGroupViewmodel assetGroupViewmodel = new AssetGroupViewmodel();
            Assets assets = new Assets();



            //binding asset values to assetgroupviewmodel
            //  ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
            var assetid = db.Assetss.Where(x => x.AssetNo == id && x.Companyid == companyid).FirstOrDefault().ID;

            assets = db.Assetss.Where(x => x.ID == assetid && x.Companyid == companyid).FirstOrDefault();
            assets.str_VoucherDate = Convert.ToDateTime(assets.VoucherDate).ToString("yyyy-MM-dd");

            DateTime checkvoucherdate;// = DateTime.Parse(assets.str_VoucherDate);
            if (DateTime.TryParseExact(assets.str_VoucherDate, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out checkvoucherdate))
            {
                string checklockflag = ImportDatevalidation(checkvoucherdate, companyid);
                if (checklockflag == "Depalreadycalculated")
                {
                    ViewBag.Lock = "Depalreadycalculated";
                }
                if (checklockflag == "Yes")
                {
                    ViewBag.Lock = "Periodlock";
                }
                if (checklockflag == "No")
                {
                    ViewBag.Lock = "Nolock";
                }
                if (checklockflag == "Nomainperiod")
                {
                    ViewBag.Lock = "Nomainperiod";
                }
            }


            List<Depreciation> deplist = new List<Depreciation>();
            deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == assets.ID).ToList();
            if (deplist.Count() != 0)
            {
                ViewBag.Lock = "Depalreadycalculated";
            }


            if ((assets.AGroupID != null || assets.AGroupID != 0) && (assets.BGroupID == 0)
                && (assets.CGroupID == 0) && (assets.DGroupID == 0))
            {
                ViewBag.groupname = db.AGroups.Where(x => x.ID == assets.AGroupID && x.Companyid == companyid).FirstOrDefault().AGroupName;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }
            if ((assets.AGroupID != 0) && (assets.BGroupID != 0)
                && (assets.CGroupID == 0) && (assets.DGroupID == 0))
            //if (assets.AGroupID != null && assets.BGroupID != null && assets.CGroupID == null && assets.DGroupID == null)
            {
                ViewBag.groupname = db.BGroups.Where(x => x.ID == assets.BGroupID && x.Companyid == companyid).FirstOrDefault().BGroupName;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID && x.BGroupID == assets.BGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }
            if ((assets.AGroupID != 0) && (assets.BGroupID != 0)
                && (assets.CGroupID != 0) && (assets.DGroupID == 0))
            //  if (assets.AGroupID != null && assets.BGroupID !=null && assets.CGroupID != null && assets.DGroupID == null)
            {
                // ViewBag.groupname = assets.CGroupID;
                ViewBag.groupname = db.CGroups.Where(x => x.ID == assets.CGroupID && x.Companyid == companyid).FirstOrDefault().CGroupName;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID && x.BGroupID == assets.BGroupID && x.CGroupID == assets.CGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }
            if ((assets.AGroupID != 0) && (assets.BGroupID != 0) &&
                (assets.CGroupID != 0) && (assets.DGroupID != 0))
            //if (assets.AGroupID != null && assets.BGroupID != null && assets.CGroupID !=null && assets.DGroupID != null)
            {
                // ViewBag.groupname = assets.DGroupID;
                ViewBag.groupname = db.DGroups.Where(x => x.ID == assets.DGroupID && x.Companyid == companyid).FirstOrDefault().DGroupName;
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID && x.BGroupID == assets.BGroupID && x.CGroupID == assets.CGroupID && x.DGroupID == assets.DGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }







            assetGroupViewmodel.ID = assets.ID;
            assetGroupViewmodel.AssetName = assets.AssetName;

            assetGroupViewmodel.VoucherNo = assets.VoucherNo;
            if (assets.DtPutToUse == null)
            {
                assetGroupViewmodel.str_DtPutToUse = "";
            }
            else
            {


                assetGroupViewmodel.str_DtPutToUse = Convert.ToDateTime(assets.DtPutToUse).ToString("dd/MM/yyyy");

            }
            if (assets.DtPutToUseIT == null)
            {
                assetGroupViewmodel.str_DtPutToUseIT = "";
            }
            else
            {


                assetGroupViewmodel.str_DtPutToUseIT = Convert.ToDateTime(assets.DtPutToUseIT).ToString("dd/MM/yyyy");

            }


            if (assets.BillDate == null)
            {
                assetGroupViewmodel.str_BillDate = "";
            }
            else
            {

                assetGroupViewmodel.str_BillDate = Convert.ToDateTime(assets.BillDate).ToString("dd/MM/yyyy");

            }
            if (assets.ReceiptDate == null)
            {
                assetGroupViewmodel.str_ReceiptDate = "";
            }
            else
            {

                assetGroupViewmodel.str_ReceiptDate = Convert.ToDateTime(assets.ReceiptDate).ToString("dd/MM/yyyy");

            }

            if (assets.VoucherDate == null)
            {
                assetGroupViewmodel.str_VoucherDate = "";
            }
            else
            {

                assetGroupViewmodel.str_VoucherDate = Convert.ToDateTime(assets.VoucherDate).ToString("dd/MM/yyyy");

            }

            if (assets.CommissioningDate == null)
            {
                assetGroupViewmodel.str_CommissioningDate = "";
            }
            else
            {

                assetGroupViewmodel.str_CommissioningDate = Convert.ToDateTime(assets.CommissioningDate).ToString("dd/MM/yyyy");

            }
            if (assets.ExpiryDate == null)
            {
                assetGroupViewmodel.str_Expirydate = "";
            }
            else
            {

                assetGroupViewmodel.str_Expirydate = Convert.ToDateTime(assets.ExpiryDate).ToString("dd/MM/yyyy");

            }
            if (assets.PODate == null)
            {
                assetGroupViewmodel.str_PODate = "";
            }
            else
            {

                assetGroupViewmodel.str_PODate = Convert.ToDateTime(assets.PODate).ToString("dd/MM/yyyy");

            }

            assetGroupViewmodel.AssetName = assets.AssetName;
            assetGroupViewmodel.ID = assets.ID;
            assetGroupViewmodel.AssetNo = assets.AssetNo;
            assetGroupViewmodel.AssetIdentificationNo = assets.AssetIdentificationNo;
            assetGroupViewmodel.PONo = assets.PONo;
            assetGroupViewmodel.BillNo = assets.BillNo;
            assetGroupViewmodel.Qty = assets.Qty;
            assetGroupViewmodel.SupplierNo = assets.SupplierNo;
            assetGroupViewmodel.OPAccDepreciation = assets.OPAccDepreciation;
            assetGroupViewmodel.UOMNo = assets.UOMNo;
            assetGroupViewmodel.GrossVal = assets.GrossVal;
            assetGroupViewmodel.Discount = assets.Discount;
            assetGroupViewmodel.DutyDrawback = assets.DutyDrawback;
            assetGroupViewmodel.ServiceCharges = assets.ServiceCharges;
            assetGroupViewmodel.Roundingoff = assets.Roundingoff;
            assetGroupViewmodel.ExciseCredit = assets.ExciseCredit;
            assetGroupViewmodel.OtherExp = assets.OtherExp;
            assetGroupViewmodel.TotDeduction = assets.TotDeduction;
            assetGroupViewmodel.ServiceTaxCredit = assets.ServiceTaxCredit;
            assetGroupViewmodel.CustomDuty = assets.CustomDuty;
            assetGroupViewmodel.InvoiceAmt = assets.InvoiceAmt;
            assetGroupViewmodel.AnyOtherDutyCredit = assets.AnyOtherDutyCredit;
            assetGroupViewmodel.ExciseDuty = assets.ExciseDuty;
            assetGroupViewmodel.VATCredit = assets.VATCredit;
            assetGroupViewmodel.ServiceTax = assets.ServiceTax;
            assetGroupViewmodel.CSTCredit = assets.CSTCredit;
            assetGroupViewmodel.AnyOtherDuty = assets.AnyOtherDuty;
            assetGroupViewmodel.CGSTCredit = assets.CGSTCredit;
            assetGroupViewmodel.VAT = assets.VAT;
            assetGroupViewmodel.SGSTCredit = assets.SGSTCredit;
            assetGroupViewmodel.CSt = assets.CSt;
            assetGroupViewmodel.IGSTCredit = assets.IGSTCredit;
            assetGroupViewmodel.CGST = assets.CGST;
            assetGroupViewmodel.AnyOtherCredit = assets.AnyOtherCredit;
            assetGroupViewmodel.AnyOtherTax = assets.AnyOtherTax;
            assetGroupViewmodel.SGST = assets.SGST;
            assetGroupViewmodel.TotalCredit = assets.TotalCredit;
            assetGroupViewmodel.IGST = assets.IGST;
            assetGroupViewmodel.AmountCapitalised = assets.AmountCapitalised;
            assetGroupViewmodel.TotalAddition = assets.TotalAddition;
            assetGroupViewmodel.AmountCapitalisedCompany = assets.AmountCapitalisedCompany;
            assetGroupViewmodel.AmountCApitalisedIT = assets.AmountCApitalisedIT;
            ////////////////////////////////////////////////////////////
            assetGroupViewmodel.Model = assets.Model;
            assetGroupViewmodel.BrandName = assets.BrandName;
            assetGroupViewmodel.SrNo = assets.SrNo;
            assetGroupViewmodel.Remarks = assets.Remarks;
            assetGroupViewmodel.IsImported = assets.IsImported;
            assetGroupViewmodel.Currency = assets.Currency;
            assetGroupViewmodel.Values = assets.Values;
            //////////////////////////////////////////////////////////////
            assetGroupViewmodel.NormalRatae = assets.NormalRatae;
            assetGroupViewmodel.AdditionalRate = assets.AdditionalRate;
            assetGroupViewmodel.TotalRate = assets.TotalRate;
            assetGroupViewmodel.AccountID = assets.AccountID;
            assetGroupViewmodel.AccAccountID = assets.AccAccountID;
            assetGroupViewmodel.DepAccountId = assets.DepAccountId;
            assetGroupViewmodel.ITGroupIDID = assets.ITGroupIDID;
            assetGroupViewmodel.AmountCApitalisedIT = assets.AmountCApitalisedIT;
            assetGroupViewmodel.Usefullife = assets.Usefullife;
            assetGroupViewmodel.YrofManufacturing = assets.YrofManufacturing;
            assetGroupViewmodel.DepreciationMethod = assets.DepreciationMethod;
            if (assets.Parent_AssetId == 0 || assets.Parent_AssetId == null)
            {
                assetGroupViewmodel.Parent_assetno = "0";
            }
            else
            {
                assetGroupViewmodel.Parent_assetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == assets.Parent_AssetId).FirstOrDefault().AssetNo;
            }
            //  assetGroupViewmodel.IsImported = assets.IsImported;
            //assetGroupViewmodel.Currency = assets.Currency;
            //assetGroupViewmodel.Values = assets.Values;
            // assetGroupViewmodel=db.
            ////////////////////////////////////////////////////
            //locationtable binding
            List<Childlocation> cllist = new List<Childlocation>();
            int locsrno = 1;
            cllist = db.childlocations.Where(x => x.AssetID == assetid && x.Companyid == companyid).ToList();
            foreach (Childlocation item in cllist)
            {
                item.Srno = locsrno;
                if (item.Date == null)
                {

                    item.str_date = "";
                }
                else
                {
                    item.str_date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                }
                if (item.ALocID == 0)
                {

                    item.str_locaname = "";
                }
                else
                {
                    var obj_ALocation = db.ALocations.Where(x => x.ID == item.ALocID && x.Companyid == companyid).FirstOrDefault();
                    if (obj_ALocation != null)
                    {
                        item.str_locaname = obj_ALocation.ALocationName;
                    }
                    else { item.str_locaname = ""; }

                }
                if (item.BLocID == 0)
                {

                    item.str_locbname = "";
                }
                else
                {
                    var obj_BLocation = db.BLocations.Where(x => x.ID == item.BLocID && x.ALocID == item.ALocID && x.Companyid == companyid).FirstOrDefault();
                    if (obj_BLocation != null)
                    {
                        item.str_locbname = obj_BLocation.BLocationName;
                    } else { item.str_locbname = ""; }

                }
                if (item.CLocID == 0 || item.CLocID == null)
                {

                    item.str_loccname = "";
                }
                else
                {
                    var obj_CLocation = db.CLocations.Where(x => x.ALocID == item.ALocID && x.BLocID == item.BLocID && x.ID == item.CLocID && x.Companyid == companyid).FirstOrDefault();
                    if (obj_CLocation != null)
                    {
                        item.str_loccname = obj_CLocation.CLocationName;
                    }
                    else { item.str_loccname = ""; }

                }
                locsrno++;
            }
            //////////////////////////////////////////
            //costcenter
            assetGroupViewmodel.locationlist = cllist;
            List<Childcostcenter> cclist = new List<Childcostcenter>();
            int ccsrno = 1;
            cclist = db.childcostcenters.Where(x => x.Ass_ID == assetid && x.Companyid == companyid).ToList();
            foreach (Childcostcenter item in cclist)
            {
                item.Srno = ccsrno;
                if (item.Date == null)
                {

                    item.str_date = "";
                }
                else
                {
                    item.str_date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                }
                if (item.AcostcenterID == 0)
                {

                    item.str_costcenteraname = "";
                }
                else
                {
                    item.str_costcenteraname = db.ACostCenters.Where(x => x.ID == item.AcostcenterID && x.Companyid == companyid).FirstOrDefault().CCDescription;
                }
                if (item.BcostcenterID == 0)
                {

                    item.str_costcenterbname = "";
                }
                else
                {
                    item.str_costcenterbname = db.BCostCenters.Where(x => x.ID == item.BcostcenterID && x.CCID == item.AcostcenterID && x.Companyid == companyid).FirstOrDefault().SCCDescription;
                }

                ccsrno++;
            }
            assetGroupViewmodel.costcenterlist = cclist;
            ///////////////////////////////////////////////////////////////////////////
            ///assetf
            ///
            List<Assetfreeofcost> alist = new List<Assetfreeofcost>();
            int assrno = 1;
            alist = db.Assetfreeofcosts.Where(x => x.Asset_id == assetid && x.Companyid == companyid).ToList();
            foreach (Assetfreeofcost item in alist)
            {
                item.Srno = assrno;
                if (item.Date == null)
                {

                    item.str_date = "";
                }
                else
                {
                    item.str_date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                }
                if (item.Uom == null)
                {

                    item.str_uomname = "";
                }
                else
                {
                    item.str_uomname = db.UOMs.Where(x => x.ID == item.Uom && x.Companyid == companyid).FirstOrDefault().Unit;
                }
                assrno++;
            }

            assetGroupViewmodel.assetfreeofcostlist = alist;
            ///////////////////////////
            ViewBag.supplierlist = new SelectList(db.Suppliers.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "SupplierName");
            ViewBag.uomlist = new SelectList(db.UOMs.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "Unit");
            ViewBag.itgrouplist = new SelectList(db.ITGroups.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "GroupName");
            ViewBag.purchaselist = new SelectList(db.Accounts.Where(x => x.GroupName == "Asset" && x.Companyid == companyid).OrderBy(e => e.AccountCode), "AccountCode", "AccountName");
            ViewBag.depreciationlist = new SelectList(db.Accounts.Where(x => x.GroupName == "Expense" && x.Companyid == companyid).OrderBy(e => e.AccountCode), "AccountCode", "AccountName");
            ViewBag.accumulatedlist = new SelectList(db.Accounts.Where(x => x.GroupName == "Asset" || x.GroupName == "Liability" && x.Companyid == companyid).OrderBy(e => e.AccountCode), "AccountCode", "AccountName");
            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");
            ViewBag.costalist = new SelectList(db.ACostCenters.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "CCDescription");

            List<SubInsurance> subinsurancelist = new List<SubInsurance>();
            subinsurancelist = db.SubInsurances.Where(x => x.AssetId == assetid && x.Companyid == companyid).ToList();
            foreach (SubInsurance item in subinsurancelist)
            {
                var fromdate = db.Insurances.Where(x => x.ID == item.InsuranceId && x.Companyid == companyid).FirstOrDefault().FromDate;
                //var assetno = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //item.AssetNo = assetno;
                var todate = db.Insurances.Where(x => x.ID == item.InsuranceId && x.Companyid == companyid).FirstOrDefault().ToDate;
                item.policydetails = db.Insurances.Where(x => x.ID == item.InsuranceId && x.Companyid == companyid).FirstOrDefault().PolicyDetails;
                if (fromdate == null)
                {
                    item.str_fromdate = "";
                }
                else
                {
                    item.str_fromdate = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                }
                if (todate == null)
                {
                    item.str_todate = "";
                }
                else
                {
                    item.str_todate = Convert.ToDateTime(todate).ToString("dd/MM/yyyy");
                }
            }
            assetGroupViewmodel.insurancelist = subinsurancelist;
            List<SubAmc> amclist = new List<SubAmc>();
            amclist = db.SubAmc.Where(x => x.AssetId == assetid && x.Companyid == companyid).ToList();
            foreach (SubAmc item in amclist)
            {
                //var assetno = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //item.AssetNo = assetno;
                var fromdate = db.AMCss.Where(x => x.ID == item.AmcId && x.Companyid == companyid).FirstOrDefault().FromDate;
                var todate = db.AMCss.Where(x => x.ID == item.AmcId && x.Companyid == companyid).FirstOrDefault().ToDate;
                item.amcdetails = db.AMCss.Where(x => x.ID == item.AmcId && x.Companyid == companyid).FirstOrDefault().AMCDetails;
                if (fromdate == null)
                {
                    item.str_fromdate = "";
                }
                else
                {
                    item.str_fromdate = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                }
                if (todate == null)
                {
                    item.str_todate = "";
                }
                else
                {
                    item.str_todate = Convert.ToDateTime(todate).ToString("dd/MM/yyyy");
                }
            }

            assetGroupViewmodel.amclist = amclist;
            List<SubLoan> loanlist = new List<SubLoan>();
            assetGroupViewmodel.loanlist = loanlist;
            loanlist = db.SubLoans.Where(x => x.AssetId == assetid && x.Companyid == companyid).ToList();
            foreach (SubLoan item in loanlist)
            {
                //var assetno = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //item.AssetNo = assetno;
                var fromdate = db.Loans.Where(x => x.ID == item.LoanId && x.Companyid == companyid).FirstOrDefault().FromDate;
                var todate = db.Loans.Where(x => x.ID == item.LoanId && x.Companyid == companyid).FirstOrDefault().ToDate;
                item.loandetails = db.Loans.Where(x => x.ID == item.LoanId && x.Companyid == companyid).FirstOrDefault().BankName;
                if (fromdate == null)
                {
                    item.str_fromdate = "";
                }
                else
                {
                    item.str_fromdate = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                }
                if (todate == null)
                {
                    item.str_todate = "";
                }
                else
                {
                    item.str_todate = Convert.ToDateTime(todate).ToString("dd/MM/yyyy");
                }
            }
            assetGroupViewmodel.loanlist = loanlist;




            return View(assetGroupViewmodel);


        }

        [AuthUser]
        [HttpGet]
        public ActionResult Edit(string id)

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




            ///    levelid is not used bcoz in edit we can get groupid from table itself


            //string Level = "";
            //levelid = "L1-1";
            //Level = levelid.Substring(0, 2);
            //int tempLength = levelid.Length;
            //string strr_id = levelid.Substring(3, tempLength - 3);
            //int int_id = Convert.ToInt32(strr_id);
            //if (Level == "L1") // selected parent level
            //{

            //    ViewBag.groupname = db.AGroups.Where(x => x.ID == int_id).FirstOrDefault().AGroupName;

            //}
            //if (Level == "L2") // selected parent level
            //{
            //    BGroup bgroup = new BGroup();
            //    var agroupid = db.BGroups.Where(x => x.ID == int_id).FirstOrDefault().AGrpID;
            //    ViewBag.groupname = db.BGroups.Where(x => x.AGrpID == agroupid && x.ID == int_id).FirstOrDefault().BGroupName;


            //}

            //if (Level == "L3") // selected parent level
            //{


            //    var agroupid = db.CGroups.Where(x => x.ID == int_id).FirstOrDefault().AGrpID;
            //    var bgroupid = db.CGroups.Where(x => x.ID == int_id).FirstOrDefault().BGrpID;
            //    ViewBag.groupname = db.CGroups.Where(x => x.AGrpID == agroupid && x.BGrpID == bgroupid && x.ID == int_id).FirstOrDefault().CGroupName;
            //}
            //if (Level == "L4") // selected parent level
            //{


            //    var agroupid = db.DGroups.Where(x => x.ID == int_id).FirstOrDefault().AGrpID;
            //    var bgroupid = db.DGroups.Where(x => x.ID == int_id).FirstOrDefault().BGrpID;
            //    var cgroupid = db.DGroups.Where(x => x.ID == int_id).FirstOrDefault().CGrpID;

            //    ViewBag.groupname = db.DGroups.Where(x => x.AGrpID == agroupid && x.BGrpID == bgroupid && x.CGrpID == cgroupid && x.ID == int_id).FirstOrDefault().DGroupName;
            //}



            AssetGroupViewmodel assetGroupViewmodel = new AssetGroupViewmodel();
            Assets assets = new Assets();

            string AGroupName = "";
            string BGroupName = "";
            string CGroupName = "";
            string DGroupName = "";


            //binding asset values to assetgroupviewmodel
            //  ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid).OrderBy(e => e.AssetNo), "AssetNo", "AssetName");
            var assetid = db.Assetss.Where(x => x.AssetNo == id && x.Companyid == companyid).FirstOrDefault().ID;

            assets = db.Assetss.Where(x => x.ID == assetid && x.Companyid == companyid).FirstOrDefault();
            assets.str_VoucherDate = Convert.ToDateTime(assets.VoucherDate).ToString("yyyy-MM-dd");

            DateTime checkvoucherdate;// = DateTime.Parse(assets.str_VoucherDate);
            if (DateTime.TryParseExact(assets.str_VoucherDate, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out checkvoucherdate))
            {
                string checklockflag = ImportDatevalidation(checkvoucherdate, companyid);
                if (checklockflag == "Depalreadycalculated")
                {
                    ViewBag.Lock = "Depalreadycalculated";
                }
                if (checklockflag == "Yes")
                {
                    ViewBag.Lock = "Periodlock";
                }
                if (checklockflag == "No")
                {
                    ViewBag.Lock = "Nolock";
                }
                if (checklockflag == "Nomainperiod")
                {
                    ViewBag.Lock = "Nomainperiod";
                }
            }


            List<Depreciation> deplist = new List<Depreciation>();
            deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == assets.ID).ToList();
            if (deplist.Count() != 0)
            {
                ViewBag.Lock = "Depalreadycalculated";
            }


            if (assets.AGroupID > 0)
            {
                AGroup a_group = db.AGroups.Where(x => x.Companyid == companyid && x.ID == assets.AGroupID).FirstOrDefault();
                if (a_group != null)
                {
                    AGroupName = a_group.AGroupName;
                }
            }


            if (assets.BGroupID > 0)
            {
                BGroup b_group = db.BGroups.Where(x => x.Companyid == companyid && x.ID == assets.BGroupID).FirstOrDefault();
                if (b_group != null)
                {
                    BGroupName = b_group.BGroupName;
                }
            }

            if (assets.CGroupID > 0)
            {
                CGroup c_group = db.CGroups.Where(x => x.Companyid == companyid && x.ID == assets.CGroupID).FirstOrDefault();

                if (c_group != null)
                {
                    CGroupName = c_group.CGroupName;
                }
            }

            if (assets.DGroupID > 0)
            {
                DGroup d_group = db.DGroups.Where(x => x.Companyid == companyid && x.ID == assets.DGroupID).FirstOrDefault();
                if (d_group != null)
                {
                    DGroupName = d_group.DGroupName;
                }
            }

            ViewBag.AGroupName = AGroupName;
            ViewBag.BGroupName = BGroupName;
            ViewBag.CGroupName = CGroupName;
            ViewBag.DGroupName = DGroupName;

            if ((assets.AGroupID != null || assets.AGroupID != 0) && (assets.BGroupID == 0)
                && (assets.CGroupID == 0) && (assets.DGroupID == 0))
            {
                AGroup a_Group = null;

                a_Group = db.AGroups.Where(x => x.ID == assets.AGroupID &&
                x.Companyid == companyid).FirstOrDefault();
                if (a_Group != null)
                {
                    ViewBag.groupname = a_Group.AGroupName;
                }
                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }

            if ((assets.AGroupID != 0) && (assets.BGroupID != 0)
                && (assets.CGroupID == 0) && (assets.DGroupID == 0))
            //if (assets.AGroupID != null && assets.BGroupID != null && assets.CGroupID == null && assets.DGroupID == null)
            {
                BGroup b_group = null;
                b_group = db.BGroups.Where(x => x.ID == assets.BGroupID && x.Companyid == companyid).FirstOrDefault();
                if (b_group != null)
                {
                    ViewBag.groupname = b_group.BGroupName;
                }

                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID && x.BGroupID == assets.BGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }
            if ((assets.AGroupID != 0) && (assets.BGroupID != 0)
                && (assets.CGroupID != 0) && (assets.DGroupID == 0))
            //  if (assets.AGroupID != null && assets.BGroupID !=null && assets.CGroupID != null && assets.DGroupID == null)
            {
                // ViewBag.groupname = assets.CGroupID;
                CGroup c_group = null;
                c_group = db.CGroups.Where(x => x.ID == assets.CGroupID && x.Companyid == companyid).FirstOrDefault();

                if (c_group != null)
                {
                    ViewBag.groupname = c_group.CGroupName;
                }


                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID && x.BGroupID == assets.BGroupID && x.CGroupID == assets.CGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }
            if ((assets.AGroupID != 0) && (assets.BGroupID != 0) &&
                (assets.CGroupID != 0) && (assets.DGroupID != 0))
            //if (assets.AGroupID != null && assets.BGroupID != null && assets.CGroupID !=null && assets.DGroupID != null)
            {
                DGroup d_group = null;
                d_group = db.DGroups.Where(x => x.ID == assets.DGroupID && x.Companyid == companyid).FirstOrDefault();
                if (d_group != null)
                {
                    ViewBag.groupname = d_group.DGroupName;
                }
                // ViewBag.groupname = assets.DGroupID;

                ViewBag.Assestlist = new SelectList(db.Assetss.Where(x => x.DisposalFlag == 0 && x.Companyid == companyid && x.AGroupID == assets.AGroupID && x.BGroupID == assets.BGroupID && x.CGroupID == assets.CGroupID && x.DGroupID == assets.DGroupID).OrderBy(e => e.AssetNo), "AssetNo", "assetcode");
            }




            assetGroupViewmodel.ID = assets.ID;
            assetGroupViewmodel.AssetName = assets.AssetName;

            assetGroupViewmodel.VoucherNo = assets.VoucherNo;
            if (assets.DtPutToUse == null)
            {
                assetGroupViewmodel.str_DtPutToUse = "";
            }
            else
            {


                assetGroupViewmodel.str_DtPutToUse = Convert.ToDateTime(assets.DtPutToUse).ToString("dd/MM/yyyy");

            }
            if (assets.DtPutToUseIT == null)
            {
                assetGroupViewmodel.str_DtPutToUseIT = "";
            }
            else
            {


                assetGroupViewmodel.str_DtPutToUseIT = Convert.ToDateTime(assets.DtPutToUseIT).ToString("dd/MM/yyyy");

            }


            if (assets.BillDate == null)
            {
                assetGroupViewmodel.str_BillDate = "";
            }
            else
            {

                assetGroupViewmodel.str_BillDate = Convert.ToDateTime(assets.BillDate).ToString("dd/MM/yyyy");

            }
            if (assets.ReceiptDate == null)
            {
                assetGroupViewmodel.str_ReceiptDate = "";
            }
            else
            {

                assetGroupViewmodel.str_ReceiptDate = Convert.ToDateTime(assets.ReceiptDate).ToString("dd/MM/yyyy");

            }

            if (assets.VoucherDate == null)
            {
                assetGroupViewmodel.str_VoucherDate = "";
            }
            else
            {

                assetGroupViewmodel.str_VoucherDate = Convert.ToDateTime(assets.VoucherDate).ToString("dd/MM/yyyy");

            }

            if (assets.CommissioningDate == null)
            {
                assetGroupViewmodel.str_CommissioningDate = "";
            }
            else
            {

                assetGroupViewmodel.str_CommissioningDate = Convert.ToDateTime(assets.CommissioningDate).ToString("dd/MM/yyyy");

            }
            if (assets.ExpiryDate == null)
            {
                assetGroupViewmodel.str_Expirydate = "";
            }
            else
            {

                assetGroupViewmodel.str_Expirydate = Convert.ToDateTime(assets.ExpiryDate).ToString("dd/MM/yyyy");

            }
            if (assets.PODate == null)
            {
                assetGroupViewmodel.str_PODate = "";
            }
            else
            {

                assetGroupViewmodel.str_PODate = Convert.ToDateTime(assets.PODate).ToString("dd/MM/yyyy");

            }

            assetGroupViewmodel.AssetName = assets.AssetName;
            assetGroupViewmodel.ID = assets.ID;
            assetGroupViewmodel.AssetNo = assets.AssetNo;
            assetGroupViewmodel.AssetIdentificationNo = assets.AssetIdentificationNo;
            assetGroupViewmodel.PONo = assets.PONo;
            assetGroupViewmodel.BillNo = assets.BillNo;
            assetGroupViewmodel.Qty = assets.Qty;
            assetGroupViewmodel.SupplierNo = assets.SupplierNo;
            assetGroupViewmodel.OPAccDepreciation = assets.OPAccDepreciation;
            assetGroupViewmodel.UOMNo = assets.UOMNo;
            assetGroupViewmodel.GrossVal = assets.GrossVal;
            assetGroupViewmodel.Discount = assets.Discount;
            assetGroupViewmodel.DutyDrawback = assets.DutyDrawback;
            assetGroupViewmodel.ServiceCharges = assets.ServiceCharges;
            assetGroupViewmodel.Roundingoff = assets.Roundingoff;
            assetGroupViewmodel.ExciseCredit = assets.ExciseCredit;
            assetGroupViewmodel.OtherExp = assets.OtherExp;
            assetGroupViewmodel.TotDeduction = assets.TotDeduction;
            assetGroupViewmodel.ServiceTaxCredit = assets.ServiceTaxCredit;
            assetGroupViewmodel.CustomDuty = assets.CustomDuty;
            assetGroupViewmodel.InvoiceAmt = assets.InvoiceAmt;
            assetGroupViewmodel.AnyOtherDutyCredit = assets.AnyOtherDutyCredit;
            assetGroupViewmodel.ExciseDuty = assets.ExciseDuty;
            assetGroupViewmodel.VATCredit = assets.VATCredit;
            assetGroupViewmodel.ServiceTax = assets.ServiceTax;
            assetGroupViewmodel.CSTCredit = assets.CSTCredit;
            assetGroupViewmodel.AnyOtherDuty = assets.AnyOtherDuty;
            assetGroupViewmodel.CGSTCredit = assets.CGSTCredit;
            assetGroupViewmodel.VAT = assets.VAT;
            assetGroupViewmodel.SGSTCredit = assets.SGSTCredit;
            assetGroupViewmodel.CSt = assets.CSt;
            assetGroupViewmodel.IGSTCredit = assets.IGSTCredit;
            assetGroupViewmodel.CGST = assets.CGST;
            assetGroupViewmodel.AnyOtherCredit = assets.AnyOtherCredit;
            assetGroupViewmodel.AnyOtherTax = assets.AnyOtherTax;
            assetGroupViewmodel.SGST = assets.SGST;
            assetGroupViewmodel.TotalCredit = assets.TotalCredit;
            assetGroupViewmodel.IGST = assets.IGST;
            assetGroupViewmodel.AmountCapitalised = assets.AmountCapitalised;
            assetGroupViewmodel.TotalAddition = assets.TotalAddition;
            assetGroupViewmodel.AmountCapitalisedCompany = assets.AmountCapitalisedCompany;
            assetGroupViewmodel.AmountCApitalisedIT = assets.AmountCApitalisedIT;
            assetGroupViewmodel.ResidualVal = assets.ResidualVal;

            ////////////////////////////////////////////////////////////
            assetGroupViewmodel.Model = assets.Model;
            assetGroupViewmodel.BrandName = assets.BrandName;
            assetGroupViewmodel.SrNo = assets.SrNo;
            assetGroupViewmodel.Remarks = assets.Remarks;
            assetGroupViewmodel.IsImported = assets.IsImported;
            assetGroupViewmodel.Currency = assets.Currency;
            assetGroupViewmodel.Values = assets.Values;
            //////////////////////////////////////////////////////////////
            assetGroupViewmodel.NormalRatae = assets.NormalRatae;
            assetGroupViewmodel.AdditionalRate = assets.AdditionalRate;
            assetGroupViewmodel.TotalRate = assets.TotalRate;
            assetGroupViewmodel.AccountID = assets.AccountID;
            assetGroupViewmodel.AccAccountID = assets.AccAccountID;
            assetGroupViewmodel.DepAccountId = assets.DepAccountId;
            assetGroupViewmodel.ITGroupIDID = assets.ITGroupIDID;
            assetGroupViewmodel.AmountCApitalisedIT = assets.AmountCApitalisedIT;
            assetGroupViewmodel.Usefullife = assets.Usefullife;
            assetGroupViewmodel.YrofManufacturing = assets.YrofManufacturing;
            assetGroupViewmodel.DepreciationMethod = assets.DepreciationMethod;
            if (assets.Parent_AssetId == 0 || assets.Parent_AssetId == null)
            {
                assetGroupViewmodel.Parent_assetno = "0";
            }
            else
            {
                assetGroupViewmodel.Parent_assetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == assets.Parent_AssetId).FirstOrDefault().AssetNo;
            }
            //  assetGroupViewmodel.IsImported = assets.IsImported;
            //assetGroupViewmodel.Currency = assets.Currency;
            //assetGroupViewmodel.Values = assets.Values;
            // assetGroupViewmodel=db.
            ////////////////////////////////////////////////////
            //locationtable binding
            List<Childlocation> cllist = new List<Childlocation>();
            int locsrno = 1;
            cllist = db.childlocations.Where(x => x.AssetID == assetid && x.Companyid == companyid).ToList();
            foreach (Childlocation item in cllist)
            {
                item.Srno = locsrno;
                if (item.Date == null)
                {

                    item.str_date = "";
                }
                else
                {
                    item.str_date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                }
                if (item.ALocID == 0)
                {

                    item.str_locaname = "";
                }
                else
                {
                    item.str_locaname = db.ALocations.Where(x => x.ID == item.ALocID && x.Companyid == companyid).FirstOrDefault().ALocationName;
                }
                if (item.BLocID == 0 || item.BLocID is null)
                {

                    item.str_locbname = "";
                }
                else
                {
                    item.str_locbname = db.BLocations.Where(x => x.ID == item.BLocID && x.ALocID == item.ALocID && x.Companyid == companyid).FirstOrDefault().BLocationName;
                }
                if (item.CLocID == 0 || item.CLocID == null)
                {

                    item.str_loccname = "";
                }
                else
                {
                    item.str_loccname = db.CLocations.Where(x => x.ALocID == item.ALocID && x.BLocID == item.BLocID && x.ID == item.CLocID && x.Companyid == companyid).FirstOrDefault().CLocationName;
                }
                locsrno++;
            }
            //////////////////////////////////////////
            //costcenter
            assetGroupViewmodel.locationlist = cllist;
            List<Childcostcenter> cclist = new List<Childcostcenter>();
            int ccsrno = 1;
            cclist = db.childcostcenters.Where(x => x.Ass_ID == assetid && x.Companyid == companyid).ToList();
            foreach (Childcostcenter item in cclist)
            {
                item.Srno = ccsrno;
                if (item.Date == null)
                {

                    item.str_date = "";
                }
                else
                {
                    item.str_date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                }
                if (item.AcostcenterID == 0)
                {

                    item.str_costcenteraname = "";
                }
                else
                {
                    item.str_costcenteraname = db.ACostCenters.Where(x => x.ID == item.AcostcenterID && x.Companyid == companyid).FirstOrDefault().CCDescription;
                }
                if (item.BcostcenterID == 0)
                {

                    item.str_costcenterbname = "";
                }
                else
                {
                    item.str_costcenterbname = db.BCostCenters.Where(x => x.ID == item.BcostcenterID && x.CCID == item.AcostcenterID && x.Companyid == companyid).FirstOrDefault().SCCDescription;
                }

                ccsrno++;
            }
            assetGroupViewmodel.costcenterlist = cclist;
            ///////////////////////////////////////////////////////////////////////////
            ///assetf
            ///
            List<Assetfreeofcost> alist = new List<Assetfreeofcost>();
            int assrno = 1;
            alist = db.Assetfreeofcosts.Where(x => x.Asset_id == assetid && x.Companyid == companyid).ToList();
            foreach (Assetfreeofcost item in alist)
            {
                item.Srno = assrno;
                if (item.Date == null)
                {

                    item.str_date = "";
                }
                else
                {
                    item.str_date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                }
                if (item.Uom == null)
                {

                    item.str_uomname = "";
                }
                else
                {
                    item.str_uomname = db.UOMs.Where(x => x.ID == item.Uom && x.Companyid == companyid).FirstOrDefault().Unit;
                }
                assrno++;
            }

            assetGroupViewmodel.assetfreeofcostlist = alist;
            ///////////////////////////
            ViewBag.supplierlist = new SelectList(db.Suppliers.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "SupplierName");
            ViewBag.uomlist = new SelectList(db.UOMs.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "Unit");
            ViewBag.itgrouplist = new SelectList(db.ITGroups.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "GroupName");
            ViewBag.purchaselist = new SelectList(db.Accounts.Where(x => x.GroupName == "Asset" && x.Companyid == companyid).OrderBy(e => e.AccountCode), "AccountCode", "AccountName");
            ViewBag.depreciationlist = new SelectList(db.Accounts.Where(x => x.GroupName == "Expense" && x.Companyid == companyid).OrderBy(e => e.AccountCode), "AccountCode", "AccountName");
            ViewBag.accumulatedlist = new SelectList(db.Accounts.Where(x => x.GroupName == "Asset" || x.GroupName == "Liability" && x.Companyid == companyid).OrderBy(e => e.AccountCode), "AccountCode", "AccountName");
            ViewBag.alocationlist = new SelectList(db.ALocations.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "ALocationName");
            ViewBag.blocationlist = new SelectList("", "ID", "BLocationName");
            ViewBag.clocationlist = new SelectList("", "ID", "CLocationName");
            ViewBag.costalist = new SelectList(db.ACostCenters.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "CCDescription");

            List<SubInsurance> subinsurancelist = new List<SubInsurance>();
            subinsurancelist = db.SubInsurances.Where(x => x.AssetId == assetid && x.Companyid == companyid).ToList();
            foreach (SubInsurance item in subinsurancelist)
            {
                var fromdate = db.Insurances.Where(x => x.ID == item.InsuranceId && x.Companyid == companyid).FirstOrDefault().FromDate;
                //var assetno = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //item.AssetNo = assetno;
                var todate = db.Insurances.Where(x => x.ID == item.InsuranceId && x.Companyid == companyid).FirstOrDefault().ToDate;
                item.policydetails = db.Insurances.Where(x => x.ID == item.InsuranceId && x.Companyid == companyid).FirstOrDefault().PolicyDetails;
                if (fromdate == null)
                {
                    item.str_fromdate = "";
                }
                else
                {
                    item.str_fromdate = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                }
                if (todate == null)
                {
                    item.str_todate = "";
                }
                else
                {
                    item.str_todate = Convert.ToDateTime(todate).ToString("dd/MM/yyyy");
                }
            }
            assetGroupViewmodel.insurancelist = subinsurancelist;
            List<SubAmc> amclist = new List<SubAmc>();
            amclist = db.SubAmc.Where(x => x.AssetId == assetid && x.Companyid == companyid).ToList();
            foreach (SubAmc item in amclist)
            {
                //var assetno = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //item.AssetNo = assetno;
                var fromdate = db.AMCss.Where(x => x.ID == item.AmcId && x.Companyid == companyid).FirstOrDefault().FromDate;
                var todate = db.AMCss.Where(x => x.ID == item.AmcId && x.Companyid == companyid).FirstOrDefault().ToDate;
                item.amcdetails = db.AMCss.Where(x => x.ID == item.AmcId && x.Companyid == companyid).FirstOrDefault().AMCDetails;
                if (fromdate == null)
                {
                    item.str_fromdate = "";
                }
                else
                {
                    item.str_fromdate = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                }
                if (todate == null)
                {
                    item.str_todate = "";
                }
                else
                {
                    item.str_todate = Convert.ToDateTime(todate).ToString("dd/MM/yyyy");
                }
            }

            assetGroupViewmodel.amclist = amclist;
            List<SubLoan> loanlist = new List<SubLoan>();
            assetGroupViewmodel.loanlist = loanlist;
            loanlist = db.SubLoans.Where(x => x.AssetId == assetid && x.Companyid == companyid).ToList();
            foreach (SubLoan item in loanlist)
            {
                //var assetno = db.Assetss.Where(x => x.ID == item.AssetId && x.Companyid == companyid).FirstOrDefault().AssetNo;
                //item.AssetNo = assetno;
                var fromdate = db.Loans.Where(x => x.ID == item.LoanId && x.Companyid == companyid).FirstOrDefault().FromDate;
                var todate = db.Loans.Where(x => x.ID == item.LoanId && x.Companyid == companyid).FirstOrDefault().ToDate;
                item.loandetails = db.Loans.Where(x => x.ID == item.LoanId && x.Companyid == companyid).FirstOrDefault().BankName;
                if (fromdate == null)
                {
                    item.str_fromdate = "";
                }
                else
                {
                    item.str_fromdate = Convert.ToDateTime(fromdate).ToString("dd/MM/yyyy");
                }
                if (todate == null)
                {
                    item.str_todate = "";
                }
                else
                {
                    item.str_todate = Convert.ToDateTime(todate).ToString("dd/MM/yyyy");
                }
            }
            assetGroupViewmodel.loanlist = loanlist;




            return View(assetGroupViewmodel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]
        public ActionResult Edit(AssetGroupViewmodel assetGroup)
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


            AssetRepository assetRepository = new AssetRepository();

            string strMessage = "";
            assetGroup.CompanyID = companyid;
            assetRepository.UpdateAsset(assetGroup, userid, ref strMessage);


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Assets asset = new Assets();
                    assetGroup.str_VoucherDate = Convert.ToDateTime(assetGroup.VoucherDate).ToString("yyyy-MM-dd");

                    DateTime checkvoucherdate;// = DateTime.Parse(assets.str_VoucherDate);
                    if (DateTime.TryParseExact(assetGroup.str_VoucherDate, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out checkvoucherdate))
                    {
                        string checklockflag = ImportDatevalidation(checkvoucherdate, companyid);
                        if (checklockflag == "Depalreadycalculated")
                        {
                            res.Data = "Depreciation already calculated";
                            return res;
                        }
                        if (checklockflag == "Yes")
                        {

                            res.Data = "Periodlock";
                            return res;
                        }
                        if (checklockflag == "No")
                        {

                        }
                        if (checklockflag == "Nomainperiod")
                        {
                            res.Data = "No main period please add period";
                            return res;

                        }
                    }

                    List<Depreciation> deplist = new List<Depreciation>();
                    deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == assetGroup.ID).ToList();
                    if (deplist.Count() != 0)
                    {
                        res.Data = "Depreciation already calculated";
                        return res;
                    }

                    if (assetGroup.VoucherDate > assetGroup.DtPutToUse)
                    {
                        res.Data = "Voucher Date cannot be greater than Dateputtouse";
                        return res;
                    }
                    if (assetGroup.VoucherDate > assetGroup.ExpiryDate)
                    {
                        res.Data = "Voucher Date cannot be greater than ExpiryDate";
                        return res;
                    }
                    DateTime vdate = Convert.ToDateTime(assetGroup.VoucherDate);

                    //this is for opening accumalted depreciation
                    var oppaccdep = str_checkopeningaccumulateddepreciation(vdate);
                    if (oppaccdep == "Yes")
                    {
                        asset.OPAccDepreciation = assetGroup.OPAccDepreciation;
                    }
                    if (oppaccdep == "No")
                    {
                        asset.OPAccDepreciation = 0;
                    }
                    if (oppaccdep == "Noperiod")
                    {
                        res.Data = "No main period please add period";
                        return res;
                    }
                    asset = db.Assetss.Where(x => x.ID == assetGroup.ID && x.Companyid == companyid).FirstOrDefault();

                    asset.AssetNo = assetGroup.AssetNo;
                    asset.AssetName = assetGroup.AssetName;
                    asset.VoucherNo = assetGroup.VoucherNo;
                    string parentassetno = assetGroup.Parent_AssetId.ToString();
                    if (parentassetno == "")
                    {
                        asset.Parent_AssetId = 0;
                    }
                    else
                    {
                        var parentassetid = db.Assetss.Where(x => x.AssetNo == parentassetno).FirstOrDefault().ID;
                        asset.Parent_AssetId = parentassetid;
                    }
                    asset.iscomponent = assetGroup.iscomponent;
                    if (assetGroup.DtPutToUse == null)
                    {
                        asset.DtPutToUse = null;
                    }
                    else
                    {


                        asset.DtPutToUse = assetGroup.DtPutToUse;

                    }
                    if (assetGroup.DtPutToUseIT == null)
                    {
                        asset.DtPutToUseIT = null;
                    }
                    else
                    {


                        asset.DtPutToUseIT = assetGroup.DtPutToUseIT;

                    }


                    if (assetGroup.BillDate == null)
                    {
                        asset.BillDate = null;
                    }
                    else
                    {

                        asset.BillDate = assetGroup.BillDate;

                    }
                    if (assetGroup.ReceiptDate == null)
                    {
                        asset.ReceiptDate = null;
                    }
                    else
                    {

                        asset.ReceiptDate = assetGroup.ReceiptDate;

                    }

                    if (assetGroup.VoucherDate == null)
                    {
                        asset.VoucherDate = null;
                    }
                    else
                    {

                        asset.VoucherDate = assetGroup.VoucherDate;

                    }

                    if (assetGroup.CommissioningDate == null)
                    {
                        asset.CommissioningDate = null;
                    }
                    else
                    {

                        asset.CommissioningDate = assetGroup.CommissioningDate;

                    }
                    if (assetGroup.ExpiryDate == null)
                    {
                        asset.ExpiryDate = null;
                    }
                    else
                    {

                        // asset.ExpiryDate = assetGroup.ExpiryDate;
                        DateTime caldate = Convert.ToDateTime(assetGroup.VoucherDate).AddYears(Convert.ToInt32(assetGroup.Usefullife));
                        if (caldate == assetGroup.ExpiryDate)
                        {
                            asset.ExpiryDate = assetGroup.ExpiryDate;
                        }
                        else
                        {
                            asset.ExpiryDate = caldate;
                        }


                    }


                    asset.AssetName = assetGroup.AssetName;

                    asset.AssetNo = assetGroup.AssetNo;
                    asset.AssetIdentificationNo = assetGroup.AssetIdentificationNo;
                    asset.PONo = assetGroup.PONo;
                    asset.BillNo = assetGroup.BillNo;
                    asset.Qty = assetGroup.Qty;
                    asset.SupplierNo = assetGroup.SupplierNo;
                    //    asset.OPAccDepreciation = assetGroup.OPAccDepreciation;
                    asset.UOMNo = assetGroup.UOMNo;

                    asset.GrossVal = decimalToDecimal(assetGroup.GrossVal);
                    asset.Discount = decimalToDecimal(assetGroup.Discount);
                    asset.DutyDrawback = decimalToDecimal(assetGroup.DutyDrawback);
                    asset.ServiceCharges = decimalToDecimal(assetGroup.ServiceCharges);
                    //  asset.OPAccDepreciation = assetGroup.OPAccDepreciation;
                    asset.Roundingoff = decimalToDecimal(assetGroup.Roundingoff);
                    asset.ExciseCredit = decimalToDecimal(assetGroup.ExciseCredit);
                    asset.OtherExp = decimalToDecimal(assetGroup.OtherExp);
                    asset.TotDeduction = decimalToDecimal(assetGroup.TotDeduction);
                    asset.ServiceTaxCredit = decimalToDecimal(assetGroup.ServiceTaxCredit);
                    asset.CustomDuty = decimalToDecimal(assetGroup.CustomDuty);

                    asset.AnyOtherDutyCredit = decimalToDecimal(assetGroup.AnyOtherDutyCredit);
                    asset.ExciseDuty = decimalToDecimal(assetGroup.ExciseDuty);
                    asset.VATCredit = decimalToDecimal(assetGroup.VATCredit);
                    asset.ServiceTax = decimalToDecimal(assetGroup.ServiceTax);
                    asset.CSTCredit = decimalToDecimal(assetGroup.CSTCredit);
                    asset.AnyOtherDuty = decimalToDecimal(assetGroup.AnyOtherDuty);
                    asset.CGSTCredit = decimalToDecimal(assetGroup.CGSTCredit);
                    asset.VAT = decimalToDecimal(assetGroup.VAT);
                    asset.SGSTCredit = decimalToDecimal(assetGroup.SGSTCredit);
                    asset.CSt = decimalToDecimal(assetGroup.CSt);
                    asset.IGSTCredit = decimalToDecimal(assetGroup.IGSTCredit);
                    asset.CGST = decimalToDecimal(assetGroup.CGST);
                    asset.AnyOtherCredit = decimalToDecimal(assetGroup.AnyOtherCredit);
                    asset.AnyOtherTax = decimalToDecimal(assetGroup.AnyOtherTax);
                    asset.SGST = decimalToDecimal(assetGroup.SGST);
                    asset.IGST = decimalToDecimal(assetGroup.IGST);
                    asset.ResidualVal = decimalToDecimal(assetGroup.ResidualVal);
                    //ajinkya server side total calculation
                    asset.TotalAddition = asset.GrossVal + asset.ServiceCharges + asset.OtherExp + asset.CustomDuty + asset.ExciseDuty + asset.ServiceTax;
                    asset.TotalAddition = asset.TotalAddition + asset.AnyOtherDuty + asset.VAT + asset.CGST + asset.IGST + asset.SGST + asset.CSt + asset.AnyOtherTax;
                    asset.InvoiceAmt = asset.TotalAddition - asset.Discount - asset.Roundingoff - asset.TotDeduction;
                    asset.TotalCredit = asset.DutyDrawback + asset.ExciseCredit + asset.ServiceTaxCredit + asset.AnyOtherDutyCredit + asset.AnyOtherCredit + asset.VATCredit;
                    asset.TotalCredit = asset.TotalCredit + asset.SGSTCredit + asset.CGSTCredit + asset.CSTCredit + asset.IGSTCredit;
                    asset.AmountCapitalised = asset.InvoiceAmt - asset.TotalCredit;
                    asset.AmountCapitalisedCompany = asset.InvoiceAmt - asset.TotalCredit;
                    asset.AmountCApitalisedIT = asset.InvoiceAmt - asset.TotalCredit;
                    ///////////
                    ////////////////////////////////////////////////////////////
                    asset.Model = assetGroup.Model;
                    asset.BrandName = assetGroup.BrandName;
                    asset.SrNo = assetGroup.SrNo;
                    asset.Remarks = assetGroup.Remarks;
                    asset.IsImported = assetGroup.IsImported;
                    asset.Currency = assetGroup.Currency;
                    asset.Values = decimalToDecimal(assetGroup.Values);
                    //////////////////////////////////////////////////////////////
                    asset.NormalRatae = decimalToDecimal(assetGroup.NormalRatae);
                    asset.AdditionalRate = decimalToDecimal(assetGroup.AdditionalRate);
                    asset.TotalRate = asset.NormalRatae + asset.AdditionalRate;
                    /////////////////////////////////////////////////////////////////
                    if (assetGroup.AccountID == null)
                    {
                        asset.AccountID = 0;
                    }
                    else
                    {
                        asset.AccountID = assetGroup.AccountID;
                    }
                    if (assetGroup.DepAccountId == null)
                    {
                        asset.DepAccountId = 0;
                    }
                    else
                    {
                        asset.DepAccountId = assetGroup.DepAccountId;
                    }
                    if (assetGroup.ITGroupIDID == null)
                    {
                        asset.ITGroupIDID = 0;
                    }
                    else
                    {
                        asset.ITGroupIDID = assetGroup.ITGroupIDID;
                    }

                    //  asset.DepAccountId = assetGroup.DepAccountId;
                    // asset.ITGroupIDID = assetGroup.ITGroupIDID;
                    //   asset.AmountCApitalisedIT = assetGroup.AmountCApitalisedIT;
                    asset.Usefullife = assetGroup.Usefullife;
                    asset.YrofManufacturing = assetGroup.YrofManufacturing;
                    asset.DepreciationMethod = assetGroup.DepreciationMethod;
                    asset.ModifiedDate = istDate;
                    asset.Modified_Userid = userid;
                    db.Entry(asset).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ////////////////////////////////////////////////////
                    //locationtable binding
                    List<Childlocation> clist = new List<Childlocation>();
                    clist = db.childlocations.Where(x => x.AssetID == assetGroup.ID && x.Companyid == companyid).ToList();
                    db.childlocations.RemoveRange(clist);
                    db.SaveChanges();

                    Childlocation childlocations = new Childlocation();
                    if (assetGroup.locationlist.Count() != 0)
                    {
                        foreach (var item in assetGroup.locationlist)
                        {
                            if (item.Date == null)
                            {
                                item.Date = null;
                            }
                            childlocations.AssetID = assetGroup.ID;
                            childlocations.Date = item.Date;
                            childlocations.ALocID = item.ALocID;
                            childlocations.BLocID = item.BLocID;
                            childlocations.CLocID = item.CLocID;
                            childlocations.ModifiedDate = istDate;
                            childlocations.Modified_Userid = userid;
                            childlocations.Companyid = companyid;
                            db.childlocations.Add(childlocations);
                            db.SaveChanges();

                        }
                    }
                    List<Childcostcenter> costlist = new List<Childcostcenter>();
                    costlist = db.childcostcenters.Where(x => x.Ass_ID == assetGroup.ID && x.Companyid == companyid).ToList();
                    db.childcostcenters.RemoveRange(costlist);
                    db.SaveChanges();
                    Childcostcenter childcostcenter = new Childcostcenter();
                    if (assetGroup.costcenterlist.Count() != 0)
                    {
                        foreach (var item in assetGroup.costcenterlist)
                        {
                            if (item.Date == null)
                            {
                                item.Date = null;
                            }

                            childcostcenter.Ass_ID = assetGroup.ID;
                            childcostcenter.Date = item.Date;
                            childcostcenter.AcostcenterID = item.AcostcenterID;
                            childcostcenter.BcostcenterID = item.BcostcenterID;
                            childcostcenter.Percentage = item.Percentage;
                            childcostcenter.Modified_Userid = userid;
                            childcostcenter.ModifiedDate = istDate;
                            childcostcenter.Companyid = companyid;
                            db.childcostcenters.Add(childcostcenter);
                            db.SaveChanges();

                        }
                    }
                    List<Assetfreeofcost> assetlist = new List<Assetfreeofcost>();
                    assetlist = db.Assetfreeofcosts.Where(x => x.Asset_id == assetGroup.ID && x.Companyid == companyid).ToList();
                    db.Assetfreeofcosts.RemoveRange(assetlist);
                    db.SaveChanges();
                    Assetfreeofcost assetfreeofcost = new Assetfreeofcost();
                    if (assetGroup.assetfreeofcostlist.Count() != 0)
                    {
                        foreach (var item in assetGroup.assetfreeofcostlist)
                        {
                            if (item.Date == null)
                            {
                                item.Date = null;
                            }
                            // item.Asset_id = asse;
                            assetfreeofcost.Asset_id = assetGroup.ID;
                            assetfreeofcost.Date = item.Date;
                            assetfreeofcost.Description = item.Description;
                            assetfreeofcost.Uom = item.Uom;
                            assetfreeofcost.ModifiedDate = istDate;
                            assetfreeofcost.Modified_Userid = userid;
                            assetfreeofcost.Companyid = companyid;
                            db.Assetfreeofcosts.Add(assetfreeofcost);
                            db.SaveChanges();


                        }
                    }
                    //////update id lastlocation in asset table and costcenter id in asset table
                    var locid = db.childlocations.Where(x => x.AssetID == assetGroup.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                    var costid = db.childcostcenters.Where(x => x.Ass_ID == assetGroup.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                    Assets assetidupdate = new Assets();
                    assetidupdate = db.Assetss.Where(x => x.ID == assetGroup.ID && x.Companyid == companyid).FirstOrDefault();
                    if (locid != null)
                    {
                        assetidupdate.LocAID = locid.ALocID;
                        assetidupdate.LocBID = locid.BLocID;
                        assetidupdate.LocCID = locid.CLocID;

                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        assetidupdate.LocAID = 0;
                        assetidupdate.LocBID = 0;
                        assetidupdate.LocCID = 0;

                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (costid != null)
                    {
                        assetidupdate.CostCenterAID = costid.AcostcenterID;
                        assetidupdate.CostCenterBID = costid.BcostcenterID;
                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        assetidupdate.CostCenterAID = 0;
                        assetidupdate.CostCenterBID = 0;
                        db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
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
                    res.Data = "Something went wrong";
                    return res;
                }

            }
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditAsset(AssetGroupViewmodel assetGroup)
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

            try {

                AssetRepository assetRepository = new AssetRepository();

                string strMessage = "";
                assetGroup.CompanyID = companyid;
                if (assetRepository.UpdateAsset(assetGroup, userid, ref strMessage))
                {
                    res.Data = "Success";
                    return res;
                }
                else
                {
                    res.Data = "Something went wrong";
                    return res;
                }

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                res.Data = "Something went wrong";
                return res;
            }
        }
    



        [HttpGet]
        public ActionResult changegroup(string id)
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



            ViewBag.agrouplist = new SelectList(db.AGroups.Where(x => x.Companyid == companyid).OrderBy(e => e.ID), "ID", "AGroupName");
            ViewBag.bgrouplist = new SelectList("", "ID", "BGroupName");
            ViewBag.cgrouplist = new SelectList("", "ID", "CGroupName");
            ViewBag.dgrouplist = new SelectList("", "ID", "CGroupName");
            var assetid = db.Assetss.Where(x => x.AssetNo == id && x.Companyid == companyid).FirstOrDefault().ID;
            Assets asset = db.Assetss.Where(x => x.ID == assetid && x.Companyid == companyid).FirstOrDefault();
            asset.str_VoucherDate = Convert.ToDateTime(asset.VoucherDate).ToString("yyyy-MM-dd");
            DateTime checkvoucherdate;//= DateTime.Parse(asset .str_VoucherDate);
            if (DateTime.TryParseExact(asset.str_VoucherDate, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out checkvoucherdate))
            {
                string checklockflag = ImportDatevalidation(checkvoucherdate, companyid);
                if (checklockflag == "Depalreadycalculated")
                {
                    ViewBag.Lock = "Depalreadycalculated";
                }
                if (checklockflag == "Yes")
                {
                    ViewBag.Lock = "Periodlock";
                }
                if (checklockflag == "No")
                {
                    ViewBag.Lock = "Nolock";
                }
            }
            List<Depreciation> deplist = new List<Depreciation>();
            deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == asset.ID).ToList();
            if (deplist.Count() != 0)
            {
                ViewBag.Lock = "Depalreadycalculated";
            }
            ViewBag.assetno = asset.AssetNo;
            ViewBag.assetname = asset.AssetName;
            ViewBag.agroupid = asset.AGroupID;
            ViewBag.bgroupid = asset.BGroupID;
            ViewBag.cgroupid = asset.CGroupID;
            ViewBag.dgroupid = asset.DGroupID;
            ViewBag.assetid = asset.ID;
            return PartialView();
        }


        [HttpPost]
        [AllowAnonymous]

        public ActionResult changegroup_post(ChangeGroupViewModel cg)
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



            try
            {

                Assets assets = db.Assetss.Where(x => x.ID == cg.Id && x.Companyid == companyid).FirstOrDefault();
                assets.AGroupID = cg.AgroupID;
                assets.BGroupID = cg.BgroupID;
                assets.CGroupID = cg.CgroupID;
                assets.DGroupID = cg.DgroupID;
                db.Entry(assets).State = System.Data.Entity.EntityState.Modified;
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
        [HttpPost]
        public ActionResult getgroupb(string id)
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

            int int_id = Convert.ToInt32(id);

            List<BGroup> blist = new List<BGroup>();
            blist = db.BGroups.Where(x => x.AGrpID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(blist, "ID", "BGroupName", 0);
            return Json(ob);

        }
        [HttpPost]
        public ActionResult getgroupc(string id)
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
            int int_id = Convert.ToInt32(id);

            List<CGroup> clist = new List<CGroup>();
            clist = db.CGroups.Where(x => x.BGrpID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(clist, "ID", "CGroupName", 0);
            return Json(ob);


        }
        [HttpPost]
        public ActionResult getgroupd(string id)
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
            int int_id = Convert.ToInt32(id);

            List<DGroup> dlist = new List<DGroup>();
            dlist = db.DGroups.Where(x => x.CGrpID == int_id && x.Companyid == companyid).ToList();
            SelectList ob = new SelectList(dlist, "ID", "DGroupName", 0);
            return Json(ob);

        }
        [HttpGet]
        public ActionResult DownloadAssetsExcel()
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
            string TEMP_DIR = Server.MapPath("~") + "\\Temp";
            if (Directory.Exists(TEMP_DIR))
            {

            }
            else
            {
                Directory.CreateDirectory(TEMP_DIR);
            }

            VerifyExcel.ExportImport export = new VerifyExcel.ExportImport();
            String strFileName = TEMP_DIR + "\\"+ "AssetTemplate" + Guid.NewGuid().ToString() + ".xlsx";
            if (export.ExportAssetTemplate(strFileName))
            {
                Response.ClearContent();
                byte[] fileBytes = System.IO.File.ReadAllBytes(strFileName);
                Response.BinaryWrite(fileBytes);
                System.IO.File.Delete(strFileName); // clean up temp files
                Response.AddHeader("Content-Disposition", "filename=assettemplate.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            else
            {
                // show error page
            }
            */

            Response.ClearContent();
            Response.BinaryWrite(generateImportassetexcel());

            string excelName = "AdditionAssets";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Asset");
        }

        public byte[] generateImportassetexcel()
        {


            int srno = 1;



            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[]

                   { "SrNo","Asset No","Asset Name", "Asset Identification No", "VoucherNo", "Voucher Date",
                      "DtPutToUse","DtPutToUseIT","PONo","PODate","BillNo","BillDate","ReceiptDate","CommissioningDate","Qty","UOMNo",
                      "SupplierNo","AGroupID","BGroupID","CGroupID","DGroupID","LocAID","LocBID","LocCID","CostCenterAID","CostCenterBID","ITGroupIDID",
                      "Normal Rate","Additional Rate","Total Rate","Depreciation Method",
                      "GrossVal","ServiceCharges","OtherExp","CustomDuty","ExciseDuty","ServiceTax","AnyOtherDuty","VAT","CST","CGST","IGST","SGST",
                      "AnyTax","Total Addition","Discount","Rounding off","Total Deduction","Invoice Amount","DutyDrawback","ExciseCredit","ServiceTaxCredit",
                      "AnyOtherDutyCredit","VATCredit","CSTCredit","CGSTCredit","SGSTCredit","IGSTCredit","AnyOtherCredit","TotalCredit","AmountCapitalised",
                      "AmountCapitalisedCompany","AmountCApitalisedIT","OPAccDepreciation","ResidualVal","BrandName","SrNo","Model","Remarks","IsImported","Currency","Values" ,"YrofManufacturing","MRRNo",
                      "AccountID","DepAccountId","AccAccountID","Usefullife","Parent_AssetNo","Iscomponent","ExpiryDate"

            };


                // Determine the header range (e.g. A1:D1)


                //  string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";
                // string headerRange1 = "B2:"+ addition;

                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                var excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }

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


        private bool GetDate(string strDate, out DateTime tDate)
        {
            DateTime dtValidDate;
            //if (DateTime.TryParseExact(strDate, "yyyyMMdd",System.Globalization.CultureInfo.InvariantCulture,
            if (DateTime.TryParseExact(strDate, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtValidDate))
            {
                tDate = dtValidDate;
                return true;
            } else
            {
                tDate = DateTime.MinValue;
                return false;
            }

        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult UploadAdditionAssetsNew()
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
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            Assets assets = new Assets();

           

           
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


                            // AssetRepository assetRepository = new AssetRepository();

                            AssetImportRepository assetImportRepository = new AssetImportRepository(db);

                            assetImportRepository.companyid = companyid;
                            assetImportRepository.ReadExcel(workSheet);

                            assetImportRepository.Validate();
                            if (assetImportRepository.lstErrors.Count == 0) { 
                                assetImportRepository.ValidateRefIntegrity();
                            }

                            if (assetImportRepository.lstErrors.Count > 0)
                            {


                                response.status = "Error";
                                if (assetImportRepository.lstErrors.Count >=25)
                                {
                                    response.data = JsonConvert.SerializeObject(assetImportRepository.lstErrors.Take(100));
                                }
                                else
                                {
                                    response.data = JsonConvert.SerializeObject(assetImportRepository.lstErrors);
                                }

                                res.Data = JsonConvert.SerializeObject(response);
                                List <AssetImportError> lstErrors = new List<AssetImportError>(); ;
                                lstErrors = assetImportRepository.lstErrors; 

                                Session["ImportErrors"] = lstErrors;
                                return res;

                            }
                            else
                            {
                                // TODO Save Assets 

                                assetImportRepository.ConvertToAssets();
                             

                                response.status = "Success";
                                res.Data = JsonConvert.SerializeObject(response);

                                return res;

                            }


                           
                        }

                    }
                }
                else
                {
                    response.status = "Error";
                    res.Data = JsonConvert.SerializeObject(response);
                    logger.Info("Error Request null");
                    return res;

                }


            }
            catch (Exception ex)
            {

                string strError;
                strError = ex.Message + "|" + ex.InnerException;

                response.status = "RError";
                res.Data = JsonConvert.SerializeObject(response);

                // transaction.Rollback();
                logger.Log(LogLevel.Error, strError);
                //res.Data = "error";
                return res;
            }

            return res;
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult UploadAdditionAssets()
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
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();


            Assets assets = new Assets();
            DbContextTransaction transaction = db.Database.BeginTransaction();
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



                            //Test Code Mandar 17 APR 2022
                            /*
                            AssetImportNew assetImport = new AssetImportNew();
                            assetImport.workSheet = workSheet;
                            Assets newAsset = assetImport.GetAsset(companyid);
                            */

                            ///

                                 bool bAutoGenerateAssetNo = true;
                                if (company.AutoGenerateAssetNo =="Y")
                                {
                                     bAutoGenerateAssetNo = true;
                                }
                                else
                                {
                                    bAutoGenerateAssetNo = false;
                                 }



                                


                                string strsql = "select max(convert(assetno, unsigned)) assetno from tblassets order by convert(assetno, unsigned)";

                                var LastAssetNo = db.Database.SqlQuery<int>(strsql).FirstOrDefault();

                                int NextAssetNo = Convert.ToInt32(LastAssetNo);


                                 TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                var tnow = System.DateTime.Now.ToUniversalTime();
                                DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {

                                    NextAssetNo = NextAssetNo + 1;


                                    string AssetNo = "";
                                    string int_srno = "";
                                    string AssetName = "";
                                    string AssetIdentificationNo = "";
                                    string grossvalue = "";
                                    string servicecharge = "";
                                    string otherexpense = "";
                                    string customduty = "";
                                    string exciseduty = "";
                                    string servicetax = "";
                                    string vat = "";
                                    string anyotherduty = "";
                                    string cst = "";
                                    string anyothertax = "";
                                    string totaladdition = "";
                                    string discount = "";
                                    string roundoff = "";
                                    string totaldeduction = "";
                                    string invoiceamt = "";
                                    string dutydrawback = "";
                                    string excisecredit = "";
                                    string servicetaxcredit = "";
                                    string anyotherdutycredit = "";
                                    string vatcredit = "";
                                    string anyothercredit = "";
                                    string cstcredit = "";
                                    string totalcredit = "";
                                    string amtcap = "";
                                    string amtcapcompanylaw = "";
                                    string amtcapincometax = "";
                                    string VoucherNo = "";
                                    string VoucherDate = "";
                                    string PODate = "";
                                    string ReceiptDate = "";
                                    string CommissioningDate = "";
                                    string BillDate = "";
                                    string DtPutToUse = "";
                                    string DtPutToUseIT = "";
                                    string PONo = "";
                                    string BillNo = "";
                                    string Qty = "";
                                    string SupplierNo = "";
                                    string AGroupId = "";
                                    string BGroupId = "";
                                    string CGroupId = "";
                                    string DGroupId = "";
                                    string LocAId = "";
                                    string LocBId = "";
                                    string LocCId = "";
                                    string CostCenterAId = "";
                                    string CostCenterBId = "";
                                    string NormalRate = "";
                                    string AdditionalRate = "";
                                    string TotalRate = "";
                                    string DepreciationMethod = "";
                                    string BrandName = "";
                                    string SrNo = "";
                                    string Model = "";
                                    string Remarks = "";
                                    string IsImported = "";
                                    string Currency = "";
                                    string Values = "";
                                    string cgstcredit = "";
                                    string igstcredit = "";
                                    string sgstcredit = "";
                                    string cgst = "";
                                    string igst = "";
                                    string sgst = "";
                                    string uomno = "";
                                    string residual = "";
                                    string opAccDepreciation = "";
                                    string YrofManufacturing = "";
                                    string MRRNo = "";
                                    string AccountID = "";
                                    string DepAccountId = "";
                                    string AccAccountID = "";
                                    string ITGroupID = "";
                                    string Usefullife = "";
                                    string Parent_AssetNo = "";
                                    string iscomponent = "";
                                    string ExpiryDate = "";

                                    bool ExpiryDateflag = false;
                                    bool iscomponentflag = false;
                                    bool Parent_AssetNoflag = false;
                                    bool Usefullifeflag = false;
                                    bool AssetNoflag = false;
                                    bool int_srnoflag = false;
                                    bool AssetNameflag = false;
                                    bool AssetIdentificationNoflag = false;
                                    bool grossvalueflag = false;
                                    bool servicechargeflag = false;
                                    bool otherexpenseflag = false;
                                    bool customdutyflag = false;
                                    bool excisedutyflag = false;
                                    bool servicetaxflag = false;
                                    bool vatflag = false;
                                    bool anyotherdutyflag = false;
                                    bool cstflag = false;
                                    bool anyothertaxflag = false;
                                    bool totaladditionflag = false;
                                    bool discountflag = false;
                                    bool roundoffflag = false;
                                    bool totaldeductionflag = false;
                                    bool invoiceamtflag = false;
                                    bool dutydrawbackflag = false;
                                    bool excisecreditflag = false;
                                    bool servicetaxcreditflag = false;
                                    bool anyotherdutycreditflag = false;
                                    bool vatcreditflag = false;
                                    bool anyothercreditflag = false;
                                    bool cstcreditflag = false;
                                    bool totalcreditflag = false;
                                    bool amtcapflag = false;
                                    bool amtcapcompanylawflag = false;
                                    bool amtcapincometaxflag = false;
                                    bool VoucherNoflag = false;
                                    bool VoucherDateflag = false;
                                    bool PODateflag = false;
                                    bool ReceiptDateflag = false;
                                    bool CommissioningDateflag = false;
                                    bool BillDateflag = false;
                                    bool DtPutToUseflag = false;
                                    bool DtPutToUseITflag = false;
                                    bool PONoflag = false;
                                    bool BillNoflag = false;
                                    bool Qtyflag = false;
                                    bool SupplierNoflag = false;
                                    bool AGroupIdflag = false;
                                    bool BGroupIdflag = false;
                                    bool CGroupIdflag = false;
                                    bool DGroupIdflag = false;
                                    bool LocAIdflag = false;
                                    bool LocBIdflag = false;
                                    bool LocCIdflag = false;
                                    bool CostCenterAIdflag = false;
                                    bool CostCenterBIdflag = false;
                                    bool NormalRateflag = false;
                                    bool AdditionalRateflag = false;
                                    bool TotalRateflag = false;
                                    bool DepreciationMethodflag = false;
                                    bool BrandNameflag = false;
                                    bool SrNoflag = false;
                                    bool Modelflag = false;
                                    bool Remarksflag = false;
                                    bool IsImportedflag = false;
                                    bool Currencyflag = false;
                                    bool Valuesflag = false;
                                    bool cgstcreditflag = false;
                                    bool igstcreditflag = false;
                                    bool sgstcreditflag = false;
                                    bool cgstflag = false;
                                    bool igstflag = false;
                                    bool sgstflag = false;
                                    bool uomnoflag = false;
                                    bool residualflag = false;
                                    bool opAccDepreciationflag = false;
                                    bool YrofManufacturingflag = false;
                                    bool MRRNoflag = false;
                                    bool AccountIDflag = false;
                                    bool DepAccountIdflag = false;
                                    bool AccAccountIDflag = false;
                                    bool ITGroupIDflag = false;

                                
                                    if (workSheet.Cells[rowIterator, 1].Text == "")
                                    {
                                        int_srno = "";
                                        int_srnoflag = false;
                                    }
                                    else
                                    {
                                        int_srno = workSheet.Cells[rowIterator, 1].Value.ToString();
                                        int_srnoflag = true;
                                    }

                               // System.Diagnostics.Debug.WriteLine("Asset No  - " + workSheet.Cells[rowIterator, 2].Text);

                                    if (workSheet.Cells[rowIterator, 2].Text == "")
                                    {
                                        AssetNo = "";
                                        AssetNoflag = false;

                                    }
                                    else
                                    {

                                        if (bAutoGenerateAssetNo==false)
                                        {
                                            AssetNo = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            AssetNoflag = true;
                                        }else
                                        {
                                            AssetNo = NextAssetNo.ToString();
                                        }
                                        
                                    }
                                System.Diagnostics.Debug.WriteLine("Import Asset No" + AssetNo);
                                if (AssetNo  == "1571")
                                {
                                    int z = 0;
                                }

                                    if (workSheet.Cells[rowIterator, 3].Text == "")
                                    {
                                        AssetName = "";
                                        AssetNameflag = false;
                                    }
                                    else
                                    {
                                        AssetName = workSheet.Cells[rowIterator, 3].Value.ToString();
                                        AssetNameflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 4].Text == "")
                                    {
                                        AssetIdentificationNo = "";
                                        AssetIdentificationNoflag = false;
                                    }
                                    else
                                    {
                                        AssetIdentificationNo = workSheet.Cells[rowIterator, 4].Value.ToString();
                                        AssetIdentificationNoflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 5].Text == "")
                                    {
                                        VoucherNo = "";
                                        VoucherNoflag = false;
                                    }
                                    else
                                    {
                                        VoucherNo = workSheet.Cells[rowIterator, 5].Value.ToString();
                                        VoucherNoflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 6].Text == "")
                                    {
                                        VoucherDate = "";
                                        VoucherDateflag = false;
                                    }
                                    else
                                    {
                                        VoucherDate = workSheet.Cells[rowIterator, 6].Value.ToString();
                                        VoucherDateflag = true;
                                    }
                                    //-----------------------------------
                                    if (workSheet.Cells[rowIterator, 7].Text == "")
                                    {
                                        DtPutToUse = "";
                                        DtPutToUseflag = false;
                                    }
                                    else
                                    {
                                        DtPutToUse = workSheet.Cells[rowIterator, 7].Value.ToString();
                                        DtPutToUseflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 8].Text == "")
                                    {
                                        DtPutToUseIT = "";
                                        DtPutToUseITflag = false;
                                    }
                                    else
                                    {
                                        DtPutToUseIT = workSheet.Cells[rowIterator, 8].Value.ToString();
                                        DtPutToUseITflag = true;
                                    }
                                    //------------------------------------------
                                    if (workSheet.Cells[rowIterator, 9].Text == "")
                                    {
                                        PONo = "";
                                        PONoflag = false;

                                    }
                                    else
                                    {

                                        PONo = workSheet.Cells[rowIterator, 9].Value.ToString();
                                        PONoflag = true;
                                    }
                                    //----------------------------------------------

                                    if (workSheet.Cells[rowIterator, 10].Text == "")
                                    {
                                        PODate = "";
                                        PODateflag = false;
                                    }
                                    else
                                    {
                                        PODate = workSheet.Cells[rowIterator, 10].Value.ToString();
                                        PODateflag = true;
                                    }
                                    //------------------------------------------
                                    if (workSheet.Cells[rowIterator, 11].Text == "")
                                    {
                                        BillNo = "";
                                        BillNoflag = false;
                                    }
                                    else
                                    {
                                        BillNo = workSheet.Cells[rowIterator, 11].Value.ToString();
                                        BillNoflag = true;

                                    }
                                    if (workSheet.Cells[rowIterator, 12].Text == "")
                                    {
                                        BillDate = "";
                                        BillDateflag = false;
                                    }
                                    else
                                    {
                                        BillDate = workSheet.Cells[rowIterator, 12].Value.ToString();
                                        BillDateflag = true;
                                    }

                                    //----------------------------------------------------------
                                    if (workSheet.Cells[rowIterator, 13].Text == "")
                                    {
                                        ReceiptDate = "";
                                        ReceiptDateflag = false;
                                    }
                                    else
                                    {
                                        ReceiptDate = workSheet.Cells[rowIterator, 13].Value.ToString();
                                        ReceiptDateflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 14].Text == "")
                                    {
                                        CommissioningDate = "";
                                        CommissioningDateflag = false;
                                    }
                                    else
                                    {
                                        CommissioningDate = workSheet.Cells[rowIterator, 14].Value.ToString();
                                        CommissioningDateflag = true;
                                    }


                                    if (workSheet.Cells[rowIterator, 15].Text == "")
                                    {
                                        Qty = "";
                                        Qtyflag = false;
                                    }
                                    else
                                    {
                                        Qty = workSheet.Cells[rowIterator, 15].Value.ToString();
                                        Qtyflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 16].Text == "")
                                    {
                                        uomno = "";
                                        uomnoflag = false;
                                    }
                                    else
                                    {
                                        uomno = workSheet.Cells[rowIterator, 16].Value.ToString();
                                        uomnoflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 17].Text == "")
                                    {
                                        SupplierNo = "";
                                        SupplierNoflag = false;
                                    }
                                    else
                                    {
                                        SupplierNo = workSheet.Cells[rowIterator, 17].Value.ToString();
                                        SupplierNoflag = true;

                                    }
                                    //---------------------------------------------------------------------------------------------
                                    if (workSheet.Cells[rowIterator, 18].Text == "")
                                    {
                                        AGroupId = "";
                                        AGroupIdflag = false;
                                    }
                                    else
                                    {
                                        AGroupId = workSheet.Cells[rowIterator, 18].Value.ToString();
                                        AGroupIdflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 19].Text == "")
                                    {
                                        BGroupId = "";
                                        BGroupIdflag = false;
                                    }
                                    else
                                    {
                                        BGroupId = workSheet.Cells[rowIterator, 19].Value.ToString();
                                        BGroupIdflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 20].Text == "")
                                    {
                                        CGroupId = "";
                                        CGroupIdflag = false;
                                    }
                                    else
                                    {
                                        CGroupId = workSheet.Cells[rowIterator, 20].Value.ToString();
                                        CGroupIdflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 21].Text == "")
                                    {
                                        DGroupId = "";
                                        DGroupIdflag = false;
                                    }
                                    else
                                    {
                                        DGroupId = workSheet.Cells[rowIterator, 21].Value.ToString();
                                        DGroupIdflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 22].Text == "")
                                    {
                                        LocAId = "";
                                        LocAIdflag = false;
                                    }
                                    else
                                    {
                                        LocAId = workSheet.Cells[rowIterator, 22].Value.ToString();
                                        LocAIdflag = true;

                                    }
                                    if (workSheet.Cells[rowIterator, 23].Text == "")
                                    {
                                        LocBId = "";
                                        LocBIdflag = false;
                                    }
                                    else
                                    {
                                        LocBId = workSheet.Cells[rowIterator, 23].Value.ToString();
                                        LocBIdflag = true;

                                    }
                                    if (workSheet.Cells[rowIterator, 24].Text == "")
                                    {
                                        LocCId = "";
                                        LocCIdflag = false;
                                    }
                                    else
                                    {
                                        LocCId = workSheet.Cells[rowIterator, 24].Value.ToString();
                                        LocCIdflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 25].Text == "")
                                    {
                                        CostCenterAId = "";
                                        CostCenterAIdflag = false;
                                    }
                                    else
                                    {
                                        CostCenterAId = workSheet.Cells[rowIterator, 25].Value.ToString();
                                        CostCenterAIdflag = true;

                                    }
                                    if (workSheet.Cells[rowIterator, 26].Text == "")
                                    {
                                        CostCenterBId = "";
                                        CostCenterBIdflag = false;
                                    }
                                    else
                                    {
                                        CostCenterBId = workSheet.Cells[rowIterator, 26].Value.ToString();
                                        CostCenterBIdflag = true;

                                    }
                                    //---------------------------------------------
                                    if (workSheet.Cells[rowIterator, 27].Text == "")
                                    {
                                        ITGroupID = "";
                                        ITGroupIDflag = false;
                                    }
                                    else
                                    {
                                        ITGroupID = workSheet.Cells[rowIterator, 27].Value.ToString();
                                        ITGroupIDflag = true;

                                    }
                                    //-------------------------------------------------

                                    if (workSheet.Cells[rowIterator, 28].Text == "")
                                    {
                                        NormalRate = "";
                                        NormalRateflag = false;
                                    }
                                    else
                                    {
                                        NormalRate = workSheet.Cells[rowIterator, 28].Value.ToString();
                                        NormalRateflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 29].Text == "")
                                    {
                                        AdditionalRate = "";
                                        AdditionalRateflag = false;
                                    }
                                    else
                                    {
                                        AdditionalRate = workSheet.Cells[rowIterator, 29].Value.ToString();
                                        AdditionalRateflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 30].Text == "")
                                    {
                                        TotalRate = "";
                                        TotalRateflag = false;
                                    }
                                    else
                                    {
                                        TotalRate = workSheet.Cells[rowIterator, 30].Value.ToString();
                                        TotalRateflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 31].Text == "")
                                    {
                                        DepreciationMethod = "";
                                        DepreciationMethodflag = false;
                                    }
                                    else
                                    {
                                        DepreciationMethod = workSheet.Cells[rowIterator, 31].Value.ToString();
                                        DepreciationMethodflag = true;
                                    }
                                    //-------------------------------------------------

                                    if (workSheet.Cells[rowIterator, 32].Text == "")
                                    {
                                        grossvalue = "";
                                        grossvalueflag = false;
                                    }
                                    else
                                    {
                                        grossvalue = workSheet.Cells[rowIterator, 32].Value.ToString();
                                        grossvalueflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 33].Text == "")
                                    {
                                        servicecharge = "";
                                        servicechargeflag = false;
                                    }
                                    else
                                    {
                                        servicecharge = workSheet.Cells[rowIterator, 33].Value.ToString();
                                        servicechargeflag = true;
                                    }



                                    if (workSheet.Cells[rowIterator, 34].Text == "")
                                    {
                                        otherexpense = "";
                                        otherexpenseflag = false;

                                    }
                                    else
                                    {

                                        otherexpense = workSheet.Cells[rowIterator, 34].Value.ToString();
                                        otherexpenseflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 35].Text == "")
                                    {
                                        customduty = "";
                                        customdutyflag = false;
                                    }
                                    else
                                    {
                                        customduty = workSheet.Cells[rowIterator, 35].Value.ToString();
                                        customdutyflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 36].Text == "")
                                    {
                                        exciseduty = "";
                                        excisedutyflag = false;
                                    }
                                    else
                                    {
                                        exciseduty = workSheet.Cells[rowIterator, 36].Value.ToString();
                                        excisedutyflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 37].Text == "")
                                    {
                                        servicetax = "";
                                        servicetaxflag = false;
                                    }
                                    else
                                    {
                                        servicetax = workSheet.Cells[rowIterator, 37].Value.ToString();
                                        servicetaxflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 38].Text == "")
                                    {
                                        anyotherduty = "";
                                        anyotherdutyflag = false;
                                    }
                                    else
                                    {
                                        anyotherduty = workSheet.Cells[rowIterator, 38].Value.ToString();
                                        anyotherdutyflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 39].Text == "")
                                    {
                                        vat = "";
                                        vatflag = false;
                                    }
                                    else
                                    {
                                        vat = workSheet.Cells[rowIterator, 39].Value.ToString();
                                        vatflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 40].Text == "")
                                    {
                                        cst = "";
                                        cstflag = false;
                                    }
                                    else
                                    {
                                        cst = workSheet.Cells[rowIterator, 40].Value.ToString();
                                        cstflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 41].Text == "")
                                    {
                                        cgst = "";
                                        cgstflag = false;

                                    }
                                    else
                                    {

                                        cgst = workSheet.Cells[rowIterator, 41].Value.ToString();
                                        cgstflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 42].Text == "")
                                    {
                                        igst = "";
                                        igstflag = false;
                                    }
                                    else
                                    {
                                        igst = workSheet.Cells[rowIterator, 42].Value.ToString();
                                        igstflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 43].Text == "")
                                    {
                                        sgst = "";
                                        sgstflag = false;
                                    }
                                    else
                                    {
                                        sgst = workSheet.Cells[rowIterator, 43].Value.ToString();
                                        sgstflag = true;
                                    }

                                    //------------------------------------------

                                    if (workSheet.Cells[rowIterator, 44].Text == "")
                                    {
                                        anyothertax = "";
                                        anyothertaxflag = false;
                                    }
                                    else
                                    {
                                        anyothertax = workSheet.Cells[rowIterator, 44].Value.ToString();
                                        anyothertaxflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 45].Text == "")
                                    {
                                        totaladdition = "";
                                        totaladditionflag = false;
                                    }
                                    else
                                    {
                                        totaladdition = workSheet.Cells[rowIterator, 45].Value.ToString();
                                        totaladditionflag = true;
                                    }



                                    if (workSheet.Cells[rowIterator, 46].Text == "")
                                    {
                                        discount = "";
                                        discountflag = false;

                                    }
                                    else
                                    {

                                        discount = workSheet.Cells[rowIterator, 46].Value.ToString();
                                        discountflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 47].Text == "")
                                    {
                                        roundoff = "";
                                        roundoffflag = false;
                                    }
                                    else
                                    {
                                        roundoff = workSheet.Cells[rowIterator, 47].Value.ToString();
                                        roundoffflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 48].Text == "")
                                    {
                                        totaldeduction = "";
                                        totaldeductionflag = false;
                                    }
                                    else
                                    {
                                        totaldeduction = workSheet.Cells[rowIterator, 48].Value.ToString();
                                        totaldeductionflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 49].Text == "")
                                    {
                                        invoiceamt = "";
                                        invoiceamtflag = false;
                                    }
                                    else
                                    {
                                        invoiceamt = workSheet.Cells[rowIterator, 49].Value.ToString();
                                        invoiceamtflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 50].Text == "")
                                    {
                                        dutydrawback = "";
                                        dutydrawbackflag = false;
                                    }
                                    else
                                    {
                                        dutydrawback = workSheet.Cells[rowIterator, 50].Value.ToString();
                                        dutydrawbackflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 51].Text == "")
                                    {
                                        excisecredit = "";
                                        excisecreditflag = false;
                                    }
                                    else
                                    {
                                        excisecredit = workSheet.Cells[rowIterator, 51].Value.ToString();
                                        excisecreditflag = true;
                                    }



                                    if (workSheet.Cells[rowIterator, 52].Text == "")
                                    {
                                        servicetaxcredit = "";
                                        servicetaxcreditflag = false;

                                    }
                                    else
                                    {

                                        servicetaxcredit = workSheet.Cells[rowIterator, 52].Value.ToString();
                                        servicetaxcreditflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 53].Text == "")
                                    {
                                        anyotherdutycredit = "";
                                        anyotherdutycreditflag = false;
                                    }
                                    else
                                    {
                                        anyotherdutycredit = workSheet.Cells[rowIterator, 53].Value.ToString();
                                        anyotherdutycreditflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 54].Text == "")
                                    {
                                        vatcredit = "";
                                        vatcreditflag = false;
                                    }
                                    else
                                    {
                                        vatcredit = workSheet.Cells[rowIterator, 54].Value.ToString();
                                        vatcreditflag = true;

                                    }

                                    if (workSheet.Cells[rowIterator, 55].Text == "")
                                    {
                                        cstcredit = "";
                                        cstcreditflag = false;
                                    }
                                    else
                                    {
                                        cstcredit = workSheet.Cells[rowIterator, 55].Value.ToString();
                                        cstcreditflag = true;
                                    }


                                    if (workSheet.Cells[rowIterator, 56].Text == "")
                                    {
                                        cgstcredit = "";
                                        cgstcreditflag = false;
                                    }
                                    else
                                    {
                                        cgstcredit = workSheet.Cells[rowIterator, 56].Value.ToString();
                                        cgstcreditflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 57].Text == "")
                                    {
                                        igstcredit = "";
                                        igstcreditflag = false;
                                    }
                                    else
                                    {
                                        igstcredit = workSheet.Cells[rowIterator, 57].Value.ToString();
                                        igstcreditflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 58].Text == "")
                                    {
                                        sgstcredit = "";
                                        sgstcreditflag = false;
                                    }
                                    else
                                    {
                                        sgstcredit = workSheet.Cells[rowIterator, 58].Value.ToString();
                                        sgstcreditflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 59].Text == "")
                                    {
                                        anyothercredit = "";
                                        anyothercreditflag = false;
                                    }

                                    else
                                    {
                                        anyothercredit = workSheet.Cells[rowIterator, 59].Value.ToString();
                                        anyothercreditflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 60].Text == "")
                                    {
                                        totalcredit = "";
                                        totalcreditflag = false;
                                    }
                                    else
                                    {
                                        totalcredit = workSheet.Cells[rowIterator, 60].Value.ToString();
                                        totalcreditflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 61].Text == "")
                                    {
                                        amtcap = "";
                                        amtcapflag = false;
                                    }
                                    else
                                    {
                                        amtcap = workSheet.Cells[rowIterator, 61].Value.ToString();
                                        amtcapflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 62].Text == "")
                                    {
                                        amtcapcompanylaw = "";
                                        amtcapcompanylawflag = false;
                                    }
                                    else
                                    {
                                        amtcapcompanylaw = workSheet.Cells[rowIterator, 62].Value.ToString();
                                        amtcapcompanylawflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 63].Text == "")
                                    {
                                        amtcapincometax = "";
                                        amtcapincometaxflag = false;
                                    }
                                    else
                                    {
                                        amtcapincometax = workSheet.Cells[rowIterator, 63].Value.ToString();
                                        amtcapincometaxflag = true;
                                    }
                                    //------------------------------------------------
                                    if (workSheet.Cells[rowIterator, 64].Text == "")
                                    {
                                        opAccDepreciation = "";
                                        opAccDepreciationflag = false;
                                    }
                                    else
                                    {
                                        opAccDepreciation = workSheet.Cells[rowIterator, 64].Value.ToString();
                                        opAccDepreciationflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 65].Text == "")
                                    {
                                        residual = "";
                                        residualflag = false;

                                    }
                                    else
                                    {

                                        residual = workSheet.Cells[rowIterator, 65].Value.ToString();
                                        residualflag = true;
                                    }

                                    //-----------------------------------------------------------------------------------------                                
                                    if (workSheet.Cells[rowIterator, 66].Text == "")
                                    {
                                        BrandName = "";
                                        BrandNameflag = false;
                                    }
                                    else
                                    {
                                        BrandName = workSheet.Cells[rowIterator, 66].Value.ToString();
                                        BrandNameflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 67].Text == "")
                                    {
                                        SrNo = "";
                                        SrNoflag = false;
                                    }
                                    else
                                    {
                                        SrNo = workSheet.Cells[rowIterator, 67].Value.ToString();
                                        SrNoflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 68].Text == "")
                                    {
                                        Model = "";
                                        Modelflag = false;
                                    }
                                    else
                                    {
                                        Model = workSheet.Cells[rowIterator, 68].Value.ToString();
                                        Modelflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 69].Text == "")
                                    {
                                        Remarks = "";
                                        Remarksflag = false;
                                    }
                                    else
                                    {
                                        Remarks = workSheet.Cells[rowIterator, 69].Value.ToString();
                                        Remarksflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 70].Text == "")
                                    {
                                        IsImported = "";
                                        IsImportedflag = false;
                                    }
                                    else
                                    {
                                        IsImported = workSheet.Cells[rowIterator, 70].Value.ToString();
                                        IsImportedflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 71].Text == "")
                                    {
                                        Currency = "";
                                        Currencyflag = false;
                                    }
                                    else
                                    {
                                        Currency = workSheet.Cells[rowIterator, 71].Value.ToString();
                                        Currencyflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 72].Text == "")
                                    {
                                        Values = "";
                                        Valuesflag = false;
                                    }
                                    else
                                    {
                                        Values = workSheet.Cells[rowIterator, 72].Value.ToString();
                                        Valuesflag = true;
                                    }
                                    //---------------------------------------
                                    if (workSheet.Cells[rowIterator, 73].Text == "")
                                    {
                                        YrofManufacturing = "";
                                        YrofManufacturingflag = false;
                                    }
                                    else
                                    {
                                        YrofManufacturing = workSheet.Cells[rowIterator, 73].Value.ToString();
                                        YrofManufacturingflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 74].Text == "")
                                    {
                                        MRRNo = "";
                                        MRRNoflag = false;
                                    }
                                    else
                                    {
                                        MRRNo = workSheet.Cells[rowIterator, 74].Value.ToString();
                                        MRRNoflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 75].Text == "")
                                    {
                                        AccountID = "";
                                        AccountIDflag = false;
                                    }
                                    else
                                    {
                                        AccountID = workSheet.Cells[rowIterator, 75].Value.ToString();
                                        AccountIDflag = true;
                                    }

                                    if (workSheet.Cells[rowIterator, 76].Text == "")
                                    {
                                        DepAccountId = "";
                                        DepAccountIdflag = false;
                                    }
                                    else
                                    {
                                        DepAccountId = workSheet.Cells[rowIterator, 76].Value.ToString();
                                        DepAccountIdflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 77].Text == "")
                                    {
                                        AccAccountID = "";
                                        AccAccountIDflag = false;
                                    }
                                    else
                                    {
                                        AccAccountID = workSheet.Cells[rowIterator, 77].Value.ToString();
                                        AccAccountIDflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 78].Text == "")
                                    {
                                        Usefullife = "";
                                        Usefullifeflag = false;
                                    }
                                    else
                                    {
                                        Usefullife = workSheet.Cells[rowIterator, 78].Value.ToString();
                                        Usefullifeflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 79].Text == "")
                                    {
                                        Parent_AssetNo = "";
                                        Parent_AssetNoflag = false;
                                    }
                                    else
                                    {
                                        Parent_AssetNo = workSheet.Cells[rowIterator, 79].Value.ToString();
                                        Parent_AssetNoflag = true;
                                    }
                                    if (workSheet.Cells[rowIterator, 80].Text == "")
                                    {
                                        iscomponent = "";
                                        iscomponentflag = false;
                                    }
                                    else
                                    {
                                        iscomponent = workSheet.Cells[rowIterator, 80].Value.ToString();
                                        iscomponentflag = true;
                                    }
                                if (workSheet.Cells[rowIterator, 81].Text == "")
                                {
                                    ExpiryDate = "";
                                    ExpiryDateflag = false;
                                }
                                else
                                {
                                    ExpiryDate = workSheet.Cells[rowIterator, 81].Value.ToString();
                                    ExpiryDateflag = true;
                                }
                                //-----------------------------------------------

                                //if (AssetNoflag == true && AssetNameflag == true
                                //         && VoucherDateflag == true && Qtyflag == true && grossvalueflag == true
                                //         && amtcapflag == true && amtcapcompanylawflag == true && amtcapincometaxflag == true 
                                //         && DtPutToUseflag == true && DtPutToUseITflag == true && Usefullifeflag == true 
                                //         && DepreciationMethodflag == true)

                                if (AssetNoflag == true && AssetNameflag == true
                                 && VoucherDateflag == true && Qtyflag == true 
                                 && amtcapflag == true && amtcapcompanylawflag == true && amtcapincometaxflag == true
                                 && DtPutToUseflag == true && DtPutToUseITflag == true && Usefullifeflag == true
                                 && DepreciationMethodflag == true)
                                {

                                        norecordsfound = true;
                                        Assets assetexits = new Assets();
                                        
                                        assetexits = db.Assetss.Where(r => r.AssetNo == AssetNo && r.Companyid == companyid).FirstOrDefault();

                                        if (assetexits != null)
                                        {
                                            errorlist.Add("Asset no record already exists in master,i.e of  row " + rowIterator);
                                            res.Data = errorlist;
                                            logger.Info("Asset No Already Exists" + rowIterator);

                                        }
                                        else
                                        {
                                            if (BillDate != "")
                                            {
                                                DateTime temp_BillDate = DateTime.MinValue;

                                                if  (GetDate(BillDate, out temp_BillDate)) {

                                                      assets.BillDate = temp_BillDate;
                                                }
                                            }
                                            if (ReceiptDate != "")
                                            {
                                               DateTime temp_receiptDate = DateTime.MinValue;
                                                if (GetDate(ReceiptDate, out temp_receiptDate))
                                                {

                                                    assets.ReceiptDate = temp_receiptDate;
                                                }

                                            }

                                            if (CommissioningDate != "")
                                            {

                                                DateTime temp_CommDate = DateTime.MinValue;

                                                if (GetDate(CommissioningDate, out temp_CommDate))
                                                {

                                                    assets.CommissioningDate = temp_CommDate;
                                                }
                                            }
                                            if (PODate != "")
                                            {
                                                DateTime temp_POdate = DateTime.MinValue;

                                                if (GetDate(PODate, out temp_POdate))
                                                {

                                                    assets.PODate = temp_POdate;
                                                }
                                            }

                                            if (VoucherDate != "")
                                            {
                                                DateTime temp_voucherDate = DateTime.MinValue;

                                                if (GetDate(VoucherDate, out temp_voucherDate))
                                                {

                                                    assets.VoucherDate = temp_voucherDate;
                                                }
                                            }

                                        if (DtPutToUse != "")
                                        {
                                            DateTime temp_DtPutToUse = DateTime.MinValue;

                                            if (GetDate(DtPutToUse, out temp_DtPutToUse))
                                            {

                                                assets.DtPutToUse = temp_DtPutToUse;
                                            }
                                        }

                                        if (ExpiryDate != "")
                                        {

                                            DateTime temp_ExpiryDate = DateTime.MinValue;

                                            if (GetDate(ExpiryDate, out temp_ExpiryDate))
                                            {

                                                assets.ExpiryDate = temp_ExpiryDate;
                                            }
                                        }
                                        else
                                        {
                                            if (Usefullife != "")
                                            {
                                                decimal x = Convert.ToDecimal(Usefullife);

                                                int nbYear = Convert.ToInt16(x);
                                                var y = x - Math.Truncate(x);

                                                int nbMonth = Convert.ToInt16(y * 12);
                                                // MessageBox .Show (string.Format (" {0} years and {1} months ",nbYear ,nbMonth ));
                                                DateTime dat = Convert.ToDateTime(assets.DtPutToUse);  // or    given date
                                                DateTime dat2 = dat.AddYears(nbYear).AddMonths(nbMonth);
                                                assets.ExpiryDate = dat2;
                                            }
                                        }
                                        List<Period> period = new List<Period>();

                                            var firstperiod = db.Periods.Where(x => x.Companyid == companyid).First();
                                            // string checkflag = "";
                                            // DateTime vdate = Convert.ToDateTime(strvdate);
                                            if (firstperiod != null)
                                            {

                                                if (assets.VoucherDate < firstperiod.FromDate)
                                                {
                                                    decimal oppaccdep = Convert.ToDecimal(opAccDepreciation);
                                                    if (oppaccdep < 0)
                                                    {
                                                        errorlist.Add("Openingaccumulated should not be zero.For row" + rowIterator);
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        assets.OPAccDepreciation = oppaccdep;
                                                    }

                                                    // res.Data = checkflag;
                                                }

                                                else
                                                {

                                                    assets.OPAccDepreciation = 0;

                                                }

                                            }
                                            else
                                            {
                                                // res.Data = "Noperiod";
                                                errorlist.Add("No period entered in master. For row" + rowIterator);
                                                res.Data = errorlist;
                                                continue;
                                            }
                                            ///////////////////////////////
                                            //for voucher date

                                            var checkvoucherdate = ImportDatevalidation(Convert.ToDateTime(assets.VoucherDate), companyid);
                                            if (checkvoucherdate == "Yes")
                                            {
                                                errorlist.Add("For voucher date period is lock you cannot add asset. For row" + rowIterator);
                                                continue;
                                            }
                                            if (checkvoucherdate == "No")
                                            {
                                                assets.VoucherDate = assets.VoucherDate;

                                            }
                                            if (checkvoucherdate == "Nosubperiod")
                                            {
                                                errorlist.Add("For voucher date No Sub period found. For row" + rowIterator);
                                                continue;
                                            }
                                            if (checkvoucherdate == "Depalreadycalculated")
                                            {
                                                errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                                continue;
                                            }
                                            //for dateputtousecompany
                                            var checkdateputtousecomp = ImportDatevalidation(Convert.ToDateTime(assets.DtPutToUse), companyid);
                                            if (checkdateputtousecomp == "Yes")
                                            {
                                                errorlist.Add("For voucher date period is lock you cannot add asset.For row" + rowIterator);
                                                continue;
                                            }
                                            if (checkdateputtousecomp == "No")
                                            {
                                                assets.DtPutToUse = assets.DtPutToUse;

                                            }
                                            if (checkvoucherdate == "Nosubperiod")
                                            {
                                                errorlist.Add("For voucher date No Sub period found. For row" + rowIterator);
                                                continue;
                                            }
                                            if (checkvoucherdate == "Depalreadycalculated")
                                            {
                                                errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                                continue;
                                            }
                                        //for dateputtousecompany
                                        DateTime dtputtuseit;

                                        ///string Str_dtputtuseit = Convert.ToDateTime(DtPutToUseIT).ToString("yyyy-MM-dd");



                                        if (DtPutToUseIT != null)
                                        {
                                            DateTime temp_DtPutToUseIT = DateTime.MinValue;

                                            if (GetDate(DtPutToUseIT,out temp_DtPutToUseIT))
                                            {


                                                var checkdateputtouseit = ImportDatevalidation(temp_DtPutToUseIT, companyid);
                                                if (checkdateputtouseit == "Yes")
                                                {
                                                    errorlist.Add("For voucher date period is lock you cannot add asset.For row" + rowIterator);
                                                    continue;
                                                }
                                                if (checkdateputtouseit == "No")
                                                {
                                                    assets.DtPutToUseIT = temp_DtPutToUseIT;

                                                }
                                                if (checkvoucherdate == "Nosubperiod")
                                                {
                                                    errorlist.Add("For voucher date No Sub period found. For row" + rowIterator);
                                                    continue;
                                                }
                                                if (checkvoucherdate == "Depalreadycalculated")
                                                {
                                                    errorlist.Add("Depriciation is calculated you cannot add asset. For row" + rowIterator);
                                                    continue;
                                                }
                                            }
                                        }
                 

                                            assets.SrNo = int_srno;
                                            assets.AssetName = AssetName;
                                            assets.AssetNo = AssetNo;
                                            assets.AssetIdentificationNo = AssetIdentificationNo;
                                            assets.VoucherNo = VoucherNo;
                                            assets.iscomponent = iscomponent;
                                            assets.PONo = PONo;
                                            assets.BillNo = BillNo;
                                            if (SupplierNo != "")
                                            {
                                                assets.SupplierNo = Convert.ToInt32(SupplierNo);
                                            }
                                            if (uomno != "")
                                            {
                                                   assets.UOMNo = Convert.ToInt32(uomno);
                                            }

                                            assets.ResidualVal = ToDecimal(residual);
                                            assets.Qty = Convert.ToInt32(Qty);
                                            assets.GrossVal = ToDecimal(grossvalue);
                                            assets.ServiceCharges = ToDecimal(servicecharge);
                                            assets.ExciseDuty = ToDecimal(exciseduty);
                                            assets.CustomDuty = ToDecimal(customduty);
                                            assets.AnyOtherDuty = ToDecimal(anyotherduty);
                                            assets.VAT = ToDecimal(vat);
                                            assets.AnyOtherTax = ToDecimal(anyothertax);
                                            assets.CSt = ToDecimal(cst);
                                            assets.CGST = ToDecimal(cgst);
                                            assets.SGST = ToDecimal(sgst);
                                            assets.IGST = ToDecimal(igst);
                                            assets.NormalRatae = ToDecimal(NormalRate);
                                            assets.AdditionalRate = ToDecimal(AdditionalRate);
                                            assets.TotalRate = ToDecimal(TotalRate);
                                            assets.ServiceTax = ToDecimal(servicetax);
                                            assets.OtherExp = ToDecimal(otherexpense);
                                            assets.TotalAddition = ToDecimal(totaladdition);
                                            assets.Discount = ToDecimal(discount);
                                            assets.Roundingoff = ToDecimal(roundoff);
                                            assets.TotDeduction = ToDecimal(totaldeduction);
                                            assets.InvoiceAmt = ToDecimal(invoiceamt);
                                            assets.AnyOtherCredit = ToDecimal(anyothercredit);
                                            assets.DutyDrawback = ToDecimal(dutydrawback);
                                            assets.ExciseCredit = ToDecimal(excisecredit);
                                            assets.AnyOtherCredit = ToDecimal(anyothercredit);
                                            assets.ServiceTaxCredit = ToDecimal(servicetaxcredit);
                                            assets.VATCredit = ToDecimal(vatcredit);
                                            assets.CSTCredit = ToDecimal(cstcredit);
                                            assets.CGSTCredit = ToDecimal(cgstcredit);
                                            assets.SGSTCredit = ToDecimal(sgstcredit);
                                            assets.IGSTCredit = ToDecimal(igstcredit);
                                            assets.MRRNo = MRRNo;
                                             assets.DepreciationMethod = DepreciationMethod;
                                            assets.TotalCredit = ToDecimal(totalcredit);
                                            assets.AmountCapitalised = ToDecimal(amtcap);
                                            assets.AmountCapitalisedCompany = ToDecimal(amtcapcompanylaw);
                                            assets.AmountCApitalisedIT = ToDecimal(amtcapincometax);
                                            assets.AnyOtherDutyCredit = ToDecimal(anyotherdutycredit);
                                            assets.BrandName = BrandName;
                                            assets.SrNo = SrNo;
                                            assets.Model = Model;
                                            assets.Remarks = Remarks;
                                            assets.IsImported = IsImported;
                                            assets.Currency = Currency;
                                            if (Values == "")
                                            {
                                                assets.Values = 0;
                                            }
                                            else
                                            {
                                                assets.Values = ToDecimal(Values);
                                            }

                                            assets.Usefullife = ToDecimal(Usefullife);
                                            if (ITGroupID == "")
                                            {
                                                assets.ITGroupIDID = 0;
                                            }
                                            else
                                            {
                                                assets.ITGroupIDID = Convert.ToInt32(ITGroupID);
                                            }

                                            assets.iscomponent = iscomponent;
                                            if (Parent_AssetNo == "" || Parent_AssetNo == "0")
                                        {
                                                assets.Parent_AssetId = 0;
                                            }
                                            else
                                            {
                                                var parentassetid = db.Assetss.Where(x => x.AssetNo == Parent_AssetNo).FirstOrDefault().ID;
                                                assets.Parent_AssetId = parentassetid;
                                            }

                                            if (AccountID == "")
                                            {
                                                assets.AccountID = 0;
                                            }
                                            else
                                            {
                                                assets.AccountID = Convert.ToInt32(AccountID);
                                            }
                                            if (DepAccountId == "")
                                            {
                                                assets.DepAccountId = 0;
                                            }
                                            else
                                            {
                                                assets.DepAccountId = Convert.ToInt32(DepAccountId);
                                            }
                                            if (AccAccountID == "")
                                            {
                                                assets.AccAccountID = 0;
                                            }
                                            else
                                            {
                                                assets.AccAccountID = Convert.ToInt32(AccAccountID);
                                            }


                                            if (AGroupId == "")
                                            {
                                                assets.AGroupID = 0;
                                            }
                                            else
                                            {
                                                assets.AGroupID = Convert.ToInt32(AGroupId);
                                            }
                                            if (BGroupId == "")
                                            {
                                                assets.BGroupID = 0;
                                            }
                                            else
                                            {
                                                assets.BGroupID = Convert.ToInt32(BGroupId);
                                            }
                                            if (CGroupId == "")
                                            {
                                                assets.CGroupID = 0;
                                            }
                                            else
                                            {
                                                assets.CGroupID = Convert.ToInt32(CGroupId);
                                            }
                                            if (DGroupId == "")
                                            {
                                                assets.DGroupID = 0;
                                            }
                                            else
                                            {
                                                assets.DGroupID = Convert.ToInt32(DGroupId);
                                            }
                                            if (LocAId == "")
                                            {
                                                assets.LocAID = 0;
                                            }
                                            else
                                            {
                                                assets.LocAID = Convert.ToInt32(LocAId);
                                            }
                                            if (LocBId == "")
                                            {
                                                assets.LocBID = 0;
                                            }
                                            else
                                            {
                                                assets.LocBID = Convert.ToInt32(LocBId);
                                            }
                                            if (LocCId == "")
                                            {
                                                assets.LocCID = 0;
                                            }
                                            else
                                            {
                                                assets.LocCID = Convert.ToInt32(LocCId);
                                            }
                                            ///////////////////costcenter
                                            if (CostCenterAId == "")
                                            {
                                                assets.CostCenterAID = 0;
                                            }
                                            else
                                            {
                                                assets.CostCenterAID = Convert.ToInt32(CostCenterAId);
                                            }
                                            if (CostCenterBId == "")
                                            {
                                                assets.CostCenterBID = 0;
                                            }
                                            else
                                            {
                                                assets.CostCenterBID = Convert.ToInt32(CostCenterBId);
                                            }
                                            //////////////


                                            assets.Companyid = companyid;
                                            assets.CreatedDate = istDate;
                                            assets.CreatedUserId = userid;

                                            db.Assetss.Add(assets);
                                            db.SaveChanges();


                                            Childlocation childlocation = new Childlocation();
                                            if (LocAId == "")
                                            {
                                                childlocation.ALocID = 0;
                                            }
                                            else
                                            {
                                                childlocation.ALocID = Convert.ToInt32(LocAId);
                                            }
                                            if (LocBId == "")
                                            {
                                                childlocation.BLocID = 0;
                                            }
                                            else
                                            {
                                                childlocation.BLocID = Convert.ToInt32(LocBId);
                                            }
                                            if (LocCId == "")
                                            {
                                                childlocation.CLocID = 0;
                                            }
                                            else
                                            {
                                                childlocation.CLocID = Convert.ToInt32(LocCId);
                                            }//check if locationdate and costcenter is less then voucherdate if asked by team
                                             //childlocation.Date =;
                                            childlocation.AssetID = assets.ID;
                                            childlocation.Date = istDate;
                                            childlocation.Companyid = companyid;
                                            childlocation.CreatedDate = istDate;
                                            childlocation.CreatedUserId = userid;

                                            if (childlocation.ALocID != 0)
                                            {
                                                db.childlocations.Add(childlocation);
                                                db.SaveChanges();
                                            }
                                            Childcostcenter childcostcenter = new Childcostcenter();
                                            if (CostCenterAId == "")
                                            {
                                                childcostcenter.AcostcenterID = 0;
                                            }
                                            else
                                            {
                                                childcostcenter.AcostcenterID = Convert.ToInt32(CostCenterAId);
                                            }
                                            if (CostCenterBId == "")
                                            {
                                                childcostcenter.BcostcenterID = 0;
                                            }
                                            else
                                            {
                                                childcostcenter.BcostcenterID = Convert.ToInt32(CostCenterBId);
                                            }

                                            //childlocation.Date =;
                                            childcostcenter.Ass_ID = assets.ID;
                                            childcostcenter.CreatedDate = istDate;
                                            childcostcenter.CreatedUserId = userid;
                                            childcostcenter.Percentage = "100";
                                            childcostcenter.Companyid = companyid;
                                        if (childcostcenter.AcostcenterID != 0)
                                        {
                                            db.childcostcenters.Add(childcostcenter);
                                            db.SaveChanges();
                                        }
                                        }

                                    }
                                    else
                                    {
                                        errorlist.Add("Something  went wrong or some value missing in the row  " + rowIterator);
                                    }

                               
                            }


                            transaction.Commit();
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
                    }
                    else
                    {
                        res.Data = "error";
                        logger.Info("Error Request null");
                    return res;

                    }


                }
                catch (Exception ex)
                {

                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                        
                transaction.Rollback();
                logger.Log(LogLevel.Error, strError);
                res.Data = "error";
                    return res;
                }
            
            return res;
        }

        // new method to check quick asset add and C1 library
        // mandar 12feb2022
        public ActionResult UploadExcel()
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
            APIResponse response = new APIResponse();

            JsonResult res;
            res = new JsonResult();

            Assets assets = new Assets();
            DbContextTransaction transaction = db.Database.BeginTransaction();
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

                        //VerifyExcel.ExportImport import = new VerifyExcel.ExportImport();

                        //import.ImportAssetQuick(fileBytes);


                    }
                }
            }catch(Exception ex)
            {
                res.Data = "error";
                return res;


            }


            res.Data = "Sucess";
            return res;


        }

        public string ImportDatevalidation(DateTime date, int companyid)
        {
            //i have used res.Data=yess for error showing error and res.data=no for no errors
            string checkflag = "";
            JsonResult res;
            res = new JsonResult();
            DateTime itperioddate = Convert.ToDateTime("01/01/0001");
            DateTime perioddate = Convert.ToDateTime("01/01/0001");
            List<ITPeriod> itperiod = new List<ITPeriod>();

            // period = db.ITPeriods.Where(x=>x.PeriodlockFlag==1).ToList();
            itperiod = db.ITPeriods.Where(x=>x.Companyid==companyid).ToList();
            // string checkflag = "";
            // DateTime vdate = Convert.ToDateTime(strvdate);
            //if (itperiod.Count!=0)
            //{
            List<ITPeriod> itperiodlock = new List<ITPeriod>();
            itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1 && x.Companyid==companyid).ToList();
            if (itperiodlock.Count != 0)
            {
                foreach (ITPeriod item in itperiodlock)
                {
                    if (date >= item.FromDate && date <= item.ToDate)
                    {
                        // checkflag = "Yes";
                        // res.Data = checkflag;
                        itperioddate = item.ToDate;

                        break;
                    }
                    //else
                    //{
                    //   itperioddate = Convert.ToDateTime("00/00/0000");
                    //    // checkflag = "No";
                    //    //res.Data = checkflag;
                    //}
                }

            }
            else
            {
                // perioddate = Convert.ToDateTime("00/00/0000");
                // res.Data = "NoLock";
            }
            //}
            //else
            //{
            //    res.Data = "Noitperiod";
            //}
            List<SubPeriod> subperiod = new List<SubPeriod>();
            // subperiod = db.SubPeriods.ToList();
            //period = db.Periods.ToList();
            //if(period.Count!=0)
            //{
            //  foreach (Period item in period)
            // {
            //if (vdate >= item.FromDate && vdate <= item.ToDate)
            //{
            //  periodid = item.ID;
            List<SubPeriod> slist = new List<SubPeriod>();
            slist = db.SubPeriods.Where(x=>x.Companyid==companyid).ToList();
            if (slist != null)
            {
                SubPeriod checkdepflag = new SubPeriod();
                checkdepflag = db.SubPeriods.Where(x => x.DepFlag == "Y" && x.FromDate <= date && x.ToDate >= date && x.Companyid==companyid).FirstOrDefault();
                if (checkdepflag == null)
                {
                    subperiod = db.SubPeriods.Where(x => x.PeriodLockFlag == "Y" && x.Companyid==companyid).ToList();
                    if (subperiod.Count != 0)
                    {
                        foreach (SubPeriod itemsub in subperiod)
                        {
                            if (date >= itemsub.FromDate && date <= itemsub.ToDate)
                            {
                                perioddate = itemsub.ToDate;
                                break;
                            }
                            //else
                            //{
                            //    perioddate = Convert.ToDateTime("00/00/0000");
                            //}
                        }
                    }
                    else
                    {
                        //perioddate = Convert.ToDateTime("00/00/0000");
                    }
                }

                else
                {
                    checkflag = "Depalreadycalculated";
                    return checkflag;
                }
            }
            else
            {
                checkflag = "Nosubperiod";
                return checkflag;
            }

            
            //}
            //else
            //{
            //    //noperiod
            //    perioddate = Convert.ToDateTime("00/00/0000");
            //}
            DateTime lockdate;
            int value = DateTime.Compare(perioddate, itperioddate);

            // checking 
            if (value > 0)
            {
                lockdate = perioddate;
                // Console.Write("date1 is later than date2. ");
            }
            else if (value < 0)
            {
                lockdate = itperioddate;
                //Console.Write("date1 is earlier than date2. ");
            }

            else
            {
                lockdate = perioddate;
                //Console.Write("date1 is the same as date2. ");
            }

            if (date <= lockdate)
            {
                //error
                checkflag = "Yes";
            }
            else
            {
                //nothing
                checkflag = "No";
            }

            return checkflag;

        }
        [HttpPost]
        [AllowAnonymous]
        
        public ActionResult Delete(string id)
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
           
           

            try
            {
                Assets assets = new Assets();
                var assetid = db.Assetss.Where(x => x.AssetNo == id && x.Companyid == companyid).FirstOrDefault().ID;

                assets = db.Assetss.Where(x => x.ID == assetid && x.Companyid == companyid).FirstOrDefault();
                assets.str_VoucherDate = Convert.ToDateTime(assets.VoucherDate).ToString("yyyy-MM-dd");
                DateTime checkvoucherdate;
                string checklockflag="";
                //= DateTime.Parse(assets.str_VoucherDate);
                if (DateTime.TryParseExact(assets.str_VoucherDate, "yyyy-MM-dd",
                                                                      System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out checkvoucherdate))
                {
                    var date = checkvoucherdate;
                    checklockflag = ImportDatevalidation(date, companyid);
                    if (checklockflag == "Depalreadycalculated")
                    {
                        res.Data = "Depalreadycalculated";
                        return res;
                    }
                    if (checklockflag == "Yes")
                    {
                        res.Data = "Periodlock";
                        return res;
                    }

                    if (checklockflag == "Nomainperiod")
                    {
                        res.Data = "Nomainperiod";
                        return res;
                    }
                }
                List<Depreciation> deplist = new List<Depreciation>();
                deplist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == assets.ID).ToList();
                if (deplist.Count() != 0)
                {res.Data = "Depalreadycalculated";
                    return res;
                }
                if (checklockflag == "No")
                {



                    ///check assets child tables or related table if it exists or not
                    List<SubAmc> amclist = new List<SubAmc>();
                    amclist = db.SubAmc.Where(x => x.Companyid == companyid && x.AssetId == assetid).ToList();
                    List<SubLoan> loanlist = new List<SubLoan>();
                    loanlist = db.SubLoans.Where(x => x.Companyid == companyid && x.AssetId == assetid).ToList();
                    List<SubInsurance> subinslist = new List<SubInsurance>();
                    subinslist = db.SubInsurances.Where(x => x.Companyid == companyid && x.AssetId == assetid).ToList();
                    List<Assetfreeofcost> assetextralist = new List<Assetfreeofcost>();
                    assetextralist = db.Assetfreeofcosts.Where(x => x.Companyid == companyid && x.Asset_id == assetid).ToList();

                    List<Disposal> desplist = new List<Disposal>();
                    desplist = db.Disposals.Where(x => x.Companyid == companyid && x.AssetId == assetid).ToList();
                    List<Depreciation> deprelist = new List<Depreciation>();
                    deprelist = db.Depreciations.Where(x => x.Companyid == companyid && x.AssetId == assetid).ToList();
                    ////


                    List<Childlocation> loclist = new List<Childlocation>();
                    loclist = db.childlocations.Where(x => x.Companyid == companyid && x.AssetID == assetid).ToList();
                    List<Childcostcenter> cclist = new List<Childcostcenter>();
                    cclist = db.childcostcenters.Where(x => x.Companyid == companyid && x.Ass_ID == assetid).ToList();


                    List<Child_Asset_Attachment> childassetattchlist = new List<Child_Asset_Attachment>();
                    childassetattchlist = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid && x.AssetID == assetid).ToList();


                    if (subinslist.Count == 0 && loanlist.Count == 0 && amclist.Count == 0 && deplist.Count == 0
                        && deprelist.Count == 0 && childassetattchlist.Count == 0 && loclist.Count == 0 && cclist.Count == 0
                        && assetextralist.Count == 0)
                    {

                        var assetdeleteobj = db.Assetss.Where(x => x.ID == assetid).FirstOrDefault();
                        db.Entry(assetdeleteobj).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        res.Data = "Success";

                    }
                    else
                    {

                        res.Data = "Failed";

                    }
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
        public static Decimal ToDecimal(string number)
        {
            if (number.Length > 0)
            {
                return Convert.ToDecimal(number);
            }
            else
            {
                return 0;
            }
        }
        public static Decimal decimalToDecimal(decimal? number)
        {
            if (number!=null)
            {
                return Convert.ToDecimal(number);
            }
            else
            {
                return 0;
            }
        }

        public ActionResult ShowImportError()
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

            List<AssetImportError> lstList = new List<AssetImportError>();
            if (Session["ImportErrors"] != null)
            {
                lstList = (List<AssetImportError>)Session["ImportErrors"];
            }
        

            return View(lstList);
        }
    }
}
