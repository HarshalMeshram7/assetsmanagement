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
    public class Export_All_ContentController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Export_All_Content
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


            Response.ClearContent();
            Response.BinaryWrite(generateallexcel( companyid));
            string excelName = "GroupAsset";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "Asset");

            
        }

            public byte[] generateallexcel( int companyid)
        {
            ////intialisation of lists
            List<Assets> assetpurchasedlst = new List<Assets>();
            List<UOM> uomlst = new List<UOM>();
            List<Supplier> splist = new List<Supplier>();
            List<Account> acclst = new List<Account>();
            List<Period> plst = new List<Period>();
            List<ITPeriod> itperiodlst = new List<ITPeriod>();
            List<Assets> locationlst = new List<Assets>();
            List<Assets> costcenterlst = new List<Assets>();
            List<Batch> blist = new List<Batch>();
            List<Loan> loanlst = new List<Loan>();
            List<Disposal> disposallst = new List<Disposal>();
            List<Depreciation> depreciationlst = new List<Depreciation>();
            List<AMC> amclst = new List<AMC>();
            List<Insurance> insurancelst = new List<Insurance>();
            List<ITGroup> itgroup = new List<ITGroup>();
            List<SubAmc> subamc = new List<SubAmc>();
            List<SubInsurance> subinc = new List<SubInsurance>();
            List<Subbatch> subbatch = new List<Subbatch>();
            List<SubLoan> subloan = new List<SubLoan>();
            List<SubPeriod> subperiod = new List<SubPeriod>();
            List<Childlocation> childloclist = new List<Childlocation>();
            List<Childcostcenter> Childcos = new List<Childcostcenter>();
            ////////calling and getting list logic/////////////////////////////////////////////////////////////////
            BusinessLogic.Export_Repository export_Repository = new BusinessLogic.Export_Repository();
            assetpurchasedlst = export_Repository.getassets(companyid);////all assets
            disposallst = export_Repository.getdisposallist(companyid);
            depreciationlst = export_Repository.getdepreciationlist(companyid);
            acclst = export_Repository.getaccountlist(companyid);
            uomlst = db.UOMs.Where(x => x.Companyid == companyid).ToList();
            splist = db.Suppliers.Where(x => x.Companyid == companyid).ToList();
            amclst = export_Repository.getamclist(companyid);
            insurancelst = export_Repository.getinsurancelist(companyid);
            loanlst = export_Repository.getloanlist(companyid);
            plst = export_Repository.getperiodlist(companyid);
            blist = export_Repository.getbatchlist(companyid);
            itgroup = export_Repository.getitgrouplist(companyid);
            itperiodlst = export_Repository.getitperiodlist(companyid);
            childloclist = export_Repository.getchildlocationlist(companyid);
            Childcos = export_Repository.getchildcostcenterlist(companyid);
            subamc = export_Repository.getsubamclist(companyid);
            subinc = export_Repository.getsubinsurancelist(companyid);
            subloan = export_Repository.getsubloanlist(companyid);
            subperiod = export_Repository.getsubperiodlist(companyid);
            subbatch = export_Repository.getsubbatchlist(companyid);
            //////////////////////////////////////////////////////////////////////////////
            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                //////////////////////////////assetpurchased///////////////////////
                excel.Workbook.Worksheets.Add("AssetPurchased");
               
                //  excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow = new string[]
                   {
                      "ID", "DisposalFlag", "CreatedUsername", "Companyname", "Modified_Username", "CreatedDate", "ModifiedDate",
                        "AssetNo","ClientID","AssetIdentificationNo","AssetName","VoucherNo","VoucherDate","PODate",
                    "ReceiptDate","CommissioningDate","BillDate","DtPutToUse","DtPutToUseIT","PONo","BillNo","MRRNo","Qty","SupplierNo",
                        "UOMNo","OPAccDepreciation","GrossVal","ServiceCharges","OtherExp","CustomDuty","ExciseDuty","ServiceTax","AnyOtherDuty",
                    "VAT","CSt","CGST","SGST","IGST","AnyOtherTax","TotalAddition","Discount","Roundingoff","TotDeduction","InvoiceAmt",
                    "DutyDrawback","ExciseCredit","ServiceTaxCredit","AnyOtherDutyCredit","VATCredit","CSTCredit","CGSTCredit","SGSTCredit","IGSTCredit",
                        "AnyOtherCredit","TotalCredit","AmountCapitalised","AmountCapitalisedCompany","AmountCApitalisedIT","AGroupname","BGroupname","CGroupname","DGroupname",
                        "MainLocation","Sub_Location","Sub_Sublocation","CostCenterAname","CostCenterBname","ITGroupname","DepreciationMethod","NormalRate","AdditionalRate","TotalRate","ResidualVal",
                    "Usefullife","YrofManufacturing","ExpiryDate","Accountname","DepAccountname","AccAccountname","BrandName","Product Serial no","Model","Remarks","IsImported",
                    "Currency","Values","Parent_AssetNo","ParentAssetName","iscomponent"
                  };


                // Determine the header range (e.g. A1:D1)
                string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["AssetPurchased"];

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
                foreach (var item in assetpurchasedlst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    if (item.DisposalFlag == 0)
                    {
                        worksheet.Cells[rowIterator, 2].Value = "no";
                    }
                    if (item.DisposalFlag == 1)
                    {
                        worksheet.Cells[rowIterator, 2].Value = "yes";
                    }

                    worksheet.Cells[rowIterator, 3].Value = item.Createdusername;
                    worksheet.Cells[rowIterator, 4].Value = item.companyname;
                    worksheet.Cells[rowIterator, 5].Value = item.Modified_Userid;
                    worksheet.Cells[rowIterator, 6].Value = item.CreatedDate;
                    worksheet.Cells[rowIterator, 7].Value = item.ModifiedDate;
                    worksheet.Cells[rowIterator, 8].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 9].Value = item.ClientID;
                    worksheet.Cells[rowIterator, 10].Value = item.AssetIdentificationNo;
                    worksheet.Cells[rowIterator, 11].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 12].Value = item.VoucherNo;
                    worksheet.Cells[rowIterator, 13].Value = item.str_VoucherDate;
                    worksheet.Cells[rowIterator, 14].Value = item.str_PODate;
                    worksheet.Cells[rowIterator, 15].Value = item.str_ReceiptDate;
                    worksheet.Cells[rowIterator, 16].Value = item.str_CommissioningDate;
                    worksheet.Cells[rowIterator, 17].Value = item.str_BillDate;
                    worksheet.Cells[rowIterator, 18].Value = item.str_DtPutToUse;
                    worksheet.Cells[rowIterator, 19].Value = item.str_DtPutToUseIT;
                    worksheet.Cells[rowIterator, 20].Value = item.PONo;
                    worksheet.Cells[rowIterator, 21].Value = item.BillNo;
                    worksheet.Cells[rowIterator, 22].Value = item.MRRNo;
                    worksheet.Cells[rowIterator, 23].Value = item.Qty;
                    worksheet.Cells[rowIterator, 24].Value = item.str_suppliername;
                    worksheet.Cells[rowIterator, 25].Value = item.uom_name;
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
                    worksheet.Cells[rowIterator, 59].Value = item.agroupname;
                    worksheet.Cells[rowIterator, 60].Value = item.bgroupname;
                    worksheet.Cells[rowIterator, 61].Value = item.cgroupname;
                    worksheet.Cells[rowIterator, 62].Value = item.dgroupname;
                    worksheet.Cells[rowIterator, 63].Value = item.str_mainlocation;
                    worksheet.Cells[rowIterator, 64].Value = item.str_sublocation;
                    worksheet.Cells[rowIterator, 65].Value = item.str_sub_sublocation;
                    worksheet.Cells[rowIterator, 66].Value = item.str_costcenteraname;
                    worksheet.Cells[rowIterator, 67].Value = item.str_costcenterbname;
                    worksheet.Cells[rowIterator, 68].Value = item.ITGroupIDID;
                    worksheet.Cells[rowIterator, 69].Value = item.DepreciationMethod;
                    worksheet.Cells[rowIterator, 70].Value = item.NormalRatae;
                    worksheet.Cells[rowIterator, 71].Value = item.AdditionalRate;
                    worksheet.Cells[rowIterator, 72].Value = item.TotalRate;
                    worksheet.Cells[rowIterator, 73].Value = item.ResidualVal;
                    worksheet.Cells[rowIterator, 74].Value = item.Usefullife;
                    worksheet.Cells[rowIterator, 75].Value = item.YrofManufacturing;
                    worksheet.Cells[rowIterator, 76].Value = item.str_Expirydate;
                    worksheet.Cells[rowIterator, 77].Value = item.str_purchaseaccountname;
                    worksheet.Cells[rowIterator, 78].Value = item.str_depricationname;
                    worksheet.Cells[rowIterator, 79].Value = item.str_accumulatedname;
                    worksheet.Cells[rowIterator, 80].Value = item.BrandName;
                    worksheet.Cells[rowIterator, 81].Value = item.SrNo;
                    worksheet.Cells[rowIterator, 82].Value = item.Model;
                    worksheet.Cells[rowIterator, 83].Value = item.Remarks;
                    worksheet.Cells[rowIterator, 84].Value = item.IsImported;
                    worksheet.Cells[rowIterator, 85].Value = item.Currency;
                    worksheet.Cells[rowIterator, 86].Value = item.Values;
                    worksheet.Cells[rowIterator, 87].Value = item.Parent_assetno;
                    worksheet.Cells[rowIterator, 88].Value = item.ParentAssetName;
                    worksheet.Cells[rowIterator, 89].Value = item.iscomponent;

                    rowIterator = rowIterator + 1;

                }
                /////////////////////////////////////////////////////////////////////////////////////////////////

                ////////////////////////////Disposal////////////////
                excel.Workbook.Worksheets.Add("Disposal");
                headerRow = new string[] { "Sr No","ID ","Asset No","Asset Name", "DisposalDate", "VoucherNo", "Voucher Date","Bill Date ",
                                     "Disposal Type","Qty","Disposal Amount","Gross Value","OP accumulated Depreciation","Total Depreciation","Net Amount","Profitloss"


                };


                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                 worksheet = excel.Workbook.Worksheets["Disposal"];

                currentSheet = excel.Workbook.Worksheets;

                 excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                 rowIterator = 2;
                foreach (Disposal item in disposallst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.int_Srno;
                    worksheet.Cells[rowIterator, 2].Value = item.ID;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 4].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 5].Value = item.str_disposalDate;
                    worksheet.Cells[rowIterator, 6].Value = item.VoucherNo;
                    worksheet.Cells[rowIterator, 7].Value = item.str_voucherDate;
                    worksheet.Cells[rowIterator, 8].Value = item.str_billDate;
                    worksheet.Cells[rowIterator, 9].Value = item.DisposalType;
                    worksheet.Cells[rowIterator, 10].Value = item.Qty;
                    worksheet.Cells[rowIterator, 11].Value = item.DisposalAmount;
                    worksheet.Cells[rowIterator, 12].Value = item.GrossAmount;
                    worksheet.Cells[rowIterator, 13].Value = item.OpAccumulatedDep;
                    worksheet.Cells[rowIterator, 14].Value = item.TotalDepreciation;
                    worksheet.Cells[rowIterator, 15].Value = item.WDvDisposedOff;
                    worksheet.Cells[rowIterator, 16].Value = item.ProfitLoss;



                    rowIterator = rowIterator + 1;

                }
                //////////////////////////////////////////////////
                ////////////////////////////Depreciation//////////
                excel.Workbook.Worksheets.Add("Depreciation");
                headerRow = new string[] { "Sr No","ID ","Asset No","Asset Name", "DisposalDate", "VoucherNo", "Voucher Date","Bill Date ",
                                     "Disposal Type","Qty","Disposal Amount","Gross Value","OP accumulated Depreciation","Total Depreciation","Net Amount","Profitloss"


                };


                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["Depreciation"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (Depreciation item in depreciationlst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.int_Srno;
                    worksheet.Cells[rowIterator, 2].Value = item.ID;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 4].Value = item.AssetName;
                    worksheet.Cells[rowIterator, 5].Value = item.str_FromDate;
                    worksheet.Cells[rowIterator, 6].Value = item.str_ToDate;
                    worksheet.Cells[rowIterator, 7].Value = item.Amount;
                    worksheet.Cells[rowIterator, 8].Value = item.TotalRate;
                    worksheet.Cells[rowIterator, 9].Value = item.DepreciationMethod;
                    worksheet.Cells[rowIterator, 10].Value = item.DepreciationDays;
                    rowIterator = rowIterator + 1;

                }
                ///////////////////////Childlocation//////////////////
                excel.Workbook.Worksheets.Add("Assetlocation");
                headerRow =
                   new string[] { "ID", "AssetNo","Asset Name", "MainLocation Name", "Sub location Name", "Sub_sub location Name", "MainLocation ID", "Sub location ID", "Sub_sub location ID"
                  };

                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["Assetlocation"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in childloclist)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.assetno;
                    worksheet.Cells[rowIterator, 3].Value = item.assetname;
                    worksheet.Cells[rowIterator, 4].Value = item.str_locaname;
                    worksheet.Cells[rowIterator, 5].Value = item.str_locbname;
                    worksheet.Cells[rowIterator, 6].Value = item.str_loccname;
                    worksheet.Cells[rowIterator, 7].Value = item.ALocID;
                    worksheet.Cells[rowIterator, 8].Value = item.BLocID;
                    worksheet.Cells[rowIterator, 9].Value = item.CLocID;
                    rowIterator = rowIterator + 1;
                }

                /////////////////////////////////////////
                ///////////////////////Childcostcenter//////////////////
                excel.Workbook.Worksheets.Add("AssetCostcenter");
                headerRow =
                   new string[] { "ID", "AssetNo","Asset Name", "Costcenter Name", "Sub Costcenter Name", "Costcenter ID", "Sub Costcenter ID"
                  };

                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["AssetCostcenter"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in Childcos)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.assetno;
                    worksheet.Cells[rowIterator, 3].Value = item.assetname;
                    worksheet.Cells[rowIterator, 4].Value = item.str_costcenteraname;
                    worksheet.Cells[rowIterator, 5].Value = item.str_costcenterbname;
                    worksheet.Cells[rowIterator, 6].Value = item.AcostcenterID;
                    worksheet.Cells[rowIterator, 7].Value = item.BcostcenterID;
                 
                    rowIterator = rowIterator + 1;
                }





                ///////////////////////////////////////



                ///////////////////////////////////////
                /////////////////////////////////////////////////Accounts////////////////////
                excel.Workbook.Worksheets.Add("Accounts");
                headerRow = new string[] { "ID ","Account Code","Account Name", "Groupname", "ClientId" 


                };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["Accounts"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (Account item in acclst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.AccountCode;
                    worksheet.Cells[rowIterator, 3].Value = item.AccountName;
                    worksheet.Cells[rowIterator, 4].Value = item.GroupName;
                    worksheet.Cells[rowIterator, 5].Value = item.ClientID;
              
                    rowIterator = rowIterator + 1;

                }
                /////Uom////////////////
                excel.Workbook.Worksheets.Add("Uom");
                headerRow = new string[] { "ID","Unit","ClientId"


                };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["Uom"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (UOM item in uomlst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.Unit;
                    worksheet.Cells[rowIterator, 3].Value = item.ClientID;
                   

                    rowIterator = rowIterator + 1;

                }
                ////////////////////////////////////////////////////////////////////
                //////////////////////////supplier//////////////////////////////////////////////////
                excel.Workbook.Worksheets.Add("Supplier");
                headerRow = new string[] { "Sr No", "SupplierCode", "Supplier Name","Contact Person ","Address ","Address 2","Address 3",
                        "City","Pincode","Phone No","Mobile No","Fax No","Excise No",
                        "Service No","Vat No","Cst No","Other No","Pan No","Tan No","Emailid","Gst No","Shop Act License"


                };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["Supplier"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (Supplier item in splist)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.Srno;
                    worksheet.Cells[rowIterator, 2].Value = item.SupplierCode;
                    worksheet.Cells[rowIterator, 3].Value = item.SupplierName;
                    worksheet.Cells[rowIterator, 4].Value = item.ContactPerson;
                    worksheet.Cells[rowIterator, 5].Value = item.Address;
                    worksheet.Cells[rowIterator, 6].Value = item.Address2;
                    worksheet.Cells[rowIterator, 7].Value = item.Address3;
                    worksheet.Cells[rowIterator, 8].Value = item.City;
                    worksheet.Cells[rowIterator, 9].Value = item.Pincode;
                    worksheet.Cells[rowIterator, 10].Value = item.PhoneNo;
                    worksheet.Cells[rowIterator, 11].Value = item.MobileNo;
                    worksheet.Cells[rowIterator, 12].Value = item.FaxNo;
                    worksheet.Cells[rowIterator, 13].Value = item.ExciseRegNo;
                    worksheet.Cells[rowIterator, 14].Value = item.ServiceTaxRegNo;
                    worksheet.Cells[rowIterator, 15].Value = item.VATRegNo;
                    worksheet.Cells[rowIterator, 16].Value = item.CSTRegNo;
                    worksheet.Cells[rowIterator, 17].Value = item.AnyOtherRegNo;
                    worksheet.Cells[rowIterator, 18].Value = item.PANNo;
                    worksheet.Cells[rowIterator, 19].Value = item.TANNo;
                    worksheet.Cells[rowIterator, 20].Value = item.EmailID;
                    worksheet.Cells[rowIterator, 21].Value = item.GSTNo;
                    worksheet.Cells[rowIterator, 22].Value = item.ServiceTaxRegNo;

                    rowIterator = rowIterator + 1;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////Amc///////////////////////////////////////////////////////////////////
                excel.Workbook.Worksheets.Add("AMC");
                headerRow = 
                    new string[] {"Id", "FromDate", "ToDate", "Reminder Mail", "Amc Details", "Remarks"
                  };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["AMC"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in amclst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 3].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 4].Value = item.ReminderEMail;
                    worksheet.Cells[rowIterator, 5].Value = item.AMCDetails;
                    worksheet.Cells[rowIterator, 6].Value = item.Remarks;
                    rowIterator = rowIterator + 1;

                }
                ////////////////////////////////////////////////////////////
                //////////////////////SubAmc///////////////////////////////////////////////////////////////////
                excel.Workbook.Worksheets.Add("Asset_AMC");
                headerRow =
                    new string[] {"Id", "AssetNo", "Asset Description", "Capitalised Amount", "Amc ID"
                  };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["Asset_AMC"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in subamc)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.Id;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetDescription;
                    worksheet.Cells[rowIterator, 4].Value = item.CapitalisedAmount;
                    worksheet.Cells[rowIterator, 5].Value = item.AmcId;
                  
                    rowIterator = rowIterator + 1;

                }
                ////////////////////////////////////////////////////////////
                //////////////////////insurance///////////////////////

                excel.Workbook.Worksheets.Add("Insurance");
                headerRow =
                   new string[] { "ID", "FromDate", "ToDate", "Reminder Mail", "Policy Details", "Remarks"
                  };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["Insurance"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in insurancelst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 3].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 4].Value = item.EMailID;
                    worksheet.Cells[rowIterator, 5].Value = item.PolicyDetails;
                    worksheet.Cells[rowIterator, 6].Value = item.Remarks;
                    rowIterator = rowIterator + 1;

                }

                ////////////////////////////////
                //////////////////////SubInsurance///////////////////////////////////////////////////////////////////
                excel.Workbook.Worksheets.Add("AssetInsurance");
                headerRow =
                    new string[] {"Id", "AssetNo", "Asset Description", "Capitalised Amount", "Insurance ID"
                  };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["AssetInsurance"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in subinc)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.Id;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetDescription;
                    worksheet.Cells[rowIterator, 4].Value = item.CapitalisedAmount;
                    worksheet.Cells[rowIterator, 5].Value = item.InsuranceId;

                    rowIterator = rowIterator + 1;

                }
                ////////////////////////////////////////////////////////////
                /////////////////loan/////////////////
                excel.Workbook.Worksheets.Add("Loan");
                headerRow =
                   new string[] { "Id", "FromDate", "ToDate", "Loan Taken from", "Year", "Percent","Amount","ClientID"
                  };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 
      
                worksheet = excel.Workbook.Worksheets["Loan"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in loanlst)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 3].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 4].Value = item.BankName;
                    worksheet.Cells[rowIterator, 5].Value = item.Year;
                    worksheet.Cells[rowIterator, 6].Value = item.Percent;
                    worksheet.Cells[rowIterator, 7].Value = item.Amount;
                    worksheet.Cells[rowIterator, 8].Value = item.ClientID;
                    rowIterator = rowIterator + 1;

                }

                ////////////////////////////////////
                //////////////////////SubLoan///////////////////////////////////////////////////////////////////
                excel.Workbook.Worksheets.Add("AssetLoan");
                headerRow =
                    new string[] {"Id", "AssetNo", "Asset Description", "Capitalised Amount", "Insurance ID"
                  };
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                worksheet = excel.Workbook.Worksheets["AssetLoan"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in subloan)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.Id;
                    worksheet.Cells[rowIterator, 2].Value = item.AssetNo;
                    worksheet.Cells[rowIterator, 3].Value = item.AssetDescription;
                    worksheet.Cells[rowIterator, 4].Value = item.CapitalisedAmount;
                    worksheet.Cells[rowIterator, 5].Value = item.LoanId;

                    rowIterator = rowIterator + 1;

                }
                ////////////////////////////////////////////////////////////
                ///////////////period///////////////////////////
                excel.Workbook.Worksheets.Add("Period(CompanyLaw)");
                headerRow =
                   new string[] { "Id", "FromDate", "ToDate", "Months", "ClientID"
                  }; 

                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["Period(CompanyLaw)"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in plst)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 3].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 4].Value = item.Months;
                    worksheet.Cells[rowIterator, 5].Value = item.ClientID;
              
                    rowIterator = rowIterator + 1;
                }
                ////////////
                ///////////////Subperiod///////////////////////////
                excel.Workbook.Worksheets.Add("SubPeriod");
                headerRow =
                   new string[] { "Id", "PeriodId","FromDate", "ToDate","Periodlock","Depreciationlock","ClientID"
                  };

                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["LocationBatch"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in subperiod)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.PeriodID;
                    worksheet.Cells[rowIterator, 3].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 4].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 5].Value = item.PeriodLockFlag;
                    worksheet.Cells[rowIterator, 6].Value = item.DepFlag;
                    worksheet.Cells[rowIterator, 7].Value = item.ClientID;
                   
                    rowIterator = rowIterator + 1;
                }
                ////////////
                /////////////////////////////batch///////////////////
                excel.Workbook.Worksheets.Add("Batch");
                headerRow =
                   new string[] { "ID", "FromDate", "ToDate", "BatchDescription", "IsBatchOpen","ClientID"
                  };
             
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["Batch"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in blist)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.ID;
                    worksheet.Cells[rowIterator, 2].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 3].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 4].Value = item.BatchDescription;
                    worksheet.Cells[rowIterator, 5].Value = item.IsBatchOpen;
                    worksheet.Cells[rowIterator, 6].Value = item.ClientID;
                    rowIterator = rowIterator + 1;
                }



                //////////////////////////////

                /////////////////////////////Subbatch///////////////////
                excel.Workbook.Worksheets.Add("LocationBatch");
                headerRow =
                   new string[] { "ID", "BatchId", "MainLocation Name", "Sublocation Name", "Sub_subloaction Name", "MainLocation ID", "Sublocation ID", "Sub_subloaction ID"
                  };

                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["LocationBatch"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in subbatch)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.Id;
                    worksheet.Cells[rowIterator, 2].Value = item.BatchId;
                    worksheet.Cells[rowIterator, 3].Value = item.LocAName;
                    worksheet.Cells[rowIterator, 4].Value = item.LocBName;
                    worksheet.Cells[rowIterator, 5].Value = item.LocCName;
                    worksheet.Cells[rowIterator, 6].Value = item.LocAId;
                    worksheet.Cells[rowIterator, 7].Value = item.LocBId;
                    worksheet.Cells[rowIterator, 8].Value = item.LocCId;
                    rowIterator = rowIterator + 1;
                }



                //////////////////////////////

                /////////////////////////Itperiod
                excel.Workbook.Worksheets.Add("Period(It law)");
                headerRow =
                   new string[] { "Sr No", "FromDate", "ToDate", "Months", "Depreciation Lock","PeriodLock","ClientID"
                  };
             
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["Period(It law)"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in itperiodlst)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.Srno;
                    worksheet.Cells[rowIterator, 2].Value = item.str_fromdate;
                    worksheet.Cells[rowIterator, 3].Value = item.str_todate;
                    worksheet.Cells[rowIterator, 4].Value = item.Months;
                    worksheet.Cells[rowIterator, 5].Value = item.DepFlag;
                    if(item.PeriodlockFlag==0)
                    {
                        worksheet.Cells[rowIterator, 6].Value = "no";
                    }
                    if (item.PeriodlockFlag == 1)
                    {
                        worksheet.Cells[rowIterator, 6].Value = "yes";
                    }
                    worksheet.Cells[rowIterator, 7].Value = item.ClientID;
                    rowIterator = rowIterator + 1;
                }

                //////////////////////////////
                /////////////////itgroups
                excel.Workbook.Worksheets.Add("AssetGroups(It law)");
                headerRow =
                   new string[] {  "GroupName", "DepRate", "OPWDV", "ClientID","DepMethod"
                  };
             
                // Determine the header range (e.g. A1:D1)
                headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet// 

                worksheet = excel.Workbook.Worksheets["AssetGroups(It law)"];

                currentSheet = excel.Workbook.Worksheets;

                excelRange = worksheet.Cells[1, 1, 1, headerRow.Length];
                // Popular header row data
                for (var i = 0; i < headerRow.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headerRow[i];
                }
                // Popular header row data
                // worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                rowIterator = 2;
                foreach (var item in itgroup)
                {

                    worksheet.Cells[rowIterator, 1].Value = item.GroupName;
                    worksheet.Cells[rowIterator, 2].Value = item.DepRate;
                    worksheet.Cells[rowIterator, 3].Value = item.OPWDV;
                    worksheet.Cells[rowIterator, 4].Value = item.ClientID;
                    worksheet.Cells[rowIterator, 5].Value = item.DepMethod;
                    
                    rowIterator = rowIterator + 1;
                }
                return excel.GetAsByteArray();
            }
        }
       
    }
}
