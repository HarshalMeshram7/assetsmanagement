﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Models
{
    [Table("tblbgroup")]
    public class BGroup
    {
        [Key]
        public int ID { get; set; }
        public string BGroupName { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int AGrpID { get; set; }
        public decimal NormalRate { get; set; }
        public decimal AdditionalRate { get; set; }
        public decimal TotalRate { get; set; }
        public string DepMethod { get; set; }
        public int ClientID { get; set; }
        [NotMapped]
        public string BGroup_name { get; set; }
    }
}