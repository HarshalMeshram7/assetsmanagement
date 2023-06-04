using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    public class AssetImportDTO
    {

        

      
        public string Row_SrNo { get; set; }
        public string AssetNo { get; set; }
        public string AssetName { get; set; }
        public string AssetIdentificationNo { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string DtPutToUse { get; set; }
        public string DtPutToUseIT { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string ReceiptDate { get; set; }
        public string CommissioningDate { get; set; }
        public string Qty { get; set; }
        public string UOMNo { get; set; } // UOM ID
        public string SupplierNo { get; set; }



        public string AGroupID { get; set; }
        public string BGroupID { get; set; }
        public string CGroupID { get; set; }
        public string DGroupID { get; set; }

        public string LocAID { get; set; }
        public string LocBID { get; set; }
        public string LocCID { get; set; }

        public string CostCenterAID { get; set; }
        public string CostCenterBID { get; set; } //Sub Cost Center ID

        public string ITGroupIDID { get; set; }

        public string NormalRatae { get; set; }
        public string AdditionalRate { get; set; }
        public string TotalRate { get; set; }


        public string DepreciationMethod { get; set; }
        public string GrossVal { get; set; }

        public string ServiceCharges { get; set; }
        public string OtherExp { get; set; }
        public string CustomDuty { get; set; }
        public string ExciseDuty { get; set; }
        public string ServiceTax { get; set; }
        public string AnyOtherDuty { get; set; }

        public string VAT { get; set; }
        public string CSt { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }

        public string AnyOtherTax { get; set; }
        public string TotalAddition { get; set; } //(A)

        public string Discount { get; set; }
        public string Roundingoff { get; set; }
        public string TotalDeduction { get; set; } //(B)
        public string InvoiceAmt { get; set; }  // (C=A-B)

        public string DutyDrawback { get; set; }
        public string ExciseCredit { get; set; }
        public string ServiceTaxCredit { get; set; }
        public string AnyOtherDutyCredit { get; set; }
        public string VATCredit { get; set; } // 
        public string CSTCredit { get; set; } // 
        public string CGSTCredit { get; set; } // 
        public string SGSTCredit { get; set; } // 
        public string IGSTCredit { get; set; }
        public string AnyOtherCredit { get; set; }
        public string TotalCredit { get; set; } // (D)
        public string AmountCapitalised { get; set; } //(E=C-D)
        public string AmountCapitalisedCompany { get; set; }
        public string AmountCApitalisedIT { get; set; }

        public string MRRNo { get; set; } //Material Receipt No.
        public string OPAccDepreciation { get; set; }
        public string ResidualVal { get; set; }
        public string Usefullife { get; set; }
        public string ParentAssetNo { get; set; }
        public string YrofManufacturing { get; set; }
        public string ExpiryDate { get; set; }
        public string AccountID { get; set; }//purchase accountid
        public string DepAccountId { get; set; } //Depreciation AccountId
        public string AccAccountID { get; set; } //Accumulated Account Id
        public string BrandName { get; set; }
        public string SrNo_Asset { get; set; }
        public string Model { get; set; }
        public string Remarks { get; set; }
        public string IsImported { get; set; }
        public string Currency { get; set; }
        public string Values { get; set; }

        public string IsComponent { get; set; }
    }
}