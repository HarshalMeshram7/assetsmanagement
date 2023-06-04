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
    public class CompanyLawDepreciation
    {

        private string ConnectionString { get; set; }
        private MySqlConnection connection = new MySqlConnection();

        public NLog.Logger logger { get; set; }

        


        public CompanyLawDepreciation(string _ConnectionString)
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
            catch(Exception ex)
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
                string strSQL = "SELECT * FROM tbldepreciationrequest where InProcess = -1";
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

                        StartCalculationV4(dtStartDate, dtEndDate);
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

        private bool StartCalculationV4(DateTime startDate, DateTime enddate)
        {


            MySqlTransaction trann =  connection.BeginTransaction();

            try
            {

                logger.Info("Start Dep Calculation " + startDate.ToString());

                string strSQL_log = "delete from tbldepreciation_log where id >=0";

                MySqlCommand cmdDelete_working_log = new MySqlCommand(strSQL_log, connection);
                cmdDelete_working_log.ExecuteNonQuery();



                string strSQL = "delete from tbldepworking where id >=0";

                MySqlCommand cmdDelete_working = new MySqlCommand(strSQL, connection);
                cmdDelete_working.ExecuteNonQuery();


                strSQL = "delete from tbldepreciation where FromDate >= '" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and ToDate <= '" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and id >" + 0;
                strSQL = strSQL + " and DepreciationType='A'";

                cmdDelete_working = new MySqlCommand(strSQL, connection);
                cmdDelete_working.ExecuteNonQuery();


                // WDV
                StartCalc_WithoutDisposal_WDV(startDate, enddate);

                

                StartCalc_WithDisposal_V1_WDV(startDate, enddate);

                // SLM 
                StartCalc_WithoutDisposal_SLM(startDate, enddate);
                StartCalc_WithDisposal_V1_SLM(startDate, enddate);

                strSQL = "";
                strSQL = " update tblsubperiod  set DepFlag = 'Y'  where FromDate = '" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and ToDate = '" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and id >" + 0;

                MySqlCommand cmd_updateperiod_flag = new MySqlCommand(strSQL, connection);
                cmd_updateperiod_flag.ExecuteNonQuery();


                strSQL = "";
                strSQL = " update tbldepreciationrequest  set InProcess = 2  where StartDate = '" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and EndDate = '" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQL = strSQL + " and id >" + 0;



                MySqlCommand cmd_request_flag = new MySqlCommand(strSQL, connection);
                cmd_request_flag.ExecuteNonQuery();




                trann.Commit();



                return true;

            }
            catch(Exception ex)
            {
                logger.Error(" Error in Calculation");
                logger.Error(ex);

                trann.Rollback();
                return false;
            }


        }

        private void StartCalc_WithoutDisposal_WDV(DateTime startDate, DateTime enddate)
        {
            int Days_In_Year = 365;
            Days_In_Year = GetDaysInYear(startDate, enddate);

            try
            {
                logger.Log(LogLevel.Info, " StartCalc_WithoutDisposal_WDV...");



                string strSQLInserAsset = "insert into tbldepworking (";
                strSQLInserAsset = strSQLInserAsset + "assetid,assetno,DtPutToUse,usefulllife,fromdate,todate,amountcapitalised,opaccdepreciation,residualvalue,eff_fromdate,eff_todate )";
                strSQLInserAsset = strSQLInserAsset + " select id,assetno,DtPutToUse,Usefullife," + "'" + startDate.ToString("yyyy-MM-dd") + "'" + " , '" + enddate.ToString("yyyy-MM-dd") + "' ,amountcapitalised,opaccdepreciation ,ResidualVal, ";
                strSQLInserAsset = strSQLInserAsset + "'" + startDate.ToString("yyyy-MM-dd") + "'" + " , '" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " from tblassets ";
                strSQLInserAsset = strSQLInserAsset + " where DepreciationMethod = 'WDV' ";
                strSQLInserAsset = strSQLInserAsset + " and DtPutToUse <=  " + "'" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " and id NOT in ( select assetid  from tbldisposal ";
                strSQLInserAsset = strSQLInserAsset + " where DisposalDate >='" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " and DisposalDate <='" + enddate.ToString("yyyy-MM-dd") + "' )";


                MySqlCommand cmdInsert_working = new MySqlCommand(strSQLInserAsset, connection);
                cmdInsert_working.ExecuteNonQuery();


                // update  deptillstdt


                string strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(Amount),0) as deptillstdt";
                strUpdate = strUpdate + "  from tbldepreciation";
                strUpdate = strUpdate + "  where tbldepreciation.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldepreciation.FromDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as depreciation ON";
                strUpdate = strUpdate + "  working.assetid = depreciation.assetid";
                strUpdate = strUpdate + " set working.deptillstdt = depreciation.deptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate = new MySqlCommand(strUpdate, connection);
                cmdUPdate.ExecuteNonQuery();


                // update  disptillstdt

                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(DisposalAmount),0) as disptillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.disptillstdt = disposal.disptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_disposal = new MySqlCommand(strUpdate, connection);
                cmdUPdate_disposal.ExecuteNonQuery();

                // add new column in tbldisposal form depreciationreversal - done // CHANGED DATABASE



                // update dep reversed till date 
                // OpAccumulatedDep = depreciationreversal
                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(OpAccumulatedDep),0) as disp_dep_reversal_tillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.dep_rev_tillstdt = disposal.disp_dep_reversal_tillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_dep_reverse = new MySqlCommand(strUpdate, connection);
                cmdUPdate_dep_reverse.ExecuteNonQuery();


                //



                strUpdate = "";
                strUpdate = strUpdate + " update tbldepworking working ";
                strUpdate = strUpdate + " inner join tblassets assets on working.assetid = assets.id ";
                strUpdate = strUpdate + " set working.depmethod = assets.DepreciationMethod , ";
                strUpdate = strUpdate + " working.deprate = assets.TotalRate ";
                strUpdate = strUpdate + " where working.id > 0 ";

                MySqlCommand cmdUPdate_rate = new MySqlCommand(strUpdate, connection);
                cmdUPdate_rate.ExecuteNonQuery();



                // update asset op gross value 

                //update opassetgross set = amountcapitalised - disptillstdt where id > 0

                strUpdate = "";
                strUpdate = " update tbldepworking set opassetgross  = (amountcapitalised - disptillstdt) where id > 0 ";


                MySqlCommand cmdUPdate_opgross = new MySqlCommand(strUpdate, connection);
                cmdUPdate_opgross.ExecuteNonQuery();



                strUpdate = "";
                strUpdate = " update tbldepworking set opdep  = (opaccdepreciation + deptillstdt) - (dep_rev_tillstdt) where id > 0 ";

                MySqlCommand cmdUPdate_opdep = new MySqlCommand(strUpdate, connection);
                cmdUPdate_opdep.ExecuteNonQuery();


                /// Mandar 05 JAN Determine Dep Method SLM or WDV
                /// 

                strUpdate = "";
                strUpdate = " update tbldepworking set amt_for_dep_calc  = (opassetgross - opdep) where id > 0 ";

                MySqlCommand cmdUPdate_amt = new MySqlCommand(strUpdate, connection);
                cmdUPdate_amt.ExecuteNonQuery();




                // start calculation 


                string strSQL = "";

                //strSQL = "select * from tbldepworking";

                strSQL = "select * from tbldepworking where depmethod = 'WDV' and disp_id = 0";

                DataTable dtAssetsWorking = getDataTable(strSQL);


                if (dtAssetsWorking.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssetsWorking.Rows)
                    {
                        decimal amountcapitalised = 0;
                        decimal opaccdepreciation = 0;
                        decimal disptillstdt = 0;
                        decimal deptillstdt = 0;
                        decimal dep_rev_tillstdt = 0;
                        decimal opassetgross = 0;
                        decimal opdep = 0;
                        decimal amt_for_dep_calc = 0;
                        string depmethod = "SLM";
                        decimal deprate = 0;
                        decimal depmamt = 0;
                        decimal dep_to_be_reversed = 0;
                        decimal residualvalue = 0;

                        int AssetID = 0;
                        int UseFullLife = 0;
                        int id = 0;

                        string assetno = "";

                        DateTime AssetExpiryDate = startDate;
                        DateTime DtPuttoUse = startDate;
                        DateTime eff_fromdate = startDate;
                        DateTime eff_todate = enddate;



                        


                        id = Convert.ToInt32(dr["id"].ToString());
                        AssetID = Convert.ToInt32(dr["assetid"].ToString());

                        string strLog = "Insert into tbldepreciation_log (assetid,message) ";
                        strLog = strLog + " Values (" + AssetID + ", 'Started Processing'";
                        strLog = strLog + ")";

                        MySqlCommand cmd_log = new MySqlCommand(strLog, connection);
                        cmd_log.ExecuteNonQuery();



                        UseFullLife = Convert.ToInt32(dr["usefulllife"].ToString());
                        amountcapitalised = Convert.ToDecimal(dr["amountcapitalised"].ToString());
                        opaccdepreciation = Convert.ToDecimal(dr["opaccdepreciation"].ToString());
                        disptillstdt = Convert.ToDecimal(dr["disptillstdt"].ToString());
                        deptillstdt = Convert.ToDecimal(dr["deptillstdt"].ToString());
                        dep_rev_tillstdt = Convert.ToDecimal(dr["dep_rev_tillstdt"].ToString());
                        opassetgross = Convert.ToDecimal(dr["opassetgross"].ToString());
                        opdep = Convert.ToDecimal(dr["opdep"].ToString());
                        amt_for_dep_calc = Convert.ToDecimal(dr["amt_for_dep_calc"].ToString());
                        depmethod = dr["depmethod"].ToString();
                        deprate = Convert.ToDecimal(dr["deprate"].ToString());
                        depmamt = Convert.ToDecimal(dr["depmamt"].ToString());
                        dep_to_be_reversed = Convert.ToDecimal(dr["dep_to_be_reversed"].ToString());

                        eff_fromdate = Convert.ToDateTime(dr["eff_fromdate"].ToString());
                        eff_todate = Convert.ToDateTime(dr["eff_todate"].ToString());

                        assetno = dr["assetno"].ToString();
                        // depmethod = dr["depmethod"].ToString();

                        residualvalue = Convert.ToDecimal(dr["residualvalue"].ToString());

                        System.Diagnostics.Debug.WriteLine("AssetNo " + assetno);

                        if (AssetID == 1017)
                        {
                            int j = 0;
                        }


                        try
                        {

                            DtPuttoUse = Convert.ToDateTime(dr["DtPuttoUse"].ToString()); ;
                            AssetExpiryDate = DtPuttoUse.AddYears(UseFullLife); // Calculate Asset Expiry Date


                        }
                        catch (Exception ex)
                        {
                            DtPuttoUse = startDate; ;
                            AssetExpiryDate = DtPuttoUse;
                        }



                        int NoOfDays = 0;

                        if (DtPuttoUse > eff_fromdate)
                        {
                            eff_fromdate = DtPuttoUse;
                            NoOfDays = (eff_todate - eff_fromdate).Days + 1;
                        }
                        else
                        {
                            NoOfDays = (eff_todate - eff_fromdate).Days + 1;
                        }




                        if (amt_for_dep_calc > 0)
                        {
                            depmamt = ((amt_for_dep_calc * deprate / 100) / Days_In_Year) * NoOfDays;
                            depmamt = Math.Round(depmamt, 2);

                            // process dep without disposal 

                            if (residualvalue > 0)
                            {

                                if ((depmamt + opdep + residualvalue) > (amountcapitalised - disptillstdt))
                                {
                                    depmamt = (amountcapitalised - disptillstdt) - (opdep + residualvalue);

                                }

                            }
                            else
                            {
                                if ((depmamt + opdep) > (amountcapitalised - disptillstdt))
                                {
                                    depmamt = amountcapitalised - opdep;
                                }
                            }

                            if (depmamt < 0)
                            {
                                depmamt = 0;
                            }
                            strUpdate = "";
                            strUpdate = " update tbldepworking set depmamt  = " + depmamt + " where id = " + id;

                            MySqlCommand cmdUPdate_dep = new MySqlCommand(strUpdate, connection);
                            cmdUPdate_dep.ExecuteNonQuery();


                            // rule 1 


                            /*todo insert into tbldepreciaton  mandar 05 dec 2021 */



                            String strSQLInsert = "INSERT INTO tbldepreciation";
                            strSQLInsert = strSQLInsert + "(AssetID,Assetname,FromDate, ";
                            strSQLInsert = strSQLInsert + " ToDate,DepreciationType,Amount,NormalRate,";
                            strSQLInsert = strSQLInsert + " AdditionRate,TotalRate, DepreciationDays,ClientID,DepreciationMethod,Companyid,";
                            strSQLInsert = strSQLInsert + " Assetno  )";
                            strSQLInsert = strSQLInsert + " Values  (";
                            strSQLInsert = strSQLInsert + AssetID + ",";
                            strSQLInsert = strSQLInsert + "''" + ",";
                            strSQLInsert = strSQLInsert + "'" + eff_fromdate.ToString("yyyy-MM-dd") + "'" + ",";
                            strSQLInsert = strSQLInsert + "'" + eff_todate.ToString("yyyy-MM-dd") + "'" + ",";
                            strSQLInsert = strSQLInsert + "'A'" + ",";
                            strSQLInsert = strSQLInsert + depmamt + ",";
                            strSQLInsert = strSQLInsert + deprate + ",";
                            strSQLInsert = strSQLInsert + 0 + ",";
                            strSQLInsert = strSQLInsert + deprate + ",";
                            strSQLInsert = strSQLInsert + NoOfDays + ",";
                            strSQLInsert = strSQLInsert + "1 ,";
                            strSQLInsert = strSQLInsert + "'" + depmethod + "'" + ",";
                            strSQLInsert = strSQLInsert + "1, ";
                            strSQLInsert = strSQLInsert + "'" + assetno + "'";
                            
                            strSQLInsert = strSQLInsert + ")";

                            MySqlCommand cmdUPdate_dep_inesrt = new MySqlCommand(strSQLInsert, connection);
                            cmdUPdate_dep_inesrt.ExecuteNonQuery();

                        }




                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(" Error in StartCalc_WithoutDisposal_WDV");
                logger.Error(ex);
                throw ex;
                //System.Diagnostics.Debug.WriteLine("error" + ex.Message);
            }

        }

        private void StartCalc_WithDisposal_V1_WDV(DateTime startDate, DateTime enddate)
        {
            int Days_In_Year = 365;
            Days_In_Year = GetDaysInYear(startDate, enddate);
            //MySqlTransaction myTrans = null;

            try
            {

                //  myTrans = connection.BeginTransaction();

                logger.Log(LogLevel.Info, " StartCalc_WithDisposal_V1_WDV...");




                string strSQLInserAsset = "insert into tbldepworking (assetid,disp_id)";
                strSQLInserAsset = strSQLInserAsset + " select distinct assetid,-1 from tbldisposal  ";
                strSQLInserAsset = strSQLInserAsset + " inner join tblassets on  tbldisposal.assetid = tblassets.id ";
                strSQLInserAsset = strSQLInserAsset + " where  tblassets.DepreciationMethod = 'WDV'";
                strSQLInserAsset = strSQLInserAsset + " and DisposalDate >='" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " and DisposalDate <='" + enddate.ToString("yyyy-MM-dd") + "' ";

                MySqlCommand cmdInsert_working = new MySqlCommand(strSQLInserAsset, connection);
               // cmdInsert_working.Transaction = myTrans;

                cmdInsert_working.ExecuteNonQuery();


                string strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join tblassets asset ";
                strUpdate = strUpdate + " on working.assetid = asset.id ";
                strUpdate = strUpdate + " set working.assetno = asset.assetno , ";
                strUpdate = strUpdate + " working.DtPutToUse = asset.DtPutToUse , ";
                strUpdate = strUpdate + " working.usefulllife = asset.Usefullife , ";
                strUpdate = strUpdate + " working.amountcapitalised = asset.amountcapitalised , ";
                strUpdate = strUpdate + " working.opaccdepreciation = asset.opaccdepreciation , ";
                strUpdate = strUpdate + " working.depmethod = asset.DepreciationMethod , ";
                strUpdate = strUpdate + " working.residualvalue = asset.ResidualVal , ";


                strUpdate = strUpdate + " working.fromdate = '" + startDate.ToString("yyyy-MM-dd") + "' , ";
                strUpdate = strUpdate + " working.todate = '" + enddate.ToString("yyyy-MM-dd") + "' ,";


                strUpdate = strUpdate + " working.eff_fromdate = '" + startDate.ToString("yyyy-MM-dd") + "' , ";
                strUpdate = strUpdate + " working.eff_todate = '" + enddate.ToString("yyyy-MM-dd") + "' ";

                strUpdate = strUpdate + " where working.id > 0 ";

                MySqlCommand cmdUPdate = new MySqlCommand(strUpdate, connection);
             //   cmdUPdate.Transaction = myTrans;
                cmdUPdate.ExecuteNonQuery();




                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(Amount),0) as deptillstdt";
                strUpdate = strUpdate + "  from tbldepreciation";
                strUpdate = strUpdate + "  where tbldepreciation.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldepreciation.FromDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as depreciation ON";
                strUpdate = strUpdate + "  working.assetid = depreciation.assetid";
                strUpdate = strUpdate + " set working.deptillstdt = depreciation.deptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                cmdUPdate = new MySqlCommand(strUpdate, connection);
                //cmdUPdate.Transaction = myTrans;
                cmdUPdate.ExecuteNonQuery();


                // update  disptillstdt

                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(DisposalAmount),0) as disptillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.disptillstdt = disposal.disptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_disposal = new MySqlCommand(strUpdate, connection);
               // cmdUPdate_disposal.Transaction = myTrans;
                cmdUPdate_disposal.ExecuteNonQuery();

                // add new column in tbldisposal form depreciationreversal - done // CHANGED DATABASE



                // update dep reversed till date 

                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(OpAccumulatedDep),0) as disp_dep_reversal_tillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.dep_rev_tillstdt = disposal.disp_dep_reversal_tillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_dep_reverse = new MySqlCommand(strUpdate, connection);
               // cmdUPdate_dep_reverse.Transaction = myTrans;
                cmdUPdate_dep_reverse.ExecuteNonQuery();


                //



                strUpdate = "";
                strUpdate = strUpdate + " update tbldepworking working ";
                strUpdate = strUpdate + " inner join tblassets assets on working.assetid = assets.id ";
                strUpdate = strUpdate + " set working.depmethod = assets.DepreciationMethod , ";
                strUpdate = strUpdate + " working.deprate = assets.TotalRate ";
                strUpdate = strUpdate + " where working.id > 0 ";

                MySqlCommand cmdUPdate_rate = new MySqlCommand(strUpdate, connection);
              //  cmdUPdate_rate.Transaction = myTrans;
                cmdUPdate_rate.ExecuteNonQuery();



                // update asset op gross value 

                //update opassetgross set = amountcapitalised - disptillstdt where id > 0

                strUpdate = "";
                strUpdate = " update tbldepworking set opassetgross  = (amountcapitalised - disptillstdt) where id > 0 ";


                MySqlCommand cmdUPdate_opgross = new MySqlCommand(strUpdate, connection);
              //  cmdUPdate_opgross.Transaction = myTrans;
                cmdUPdate_opgross.ExecuteNonQuery();



                strUpdate = "";
                strUpdate = " update tbldepworking set opdep  = (opaccdepreciation + deptillstdt) - (dep_rev_tillstdt) where id > 0 ";

                MySqlCommand cmdUPdate_opdep = new MySqlCommand(strUpdate, connection);
               // cmdUPdate_opdep.Transaction = myTrans;
                cmdUPdate_opdep.ExecuteNonQuery();


                strUpdate = "";
                strUpdate = " update tbldepworking set amt_for_dep_calc  = (opassetgross - opdep) where id > 0 ";

                MySqlCommand cmdUPdate_amt = new MySqlCommand(strUpdate, connection);
               // cmdUPdate_amt.Transaction = myTrans;
                cmdUPdate_amt.ExecuteNonQuery();






                // start calculation 


                string strSQL;


                DataTable dtDepWorking = new DataTable();
                strSQL = "Select * from tbldepworking where disp_id = -1 and depmethod = 'WDV'";
                dtDepWorking = getDataTable(strSQL);


                if (dtDepWorking.Rows.Count > 0)
                {

                    foreach (DataRow dr_working in dtDepWorking.Rows)
                    {

                        decimal amountcapitalised = 0;
                        decimal opaccdepreciation = 0;
                        decimal disptillstdt = 0;
                        decimal deptillstdt = 0;
                        decimal dep_rev_tillstdt = 0;
                        decimal opassetgross = 0;
                        decimal opdep = 0;
                        decimal amt_for_dep_calc = 0;
                        string depmethod = "WDV";
                        decimal deprate = 0;
                        decimal depmamt = 0;
                        decimal dep_to_be_reversed = 0;

                        decimal residualvalue = 0;
                        decimal disposal_amt = 0; // from disposal table
                        string assetno = "";

                        int AssetID = 0;
                        int UseFullLife = 0;
                        int id = 0;

                        DateTime AssetExpiryDate = startDate;
                        DateTime DtPuttoUse = startDate;

                        DateTime eff_fromdate = startDate;
                        DateTime eff_todate = enddate;

                        id = Convert.ToInt32(dr_working["id"].ToString());
                        AssetID = Convert.ToInt32(dr_working["assetid"].ToString());


                        string strLog = "Insert into tbldepreciation_log (assetid,message) ";
                        strLog = strLog + " Values (" + AssetID + ", 'Started Processing'";
                        strLog = strLog + ")";

                        MySqlCommand cmd_log = new MySqlCommand(strLog, connection);
                        cmd_log.ExecuteNonQuery();


                        if (AssetID == 12980)
                        {
                            int j = 0;
                        }

                        DtPuttoUse = Convert.ToDateTime(dr_working["DtPuttoUse"].ToString());

                        if (DtPuttoUse > startDate)
                        {
                            eff_fromdate = DtPuttoUse;
                        }


                        //disp_id = Convert.ToInt32(dr_working["disp_id"].ToString()); // disposal record id from disposal table

                        UseFullLife = Convert.ToInt32(dr_working["usefulllife"].ToString());
                        amountcapitalised = Convert.ToDecimal(dr_working["amountcapitalised"].ToString()); // from asset 
                        opaccdepreciation = Convert.ToDecimal(dr_working["opaccdepreciation"].ToString()); // from asset

                        depmethod = dr_working["depmethod"].ToString();
                        deprate = Convert.ToDecimal(dr_working["deprate"].ToString());
                        eff_fromdate = Convert.ToDateTime(dr_working["eff_fromdate"].ToString());
                        eff_todate = Convert.ToDateTime(dr_working["eff_todate"].ToString());

                        disptillstdt = Convert.ToDecimal(dr_working["disptillstdt"].ToString());

                        deptillstdt = Convert.ToDecimal(dr_working["deptillstdt"].ToString());

                        dep_rev_tillstdt = Convert.ToDecimal(dr_working["dep_rev_tillstdt"].ToString());

                        opassetgross = Convert.ToDecimal(dr_working["opassetgross"].ToString());
                        opdep = Convert.ToDecimal(dr_working["opdep"].ToString());

                        disposal_amt = Convert.ToDecimal(dr_working["disposalamt"].ToString());


                        assetno = dr_working["assetno"].ToString();

                        System.Diagnostics.Debug.WriteLine("AssetNo " + assetno);


                        residualvalue = Convert.ToDecimal(dr_working["residualvalue"].ToString());


                        decimal temp_GrossAmt = 0;
                        decimal temp_Dep_TillStartDate = 0;

                        temp_GrossAmt = (amountcapitalised - disptillstdt);

                        temp_Dep_TillStartDate = (opaccdepreciation + deptillstdt) - dep_rev_tillstdt;

                        amt_for_dep_calc = (temp_GrossAmt - temp_Dep_TillStartDate);




                        DtPuttoUse = Convert.ToDateTime(dr_working["DtPuttoUse"].ToString()); ;
                        AssetExpiryDate = DtPuttoUse.AddYears(UseFullLife); // Calculate Asset Expiry Date

                        int NoOfDays_temp = 0;
                        NoOfDays_temp = (eff_todate - eff_fromdate).Days + 1;


                        depmamt = ((amt_for_dep_calc * deprate / 100) / Days_In_Year) * NoOfDays_temp;
                        depmamt = Math.Round(depmamt, 2);


                        // calculated dep to be reversed

                        decimal totaldep_till_date = (depmamt + temp_Dep_TillStartDate);

                        // formula  dep_to_be_reversed  = dep / (op gros blcok - all disposal till date) * disposal amt

                        strUpdate = " update tbldepworking set depmamt  = " + depmamt + " where id = " + id;

                        MySqlCommand cmdUPdate_dep2 = new MySqlCommand(strUpdate, connection);
                        cmdUPdate_dep2.ExecuteNonQuery();



                        strUpdate = " update tbldisposal set OpAccumulatedDep  = " + dep_to_be_reversed + ", OpAccumulatedDep = " + dep_to_be_reversed + " where ID = " + id;

                        cmdUPdate_dep2 = new MySqlCommand(strUpdate, connection);
                        cmdUPdate_dep2.ExecuteNonQuery();


                        // check disposal for the asset 

                        strSQL = "";
                        strSQL = " select * from tbldisposal ";
                        strSQL = strSQL + "  where tbldisposal.companyid = 1 ";
                        strSQL = strSQL + "  and  tbldisposal.AssetId =" + AssetID;
                        strSQL = strSQL + "  and  tbldisposal.DisposalDate >='" + startDate.ToString("yyyy-MM-dd") + "'";
                        strSQL = strSQL + "  and  tbldisposal.DisposalDate <='" + enddate.ToString("yyyy-MM-dd") + "'";


                        DataTable dtDisposal = new DataTable();
                        dtDisposal = getDataTable(strSQL);
                        // decimal TotalReversal = 0;
                        // decimal Total_Extra_Dep_ToBeReduced = 0;

                        int disp_id = 0;

                        foreach (DataRow disp_Row in dtDisposal.Rows)
                        {

                            disp_id = Convert.ToInt32(disp_Row["ID"].ToString());

                            decimal disposalAmt = Convert.ToDecimal(disp_Row["DisposalAmount"].ToString());
                            DateTime disposalDate = Convert.ToDateTime(disp_Row["DisposalDate"].ToString());
                            decimal OpAsset = amountcapitalised - disptillstdt;

                            decimal DepReversal_1 = Math.Round((temp_Dep_TillStartDate * disposalAmt) / OpAsset, 2);

                            int Reverse_days = (disposalDate - startDate).Days + 1;
                            int extra_days = (enddate - disposalDate).Days;

                            decimal temp_amt = (disposalAmt - DepReversal_1);

                            //decimal DepReversal_2 = (temp_amt) / OpAsset;

                            decimal DepReversal_2 = Math.Round(((temp_amt * deprate / 100) / Days_In_Year) * Reverse_days, 2);


                            decimal total_temp_reversal = DepReversal_1 + DepReversal_2;


                            decimal extra_dep_reversal = 0 - ((disposalAmt - DepReversal_1) * deprate / 100 / Days_In_Year) * extra_days;
                            extra_dep_reversal = Math.Round(extra_dep_reversal, 2);

                            string strSQL_insert_temp = "";

                            strSQL_insert_temp = "insert into tbldepworking (assetid,depmamt,dep_to_be_reversed,disp_id) values ( ";
                            strSQL_insert_temp = strSQL_insert_temp + AssetID + "," + extra_dep_reversal + "," + total_temp_reversal + "," + disp_id + ")";

                            MySqlCommand cmdUPdate_dep_inesrt_1 = new MySqlCommand(strSQL_insert_temp, connection);
                            cmdUPdate_dep_inesrt_1.ExecuteNonQuery();


                            /// update reversal in disposal table
                            /// TODO : Mandar 16 DEC 2021
                            /// 
                            strSQL = "";



                            strSQL = " update tbldisposal set OpAccumulatedDep  = " + total_temp_reversal + ", OpAccumulatedDep = " + total_temp_reversal + " where ID = " + disp_id;

                            cmdUPdate_dep_inesrt_1 = new MySqlCommand(strSQL, connection);
                            cmdUPdate_dep_inesrt_1.ExecuteNonQuery();

                        }

                        //
                        //   deprate 





                        //   *todo insert into tbldepreciaton  mandar 05 dec 2021 */


                        strSQL = "";
                        strSQL = "select  IFNULL(sUM(depmamt),0) depamt from tbldepworking where assetid = " + AssetID;

                        DataTable dtDepTemp = new DataTable();

                        dtDepTemp = getDataTable(strSQL);

                        decimal final_dep_amt = 0;


                        final_dep_amt = Convert.ToDecimal(dtDepTemp.Rows[0][0]);


                        if (residualvalue > 0)
                        {

                            if ((depmamt + opdep + residualvalue) > (amountcapitalised - disptillstdt))
                            {
                                depmamt = (amountcapitalised - disptillstdt) - (opdep + residualvalue);

                            }

                        }
                        else
                        {
                            if ((depmamt + opdep) > (amountcapitalised - disptillstdt))
                            {
                                depmamt = amountcapitalised - opdep;
                            }
                        }

                        if (depmamt < 0)
                        {
                            depmamt = 0;
                        }


                        // TO DO 
                        // Check to be implemented for Depreciation


                        // 1) check if Residual Value 



                        // 2) total depreaciaton should not be more than amt capitalized



                        // 3) 





                        String strSQLInsert = "INSERT INTO tbldepreciation";
                        strSQLInsert = strSQLInsert + "(AssetID,Assetname,FromDate, ";
                        strSQLInsert = strSQLInsert + " ToDate,DepreciationType,Amount,NormalRate,";
                        strSQLInsert = strSQLInsert + " AdditionRate,TotalRate, DepreciationDays,ClientID,DepreciationMethod,Companyid";
                        strSQLInsert = strSQLInsert + "   )";
                        strSQLInsert = strSQLInsert + " Values  (";
                        strSQLInsert = strSQLInsert + AssetID + ",";
                        strSQLInsert = strSQLInsert + "''" + ",";
                        strSQLInsert = strSQLInsert + "'" + startDate.ToString("yyyy-MM-dd") + "'" + ",";
                        strSQLInsert = strSQLInsert + "'" + enddate.ToString("yyyy-MM-dd") + "'" + ",";
                        strSQLInsert = strSQLInsert + "'A'" + ",";
                        strSQLInsert = strSQLInsert + final_dep_amt + ",";
                        strSQLInsert = strSQLInsert + deprate + ",";
                        strSQLInsert = strSQLInsert + 0 + ",";
                        strSQLInsert = strSQLInsert + deprate + ",";
                        strSQLInsert = strSQLInsert + NoOfDays_temp + ",";
                        strSQLInsert = strSQLInsert + "1 ,";
                        strSQLInsert = strSQLInsert + "'" + depmethod + "'" + ",";
                        strSQLInsert = strSQLInsert + "1";
                        
                        strSQLInsert = strSQLInsert + ")";

                        MySqlCommand cmdUPdate_dep_inesrt = new MySqlCommand(strSQLInsert, connection);
                       // cmdUPdate_dep_inesrt.Transaction = myTrans;
                        cmdUPdate_dep_inesrt.ExecuteNonQuery();

                    }

                }

               // myTrans.Commit();
            }
            catch (Exception ex)
            {

                logger.Error(" Error in StartCalc_WithDisposal_V1_WDV");
                logger.Error(ex);

                throw ex;

                // myTrans.Rollback();
                //DtPuttoUse = startDate; ;
                //AssetExpiryDate = DtPuttoUse;
            }


        }

        private void StartCalc_WithoutDisposal_SLM(DateTime startDate, DateTime enddate)
        {
            int Days_In_Year = 365;
            Days_In_Year = GetDaysInYear(startDate, enddate);


            try
            {
                logger.Log(LogLevel.Info, " StartCalc_WithoutDisposal_SLM...");


                string strSQLInserAsset = "insert into tbldepworking (";
                strSQLInserAsset = strSQLInserAsset + "assetid,assetno,DtPutToUse,usefulllife,fromdate,todate,amountcapitalised,opaccdepreciation,residualvalue,DisposalFlag,eff_fromdate,eff_todate )";
                strSQLInserAsset = strSQLInserAsset + " select id,assetno,DtPutToUse,Usefullife," + "'" + startDate.ToString("yyyy-MM-dd") + "'" + " , '" + enddate.ToString("yyyy-MM-dd") + "' ,amountcapitalised,opaccdepreciation ,ResidualVal,DisposalFlag, ";
                strSQLInserAsset = strSQLInserAsset + "'" + startDate.ToString("yyyy-MM-dd") + "'" + " , '" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " from tblassets ";
                strSQLInserAsset = strSQLInserAsset + " where DepreciationMethod = 'SLM' ";
                strSQLInserAsset = strSQLInserAsset + " and DtPutToUse <=  " + "'" + enddate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " and id NOT in ( select assetid  from tbldisposal ";
                strSQLInserAsset = strSQLInserAsset + " where DisposalDate >='" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " and DisposalDate <='" + enddate.ToString("yyyy-MM-dd") + "' )";


                MySqlCommand cmdInsert_working = new MySqlCommand(strSQLInserAsset, connection);
                cmdInsert_working.ExecuteNonQuery();


                // update  deptillstdt


                string strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(Amount),0) as deptillstdt";
                strUpdate = strUpdate + "  from tbldepreciation";
                strUpdate = strUpdate + "  where tbldepreciation.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldepreciation.FromDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as depreciation ON";
                strUpdate = strUpdate + "  working.assetid = depreciation.assetid";
                strUpdate = strUpdate + " set working.deptillstdt = depreciation.deptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate = new MySqlCommand(strUpdate, connection);
                cmdUPdate.ExecuteNonQuery();


                // update  disptillstdt

                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(DisposalAmount),0) as disptillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.disptillstdt = disposal.disptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_disposal = new MySqlCommand(strUpdate, connection);
                cmdUPdate_disposal.ExecuteNonQuery();

                // add new column in tbldisposal form depreciationreversal - done // CHANGED DATABASE



                // update dep reversed till date 

                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(OpAccumulatedDep),0) as disp_dep_reversal_tillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.dep_rev_tillstdt = disposal.disp_dep_reversal_tillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_dep_reverse = new MySqlCommand(strUpdate, connection);
                cmdUPdate_dep_reverse.ExecuteNonQuery();


                //



                strUpdate = "";
                strUpdate = strUpdate + " update tbldepworking working ";
                strUpdate = strUpdate + " inner join tblassets assets on working.assetid = assets.id ";
                strUpdate = strUpdate + " set working.depmethod = assets.DepreciationMethod , ";
                strUpdate = strUpdate + " working.deprate = assets.TotalRate ";
                strUpdate = strUpdate + " where working.id > 0 ";

                MySqlCommand cmdUPdate_rate = new MySqlCommand(strUpdate, connection);
                cmdUPdate_rate.ExecuteNonQuery();



                // update asset op gross value 

                //update opassetgross set = amountcapitalised - disptillstdt where id > 0

                strUpdate = "";
                strUpdate = " update tbldepworking set opassetgross  = (amountcapitalised - disptillstdt) where id > 0 ";


                MySqlCommand cmdUPdate_opgross = new MySqlCommand(strUpdate, connection);
                cmdUPdate_opgross.ExecuteNonQuery();



                strUpdate = "";
                strUpdate = " update tbldepworking set opdep  = (opaccdepreciation + deptillstdt) - (dep_rev_tillstdt) where id > 0 ";

                MySqlCommand cmdUPdate_opdep = new MySqlCommand(strUpdate, connection);
                cmdUPdate_opdep.ExecuteNonQuery();


                /// Mandar 06 JAN 
                /// 

                strUpdate = "";
                strUpdate = " update tbldepworking set amt_for_dep_calc  = opassetgross  where id > 0 ";

                MySqlCommand cmdUPdate_amt = new MySqlCommand(strUpdate, connection);
                cmdUPdate_amt.ExecuteNonQuery();




                // start calculation 


                string strSQL = "";

                strSQL = "select * from tbldepworking where depmethod = 'SLM' and disp_id = 0";

                DataTable dtAssetsWorking = getDataTable(strSQL);


                if (dtAssetsWorking.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAssetsWorking.Rows)
                    {
                        decimal amountcapitalised = 0;
                        decimal opaccdepreciation = 0;
                        decimal disptillstdt = 0;
                        decimal deptillstdt = 0;
                        decimal dep_rev_tillstdt = 0;
                        decimal opassetgross = 0;
                        decimal opdep = 0;
                        decimal amt_for_dep_calc = 0;
                        string depmethod = "SLM";
                        decimal deprate = 0;
                        decimal depmamt = 0;
                        decimal dep_to_be_reversed = 0;
                        decimal residualvalue = 0;

                        int AssetID = 0;
                        int UseFullLife = 0;
                        int id = 0;

                        string assetno = "";

                        DateTime AssetExpiryDate = startDate;
                        DateTime DtPuttoUse = startDate;
                        DateTime eff_fromdate = startDate;
                        DateTime eff_todate = enddate;
                        int DisposalFlag = 0;


                        id = Convert.ToInt32(dr["id"].ToString());
                        AssetID = Convert.ToInt32(dr["assetid"].ToString());


                        string strLog = "Insert into tbldepreciation_log (assetid,message) ";
                        strLog = strLog + " Values (" + AssetID + ", 'Started Processing'";
                        strLog = strLog + ")";

                        MySqlCommand cmd_log = new MySqlCommand(strLog, connection);
                        cmd_log.ExecuteNonQuery();


                        DisposalFlag = Convert.ToInt32(dr["DisposalFlag"].ToString());
                        UseFullLife = Convert.ToInt32(dr["usefulllife"].ToString());
                        amountcapitalised = Convert.ToDecimal(dr["amountcapitalised"].ToString());
                        opaccdepreciation = Convert.ToDecimal(dr["opaccdepreciation"].ToString());
                        disptillstdt = Convert.ToDecimal(dr["disptillstdt"].ToString());
                        deptillstdt = Convert.ToDecimal(dr["deptillstdt"].ToString());
                        dep_rev_tillstdt = Convert.ToDecimal(dr["dep_rev_tillstdt"].ToString());
                        opassetgross = Convert.ToDecimal(dr["opassetgross"].ToString());
                        opdep = Convert.ToDecimal(dr["opdep"].ToString());
                        amt_for_dep_calc = Convert.ToDecimal(dr["amt_for_dep_calc"].ToString());
                        depmethod = dr["depmethod"].ToString();
                        deprate = Convert.ToDecimal(dr["deprate"].ToString());
                        depmamt = Convert.ToDecimal(dr["depmamt"].ToString());
                        dep_to_be_reversed = Convert.ToDecimal(dr["dep_to_be_reversed"].ToString());

                        eff_fromdate = Convert.ToDateTime(dr["eff_fromdate"].ToString());
                        eff_todate = Convert.ToDateTime(dr["eff_todate"].ToString());

                        assetno = dr["assetno"].ToString();
                        // depmethod = dr["depmethod"].ToString();

                        residualvalue = Convert.ToDecimal(dr["residualvalue"].ToString());

                        if (residualvalue> 0)
                        {
                            int ix = 0;
                        }

                        //System.Diagnostics.Debug.WriteLine("AssetNo " + assetno);

                        if (AssetID == 13487)
                        {
                            int j = 0;
                        }


                        try
                        {

                            DtPuttoUse = Convert.ToDateTime(dr["DtPuttoUse"].ToString()); ;
                            AssetExpiryDate = DtPuttoUse.AddYears(UseFullLife); // Calculate Asset Expiry Date


                        }
                        catch (Exception ex)
                        {
                            DtPuttoUse = startDate; ;
                            AssetExpiryDate = DtPuttoUse;
                        }



                        int NoOfDays = 0;

                        if (DtPuttoUse > eff_fromdate)
                        {
                            eff_fromdate = DtPuttoUse;
                            NoOfDays = (eff_todate - eff_fromdate).Days + 1;
                        }
                        else
                        {
                            NoOfDays = (eff_todate - eff_fromdate).Days + 1;
                        }

                      

                        if (amt_for_dep_calc > 0)
                        {



                            depmamt = ((amt_for_dep_calc * deprate / 100) / Days_In_Year) * NoOfDays;
                            depmamt = Math.Round(depmamt, 2);



                            
                            if ((depmamt + opdep + residualvalue) > (amountcapitalised - disptillstdt))
                            {
                                depmamt = (amountcapitalised - disptillstdt) - (opdep + residualvalue);

                            }

                            if ((depmamt + opdep) > (amountcapitalised - disptillstdt))
                            {
                                depmamt = (amountcapitalised - disptillstdt) - (opdep);

                            }


                            if (depmamt < 0)
                            {
                                depmamt = 0;
                            }

                            if (DisposalFlag == 1)
                            {
                                depmamt = 0;
                            }
                            strUpdate = "";
                            strUpdate = " update tbldepworking set depmamt  = " + depmamt + " where id = " + id;

                            MySqlCommand cmdUPdate_dep = new MySqlCommand(strUpdate, connection);
                            cmdUPdate_dep.ExecuteNonQuery();



                            // rule 1 


                            /*todo insert into tbldepreciaton  mandar 05 dec 2021 */



                            String strSQLInsert = "INSERT INTO tbldepreciation";
                            strSQLInsert = strSQLInsert + "(AssetID,Assetname,FromDate, ";
                            strSQLInsert = strSQLInsert + " ToDate,DepreciationType,Amount,NormalRate,";
                            strSQLInsert = strSQLInsert + " AdditionRate,TotalRate, DepreciationDays,ClientID,DepreciationMethod,Companyid,";
                            strSQLInsert = strSQLInsert + " Assetno)";
                            strSQLInsert = strSQLInsert + " Values  (";
                            strSQLInsert = strSQLInsert + AssetID + ",";
                            strSQLInsert = strSQLInsert + "''" + ",";
                            strSQLInsert = strSQLInsert + "'" + eff_fromdate.ToString("yyyy-MM-dd") + "'" + ",";
                            strSQLInsert = strSQLInsert + "'" + eff_todate.ToString("yyyy-MM-dd") + "'" + ",";
                            strSQLInsert = strSQLInsert + "'A'" + ",";
                            strSQLInsert = strSQLInsert + depmamt + ",";
                            strSQLInsert = strSQLInsert + deprate + ",";
                            strSQLInsert = strSQLInsert + 0 + ",";
                            strSQLInsert = strSQLInsert + deprate + ",";
                            strSQLInsert = strSQLInsert + NoOfDays + ",";
                            strSQLInsert = strSQLInsert + "1 ,";
                            strSQLInsert = strSQLInsert + "'" + depmethod + "'" + ",";
                            strSQLInsert = strSQLInsert + "1, ";
                            strSQLInsert = strSQLInsert + "'" + assetno + "'";
                            strSQLInsert = strSQLInsert + ")";

                            MySqlCommand cmdUPdate_dep_inesrt = new MySqlCommand(strSQLInsert, connection);
                            cmdUPdate_dep_inesrt.ExecuteNonQuery();

                        }




                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(" Error in StartCalc_WithoutDisposal_SLM");
                logger.Error(ex);

                throw ex;
               // System.Diagnostics.Debug.WriteLine("error" + ex.Message);
            }

        }

        private void StartCalc_WithDisposal_V1_SLM(DateTime startDate, DateTime enddate)
        {

            int Days_In_Year = 365;
            Days_In_Year = GetDaysInYear(startDate, enddate);
           // MySqlTransaction myTrans = null;

            try
            {

                logger.Log(LogLevel.Info, " StartCalc_WithDisposal_V1_SLM...");

                //  myTrans = connection.BeginTransaction();


                //string strSQLInserAsset = "insert into tbldepworking (assetid,disp_id)";
                //strSQLInserAsset = strSQLInserAsset + " select distinct assetid,-1 from tbldisposal  ";
                //strSQLInserAsset = strSQLInserAsset + " where DisposalDate >='" + startDate.ToString("yyyy-MM-dd") + "'";
                //strSQLInserAsset = strSQLInserAsset + " and DisposalDate <='" + enddate.ToString("yyyy-MM-dd") + "' ";


                string strSQLInserAsset = "insert into tbldepworking (assetid,disp_id)";
                strSQLInserAsset = strSQLInserAsset + " select distinct assetid,-1 from tbldisposal  ";
                strSQLInserAsset = strSQLInserAsset + " inner join tblassets on  tbldisposal.assetid = tblassets.id ";
                strSQLInserAsset = strSQLInserAsset + " where  tblassets.DepreciationMethod = 'SLM'";
                strSQLInserAsset = strSQLInserAsset + " and DisposalDate >='" + startDate.ToString("yyyy-MM-dd") + "'";
                strSQLInserAsset = strSQLInserAsset + " and DisposalDate <='" + enddate.ToString("yyyy-MM-dd") + "' ";



                MySqlCommand cmdInsert_working = new MySqlCommand(strSQLInserAsset, connection);
              //  cmdInsert_working.Transaction = myTrans;

                cmdInsert_working.ExecuteNonQuery();


                string strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join tblassets asset ";
                strUpdate = strUpdate + " on working.assetid = asset.id ";
                strUpdate = strUpdate + " set working.assetno = asset.assetno , ";
                strUpdate = strUpdate + " working.DtPutToUse = asset.DtPutToUse , ";
                strUpdate = strUpdate + " working.usefulllife = asset.Usefullife , ";
                strUpdate = strUpdate + " working.amountcapitalised = asset.amountcapitalised , ";
                strUpdate = strUpdate + " working.opaccdepreciation = asset.opaccdepreciation , ";
                strUpdate = strUpdate + " working.depmethod = asset.DepreciationMethod , ";
                strUpdate = strUpdate + " working.residualvalue = asset.ResidualVal , ";

                strUpdate = strUpdate + " working.DisposalFlag = asset.DisposalFlag , ";


                strUpdate = strUpdate + " working.fromdate = '" + startDate.ToString("yyyy-MM-dd") + "' , ";
                strUpdate = strUpdate + " working.todate = '" + enddate.ToString("yyyy-MM-dd") + "' ,";


                strUpdate = strUpdate + " working.eff_fromdate = '" + startDate.ToString("yyyy-MM-dd") + "' , ";
                strUpdate = strUpdate + " working.eff_todate = '" + enddate.ToString("yyyy-MM-dd") + "' ";

                strUpdate = strUpdate + " where working.id > 0 ";

                MySqlCommand cmdUPdate = new MySqlCommand(strUpdate, connection);
                //cmdUPdate.Transaction = myTrans;
                cmdUPdate.ExecuteNonQuery();




                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(Amount),0) as deptillstdt";
                strUpdate = strUpdate + "  from tbldepreciation";
                strUpdate = strUpdate + "  where tbldepreciation.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldepreciation.FromDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as depreciation ON";
                strUpdate = strUpdate + "  working.assetid = depreciation.assetid";
                strUpdate = strUpdate + " set working.deptillstdt = depreciation.deptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                cmdUPdate = new MySqlCommand(strUpdate, connection);
              //  cmdUPdate.Transaction = myTrans;
                cmdUPdate.ExecuteNonQuery();


                // update  disptillstdt

                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(DisposalAmount),0) as disptillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.disptillstdt = disposal.disptillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_disposal = new MySqlCommand(strUpdate, connection);
               // cmdUPdate_disposal.Transaction = myTrans;
                cmdUPdate_disposal.ExecuteNonQuery();

                // add new column in tbldisposal form depreciationreversal - done // CHANGED DATABASE



                // update dep reversed till date 

                strUpdate = "";
                strUpdate = " update tbldepworking working ";
                strUpdate = strUpdate + " inner join ( ";
                strUpdate = strUpdate + " select assetid,IFNULL(sum(OpAccumulatedDep),0) as disp_dep_reversal_tillstdt";
                strUpdate = strUpdate + "  from tbldisposal";
                strUpdate = strUpdate + "  where tbldisposal.companyid = 1 ";
                strUpdate = strUpdate + "  and  tbldisposal.DisposalDate < '" + startDate.ToString("yyyy-MM-dd") + "'";
                strUpdate = strUpdate + "  group by assetid";
                strUpdate = strUpdate + " ) as disposal ON";
                strUpdate = strUpdate + "  working.assetid = disposal.assetid";
                strUpdate = strUpdate + " set working.dep_rev_tillstdt = disposal.disp_dep_reversal_tillstdt ";
                strUpdate = strUpdate + " where working.id > 0 ";


                MySqlCommand cmdUPdate_dep_reverse = new MySqlCommand(strUpdate, connection);
                //cmdUPdate_dep_reverse.Transaction = myTrans;
                cmdUPdate_dep_reverse.ExecuteNonQuery();


                //



                strUpdate = "";
                strUpdate = strUpdate + " update tbldepworking working ";
                strUpdate = strUpdate + " inner join tblassets assets on working.assetid = assets.id ";
                strUpdate = strUpdate + " set working.depmethod = assets.DepreciationMethod , ";
                strUpdate = strUpdate + " working.deprate = assets.TotalRate ";
                strUpdate = strUpdate + " where working.id > 0 ";

                MySqlCommand cmdUPdate_rate = new MySqlCommand(strUpdate, connection);
              //  cmdUPdate_rate.Transaction = myTrans;
                cmdUPdate_rate.ExecuteNonQuery();



                // update asset op gross value 

                //update opassetgross set = amountcapitalised - disptillstdt where id > 0

                strUpdate = "";
                strUpdate = " update tbldepworking set opassetgross  = (amountcapitalised - disptillstdt) where id > 0 ";


                MySqlCommand cmdUPdate_opgross = new MySqlCommand(strUpdate, connection);
              //  cmdUPdate_opgross.Transaction = myTrans;
                cmdUPdate_opgross.ExecuteNonQuery();



                strUpdate = "";
                strUpdate = " update tbldepworking set opdep  = (opaccdepreciation + deptillstdt) - (dep_rev_tillstdt) where id > 0 ";

                MySqlCommand cmdUPdate_opdep = new MySqlCommand(strUpdate, connection);
             //   cmdUPdate_opdep.Transaction = myTrans;
                cmdUPdate_opdep.ExecuteNonQuery();


                strUpdate = "";
                strUpdate = " update tbldepworking set amt_for_dep_calc  = (opassetgross) where id > 0 ";

                MySqlCommand cmdUPdate_amt = new MySqlCommand(strUpdate, connection);
              //  cmdUPdate_amt.Transaction = myTrans;
                cmdUPdate_amt.ExecuteNonQuery();






                // start calculation 


                string strSQL;


                DataTable dtDepWorking = new DataTable();
                strSQL = "Select * from tbldepworking where disp_id = -1 and depmethod = 'SLM'";
                dtDepWorking = getDataTable(strSQL);


                if (dtDepWorking.Rows.Count > 0)
                {

                    foreach (DataRow dr_working in dtDepWorking.Rows)
                    {

                        decimal amountcapitalised = 0;
                        decimal opaccdepreciation = 0;
                        decimal disptillstdt = 0;
                        decimal deptillstdt = 0;
                        decimal dep_rev_tillstdt = 0;
                        decimal opassetgross = 0;
                        decimal opdep = 0;
                        decimal amt_for_dep_calc = 0;
                        string depmethod = "SLM";
                        decimal deprate = 0;
                        decimal depmamt = 0;
                        decimal dep_to_be_reversed = 0;

                        decimal residualvalue = 0;
                        decimal disposal_amt = 0; // from disposal table
                        string assetno = "";

                        int AssetID = 0;
                        int UseFullLife = 0;
                        int id = 0;
                        int DisposalFlag = 0;

                        DateTime AssetExpiryDate = startDate;
                        DateTime DtPuttoUse = startDate;

                        DateTime eff_fromdate = startDate;
                        DateTime eff_todate = enddate;

                        id = Convert.ToInt32(dr_working["id"].ToString());
                        AssetID = Convert.ToInt32(dr_working["assetid"].ToString());
                        DisposalFlag = Convert.ToInt32(dr_working["DisposalFlag"].ToString());



                        string strLog = "Insert into tbldepreciation_log (assetid,message) ";
                        strLog = strLog + " Values (" + AssetID + ", 'Started Processing'";
                        strLog = strLog + ")";

                        MySqlCommand cmd_log = new MySqlCommand(strLog, connection);
                        cmd_log.ExecuteNonQuery();


                        if (AssetID == 10634)
                        {
                            int j = 0;
                        }

                        DtPuttoUse = Convert.ToDateTime(dr_working["DtPuttoUse"].ToString());

                        if (DtPuttoUse > startDate)
                        {
                            eff_fromdate = DtPuttoUse;
                        }


                        //disp_id = Convert.ToInt32(dr_working["disp_id"].ToString()); // disposal record id from disposal table

                        UseFullLife = Convert.ToInt32(dr_working["usefulllife"].ToString());
                        amountcapitalised = Convert.ToDecimal(dr_working["amountcapitalised"].ToString()); // from asset 
                        opaccdepreciation = Convert.ToDecimal(dr_working["opaccdepreciation"].ToString()); // from asset

                        depmethod = dr_working["depmethod"].ToString();
                        deprate = Convert.ToDecimal(dr_working["deprate"].ToString());
                        eff_fromdate = Convert.ToDateTime(dr_working["eff_fromdate"].ToString());
                        eff_todate = Convert.ToDateTime(dr_working["eff_todate"].ToString());

                        disptillstdt = Convert.ToDecimal(dr_working["disptillstdt"].ToString());

                        deptillstdt = Convert.ToDecimal(dr_working["deptillstdt"].ToString());

                        dep_rev_tillstdt = Convert.ToDecimal(dr_working["dep_rev_tillstdt"].ToString());

                        opassetgross = Convert.ToDecimal(dr_working["opassetgross"].ToString());
                        opdep = Convert.ToDecimal(dr_working["opdep"].ToString());

                        disposal_amt = Convert.ToDecimal(dr_working["disposalamt"].ToString());


                        assetno = dr_working["assetno"].ToString();

                        System.Diagnostics.Debug.WriteLine("AssetNo " + assetno);


                        residualvalue = Convert.ToDecimal(dr_working["residualvalue"].ToString());


                        decimal temp_GrossAmt = 0;
                        decimal temp_Dep_TillStartDate = 0;

                        temp_GrossAmt = (amountcapitalised - disptillstdt);

                        temp_Dep_TillStartDate = (opaccdepreciation + deptillstdt) - dep_rev_tillstdt;

                        //amt_for_dep_calc = (temp_GrossAmt - temp_Dep_TillStartDate);
                        //commented  by mandar 06 jan 2022 


                        amt_for_dep_calc = temp_GrossAmt;

                        DtPuttoUse = Convert.ToDateTime(dr_working["DtPuttoUse"].ToString()); ;
                        AssetExpiryDate = DtPuttoUse.AddYears(UseFullLife); // Calculate Asset Expiry Date

                        int NoOfDays_temp = 0;
                        NoOfDays_temp = (eff_todate - eff_fromdate).Days + 1;


                        depmamt = ((amt_for_dep_calc * deprate / 100) / Days_In_Year) * NoOfDays_temp;
                        depmamt = Math.Round(depmamt, 2);


                        // calculated dep to be reversed

                        decimal totaldep_till_date = (depmamt + temp_Dep_TillStartDate);

                        // formula  dep_to_be_reversed  = dep / (op gros blcok - all disposal till date) * disposal amt

                        strUpdate = " update tbldepworking set depmamt  = " + depmamt + " where id = " + id;

                        MySqlCommand cmdUPdate_dep2 = new MySqlCommand(strUpdate, connection);
                        cmdUPdate_dep2.ExecuteNonQuery();



                        strUpdate = " update tbldisposal set OpAccumulatedDep  = " + dep_to_be_reversed + ", OpAccumulatedDep = " + dep_to_be_reversed + " where ID = " + id;

                        cmdUPdate_dep2 = new MySqlCommand(strUpdate, connection);
                        cmdUPdate_dep2.ExecuteNonQuery();


                        // check disposal for the asset 

                        strSQL = "";
                        strSQL = " select * from tbldisposal ";
                        strSQL = strSQL + "  where tbldisposal.companyid = 1 ";
                        strSQL = strSQL + "  and  tbldisposal.AssetId =" + AssetID;
                        strSQL = strSQL + "  and  tbldisposal.DisposalDate >='" + startDate.ToString("yyyy-MM-dd") + "'";
                        strSQL = strSQL + "  and  tbldisposal.DisposalDate <='" + enddate.ToString("yyyy-MM-dd") + "'";


                        DataTable dtDisposal = new DataTable();
                        dtDisposal = getDataTable(strSQL);
                        // decimal TotalReversal = 0;
                        // decimal Total_Extra_Dep_ToBeReduced = 0;

                        int disp_id = 0;
                        decimal disposal_during_period = 0;
                        decimal total_dep_reduction = 0;
                        decimal total_dep = 0;

                        foreach (DataRow disp_Row in dtDisposal.Rows)
                        {

                            disp_id = Convert.ToInt32(disp_Row["ID"].ToString());

                            decimal disposalAmt = Convert.ToDecimal(disp_Row["DisposalAmount"].ToString());
                            disposal_during_period = disposal_during_period + disposalAmt;

                            DateTime disposalDate = Convert.ToDateTime(disp_Row["DisposalDate"].ToString());
                            decimal OpAsset = amountcapitalised - disptillstdt;

                            decimal DepReversal_1 = Math.Round((temp_Dep_TillStartDate * disposalAmt) / OpAsset, 2);

                            int Reverse_days = (disposalDate - startDate).Days;
                            int extra_days = (enddate - disposalDate).Days + 1;

                            // decimal temp_amt = (disposalAmt - DepReversal_1);

                            decimal temp_amt = (disposalAmt);

                            //decimal DepReversal_2 = (temp_amt) / OpAsset;

                            decimal DepReversal_2 = Math.Round(((temp_amt * deprate / 100) / Days_In_Year) * Reverse_days, 2);


                            decimal total_temp_reversal = DepReversal_1 + DepReversal_2;


                            //decimal extra_dep_reversal = 0 - ((disposalAmt - DepReversal_1) * deprate / 100 / Days_In_Year) * extra_days;
                            decimal extra_dep_reversal = 0 - ((disposalAmt) * deprate / 100 / Days_In_Year) * extra_days;
                            extra_dep_reversal = Math.Round(extra_dep_reversal, 2);
                            total_dep = total_dep + extra_dep_reversal;



                            total_dep_reduction = total_dep_reduction + total_temp_reversal;

                            string strSQL_insert_temp = "";

                            strSQL_insert_temp = "insert into tbldepworking (assetid,depmamt,dep_to_be_reversed,disp_id) values ( ";
                            strSQL_insert_temp = strSQL_insert_temp + AssetID + "," + extra_dep_reversal + "," + total_temp_reversal + "," + disp_id + ")";

                            MySqlCommand cmdUPdate_dep_inesrt_1 = new MySqlCommand(strSQL_insert_temp, connection);
                            cmdUPdate_dep_inesrt_1.ExecuteNonQuery();


                            /// update reversal in disposal table
                            /// TODO : Mandar 16 DEC 2021
                            /// 
                            strSQL = "";


                            //if opgross  - depmamt till date - total of dep  on disposals in current period <= 0

                            if ((opassetgross - opdep  - total_dep) <=0 )
                            {
                                strSQL = " update tbldisposal set OpAccumulatedDep  = " + 0 + ", OpAccumulatedDep = " + 0 + " where ID = " + disp_id;
                                cmdUPdate_dep_inesrt_1 = new MySqlCommand(strSQL, connection);
                                cmdUPdate_dep_inesrt_1.ExecuteNonQuery();

                            }
                            else
                            {
                                strSQL = " update tbldisposal set OpAccumulatedDep  = " + total_temp_reversal + ", OpAccumulatedDep = " + total_temp_reversal + " where ID = " + disp_id;

                                cmdUPdate_dep_inesrt_1 = new MySqlCommand(strSQL, connection);
                                cmdUPdate_dep_inesrt_1.ExecuteNonQuery();


                            }



                        }

                        //
                        //   deprate 





                        //   *todo insert into tbldepreciaton  mandar 05 dec 2021 */


                        strSQL = "";
                        strSQL = "select  IFNULL(sUM(depmamt),0) depamt from tbldepworking where assetid = " + AssetID;

                        DataTable dtDepTemp = new DataTable();

                        dtDepTemp = getDataTable(strSQL);

                        decimal final_dep_amt = 0;

                        // final_dep_amt = total_dep;

                        final_dep_amt = Convert.ToDecimal(dtDepTemp.Rows[0][0]);

                        decimal check_diff = (final_dep_amt + opdep + residualvalue - total_dep_reduction);
                        decimal check_diff1 = (amountcapitalised - disptillstdt - disposal_during_period);


                        if ((final_dep_amt + opdep + residualvalue - total_dep_reduction) > (amountcapitalised - disptillstdt - disposal_during_period))
                        {
                            final_dep_amt = (amountcapitalised - disptillstdt - disposal_during_period) - (opdep + residualvalue);

                        }
                        //else if ((final_dep_amt + opdep + residualvalue - total_dep_reduction) == (amountcapitalised - disptillstdt - disposal_during_period))
                        //{
                        //    final_dep_amt = (amt_for_dep_calc - opdep);

                        // commented 01 nov 2022 galleghar issue 
                        //}
                        
                        else if (((check_diff >= -0.99M) && (check_diff <= 0.99M)) && ((check_diff1 >= -0.99M) && (check_diff1 <= 0.99M)))
                        {
                            final_dep_amt = 0;
                        }

                        /*
                        if (((amountcapitalised - disptillstdt) - opdep) < depmamt)
                        {
                            depmamt = ((amountcapitalised - disptillstdt) - opdep);
                        }
                        */

                        //if (residualvalue > 0)
                        //{

                        //    if ((depmamt  + opdep + residualvalue) > (amountcapitalised - disptillstdt))
                        //    {
                        //        depmamt = (amountcapitalised - disptillstdt) - (  opdep + residualvalue);

                        //    }

                        //}
                        //else
                        //{
                        //    if ((depmamt  + opdep) > (amountcapitalised - disptillstdt))
                        //    {
                        //        depmamt = amountcapitalised - ( opdep);
                        //    }
                        //}


                        if (final_dep_amt < 0)
                        {
                            final_dep_amt = 0;
                        }

                        //if (DisposalFlag ==1)
                        //{
                        //    final_dep_amt = 0;
                        //}


                        // TO DO 
                        // Check to be implemented for Depreciation


                        // 1) check if Residual Value 



                        // 2) total depreaciaton should not be more than amt capitalized



                        // 3) 



                        String strSQLInsert = "INSERT INTO tbldepreciation";
                        strSQLInsert = strSQLInsert + "(AssetID,Assetname,FromDate, ";
                        strSQLInsert = strSQLInsert + " ToDate,DepreciationType,Amount,NormalRate,";
                        strSQLInsert = strSQLInsert + " AdditionRate,TotalRate, DepreciationDays,ClientID,DepreciationMethod,Companyid";
                        strSQLInsert = strSQLInsert + "   )";
                        strSQLInsert = strSQLInsert + " Values  (";
                        strSQLInsert = strSQLInsert + AssetID + ",";
                        strSQLInsert = strSQLInsert + "''" + ",";
                        strSQLInsert = strSQLInsert + "'" + startDate.ToString("yyyy-MM-dd") + "'" + ",";
                        strSQLInsert = strSQLInsert + "'" + enddate.ToString("yyyy-MM-dd") + "'" + ",";
                        strSQLInsert = strSQLInsert + "'A'" + ",";
                        strSQLInsert = strSQLInsert + final_dep_amt + ",";
                        strSQLInsert = strSQLInsert + deprate + ",";
                        strSQLInsert = strSQLInsert + 0 + ",";
                        strSQLInsert = strSQLInsert + deprate + ",";
                        strSQLInsert = strSQLInsert + NoOfDays_temp + ",";
                        strSQLInsert = strSQLInsert + "1 ,";
                        strSQLInsert = strSQLInsert + "'" + depmethod + "'" + ",";
                        strSQLInsert = strSQLInsert + "1 ";
                        
                        strSQLInsert = strSQLInsert + ")";

                        MySqlCommand cmdUPdate_dep_inesrt = new MySqlCommand(strSQLInsert, connection);
                       // cmdUPdate_dep_inesrt.Transaction = myTrans;
                        cmdUPdate_dep_inesrt.ExecuteNonQuery();

                    }

                }

               // myTrans.Commit();
            }
            catch (Exception ex)
            {
                logger.Error(" Error in StartCalc_WithDisposal_V1_SLM");
                logger.Error(ex);

                throw ex;
              //  myTrans.Rollback();
                //DtPuttoUse = startDate; ;
                //AssetExpiryDate = DtPuttoUse;
            }

        }

        private int GetDaysInYear(DateTime dtStartDate, DateTime dtEndDate)
        {
            int temp_days = 365;

            logger.Log(LogLevel.Info, " GetDaysInYear ");



            string strGetDaysInYear = "select ID, FromDate, ToDate from tblperiod";
            strGetDaysInYear = strGetDaysInYear + " where id in (select periodid from tblsubperiod where FromDate >= '" + dtStartDate.ToString("yyyy-MM-dd") + "'";
            strGetDaysInYear = strGetDaysInYear + " and ToDate <= '" + dtEndDate.ToString("yyyy-MM-dd") + "')";


            DataTable dtPeriod = getDataTable(strGetDaysInYear);
            if (dtPeriod.Rows.Count > 0)
            {
                DateTime periodStartDate = Convert.ToDateTime(dtPeriod.Rows[0]["FromDate"]);
                DateTime periodEndDate = Convert.ToDateTime(dtPeriod.Rows[0]["ToDate"]);

                temp_days = (periodEndDate - periodStartDate).Days + 1;

            }

            return temp_days;
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
