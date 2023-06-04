using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class LoanViewmodel
    {
        public LoanViewmodel()
        {
            this.LoanViewModellist = new List<SubloanTable>();
        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string BankName { get; set; }
        public int Year { get; set; }
        public decimal Percent { get; set; }
        public decimal Amount { get; set; }
        public int ClientID { get; set; }
        public int ID { get; set; }

        public IEnumerable<SubloanTable> LoanViewModellist { get; set; }
    }
    public class SubloanTable
    {
        public SubloanTable()
        {

        }

        public int AssetId { get; set; }
        
        public string AssetNo { get; set; }
        public string AssetDescription { get; set; }
        public decimal CapitalisedAmount { get; set; }
        public int LoanId { get; set; }

    }
}