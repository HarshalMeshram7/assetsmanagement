using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblsubperiod")]
    public class SubPeriod
    {
        [Key]
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public int PeriodID { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PeriodLockFlag { get; set; } // (Y/N)
        public string DepFlag { get; set; }  //Depreciation Calculation Flag (Y/N)
        public int ClientID { get; set; }
        [NotMapped]
        public string str_fromdate { get; set; }
        [NotMapped]
        public string str_todate { get; set; }
        [NotMapped]
        public int Srno { get; set; }

        [NotMapped]
        public string Fromdatetodate { get { return str_fromdate + " -To- " + str_todate; } }
    }
}