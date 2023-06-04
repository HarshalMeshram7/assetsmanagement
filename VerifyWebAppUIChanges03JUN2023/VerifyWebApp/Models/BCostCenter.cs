using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblbcostcenter")]
    public class BCostCenter
    {
        [Key]
        public int ID { get; set; }
        public int? Companyid { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int CCID { get; set; } // Cost Center Id
        public string SCCCode { get; set; }   // Sub Cost Center Code
        public string SCCDescription { get; set; } //Sub Cost Center Description
        [NotMapped]
        public string CCCode { get; set; }   // Cost Center Code
        [NotMapped]
        public string CCDescription { get; set; } //Cost Center Description
        public int ClientID { get; set; }
    }
}