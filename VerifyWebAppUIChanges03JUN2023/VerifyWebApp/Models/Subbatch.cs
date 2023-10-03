using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblsubbatch")]
    public class Subbatch
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        [NotMapped]
        public int Srno { get; set; }
        public int LocAId { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int LocBId { get; set; }
        public int LocCId { get; set; }

        public string LocAName { get; set; }
        public string LocBName { get; set; }
        public string LocCName { get; set; }

        public int BatchId { get; set; }

        [NotMapped]
        public int Srnocc { get; set; }
        public int CCId { get; set; }
        public int SCCId { get; set; }
        public string CCDescription { get; set; }
        public string SCCDescription { get; set; }

        [NotMapped]
        public int Srnoac { get; set; }
        public int Minvalue { get; set; }
        public int Maxvalue { get; set; }

    }
}