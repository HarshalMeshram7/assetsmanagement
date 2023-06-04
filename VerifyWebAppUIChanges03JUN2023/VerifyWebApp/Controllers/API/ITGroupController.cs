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
    public class ITGroupController : ApiController
    {
        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getITgroups")]
        public APIResponse GetITGroup(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<ITGroup> lstITGroup = new List<ITGroup>();

                lstITGroup = db.ITGroups.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstITGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.GroupName = item.GroupName;
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