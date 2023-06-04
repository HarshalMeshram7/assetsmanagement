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
    public class AccountsController : ApiController
    {

        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getaccounts")]
        public APIResponse GetAccunts(int companyid)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<Account> lstAccounts= new List<Account>();


                lstAccounts = db.Accounts.Where(x => x.Companyid == companyid).ToList();

                foreach (var item in lstAccounts)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.accountcode= item.AccountCode;
                    obj.accountname = item.AccountName;
                    obj.group = item.GroupName;
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