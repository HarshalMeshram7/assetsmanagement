using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblSubloan")]
    public class SubLoan
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        [NotMapped]
        public int Srno { get; set; }
        public int AssetId { get; set; }
        [NotMapped]
        public string AssetNo { get; set; }
        public string AssetDescription { get; set; }
        public decimal CapitalisedAmount { get; set; }
        public int LoanId { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [NotMapped]
        public string str_fromdate { get; set; }
        [NotMapped]
        public string str_todate { get; set; }
        [NotMapped]
        public string loandetails { get; set; }
    }
}