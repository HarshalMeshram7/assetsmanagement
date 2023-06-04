using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tbllogin")]
    public class Login
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int ClientID { get; set; }
        public int? CompanyId { get; set; }
        public string Userlevel { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public int? CreatedUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string AppUserid { get; set; }//newlyadded04 nov 2020
        public string IsAppAccess { get; set; }//newlyadded04 nov 2020
        public string IsTwoFactor { get; set; }//newlyadded 06 AUG 2022
        public string OTP { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        [NotMapped]
        public string NewPassword { get; set; }

        
    }
}