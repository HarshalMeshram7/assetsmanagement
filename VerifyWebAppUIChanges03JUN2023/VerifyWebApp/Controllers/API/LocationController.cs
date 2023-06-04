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
    public class LocationController : ApiController
    {
        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getALocation")]
        public APIResponse GetALocation(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<ALocation> lstALocationList = new List<ALocation>();

                lstALocationList = db.ALocations.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstALocationList)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.ALocationName = item.ALocationName;

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
        [Route("api/getBLocation")]
        public APIResponse GetBLocation(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<BLocation> lstBLocationList = new List<BLocation>();

                lstBLocationList = db.BLocations.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstBLocationList)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.BLocationName = item.BLocationName;
                    obj.ALocID = item.ALocID;

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
        [Route("api/getCLocation")]
        public APIResponse GetCLocation(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<CLocation> lstCLocationList = new List<CLocation>();

                lstCLocationList = db.CLocations.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstCLocationList)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.CLocationName = item.CLocationName;
                    obj.ALocID = item.ALocID;
                    obj.BLocID = item.BLocID;

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