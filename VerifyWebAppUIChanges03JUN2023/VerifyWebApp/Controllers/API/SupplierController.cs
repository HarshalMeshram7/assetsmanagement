using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VerifyWebApp.Models;
using System.Transactions;
using Newtonsoft.Json;
using NLog;
using Newtonsoft.Json.Linq;

namespace VerifyWebApp.Controllers.API
{
    public class SupplierController : ApiController
    {

        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getsupplier")]
        public APIResponse GetSupplier(int companyid)
        {
            APIResponse resp = new APIResponse();
            try
            {
                List<Supplier> lstSupplier = new List<Supplier>();
                List<JObject> lstOutput = new List<JObject>();
                
                lstSupplier = db.Suppliers.Where(x=>x.Companyid == companyid).ToList();
                foreach (var item in lstSupplier)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.code= item.SupplierCode;
                    obj.suppliername = item.SupplierName;

                    lstOutput.Add(obj);

                }
                resp.status = "true";
                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstOutput);
                resp.data = jsonstring;

            }
            catch(Exception ex)
            {
                resp.status = "False";
                resp.data = "ERROR";
            }
            return resp;
        }
    }
}