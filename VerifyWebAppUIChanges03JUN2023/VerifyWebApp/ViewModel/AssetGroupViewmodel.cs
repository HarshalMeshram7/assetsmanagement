using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
namespace VerifyWebApp.ViewModel
{
    public class AssetGroupViewmodel
    {
        public AssetGroupViewmodel()
        {
            this.locationlist = new List<Childlocation>();
            this.costcenterlist = new List<Childcostcenter>();
            this.assetfreeofcostlist = new List<Assetfreeofcost>();
            this.insurancelist = new List<SubInsurance>();
            this.amclist = new List<SubAmc>();
            this.loanlist = new List<SubLoan>();
        }
        public int ID { get; set; }
        public int DisposalFlag { get; set; }
        public int? Parent_AssetId { get; set; }
        public string iscomponent { get; set; }//yes/no
        public string Parent_assetno { get; set; }
        public int? ClientID { get; set; }
        public int CompanyID { get; set; }
        public string AssetNo { get; set; }
        public string AssetIdentificationNo { get; set; }
        public string AssetName { get; set; }
        public string VoucherNo { get; set; }
        public DateTime? VoucherDate { get; set; }
        public DateTime? PODate { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? CommissioningDate { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? DtPutToUse { get; set; }
        public DateTime? DtPutToUseIT { get; set; }
        public string PONo { get; set; }
        public string BillNo { get; set; }
        public string MRRNo { get; set; } //Material Receipt No.

        public int? Qty { get; set; }
        public int? SupplierNo { get; set; }
        public int? UOMNo { get; set; } // UOMNo
        public decimal? OPAccDepreciation { get; set; }
        public decimal? GrossVal { get; set; }
        public decimal? ServiceCharges { get; set; }
        public decimal? OtherExp { get; set; }
        public decimal? CustomDuty { get; set; }
        public decimal? ExciseDuty { get; set; }
        public decimal? ServiceTax { get; set; }
        public decimal? AnyOtherDuty { get; set; }
        public decimal? VAT { get; set; }
        public decimal? CSt { get; set; }
        public decimal? CGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? IGST { get; set; }
        public decimal? AnyOtherTax { get; set; }
        public decimal? TotalAddition { get; set; } //(A)
        public decimal? Discount { get; set; }
        public decimal? Roundingoff { get; set; }
        public decimal? TotDeduction { get; set; } //(B)
        public decimal? InvoiceAmt { get; set; }  // (C=A-B)
        public decimal? DutyDrawback { get; set; }
        public decimal? ExciseCredit { get; set; }
        public decimal? ServiceTaxCredit { get; set; }
        public decimal? AnyOtherDutyCredit { get; set; }
        public decimal? VATCredit { get; set; } // 
        public decimal? CSTCredit { get; set; } // 
        public decimal? CGSTCredit { get; set; } // 
        public decimal? SGSTCredit { get; set; } // 
        public decimal? IGSTCredit { get; set; }
        public decimal? AnyOtherCredit { get; set; }
        public decimal? TotalCredit { get; set; } // (D)
        public decimal? AmountCapitalised { get; set; } //(E=C-D)
        public decimal? AmountCapitalisedCompany { get; set; }
        public decimal? AmountCApitalisedIT { get; set; }
        public int? AGroupID { get; set; }
        public int? BGroupID { get; set; }
        public int? CGroupID { get; set; }
        public int? DGroupID { get; set; }
        public int? LocAID { get; set; }
        public int? LocBID { get; set; }
        public int? LocCID { get; set; }
        public int? CostCenterAID { get; set; }
        public int? CostCenterBID { get; set; } //Sub Cost Center ID
        public int? ITGroupIDID { get; set; }
        public string DepreciationMethod { get; set; } //SLM WDV
        public decimal? NormalRatae { get; set; }
        public decimal? AdditionalRate { get; set; }
        public decimal? TotalRate { get; set; }
        public decimal? ResidualVal { get; set; }
        public decimal? Usefullife { get; set; }
        public int? YrofManufacturing { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? AccountID { get; set; }//purchase accountid
        public int? DepAccountId { get; set; } //Depreciation AccountId
        public int? AccAccountID { get; set; } //Accumulated Account Id
        public string BrandName { get; set; }
        public string SrNo { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public string IsImported { get; set; }
        public string Currency { get; set; }
        public decimal? Values { get; set; }


       
        public int Attachemntcount { get; set; }
        public string levelid { get; set; }
        public int int_Srno { get; set; }
        
        public string str_VoucherDate { get; set; }
   
        public string str_PODate { get; set; }
       
        public string str_ReceiptDate { get; set; }
    
        public string str_CommissioningDate { get; set; }
       
        public string str_BillDate { get; set; }
        
        public string str_DtPutToUse { get; set; }

        public string str_DtPutToUseIT { get; set; }

        public string str_suppliername { get; set; }
  
        public string uom_name { get; set; }

        public string str_Expirydate { get; set; }
      
        public string str_locationname { get; set; }
  
        public string str_mainlocation { get; set; }
       
        public string str_sublocation { get; set; }
       
        public string str_sub_sublocation { get; set; }
        
        public string str_purchaseaccountname { get; set; }
       
        public string str_depricationname { get; set; }
        
        public string str_accumulatedname { get; set; }
        
        public string str_it_name { get; set; }
        
        public string str_costcenteraname { get; set; }
        
        public string str_costcenterbname { get; set; }

        public IEnumerable<Childlocation> locationlist { get; set; }
        public IEnumerable<Childcostcenter> costcenterlist { get; set; }
        public IEnumerable<Assetfreeofcost> assetfreeofcostlist { get; set; }
        public IEnumerable<SubInsurance> insurancelist { get; set; }
        public IEnumerable<SubAmc> amclist { get; set; }
        public IEnumerable<SubLoan> loanlist { get; set; }
    }
    //public class locationtable
    //{
    //    public locationtable()
    //    {

    //    }

    //    public int ID { get; set; }
    //    public DateTime Date { get; set; }
    //    public decimal Locationaid { get; set; }
    //    public int Locationbid { get; set; }
    //    public int Locationcid { get; set; }
    //    public string str_locaname { get; set; }
    //    public string str_locbname { get; set; }
    //    public string str_loccname { get; set; }
    //    public int srno { get; set; }
    //    public string str_date { get; set; }
    //}
    //public class costcentertable
    //{
    //    public costcentertable()
    //    {

    //    }

    //    public int ID { get; set; }
    //    public DateTime Date { get; set; }
    //    public decimal Percentage { get; set; }
    //    public int Costcenteraid { get; set; }
    //    public int Costcenterbid { get; set; }
    //    public string str_costcenteraname { get; set; }
    //    public string str_costcenterbname { get; set; }
    //    public int srno { get; set; }
    //    public string str_date { get; set; }
    //}
    //public class assetfreeofcosttable
    //{
    //    public assetfreeofcosttable()
    //    {

    //    }

    //    public int ID { get; set; }
    //    public string Description { get; set; }   // Cost Center Code
    //  //  public int Asset_id { get; set; } //Cost Center Description
    //    public DateTime Date { get; set; }
    //    public int Uom { get; set; }
    //    public string str_uomname { get; set; }
    //    public int Qty { get; set; }
    //    public int srno { get; set; }
    //    public string str_date { get; set; }
    //}
}