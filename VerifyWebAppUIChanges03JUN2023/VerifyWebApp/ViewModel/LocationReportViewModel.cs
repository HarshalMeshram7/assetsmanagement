using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class LocationReportViewModel
    {

        public int assetid { get; set; }
        public int companyid { get; set; }
        public string AssetNo { get; set; }
        public string AssetName { get; set; }
        public string AssetIdentification { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime Dateputtouse { get; set; }
        public string SupplierName { get; set; }
        public int Qty { get; set; }
        public string Location { get; set; }
        public string Sublocation { get; set; }
        public string Sub_Sublocation { get; set; }
        public decimal? AmountCapitalised { get; set; }
        public DateTime IssueDate { get; set; }
        public string str_voucherdate { get; set; }
        public string str_DtPutToUse { get; set; }
        public string str_issuedate { get; set; }
    }
}