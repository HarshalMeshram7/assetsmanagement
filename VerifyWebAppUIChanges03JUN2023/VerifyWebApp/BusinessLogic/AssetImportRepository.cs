using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using OfficeOpenXml;
using VerifyWebApp.ViewModel;
using VerifyWebApp.Common;
using System.Data.Entity;

namespace VerifyWebApp.BusinessLogic
{

    /// <summary>
    /// Support Import Asset Process thru Excel and CSV
    /// </summary>
    public class AssetImportRepository
    {

        // define all column variables 


        public int COL_SRNo = 1;
        public int COL_ASSETNO = 2;
        public int COL_ASSETNAME = 3;

        public int COL_ASSETIDENTIFICATION_NO = 4;

        public int COL_VOUCHERNO = 5;

        public int COL_VOUCHERDATE = 6;

        public int COL_DTPUTTOUSE = 7;
        public int COL_DtPutToUseIT = 8;

        public int COL_PONo = 9;
        public int COL_PODate = 10;

        public int COL_BillNo = 11;
        public int COL_BillDate = 12;
        public int COL_ReceiptDate = 13;
        public int COL_CommissioningDate = 14;
        public int COL_Qty = 15;
        public int COL_uomno = 16;
        public int COL_SupplierNo = 17;
        public int COL_AGroupId = 18;
        public int COL_BGroupId = 19;
        public int COL_CGroupId = 20;
        public int COL_DGroupId = 21;

        public int COL_LocAId = 22;
        public int COL_LocBId = 23;
        public int COL_LocCId = 24;
        public int COL_CostCenterAId = 25;
        public int COL_CostCenterBId = 26;
        public int COL_ITGroupID = 27;
        public int COL_NormalRate = 28;
        public int COL_AdditionalRate = 29;
        public int COL_TotalRate = 30;

        public int COL_DepreciationMethod = 31;

        public int COL_grossvalue = 32;
        public int COL_servicecharge = 33;
        public int COL_otherexpense = 34;
        public int COL_customduty = 35;
        public int COL_exciseduty = 36;
        public int COL_servicetax = 37;
        public int COL_anyotherduty = 38;
        public int COL_vat = 39;

        public int COL_cst = 40;
        public int COL_cgst = 41;
        public int COL_igst = 42;
        public int COL_sgst = 43;

        public int COL_anyothertax = 44;
        public int COL_totaladdition = 45;
        public int COL_discount = 46;
        public int COL_roundoff = 47;
        public int COL_totaldeduction = 48;
        public int COL_invoiceamt = 49;
        public int COL_dutydrawback = 50;

        public int COL_excisecredit = 51;
        public int COL_servicetaxcredit = 52;
        public int COL_anyotherdutycredit = 53;
        public int COL_vatcredit = 54;
        public int COL_cstcredit = 55;
        public int COL_cgstcredit = 56;
        public int COL_sgstcredit = 57;
        public int COL_igstcredit = 58;
        public int COL_anyothercredit = 59;
        public int COL_totalcredit = 60;

        public int COL_AMOUNTCAPITALISED = 61;
        public int COL_AMOUNT_CAPT_COMPANYLAW = 62;
        public int COL_AMOUNT_CAPT_ITLAW = 63;
        public int COL_OPAccDepreciation = 64;
        public int COL_residual = 65;
        public int COL_BrandName = 66;
        public int COL_ASSET_SRNO = 67;
        public int COL_MODEL = 68;
        public int COL_REMARKS = 69;
        public int COL_ISIMPORTED = 70;
        public int COL_CURRENCY = 71;
        public int COL_VALUES = 72;
        public int COL_YEAR_OF_MFG = 73;
        public int COL_MRRNo = 74;
        public int COL_AccountID = 75;
        public int COL_DepAccountId = 76;
        public int COL_AccAccountID = 77;
        public int COL_Usefullife = 78;
        public int COL_ParentAssetNo = 79;
        public int COL_iscomponent = 80;
        public int COL_ExpiryDate = 81;

        public string DateFormat { get; set; }

        //public Dictionary<int, string> dictErrors = new Dictionary<int, string>();

        public List<AssetImportError> lstErrors = new List<AssetImportError>();

        public List<Assets> lstAssets = new List<Assets>();


        private VerifyDB db;

        private int ErrCounter = 0;

        public AssetImportRepository(VerifyDB db)
        {
            this.db = db;
            //DateFormat = "DDMMYYYY";
            DateFormat = "ddMMyyyy";

        }

        public int companyid { get; set; }

        public int userid { get; set; }

        private bool IsNumeric(string strNum)
        {
            int n;
            bool isNumeric;
            try
            {
                isNumeric = int.TryParse(strNum, out n);
                return isNumeric;
            }
            catch(Exception ex)
            {
                return false;
            }
        }



        private bool IsDecimal(string strNum)
        {
            decimal n;
            bool IsDecimal;
            try
            {
                IsDecimal = Decimal.TryParse(strNum, out n);
                return IsDecimal;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        public List<AssetImportDTO> AssetDTOList { get; set; }

        public void ReadExcel(ExcelWorksheet workSheet)
        {
            //List<AssetImportDTO> lstAssetlist = new List<AssetImportDTO>();
            AssetDTOList = new List<AssetImportDTO>();

            AssetDTOList.Clear();

            try
            {

                int noOfCol = workSheet.Dimension.End.Column;
                int noOfRow = workSheet.Dimension.End.Row;


                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {

                    AssetImportDTO asset = new AssetImportDTO();

                    


                    if (workSheet.Cells[rowIterator, COL_SRNo].Text == "")
                    {
                        asset.Row_SrNo = "";
                    }
                    else
                    {
                        asset.Row_SrNo = workSheet.Cells[rowIterator, 1].Value.ToString();
                    }


                    System.Diagnostics.Debug.WriteLine(asset.Row_SrNo);

                    if (workSheet.Cells[rowIterator, COL_ASSETNO].Text == "")
                    {
                        asset.AssetNo = "";
                    }
                    else
                    {
                        asset.AssetNo = workSheet.Cells[rowIterator, COL_ASSETNO].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_ASSETNAME].Text == "")
                    {
                        asset.AssetName = "";
                    }
                    else
                    {
                        asset.AssetName = workSheet.Cells[rowIterator, COL_ASSETNAME].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_ASSETIDENTIFICATION_NO].Text == "")
                    {
                        asset.AssetIdentificationNo = "";
                    }
                    else
                    {
                        asset.AssetIdentificationNo = workSheet.Cells[rowIterator, COL_ASSETIDENTIFICATION_NO].Value.ToString();
                    }

                    if (workSheet.Cells[rowIterator, COL_VOUCHERNO].Text == "")
                    {
                        asset.VoucherNo = "";
                    }
                    else
                    {
                        asset.VoucherNo = workSheet.Cells[rowIterator, COL_VOUCHERNO].Value.ToString();
                    }

                    if (workSheet.Cells[rowIterator, COL_VOUCHERDATE].Text == "")
                    {
                        asset.VoucherDate = "";
                    }
                    else
                    {
                        asset.VoucherDate = workSheet.Cells[rowIterator, COL_VOUCHERDATE].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_DTPUTTOUSE].Text == "")
                    {
                        asset.DtPutToUse = "";
                    }
                    else
                    {
                        asset.DtPutToUse = workSheet.Cells[rowIterator, COL_DTPUTTOUSE].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_DtPutToUseIT].Text == "")
                    {
                        asset.DtPutToUseIT = "";
                    }
                    else
                    {
                        asset.DtPutToUseIT = workSheet.Cells[rowIterator, COL_DtPutToUseIT].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_PONo].Text == "")
                    {
                        asset.PONo = "";
                    }
                    else
                    {
                        asset.PONo = workSheet.Cells[rowIterator, COL_PONo].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_PODate].Text == "")
                    {
                        asset.PODate = "";
                    }
                    else
                    {
                        asset.PODate = workSheet.Cells[rowIterator, COL_PODate].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_BillNo].Text == "")
                    {
                        asset.BillNo = "";
                    }
                    else
                    {
                        asset.BillNo = workSheet.Cells[rowIterator, COL_BillNo].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_BillDate].Text == "")
                    {
                        asset.BillDate = "";
                    }
                    else
                    {
                        asset.BillDate = workSheet.Cells[rowIterator, COL_BillDate].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_ReceiptDate].Text == "")
                    {
                        asset.ReceiptDate = "";
                    }
                    else
                    {
                        asset.ReceiptDate = workSheet.Cells[rowIterator, COL_ReceiptDate].Value.ToString();
                    }

                    if (workSheet.Cells[rowIterator, COL_CommissioningDate].Text == "")
                    {
                        asset.CommissioningDate = "";
                    }
                    else
                    {
                        asset.CommissioningDate = workSheet.Cells[rowIterator, COL_CommissioningDate].Value.ToString();
                    }

                    if (workSheet.Cells[rowIterator, COL_Qty].Text == "")
                    {
                        asset.Qty = "";
                    }
                    else
                    {
                        asset.Qty = workSheet.Cells[rowIterator, COL_Qty].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_uomno].Text == "")
                    {
                        asset.UOMNo = "";
                    }
                    else
                    {
                        asset.UOMNo = workSheet.Cells[rowIterator, COL_uomno].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_SupplierNo].Text == "")
                    {
                        asset.SupplierNo = "";
                    }
                    else
                    {
                        asset.SupplierNo = workSheet.Cells[rowIterator, COL_SupplierNo].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_AGroupId].Text == "")
                    {
                        asset.AGroupID = "";
                    }
                    else
                    {
                        asset.AGroupID = workSheet.Cells[rowIterator, COL_AGroupId].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_BGroupId].Text == "")
                    {
                        asset.BGroupID = "";
                    }
                    else
                    {
                        asset.BGroupID = workSheet.Cells[rowIterator, COL_BGroupId].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_CGroupId].Text == "")
                    {
                        asset.CGroupID = "";
                    }
                    else
                    {
                        asset.CGroupID = workSheet.Cells[rowIterator, COL_CGroupId].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_DGroupId].Text == "")
                    {
                        asset.DGroupID = "";
                    }
                    else
                    {
                        asset.DGroupID = workSheet.Cells[rowIterator, COL_DGroupId].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_LocAId].Text == "")
                    {
                        asset.LocAID = "";
                    }
                    else
                    {
                        asset.LocAID = workSheet.Cells[rowIterator, COL_LocAId].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_LocBId].Text == "")
                    {
                        asset.LocBID = "";
                    }
                    else
                    {
                        asset.LocBID = workSheet.Cells[rowIterator, COL_LocBId].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_LocCId].Text == "")
                    {
                        asset.LocCID = "";
                    }
                    else
                    {
                        asset.LocCID = workSheet.Cells[rowIterator, COL_LocCId].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_CostCenterAId].Text == "")
                    {
                        asset.CostCenterAID = "";
                    }
                    else
                    {
                        asset.CostCenterAID = workSheet.Cells[rowIterator, COL_CostCenterAId].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_CostCenterBId].Text == "")
                    {
                        asset.CostCenterBID = "";
                    }
                    else
                    {
                        asset.CostCenterBID = workSheet.Cells[rowIterator, COL_CostCenterBId].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_ITGroupID].Text == "")
                    {
                        asset.ITGroupIDID = "";
                    }
                    else
                    {
                        asset.ITGroupIDID = workSheet.Cells[rowIterator, COL_ITGroupID].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_NormalRate].Text == "")
                    {
                        asset.NormalRatae = "";
                    }
                    else
                    {
                        asset.NormalRatae = workSheet.Cells[rowIterator, COL_NormalRate].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_AdditionalRate].Text == "")
                    {
                        asset.AdditionalRate = "";
                    }
                    else
                    {
                        asset.AdditionalRate = workSheet.Cells[rowIterator, COL_AdditionalRate].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_TotalRate].Text == "")
                    {
                        asset.TotalRate = "";
                    }
                    else
                    {
                        asset.TotalRate = workSheet.Cells[rowIterator, COL_TotalRate].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_DepreciationMethod].Text == "")
                    {
                        asset.DepreciationMethod = "";
                    }
                    else
                    {
                        asset.DepreciationMethod = workSheet.Cells[rowIterator, COL_DepreciationMethod].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_grossvalue].Text == "")
                    {
                        asset.GrossVal = "";
                    }
                    else
                    {
                        asset.GrossVal = workSheet.Cells[rowIterator, COL_grossvalue].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_servicecharge].Text == "")
                    {
                        asset.ServiceCharges = "";
                    }
                    else
                    {
                        asset.ServiceCharges = workSheet.Cells[rowIterator, COL_servicecharge].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_otherexpense].Text == "")
                    {
                        asset.OtherExp = "";
                    }
                    else
                    {
                        asset.OtherExp = workSheet.Cells[rowIterator, COL_otherexpense].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_customduty].Text == "")
                    {
                        asset.CustomDuty = "";
                    }
                    else
                    {
                        asset.CustomDuty = workSheet.Cells[rowIterator, COL_customduty].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_exciseduty].Text == "")
                    {
                        asset.ExciseDuty = "";
                    }
                    else
                    {
                        asset.ExciseDuty = workSheet.Cells[rowIterator, COL_exciseduty].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_servicetax].Text == "")
                    {
                        asset.ServiceTax = "";
                    }
                    else
                    {
                        asset.ServiceTax = workSheet.Cells[rowIterator, COL_servicetax].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_anyotherduty].Text == "")
                    {
                        asset.AnyOtherDuty = "";
                    }
                    else
                    {
                        asset.AnyOtherDuty = workSheet.Cells[rowIterator, COL_anyotherduty].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_vat].Text == "")
                    {
                        asset.VAT = "";
                    }
                    else
                    {
                        asset.VAT = workSheet.Cells[rowIterator, COL_vat].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_cst].Text == "")
                    {
                        asset.CSt = "";
                    }
                    else
                    {
                        asset.CSt = workSheet.Cells[rowIterator, COL_cst].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_cgst].Text == "")
                    {
                        asset.CGST = "";
                    }
                    else
                    {
                        asset.CGST = workSheet.Cells[rowIterator, COL_cgst].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_igst].Text == "")
                    {
                        asset.IGST = "";
                    }
                    else
                    {
                        asset.IGST = workSheet.Cells[rowIterator, COL_igst].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_sgst].Text == "")
                    {
                        asset.SGST = "";
                    }
                    else
                    {
                        asset.SGST = workSheet.Cells[rowIterator, COL_sgst].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_anyothertax].Text == "")
                    {
                        asset.AnyOtherTax = "";
                    }
                    else
                    {
                        asset.AnyOtherTax = workSheet.Cells[rowIterator, COL_anyothertax].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_totaladdition].Text == "")
                    {
                        asset.TotalAddition = "";
                    }
                    else
                    {
                        asset.TotalAddition = workSheet.Cells[rowIterator, COL_totaladdition].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_discount].Text == "")
                    {
                        asset.Discount = "";
                    }
                    else
                    {
                        asset.Discount = workSheet.Cells[rowIterator, COL_discount].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_roundoff].Text == "")
                    {
                        asset.Roundingoff = "";
                    }
                    else
                    {
                        asset.Roundingoff = workSheet.Cells[rowIterator, COL_roundoff].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_totaldeduction].Text == "")
                    {
                        asset.TotalDeduction = "";
                    }
                    else
                    {
                        asset.TotalDeduction = workSheet.Cells[rowIterator, COL_totaldeduction].Value.ToString();
                    }





                    if (workSheet.Cells[rowIterator, COL_invoiceamt].Text == "")
                    {
                        asset.InvoiceAmt = "";
                    }
                    else
                    {
                        asset.InvoiceAmt = workSheet.Cells[rowIterator, COL_invoiceamt].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_dutydrawback].Text == "")
                    {
                        asset.DutyDrawback = "";
                    }
                    else
                    {
                        asset.DutyDrawback = workSheet.Cells[rowIterator, COL_dutydrawback].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_excisecredit].Text == "")
                    {
                        asset.ExciseCredit = "";
                    }
                    else
                    {
                        asset.ExciseCredit = workSheet.Cells[rowIterator, COL_excisecredit].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_servicetaxcredit].Text == "")
                    {
                        asset.ServiceTaxCredit = "";
                    }
                    else
                    {
                        asset.ServiceTaxCredit = workSheet.Cells[rowIterator, COL_servicetaxcredit].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_anyotherdutycredit].Text == "")
                    {
                        asset.AnyOtherDutyCredit = "";
                    }
                    else
                    {
                        asset.AnyOtherDutyCredit = workSheet.Cells[rowIterator, COL_anyotherdutycredit].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_vatcredit].Text == "")
                    {
                        asset.VATCredit = "";
                    }
                    else
                    {
                        asset.VATCredit = workSheet.Cells[rowIterator, COL_vatcredit].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_cstcredit].Text == "")
                    {
                        asset.CSTCredit = "";
                    }
                    else
                    {
                        asset.CSTCredit = workSheet.Cells[rowIterator, COL_cstcredit].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_cgstcredit].Text == "")
                    {
                        asset.CGSTCredit = "";
                    }
                    else
                    {
                        asset.CGSTCredit = workSheet.Cells[rowIterator, COL_cgstcredit].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_sgstcredit].Text == "")
                    {
                        asset.SGSTCredit = "";
                    }
                    else
                    {
                        asset.SGSTCredit = workSheet.Cells[rowIterator, COL_sgstcredit].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_igstcredit].Text == "")
                    {
                        asset.IGSTCredit = "";
                    }
                    else
                    {
                        asset.IGSTCredit = workSheet.Cells[rowIterator, COL_igstcredit].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_anyothercredit].Text == "")
                    {
                        asset.AnyOtherCredit = "";
                    }
                    else
                    {
                        asset.AnyOtherCredit = workSheet.Cells[rowIterator, COL_anyothercredit].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_totalcredit].Text == "")
                    {
                        asset.TotalCredit = "";
                    }
                    else
                    {
                        asset.TotalCredit = workSheet.Cells[rowIterator, COL_totalcredit].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_AMOUNTCAPITALISED].Text == "")
                    {
                        asset.AmountCapitalised = "";
                    }
                    else
                    {
                        asset.AmountCapitalised = workSheet.Cells[rowIterator, COL_AMOUNTCAPITALISED].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_AMOUNT_CAPT_COMPANYLAW].Text == "")
                    {
                        asset.AmountCapitalisedCompany = "";
                    }
                    else
                    {
                        asset.AmountCapitalisedCompany = workSheet.Cells[rowIterator, COL_AMOUNT_CAPT_COMPANYLAW].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_AMOUNT_CAPT_ITLAW].Text == "")
                    {
                        asset.AmountCApitalisedIT = "";
                    }
                    else
                    {
                        asset.AmountCApitalisedIT = workSheet.Cells[rowIterator, COL_AMOUNT_CAPT_ITLAW].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_OPAccDepreciation].Text == "")
                    {
                        asset.OPAccDepreciation = "";
                    }
                    else
                    {
                        asset.OPAccDepreciation = workSheet.Cells[rowIterator, COL_OPAccDepreciation].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_residual].Text == "")
                    {
                        asset.ResidualVal = "";
                    }
                    else
                    {
                        asset.ResidualVal = workSheet.Cells[rowIterator, COL_residual].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_BrandName].Text == "")
                    {
                        asset.BrandName = "";
                    }
                    else
                    {
                        asset.BrandName = workSheet.Cells[rowIterator, COL_BrandName].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_ASSET_SRNO].Text == "")
                    {
                        asset.SrNo_Asset = "";
                    }
                    else
                    {
                        asset.SrNo_Asset = workSheet.Cells[rowIterator, COL_ASSET_SRNO].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_MODEL].Text == "")
                    {
                        asset.Model = "";
                    }
                    else
                    {
                        asset.Model = workSheet.Cells[rowIterator, COL_MODEL].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_REMARKS].Text == "")
                    {
                        asset.Remarks = "";
                    }
                    else
                    {
                        asset.Remarks = workSheet.Cells[rowIterator, COL_REMARKS].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_ISIMPORTED].Text == "")
                    {
                        asset.IsImported = "";
                    }
                    else
                    {
                        asset.IsImported = workSheet.Cells[rowIterator, COL_ISIMPORTED].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_CURRENCY].Text == "")
                    {
                        asset.Currency = "";
                    }
                    else
                    {
                        asset.Currency = workSheet.Cells[rowIterator, COL_CURRENCY].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_VALUES].Text == "")
                    {
                        asset.Values = "";
                    }
                    else
                    {
                        asset.Values = workSheet.Cells[rowIterator, COL_VALUES].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_YEAR_OF_MFG].Text == "")
                    {
                        asset.YrofManufacturing = "";
                    }
                    else
                    {
                        asset.YrofManufacturing = workSheet.Cells[rowIterator, COL_YEAR_OF_MFG].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_MRRNo].Text == "")
                    {
                        asset.MRRNo = "";
                    }
                    else
                    {
                        asset.MRRNo = workSheet.Cells[rowIterator, COL_MRRNo].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_AccountID].Text == "")
                    {
                        asset.AccountID = "";
                    }
                    else
                    {
                        asset.AccountID = workSheet.Cells[rowIterator, COL_AccountID].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_DepAccountId].Text == "")
                    {
                        asset.DepAccountId = "";
                    }
                    else
                    {
                        asset.DepAccountId = workSheet.Cells[rowIterator, COL_DepAccountId].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_AccAccountID].Text == "")
                    {
                        asset.AccAccountID = "";
                    }
                    else
                    {
                        asset.AccAccountID = workSheet.Cells[rowIterator, COL_AccAccountID].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_Usefullife].Text == "")
                    {
                        asset.Usefullife = "";
                    }
                    else
                    {
                        asset.Usefullife = workSheet.Cells[rowIterator, COL_Usefullife].Value.ToString();
                    }


                    if (workSheet.Cells[rowIterator, COL_ParentAssetNo].Text == "")
                    {
                        asset.ParentAssetNo = "";
                    }
                    else
                    {
                        asset.ParentAssetNo = workSheet.Cells[rowIterator, COL_ParentAssetNo].Value.ToString();
                    }



                    if (workSheet.Cells[rowIterator, COL_iscomponent].Text == "")
                    {
                        asset.IsComponent = "";
                    }
                    else
                    {
                        asset.IsComponent = workSheet.Cells[rowIterator, COL_iscomponent].Value.ToString();
                    }




                    if (workSheet.Cells[rowIterator, COL_ExpiryDate].Text == "")
                    {
                        asset.ExpiryDate = "";
                    }
                    else
                    {
                        asset.ExpiryDate = workSheet.Cells[rowIterator, COL_ExpiryDate].Value.ToString();
                    }



                    AssetDTOList.Add(asset);

                }
               
            }catch(Exception ex)
            {

               // return lstAssetlist;
            }

            //return lstAssetlist;

        }


        /// <summary>
        /// Convert from temp dto to Assets
        /// </summary>
        public void ConvertToAssets()
        {

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);


            DateUtil util = new DateUtil();

            DbContextTransaction transaction = db.Database.BeginTransaction();
            try
            {

                foreach (var item in AssetDTOList)
                {

                    Assets assets = new Assets();
                    assets.AssetNo = item.AssetNo;
                    assets.AssetName = item.AssetName;
                    assets.AssetIdentificationNo = item.AssetIdentificationNo;
                    assets.VoucherNo = item.VoucherNo;

                    DateTime dtTempVoucherDate = new DateTime();

                    if (util.GetDate(item.VoucherDate, out dtTempVoucherDate))
                    {
                        assets.VoucherDate = dtTempVoucherDate;
                    }

                    DateTime dtTempPODate = new DateTime();

                    if (util.GetDate(item.PODate, out dtTempPODate))
                    {
                        assets.PODate = dtTempPODate;
                    }

                    DateTime dtTempReceiptDate = new DateTime();

                    if (util.GetDate(item.ReceiptDate, out dtTempReceiptDate))
                    {
                        assets.ReceiptDate = dtTempReceiptDate;
                    }

                    DateTime dtTempCommDate = new DateTime();

                    if (util.GetDate(item.CommissioningDate, out dtTempCommDate))
                    {
                        assets.CommissioningDate = dtTempCommDate;
                    }



                    DateTime dtTempBillDate = new DateTime();

                    if (util.GetDate(item.BillDate, out dtTempBillDate))
                    {
                        assets.BillDate = dtTempBillDate;
                    }


                    DateTime dtTempDTPutToUse = new DateTime();

                    if (util.GetDate(item.DtPutToUse, out dtTempDTPutToUse))
                    {
                        assets.DtPutToUse = dtTempDTPutToUse;
                    }


                    DateTime dtTempDTPutToUseIT = new DateTime();

                    if (util.GetDate(item.DtPutToUseIT, out dtTempDTPutToUseIT))
                    {
                        assets.DtPutToUseIT = dtTempDTPutToUseIT;
                    }


                    assets.PONo = item.PONo;
                    assets.BillNo = item.BillNo;

                    assets.MRRNo = item.MRRNo;
                    assets.Qty = Convert.ToInt32(item.Qty);

                    if (item.SupplierNo.Length > 0)
                    {
                        assets.SupplierNo = Convert.ToInt32(item.SupplierNo);
                    }
                    

                    if (item.UOMNo.Length > 0)
                    {
                        assets.UOMNo = Convert.ToInt32(item.UOMNo);
                    }else
                    {
                        assets.UOMNo = 1;
                    }
                    

                    assets.BrandName = item.BrandName;

                    assets.SrNo = item.SrNo_Asset;

                    assets.Model = item.Model;

                    assets.Remarks = item.Remarks;
                    assets.IsImported = item.IsImported;
                    assets.Currency = item.Currency;
                    //assets.Values = Convert.ToDecimal(item.Values);
                    assets.AGroupID = Convert.ToInt32(item.AGroupID);

                    if (item.BGroupID.Length > 0)
                    {
                        assets.BGroupID = Convert.ToInt32(item.BGroupID);
                    }


                    if (item.CGroupID.Length > 0)
                    {
                        assets.CGroupID = Convert.ToInt32(item.CGroupID);
                    }

                    if (item.DGroupID.Length > 0)
                    {
                        assets.DGroupID = Convert.ToInt32(item.DGroupID);
                    }



                    if (item.ITGroupIDID.Length > 0)
                    {
                        assets.ITGroupIDID = Convert.ToInt32(item.ITGroupIDID);
                    }


                   

                    assets.DepreciationMethod = item.DepreciationMethod;

                    assets.NormalRatae = Convert.ToDecimal(item.NormalRatae);
                    assets.AdditionalRate = Convert.ToDecimal(item.AdditionalRate);
                    assets.TotalRate = Convert.ToDecimal(item.TotalRate);
                    assets.Usefullife = Convert.ToInt32(item.Usefullife);

                    if (item.YrofManufacturing.Length > 0)
                    {
                        assets.YrofManufacturing = Convert.ToInt32(item.YrofManufacturing);
                    }
                   


                    DateTime dat = Convert.ToDateTime(assets.DtPutToUse);  // or    given date

                    assets.ExpiryDate = dat.AddYears(Convert.ToInt32(assets.Usefullife));

                    if (item.AccountID.Length > 0)
                    {
                        assets.AccountID = Convert.ToInt32(item.AccountID);
                    }
                    if (item.AccAccountID.Length > 0)
                    {
                        assets.AccAccountID = Convert.ToInt32(item.AccAccountID);
                    }
                    if (item.DepAccountId.Length > 0)
                    {
                        assets.DepAccountId = Convert.ToInt32(item.DepAccountId);
                    }

                    if (item.OPAccDepreciation.Length > 0)
                    {
                        assets.OPAccDepreciation = Convert.ToDecimal(item.OPAccDepreciation);
                    }
                    else
                    {
                        assets.OPAccDepreciation = 0;
                    }

                    if (item.GrossVal.Length > 0)
                    {
                        assets.GrossVal = Convert.ToDecimal(item.GrossVal);
                    }


                    if (item.ServiceCharges.Length > 0)
                    {
                        assets.ServiceCharges = Convert.ToDecimal(item.ServiceCharges);
                    }

                    if (item.OtherExp.Length > 0)
                    {
                        assets.OtherExp = Convert.ToDecimal(item.OtherExp);
                    }

                    if (item.CustomDuty.Length > 0)
                    {
                        assets.CustomDuty = Convert.ToDecimal(item.CustomDuty);
                    }
                    if (item.ExciseDuty.Length > 0)
                    {
                        assets.ExciseDuty = Convert.ToDecimal(item.ExciseDuty);
                    }

                    if (item.ServiceTax.Length > 0)
                    {
                        assets.ServiceTax = Convert.ToDecimal(item.ServiceTax);
                    }

                    if (item.AnyOtherDuty.Length > 0)
                    {
                        assets.AnyOtherDuty = Convert.ToDecimal(item.AnyOtherDuty);
                    }

                    if (item.VAT.Length > 0)
                    {
                        assets.VAT = Convert.ToDecimal(item.VAT);
                    }
                    if (item.CSt.Length > 0)
                    {
                        assets.CSt = Convert.ToDecimal(item.CSt);
                    }

                    if (item.CGST.Length > 0)
                    {
                        assets.CGST = Convert.ToDecimal(item.CGST);
                    }

                    if (item.SGST.Length > 0)
                    {
                        assets.SGST = Convert.ToDecimal(item.SGST);
                    }
                    if (item.IGST.Length > 0)
                    {
                        assets.IGST = Convert.ToDecimal(item.IGST);
                    }

                    if (item.AnyOtherTax.Length > 0)
                    {
                        assets.AnyOtherTax = Convert.ToDecimal(item.AnyOtherTax);
                    }

                    if (item.TotalAddition.Length > 0)
                    {
                        assets.TotalAddition = Convert.ToDecimal(item.TotalAddition);
                    }

                    if (item.Discount.Length > 0)
                    {
                        assets.Discount = Convert.ToDecimal(item.Discount);
                    }


                    if (item.Roundingoff.Length > 0)
                    {
                        assets.Roundingoff = Convert.ToDecimal(item.Roundingoff);
                    }

                    if (item.TotalDeduction.Length > 0)
                    {
                        assets.TotDeduction = Convert.ToDecimal(item.TotalDeduction);
                    }

                    if (item.InvoiceAmt.Length > 0)
                    {
                        assets.InvoiceAmt = Convert.ToDecimal(item.InvoiceAmt);
                    }

                    if (item.DutyDrawback.Length > 0)
                    {
                        assets.DutyDrawback = Convert.ToDecimal(item.DutyDrawback);
                    }

                    if (item.ExciseCredit.Length > 0)
                    {
                        assets.ExciseCredit = Convert.ToDecimal(item.ExciseCredit);
                    }

                    if (item.ServiceTaxCredit.Length > 0)
                    {
                        assets.ServiceTaxCredit = Convert.ToDecimal(item.ServiceTaxCredit);
                    }

                    if (item.AnyOtherDutyCredit.Length > 0)
                    {
                        assets.AnyOtherDutyCredit = Convert.ToDecimal(item.AnyOtherDutyCredit);
                    }

                    if (item.VATCredit.Length > 0)
                    {
                        assets.VATCredit = Convert.ToDecimal(item.VATCredit);
                    }

                    if (item.CSTCredit.Length > 0)
                    {
                        assets.CSTCredit = Convert.ToDecimal(item.CSTCredit);
                    }


                    if (item.CGSTCredit.Length > 0)
                    {
                        assets.CGSTCredit = Convert.ToDecimal(item.CGSTCredit);
                    }

                    if (item.SGSTCredit.Length > 0)
                    {
                        assets.SGSTCredit = Convert.ToDecimal(item.SGSTCredit);
                    }

                    if (item.IGSTCredit.Length > 0)
                    {
                        assets.IGSTCredit = Convert.ToDecimal(item.IGSTCredit);
                    }

                    if (item.AnyOtherCredit.Length > 0)
                    {
                        assets.AnyOtherCredit = Convert.ToDecimal(item.AnyOtherCredit);
                    }

                    if (item.TotalCredit.Length > 0)
                    {
                        assets.TotalCredit = Convert.ToDecimal(item.TotalCredit);
                    }


                    if (item.AmountCapitalised.Length > 0)
                    {
                        assets.AmountCapitalised = Convert.ToDecimal(item.AmountCapitalised);
                    }

                    if (item.AmountCapitalisedCompany.Length > 0)
                    {
                        assets.AmountCapitalisedCompany = Convert.ToDecimal(item.AmountCapitalisedCompany);
                    }

                    if (item.AmountCApitalisedIT.Length > 0)
                    {
                        assets.AmountCApitalisedIT = Convert.ToDecimal(item.AmountCApitalisedIT);
                    }


                    if (item.ResidualVal.Length > 0)
                    {
                        assets.ResidualVal = Convert.ToDecimal(item.ResidualVal);
                    }
                    else
                    {
                        assets.ResidualVal = 0;
                    }

                    assets.DisposalFlag = 0;
                    assets.CreatedUserId = userid;
                    assets.Modified_Userid = userid;
                    assets.Companyid = companyid;
                    //assets.Parent_AssetId = Convert.ToInt32(item.Parent_AssetId);
                    assets.iscomponent = item.IsComponent;

                    Childlocation childlocation = new Childlocation();

                    if (item.LocAID.Length > 0)
                    {
                        assets.LocAID = Convert.ToInt32(item.LocAID);
                        childlocation.ALocID = assets.LocAID;
                    }

                    if (item.LocBID.Length > 0)
                    {
                        assets.LocBID = Convert.ToInt32(item.LocBID);
                        childlocation.BLocID = assets.LocBID;
                    }

                    if (item.LocCID.Length > 0)
                    {
                        assets.LocCID = Convert.ToInt32(item.LocCID);
                        childlocation.CLocID = assets.LocCID;
                    }


                    db.Assetss.Add(assets);
                    db.SaveChanges();


                    childlocation.AssetID = assets.ID;
                    childlocation.Date = assets.DtPutToUse;
                    childlocation.Companyid = companyid;
                    childlocation.CreatedDate = istDate;
                    childlocation.CreatedUserId = userid;

                    if (assets.LocAID > 0)
                    {
                        db.childlocations.Add(childlocation);
                        db.SaveChanges();
                    }

                    Childcostcenter childcostcenter = new Childcostcenter();
                    childcostcenter.Ass_ID = assets.ID;
                    if (item.CostCenterAID.Length > 0)
                    {
                        assets.CostCenterAID = Convert.ToInt32(item.CostCenterAID);
                        childcostcenter.AcostcenterID = assets.CostCenterAID;

                    }


                    if (item.CostCenterBID.Length > 0)
                    {
                        assets.CostCenterBID = Convert.ToInt32(item.CostCenterBID);
                        childcostcenter.BcostcenterID = assets.CostCenterBID;

                    }

                    childcostcenter.CreatedDate = istDate;
                    childcostcenter.CreatedUserId = userid;
                    childcostcenter.Percentage = "100";
                    childcostcenter.Companyid = companyid;

                    if (assets.CostCenterAID > 0)
                    {
                        db.childcostcenters.Add(childcostcenter);
                        db.SaveChanges();
                    }

                }
                transaction.Commit();
            }
            catch (Exception ex)
            {

                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                transaction.Rollback();
               // logger.Log(LogLevel.Error, strError);
               // res.Data = "error";
                //return res;
            }

        }
      

        public void Validate()
        {
            

            foreach (var item in AssetDTOList)
            {
                System.Diagnostics.Debug.WriteLine(item.AssetNo);

                // asset no
                validate_assetno(item.AssetNo, item.Row_SrNo);
                validate_assetname(item.AssetName, item.Row_SrNo);
                validate_assetidentificationNo(item.AssetIdentificationNo, item.Row_SrNo);
                validate_voucherNo(item.VoucherNo, item.Row_SrNo);
                validate_voucherDate(item.VoucherDate, item.Row_SrNo);
                validate_DtPutToUse(item.DtPutToUse, item.Row_SrNo);
                validate_DtPutToUseIT(item.DtPutToUseIT, item.Row_SrNo);
                validate_PONo(item.PONo, item.Row_SrNo);
                validate_PODate(item.PODate, item.Row_SrNo);
                validate_BillNo(item.BillNo, item.Row_SrNo);
                validate_BillDate(item.BillDate, item.Row_SrNo);
                validate_ReceiptDate(item.ReceiptDate, item.Row_SrNo);
                validate_CommissioningDate(item.CommissioningDate, item.Row_SrNo);
                validate_Qty(item.Qty, item.Row_SrNo);
                validate_UOM(item.UOMNo, item.Row_SrNo);
                validate_SupplierNo(item.SupplierNo, item.Row_SrNo); //FK

                validate_AGroupID(item.AGroupID, item.Row_SrNo);
                validate_BGroupID(item.BGroupID, item.Row_SrNo);
                validate_CGroupID(item.CGroupID, item.Row_SrNo);
                validate_DGroupID(item.DGroupID, item.Row_SrNo);

                validate_LocAID(item.LocAID, item.Row_SrNo);
                validate_LocBID(item.LocBID, item.Row_SrNo);
                validate_LocCID(item.LocCID, item.Row_SrNo);

                validate_CostCenterAID(item.CostCenterAID, item.Row_SrNo);
                validate_CostCenterBID(item.CostCenterBID, item.Row_SrNo);

                validate_ITGroupID(item.ITGroupIDID, item.Row_SrNo);

                validate_NormalRate(item.NormalRatae, item.Row_SrNo);
                validate_AdditionalRate(item.AdditionalRate, item.Row_SrNo);
                validate_TotalRate(item.TotalRate, item.Row_SrNo);

                validate_DepMethod(item.DepreciationMethod, item.Row_SrNo);

                validate_GrossVal(item.GrossVal, item.Row_SrNo);

                validate_ServiceCharges(item.ServiceCharges, item.Row_SrNo);
                validate_OtherExp(item.OtherExp, item.Row_SrNo);

                validate_CustomDuty(item.CustomDuty, item.Row_SrNo);
                validate_ExciseDuty(item.ExciseDuty, item.Row_SrNo);

                validate_ServiceTax(item.ServiceTax, item.Row_SrNo);

                validate_AnyOtherDuty(item.AnyOtherDuty, item.Row_SrNo);

                validate_VAT(item.VAT, item.Row_SrNo);
                validate_CST(item.CSt, item.Row_SrNo);
                validate_CGST(item.CGST, item.Row_SrNo);

                validate_IGST(item.IGST, item.Row_SrNo);
                validate_SGST(item.SGST, item.Row_SrNo);
                validate_AnyTax(item.AnyOtherTax, item.Row_SrNo);

                validate_TotalAddition(item.TotalAddition, item.Row_SrNo);

                validate_InvoiceAmount(item.InvoiceAmt, item.Row_SrNo);

                validate_DutyDrwaback(item.DutyDrawback, item.Row_SrNo);

                validate_ExciseCredit(item.ExciseCredit, item.Row_SrNo);

                validate_ServiceTaxCredit(item.ServiceTaxCredit, item.Row_SrNo);

                validate_AnyOtherDutyCredit(item.AnyOtherDutyCredit, item.Row_SrNo);

                validate_VatCredit(item.VATCredit, item.Row_SrNo);

                validate_CSTCredit(item.CSTCredit, item.Row_SrNo);

                validate_CGSTCredit(item.CGSTCredit, item.Row_SrNo);

                validate_SGSTCredit(item.SGSTCredit, item.Row_SrNo);

                validate_IGSTCredit(item.IGSTCredit, item.Row_SrNo);

                validate_AnyOtherCredit(item.AnyOtherCredit, item.Row_SrNo);

                validate_TotalCredit(item.TotalCredit, item.Row_SrNo);

                validate_AmtCaptilized(item.AmountCapitalised, item.Row_SrNo);

                validate_AmtCaptilizedCompany(item.AmountCapitalisedCompany, item.Row_SrNo);

                validate_AmtCaptilizedIT(item.AmountCApitalisedIT, item.Row_SrNo);

                validate_OPAccDepreciation(item.OPAccDepreciation, item.Row_SrNo);

                validate_ResidualVal(item.ResidualVal, item.Row_SrNo);

            }



        }


        public void ValidateRefIntegrity()
        {
            System.Diagnostics.Debug.WriteLine("ValidateRefIntegrity");

            List<Supplier> lstSupplers = new List<Supplier>();
            List<AGroup> lstAGroup = new List<AGroup>();
            List<BGroup> lstBGroup = new List<BGroup>();
            List<CGroup> lstCGroup = new List<CGroup>();
            List<DGroup> lstDGroup = new List<DGroup>();

            List<ALocation> lstALocation = new List<ALocation>();
            List<BLocation> lstBLocation = new List<BLocation>();
            List<CLocation> lstCLocation = new List<CLocation>();

            List<ACostCenter> lstACostCenter = new List<ACostCenter>();
            List<BCostCenter> lstBCostCenter = new List<BCostCenter>();



            lstSupplers = db.Suppliers.Where(x => x.Companyid == companyid).ToList();

            lstAGroup = db.AGroups.Where(x => x.Companyid == companyid).ToList();
            lstBGroup = db.BGroups.Where(x => x.Companyid == companyid).ToList();
            lstCGroup = db.CGroups.Where(x => x.Companyid == companyid).ToList();
            lstDGroup = db.DGroups.Where(x => x.Companyid == companyid).ToList();


            lstALocation = db.ALocations.Where(x => x.Companyid == companyid).ToList();
            lstBLocation = db.BLocations.Where(x => x.Companyid == companyid).ToList();
            lstCLocation = db.CLocations.Where(x => x.Companyid == companyid).ToList();

            lstACostCenter = db.ACostCenters.Where(x => x.Companyid == companyid).ToList();
            lstBCostCenter = db.BCostCenters.Where(x => x.Companyid == companyid).ToList();




            foreach (var item in AssetDTOList)
            {
                System.Diagnostics.Debug.WriteLine(item.AssetNo);

                // validate supplier
                if (item.Row_SrNo == "11")
                {
                    int ii = 0;
                }


                if (item.SupplierNo.Length > 0)
                {
                    Supplier supplier = lstSupplers.Where(x => x.ID == Convert.ToInt32(item.SupplierNo)).FirstOrDefault();

                    if (supplier == null)
                    {
                        AddError(item.Row_SrNo, "Supplier id  " + item.SupplierNo + " does not exists!");
                    }

                }



                AGroup a_group = lstAGroup.Where(x => x.ID == Convert.ToInt32(item.AGroupID)).FirstOrDefault();

                if (a_group == null)
                {
                    AddError(item.Row_SrNo, "AGroup ID " + item.AGroupID + " does not exists!");
                }


                if  (item.BGroupID.Trim().Length > 0 && IsNumeric(item.BGroupID))
                {
                    BGroup b_group = lstBGroup.Where(x => x.ID == Convert.ToInt32(item.BGroupID)).FirstOrDefault();
                    if (b_group == null)
                    {
                        AddError(item.Row_SrNo, "BGroup ID " + item.BGroupID + " does not exists!");
                    }else
                    {
                        int temp_parent = b_group.AGrpID;
                        if (a_group != null)
                        {
                            if (temp_parent != a_group.ID)
                            {
                                AddError(item.Row_SrNo, "BGroup ID " + item.BGroupID + " is Invalid.");
                            }
                        }

                    }

                }

                if (item.CGroupID.Trim().Length > 0 && IsNumeric(item.CGroupID))
                {
                    CGroup c_group = lstCGroup.Where(x => x.ID == Convert.ToInt32(item.CGroupID)).FirstOrDefault();
                    if (c_group == null)
                    {
                        AddError(item.Row_SrNo, "CGroup ID " + item.CGroupID + " does not exists!");
                    }

                }

                if (item.DGroupID.Trim().Length > 0 && IsNumeric(item.DGroupID))
                {
                    DGroup d_group = lstDGroup.Where(x => x.ID == Convert.ToInt32(item.DGroupID)).FirstOrDefault();
                    if (d_group == null)
                    {
                        AddError(item.Row_SrNo, "DGroup ID " + item.DGroupID + " does not exists!");
                    }

                }


                // validate location

                if (item.LocAID.Trim().Length > 0 && IsNumeric(item.LocAID))
                {

                    ALocation location_A = lstALocation.Where(x => x.ID == Convert.ToInt32(item.LocAID)).FirstOrDefault();

                    if (location_A == null)
                    {
                        AddError(item.Row_SrNo, "A Location ID " + item.LocAID + " does not exists!");
                    }


                }
                if (item.LocBID.Trim().Length > 0 && IsNumeric(item.LocBID))
                {

                    BLocation location_b = lstBLocation.Where(x => x.ID == Convert.ToInt32(item.LocBID)).FirstOrDefault();

                    if (location_b == null)
                    {
                        AddError(item.Row_SrNo, "B Location ID " + item.LocBID + " does not exists!");
                    }
                    else
                    {
                        // check parent of B Location
                        if (location_b.ALocID != Convert.ToInt32(item.LocAID))
                        {
                            AddError(item.Row_SrNo, "Invalid B Location ID " + item.LocBID + "");
                        }
                    }

                }

                if (item.LocCID.Trim().Length > 0 && IsNumeric(item.LocCID))
                {

                    CLocation location_c = lstCLocation.Where(x => x.ID == Convert.ToInt32(item.LocCID)).FirstOrDefault();

                    if (location_c == null)
                    {
                        AddError(item.Row_SrNo, "C Location ID " + item.LocCID + " does not exists!");
                    }
                    else
                    {
                        // check parent of B Location
                        if (location_c.BLocID != Convert.ToInt32(item.LocBID))
                        {
                            AddError(item.Row_SrNo, "Invalid Location ID " + item.LocBID + "");
                        }

                        // check parent of B Location
                        if (location_c.ALocID != Convert.ToInt32(item.LocAID))
                        {
                            AddError(item.Row_SrNo, "Invalid C Location ID " + item.LocCID + "");
                        }
                    }

                }
                // check cost center 
                if (item.CostCenterAID.Trim().Length > 0 && IsNumeric(item.CostCenterAID))
                {

                    ACostCenter costCenter_A = lstACostCenter.Where(x => x.ID == Convert.ToInt32(item.CostCenterAID)).FirstOrDefault();

                    if (costCenter_A == null)
                    {
                        AddError(item.Row_SrNo, "Cost Center A " + item.CostCenterAID + " does not exists!");
                    }
                }

                // check cost center 
                if (item.CostCenterBID.Trim().Length > 0 && IsNumeric(item.CostCenterBID))
                {

                    BCostCenter costCenter_B = lstBCostCenter.Where(x => x.ID == Convert.ToInt32(item.CostCenterBID)).FirstOrDefault();

                    if (costCenter_B == null)
                    {
                        AddError(item.Row_SrNo, "Cost Center B " + item.CostCenterBID + " does not exists!");
                    }
                }



            }

        }

        private void AddError(string strNo,string ErrorString)
        {
            AssetImportError error = new AssetImportError();
            error.SrNo = strNo;
            error.Error = ErrorString; 
            lstErrors.Add(error);
        }


        private void validate_assetno(string AssetNo, string strNo)
        {


            if (AssetNo.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset no is mandatory Input Ref. SrNo" + strNo);
                AddError(strNo, "Asset no is mandatory Input");



            }


            //asset no should be numeric
            if (IsNumeric(AssetNo) == false)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset no must be number Ref. SrNo" + strNo);
                AddError(strNo, "Asset no must be number");
            }

        }

        private void validate_assetname(string AssetName, string strNo)
        {


            if (AssetName.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset Name is mandatory  Ref. SrNo" + strNo);
                AddError(strNo, "Asset Name is mandatory");

            }

            // check lengths 
            if (AssetName.Length > 200)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                AddError(strNo, "Asset name length should not be more thant 200 chars");
            }

        }

        private void validate_assetidentificationNo(string AssetIdentificationNo, string strNo)
        {

            if (AssetIdentificationNo.Length > 200)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset ID No length should not be more thant 200 chars. Ref. SrNo" + strNo);
                AddError(strNo, "Asset Identifiation No length should not be more thant 200 chars");
            }

        }



        private void validate_voucherNo(string VoucherNO, string strNo)
        {

            if (VoucherNO.Length > 200)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Voucher length should not be more thant 200 chars. Ref. SrNo" + strNo);
                AddError(strNo, "Voucher length should not be more thant 200 chars");
            }


        }



        private void validate_voucherDate(string VoucherDate, string strNo)
        {

            if (VoucherDate.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset Voucher Date is mandatory  Ref. SrNo" + strNo);
                AddError(strNo, "Asset Voucher Date is mandatory");
            }

            if (VoucherDate.Trim().Length > 0)
            {
                // check the format of date
                
                if (ValidateDate(VoucherDate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Invalid Date Format for Voucher Date Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format for Voucher Date");
                }

            }

        }

        private void validate_DtPutToUse(string DtPutToUse, string strNo)
        {

            if (DtPutToUse.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset DtPutToUse Date is mandatory  Ref. SrNo" + strNo);
                AddError(strNo, "Asset DtPutToUse Date is mandatory");
            }

            if (DtPutToUse.Trim().Length > 0)
            {
                // check the format of date

                if (ValidateDate(DtPutToUse) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Invalid Date Format for DtPutToUse Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format for DtPutToUse");
                }

            }

        }


        private void validate_DtPutToUseIT(string DtPutToUseIT, string strNo)
        {

            if (DtPutToUseIT.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset DtPutToUse IT Date is mandatory  Ref. SrNo" + strNo);
                AddError(strNo, "Asset DtPutToUse IT Date is mandatory");
            }

            if (DtPutToUseIT.Trim().Length > 0)
            {
                // check the format of date

                if (ValidateDate(DtPutToUseIT) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Invalid Date Format DtPutToUse IT   Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format DtPutToUse IT");
                }
            }
        }

        private void validate_PONo(string PONO, string strNo)
        {

            if (PONO.Length > 50)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "PO Number length should not be more thant 200 chars. Ref. SrNo" + strNo);
                AddError(strNo, "PO Number length should not be more thant 50 chars");
            }
        }


        private void validate_PODate(string PODate, string strNo)
        {

            if (PODate.Trim().Length > 0)
            {
                // check the format of date

                if (ValidateDate(PODate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Invalid Date Format is mandatory  Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format for PODate");
                }
            }
        }



        private void validate_BillNo(string BillNo, string strNo)
        {

            if (BillNo.Length > 50)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Bill NO length should not be more thant 200 chars. Ref. SrNo" + strNo);
                AddError(strNo, "Bill NO length should not be more thant 50 chars");
            }
        }


        private void validate_BillDate(string BillDate, string strNo)
        {

            if (BillDate.Trim().Length > 0)
            {
                // check the format of date

                if (ValidateDate(BillDate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "Invalid Date Format is mandatory  Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format for Bill Date");
                }
            }
        }


        private void validate_ReceiptDate(string ReceiptDate, string strNo)
        {

            if (ReceiptDate.Trim().Length > 0)
            {
                // check the format of date

                if (ValidateDate(ReceiptDate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Invalid Date Format is mandatory  Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format for Receipt Date");
                }
            }
        }




        private void validate_CommissioningDate(string CommissioningDate, string strNo)
        {

            if (CommissioningDate.Trim().Length > 0)
            {
                // check the format of date

                if (ValidateDate(CommissioningDate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Invalid Date Format is mandatory  Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format for Commissioning Date");
                }
            }
        }



        private void validate_Qty(string Qty, string strNo)
        {

            if (Qty.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset no is mandatory Input Ref. SrNo" + strNo);
                AddError(strNo, "Qty  is mandatory");
            }


            //asset no should be numeric
            if (IsNumeric(Qty) == false)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset no must be number Ref. SrNo" + strNo);
                AddError(strNo, "Qty  must be number");
            }

        }



        private void validate_UOM(string UOM, string strNo)
        {

            if (UOM.Trim().Length >0 )
            {
                //asset no should be numeric
                if (IsNumeric(UOM) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "UOM no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "UOM must be number");
                }
                else
                {
                        //int UOMID = Convert.ToInt32(UOM);
                        //UOM uom = db.UOMs.Where(x => x.ID == UOMID).FirstOrDefault();
                        //if (uom == null)
                        //{
                        //    ErrCounter = ErrCounter + 1;
                        //    AddError(strNo, "UOM Id Not found");
                        //}
                    
                }
            }
        }




        private void validate_SupplierNo(string SupplierNo, string strNo)
        {

            if (SupplierNo.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(SupplierNo) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "SupplierNo no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "SupplierNo no must be number");
                }
                else
                {
                    //int SupplierID = Convert.ToInt32(SupplierNo);
                    //Supplier supplier = db.Suppliers.Where(x => x.ID == SupplierID).FirstOrDefault();
                    //if (supplier == null)
                    //{
                    //    ErrCounter = ErrCounter + 1;
                    //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                    //    AddError(strNo, "Supplier Id Not found");
                    //}
                }
            }
        }



        private void validate_AGroupID(string AGroupID, string strNo)
        {


            if (AGroupID.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "AGroupID is mandatory Input Ref. SrNo" + strNo);
                AddError(strNo, "AGroupID is mandatory Input");
            }

            if (AGroupID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(AGroupID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "AGroupID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AGroupID no must be number");
                }
            }
            else
            {
                //int _AGroupID = Convert.ToInt32(AGroupID);
                //AGroup agroup = db.AGroups.Where(x => x.ID == _AGroupID).FirstOrDefault();
                //if (agroup == null)
                //{
                //    ErrCounter = ErrCounter + 1;
                //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                //    AddError(strNo, "AGroup Id Not found");
                //}
            }

        }





        private void validate_BGroupID(string BGroupID, string strNo)
        {

            if (BGroupID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(BGroupID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "BGroupID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "BGroupID no must be number");
                }
                else
                {
                    //int _BGroupID = Convert.ToInt32(BGroupID);
                    //BGroup bgroup = db.BGroups.Where(x => x.ID == _BGroupID).FirstOrDefault();
                    //if (bgroup == null)
                    //{
                    //    ErrCounter = ErrCounter + 1;
                    //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                    //    AddError(strNo, "BGroup Id Not found");
                    //}
                }

            }
        }



        private void validate_CGroupID(string CGroupID, string strNo)
        {

            if (CGroupID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(CGroupID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "CGroupID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "CGroupID no must be number");
                }
                else
                {
                    //int _CGroupID = Convert.ToInt32(CGroupID);
                    //CGroup cgroup = db.CGroups.Where(x => x.ID == _CGroupID).FirstOrDefault();
                    //if (cgroup == null)
                    //{
                    //    ErrCounter = ErrCounter + 1;
                    //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                    //    AddError(strNo, "CGroup Id Not found");
                    //}
                }
            }
        }



        private void validate_DGroupID(string DGroupID, string strNo)
        {

            if (DGroupID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(DGroupID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "DGroupID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "DGroupID no must be number");
                }
                else
                {
                    int _DGroupID = Convert.ToInt32(DGroupID);
                    //DGroup dgroup = db.DGroups.Where(x => x.ID == _DGroupID).FirstOrDefault();
                    //if (dgroup == null)
                    //{
                    //    ErrCounter = ErrCounter + 1;
                    //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                    //    AddError(strNo, "DGroup Id Not found");
                    //}
                }
            }
        }


        private void validate_LocAID(string LocAID, string strNo)
        {

            if (LocAID.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                // dictErrors.Add(ErrCounter, "LocAID is mandatory Input Ref. SrNo" + strNo);
                AddError(strNo, "LocAID is mandatory Input");
            }

            if (LocAID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(LocAID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "LocAID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "LocAID no must be number");
                }
                else
                {
                    //int _ALOCID = Convert.ToInt32(LocAID);
                    //ALocation alocation = db.ALocations.Where(x => x.ID == _ALOCID).FirstOrDefault();
                    //if (alocation == null)
                    //{
                    //    ErrCounter = ErrCounter + 1;
                    //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                    //    AddError(strNo, "LocationA  Id Not found");
                    //}
                }
            }
        }


        private void validate_LocBID(string LocBID, string strNo)
        {


            if (LocBID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(LocBID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "LocBID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "LocBID no must be number");
                }
                else
                {
                    //int _BLOCID = Convert.ToInt32(LocBID);
                    //BLocation blocation = db.BLocations.Where(x => x.ID == _BLOCID).FirstOrDefault();
                    //if (blocation == null)
                    //{
                    //    ErrCounter = ErrCounter + 1;
                    //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                    //    AddError(strNo, "Location B Id Not found");
                    //}
                }

            }
        }



        private void validate_LocCID(string LocCID, string strNo)
        {


            if (LocCID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(LocCID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "LocCID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "LocCID no must be number");
                }
                else
                {
                    //int _CLOCID = Convert.ToInt32(LocCID);
                    //CLocation clocation = db.CLocations.Where(x => x.ID == _CLOCID).FirstOrDefault();
                    //if (clocation == null)
                    //{
                    //    ErrCounter = ErrCounter + 1;
                    //    //dictErrors.Add(ErrCounter, "Supplier No Not found Ref. SrNo" + strNo);
                    //    AddError(strNo, "Location C Id Not found");
                    //}
                }
            }
        }



        private void validate_CostCenterAID(string CostCenterAID, string strNo)
        {


            if (CostCenterAID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(CostCenterAID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "CostCenterAID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "CostCenterAID no must be number");
                }
            }
        }



        private void validate_CostCenterBID(string CostCenterBID, string strNo)
        {
            if (CostCenterBID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(CostCenterBID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    // dictErrors.Add(ErrCounter, "CostCenterBID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "CostCenterBID no must be number");
                }
            }
        }


        private void validate_ITGroupID(string ITGroupID, string strNo)
        {
            if (ITGroupID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(ITGroupID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "ITGroupID no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "ITGroupID no must be number");
                }
            }
        }



        private void validate_NormalRate(string NormalRate, string strNo)
        {
            if (NormalRate.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(NormalRate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Normal Rate no must be number");
                }
            }
        }



        private void validate_AdditionalRate(string AdditionalRate, string strNo)
        {
            if (AdditionalRate.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AdditionalRate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Additional Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Additional Rate no must be number");
                }
            }
        }


        private void validate_TotalRate(string TotalRate, string strNo)
        {


            if (TotalRate.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "TotalRate is mandatory Input Ref. SrNo" + strNo);
                AddError(strNo, "TotalRate is mandatory");
            }

            if (TotalRate.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(TotalRate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Additional Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Additional Rate no must be number ");
                }
            }
        }


        private void validate_DepMethod(string DepMethod,string strNo)
        {


            if (DepMethod.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset Name is mandatory  Ref. SrNo" + strNo);
                AddError(strNo, "Depreciation Method is mandatory");
            }else
            {
                if ((DepMethod =="WDV") || (DepMethod == "SLM"))
                {
                    int zzz = 0;
                }else
                {
                    AddError(strNo, "Depreciation Method must be one of 'WDV' or 'SLM' ");
                }
            }
        }

        private void validate_GrossVal(string GrossVal, string strNo)
        {
            if (GrossVal.Trim().Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Asset Name is mandatory  Ref. SrNo" + strNo);
                AddError(strNo, "Gross Value is mandatory");
            }
            else if (GrossVal.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(GrossVal) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Gross Value must be number");
                }
            }
        }

        private void validate_ServiceCharges(string ServiceCharges, string strNo)
        {
            if (ServiceCharges.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(ServiceCharges) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Service Charges must be number");
                }
            }
        }

        private void validate_OtherExp(string OtherExp, string strNo)
        {
            if (OtherExp.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(OtherExp) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Other Exp must be number");
                }
            }
        }


        private void validate_CustomDuty(string CustomDuty, string strNo)
        {
            if (CustomDuty.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(CustomDuty) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Other Exp must be number");
                }
            }
        }

        private void validate_ExciseDuty(string ExciseDuty, string strNo)
        {
            if (ExciseDuty.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(ExciseDuty) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Excise Duty must be number");
                }
            }
        }


        private void validate_ServiceTax(string ServiceTax, string strNo)
        {
            if (ServiceTax.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(ServiceTax) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Service Tax must be number");
                }
            }
        }


        private void validate_AnyOtherDuty(string AnyOtherDuty, string strNo)
        {
            if (AnyOtherDuty.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AnyOtherDuty) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Any Other Duty must be number");
                }
            }
        }

        private void validate_VAT(string VAT, string strNo)
        {
            if (VAT.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(VAT) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "VAT must be number");
                }
            }
        }


        private void validate_CST(string CST, string strNo)
        {
            if (CST.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(CST) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "CST must be number");
                }
            }
        }


        private void validate_CGST(string CGST, string strNo)
        {
            if (CGST.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(CGST) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "CGST must be number");
                }
            }
        }


        private void validate_IGST(string IGST, string strNo)
        {

            if (IGST.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(IGST) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "IGST must be number");
                }
            }

        }


        private void validate_SGST(string SGST, string strNo)
        {

            if (SGST.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(SGST) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "SGST must be number");
                }
            }

        }



        private void validate_AnyTax(string AnyTax, string strNo)
        {

            if (AnyTax.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AnyTax) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AnyTax must be number");
                }
            }

        }

        private void validate_TotalAddition(string TotalAddition, string strNo)
        {

            if (TotalAddition.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(TotalAddition) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "TotalAddition must be number");
                }
            }

        }



        private void validate_Discount(string Discount, string strNo)
        {

            if (Discount.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(Discount) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Discount must be number");
                }
            }

        }



        private void validate_RoundingOff(string RoundingOff, string strNo)
        {

            if (RoundingOff.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(RoundingOff) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "RoundingOff must be number");
                }
            }

        }



        private void validate_TotalDeduction(string TotalDeduction, string strNo)
        {

            if (TotalDeduction.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(TotalDeduction) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "TotalDeduction must be number");
                }
            }
        }




        private void validate_InvoiceAmount(string InvoiceAmount, string strNo)
        {

            if (InvoiceAmount.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(InvoiceAmount) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "InvoiceAmount must be number");
                }
            }
        }


        private void validate_DutyDrwaback(string DutyDrwaback, string strNo)
        {

            if (DutyDrwaback.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(DutyDrwaback) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "DutyDrwaback must be number");
                }
            }
        }



        private void validate_ExciseCredit(string ExciseCredit, string strNo)
        {

            if (ExciseCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(ExciseCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "ExciseCredit must be number");
                }
            }
        }


        private void validate_ServiceTaxCredit(string ServiceTaxCredit, string strNo)
        {

            if (ServiceTaxCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(ServiceTaxCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "ServiceTaxCredit must be number");
                }
            }

        }



        private void validate_AnyOtherDutyCredit(string AnyOtherDutyCredit, string strNo)
        {

            if (AnyOtherDutyCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AnyOtherDutyCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AnyOtherDutyCredit must be number");
                }
            }

        }



        private void validate_VatCredit(string VatCredit, string strNo)
        {

            if (VatCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(VatCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "VatCredit must be number");
                }
            }
        }



        private void validate_CSTCredit(string CSTCredit, string strNo)
        {

            if (CSTCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(CSTCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "CSTCredit must be number");
                }
            }
        }



        private void validate_CGSTCredit(string CGSTCredit, string strNo)
        {

            if (CGSTCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(CGSTCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "CGSTCredit must be number");
                }
            }
        }



        private void validate_SGSTCredit(string SGSTCredit, string strNo)
        {

            if (SGSTCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(SGSTCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "SGSTCredit must be number");
                }
            }
        }



        private void validate_IGSTCredit(string IGSTCredit, string strNo)
        {

            if (IGSTCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(IGSTCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "IGSTCredit must be number");
                }
            }
        }



        private void validate_AnyOtherCredit(string AnyOtherCredit, string strNo)
        {

            if (AnyOtherCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AnyOtherCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AnyOtherCredit must be number");
                }
            }
        }



        private void validate_TotalCredit(string TotalCredit, string strNo)
        {

            if (TotalCredit.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(TotalCredit) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "TotalCredit must be number");
                }
            }
        }



        private void validate_AmtCaptilized(string AmtCaptilized, string strNo)
        {
            if (AmtCaptilized.Length == 0)
            { 
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AmtCaptilized must be number");
            }
            else if (AmtCaptilized.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AmtCaptilized) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AmtCaptilized must be number");
                }
            }
        }

        private void validate_AmtCaptilizedCompany(string AmtCaptilizedCompany, string strNo)
        {
            if (AmtCaptilizedCompany.Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                AddError(strNo, "AmtCaptilizedCompany must be number");
            }
            else if (AmtCaptilizedCompany.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AmtCaptilizedCompany) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AmtCaptilizedCompany must be number");
                }
            }
        }

        private void validate_AmtCaptilizedIT(string AmtCaptilizedIT, string strNo)
        {
            if (AmtCaptilizedIT.Length == 0)
            {
                ErrCounter = ErrCounter + 1;
                //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                AddError(strNo, "AmtCaptilizedCompany must be number");
            }
            else if (AmtCaptilizedIT.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(AmtCaptilizedIT) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AmtCaptilizedIT must be number");
                }
            }
        }




        private void validate_OPAccDepreciation(string OPAccDepreciation, string strNo)
        {
            if (OPAccDepreciation.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(OPAccDepreciation) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "OPAccDepreciation must be number");
                }
            }
        }



        private void validate_ResidualVal(string ResidualVal, string strNo)
        {
            if (ResidualVal.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(ResidualVal) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "ResidualVal must be number");
                }
            }
        }


        private void validate_BrandName(string BrandName, string strNo)
        {
            if (BrandName.Trim().Length > 0)
            {
                // check lengths 
                if (BrandName.Length > 100)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                    AddError(strNo, "BrandName length should not be more thant 100 chars");
                }
            }
        }

        private void validate_SrNo(string AssetSrno, string strNo)
        {
            if (AssetSrno.Trim().Length > 0)
            {
                // check lengths 
                if (AssetSrno.Length > 150)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                    AddError(strNo, "AssetSrno length should not be more thant 150 chars");
                }
            }
        }
        private void validate_Model(string Model, string strNo)
        {
            if (Model.Trim().Length > 0)
            {
                // check lengths 
                if (Model.Length > 150)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                    AddError(strNo, "Model length should not be more thant 150 chars");
                }
            }
        }

        private void validate_Remarks(string Remarks, string strNo)
        {
            if (Remarks.Trim().Length > 0)
            {
                // check lengths 
                if (Remarks.Length > 200)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                    AddError(strNo, "Remarks length should not be more thant 200 chars");
                }
            }
        }



        private void validate_IsImported(string IsImported, string strNo)
        {
            if (IsImported.Trim().Length > 0)
            {
                // check lengths 
                if (IsImported.Length > 1)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                    AddError(strNo, "Remarks length should not be more thant 1 (Y/N) chars");
                }
            }
        }


        private void validate_Currency(string Currency, string strNo)
        {
            if (Currency.Trim().Length > 0)
            {
                // check lengths 
                if (Currency.Length > 100)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                    AddError(strNo, "Currency length should not be more than 100 chars");
                }
            }
        }

        private void validate_Values(string Values, string strNo)
        {
            if (Values.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(Values) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "ResidualVal must be number");
                }
            }
        }


        private void validate_YrofManufacturing(string YrofManufacturing, string strNo)
        {
            if (YrofManufacturing.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(YrofManufacturing) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "YrofManufacturing must be number");
                }
            }
        }



        private void validate_MRRNo(string MRRNo, string strNo)
        {
            if (MRRNo.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsDecimal(MRRNo) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "MRRNo must be number");
                }
            }
        }



        private void validate_AccountID(string AccountID, string strNo)
        {
            if (AccountID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(AccountID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AccountID must be number");
                }
            }
        }


        private void validate_DepAccountId(string DepAccountID, string strNo)
        {
            if (DepAccountID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(DepAccountID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "DepAccountID must be number");
                }
            }
        }


        private void validate_AccAccountID(string AccAccountID, string strNo)
        {
            if (AccAccountID.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(AccAccountID) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "AccAccountID must be number");
                }
            }
        }



        private void validate_Usefullife(string Usefullife, string strNo)
        {
            if (Usefullife.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(Usefullife) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Usefullife must be number");
                }
            }
        }


        private void validate_Parent_AssetNo(string Parent_AssetNo, string strNo)
        {
            if (Parent_AssetNo.Trim().Length > 0)
            {
                //asset no should be numeric
                if (IsNumeric(Parent_AssetNo) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Normal Rate no must be number Ref. SrNo" + strNo);
                    AddError(strNo, "Parent_AssetNo must be number");
                }
            }
        }


        private void validate_Parent_Iscomponent(string Iscomponent, string strNo)
        {
            if (Iscomponent.Trim().Length > 0)
            {
                if (Iscomponent.Length > 1)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Asset name length should not be more thant 200 chars. Ref. SrNo" + strNo);
                    AddError(strNo, "Iscomponent length should not be more than 1 (Y/N) chars");
                }
            }
        }

        private void validate_ExpiryDate(string ExpiryDate, string strNo)
        {
            if (ExpiryDate.Trim().Length > 0)
            {
                // check the format of date

                if (ValidateDate(ExpiryDate) == false)
                {
                    ErrCounter = ErrCounter + 1;
                    //dictErrors.Add(ErrCounter, "Invalid Date Format is mandatory  Ref. SrNo" + strNo);
                    AddError(strNo, "Invalid Date Format for Receipt Date");
                }
            }
        }

        private bool ValidateDate(string strDate)
        {

            DateTime tempDate;
            if (DateTime.TryParseExact(strDate, DateFormat, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out tempDate) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool SaveAssets()
        {
            return false;
        }
    }
}