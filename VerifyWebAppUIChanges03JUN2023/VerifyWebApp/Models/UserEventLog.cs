using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace VerifyWebApp.Models
{
    [Table("tblusereventlog")]
    public class UserEventLog
    {
        [Key]
        public int ID { get; set; }
        public DateTime TranDate { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public string Event { get; set; } // ADD / EDIT / DELETE / EXECUTE 
        public string EventDescription { get; set; }
        public string SourcePage { get; set; } // e.g. AssetGroup / // Location // / AMC 



    }
}