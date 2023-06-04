using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Models
{
    [Table("tblitdepreciation")]
    public class ITDepreciation
    {
        [Key]
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int Companyid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int ITGrpID { get; set; }
        public string ITGroupName { get; set; }
        public decimal? OpeningWDV { get; set; }

        public decimal? DepreciationRate { get; set; }
        public decimal? Additionbefore { get; set; }
        public decimal? AdditionAfter { get; set; }
        public decimal? TotalFull { get; set; }
        public decimal? TotalHalf { get; set; }
        public decimal? DisposalBefore { get; set; }
        public decimal? DisposalAfter { get; set; }
        public decimal? FinalTotalFull { get; set; }
        public decimal? FinalTotalHalf { get; set; }
        public decimal? FinalTotal { get; set; }
        public decimal? Profit { get; set; }

        public decimal? DepFull { get; set; }
        public decimal? DepHalf { get; set; }

        public decimal? TotalDep { get; set; }
        public decimal? ClosingWDV { get; set; }
        

        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set;}
        public int? Modified_Userid { get; set; }

        
        }
    

    /* Used to get values from stored proce */

    public class ITDepreciation_Temp
    {
        public int id { get; set; }
        public int  itgroupid { get; set; }
        public string groupname { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal DepreciationRate { get; set; }
        public decimal Opwdv { get; set; }
        public decimal	Additionbefore { get; set; }
        public decimal AdditionAfter { get; set; }
        public decimal  Disposal { get; set; }
        public decimal DeponOPwdv { get; set; }
        public decimal DepBefore { get; set; } // asseets purchase before 30sept
        public decimal DepAfter { get; set; } // purchase after 30 sept 
        public decimal  TotalDep { get; set; }
        public decimal  ClosingWDV { get; set; }

    }

}