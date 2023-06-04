using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VerifyWebApp.Models;
using System.Transactions;
using Newtonsoft.Json;
using NLog;
using Newtonsoft.Json.Linq;


namespace VerifyWebApp.Controllers.API
{
    public class AssetController : ApiController
    {
        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getAsset")]
        public APIResponse GetAsset(int companyid,string AssetNo)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<Assets> lstAssets = new List<Assets>();



                lstAssets = db.Assetss.Where(x => x.Companyid == companyid &&  (x.AssetNo.Contains(AssetNo) || x.AssetName.ToLower().Contains(AssetNo))).Take(25).ToList();

                foreach (var item in lstAssets)
                {
                        dynamic obj = new JObject();
                        obj.id = item.ID;
                        obj.assetno= item.AssetNo;
                        obj.assetname = item.AssetName;
                        obj.assetnoname = item.AssetNo + " - " + item.AssetName ;
                        obj.amtcapitalized = item.AmountCapitalisedCompany;
                        obj.dtputtouse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
                        obj.serialno = item.SrNo;
                        obj.depmethod = item.DepreciationMethod;

                        int ALocID = item.LocAID ?? 0;
                        int BLocID = item.LocBID ?? 0;
                        int CLocID = item.LocCID ?? 0;

                        string CLoc_Name = "";
                        string BLoc_Name = "";
                        string ALoc_Name = "";
                        string location = "";

                        if (CLocID > 0)
                        {
                            CLocation clocation = db.CLocations.Where(x => x.ID == CLocID).FirstOrDefault();
                            if (clocation != null)
                            {
                                CLoc_Name = clocation.CLocationName;
                            }
                        }

                        if (BLocID > 0)
                        {
                            BLocation blocation = db.BLocations.Where(x => x.ID == BLocID).FirstOrDefault();
                            if (blocation != null)
                            {
                                BLoc_Name = blocation.BLocationName;
                            }
                        }

                        if (ALocID > 0)
                        {
                            ALocation alocation = db.ALocations.Where(x => x.ID == ALocID).FirstOrDefault();
                            if (alocation != null)
                            {
                                ALoc_Name = alocation.ALocationName;
                            }
                        }

                    if (ALoc_Name.Length > 0)
                    {
                        location =  ALoc_Name;
                    }
                    if (BLoc_Name.Length > 0)
                    {
                        location = location + "-" + BLoc_Name;
                    }

                    if (CLoc_Name.Length > 0)
                    {
                        location = location + "-" + CLoc_Name;
                    }


                        obj.location = location;

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
        [Route("api/searchAsset")]
        public APIResponse SearchAsset(int companyid, string searchString)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {

                List<Assets> lstAssets = new List<Assets>();

                
                lstAssets = db.Assetss.Where(x => x.Companyid == companyid &&
                (x.AssetNo.Contains(searchString) || x.AssetName.ToLower().Contains(searchString)
                    || x.AssetIdentificationNo.ToLower().Contains(searchString)
                    || x.SrNo.ToLower().Contains(searchString)
                )).ToList();

                foreach (var item in lstAssets)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.assetno = item.AssetNo;
                    obj.assetname = item.AssetName;
                    obj.amtcapitalized = item.AmountCapitalisedCompany;
                    obj.dtputtouse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
                    obj.serialno = item.SrNo;
                    obj.AssetIdentificationNo = item.AssetIdentificationNo;
                    obj.depmethod = item.DepreciationMethod;

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


        [HttpPost]
        [Route("api/updateAsset")]
        public APIResponse UpdateAsset([FromBody] Assets asset)
        {
            APIResponse resp = new APIResponse();
            try
            {
                if (asset != null)
                {

                    int AGroupID = asset.AGroupID ??  0;
                    int BGroupID = asset.BGroupID ?? 0;
                    int CGroupID = asset.CGroupID ?? 0;



                }

                return resp;
            }catch(Exception ex)
            {
                resp.status = "False";
                resp.data = "ERROR";
                return resp;
            }
        }

        [HttpGet]
        [Route("api/getlastassetno")]
        public APIResponse GetLastAssetNo(int companyid)
        {
            APIResponse response = new APIResponse() ;

            try
            {

                List<Assets> lstassets = db.Assetss.OrderByDescending(x => x.ID).Take(1).ToList();

                Assets assets = lstassets[0];
                response.status = "True";

                dynamic obj = new JObject();

                obj.assetno = assets.AssetNo;
                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                response.data = jsonstring;

            }
            catch(Exception ex)
            {
                response.status = "False";
                response.data = "";

            }

            return response;
        }

    }

}
