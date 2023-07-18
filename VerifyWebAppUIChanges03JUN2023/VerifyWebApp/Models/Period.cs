using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblperiod")]
    public class Period
    {[Key]
        public int ID { get; set; }
        public DateTime FromDate { get; set; }
       
        public DateTime ToDate { get; set; }
        public int Months { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ClientID { get; set; }
        [NotMapped]
        public string str_fromdate { get; set; }
        [NotMapped]
        public string str_todate { get; set; }
        [NotMapped]
        public int Srno { get; set; }
        [NotMapped]
        public List<SubPeriod> SubPeriodTable { get; set; }

        // public string DepFlag { get; set; }  //Depreciation Calculation Flag (Y/N)        
    }
}