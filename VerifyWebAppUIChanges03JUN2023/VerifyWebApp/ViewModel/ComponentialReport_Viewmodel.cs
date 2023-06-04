using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class ComponentialReport_Viewmodel
    {
        public string componentname{get;set;}
        public string parentassetname { get; set; }
        public string str_componentdate { get; set; }
        public string str_parent_assetdate { get; set; }
        public decimal? componentusefullife { get; set; }
        public decimal? parent_asset_usefullife { get; set; }
        public decimal? componentamtcapcomp { get; set; }
        public decimal? additionamtcapcomp { get; set; }
        public decimal? parent_asset_amtcapcomp { get; set; }
        public string Assetno { get; set; }
        public string ParentAssetno { get; set; }

    }
}