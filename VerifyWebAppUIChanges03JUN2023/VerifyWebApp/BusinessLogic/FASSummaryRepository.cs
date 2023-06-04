using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.BusinessLogic
{
    public class FASSummaryRepository
    {
        public VerifyDB db = new VerifyDB();
        public List<FASSummaryViewmodel> getFASSummary(int companyid, DateTime FromDate, DateTime ToDate)
        {
            //List<FASSummaryViewmodel> clist = new List<FASSummaryViewmodel>();
            //// FARReportViewmodel cobj = new FARReportViewmodel();
            //List<AGroup> alist = new List<AGroup>();
            //alist = db.AGroups.Where(x => x.Companyid == companyid).ToList();
            //foreach (AGroup item in alist)
            //{

            //    var Calculatedvalue = GetCalculatedvalues(item.ID, FromDate, ToDate, companyid);
            //    Calculatedvalue.AGroupName = item.AGroupName;
            //    clist.Add(Calculatedvalue);


            //}//call usp_report_getFASSummary('1','2021-03-01','2021-03-31')
            string strSQL = null;
            string str_comid = companyid.ToString();
            string fromDate = FromDate.ToString("yyyy-MM-dd");
            string toDate = ToDate.ToString("yyyy-MM-dd");
            strSQL = "Call usp_report_getFASSummaryV1_New( " + companyid + ",'" + fromDate + "','" + toDate + "')";
            var result = db.Database.SqlQuery<FASSummaryViewmodel>(strSQL).ToList();
            // return clist;
            return result;
        }


        private List<FASSummaryDetailViewmodel> GetProcResult(int companyid, DateTime FromDate, DateTime ToDate,
                     int AGroupID, int BGroupID, int CGroupID, int DGroupID)
        {

            try
            {



                string strSQL = null;
                string str_comid = companyid.ToString();
                string fromDate = FromDate.ToString("yyyy-MM-dd");
                string toDate = ToDate.ToString("yyyy-MM-dd");

                strSQL = "";
                strSQL = "Call usp_report_getFASDetail(";
                strSQL = strSQL + companyid + ",";
                strSQL = strSQL + "'" + fromDate + "',";
                strSQL = strSQL + "'" + toDate + "',";
                strSQL = strSQL + AGroupID + ",";
                strSQL = strSQL + BGroupID + ",";
                strSQL = strSQL + CGroupID + ",";
                strSQL = strSQL + DGroupID + ")";

                db.Database.CommandTimeout = 180;

                var result = db.Database.SqlQuery<FASSummaryDetailViewmodel>(strSQL).ToList();
                return result;
            }
            catch(Exception ex)
            {
                // TODO log this error on nlog

                List<FASSummaryDetailViewmodel> result = new List<FASSummaryDetailViewmodel>();
                return result;
            }

            

        }
        public List<FASSummaryDetailViewmodel> getFASSummaryDetail(int companyid, DateTime FromDate, DateTime ToDate)
        {


           

            

            List<FASSummaryDetailViewmodel> lstMain = new List<FASSummaryDetailViewmodel>();

            List<AGroup> lstAGroup = db.AGroups.Where(x => x.Companyid == companyid).ToList();


            foreach (var item_AGroup in lstAGroup)
            {

                var result_AGroup = GetProcResult(companyid, FromDate, ToDate, item_AGroup.ID, 0, 0, 0);

                lstMain.AddRange(result_AGroup);

                List<BGroup> lstBGroup = db.BGroups.Where(x => x.Companyid == companyid
                        && x.AGrpID == item_AGroup.ID).ToList();

                foreach (var item_BGroup in lstBGroup)
                {
                    var result_BGroup = GetProcResult(companyid, FromDate, ToDate,
                        item_AGroup.ID, item_BGroup.ID, 0, 0);


                    lstMain.AddRange(result_BGroup);


                    List<CGroup> lstCGroup = db.CGroups.Where(x => x.Companyid == companyid
                        && x.AGrpID == item_AGroup.ID && x.BGrpID == item_BGroup.ID).ToList();


                    foreach (var item_CGroup in lstCGroup)
                    {

                        var result_CGroup = GetProcResult(companyid, FromDate, ToDate,
                            item_AGroup.ID, item_BGroup.ID, item_CGroup.ID, 0);


                        lstMain.AddRange(result_CGroup);

                        List<DGroup> lstDGroup = db.DGroups.Where(x => x.Companyid == companyid
                            && x.AGrpID == item_AGroup.ID &&
                            x.BGrpID == item_BGroup.ID &&
                            x.CGrpID == item_CGroup.ID
                            ).ToList();

                        foreach (var item_DGroup in lstDGroup)
                        {
                            var result_DGroup = GetProcResult(companyid, FromDate, ToDate,
                            item_AGroup.ID, item_BGroup.ID, item_CGroup.ID, item_DGroup.ID);

                            lstMain.AddRange(result_DGroup);
                        }

                    }

                }

            }
            foreach (var item in lstMain)
            {

                if (item.agroupid != 0 )
                {
                    item.AGroupName = db.AGroups.Where(x => x.Companyid == companyid && x.ID == item.agroupid).FirstOrDefault().AGroupName;
                }
                if (item.bgroupid != 0)
                {
                    item.BGroupName = db.BGroups.Where(x => x.Companyid == companyid && x.ID == item.bgroupid).FirstOrDefault().BGroupName;
                }
                if (item.cgroupid != 0)
                {
                    item.CGroupName = db.CGroups.Where(x => x.Companyid == companyid && x.ID == item.cgroupid).FirstOrDefault().CGroupName;
                }
                if (item.dgroupid != 0)
                {
                    item.DGroupName = db.DGroups.Where(x => x.Companyid == companyid && x.ID == item.dgroupid).FirstOrDefault().DGroupName;
                }
            }  

            return lstMain;
        }


    }





}