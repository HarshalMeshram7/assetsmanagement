using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblbatch")]
    public class Batch
    {
        [Key]
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string BatchDescription { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string IsBatchOpen { get; set; } //(Flag Y/N)
        public int ClientID { get; set; }

        [NotMapped]
        public string str_fromdate { get; set; }
        [NotMapped]
        public string str_todate { get; set; }
        [NotMapped]
        public int Srno { get; set; }
        
        public string batchcode
        {
            get { return  BatchDescription  + " -- "+ID ; }
        }
    }

    [Table("tblbatchassets")]
    public class BatchAsset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Companyid { get; set; }
        public int BatchID { get; set; }
        public string assetno { get; set; }
        public int AssetID { get; set; }// internal to db
    }
}