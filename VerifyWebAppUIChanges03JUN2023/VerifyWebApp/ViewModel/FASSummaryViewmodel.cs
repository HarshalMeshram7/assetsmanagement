using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class FASSummaryDetailViewmodel
    {
        public int id { get; set; }
        public int agroupid { get; set; }
        public int bgroupid { get; set; }
        public int cgroupid { get; set; }
        public int dgroupid { get; set; }
        public string AGroupName { get; set; }
        public string BGroupName { get; set; }
        public string CGroupName { get; set; }
        public string DGroupName { get; set; }
        public decimal? OpGross { get; set; }
        public decimal? Addition { get; set; }
        public decimal? Disposal { get; set; }
        public decimal? ClGross { get; set; }
        public decimal? OpDepAsset { get; set; }
        public decimal? UpToDep { get; set; }
        public decimal? OpDep { get; set; }
        public decimal? DepForPeriod { get; set; }
        public decimal? DispoDep { get; set; }
        public decimal? TotDep { get; set; }
        public decimal? NetBalance { get; set; }

    }
    public class FASSummaryViewmodel
    {
        public int id { get; set; }
        public string AGroupName { get; set; }
        public string BGroupName { get; set; }
        public string CGroupName { get; set; }
        public string DGroupName { get; set; }
        public int agropid { get; set; }
        public int? BGrpNo { get; set; }
        public int? CGrpNo { get; set; }
        public int? DGrpNo { get; set; }
        public decimal? OpGross { get; set; }
        public decimal? Addition { get; set; }
        public decimal? Disposal { get; set; }
        public decimal? ClGross { get; set; }

        public decimal? OpDep { get; set; }
        public decimal? DepForPeriod { get; set; }
        public decimal? UpToDep { get; set; }
        public decimal? DispoDep { get; set; } //Disposal Depreciation
        public decimal? TotDep { get; set; } //Total Depreciation
        public decimal? NetBalance { get; set; }



    }
}