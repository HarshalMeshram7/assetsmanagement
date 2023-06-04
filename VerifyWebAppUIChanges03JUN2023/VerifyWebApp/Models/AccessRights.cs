using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblaccessrights")]
    public class AccessRights
    {
        [Key]
        public int Id { get; set; }
        public int Userid  { get; set; }
        [NotMapped]
        public string username { get; set; }
        public string ControllerName  { get; set; }
        public string Add  { get; set; }
        public string Edit  { get; set; }
        public string Delete  { get; set; }
        public int? CreatedUserid { get; set; }
        public int? Companyid { get; set; }
        public int? ModifiedUserid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Export { get; set; }
        public string Import { get; set; }
        public string Index { get; set; }
      
    }
}