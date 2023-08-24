using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class BatchVerificationViewModel
    {
        public int assetid { get; set; }
        public int companyid { get; set; }
        public string AssetNo { get; set; }
        public string AssetIdentificationno { get; set; }
        public string VerificationStatus { get; set; }
        public string AssetName { get; set; }
        public string SrNo { get; set; }
        public string Model { get; set; }
        public string systemassetid { get; set; }
        public string Location { get; set; }
        public string SubLocation { get; set; }
        public string Sub_SubLocation { get; set; }
        public string Remarks { get; set; }
        public string GeoLocation { get; set; }

        public DateTime ? Lastupdatetimestamp { get; set; }

        //public string nameofperson { get; set; }

    }
}