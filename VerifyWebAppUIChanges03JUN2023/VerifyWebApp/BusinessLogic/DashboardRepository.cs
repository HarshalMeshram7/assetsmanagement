using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace VerifyWebApp.BusinessLogic
{
    public class DashboardRepository
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public DashboardRepository()
        {
            
        }
        public DataTable GetGroupWiseAssets(int CompanyID)
        {

            //select AGroupID, BGroupID, tblagroup.AGroupName,tblbgroup.BGroupName, SUM(AmountCapitalised) AmountCapitalised from tblassets
            //left join tblagroup on tblassets.AGroupID = tblagroup.ID
            //left join tblbgroup on tblassets.BGroupID = tblbgroup.ID
            //Group BY AGroupID,BGroupID

            //select AGroupID, BGroupID, tblagroup.AGroupName,tblbgroup.BGroupName ,SUM(DisposalAmount) from tbldisposal
            //inner join tblassets on tbldisposal.assetid = tblassets.id
            //left
            //join tblagroup on tblassets.AGroupID = tblagroup.ID
            //left
            //join tblbgroup on tblassets.BGroupID = tblbgroup.ID
            //Group BY AGroupID, BGroupID


            DataTable dtAssets = new DataTable();
            return dtAssets;
        }
    }
}