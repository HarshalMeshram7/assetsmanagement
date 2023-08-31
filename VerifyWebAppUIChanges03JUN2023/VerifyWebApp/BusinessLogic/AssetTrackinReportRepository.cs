using System;
using System.Collections.Generic;

using MySql.Data;
using MySql.Data.MySqlClient;

using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.BusinessLogic
{
    public class AssetTrackinReportRepository
    {
        public VerifyDB db = new VerifyDB();
        public List<AssetTrackingViewmodel> getAssettracking(int companyid, DateTime? Startdate, DateTime?
            Enddate, string fromassetno, string toassetno, int alocId)
        {
            string strSQL = null;
            string str_comid = companyid.ToString();
            string alocid = "0";
            string conv_startdate = null;
            string conv_enddate = null;
            // if (Startdate != "") {
            //   DateTime startdate = Convert.ToDateTime(Startdate);
            if (Startdate != null)
            {
                conv_startdate = Convert.ToDateTime(Startdate).ToString("yyyy-MM-dd");
            }
            //}

            //     if (Enddate != "")
            //   {
            //     DateTime enddate = Convert.ToDateTime(Enddate);
            if (Enddate != null)
            {
                conv_enddate = Convert.ToDateTime(Enddate).ToString("yyyy-MM-dd");
            }
            //}
            if (alocId != 0)
            {
                alocid = alocId.ToString();
            }

            int i_FromAssetNo=0;
            int i_ToAssetNo = 0;


            if (fromassetno == "" )
            {
                i_FromAssetNo = 0;
            }
            else
            {
                i_FromAssetNo = Convert.ToInt32(fromassetno);
            }
            if (toassetno == "")
            {
                i_ToAssetNo = 0;
            }else
            {
                i_ToAssetNo = Convert.ToInt32(toassetno);
            }

            if (conv_startdate == null)
            {
                conv_startdate = "null";
            }
            if (conv_enddate == null)
            {
                conv_enddate = "null";
            }

            /*
             * 
            object[] xparams = {
            new SqlParameter("@ParametterWithNummvalue", DBNull.Value),
            new SqlParameter("@In_Parameter", "Value"),
            new SqlParameter("@Out_Parameter", SqlDbType.Int) {Direction = ParameterDirection.Output}};

                YourDbContext.Database.ExecuteSqlCommand("exec StoreProcedure_Name @ParametterWithNummvalue, @In_Parameter, @Out_Parameter", xparams);
                var ReturnValue = ((SqlParameter)params[2]).Value;   

             */

            strSQL = "";
            strSQL = "Call assettrackingreport(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + conv_startdate + "'," + "'" + conv_enddate + "',";
            strSQL = strSQL + "'" + fromassetno + "'," + "'" + toassetno + "',";
            strSQL = strSQL + alocid + ")";


            //var result = db.Database.SqlQuery<AssetTrackingViewmodel>(strSQL).ToList();

            object[] xparams = {
            new MySqlParameter("@In_companyid", companyid),
            new MySqlParameter("@In_FromDate", Startdate),
            new MySqlParameter("@In_ToDate",Enddate ),
            new MySqlParameter("@In_FromAssetNo",i_FromAssetNo ),
            new MySqlParameter("@In_ToAssetNo",i_ToAssetNo ),
            new MySqlParameter("@In_LocationID",alocid ),
            };

            strSQL = "call  assettrackingreport (@In_companyid, @In_FromDate, @In_ToDate,@In_FromAssetNo,@In_ToAssetNo,@In_LocationID)";

            var result = db.Database.SqlQuery<AssetTrackingViewmodel>(strSQL, xparams).ToList();



            foreach (AssetTrackingViewmodel item in result)
            {
                if (item.IssueDate != null)
                {
                    item.str_IssueDate = item.IssueDate.ToString("dd/MM/yyyy");
                }
                if (item.alocid != 0)
                {
                    item.ALocName = db.ALocations.Where(x => x.Companyid == companyid && x.ID == item.alocid).FirstOrDefault().ALocationName;
                }

            }

            return result;

        }
        public List<EmployeeAssetReport> getEmployeeAssettracking(int companyid,/* DateTime? Startdate, DateTime? Enddate,*/ string fromassetno, string toassetno, string str_empid)
        {
            string strSQL = null;
            string str_comid = companyid.ToString();

            if (str_empid == "0")
            {
                str_empid = "";
            }
            string empid = "";

            if (str_empid != "")
            {
                empid = db.Employee.Where(x => x.Companyid == companyid && x.EmpId == str_empid).FirstOrDefault().ID.ToString();
            }
            else
            {
                empid = "0";
            }
            //by me
            //string conv_startdate = "";
            //string conv_enddate = "";
            //if (Startdate != null)
            //{
            //    conv_startdate = Convert.ToDateTime(Startdate).ToString("yyyy-MM-dd");
            //}
            //=============
            //}

            //     if (Enddate != "")
            //   {
            //     DateTime enddate = Convert.ToDateTime(Enddate);
            // by me
            //if (Enddate != null)
            //{
            //    conv_enddate = Convert.ToDateTime(Enddate).ToString("yyyy-MM-dd");
            //}
            //======



            //Commented by Mandar 26 MAR 2022

            strSQL = "";
            strSQL = "Call employeeassettrackingreport(";
            strSQL = strSQL + companyid + ",";
            //strSQL = strSQL + "'" + conv_startdate + "'," + "'" + conv_enddate + "',";
            strSQL = strSQL + "'" + fromassetno + "'," + "'" + toassetno + "',";
            strSQL = strSQL + empid + ")";


            var result = db.Database.SqlQuery<EmployeeAssetReport>(strSQL).ToList();
            // by me
            foreach (EmployeeAssetReport item in result)
            {
                if (item.IssueDate != null)
                {
                    item.str_IssueDate = item.IssueDate.ToString("dd/MM/yyyy");
                }
                if (item.employeeid != 0)
                {
                    item.EmpName = db.Employee.Where(x => x.Companyid == companyid && x.ID == item.employeeid).FirstOrDefault().FullName;
                }
            }

            return result;


        }
    }
}