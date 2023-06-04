using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{[Table("tblcompanypermission")]
    public class CompanyPermission
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
}