using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblemployeeasset")]
    public class EmployeeAsset
    {
        [Key]
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime IssueDate { get; set; }
        
        [NotMapped]
        public string str_empname { get; set; }
        [NotMapped]
        public string str_assetno { get; set; }
        [NotMapped]
        public string str_assetname{ get; set; }
        [NotMapped]
        public string str_IssueDate { get; set; }
        public string AssetRecievedFlag { get; set; }
        public int Companyid { get; set; }
        public int CreatedUserId { get; set; }
        public int ModifiedUserId { get; set; }
        public int AssetId { get; set; }
        [NotMapped]
        public string str_employeeid { get; set; }
        public int EmpId { get; set; }
        public string Empid { get; set; }


        public DateTime? RecievedDate { get; set; }
        [NotMapped]
        public string str_RecievedDate { get; set; }
        [NotMapped]
        public string empno { get; set; }

        [NotMapped]
        public string AssetIdentificationNo { get; set; }



    }
}