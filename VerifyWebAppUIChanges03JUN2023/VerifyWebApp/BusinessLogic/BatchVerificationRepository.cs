using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.BusinessLogic
{
    public class BatchVerificationRepository
    {
        public VerifyDB db = new VerifyDB();
        public List<BatchVerificationViewModel> getVerifiedAssets(int companyid, int  BatchId)
        {
            string strSQL = null;
            string str_comid = companyid.ToString();
            string Batchid = BatchId.ToString();

            //strSQL = "";
            //strSQL = "Call getverifiedasset(";
            //strSQL = strSQL + companyid + ",";

            //strSQL = strSQL + Batchid + ")";


            strSQL = "";
            strSQL = "Call getverifiedasset_v1(";
            strSQL = strSQL + companyid + ",";

            strSQL = strSQL + Batchid + ")";



            var result = db.Database.SqlQuery<BatchVerificationViewModel>(strSQL).ToList();
            return result;

        }
        public List<BatchVerificationViewModel> getAssetsfound(int companyid, int BatchId)
        {
            string strSQL = null;
            string str_comid = companyid.ToString();
            string Batchid = BatchId.ToString();

            //strSQL = "";
            //strSQL = "Call get_assetfound(";
            //strSQL = strSQL + companyid + ",";

            //strSQL = strSQL + Batchid + ")";


            strSQL = "";
            strSQL = "Call get_assetfound_v1(";
            strSQL = strSQL + companyid + ",";

            strSQL = strSQL + Batchid + ")";



            var result = db.Database.SqlQuery<BatchVerificationViewModel>(strSQL).ToList();
            return result;

        }
        public List<BatchVerificationViewModel> getAssetsextrafound(int companyid, int BatchId)
        {
            string strSQL = null;
            string str_comid = companyid.ToString();
            string Batchid = BatchId.ToString();

            strSQL = "";
            strSQL = "Call get_assetextrafound_v1(";
            strSQL = strSQL + companyid + ",";

            strSQL = strSQL + Batchid + ")";


            var result = db.Database.SqlQuery<BatchVerificationViewModel>(strSQL).ToList();
            return result;

        }
        public List<BatchVerificationViewModel> getAssetsnotfound(int companyid, int BatchId)
        {
            string strSQL = null;
            string str_comid = companyid.ToString();
            string Batchid = BatchId.ToString();

            //strSQL = "";
            //strSQL = "Call get_assetnotfound(";
            //strSQL = strSQL + companyid + ",";

            //strSQL = strSQL + Batchid + ")";


            strSQL = "";
            strSQL = "Call get_assetnotfound_v1(";
            strSQL = strSQL + companyid + ",";

            strSQL = strSQL + Batchid + ")";


            var result = db.Database.SqlQuery<BatchVerificationViewModel>(strSQL).ToList();
            return result;

        }
    }
}