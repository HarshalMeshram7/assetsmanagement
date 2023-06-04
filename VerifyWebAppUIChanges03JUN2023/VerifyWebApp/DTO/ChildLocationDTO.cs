using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.DTO
{
    public class ChildLocationDTO
    {
        public int ID { get; set; }
        public int? Companyid { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ALocID { get; set; }
        public int? BLocID { get; set; }
        public int? CLocID { get; set; }
        public DateTime? Date { get; set; }
        public int? AssetID { get; set; }
        
        public string str_date { get; set; }
        
        public int Srno { get; set; }
        
        public string str_locaname { get; set; }
        
        public string str_locbname { get; set; }
        
        public string str_loccname { get; set; }
        
        public string assetno { get; set; }
        
        public string assetname { get; set; }
    }
}