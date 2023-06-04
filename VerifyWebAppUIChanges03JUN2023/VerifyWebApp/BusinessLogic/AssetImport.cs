using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
using System.Data.Entity;
using VerifyWebApp.BusinessLogic;
using VerifyWebApp.Filter;
using System.Globalization;

namespace VerifyWebApp.BusinessLogic
{
    public class AssetImport
    {
        public ExcelWorksheet ImportSheet { get; set; }

        // Purpose : to check if row is valid to Import in dataabase

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
        public int COL_igstcredit = 57;
        public int COL_sgstcredit = 58;

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
        public int COL_CURRENCY= 71;
        public int COL_VALUES = 72;
        public int COL_YEAR_OF_MFG = 73;
        public int COL_MRRNo = 74;
        public int COL_AccountID = 75;
        public int COL_DepAccountId = 76;
        public int COL_AccAccountID = 77;
        public int COL_Usefullife = 78;
        public int COL_iscomponent = 79;
        public int COL_ExpiryDate = 80;

        // Main function to import excel

        public string DATE_FORMAT_EXCEL = "dd/MM/yyyy";

        List<ImportError> lstError = new List<ImportError>();

        Dictionary<string, string> dictAssetNo = new Dictionary<string, string>();

        public VerifyDB db = new VerifyDB();

        public void ImportExcel()
        {

            int noOfCol = ImportSheet.Dimension.End.Column;
            int  noOfRow = ImportSheet.Dimension.End.Row;

            DATE_FORMAT_EXCEL = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            if (ImportSheet == null)
            {
                return;
            }

            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                Assets obj_Asset = new Assets();

                ValidateRow(rowIterator);
            }

        }

        public void Validate_SRNo(string _srNO,int rowNumber)
        {
            try
            {
                int _srno = Convert.ToInt32(_srNO);
                //return true;
            }
            catch (Exception ex)
            {
                ImportError error_SRNO = new ImportError();
                error_SRNO.RowNo = rowNumber;
                error_SRNO.ErrorFound = "Sr No is Not Numeric";
                error_SRNO.ColumnNo = COL_SRNo;
                lstError.Add(error_SRNO);
                //return false;
            }
        }

        public void Validate_AssetNo(string _AssetNo,int rowNumber)
        {
            try
            {
                int _assetno = Convert.ToInt32(_AssetNo);
                Assets ast = db.Assetss.Where(x => x.AssetNo == _assetno.ToString()).FirstOrDefault();

                // Check if exists in database

                if (ast != null)
                {
                    ImportError error_AssetNo = new ImportError();
                    error_AssetNo.RowNo = rowNumber;
                    error_AssetNo.ErrorFound = "Asset No" + _assetno + " Already Exists";
                    error_AssetNo.ColumnNo = COL_ASSETNO;
                    lstError.Add(error_AssetNo);
                }

                try
                {
                    dictAssetNo.Add(_assetno.ToString(), _assetno.ToString());
                }
                catch (Exception ex) // check if Duplicate asset no found in Excel sheet
                {
                    ImportError error_AssetNo = new ImportError();
                    error_AssetNo.RowNo = rowNumber;
                    error_AssetNo.ErrorFound = "Duplicate Asset No" + _assetno + " Row No" + rowNumber;
                    error_AssetNo.ColumnNo = COL_ASSETNO;
                    lstError.Add(error_AssetNo);

                }

            }
            catch (Exception ex)
            {
                ImportError error_AssetNo = new ImportError();
                error_AssetNo.RowNo = rowNumber;
                error_AssetNo.ErrorFound = "Asset No is Not Numeric";
                error_AssetNo.ColumnNo = COL_ASSETNO;
                lstError.Add(error_AssetNo);
            }
        }

        public void Validate_VoucherDate(string _VoucherDate, int rowNumber)
        {
            // verify if Voucher Data exits  and Format is dd/MM/yyyy;

            if (_VoucherDate.Length == 0)
            {
                ImportError error_AssetNo = new ImportError();
                error_AssetNo.RowNo = rowNumber;
                error_AssetNo.ErrorFound = "Voucher Date required for Row No" + rowNumber + " required format (dd/MM/yyy)";
                error_AssetNo.ColumnNo = COL_VOUCHERDATE;
                lstError.Add(error_AssetNo);

            }


            DateTime dt;

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            bool valid = DateTime.TryParseExact(_VoucherDate, DATE_FORMAT_EXCEL, null, DateTimeStyles.None, out dt);

            

            if (!valid)
            {

                ImportError error_AssetNo = new ImportError();
                error_AssetNo.RowNo = rowNumber;
                error_AssetNo.ErrorFound = "Invalid Date format for  Row No" + rowNumber + " required format (dd/MM/yyy)";
                error_AssetNo.ColumnNo = COL_VOUCHERDATE;
                lstError.Add(error_AssetNo);

            }


        }


        public void Validate_DatePutToUse(string _Date, int rowNumber)
        {
            // verify if Voucher Data exits  and Format is dd/MM/yyyy;

            if (_Date.Length == 0)
            {
                ImportError error_AssetNo = new ImportError();
                error_AssetNo.RowNo = rowNumber;
                error_AssetNo.ErrorFound = "Dt Put to Use required for Row No" + rowNumber + " required format (dd/MM/yyy)";
                error_AssetNo.ColumnNo = COL_DTPUTTOUSE;
                lstError.Add(error_AssetNo);

            }
            DateTime dt;

            bool valid = DateTime.TryParseExact(_Date, DATE_FORMAT_EXCEL, null, DateTimeStyles.None, out dt);

            if (!valid)
            {

                ImportError error_AssetNo = new ImportError();
                error_AssetNo.RowNo = rowNumber;
                error_AssetNo.ErrorFound = "Dt Put to Use for Row No" + rowNumber + " required format (dd/MM/yyy)";
                error_AssetNo.ColumnNo = COL_DTPUTTOUSE;
                lstError.Add(error_AssetNo);

            }


        }

        public void ValidateRow(int rowIterator)
        {

            string value_SrNo = "";
            string value_AssetNo = "";
            string value_VoucherDate = "";
            string value_DtPutToUse = "";

            value_SrNo = ImportSheet.Cells[rowIterator, COL_SRNo].Text;
            Validate_SRNo(value_SrNo, rowIterator);

            value_AssetNo = ImportSheet.Cells[rowIterator, COL_ASSETNO].Text;
            Validate_AssetNo(value_AssetNo, rowIterator);

            value_VoucherDate = ImportSheet.Cells[rowIterator, COL_VOUCHERDATE].Text; // Mandatory Column
            Validate_VoucherDate(value_VoucherDate, rowIterator); // Mandatory Column

            value_DtPutToUse = ImportSheet.Cells[rowIterator, COL_DTPUTTOUSE].Text; // Mandatory Column
            Validate_DatePutToUse(value_VoucherDate, rowIterator); // Mandatory Column



        }
    }

    public class ImportError
    {
        public int RowNo { get; set; }
        public string ErrorFound { get; set; }
        public int ColumnNo { get; set; }

    }
}