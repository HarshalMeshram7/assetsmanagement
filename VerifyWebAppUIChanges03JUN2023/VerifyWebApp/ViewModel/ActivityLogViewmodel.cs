using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.ViewModel
{
    public class ActivityLogViewmodel
    {

        public int UserId { get; set; }

        public string UserName { get; set; }
        public int EventId { get; set; }
        public int RecordType { get; set; }
        public DateTime TranDate { get; set; }
        public string Column { get; set; }
        public string Oldvalue { get; set; }
        public string Newvalue { get; set; }

    }
}