using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;

namespace VerifyWebApp.ViewModel
{
    public class DisposalViewModel
    {
        //public DateTime DispsalDate { get; set; }

        public decimal? AssetAmtCapitalised { get; set; }
        public decimal? OpAccDepTillDipodate { get; set; }
        public decimal? TotalRate { get; set; } //Depreciation Rate
        public string DepMethod { get; set; } //SLM/WDV
        public decimal? OpAccDepreciation { get; set; }
        public decimal? DepChargeFromDDateToDepToDate { get; set; }
        public decimal? DepTillDate { get; set; }
        public decimal? TotalDepreciation { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal opDep { get; set; }
        //new changes done for  updation of disposal page
        public decimal? disposaltilldate { get; set; }
        public decimal? depreciationtilldate { get; set; }
        public decimal? depreciationreversed { get; set; }
        ////////////////
        //public int ID { get; set; }

        public int OpeningQty { get; set; } // added by mandar 17 feb 2021
        public int DisposedQtyTillDate { get; set; } // added by mandar 17 feb 2021

    }

    public class DisposalViewModelWrapper
    {
        public bool IsValid { get; set; }
        public DisposalViewModel model { get; set; }
    }
}