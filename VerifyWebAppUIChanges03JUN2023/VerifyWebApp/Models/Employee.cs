using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblemployee")]
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        public string EmpId { get; set; }
        public int Companyid { get; set; }
        public int CreatedUserId { get; set; }
        public int ModifiedUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Emailid { get; set; }
        public string Address1 { get; set; }
        public string Mobileno { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}