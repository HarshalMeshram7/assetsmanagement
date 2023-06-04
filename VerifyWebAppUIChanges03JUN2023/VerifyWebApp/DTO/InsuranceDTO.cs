using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.DTO
{
    public class InsuranceDTO
    {

        public int ID { get; set; }
        public DateTime FromDate { get; set; }
        public string Senderccemailid { get; set; }
        public DateTime ToDate { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string EMailID { get; set; }
        public string PolicyDetails { get; set; }
        public string Remarks { get; set; }
        public int ClientID { get; set; }
  
        public string str_fromdate { get; set; }

        public string str_todate { get; set; }

        public int Srno { get; set; }
    }
}