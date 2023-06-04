using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace VerifyWebApp.Models
{
    [Table("tbldepreciationrequest")]
    public  class DepreciationRequest
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime  EndDateTime { get; set; }
        public int InProcess { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    [Table("tbldepreciationrequest_incometax")]
    public class DepreciationRequestIncomeTax
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int InProcess { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
