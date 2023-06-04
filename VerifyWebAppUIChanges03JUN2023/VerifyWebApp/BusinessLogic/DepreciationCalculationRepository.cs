using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyDepCalculator;

namespace VerifyWebApp.BusinessLogic
{
    public class DepreciationCalculationRepository
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public VerifyDB db = new VerifyDB();
        public List<Depreciation> Getdepcal(int companyid, string Startdate, string Enddate, int userid)
        {
            string strSQL = null;
            string str_comid = companyid.ToString();
            DateTime dtstartdate = Convert.ToDateTime(Startdate);
            DateTime dtenddate = Convert.ToDateTime(Enddate);

            string Userid = userid.ToString();

            string startdate = Convert.ToDateTime(dtstartdate).ToString("yyyy/MM/dd");
            string enddate = Convert.ToDateTime(dtenddate).ToString("yyyy/MM/dd");


            strSQL = "";
            strSQL = "Call usp_calc_depriciationV1(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + startdate + "'," + "'" + enddate + "',";

            strSQL = strSQL + Userid + ")";


            var result = db.Database.SqlQuery<Depreciation>(strSQL).ToList();
            foreach (Depreciation item in result)
            {
                if (item.FromDate != null)
                {
                    item.str_FromDate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
                }
                if (item.ToDate != null)
                {
                    item.str_ToDate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy");
                }
                item.AssetNo = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;

            }
            return result;
        }

        public bool StartCalculationRequest(int companyid, DateTime Startdate, DateTime Enddate, int userid)
        {


            DateTime dtstartdate;
            DateTime dtenddate;

            try
            {


                DepreciationRequest request = new DepreciationRequest();

                request.CompanyID = companyid;
                request.UserID = userid;
                request.StartDate = Startdate;
                request.EndDate = Enddate;
                request.InProcess = -1;

                db.DepreciationRequest.Add(request);
                db.SaveChanges();

                string strConnString = db.Database.Connection.ConnectionString;

                VerifyDepCalculator.CompanyLawDepreciation companyLaw = new CompanyLawDepreciation(strConnString);

                companyLaw.logger = logger;

                companyLaw.StartCalculation();





                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool StartCalculationRequest_IncomeTax(int companyid, DateTime Startdate, DateTime Enddate, int userid)
        {


            DateTime dtstartdate;
            DateTime dtenddate;

            try
            {


                DepreciationRequestIncomeTax request = new DepreciationRequestIncomeTax();

                request.CompanyID = companyid;
                request.UserID = userid;
                request.StartDate = Startdate;
                request.EndDate = Enddate;
                request.InProcess = -1;

                db.DepreciationRequestIncomeTax.Add(request);
                db.SaveChanges();

                string strConnString = db.Database.Connection.ConnectionString;

                VerifyDepCalculator.IncomeTaxLawDepCalculator incomeTaxLawDepCalculator = new IncomeTaxLawDepCalculator(strConnString);

                incomeTaxLawDepCalculator.logger = logger;

                incomeTaxLawDepCalculator.StartCalculation();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /*
        public bool StartCalculationITDepreciation(int companyid, DateTime Startdate, DateTime Enddate, int userid, string cutoff)
        {



            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {



                try
                {

                    string strSQL = null;
                    string str_comid = companyid.ToString();
                    string str_fromdate = Startdate.ToString("yyyy-MM-dd");
                    string str_todate = Enddate.ToString("yyyy-MM-dd");

                    string str_cutoffdate = new DateTime(Startdate.Year, 9, 30).ToString("yyyy-MM-dd"); //  30 sept 2020

                    strSQL = "";
                    strSQL = "Call usp_ITDepreciationCalc(";
                    strSQL = strSQL + companyid + ",";
                    strSQL = strSQL + "'" + str_fromdate + "',";
                    strSQL = strSQL + "'" + str_todate + "',";
                    strSQL = strSQL + "'" + str_cutoffdate + "')";


                    List<ITDepreciation_Temp> lstData = new List<ITDepreciation_Temp>();


                    lstData = db.Database.SqlQuery<ITDepreciation_Temp>(strSQL).ToList();



                    foreach (var item in lstData)
                    {

                        decimal temp_disposal = item.Disposal;
                        decimal temp_opwdv_balance = item.Opwdv;
                        decimal temp_asset_with_more_than180_balance = item.Additionbefore; // additions before cut off date for FIFO 
                        decimal temp_asset_with_less_than180_balance = item.AdditionAfter; // additions after 30 th cut off date for FIFO 

                        decimal temp1 = 0;
                        decimal temp2 = 0;
                        decimal temp3 = 0;

                        decimal opwdv_dep = 0;
                        decimal asset_morethan180_dep = 0;
                        decimal asset_lessthan180_dep = 0;

                        decimal totaldep = 0;

                        if (item.Disposal > 0)  // if there is disposal 
                        {


                            temp1 = (temp_opwdv_balance - temp_disposal);

                            if (temp1 > 0)
                            {
                                temp_opwdv_balance = temp1; // no balance in disposal variable
                            }
                            else
                            {
                                temp_opwdv_balance = 0;
                                temp2 = (temp_asset_with_more_than180_balance - Math.Abs(temp1)); // balance amt in diposal shoudl be deducted from assets more than 180 days
                            }

                            if (temp2 > 0)
                            {
                                temp_asset_with_more_than180_balance = temp2;

                            }
                            else
                            {
                                temp_asset_with_more_than180_balance = 0;
                                temp3 = (temp_asset_with_less_than180_balance - Math.Abs(temp2));
                            }


                            if (temp3 > 0)
                            {

                                temp_asset_with_less_than180_balance = temp3;
                            }
                            else
                            {
                                temp_asset_with_less_than180_balance = 0;

                            }


                            // calculate dep on balances



                            if (temp_opwdv_balance > 0)
                            {

                                opwdv_dep = (temp_opwdv_balance * item.DepreciationRate / 100);
                            }


                            if (temp_asset_with_more_than180_balance > 0)
                            {
                                asset_morethan180_dep = (temp_asset_with_more_than180_balance * item.DepreciationRate / 100);
                            }

                            if (temp_asset_with_less_than180_balance > 0)
                            {
                                asset_lessthan180_dep = ((temp_asset_with_less_than180_balance * item.DepreciationRate / 100) / 2);
                            }


                        }
                        else
                        {
                            // TODO ??
                            opwdv_dep = (temp_opwdv_balance * item.DepreciationRate / 100);
                            asset_morethan180_dep = (temp_asset_with_more_than180_balance * item.DepreciationRate / 100);
                            asset_lessthan180_dep = ((temp_asset_with_less_than180_balance * item.DepreciationRate / 100) / 2);


                        }

                        // calcualte closing wdv

                        totaldep = opwdv_dep + asset_morethan180_dep + asset_lessthan180_dep;

                        item.DeponOPwdv = opwdv_dep;
                        item.DepBefore = asset_morethan180_dep;
                        item.DepAfter = asset_lessthan180_dep;
                        item.ClosingWDV = ((item.Opwdv + item.Additionbefore + item.AdditionAfter) - item.Disposal) - totaldep;





                        ITDepreciation dep = new ITDepreciation();

                        dep.Companyid = companyid;
                        dep.ITGroupID = item.itgroupid;
                        dep.GroupName = item.groupname;
                        dep.FromDate = item.FromDate;
                        dep.ToDate = item.ToDate;

                        dep.Opwdv = item.Opwdv;
                        dep.Additionbefore = item.Additionbefore;
                        dep.AdditionAfter = item.AdditionAfter;

                        dep.DeponOPwdv = item.DeponOPwdv;
                        dep.AdditionDepfull = item.DepBefore;
                        dep.AdditionDephalf = item.DepAfter;
                        dep.TotalDep = item.TotalDep;
                        dep.ClosingWDV = item.ClosingWDV;


                        db.ITDepreciation.Add(dep);

                    }

                    ITPeriod depPeriod = db.ITPeriods.Where(x => x.FromDate == Startdate
                    && x.ToDate == Enddate
                    && x.Companyid == companyid).FirstOrDefault();
                    if (depPeriod != null)
                    {
                        depPeriod.DepFlag = "Y";
                        db.Entry(depPeriod).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();
                    transaction.Commit();

                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    return false;
                    //int i = 0;
                    // TODO Log Error here
                }
            }



            return true;

        }
        */
    }
}