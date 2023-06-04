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

    [Table("tbldepreciation")]
    public class Depreciation
    {
        [Key]
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public int Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int AssetId { get; set; }
        public string AssetName { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public decimal Amount { get; set; }
        public string DepreciationType { get; set; } //Auto or Manual A or M

        public decimal NormalRate { get; set; }
        public decimal AdditionRate { get; set; }
        public decimal TotalRate { get; set; }
        public int DepreciationDays { get; set; }
        public string DepreciationMethod { get; set; } //SLM or WDV
        public int ClientID { get; set; }

        [NotMapped]
        public string AssetNo { get; set; }///  /// from asset id get assetnos
        [NotMapped]
        public int int_Srno { get; set; }
        [NotMapped]
        public string str_FromDate { get; set; }
        [NotMapped]
        public string str_ToDate { get; set; }
    }
}