using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblaccount")]
    public class Account
    {
        [Key]
        public int ID { get; set; }
       
        public string AccountCode { get; set; }   
        public string AccountName { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string GroupName { get; set; }
        public int ClientID { get; set; }
        [NotMapped]
        public int Srno { get; set; }
    }
}