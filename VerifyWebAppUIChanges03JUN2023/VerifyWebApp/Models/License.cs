using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{       [Table("tbllicense")]
    public class License
    {
        [Key]
       public int  ID { get; set; }
       public int Company_Creation_Count { get; set; }
        public DateTime? Valid_From { get; set; }
        public DateTime? Valid_Till { get; set; }
        public string  CompanyCode { get; set; }
    }
}