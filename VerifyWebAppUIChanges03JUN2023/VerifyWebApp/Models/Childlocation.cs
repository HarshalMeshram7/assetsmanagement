using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblchildlocation")]
    public class Childlocation
    {
        [Key]

        public int ID { get; set; }
        public int? Companyid { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ALocID { get; set; }
        public int? BLocID { get; set; }
        public int? CLocID { get; set; }
        public DateTime? Date { get; set; }
        public int? AssetID { get; set; }
        [NotMapped]
        public string str_date { get; set; }
        [NotMapped]
        public int Srno { get; set; }
        [NotMapped]
        public string str_locaname { get; set; }
        [NotMapped]
        public string str_locbname { get; set; }
        [NotMapped]
        public string str_loccname { get; set; }
        [NotMapped]
        public string assetno { get; set; }
        [NotMapped]
        public string assetname { get; set; }
    }
}