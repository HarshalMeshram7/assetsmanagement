using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace VerifyWebApp.Models
{
    [Table("tblitgroup")]
    public class ITGroup
    {
        [Key]
        public int ID { get; set; }
        public string GroupName { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        public decimal DepRate { get; set; }
        public string DepMethod { get; set; }
        public decimal OPWDV { get; set; }
        public int ClientID { get; set; }
    }
}