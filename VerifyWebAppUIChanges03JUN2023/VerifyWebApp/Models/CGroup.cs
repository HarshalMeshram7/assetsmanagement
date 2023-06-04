using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblcgroup")]
    public class CGroup
    {
        [Key]
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public string CGroupName { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int AGrpID { get; set; }
        public int BGrpID { get; set; }
        public decimal NormalRate { get; set; }
        public decimal AdditionalRate { get; set; }
        public decimal TotalRate { get; set; }
        public string DepMethod { get; set; }
        public int ClientID { get; set; }
        [NotMapped]
        public string CGroup_name { get; set; }
    }
}