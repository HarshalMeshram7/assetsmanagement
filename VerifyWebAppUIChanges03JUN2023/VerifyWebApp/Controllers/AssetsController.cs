using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;


namespace VerifyWebApp.Controllers
{

    public class AssetsController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Assets
        public ActionResult Index()
        {
            List<Assets> lstasset = new List<Assets>();
            int srno = 1;
            try
            {

                lstasset = db.Assetss.ToList();

                foreach (var item in lstasset)
                {
                    item.Srno = srno;
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
                    if (item.CommissioningDate == null)
                    {
                        item.str_CommissioningDate = "";
                    }
                    else
                    {

                        item.str_CommissioningDate = Convert.ToDateTime(item.CommissioningDate).ToString("dd/MM/yyyy");

                    }
                    if (item.ReceiptDate == null)
                    {
                        item.str_ReceiptDate = "";
                    }
                    else
                    {

                        item.str_ReceiptDate = Convert.ToDateTime(item.ReceiptDate).ToString("dd/MM/yyyy");

                    }
                    if (item.BillDate == null)
                    {
                        item.str_BillDate = "";
                    }
                    else
                    {

                        item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                    }
                    if (item.PODate == null)
                    {
                        item.str_PODate = "";
                    }
                    else
                    {

                        item.str_PODate = Convert.ToDateTime(item.PODate).ToString("dd/MM/yyyy");

                    }
                    if (item.VoucherDate == null)
                    {
                        item.str_VoucherDate = "";
                    }
                    else
                    {

                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                    }
                    if (item.ExpiryDate == null)
                    {
                        item.str_Expirydate = "";
                    }
                    else
                    {

                        item.str_Expirydate = Convert.ToDateTime(item.ExpiryDate).ToString("dd/MM/yyyy");


                    }

                    item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo).FirstOrDefault().SupplierName;
                    item.uom_name = db.UOMs.Where(x => x.ID == item.UOMNo).FirstOrDefault().Unit;

                    srno++;
                }
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            ViewBag.Srno = srno;

            return View(lstasset);
            // return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            int userid = 0;

            // Addition addition = new Addition();
            var addition = db.Assetss.Max(x => x.ID == 0 ? 0 : x.ID);
            if (addition == null)
            {
                ViewBag.additionno = 1;
            }
            else
            {
                ViewBag.additionno = addition + 1;
            }

            //ViewBag.Assestlist = new SelectList(db.Assetss.OrderBy(e => e.ID), "ID", "AssetName");
            ViewBag.supplierlist = new SelectList(db.Suppliers.OrderBy(e => e.ID), "ID", "SupplierName");
            ViewBag.uomlist = new SelectList(db.UOMs.OrderBy(e => e.ID), "ID", "Unit");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNew(Assets assets)
        {

            JsonResult res;
            res = new JsonResult();

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            try
            {

                db.Assetss.Add(assets);
                res.Data = "Success";
                db.SaveChanges();
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
        private DateTime ParseExact(string str_fromdate, string v, CultureInfo invariantCulture)
        {
            throw new NotImplementedException();
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {

            Assets assets = new Assets();

            try
            {
                assets = db.Assetss.Where(x => x.ID == id).FirstOrDefault();
                //amc = db.AMCss.Where(x => x.ID == id).FirstOrDefault();
                ViewBag.Assestlist = new SelectList(db.Assetss.OrderBy(e => e.ID), "ID", "AssetName");
                ViewBag.supplierlist = new SelectList(db.Suppliers.OrderBy(e => e.ID), "ID", "SupplierName");
                ViewBag.uomlist = new SelectList(db.UOMs.OrderBy(e => e.ID), "ID", "Unit");

                assets.ID = assets.ID;
                ViewBag.supplierno = assets.SupplierNo;
                ViewBag.uomno = assets.UOMNo;
                ViewBag.assetno = assets.ID;
                if (assets.DtPutToUse == null)
                {
                    assets.str_DtPutToUse = "";
                }
                else
                {


                    assets.str_DtPutToUse = Convert.ToDateTime(assets.DtPutToUse).ToString("dd/MM/yyyy");

                }
                if (assets.DtPutToUseIT == null)
                {
                    assets.str_DtPutToUseIT = "";
                }
                else
                {

                    assets.str_DtPutToUseIT = Convert.ToDateTime(assets.DtPutToUseIT).ToString("dd/MM/yyyy");

                }
                if (assets.CommissioningDate == null)
                {
                    assets.str_CommissioningDate = "";
                }
                else
                {

                    assets.str_CommissioningDate = Convert.ToDateTime(assets.CommissioningDate).ToString("dd/MM/yyyy");

                }
                if (assets.ReceiptDate == null)
                {
                    assets.str_ReceiptDate = "";
                }
                else
                {

                    assets.str_ReceiptDate = Convert.ToDateTime(assets.ReceiptDate).ToString("dd/MM/yyyy");

                }
                if (assets.BillDate == null)
                {
                    assets.str_BillDate = "";
                }
                else
                {

                    assets.str_BillDate = Convert.ToDateTime(assets.BillDate).ToString("dd/MM/yyyy");

                }
                if (assets.PODate == null)
                {
                    assets.str_PODate = "";
                }
                else
                {

                    assets.str_PODate = Convert.ToDateTime(assets.PODate).ToString("dd/MM/yyyy");

                }
                if (assets.VoucherDate == null)
                {
                    assets.str_VoucherDate = "";
                }
                else
                {

                    assets.str_VoucherDate = Convert.ToDateTime(assets.VoucherDate).ToString("dd/MM/yyyy");

                }
                if (assets.ExpiryDate == null)
                {
                    assets.str_Expirydate = "";
                }
                else
                {

                    assets.str_Expirydate = Convert.ToDateTime(assets.ExpiryDate).ToString("dd/MM/yyyy");


                }
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
            return View(assets);
        }
        [HttpPost]
        public ActionResult Edit(Assets assets)
        {
            JsonResult res;
            res = new JsonResult();


            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);

            try
            {

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
    }
}