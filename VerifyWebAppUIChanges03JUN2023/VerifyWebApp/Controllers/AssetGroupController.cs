using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers
{
    public class AssetGroupController : Controller
    {
        // GET: AssetGroup
        public VerifyDB db = new VerifyDB();

        // GET: Location
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
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
                    CGroup cgroup= new CGroup();
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
                        dgroup.CGrpID= Convert.ToInt32(id);
                        dgroup.DGroupName = node.GroupName;

                        db.DGroups.Add(dgroup);
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
        public JsonResult GetGroups()
        {
            List<JsTreeModel> list = new List<JsTreeModel>();

            List<AGroup> lstAgroup = new List<AGroup>();
            List<BGroup> lstBgroup = new List<BGroup>();
            List<CGroup> lstCgroup = new List<CGroup>();
            List<DGroup> lstDgroup = new List<DGroup>();

            lstAgroup = db.AGroups.ToList();

            lstBgroup = db.BGroups.ToList();

            lstCgroup = db.CGroups.ToList();
            lstDgroup = db.DGroups.ToList();

            JsTreeModel oNodeL0 = new JsTreeModel();

            oNodeL0.id = "L0-0";
            oNodeL0.text = "Location List";
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
            model.text = "Location list";
            model.parent = "#";
            model.children = false;


            list.Add(model);

            JsTreeModel model_L1 = new JsTreeModel();

            model_L1.id = "2";
            model_L1.text = "Pune";
            model_L1.parent = "1";
            model_L1.children = false;


            list.Add(model_L1);

            JsTreeModel model_L2 = new JsTreeModel();

            model_L2.id = "3";
            model_L2.text = "Hyd";
            model_L2.parent = "1";
            model_L2.children = false;


            list.Add(model_L2);

            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //[HttpPost]
        //public JsonResult EditSaveLocationNode(JsTreeNode node)
        //{
        //    bool bResult = true;
        //    try
        //    {
        //        string Level = "";
        //        Level = node.id.Substring(0, 2);
        //        int tempLength = node.id.Length;
        //        string id = node.id.Substring(3, tempLength - 3);
        //        int int_id = Convert.ToInt32(id);
        //        if (Level == "L1") // selected parent level
        //        {
        //            ALocation aLocation = new ALocation();
        //            aLocation = db.ALocations.Where(x => x.ID == int_id).FirstOrDefault();
        //            aLocation.ClientID = 1;
        //            aLocation.ALocationName = node.location;

        //            db.Entry(aLocation).State = System.Data.Entity.EntityState.Modified;
        //            db.SaveChanges();

        //        }
        //        if (Level == "L2") // selected parent level
        //        {
        //            BLocation bLocation = new BLocation();
        //            bLocation = db.BLocations.Where(x => x.ID == int_id).FirstOrDefault();
        //            bLocation.ClientID = 1;
        //            bLocation.ALocID = Convert.ToInt32(id);
        //            bLocation.BLocationName = node.location;

        //            db.Entry(bLocation).State = System.Data.Entity.EntityState.Modified;
        //            db.SaveChanges();

        //        }

        //        if (Level == "L3") // selected parent level
        //        {
        //            CLocation cLocation = new CLocation();
        //            cLocation = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault();
        //            cLocation.ClientID = 1;
        //            int tempID = Convert.ToInt32(id);
        //            BLocation bloc = db.BLocations.Where(x => x.ID == tempID).FirstOrDefault();

        //            if (bloc != null)
        //            {
        //                cLocation.ALocID = bloc.ALocID;
        //                cLocation.BLocID = Convert.ToInt32(id);
        //                cLocation.CLocationName = node.location;

        //                db.Entry(cLocation).State = System.Data.Entity.EntityState.Modified;
        //                db.SaveChanges();
        //            }


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        bResult = false;
        //    }
        //    return new JsonResult { Data = bResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        [HttpPost]
        public JsonResult GetAssetList(DataTableAjaxPostModel filter, string id)
        {
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

                    alist = db.Assetss.Where(x => x.AGroupID == int_id).ToList();
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


                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                        item.str_locationname = db.ALocations.Where(x => x.ID == int_id).FirstOrDefault().ALocationName;

                    }
                }
                if (Level == "L2") // selected parent level
                {
                    BLocation bLocation = new BLocation();
                    var alocid = db.BLocations.Where(x => x.ID == int_id).FirstOrDefault().ALocID;
                    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == int_id).ToList();
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


                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                        item.str_locationname = db.BLocations.Where(x => x.ID == int_id).FirstOrDefault().BLocationName;

                    }

                }

                if (Level == "L3") // selected parent level
                {


                    var alocid = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().ALocID;
                    var blocid = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().BLocID;
                    alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == blocid && x.ID == int_id).ToList();
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


                        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                        item.str_locationname = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().CLocationName;

                    }
                }

            }
            catch (Exception ex)
            {
                //bResult = false;
            }
            totalResultsCount = alist.Count;

            filteredResultsCount = alist.Count;

            return Json(new
            {
                // this is what datatables wants sending back
                draw = filter.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = alist
            });
        }
        [HttpGet]

        public void LocationassetExport(string id)
        {


            Response.ClearContent();
            Response.BinaryWrite(generatelocationexcel(id));
            string excelName = "Location Assets";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();


        }
        public byte[] generatelocationexcel(string id)
        {
            List<Assets> alist = new List<Assets>();
            int srno = 1;
            string Level = "";
            Level = id.Substring(0, 2);
            int tempLength = id.Length;
            string strr_id = id.Substring(3, tempLength - 3);
            int int_id = Convert.ToInt32(strr_id);

            //lstins = db.AMCss.ToList();

            if (Level == "L1") // selected parent level
            {

                alist = db.Assetss.Where(x => x.LocAID == int_id).ToList();
            }
            if (Level == "L2") // selected parent level
            {
                BLocation bLocation = new BLocation();
                var alocid = db.BLocations.Where(x => x.ID == int_id).FirstOrDefault().ALocID;
                alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == int_id).ToList();
            }
            if (Level == "L3") // selected parent level
            {


                var alocid = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().ALocID;
                var blocid = db.CLocations.Where(x => x.ID == int_id).FirstOrDefault().BLocID;
                alist = db.Assetss.Where(x => x.LocAID == alocid && x.LocBID == blocid && x.ID == int_id).ToList();
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



                    worksheet.Cells[rowIterator, 1].Value = item.ID;
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
                    item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                    item.str_locationname = db.BLocations.Where(x => x.ID == int_id).FirstOrDefault().BLocationName;
                    worksheet.Cells[rowIterator, 8].Value = item.str_locationname;
                    worksheet.Cells[rowIterator, 9].Value = item.str_suppliername;
                    worksheet.Cells[rowIterator, 10].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 8].Value = item.Model;
                    worksheet.Cells[rowIterator, 9].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 10].Value = item.MRRNo;
                    if (item.DtPutToUse == null)
                    {
                        item.str_DtPutToUse = "";
                        worksheet.Cells[rowIterator, 11].Value = item.str_DtPutToUse;
                    }
                    else
                    {


                        item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
                        worksheet.Cells[rowIterator, 11].Value = item.str_DtPutToUse;

                    }
                    rowIterator = rowIterator + 1;





                }

                return excel.GetAsByteArray();

            }
        }
    }
}
