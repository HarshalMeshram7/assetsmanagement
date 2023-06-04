using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tbladdition")]
    public class Addition
    {
       
        [NotMapped]
        public int int_Srno { get; set; }
        public int? CreatedUserId { get; set; }
        [NotMapped]
        public int Attachemntcount { get; set; }
        [NotMapped]
        public string str_VoucherDate { get; set; }
        [NotMapped]
        public string str_PODate { get; set; }
        [NotMapped]
        public string str_ReceiptDate { get; set; }
        [NotMapped]
        public string str_CommissioningDate { get; set; }
        [NotMapped]
        public string str_BillDate { get; set; }
        [NotMapped]
        public string str_DtPutToUse { get; set; }
        [NotMapped]
        public string str_DtPutToUseIT { get; set; }
        [NotMapped]
        public string str_suppliername { get; set; }
        [NotMapped]
        public string uom_name { get; set; }
        [NotMapped]
        public string AssetNo { get; set; }///  /// from asset id get assetnos

        /// </summary>
        [Key]
        public int ID { get; set; }
        public int? AdditionNo { get; set; }
        public int? AssetId { get; set; }
        public int ClientID { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string AssetName { get; set; }
        public string AdditionAssetName { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime PODate { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime CommissioningDate { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DtPutToUse { get; set; }
        public DateTime DtPutToUseIT { get; set; }
        public string PONo { get; set; }
        public string BillNo { get; set; }
        public string MRRNo { get; set; }
        public int Qty { get; set; }
        public int SupplierNo { get; set; }
        public string BrandName { get; set; }
        public string SrNo { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public string IsImported { get; set; }
        public string Currency { get; set; }
        public decimal Values { get; set; }
        public decimal GrossVal { get; set; }
        public decimal ServiceCharges { get; set; }
        public decimal OtherExp { get; set; }
        public decimal CustomDuty { get; set; }
        public decimal ExciseDuty { get; set; }
        public decimal ServiceTax { get; set; }
        public decimal AnyOtherDuty { get; set; }
        public decimal VAT { get; set; }
        public decimal CST { get; set; }
        public decimal GST { get; set; }
        public decimal GSTCredit { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal AnyOtherTax { get; set; }
        public decimal TotalAddition { get; set; }
        public decimal Discount { get; set; }
        public decimal Roundingoff { get; set; }
        public decimal TotDeduction { get; set; }
        public decimal InvoiceAmt { get; set; }
        public decimal DutyDrawback { get; set; }
        public decimal ExciseCredit { get; set; }
        public decimal ServiceTaxCredit { get; set; }
        public decimal AnyOtherDutyCredit { get; set; }
        public decimal VATCredit { get; set; }
        public decimal CSTCredit { get; set; }
        public decimal CGSTCredit { get; set; }
        public decimal SGSTCredit { get; set; }
        public decimal IGSTCredit { get; set; }
        public decimal AnyOtherCredit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal AmountCapitalised { get; set; }
        public decimal AmountCapitalisedCompany { get; set; }
        public decimal AmountCApitalisedIT { get; set; }
        public decimal ResidualVal { get; set; }
        public int uom { get; set; }
      


    }
}