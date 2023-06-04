using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tbldepreciation_log")]
    public class DepreciationLog
    {
        [Key]
        public int ID { get; set; }
        public int AssetID { get; set; }
        public string message { get; set; }

    }
}
