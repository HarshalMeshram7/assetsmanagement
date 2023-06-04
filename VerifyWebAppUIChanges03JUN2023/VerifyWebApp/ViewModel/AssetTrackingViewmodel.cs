using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class AssetTrackingViewmodel
    {
        public int assetid { get; set; }
        public int companyid { get; set; }
        public int employeeid { get; set; }
        public int alocid { get; set; }
        public string AssetNo { get; set; }
        public string AssetIdentificationNo { get; set; }
        public string AssetName { get; set; }
        public string SrNo { get; set; }
        public string Model { get; set; }
        public string ALocName { get; set; }
        public string EmpName { get; set; }
        public string SystemAssetId { get; set; }
        public DateTime IssueDate { get; set; }
         public string str_IssueDate { get; set; }
        public string Remarks { get; set; }
    }
}