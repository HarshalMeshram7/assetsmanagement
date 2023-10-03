using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;

namespace VerifyWebApp.ViewModel
{
    public class BatchViewmodel
    {

        public BatchViewmodel()
        {
            this.locationlist = new List<Subbatch>();
            // this.BatchViewModellist = new List<SubbatchTable>();

            this.costcenterlist = new List<Subbatch>();

            this.rangelist = new List<Subbatch>();
        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string BatchDescription { get; set; }
        public string IsBatchOpen { get; set; }
        public int ClientID { get; set; }
        public int ID { get; set; }
        public string str_FromDate { get; set; }
        public string str_ToDate { get; set; }

        public IEnumerable<Subbatch> locationlist { get; set; }
        // public IEnumerable<SubbatchTable> BatchViewModellist { get; set; }

        public IEnumerable<Subbatch> costcenterlist { get; set; }

        public IEnumerable<Subbatch> rangelist { get; set; }
    }

    //public class SubbatchTable
    //{
    //    public SubbatchTable()
    //    {

    //    }

    //    public int LocAId { get; set; }
    //    public int LocBId { get; set; }
    //    public int LocCId { get; set; }
    //    public string LocAName { get; set; }
    //    public string LocBName { get; set; }
    //    public string LocCName { get; set; }
    //    public int BatchId { get; set; }

    //}
}