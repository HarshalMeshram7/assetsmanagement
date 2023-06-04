using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyDepCalculator
{
    public class ITGroupDep
    {

        public int itgroupid { get; set; }
        public string groupname { get; set; }
        public decimal OpWDV { get; set; }
        public decimal DepRate { get; set; }
        public decimal Additions_LessThan180 { get; set; }
        public decimal Additions_MoreThan180 { get; set; }
        public decimal Total_Full { get; set; }
        public decimal Total_Half { get; set; }

        public decimal Disposal_LessThan180 { get; set; }
        public decimal Disposal_MoreThan180 { get; set; }

        public decimal Final_Total_Full{ get; set; }
        public decimal Final_Total_Half { get; set; }

        public decimal Final_Total  { get; set; }
        public decimal Profit { get; set; }

        public decimal Dep_Full { get; set; }
        public decimal Dep_Half { get; set; }

        public decimal Total_Depreciation { get; set; }
        public decimal Closing_WDV { get; set; }


    }
}
