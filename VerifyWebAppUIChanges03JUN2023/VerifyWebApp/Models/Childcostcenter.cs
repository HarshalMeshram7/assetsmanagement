using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblchildcostcenter")]
    public class Childcostcenter
    {
        [Key]
       
        public int ID { get; set; }
        public int? Companyid { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AcostcenterID { get; set; }
        public int? BcostcenterID { get; set; }
        public string Percentage { get; set; }
        public DateTime? Date { get; set; }
        public int? Ass_ID { get; set; }
        [NotMapped]
        public string str_date { get; set; }
        [NotMapped]
        public int Srno { get; set; }
        [NotMapped]
        public string str_costcenteraname { get; set; }
        [NotMapped]
        public string str_costcenterbname { get; set; }
        [NotMapped]
        public string assetno { get; set; }
        [NotMapped]
        public string assetname { get; set; }

    }
}