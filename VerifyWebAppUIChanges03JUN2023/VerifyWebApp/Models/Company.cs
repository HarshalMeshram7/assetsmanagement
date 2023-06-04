using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblcompany")]
    public class Company
    {
        [Key]
        public int ID { get; set; }
        public int? ClientID { get; set; }
        [Required(ErrorMessage = "Customer Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public int Userid { get; set; }
        public int?  CreatedUserId { get; set; }
        public int? ModifiedUserid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal ResidualValuePercent { get; set; }
        public string AutoGenerateAssetNo { get; set; }

    }
}