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
    public class CostCenterController : ApiController
    {
        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getcostcenter")]
        public APIResponse GetCostCenter(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<ACostCenter> lstACostCenter = new List<ACostCenter>();


                lstACostCenter = db.ACostCenters.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstACostCenter)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.code = item.CCCode;
                    obj.description  = item.CCDescription;
                    lstOutput.Add(obj);
                }

                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstOutput);
                resp.status = "true";
                resp.data = jsonstring;

            }
            catch (Exception ex)
            {
                resp.status = "False";
                resp.data = "ERROR";
            }
            return resp;
        }

        [HttpGet]
        [Route("api/getsubcostcenter")]
        public APIResponse GetSubCostCenter(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<BCostCenter> lstBCostCenter = new List<BCostCenter>();


                lstBCostCenter = db.BCostCenters.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstBCostCenter)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.code = item.SCCCode;
                    obj.description = item.SCCDescription;
                    obj.parentid = item.CCID;
                    lstOutput.Add(obj);
                }

                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstOutput);
                resp.status = "true";
                resp.data = jsonstring;

            }
            catch (Exception ex)
            {
                resp.status = "False";
                resp.data = "ERROR";
            }
            return resp;
        }
    }
}