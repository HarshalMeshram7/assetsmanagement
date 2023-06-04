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
    public class AssetAttachmentController : ApiController
    {

        public VerifyDB db = new VerifyDB();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        [HttpPost]
        [Route("api/asset/uploadsingleassetattachment")]
        public APIResponse UploadSingleAssetAttachment([FromBody] Child_Asset_Attachment calist)
        {

            APIResponse resp = new APIResponse();
            //List<Batch> lstBatch = new List<Batch>();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                 
                    db.Child_Asset_Attachments.Add(calist);
                    db.Entry(calist).State = EntityState.Added;
                    db.SaveChanges();
                    resp.status = "True";
                    resp.data = calist.AssetNumber.ToString();
                    transaction.Commit();

    
                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    logger.Log(LogLevel.Error, strError);

                   
                    resp.status = "False";
                    resp.data = "ERROR";

                    //return resp;

                
            }
            }
            return resp;
        }

        [HttpPost]
        [Route("api/asset/uploadassetattachment")]
        public APIResponse UploadAssetAttachment([FromBody] List<Child_Asset_Attachment> calist)
        {

            //List<Batch> lstBatch = new List<Batch>();
            db.Configuration.AutoDetectChangesEnabled = false;
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var resp = new APIResponse();

                   
                    db.Child_Asset_Attachments.AddRange(calist);
                    db.SaveChanges();
                    transaction.Commit();
                   
                    resp.status = "True";
                    resp.data = calist.Count.ToString();
                    db.Configuration.AutoDetectChangesEnabled = true;
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

        [HttpPost]
        [Route("api/asset/updateimages")]
        public APIResponse UpdateAssetID()
        {

            APIResponse resp = new APIResponse();

            //usp_UpdateImagesAssetID
            try
            {


               string strSQL = "";
               strSQL = "Call usp_UpdateImagesAssetID()";

                var returnValue = db.Database.ExecuteSqlCommand(strSQL);


                resp.status = "True";
                resp.data = "";
                return resp;

            }
            catch(Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);


                resp.status = "False";
                resp.data = "ERROR";

                return resp;
            }

        }

    }
}
