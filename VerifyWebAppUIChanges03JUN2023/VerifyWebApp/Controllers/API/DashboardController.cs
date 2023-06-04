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
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.Controllers.API
{
    public class DashboardController : ApiController
    {
        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [HttpGet]
        [Route("api/getGroupWiseAssets")]
        public APIResponse GetGroupWiseData(int companyid,int GroupLevel)
        {


            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {

                IList<DashBoardGroupWiseAssets> lstGroupWiseAssets = new List<DashBoardGroupWiseAssets>();



                string strSQL = "select BGroupID as id, BGroupName as groupname, Round(Sum(AmountCapitalisedCompany)) as amt from tblassets ";
                strSQL = strSQL + " Inner JOIN tblbgroup on tblbgroup.ID = tblAssets.BGroupID ";
                strSQL = strSQL + "  Group BY BGroupID,BGroupName ";

                lstGroupWiseAssets = db.Database.SqlQuery<DashBoardGroupWiseAssets>(strSQL).ToList();
               
                //foreach (var item in lstACostCenter)
                //{
                //    dynamic obj = new JObject();
                //    obj.id = item.ID;
                //    obj.code = item.CCCode;
                //    obj.description = item.CCDescription;
                //    lstOutput.Add(obj);
                //}

                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstGroupWiseAssets);
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
        [Route("api/getLocationpWiseAssets")]
        public APIResponse GetLocationWiseData(int companyid, int level)
        {


            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {

                IList<DashBoardGroupWiseAssets> lstLocationWiseAssets = new List<DashBoardGroupWiseAssets>();

                string strSQL = "";
                if (level == 1)
                {
                    strSQL = " select LocAID as id, ALocationName as groupname, Round(Sum(AmountCapitalisedCompany)) as amt from tblassets";
                    strSQL = strSQL + " Inner JOIN tblalocation on tblalocation.ID = tblAssets.LocAID ";
                    strSQL = strSQL + " Group BY LocAID,ALocationName";

                }
                else if (level == 2)
                {
                    strSQL = " select LocAID as id, ALocationName as groupname, Round(Sum(AmountCapitalisedCompany)) as amt from tblassets";
                    strSQL = strSQL + " Inner JOIN tblalocation on tblalocation.ID = tblAssets.LocAID ";
                    strSQL = strSQL + " Group BY LocAID,ALocationName";
                }

                else if (level == 3)
                {
                    strSQL = " select LocAID as id, ALocationName as groupname, Round(Sum(AmountCapitalisedCompany)) as amt from tblassets";
                    strSQL = strSQL + " Inner JOIN tblalocation on tblalocation.ID = tblAssets.LocAID ";
                    strSQL = strSQL + " Group BY LocAID,ALocationName";
                }



                lstLocationWiseAssets = db.Database.SqlQuery<DashBoardGroupWiseAssets>(strSQL).ToList();


                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstLocationWiseAssets);
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
        [Route("api/getAssetsTop10")]
        public APIResponse GetAssetsTop10(int companyid)
        {
            
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {

                IList<DashBoardAsset> lstLocationWiseAssets = new List<DashBoardAsset>();

                string strSQL = "";

                strSQL = "select tblassets.id,assetno,tblassets.AssetName,Round(AmountCapitalisedCompany) as amtcapitalised";
                strSQL = strSQL + " from tblassets ";
                strSQL = strSQL + " where AmountCapitalisedCompany > 0 ";
                strSQL = strSQL + " order by AmountCapitalisedCompany DESC limit 10";


                lstLocationWiseAssets = db.Database.SqlQuery<DashBoardAsset>(strSQL).ToList();



                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstLocationWiseAssets);
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
