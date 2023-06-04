using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using NLog;


namespace VerifyDepCalculator
{
   public class IncomeTaxLawDepCalculator
    {
        private string ConnectionString { get; set; }
        private MySqlConnection connection = new MySqlConnection();

        public NLog.Logger logger { get; set; }

        
        public IncomeTaxLawDepCalculator(string _ConnectionString)
        {
            this.ConnectionString = _ConnectionString;
        }

        public bool StartCalculation()
        {
            bool bResult = false;
            try
            {
                logger.Log(LogLevel.Info, "Opening Connection to DB...");

                connection.ConnectionString = this.ConnectionString;
                connection.Open();

                 CheckRequest();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }


                bResult = true;
                return bResult;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                bResult = false;
                return bResult;
            }

        }

        private void CheckRequest()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                string strSQL = "SELECT * FROM tbldepreciationrequest_incometax where InProcess = -1";
                cmd.Connection = connection;
                cmd.CommandText = strSQL;
                MySqlDataAdapter adapater = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapater.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {

                        DateTime dtStartDate;
                        DateTime dtEndDate;

                        dtStartDate = Convert.ToDateTime(dt.Rows[0]["StartDate"].ToString());
                        dtEndDate = Convert.ToDateTime(dt.Rows[0]["EndDate"].ToString());

                        StartITDepCalculation(dtStartDate, dtEndDate);
                        //MessageBox.Show("Calculation Complete");
                    }
                }

            }
            catch (Exception ex)
            {
                // logger.Error(ex);
                //logger.Error(ex.InnerException);
            }
        }

        public void StartITDepCalculation(DateTime startDate, DateTime enddate)
        {

            MySqlTransaction trann = connection.BeginTransaction();

            try
            {
                // DateTime fromDate = new DateTime(2021, 04, 1);
                //DateTime toDate = new DateTime(2022, 03, 31);

                DateTime fromDate = startDate;
                DateTime toDate = enddate;


                DateTime Days_180Date = fromDate.AddDays(181);
                int ClientID = 1;
                int CompanyID = 1;

                // check if table has any records in working table tblitdepreciation ;

                string strSQL = "select count(*) from  tblitdepreciation";

                DataTable dtDep = getDataTable(strSQL);
                // get addtions less that 180 days
                string temp_date_180 = Convert.ToDateTime(Days_180Date).ToString("yyyy-MM-dd");
                string str_fromdate = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd");
                string str_todate = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");


                //fetch opening from table 
                string strSQLOpening = "SELECT ID, GroupName, IFNULL(DepRate,0) DepRate,IFNULL(OPWDV,0) OPWDV FROM tblitgroup";
                DataTable dtDepOpening = getDataTable(strSQLOpening);

                Dictionary<int, ITGroupDep> dictDep = new Dictionary<int, ITGroupDep>();

                //List<ITGroupDep> lstDepreciation = new List<ITGroupDep>();

                if (dtDep.Rows[0][0].ToString() == "0")
                {
                    foreach (DataRow item in dtDepOpening.Rows)
                    {

                        ITGroupDep tempDep = new ITGroupDep();
                        tempDep.itgroupid = Convert.ToInt32(item["ID"].ToString());
                        tempDep.groupname = item["GroupName"].ToString();
                        tempDep.OpWDV = Convert.ToDecimal(item["OPWDV"].ToString());
                        tempDep.DepRate = Convert.ToDecimal(item["DepRate"].ToString());

                        dictDep.Add(tempDep.itgroupid, tempDep);

                    }

                }
                else
                {
                    // calculate opening for the period

                    foreach (DataRow item in dtDepOpening.Rows)
                    {

                        ITGroupDep tempDep = new ITGroupDep();
                        tempDep.itgroupid = Convert.ToInt32(item["ID"].ToString());
                        tempDep.groupname = item["GroupName"].ToString();
                        tempDep.DepRate = Convert.ToDecimal(item["DepRate"].ToString());


                        // 02-MAR-2023 fetch last year closing as this year opening 

                        StringBuilder sbClosing = new StringBuilder();

                        sbClosing.Append(" select ClosingWDV from tblitdepreciation ");
                        sbClosing.Append(" where FromDate < '" + str_fromdate + "'");
                        sbClosing.Append(" and ITGrpID =").Append(tempDep.itgroupid);
                        sbClosing.Append(" Order by ID DESC LIMIT 1");

                        DataTable dtDepOpeningNew = getDataTable(sbClosing.ToString());

                        if (dtDepOpeningNew.Rows.Count > 0)
                        {
                            tempDep.OpWDV = Convert.ToDecimal(dtDepOpeningNew.Rows[0]["ClosingWDV"].ToString());
                        }


                        dictDep.Add(tempDep.itgroupid, tempDep);


                    }


                }


             
                // check any additions 
                string strlSQLAdditionsLess180Days = "select ITGroupIDID,sum(IFNULL(AmountCApitalisedIT,0)) as Additions_LessThan180_Amount from tblassets where DtPutToUseIT >= '" + str_fromdate + "'" + "and  DtPutToUseIT < '" + temp_date_180 + "' group by ITGroupIDID ";
                DataTable dtAdditions_LessThan180 = getDataTable(strlSQLAdditionsLess180Days);


                foreach (DataRow item in dtAdditions_LessThan180.Rows)
                {
                    int groupid = Convert.ToInt32(item["ITGroupIDID"].ToString());

                    if (dictDep.ContainsKey(groupid))
                    {
                        ITGroupDep tempDep = dictDep[groupid];
                        tempDep.Additions_LessThan180 = Convert.ToDecimal(item["Additions_LessThan180_Amount"].ToString());

                        dictDep[groupid] = tempDep;
                    }
                }

                // check any additions 
                string strlSQLAdditionsMore180Days = "select ITGroupIDID,sum(IFNULL(AmountCApitalisedIT,0)) as Additions_MoreThan180 from tblassets where DtPutToUseIT >= '" + str_fromdate + "'" + " and  DtPutToUseIT > '" + temp_date_180 + "' and DtPutToUseIT <= '" + str_todate   + "'" +  " group by ITGroupIDID";
                DataTable dtAdditions_MoreThan180 = getDataTable(strlSQLAdditionsMore180Days);

                foreach (DataRow item in dtAdditions_MoreThan180.Rows)
                {
                    int groupid = Convert.ToInt32(item["ITGroupIDID"].ToString());

                    if (dictDep.ContainsKey(groupid))
                    {
                        ITGroupDep tempDep = dictDep[groupid];
                        tempDep.Additions_MoreThan180 = Convert.ToDecimal(item["Additions_MoreThan180"].ToString());

                        dictDep[groupid] = tempDep;
                    }
                }


                StringBuilder strDisposal_lesstha180 = new StringBuilder();

                strDisposal_lesstha180.Append("SELECT assets.id,assets.ITGroupIDID GroupID ,sum(ifnull(SaleValue, 0)) SaleValue");
                strDisposal_lesstha180.Append(" FROM tbldisposal disp ");
                strDisposal_lesstha180.Append(" inner join tblassets assets on assets.id = disp.AssetId ");
                strDisposal_lesstha180.Append(" where disp.DisposalDate >= '" + str_fromdate + "'");
                strDisposal_lesstha180.Append(" and disp.DisposalDate < ' " + temp_date_180 + "'");
                strDisposal_lesstha180.Append(" and assets.ITGroupIDID is not null ");
                strDisposal_lesstha180.Append(" group by assets.id,assets.ITGroupIDID ");

                DataTable dtDisposal_LessThan180 = getDataTable(strDisposal_lesstha180.ToString());

                foreach (DataRow item in dtDisposal_LessThan180.Rows)
                {
                    int groupid = Convert.ToInt32(item["GroupID"].ToString());

                    if (dictDep.ContainsKey(groupid))
                    {
                        ITGroupDep tempDep = dictDep[groupid];
                        // sale value before 180
                        tempDep.Disposal_LessThan180 = Convert.ToDecimal(item["SaleValue"].ToString());

                        dictDep[groupid] = tempDep;
                    }
                }


                // 


                StringBuilder strDisposal_morethan180 = new StringBuilder();

                strDisposal_morethan180.Append("SELECT assets.id,assets.ITGroupIDID GroupID ,sum(ifnull(SaleValue, 0)) SaleValue");
                strDisposal_morethan180.Append(" FROM tbldisposal disp ");
                strDisposal_morethan180.Append(" inner join tblassets assets on assets.id = disp.AssetId ");
                strDisposal_morethan180.Append(" where DisposalDate >= '" + temp_date_180 + "'");
                strDisposal_morethan180.Append(" and disp.DisposalDate < ' " + str_todate + "'");
                strDisposal_morethan180.Append(" and assets.ITGroupIDID is not null ");
                strDisposal_morethan180.Append(" group by assets.id,assets.ITGroupIDID ");

                DataTable dtDisposal_MoreThan180 = getDataTable(strDisposal_morethan180.ToString());

                foreach (DataRow item in dtDisposal_MoreThan180.Rows)
                {
                    int groupid = Convert.ToInt32(item["GroupID"].ToString());

                    if (dictDep.ContainsKey(groupid))
                    {
                        ITGroupDep tempDep = dictDep[groupid];
                        // sale value more than 180
                        tempDep.Disposal_MoreThan180 = Convert.ToDecimal(item["SaleValue"].ToString());

                        dictDep[groupid] = tempDep;
                    }
                }


                foreach (var item in dictDep.Values)
                {

                    int group_id = item.itgroupid;

                    item.Total_Full = item.OpWDV + item.Additions_LessThan180;
                    item.Total_Half = item.Additions_MoreThan180;

                    item.Final_Total_Full = item.Total_Full - item.Disposal_LessThan180;
                    item.Final_Total_Half = item.Total_Half - item.Disposal_MoreThan180;

                    item.Final_Total = item.Final_Total_Full + item.Final_Total_Half;

                    item.Dep_Full = (item.Final_Total_Full * item.DepRate) / 100;
                    item.Dep_Half = ((item.Final_Total_Half * item.DepRate) / 100) / 2;

                    item.Total_Depreciation = item.Dep_Full + item.Dep_Half;

                    item.Closing_WDV = item.Final_Total - item.Total_Depreciation;

                }

                // Save to Database

                foreach (var item in dictDep.Values)
                {

                    StringBuilder strSQLInserAsset = new StringBuilder();



                    strSQLInserAsset.Append(" insert into tblitdepreciation (ClientID,Companyid,FromDate,ToDate,ITGrpID");
                    strSQLInserAsset.Append(" , ITGroupName , OpeningWDV, DepreciationRate , Additionbefore, AdditionAfter ");
                    strSQLInserAsset.Append(" , TotalFull , TotalHalf, DisposalBefore , DisposalAfter, FinalTotalFull ");
                    strSQLInserAsset.Append(" , FinalTotalHalf , FinalTotal, Profit , DepFull, DepHalf,TotalDep ");
                    strSQLInserAsset.Append(" , ClosingWDV ");
                    //strSQLInserAsset.Append(" , CreatedDate, ModifiedDate , Modified_Userid, CreatedUserId ");
                    strSQLInserAsset.Append(" ) Values ( ");

                    strSQLInserAsset.Append(ClientID).Append(",");
                    strSQLInserAsset.Append(CompanyID).Append(",");

                    strSQLInserAsset.Append("'" + str_fromdate + "'").Append(",");
                    strSQLInserAsset.Append("'" + str_todate + "'").Append(",");
                    strSQLInserAsset.Append(item.itgroupid).Append(",");
                    strSQLInserAsset.Append("'" + item.groupname + "'").Append(",");

                    strSQLInserAsset.Append(item.OpWDV).Append(",");

                    strSQLInserAsset.Append(item.DepRate).Append(",");
                    strSQLInserAsset.Append(item.Additions_LessThan180).Append(",");
                    strSQLInserAsset.Append(item.Additions_MoreThan180).Append(",");
                    strSQLInserAsset.Append(item.Total_Full).Append(",");
                    strSQLInserAsset.Append(item.Total_Half).Append(",");

                    strSQLInserAsset.Append(item.Disposal_LessThan180).Append(",");
                    strSQLInserAsset.Append(item.Disposal_MoreThan180).Append(",");

                    strSQLInserAsset.Append(item.Final_Total_Full).Append(",");
                    strSQLInserAsset.Append(item.Final_Total_Half).Append(",");

                    strSQLInserAsset.Append(item.Final_Total).Append(",");

                    strSQLInserAsset.Append(item.Profit).Append(",");

                    strSQLInserAsset.Append(item.Dep_Full).Append(",");

                    strSQLInserAsset.Append(item.Dep_Half).Append(",");

                    strSQLInserAsset.Append(item.Total_Depreciation).Append(",");

                    strSQLInserAsset.Append(item.Closing_WDV).Append("");

                    strSQLInserAsset.Append(" ); ");

                    MySqlCommand cmdInsert_working = new MySqlCommand(strSQLInserAsset.ToString(), connection);
                    cmdInsert_working.CommandTimeout = 86400;
                    cmdInsert_working.ExecuteNonQuery();

                }


                strSQL = "";
                strSQL = " update tblitperiod  set DepFlag = 'Y'  where FromDate = '" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and ToDate = '" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and id >" + 0;

                MySqlCommand cmd_updateperiod_flag = new MySqlCommand(strSQL, connection);
                cmd_updateperiod_flag.ExecuteNonQuery();


                strSQL = "";
                strSQL = " update tbldepreciationrequest_incometax  set InProcess = 2  where StartDate = '" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and EndDate = '" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and id >" + 0;



                MySqlCommand cmd_request_flag = new MySqlCommand(strSQL, connection);
                cmd_request_flag.ExecuteNonQuery();

                trann.Commit();

            }
            catch(Exception ex)
            {
                logger.Error(" Error in Calculation");
                logger.Error(ex);

                trann.Rollback();
               // return false;
            }






        }

        private DataTable getDataTable(string strSQL)
        {

            logger.Log(LogLevel.Info, " getDataTable ");

            DataTable dtTemp = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = connection;
                cmd.CommandText = strSQL;

                MySqlDataAdapter adapater = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapater.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    dtTemp = ds.Tables[0];
                }
            }

            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                throw ex;
            }

            return dtTemp;

        }
    }
}
