using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System;


namespace VerifyWebApp.Models
{
    [Table("tbldepreciationcalculation")]
    public class DepCalculation
    {
        [Key]
        public int ID { get; set; }
        public int AssetID { get; set; }
        public decimal OpeningGross { get; set; }
        public decimal OpeningAccumalatedDep { get; set; }
        public decimal DepRate { get; set; }
        public string DepType { get; set; }
        public decimal ResidualValue { get; set; }
        public DateTime? AssetExpiryDate { get; set; }
        public decimal DepTillFromDate { get; set; }
        public decimal DisposedTillFromDate { get; set; }
        public string DisposalType { get; set; }
        public decimal DisposalAmt { get; set; }
        public DateTime? DisposalDate { get; set; }
        public decimal AssetAmt { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int NoOfDays { get; set; }
        public decimal DepreciationAmt { get; set; }
        public int companyid { get; set; }
        public string AssetName { get; set; }
        public decimal NormalRate { get; set; }
        public decimal AdditionRate { get; set; }

        public string DepMethod { get; set; }
        public DateTime? Assetdtputuse { get; set; }
        public int Usefullife { get; set; }
        public decimal dep_till_startdt { get; set; }
        public decimal disp_gross_block { get; set; }
        public decimal dep_on_disp_st_dt_to_sale_dt { get; set; }
        public decimal dep_rev_on_disposal { get; set; }
        public decimal amt_for_dep_calc { get; set; }
        public decimal net_block_stdt { get; set; }
        public decimal dep_for_period { get; set; }
        public decimal net_block_endt { get; set; }

        [NotMapped]
        public int AssetLifeInDays { get; set; }

        [NotMapped]
        public int LifeTillStartDate_InDays { get; set; }


    }
}
