using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{

    public class BatchLocation
    {
        [Key]
        public int ID { get; set; }
        public int BatchID { get; set; }
        public int LocAID { get; set; }
        public int LocBID { get; set; }
        public int LocCID { get; set; }
        public string LocA { get; set; }       
        public string LocB { get; set; }
        public string LocC { get; set; }
        public int? ClientID { get; set;}
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}