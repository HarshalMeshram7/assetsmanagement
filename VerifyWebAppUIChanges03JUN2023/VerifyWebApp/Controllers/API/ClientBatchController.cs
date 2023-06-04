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

namespace VerifyWebApp.Controllers.API
{
    public class ClientBatchController : ApiController
    {

        public VerifyDB db = new VerifyDB();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        [HttpGet]
        [Route("api/batch/listopen/{ClientCode}")]
        public APIResponse GetOpenBatchList(int ClientCode)
        {

            List<Batch> lstBatch = new List<Batch>();
            try
            {
                if (ClientCode > 0)
                {
                    // TODO Need to change this to Company Code 
                    // Mandar 13 NOV 2020

                        lstBatch = db.Batchs.Where(x => x.ClientID == ClientCode && x.IsBatchOpen.ToLower() == "yes").ToList();
                }

                var resp = new APIResponse();
                resp.status = "True";
                resp.data = JsonConvert.SerializeObject(lstBatch);

                return resp;

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);

                var resp = new APIResponse();
                resp.status = "False";
                resp.data = "ERROR";

                return resp;

            }

        }
    }
}
