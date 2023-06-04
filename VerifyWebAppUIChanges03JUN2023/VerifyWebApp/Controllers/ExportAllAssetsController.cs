using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class ExportAllAssetsController : Controller
    {
        // GET: ExportAllAssets
        public VerifyDB db = new VerifyDB();
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
            List<Assets> alist = new List<Assets>();
            alist = db.Assetss.Where(x => x.Companyid == companyid).ToList();
            foreach (Assets item in alist)
            {
                item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
            }

            return View(alist);
            // return View();
        }

        //[HttpGet]
        //public ActionResult exportassets()
        //{
        //    int userid = 0;
        //    Login user = (Login)(Session["PUser"]);

        //    if (user != null)
        //    {
        //        ViewBag.LogonUser = user.UserName;
        //        userid = user.ID;
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Login");
        //    }
        //    int companyid = 0;
        //    Company company = (Company)(Session["Cid"]);

        //    if (company != null)
        //    {
        //        ViewBag.LoggedCompany = company.CompanyName;
        //        companyid = company.ID;
        //    }
        //    else
        //    {
        //        return RedirectToAction("CompanySelection", "Company");
        //    }


        //    Response.ClearContent();
        //    Response.BinaryWrite(generateaassetsexcel(companyid));
        //    string excelName = "All Assets";
        //    Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Flush();
        //    Response.End();
        //    return RedirectToAction("Index", "ExportAllAssets");

        //}

        [HttpPost]
        public ActionResult exportassets()
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
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }


            List<Assets> alst = new List<Assets>();



            alst = db.Assetss.Where(x => x.Companyid == companyid).ToList();


            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[]
                   {
                      "ID", "DisposalFlag", "CreatedUserId", "Companyid", "Modified_Userid", "CreatedDate", "ModifiedDate",
                        "AssetNo","ClientID","AssetIdentificationNo","AssetName","VoucherNo","VoucherDate","PODate",
                    "ReceiptDate","CommissioningDate","BillDate","DtPutToUse","DtPutToUseIT","PONo","BillNo","MRRNo","Qty","SupplierNo",
                        "UOMNo","OPAccDepreciation","GrossVal","ServiceCharges","OtherExp","CustomDuty","ExciseDuty","ServiceTax","AnyOtherDuty",
                    "VAT","CSt","CGST","SGST","IGST","AnyOtherTax","TotalAddition","Discount","Roundingoff","TotDeduction","InvoiceAmt",
                    "DutyDrawback","ExciseCredit","ServiceTaxCredit","AnyOtherDutyCredit","VATCredit","CSTCredit","CGSTCredit","SGSTCredit","IGSTCredit",
                        "AnyOtherCredit","TotalCredit","AmountCapitalised","AmountCapitalisedCompany","AmountCApitalisedIT","AGroupID","BGroupID","CGroupID","DGroupID",
                        "LocAID","LocBID","LocCID","CostCenterAID","CostCenterBID","ITGroupIDID","DepreciationMethod","NormalRate","AdditionalRate","TotalRate","ResidualVal",
                    "Usefullife","YrofManufacturing","ExpiryDate","AccountID","DepAccountId","AccAccountID","BrandName","SrNo","Model","Remarks","IsImported",
                    "Currency","Values","Parent_AssetId","iscomponent"
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


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
                // Popular header row data
               // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                int rowIterator = 2;
                foreach (var item in alst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.DisposalFlag;
                    worksheet.Cells[rowIterator, 3].Value = item.CreatedUserId;
                    worksheet.Cells[rowIterator, 4].Value = item.Companyid;
                    worksheet.Cells[rowIterator, 5].Value = item.Modified_Userid;
                    worksheet.Cells[rowIterator, 6].Value = item.CreatedDate;
                    worksheet.Cells[rowIterator, 7].Value = item.ModifiedDate;
                    worksheet.Cells[rowIterator, 8].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 9].Value = item.ClientID;
                    worksheet.Cells[rowIterator, 10].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 11].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 12].Value = item.VoucherNo;
                    worksheet.Cells[rowIterator, 13].Value = item.VoucherDate;
                    worksheet.Cells[rowIterator, 14].Value = item.PODate;
                    worksheet.Cells[rowIterator, 15].Value = item.ReceiptDate;
                    worksheet.Cells[rowIterator, 16].Value = item.CommissioningDate;
                    worksheet.Cells[rowIterator, 17].Value = item.BillDate;
                    worksheet.Cells[rowIterator, 18].Value = item.DtPutToUse;
                    worksheet.Cells[rowIterator, 19].Value = item.DtPutToUseIT;
                    worksheet.Cells[rowIterator, 20].Value = item.PONo;
                    worksheet.Cells[rowIterator, 21].Value = item.BillNo;
                    worksheet.Cells[rowIterator, 22].Value = item.MRRNo;
                    worksheet.Cells[rowIterator, 23].Value = item.Qty;
                    worksheet.Cells[rowIterator, 24].Value = item.SupplierNo;
                    worksheet.Cells[rowIterator, 25].Value = item.UOMNo;
                    worksheet.Cells[rowIterator, 26].Value = item.OPAccDepreciation;
                    worksheet.Cells[rowIterator, 27].Value = item.GrossVal;
                    worksheet.Cells[rowIterator, 28].Value = item.ServiceCharges;
                    worksheet.Cells[rowIterator, 29].Value = item.OtherExp;
                    worksheet.Cells[rowIterator, 30].Value = item.CustomDuty;
                    worksheet.Cells[rowIterator, 31].Value = item.ExciseDuty;
                    worksheet.Cells[rowIterator, 32].Value = item.ServiceTax;
                    worksheet.Cells[rowIterator, 33].Value = item.AnyOtherDuty;
                    worksheet.Cells[rowIterator, 34].Value = item.VAT;
                    worksheet.Cells[rowIterator, 35].Value = item.CSt;
                    worksheet.Cells[rowIterator, 36].Value = item.CGST;
                    worksheet.Cells[rowIterator, 37].Value = item.SGST;
                    worksheet.Cells[rowIterator, 38].Value = item.IGST;
                    worksheet.Cells[rowIterator, 39].Value = item.AnyOtherTax;
                    worksheet.Cells[rowIterator, 40].Value = item.TotalAddition;
                    worksheet.Cells[rowIterator, 41].Value = item.Discount;
                    worksheet.Cells[rowIterator, 42].Value = item.Roundingoff;
                    worksheet.Cells[rowIterator, 43].Value = item.TotDeduction;
                    worksheet.Cells[rowIterator, 44].Value = item.InvoiceAmt;
                    worksheet.Cells[rowIterator, 45].Value = item.DutyDrawback;
                    worksheet.Cells[rowIterator, 46].Value = item.ExciseCredit;
                    worksheet.Cells[rowIterator, 47].Value = item.ServiceTaxCredit;
                    worksheet.Cells[rowIterator, 48].Value = item.AnyOtherDutyCredit;
                    worksheet.Cells[rowIterator, 49].Value = item.VATCredit;
                    worksheet.Cells[rowIterator, 50].Value = item.CSTCredit;
                    worksheet.Cells[rowIterator, 51].Value = item.CGSTCredit;
                    worksheet.Cells[rowIterator, 52].Value = item.SGSTCredit;
                    worksheet.Cells[rowIterator, 53].Value = item.IGSTCredit;
                    worksheet.Cells[rowIterator, 54].Value = item.AnyOtherCredit;
                    worksheet.Cells[rowIterator, 55].Value = item.TotalCredit;
                    worksheet.Cells[rowIterator, 56].Value = item.AmountCapitalised;
                    worksheet.Cells[rowIterator, 57].Value = item.AmountCapitalisedCompany;
                    worksheet.Cells[rowIterator, 58].Value = item.AmountCApitalisedIT;
                    worksheet.Cells[rowIterator, 59].Value = item.AGroupID;
                    worksheet.Cells[rowIterator, 60].Value = item.BGroupID;
                    worksheet.Cells[rowIterator, 61].Value = item.CGroupID;
                    worksheet.Cells[rowIterator, 62].Value = item.DGroupID;
                    worksheet.Cells[rowIterator, 63].Value = item.LocAID;
                    worksheet.Cells[rowIterator, 64].Value = item.LocBID;
                    worksheet.Cells[rowIterator, 65].Value = item.LocCID;
                    worksheet.Cells[rowIterator, 66].Value = item.CostCenterAID;
                    worksheet.Cells[rowIterator, 67].Value = item.CostCenterBID;
                    worksheet.Cells[rowIterator, 68].Value = item.ITGroupIDID;
                    worksheet.Cells[rowIterator, 69].Value = item.DepreciationMethod;
                    worksheet.Cells[rowIterator, 70].Value = item.NormalRatae;
                    worksheet.Cells[rowIterator, 71].Value = item.AdditionalRate;
                    worksheet.Cells[rowIterator, 72].Value = item.TotalRate;
                    worksheet.Cells[rowIterator, 73].Value = item.ResidualVal;
                    worksheet.Cells[rowIterator, 74].Value = item.Usefullife;
                    worksheet.Cells[rowIterator, 75].Value = item.YrofManufacturing;
                    worksheet.Cells[rowIterator, 76].Value = item.ExpiryDate;
                    worksheet.Cells[rowIterator, 77].Value = item.AccountID;
                    worksheet.Cells[rowIterator, 78].Value = item.DepAccountId;
                    worksheet.Cells[rowIterator, 79].Value = item.AccAccountID;
                    worksheet.Cells[rowIterator, 80].Value = item.BrandName;
                    worksheet.Cells[rowIterator, 81].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 82].Value = item.Model;
                    worksheet.Cells[rowIterator, 83].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 84].Value = item.IsImported;
                    worksheet.Cells[rowIterator, 85].Value = item.Currency;
                    worksheet.Cells[rowIterator, 86].Value = item.Values;
                    worksheet.Cells[rowIterator, 87].Value = item.Parent_AssetId;
                    worksheet.Cells[rowIterator, 88].Value = item.iscomponent;




                    rowIterator = rowIterator + 1;

                }


                string excelName = "Allassets.xlsx";

                string handle = Guid.NewGuid().ToString();
                excel.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();










                // Note we are returning a filename as well as the handle
                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = excelName }
                };
            }
        }
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
    }
}