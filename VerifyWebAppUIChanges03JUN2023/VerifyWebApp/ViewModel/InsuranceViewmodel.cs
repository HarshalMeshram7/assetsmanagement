using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class InsuranceViewmodel
    {
        public InsuranceViewmodel()
        {
            this.InsuranceViewModellist = new List<SubinsuranceTable>();
        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string EMailID { get; set; }
        public string Senderccemailid { get; set; }
        public string PolicyDetails { get; set; }
        public string Remarks { get; set; }
        public int ClientID { get; set; }
        public int ID { get; set; }

        public IEnumerable<SubinsuranceTable> InsuranceViewModellist { get; set; }
    }
    public class SubinsuranceTable
    {
        public SubinsuranceTable()
        {
            
        }

        public int AssetId { get; set; }
        
        public string AssetNo { get; set; }
        public string AssetDescription { get; set; }
        public decimal CapitalisedAmount { get; set; }
        public int InsuranceId { get; set; }

    }
}