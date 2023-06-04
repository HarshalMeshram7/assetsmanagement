using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblassetfreeofcost")]
    public class Assetfreeofcost
    {
        [Key]
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public string Description { get; set; }   // Cost Center Code
        public int? Asset_id { get; set; } //Cost Center Description
        public DateTime? Date { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Uom { get; set; }
        public int Qty { get; set; }
        [NotMapped]
        public string str_date { get; set; }
        [NotMapped]
        public string str_uomname { get; set; }
        [NotMapped]
        public int Srno { get; set; }
    }
}