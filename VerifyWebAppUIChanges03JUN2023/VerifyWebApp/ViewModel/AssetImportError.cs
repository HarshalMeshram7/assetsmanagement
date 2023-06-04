using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class AssetImportError
    {
        
        public int ExcelRowNo { get; set; }
        public string SrNo { get; set; }
        public string AssetName { get; set; }
        public string Error { get; set; }
    }
}