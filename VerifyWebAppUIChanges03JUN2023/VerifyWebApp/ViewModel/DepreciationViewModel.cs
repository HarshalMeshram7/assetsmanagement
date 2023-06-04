using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{


    
    public class DepreciationViewModel
    {
        public int ID { get; set; }
        public int AssetID{ get; set; }
        public string AssetNo { get; set; }
        public string AssetName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string str_FromDate { get; set; }
        public string str_ToDate { get; set; }
        public decimal TotalRate { get; set; }
        public decimal Amount { get; set; }
        public string DepreciationMethod { get; set; }
        public int DepreciationDays { get; set; }

    }
}