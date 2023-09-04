using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblsupplier")]
    public class Supplier
    {
        // [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Supplier Code is required")]
        [MaxLength(100, ErrorMessage = "Code cannot be longer than 100 characters.")]
        public string SupplierCode { get; set; }
        [Required(ErrorMessage = "Supplier Name is required")]
        [MaxLength(150, ErrorMessage = "Name cannot be longer than 150 characters.")]
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string City { get; set; }
        public int? Pincode { get; set; }
        public string Msemeno { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public string ExciseRegNo { get; set; }
        public string ServiceTaxRegNo { get; set; }
        public string VATRegNo { get; set; }
        public string CSTRegNo { get; set; }
        public string AnyOtherRegNo { get; set; }
        public string PANNo { get; set; }
        public string TANNo { get; set; }
        public string GSTNo { get; set; }
        public string EmailID { get; set; }
        public string ShopActLicence { get; set; }
        public int? ClientID { get; set; }

        [NotMapped]
        public int Srno { get; set; }

    }
}