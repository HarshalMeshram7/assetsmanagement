using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblacostcenter")]
    public class ACostCenter
    {
        [Key]
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public string CCCode { get; set; }   // Cost Center Code
        public string CCDescription { get; set; } //Cost Center Description
        public string Operative { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ClientID { get; set; }

    }
}