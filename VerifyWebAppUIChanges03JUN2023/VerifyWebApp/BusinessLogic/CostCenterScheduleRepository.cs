using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.BusinessLogic
{
    public class CostCenterScheduleRepository
    {
        public VerifyDB db = new VerifyDB();
        public List<CostCenterScheduleViewmodel> getCCSchedule(int companyid, DateTime FromDate, DateTime ToDate)
        {
            List<CostCenterScheduleViewmodel> clist = new List<CostCenterScheduleViewmodel>();
            string strSQL = null;
            string str_comid = companyid.ToString();
            string fromDate = FromDate.ToString("yyyy-MM-dd");
            string toDate = ToDate.ToString("yyyy-MM-dd");

            strSQL = "";
            strSQL = "Call usp_report_getCCSchedule(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + fromDate + "',";
            strSQL = strSQL + "'" + toDate + "')";


            List<Childlocation> loclist = new List<Childlocation>();
            loclist = db.childlocations.Where(x => x.Companyid == companyid).ToList();
            List<Childcostcenter> cclist = new List<Childcostcenter>();
            cclist = db.childcostcenters.Where(x => x.Companyid == companyid).ToList();
            clist = db.Database.SqlQuery<CostCenterScheduleViewmodel>(strSQL).OrderBy(x=>x.assetid).ToList();
            foreach (CostCenterScheduleViewmodel item in clist.ToList())
            {
                
              
                if (item.voucherDate != null)
                {
                    item.str_voucherdate = Convert.ToDateTime(item.voucherDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_voucherdate = "";
                }
                if (item.BillDate != null)
                {
                    item.str_billdate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_billdate = "";
                }
                if (item.DTPutUse != null)
                {
                    item.str_dateputtousedate = Convert.ToDateTime(item.DTPutUse).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_voucherdate = "";
                }

               /* if (item.OpGross==0 && item.Addition == 0 && item.ClGross == 0 && item.Disposal == 0 && item.DispoDep == 0 && item.UpToDep == 0
                    && item.TotDep == 0 && item.NetBalance == 0 && item.OpDep == 0)
                {
                    clist.Remove(item);
                }
                */

                if (loclist.Count > 0)
                {
                    //foreach (Childlocation locitem in loclist)
                    //{

                    //    if (item.assetid != locitem.AssetID)
                    //    {
                    //        clist.Remove(item);
                    //    }

                    //}
                    var checkassetexists = db.childlocations.Where(x => x.Companyid == companyid && x.AssetID == item.assetid).FirstOrDefault();
                    if (checkassetexists == null)
                    {
                   //     clist.Remove(item);
                    }

                    else
                    {
                        if (checkassetexists.Date > ToDate)
                        {
                           // clist.Remove(item);
                        }
                    }

                }

                if (cclist.Count > 0)
                {
                    //foreach (Childcostcenter ccitem in cclist)
                    //{

                    //    if (item.assetid != ccitem.Ass_ID)
                    //    {
                    //        clist.Remove(item);
                    //    }

                    //}

                    var checkassetexists = db.childcostcenters.Where(x => x.Companyid == companyid && x.Ass_ID == item.assetid).FirstOrDefault();
                    if (checkassetexists == null)
                    {
                     //   clist.Remove(item);
                    }
                    else
                    {
                        if (checkassetexists.Date > ToDate)
                        {
                        //    clist.Remove(item);
                        }
                        else
                        {
                            var intpercentage = Convert.ToInt32(checkassetexists.Percentage);
                            item.OpGross = item.OpGross * (intpercentage / 100);
                            item.Addition = item.Addition * (intpercentage / 100);
                            item.ClGross = item.ClGross * (intpercentage / 100);
                            item.Disposal = item.Disposal * (intpercentage / 100);
                            item.OpDep = item.OpDep * (intpercentage / 100);
                            item.UpToDep = item.UpToDep * (intpercentage / 100);
                            item.DispoDep = item.DispoDep * (intpercentage / 100);
                            item.TotDep = item.TotDep * (intpercentage / 100);
                            item.NetBalance = item.NetBalance * (intpercentage / 100);
                        }
                    }
                    
                   
                }

            }
            return clist;
            

        }
    }
}