using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tbldisposal")]
    public class Disposal
    {
        [Key]
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public string VoucherNo { get; set; }
        public string BillNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime DisposalDate { get; set; }
        public DateTime BillDate { get; set; }
        public string DisposalType { get; set; } //Full or Partial
        public string Remarks { get; set; }
        public int Qty { get; set; }
        public string CustomerName { get; set; }
        public decimal DisposalAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal OpAccumulatedDepTill { get; set; }
        public decimal DepChargedFrom { get; set; }
        public decimal OpAccumulatedDep { get; set; }
        public decimal DepForFromDtToDt { get; set; }
        public decimal TotalDepreciation { get; set; }
        public decimal WDvDisposedOff { get; set; }
        public decimal ProfitLoss { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DisposalCategory { get; set; } // 
        [NotMapped]
        public int int_Srno { get; set; }
        [NotMapped]
        public string str_voucherDate { get; set; }
        [NotMapped]
        public string str_disposalDate { get; set; }
        [NotMapped]
        public string str_billDate { get; set; }
        [NotMapped]
        public decimal? DepRate { get; set; }
        [NotMapped]
        public string DepMethod { get; set; }
        [NotMapped]
        public string AssetNo { get; set; }///  /// from asset id get assetnos
        [NotMapped]
        public int Attachemntcount { get; set; }


        //extrafield salevalue
        public decimal SaleValue { get; set; }
    }
}