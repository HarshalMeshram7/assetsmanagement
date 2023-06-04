using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using OfficeOpenXml;

namespace VerifyWebApp.BusinessLogic
{
    /// <summary>
    /// Provide logic to import and validate all asset
    /// </summary>
    public class AssetImportNew
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

        // dict to maintian if value is present in Excel Row ,, 
        Dictionary<int, bool> dictIsValuePresent = new Dictionary<int, bool>();
        Dictionary<int, string> dictValues = new Dictionary<int, string>();

        Dictionary<int, string> dictErrors = new Dictionary<int, string>();

        public ExcelWorksheet workSheet { get; set; }

        List<Assets> lstAssets = new List<Assets>();

        public VerifyDB db = new VerifyDB();

        public int ErrCounter = 0;

        public AssetImportNew()
        {
            lstAssets = db.Assetss.ToList();

        }

        private bool ReadRow(int RowNum )
        {
            try
            {
                for (int iColCounter = 1; iColCounter <= 81; iColCounter++)
                {

                    if (workSheet.Cells[RowNum, iColCounter].Text == "")
                    {
                        dictIsValuePresent[iColCounter] = false;
                        dictValues[iColCounter] = "";

                    }
                    else
                    {
                        string strValue = workSheet.Cells[RowNum, iColCounter].Value.ToString();
                        dictValues[iColCounter] = strValue;
                        dictIsValuePresent[iColCounter] = true;
                    }

                }
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private AssetImportDTO ReadAsset()
        {
            AssetImportDTO tempAsset = new AssetImportDTO();
            try
            {
                foreach (var item in dictValues)
                {
                    if (item.Key == COL_SRNo)
                    {
                        tempAsset.Row_SrNo = item.Value;
                    }
                    if (item.Key == COL_ASSETNO)
                    {
                        tempAsset.AssetNo = item.Value;
                    }
                    if (item.Key == COL_ASSETNAME)
                    {
                        tempAsset.AssetName = item.Value;
                    }

                    if (item.Key == COL_ASSETIDENTIFICATION_NO)
                    {
                        tempAsset.AssetIdentificationNo = item.Value;
                    }

                    if (item.Key == COL_VOUCHERNO)
                    {
                        tempAsset.VoucherNo = item.Value;
                    }

                    if (item.Key == COL_VOUCHERDATE)
                    {
                        tempAsset.VoucherDate = item.Value;
                    }


                    if (item.Key == COL_DTPUTTOUSE)
                    {
                        tempAsset.DtPutToUse = item.Value;
                    }

                    if (item.Key == COL_DtPutToUseIT)
                    {
                        tempAsset.DtPutToUseIT = item.Value;
                    }

                    if (item.Key == COL_PONo)
                    {
                        tempAsset.PONo = item.Value;
                    }

                    if (item.Key == COL_PODate)
                    {
                        tempAsset.PODate = item.Value;
                    }


                    if (item.Key == COL_BillNo)
                    {
                        tempAsset.BillNo = item.Value;
                    }

                    if (item.Key == COL_BillDate)
                    {
                        tempAsset.BillDate = item.Value;
                    }


                    if (item.Key == COL_ReceiptDate)
                    {
                        tempAsset.ReceiptDate = item.Value;
                    }

                    if (item.Key == COL_CommissioningDate)
                    {
                        tempAsset.CommissioningDate = item.Value;
                    }


                    if (item.Key == COL_Qty)
                    {
                        tempAsset.Qty = item.Value;
                    }


                    if (item.Key == COL_uomno)
                    {
                        tempAsset.UOMNo= item.Value;
                    }


                    if (item.Key == COL_uomno)
                    {
                        tempAsset.UOMNo = item.Value;
                    }

                    if (item.Key == COL_SupplierNo)
                    {
                        tempAsset.SupplierNo = item.Value;
                    }

                    if (item.Key == COL_AGroupId)
                    {
                        tempAsset.AGroupID = item.Value;
                    }

                    if (item.Key == COL_BGroupId)
                    {
                        tempAsset.BGroupID = item.Value;
                    }

                    if (item.Key == COL_CGroupId)
                    {
                        tempAsset.CGroupID = item.Value;
                    }

                    if (item.Key == COL_DGroupId)
                    {
                        tempAsset.DGroupID = item.Value;
                    }

                    if (item.Key == COL_LocAId)
                    {
                        tempAsset.LocAID= item.Value;
                    }

                    if (item.Key == COL_LocBId)
                    {
                        tempAsset.LocBID = item.Value;
                    }

                    if (item.Key == COL_LocCId)
                    {
                        tempAsset.LocCID= item.Value;
                    }

                    if (item.Key == COL_CostCenterAId)
                    {
                        tempAsset.CostCenterAID = item.Value;
                    }

                    if (item.Key == COL_CostCenterBId)
                    {
                        tempAsset.CostCenterBID = item.Value;
                    }

                    if (item.Key == COL_NormalRate)
                    {
                        tempAsset.NormalRatae = item.Value;
                    }

                    if (item.Key == COL_AdditionalRate)
                    {
                        tempAsset.AdditionalRate = item.Value;
                    }

                    if (item.Key == COL_TotalRate)
                    {
                        tempAsset.TotalRate = item.Value;
                    }


                    if (item.Key == COL_DepreciationMethod)
                    {
                        tempAsset.DepreciationMethod = item.Value;
                    }

                    if (item.Key == COL_grossvalue)
                    {
                        tempAsset.GrossVal = item.Value;
                    }

                    if (item.Key == COL_servicecharge)
                    {
                        tempAsset.ServiceCharges = item.Value;
                    }

                    if (item.Key == COL_otherexpense)
                    {
                        tempAsset.OtherExp = item.Value;
                    }

                    if (item.Key == COL_exciseduty)
                    {
                        tempAsset.ExciseDuty = item.Value;
                    }


                    if (item.Key == COL_servicetax)
                    {
                        tempAsset.ServiceTax = item.Value;
                    }

                    if (item.Key == COL_anyotherduty)
                    {
                        tempAsset.AnyOtherDuty = item.Value;
                    }

                    if (item.Key == COL_vat)
                    {
                        tempAsset.VAT = item.Value;
                    }

                    if (item.Key == COL_cst)
                    {
                        tempAsset.CSt = item.Value;
                    }

                    if (item.Key == COL_cgst)
                    {
                        tempAsset.CGST = item.Value;
                    }

                    if (item.Key == COL_igst)
                    {
                        tempAsset.IGST = item.Value;
                    }

                    if (item.Key == COL_sgst)
                    {
                        tempAsset.SGST = item.Value;
                    }


                    if (item.Key == COL_anyothertax)
                    {
                        tempAsset.AnyOtherTax = item.Value;
                    }

                    if (item.Key == COL_totaladdition)
                    {
                        tempAsset.TotalAddition = item.Value;
                    }

                    if (item.Key == COL_discount)
                    {
                        tempAsset.Discount = item.Value;
                    }

                    if (item.Key == COL_roundoff)
                    {
                        tempAsset.Roundingoff = item.Value;
                    }

                    if (item.Key == COL_totaldeduction)
                    {
                        tempAsset.TotalDeduction= item.Value;
                    }

                    if (item.Key == COL_invoiceamt)
                    {
                        tempAsset.InvoiceAmt = item.Value;
                    }

                    if (item.Key == COL_dutydrawback)
                    {
                        tempAsset.DutyDrawback = item.Value;
                    }

                    if (item.Key == COL_excisecredit)
                    {
                        tempAsset.ExciseCredit = item.Value;
                    }

                    if (item.Key == COL_servicetaxcredit)
                    {
                        tempAsset.ServiceTaxCredit = item.Value;
                    }

                    if (item.Key == COL_anyotherdutycredit)
                    {
                        tempAsset.AnyOtherDutyCredit = item.Value;
                    }

                    if (item.Key == COL_vatcredit)
                    {
                        tempAsset.VATCredit = item.Value;
                    }

                    if (item.Key == COL_cstcredit)
                    {
                        tempAsset.CSTCredit = item.Value;
                    }


                    if (item.Key == COL_cgstcredit)
                    {
                        tempAsset.CGSTCredit = item.Value;
                    }

                    if (item.Key == COL_sgstcredit)
                    {
                        tempAsset.SGSTCredit = item.Value;
                    }

                    if (item.Key == COL_igstcredit)
                    {
                        tempAsset.IGSTCredit = item.Value;
                    }


                    if (item.Key == COL_anyothercredit)
                    {
                        tempAsset.AnyOtherCredit = item.Value;
                    }

                    if (item.Key == COL_totalcredit)
                    {
                        tempAsset.TotalCredit = item.Value;
                    }

                    if (item.Key == COL_AMOUNTCAPITALISED)
                    {
                        tempAsset.AmountCapitalised = item.Value;
                    }

                    if (item.Key == COL_AMOUNT_CAPT_COMPANYLAW)
                    {
                        tempAsset.AmountCapitalisedCompany= item.Value;
                    }

                    if (item.Key == COL_AMOUNT_CAPT_ITLAW)
                    {
                        tempAsset.AmountCApitalisedIT = item.Value;
                    }

                    if (item.Key == COL_OPAccDepreciation)
                    {
                        tempAsset.OPAccDepreciation = item.Value;
                    }

                    if (item.Key == COL_residual)
                    {
                        tempAsset.ResidualVal= item.Value;
                    }

                    if (item.Key == COL_BrandName)
                    {
                        tempAsset.BrandName = item.Value;
                    }


                    if (item.Key == COL_SRNo)
                    {
                        tempAsset.SrNo_Asset = item.Value;
                    }

                    if (item.Key == COL_MODEL)
                    {
                        tempAsset.Model = item.Value;
                    }

                    if (item.Key == COL_REMARKS)
                    {
                        tempAsset.Remarks = item.Value;
                    }


                    if (item.Key == COL_ISIMPORTED)
                    {
                        tempAsset.IsImported = item.Value;
                    }


                    if (item.Key == COL_CURRENCY)
                    {
                        tempAsset.Currency = item.Value;
                    }


                    if (item.Key == COL_VALUES)
                    {
                        tempAsset.Values = item.Value;
                    }

                    if (item.Key == COL_YEAR_OF_MFG)
                    {
                        tempAsset.YrofManufacturing= item.Value;
                    }

                    if (item.Key == COL_MRRNo)
                    {
                        tempAsset.MRRNo = item.Value;
                    }

                    if (item.Key == COL_AccountID)
                    {
                        tempAsset.AccountID = item.Value;
                    }


                    if (item.Key == COL_DepAccountId)
                    {
                        tempAsset.DepAccountId = item.Value;
                    }


                    if (item.Key == COL_AccAccountID)
                    {
                        tempAsset.AccAccountID = item.Value;
                    }


                    if (item.Key == COL_Usefullife)
                    {
                        tempAsset.Usefullife = item.Value;
                    }

                    if (item.Key == COL_ParentAssetNo)
                    {
                        tempAsset.ParentAssetNo = item.Value;
                    }


                    if (item.Key == COL_ParentAssetNo)
                    {
                        tempAsset.ParentAssetNo = item.Value;
                    }



                    if (item.Key == COL_iscomponent)
                    {
                        tempAsset.IsComponent = item.Value;
                    }

                    if(item.Key == COL_ExpiryDate)
                    {
                        tempAsset.ExpiryDate = item.Value;
                    }

                }
                return tempAsset;

            }
            catch(Exception ex)
            {
                return tempAsset;
            }
        }

        private bool ValidateColumns()
        {
            // 1. Check Unique Asset No
            /* Check Mandatory columns
             * 
                -- Asset No
                -- Asset Name
                -- Voucher Date
                -- Date Put to Use
                -- Date Put to Use IT
                -- AGroupID
                -- ALoc ID
                -- Dep Method
                -- Dep Rate
                -- Amt Cap
                -- Amt CapIT
                -- Usefill Life



            // check if all dates are in valid format

            // check reference integrity for
                AGroup 
                BGroup
                CGroup
                DGroup

                Location A ID
                Location B ID
                Location C ID


                Cost Center A ID
                Cost Center B ID

                Supplier No


            */
            try
            {
                if (dictValues.Count > 0)
                {
                    string strSrno = dictValues[COL_SRNo].ToString();

                    string strTemp = "";
                    // check if mandatory present 
                    strTemp = dictIsValuePresent[COL_ASSETNO].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter+1, "Asset no is mandatory Input Ref. SrNo" + strSrno); 
                        return false;
                    }

                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_ASSETNAME].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter+1, "Asset name is mandatory Input Ref. SrNo" + strSrno);
                        return false;
                    }


                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_VOUCHERDATE].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Voucher date is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }


                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_DTPUTTOUSE].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Date Put to Use is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }

                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_DtPutToUseIT].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Date Put to Use IT is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }


                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_AGroupId].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "A Group ID is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }



                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_LocAId].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "A Location  is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }

                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_DepreciationMethod].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Depreciation Method is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }


                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_TotalRate].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Total Rate is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }


                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_AMOUNT_CAPT_COMPANYLAW].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Amt Capitalized is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }


                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_AMOUNT_CAPT_ITLAW].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Amt Capitalized IT Law is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }

                    strTemp = "";
                    strTemp = dictIsValuePresent[COL_Usefullife].ToString();

                    if (strTemp == "")
                    {
                        dictErrors.Add(ErrCounter + 1, "Usefull life is mandatory Input. Ref. SrNo" + strSrno);
                        return false;
                    }




                    // check valid dates ?






                    return true;
                }else
                {
                    return false;
                }

            }catch(Exception ex)
            {
                return false;
            }
        }
        public Assets GetAsset(int CompanyID)
        {

            Assets obj_asset = new Assets();


            try
            {
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;

                List<Assets> lstAssets = new List<Assets>();
                List<AGroup> lstAGroups = new List<AGroup>();
                List<BGroup> lstBGroups = new List<BGroup>();
                List<CGroup> lstCGroups = new List<CGroup>();
                List<DGroup> lstDGroups = new List<DGroup>();



                lstAssets = db.Assetss.Where(x=>x.Companyid == CompanyID).ToList();
                lstAGroups = db.AGroups.Where(x => x.Companyid == CompanyID).ToList();
                lstBGroups = db.BGroups.Where(x => x.Companyid == CompanyID).ToList();
                lstCGroups = db.CGroups.Where(x => x.Companyid == CompanyID).ToList();
                lstDGroups = db.DGroups.Where(x => x.Companyid == CompanyID).ToList();


                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {

                    dictIsValuePresent.Clear();
                    dictValues.Clear();

                    bool bRow =
                    bRow = ReadRow(rowIterator); // read row and add it to DICT

                    if (bRow == true)
                    {

                        AssetImportDTO temp_asset = ReadAsset();
                    }

                   

                }



            }
            catch (Exception ex)
            {
                // TODO  Log error
            }



            return obj_asset;

            // Mandar 17 APR 2022

            // perform All Validations Here

        }
    }
}