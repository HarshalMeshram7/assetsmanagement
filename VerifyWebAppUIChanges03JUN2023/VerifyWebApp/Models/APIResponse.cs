using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace VerifyWebApp.Models
{
    public class APIResponse
    {
        public string status { get; set; }
        public string data { get; set; }
        public string error { get; set; }
    }
}