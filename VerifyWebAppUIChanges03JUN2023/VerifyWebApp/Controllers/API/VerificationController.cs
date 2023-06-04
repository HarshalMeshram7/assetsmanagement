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
namespace VerifyWebApp.APIControllers
{
    public class VerificationController : ApiController
    {
        public VerifyDB db = new VerifyDB();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpPost]
        [Route("api/verification/uploadsingle")]
        public APIResponse SaveSingleAsset([FromBody] BatchVerification asset)
        {

            // validate client code

            //Client client = null;
            //client = db.Clients.Where(x => x.ClientCode == asset.ClientCode).FirstOrDefault();
            //if (client == null)
            //{
            //    var respNF = new APIResponse();
            //    respNF.status = "False";
            //    respNF.data = "Client Code not found";
            //    return respNF;
            //}

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    //List<ClientBatch> lstBatches;
                    //lstBatches = db.Batches.Where(x => x.ClientCode == asset.ClientCode).ToList();

                    //ClientBatch batch = lstBatches.Where(x => asset.LastUpdateTimeStamp >= x.StartDate).FirstOrDefault();
                    //asset.BatchID = batch.BatchID;

                    // 20 May 2019 - No Need to set batch id here .. batch id coming from App



                    BatchVerification verification;

                    //  verification = db.BatchVerification.Where(x => x.ClientCode == client.ClientCode && x.BatchID == asset.BatchID && x.AssetNumber == asset.AssetNumber && x.AssetIndex == asset.AssetIndex).FirstOrDefault();
                    verification = db.BatchVerification.Where(x => x.BatchID == asset.BatchID && x.AssetNumber == asset.AssetNumber && x.AssetIndex == asset.AssetIndex).FirstOrDefault();
                    if (verification != null)
                    {
                        var resp = new APIResponse();
                        resp.status = "True";
                        resp.data = "Batch Uploaded";
                        return resp;

                    }
                    else
                    {
                        db.BatchVerification.Add(asset);
                        db.Entry(asset).State = EntityState.Added;
                        db.SaveChanges();
                        transaction.Commit();
                        var resp = new APIResponse();
                        resp.status = "True";
                        resp.data = "Batch Uploaded";
                        return resp;

                    }


                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    logger.Log(LogLevel.Error, strError);
                    var resp = new APIResponse();
                    resp.status = "False";
                    resp.data = "Error";
                    return resp;
                }

            }
        }

        [HttpPost]
        [Route("api/verification/upload")]
        public APIResponse SaveVerified([FromBody] List<BatchVerification> list)
        {

            //var list = JsonConvert.DeserializeObject<List<BatchVerification>>(strJSON);
            db.Configuration.AutoDetectChangesEnabled = false;

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {


                try
                {

                    // get the first from the list and validate 

                    BatchVerification tempVerification = null;

                    tempVerification = list.FirstOrDefault();

                    if (tempVerification != null)
                    {

                        // validate client code

                        //Client client = null;
                        //client = db.Clients.Where(x => x.ClientCode == tempVerification.ClientCode).FirstOrDefault();
                        //if (client == null)
                        //{
                        //    var respNF = new APIResponse();
                        //    respNF.status = "False";
                        //    respNF.data = "Client Code not found";
                        //    return respNF;
                        //}

                        string strSQL = "";

                        foreach (BatchVerification item in list)
                        {

                            if (item.BatchID > 0)
                            {
                                //strSQL = "DELETE FROM tblBatchVerification WHERE ClientCode = " + item.ClientCode;
                                //strSQL = strSQL + " AND BatchID = " + item.BatchID;
                                //strSQL = strSQL + " AND AssetNumber = " + item.AssetNumber;
                                //strSQL = strSQL + " AND AssetIndex = " + item.AssetIndex;
                                strSQL = "DELETE FROM tblBatchVerification WHERE ";
                                strSQL = strSQL + "BatchID = " + item.BatchID;
                                strSQL = strSQL + " AND AssetNumber = '" + item.AssetNumber + "'";
                                strSQL = strSQL + " AND AssetIndex = " + item.AssetIndex;

                                db.Database.ExecuteSqlCommand(strSQL);

                            }


                        }


                        foreach (BatchVerification item in list)
                        {
                            item.Longitude = Convert.ToDouble(item.Longitude);
                            item.Latitude = Convert.ToDouble(item.Latitude);

                        }
                        db.BatchVerification.AddRange(list);

                        db.SaveChanges();
                        transaction.Commit();
                        var resp = new APIResponse();
                        resp.status = "True";
                        resp.data = list.Count.ToString();
                        db.Configuration.AutoDetectChangesEnabled = true;
                        return resp;

                    }
                    else
                    {
                        var resp = new APIResponse();
                        resp.status = "False";
                        resp.data = "Not Found";
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    logger.Log(LogLevel.Error, strError);
                    var resp = new APIResponse();
                    resp.status = "False";
                    resp.data = "Error";
                    db.Configuration.AutoDetectChangesEnabled = true;
                    return resp;
                }
            }

        }

        [HttpGet]
        [Route("api/verification/getlist")]
        public List<BatchVerification> GetAssetList(int ClientCode, int BatchID)
        {

            List<BatchVerification> lstAssetList = new List<BatchVerification>();
            try
            {
                lstAssetList = db.BatchVerification.Where(x => x.ClientCode == ClientCode && x.BatchID == BatchID).ToList();
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);
            }

            return lstAssetList;
        }



    }
}
