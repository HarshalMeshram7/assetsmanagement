using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.DTO
{

    //public interface Location
    //{
    //     int id { get; set; }
    //     int LocationName { get; set; }
    //}

    public class LocationDTO 
    {
        public string AssetNo { get; set; }

        public int ALocID { get; set; }
        public string  ALocationName { get; set; }

        public int BLocID { get; set; }
        public string BLocationName { get; set; }

        public int CLocID { get; set; }
        public string CLocationName { get; set; }

    }

    //public class BLocationDTO : Location
    //{
    //    public int id { get; set; }
    //    public int LocationName { get; set; }
    //    public int ALocID { get; set; }
    //    public int ALocationName { get; set; }
    //}

    //public class CLocationDTO : Location
    //{
    //    public int id { get; set; }
    //    public int LocationName { get; set; }
    //    public int BLocID { get; set; }
    //    public int BLocationName { get; set; }
    //}
}