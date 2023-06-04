using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class UsefullifeReportViewModel
    {
        public int id { get; set; }
        public int companyid { get; set; }
        public string AssetNo { get; set; }
        public string AssetName { get; set; }
        public decimal? AmountCapitalised { get; set; }
        public DateTime Dateputtouse { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string str_Dateputtouse { get; set; }
        public string str_ExpiryDate { get; set; }
        public decimal? OpGross { get; set; }
        public decimal? Addition { get; set; }
        public decimal? Disposal { get; set; }
        public decimal? ClGross { get; set; }
        public decimal? OpDep { get; set; }
        public decimal? UpToDep { get; set; }
        public decimal? DispoDep { get; set; }
        public decimal? TotDep { get; set; }
        public decimal? NetBalance { get; set; }


    }
}