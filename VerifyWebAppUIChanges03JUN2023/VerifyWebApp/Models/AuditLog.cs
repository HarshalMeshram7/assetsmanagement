using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace VerifyWebApp.Models
{
    [Table("tblauditlog")]
    public class AuditLogRecord
    {


        [Key]
        public int id { get; set; }
        public int userid { get; set; }
        public int companyid { get; set; }
        public DateTime trandate { get; set; }
        public int eventid { get; set; }
        public int recordtype { get; set; }
        public string guid { get; set; } // grouping of record
        public string column { get; set; }
        public string oldvalue { get; set; }
        public string newvalue { get; set; }

    }

    public class AuditRecord {
        public string column { get; set; }
        public string oldvalue { get; set; }
        public string newvalue { get; set; }
    }
}