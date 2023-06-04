using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.ViewModel;
using VerifyWebApp.Models;
 using OfficeOpenXml;

namespace VerifyWebApp.BusinessLogic
{
    public class AssetDisposalRepository
    {
        public int COL_SRNo = 1;
        public int COL_ASSETNO = 2;
        public int COL_ASSETNAME = 3;
        public int COL_Disposal_DATE = 4;
        public int COL_VOUCHER_NO = 5;
        public int COL_VOUCHER_DATE = 6;
        public int COL_BILL_DATE = 7;
        public int COL_DISPOSAL_TYPE = 8; // F-Full / P-Partial
        public int COL_QTY = 9;
        public int COL_DISPOSAL_AMOUNT = 10;
        public int COL_DISPOSAL_DEPRECIATION = 11;
        public int COL_DISPOSAL_CATEGORY = 12; // Others , Scrap, Sold

        private string DateFormat { get; set; }

        public List<AssetImportError> lstErrors = new List<AssetImportError>();

        private VerifyDB db;

        private int ErrCounter = 0;

        public AssetDisposalRepository(VerifyDB db)
        {
            this.db = db;
            //DateFormat = "DDMMYYYY";
            DateFormat = "ddMMyyyy";

        }

        public int companyid { get; set; }

        public int userid { get; set; }

        /* ReadExcel - To load excel rows in temp variables
         */
        public void ReadExcel(ExcelWorksheet workSheet)
        {

        }

        public void Validate()
        {

        }

        public void SaveDisposals()
        {

        }

    }
}