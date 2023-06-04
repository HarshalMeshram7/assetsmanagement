using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class DisposalReportViewModel
    {
        
        public int assetid { get; set; }
        public int companyid { get; set; }
        public string AGroupName { get; set; }
        public string BGroupName { get; set; }
        public string CGroupName { get; set; }
        public string DGroupName { get; set; }
        public string AssetNo { get; set; }
        public string AssetIdentificationNo { get; set; }
        public string AssetName { get; set; }

        public decimal? Disposal { get; set; }
        
        public decimal? DispoDep { get; set; } //Disposal Depreciation
        
        public int? Qty { get; set; }
        public string SrNo { get; set; }
        public string Model { get; set; }
        public string ALocName { get; set; }
        public string BLocName { get; set; }
        public string CLocName { get; set; }
        public DateTime? DisposalDate { get; set; }


    }
}