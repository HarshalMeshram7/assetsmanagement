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
    public class GroupsController : ApiController
    {
        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getgroups")]
        public APIResponse GetCLGroups(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<AGroup> lstAGroup = new List<AGroup>();
                List<BGroup> lstBGroup = new List<BGroup>();
                List<CGroup> lstCGroup = new List<CGroup>();
                List<DGroup> lstDGroup = new List<DGroup>();


                lstAGroup = db.AGroups.Where(x => x.Companyid == companyid).ToList();

                lstBGroup = db.BGroups.Where(x => x.Companyid == companyid).ToList();

                lstCGroup = db.CGroups.Where(x => x.Companyid == companyid).ToList();

                lstDGroup = db.DGroups.Where(x => x.Companyid == companyid).ToList();

                List<JObject> lstOutputAGroup = new List<JObject>();
                List<JObject> lstOutputBGroup = new List<JObject>();
                List<JObject> lstOutputCGroup = new List<JObject>();
                List<JObject> lstOutputDGroup = new List<JObject>();





                foreach (var item in lstAGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.groupname = item.AGroupName;
                    lstOutputAGroup.Add(obj);
                }


                foreach (var item in lstBGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.groupname = item.BGroupName;
                    obj.agroupid = item.AGrpID;
                    lstOutputBGroup.Add(obj);
                }


                foreach (var item in lstCGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.groupname = item.CGroupName;
                    obj.agroupid = item.AGrpID;
                    obj.bgroupid = item.BGrpID;

                    lstCGroup.Add(obj);
                }


                foreach (var item in lstDGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.groupname = item.DGroupName;
                    obj.agroupid = item.AGrpID;
                    obj.bgroupid = item.BGrpID;
                    obj.cgroupid = item.CGrpID;

                    lstDGroup.Add(obj);
                }


                dynamic objRoot = new JObject();

                objRoot.AGroup = lstOutputAGroup;
                objRoot.BGroup = lstOutputBGroup;
                objRoot.CGroup = lstOutputCGroup;
                objRoot.DGroup = lstOutputDGroup;

                lstOutput.Add(objRoot);

                resp.status = "true";


                // string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstOutput);
                //resp.data = jsonstring;

            }
            catch (Exception ex)
            {
                resp.status = "False";
                resp.data = "ERROR";
            }
            return resp;
        }

        [HttpGet]
        [Route("api/getAgroups")]
        public APIResponse GetAGroup(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<AGroup> lstAGroup = new List<AGroup>();

                lstAGroup = db.AGroups.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstAGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.AGroupName = item.AGroupName;
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
        [Route("api/getBgroups")]
        public APIResponse GetBGroup(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<BGroup> lstBGroup = new List<BGroup>();

                lstBGroup = db.BGroups.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstBGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.BGroupName = item.BGroupName;
                    obj.AGrpID = item.AGrpID;
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
        [Route("api/getCgroups")]
        public APIResponse GetCGroup(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<CGroup> lstCGroup = new List<CGroup>();

                lstCGroup = db.CGroups.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstCGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.CGroupName = item.CGroupName;
                    obj.AGrpID = item.AGrpID;
                    obj.BGrpID = item.BGrpID;

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
        [Route("api/getDgroups")]
        public APIResponse GetDGroup(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<DGroup> lstDGroup = new List<DGroup>();

                lstDGroup = db.DGroups.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstDGroup)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.DGroupName = item.DGroupName;
                    obj.AGrpID = item.AGrpID;
                    obj.BGrpID = item.BGrpID;
                    obj.CGrpID = item.CGrpID;

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