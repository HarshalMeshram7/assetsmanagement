using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class DashBoardGroupWiseAssets
    {
        public int id { get; set; }
        public string  groupname { get; set; }
        public decimal amt { get; set; }
    }

    public class DashBoardAsset
    {
        public int id { get; set; }
        public string assetno { get; set; }
        public string assetname { get; set; }
        public decimal amtcapitalised { get; set; }
        public decimal amtdep { get; set; }
        public decimal amtdisposed { get; set; }
    }
}