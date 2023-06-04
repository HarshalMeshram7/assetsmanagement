﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class FARReportViewmodel
    {

        public string AGroupName { get; set; }
        public int assetid { get; set; }
        public string BGroupName { get; set; }
        public string CGroupName { get; set; }
        public string DGroupName { get; set; }
        public int? AGrpNo { get; set; }
        public int? BGrpNo { get; set; }
        public int? CGrpNo { get; set; }
        public int? DGrpNo { get; set; }

        public string AssetNo { get; set; }
        public string AssetIdentificationNo { get; set; }
        public string AssetName { get; set; }

        public decimal? OpGross { get; set; }
        public decimal? Addition { get; set; }
        public decimal? Disposal { get; set; }
        public decimal? ClGross { get; set; }

        public decimal? OpDep { get; set; }
        public decimal? UpToDep { get; set; }
        public decimal? DispoDep { get; set; } //Disposal Depreciation
        public decimal? TotDep { get; set; } //Total Depreciation
        public decimal? NetBalance { get; set; }

        public decimal? ResidualVal { get; set; }

        public decimal? DepRate { get; set; }
        public string DepMethod { get; set; }

        public DateTime? voucherDate { get; set; }
        //public string voucherDate { get; set; }

        public string VoucherNo { get; set; }
        public string PONo { get; set; }

        public DateTime? DTPutUse { get; set; }
        //public string DTPutUse { get; set; }

        public string Remarks { get; set; }
        public int? Qty { get; set; }

        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        //public string BillDate { get; set; }


        public string SrNo { get; set; }
        public string Model { get; set; }
        public string ALocName { get; set; }
        public string BLocName { get; set; }
        public string CLocName { get; set; }
        public string SupplierName { get; set; }

        //public string CCCode { get; set; }
        //public string CCDescription { get; set; }
        public string str_voucherdate { get; set; }
        public string str_billdate { get; set; }
        public string str_dateputtousedate { get; set; }
        public int OpeningQty { get; set; }
        public int DisposedQtyTillFromDate { get; set; }
        public int DisposedQty { get; set; }
        public int ClosingQty { get; set; }
        /*
        public int companyid { get; set; }
        public int assetid { get; set; }

        public string AGroupName { get; set; }
        public string BGroupName { get; set; }
        public string CGroupName { get; set; }
        public string DGroupName { get; set; }
        public string AssetNo { get; set; }
        public string AssetIdentificationNo { get; set; }
        public string AssetName { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime Dateputtouse { get; set; }
        public string str_VoucherDate { get; set; }
        public string str_DTPutUseCompany { get; set; }
        public int? Qty { get; set; }
        public string SupplierName { get; set; }
        public decimal? DepRate { get; set; } //Depreciation Rate
        public string DepMethod { get; set; } //Depreciation Method(SLM/WDV)
        public decimal? AmountCapitalisedCompany { get; set; }
        public decimal? AmountCapitalisedIT { get; set; }
        public decimal? DepreciationAmount { get; set; }
        public decimal? NetBalance { get; set; }
        public decimal? TotalCedit { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string TransactionType { get; set; }

    */



    }
}