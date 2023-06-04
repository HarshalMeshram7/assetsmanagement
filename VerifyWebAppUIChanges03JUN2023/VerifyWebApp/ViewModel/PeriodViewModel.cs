using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class PeriodViewModel
    {
        public PeriodViewModel()
        {
            this.PeriodViewModellist = new List<SubPeriodTable>();
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int ID { get; set; }
        public int Months { get; set; }
        public int ClientID { get; set; }
        public IEnumerable<SubPeriodTable> PeriodViewModellist { get; set; }
    }
    public class SubPeriodTable
    {
        public SubPeriodTable()
        {
            //this.PeriodViewModellist = new List<PeriodViewModel>();
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PeriodLockFlag { get; set; } // (Y/N)
        public string DepFlag { get; set; }  //Depreciation Calculation Flag (Y/N)
        public int ClientID { get; set; }
    }
}