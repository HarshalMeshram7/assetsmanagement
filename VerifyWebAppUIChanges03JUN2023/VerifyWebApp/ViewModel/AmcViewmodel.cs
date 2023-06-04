using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class AmcViewmodel
    {
        public AmcViewmodel()
        {
            this.AmcViewModellist = new List<SubamcTable>();
        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ReminderEMailID { get; set; }
        public string Senderccemailid { get; set; }
        public string AmcDetails { get; set; }
        public string Remarks { get; set; }
        public int ClientID { get; set; }
        public int ID { get; set; }

        public IEnumerable<SubamcTable> AmcViewModellist { get; set; }
    }
    public class SubamcTable
    {
        public SubamcTable()
        {

        }

        public int AssetId { get; set; }
     
        public string AssetNo { get; set; }
        public string AssetDescription { get; set; }
        public decimal CapitalisedAmount { get; set; }
        public int InsuranceId { get; set; }

    }
}