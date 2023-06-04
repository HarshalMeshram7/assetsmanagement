using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.BusinessLogic
{
    public class ReportRepository
    {
        public VerifyDB db = new VerifyDB();

        public List<LocationReportViewModel> getalllocationwiseasset(int companyid, DateTime AsonDate)
        {
            //List<Assets> alist = new List<Assets>();

            //alist = db.Assetss.Where(x => x.Companyid == companyid && x.VoucherDate<=AsonDate && x.DisposalFlag==0).ToList();
            //foreach (Assets item in alist)
            //{
            //    item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
            //    item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
            //    //item.str_issuedate
            //    var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
            //    if (locid != null)
            //    {
            //        if (locid.Date == null)
            //        {
            //            item.str_issuedate = "";
            //        }
            //        else
            //        {
            //            item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

            //        }

            //    }
            //    if (item.SupplierNo == null)
            //    {
            //        item.str_suppliername = "";
            //    }
            //    else
            //    {
            //        item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
            //    }
            //    if (item.LocAID == 0 || item.LocAID == null)
            //    {
            //        item.str_mainlocation = "";
            //    }
            //    else
            //    {

            //        item.str_mainlocation = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault().ALocationName;

            //    }
            //    if (item.LocBID == 0 || item.LocBID == null)
            //    {
            //        item.str_sublocation = "";
            //    }
            //    else
            //    {

            //        item.str_sublocation = db.BLocations.Where(x => x.ID == item.LocBID && x.Companyid == companyid).FirstOrDefault().BLocationName;

            //    }
            //    if (item.LocCID == 0 || item.LocCID == null)
            //    {
            //        item.str_sub_sublocation = "";
            //    }
            //    else
            //    {

            //        item.str_sub_sublocation = db.CLocations.Where(x => x.ID == item.LocCID && x.Companyid == companyid).FirstOrDefault().CLocationName;

            //    }
            //}

            //return alist;

            List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
            string strSQL = null;
            string str_comid = companyid.ToString();
            string asondate = AsonDate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call allocationreport(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + asondate + "')";






            alist = db.Database.SqlQuery<LocationReportViewModel>(strSQL).ToList();
            foreach (LocationReportViewModel item in alist)
            {

                if (item.VoucherDate != null)
                {
                    item.str_voucherdate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_voucherdate = "";
                }

                if (item.Dateputtouse != null)
                {
                    item.str_DtPutToUse = Convert.ToDateTime(item.Dateputtouse).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_DtPutToUse = "";
                }
                var locid = db.childlocations.Where(x => x.AssetID == item.assetid && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                if (locid != null)
                {
                    if (locid.Date == null)
                    {
                        item.str_issuedate = "";
                    }
                    else
                    {
                        item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                    }

                }
            }
            return alist;

        }

        public List<LocationReportViewModel> getsinglelocationwiseasset(int companyid, SingleLocationParameter singleLocationParameter)
        {

            DateTime asondate = Convert.ToDateTime(singleLocationParameter.AsonDate);
            if (singleLocationParameter.sublocationid == 0)
            {
                singleLocationParameter.sublocationid = 0;

            }
            if (singleLocationParameter.sub_sublocationid == 0)
            {

                singleLocationParameter.sub_sublocationid = 0;
            }
            List<LocationReportViewModel> alist = new List<LocationReportViewModel>();
            string strSQL = null;
            string str_comid = companyid.ToString();
            string str_alocid = singleLocationParameter.locationid.ToString();
            string str_blocid = singleLocationParameter.sublocationid.ToString();
            string str_clocid = singleLocationParameter.sub_sublocationid.ToString();
            string str_asondate = asondate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call singlelocationreport(";
            strSQL = strSQL + str_comid + ",";
            strSQL = strSQL + "'" + str_asondate + "',";

            strSQL = strSQL + str_alocid + ",";
            strSQL = strSQL + str_blocid + ",";
            strSQL = strSQL + str_clocid + ")";





            alist = db.Database.SqlQuery<LocationReportViewModel>(strSQL).ToList();
            foreach (LocationReportViewModel item in alist)
            {

                if (item.VoucherDate != null)
                {
                    item.str_voucherdate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_voucherdate = "";
                }

                if (item.Dateputtouse != null)
                {
                    item.str_DtPutToUse = Convert.ToDateTime(item.Dateputtouse).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_DtPutToUse = "";
                }
                var locid = db.childlocations.Where(x => x.AssetID == item.assetid && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
                if (locid != null)
                {
                    if (locid.Date == null)
                    {
                        item.str_issuedate = "";
                    }
                    else
                    {
                        item.str_issuedate = Convert.ToDateTime(locid.Date).ToString("dd/MM/yyyy");

                    }

                }
            }
            return alist;
        }

        public List<Assets> getallgroupwisewiseasset(int companyid, DateTime AsonDate)
        {
            List<Assets> alist = new List<Assets>();

            alist = db.Assetss.Where(x => x.Companyid == companyid && x.VoucherDate <= AsonDate && x.DisposalFlag == 0).ToList();

            return alist;
        }

        public List<Assets> getsinglegroupwiseasset(int companyid, int agroupid, int bgoupnid, int cgroup, int dgroup, DateTime AsonDate)
        {
            List<Assets> alist = new List<Assets>();

            alist = db.Assetss.Where(x => x.Companyid == companyid && x.DisposalFlag == 0 && x.AGroupID == agroupid && x.BGroupID == bgoupnid && x.CGroupID == cgroup && x.DGroupID == dgroup && x.VoucherDate <= AsonDate).ToList();

            return alist;
        }
        public List<ComponentialReport_Viewmodel> getcomponentialasset(int companyid, DateTime fromdate, DateTime todate)
        {


            List<Assets> alist = new List<Assets>();
            List<ComponentialReport_Viewmodel> clist = new List<ComponentialReport_Viewmodel>(); ;
            alist = db.Assetss.Where(x => x.Companyid == companyid && x.VoucherDate >= fromdate && x.VoucherDate <= todate && x.Parent_AssetId != null && x.Parent_AssetId != 0 && x.DisposalFlag == 0 && x.iscomponent == "yes").ToList();
            foreach (Assets item in alist)
            {
                ComponentialReport_Viewmodel cobj = new ComponentialReport_Viewmodel();

                Assets parentasset = new Assets();
                parentasset = db.Assetss.Where(x => x.ID == item.Parent_AssetId).FirstOrDefault();
                cobj.Assetno = item.AssetNo;
                cobj.ParentAssetno = parentasset.AssetNo;
                cobj.componentname = item.AssetName;
                cobj.str_componentdate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                cobj.str_parent_assetdate = Convert.ToDateTime(parentasset.VoucherDate).ToString("dd/MM/yyyy");
                cobj.parentassetname = parentasset.AssetName;
                cobj.parent_asset_usefullife = parentasset.Usefullife;
                cobj.componentusefullife = item.Usefullife;
                if (item.iscomponent == "no")
                {
                    cobj.additionamtcapcomp = item.AmountCapitalisedCompany;
                }
                if (item.iscomponent == "yes")
                {
                    cobj.componentamtcapcomp = item.AmountCapitalisedCompany;
                }

                cobj.parent_asset_amtcapcomp = parentasset.AmountCapitalisedCompany;
                clist.Add(cobj);
                //return clist;
            }


            return clist;
        }

        public List<UsefullifeReportViewModel> getusefullifereport(int companyid, string usefullifetype)
        {


            List<UsefullifeReportViewModel> alist = new List<UsefullifeReportViewModel>();
            string strSQL = null;
            string str_comid = companyid.ToString();
            // string asondate = AsonDate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call usefullifereport(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + usefullifetype + "')";


            alist = db.Database.SqlQuery<UsefullifeReportViewModel>(strSQL).ToList();
            foreach (UsefullifeReportViewModel item in alist)
            {

                if (item.Dateputtouse != null)
                {
                    item.str_Dateputtouse = Convert.ToDateTime(item.Dateputtouse).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_Dateputtouse = "";
                }

                if (item.ExpiryDate != null)
                {
                    item.str_ExpiryDate = Convert.ToDateTime(item.ExpiryDate).ToString("dd/MM/yyyy");
                }
                else
                {
                    item.str_ExpiryDate = "";
                }

            }
            return alist;


        }

        public List<FARReportViewmodel> getAssetAdditionsReport(int companyid, DateTime FromDate, DateTime ToDate)
        {

            List<FARReportViewmodel> flist = new List<FARReportViewmodel>();
            string strSQL = null;
            string str_comid = companyid.ToString();

            string fromDate = FromDate.ToString("yyyy-MM-dd");
            string todate = ToDate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call usp_report_Addition(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + fromDate + "',";
            strSQL = strSQL + "'" + todate + "')";

            db.Database.CommandTimeout = 180;

            flist = db.Database.SqlQuery<FARReportViewmodel>(strSQL).ToList();
            /*
            foreach (FARReportViewmodel item in flist)
            {
                if (item.VoucherDate != null)
                {
                    item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                }
                if (item.Dateputtouse != null)
                {
                    item.str_DTPutUseCompany = Convert.ToDateTime(item.Dateputtouse).ToString("dd/MM/yyyy");

                }

            }*/

            return flist;
        }

        public List<DisposalReportViewModel> getAssetDisposalReport(int companyid, DateTime FromDate, DateTime ToDate)
        {

            //List<FARReportViewmodel> flist = new List<FARReportViewmodel>();
            List<DisposalReportViewModel> list = new List<DisposalReportViewModel>();

            string strSQL = null;
            string str_comid = companyid.ToString();

            string fromDate = FromDate.ToString("yyyy-MM-dd");
            string todate = ToDate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call usp_report_Disposal(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + fromDate + "',";
            strSQL = strSQL + "'" + todate + "')";

            db.Database.CommandTimeout = 180;

            list = db.Database.SqlQuery<DisposalReportViewModel>(strSQL).ToList();
           

            return list;
        }
    }
}