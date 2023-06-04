using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.DTO
{
    public class AssetfreeofcostDTO
    {
      
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public string Description { get; set; }   // Cost Center Code
        public int? Asset_id { get; set; } //Cost Center Description
        public DateTime? Date { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Uom { get; set; }
        public int Qty { get; set; }
     
        public string str_date { get; set; }
      
        public string str_uomname { get; set; }
       
        public int Srno { get; set; }
    }
}