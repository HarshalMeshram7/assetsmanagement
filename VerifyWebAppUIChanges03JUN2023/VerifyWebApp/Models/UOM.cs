using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerifyWebApp.Models
{
    [Table("tbluom")]
    public class UOM
    {
        public int ID { get; set; }
        public string Unit { get; set; }
        public int? ClientID { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid  {get;set;}
        public int?  Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}