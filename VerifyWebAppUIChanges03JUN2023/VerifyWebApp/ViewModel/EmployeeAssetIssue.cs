using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class EmployeeAssetIssue
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int AssetNo { get; set; }
        public string AssetName { get; set; }
        public DateTime IssueDate { get; set; }
        public string AssetReturn { get; set; }
        public DateTime AssetReturnDate { get; set; }

    }
}