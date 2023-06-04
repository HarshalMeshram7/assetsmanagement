using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class Disposal_ViewModel
    {
        public int ID { get; set; }
        public int companyid { get; set; }
        public string AssetName { get; set; }
        public string AssetNo { get; set; }
        
        public decimal DisposalAmount { get; set; }
        public string DisposalDate { get; set; }
       
        public string DisposalType { get; set; } //Full or Partial
        public string Remarks { get; set; }
        public int Qty { get; set; }

    }
}