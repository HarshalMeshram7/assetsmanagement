using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;
namespace VerifyWebApp.BusinessLogic
{
    public class FARReportRepository
    {
        public VerifyDB db = new VerifyDB();
        //public List<FARReportViewmodel> getAllassetForFAR(int companyid, DateTime AsonDate)
        //{
        //    List<FARReportViewmodel> clist = new List<FARReportViewmodel>();
        //   // FARReportViewmodel cobj = new FARReportViewmodel();
        //    List<Assets> alist = new List<Assets>();
        //    alist = db.Assetss.Where(x => x.Companyid == companyid && x.VoucherDate <= AsonDate).ToList();
        //    foreach (Assets item in alist)
        //    {
        //        //Assets parentasset = new Assets();
        //        FARReportViewmodel cobj = new FARReportViewmodel();
        //        //string AGrpName = db.AGroups.Where(x => x.ID == item.AGroupID && x.Companyid == companyid).FirstOrDefault().AGroupName;
        //        //string BGrpName = db.BGroups.Where(x => x.ID == item.BGroupID && x.Companyid == companyid).FirstOrDefault().BGroupName;
        //        //string CGrpName = db.CGroups.Where(x => x.ID == item.CGroupID && x.Companyid == companyid).FirstOrDefault().CGroupName;
        //        //string DGrpName = db.DGroups.Where(x => x.ID == item.DGroupID && x.Companyid == companyid).FirstOrDefault().DGroupName;
        //        //string SuppName = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
        //        if (item.AGroupID == 0|| item.AGroupID==null)
        //        {
        //            cobj.AGroupName = "";
        //        }
        //        else
        //        {
        //            cobj.AGroupName = db.AGroups.Where(x => x.ID == item.AGroupID && x.Companyid == companyid).FirstOrDefault().AGroupName;
        //        }
        //        if (item.BGroupID == 0 || item.BGroupID == null)
        //        {
        //            cobj.BGroupName = "";
        //        }
        //        else
        //        {
        //            cobj.BGroupName = db.BGroups.Where(x => x.ID == item.BGroupID && x.Companyid == companyid).FirstOrDefault().BGroupName;
        //        }
        //        if (item.CGroupID == 0 || item.CGroupID == null)
        //        {
        //            cobj.CGroupName = "";
        //        }
        //        else
        //        {
        //            cobj.CGroupName = db.CGroups.Where(x => x.ID == item.CGroupID && x.Companyid == companyid).FirstOrDefault().CGroupName;
        //        }
        //        if (item.DGroupID == 0 || item.DGroupID == null)
        //        {
        //            cobj.DGroupName = "";
        //        }
        //        else
        //        {
        //            cobj.DGroupName = db.DGroups.Where(x => x.ID == item.DGroupID && x.Companyid == companyid).FirstOrDefault().DGroupName;
        //        }
        //        if (item.SupplierNo == 0 || item.SupplierNo == null)
        //        {
        //            cobj.SupplierName = "";
        //        }
        //        else
        //        {
        //            cobj.SupplierName = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
        //        }


        //        cobj.AssetNo = item.AssetNo;
        //        cobj.AssetIdentificationNo = item.AssetIdentificationNo;
        //        cobj.AssetName = item.AssetName;
        //        cobj.VoucherNo = item.VoucherNo;
        //        cobj.VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
        //        cobj.DTPutUseCompany = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");
        //        cobj.Qty = item.Qty;
        //        //cobj.SupplierName = SuppName;
        //        cobj.DepRate = item.TotalRate;
        //        cobj.DepMethod = item.DepreciationMethod;
        //        cobj.AmountCapitalisedCompany = item.AmountCapitalisedCompany;
        //        cobj.AmountCapitalisedIT = item.AmountCApitalisedIT;
        //        cobj.DepreciationAmount = item.OPAccDepreciation;
        //        cobj.NetBalance = item.AmountCapitalisedCompany - item.OPAccDepreciation;
        //        cobj.TotalCedit = item.TotalCredit;
        //        cobj.InvoiceAmount = item.InvoiceAmt;
        //        cobj.TransactionType = "Purchase";

        //        clist.Add(cobj);
        //    } //all assets addes


        //      //--------------------start addition to asset-----
        //      //List<Addition> addlist = new List<Addition>();
        //      //addlist = db.Additions.Where(x => x.Companyid == companyid && x.VoucherDate <= AsonDate).ToList();
        //      //foreach (Addition item1 in addlist)
        //      //{
        //      //    Assets assobj = new Assets();


        //    //    if (item1.SupplierNo == 0)
        //    //    {
        //    //        cobj.SupplierName = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        cobj.SupplierName = db.Suppliers.Where(x => x.ID == item1.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;
        //    //    }


        //    //    cobj.AssetNo = item1.AssetNo;
        //    //    var AssId = db.Assetss.Where(x => x.AssetNo == item1.AssetNo && x.Companyid == companyid).FirstOrDefault().ID;
        //    //    assobj = db.Assetss.Where(x => x.ID == AssId && x.Companyid == companyid).FirstOrDefault();

        //    //    if (assobj.AGroupID == 0 || assobj.AGroupID == null)
        //    //    {
        //    //        cobj.AGroupName = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        cobj.AGroupName = db.AGroups.Where(x => x.ID == assobj.AGroupID && x.Companyid == companyid).FirstOrDefault().AGroupName;
        //    //    }
        //    //    if (assobj.BGroupID == 0 || assobj.BGroupID == null)
        //    //    {
        //    //        cobj.BGroupName = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        cobj.BGroupName = db.BGroups.Where(x => x.ID == assobj.BGroupID && x.Companyid == companyid).FirstOrDefault().BGroupName;
        //    //    }
        //    //    if (assobj.CGroupID == 0 || assobj.CGroupID == null)
        //    //    {
        //    //        cobj.CGroupName = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        cobj.CGroupName = db.CGroups.Where(x => x.ID == assobj.CGroupID && x.Companyid == companyid).FirstOrDefault().CGroupName;
        //    //    }
        //    //    if (assobj.DGroupID == 0 || assobj.DGroupID == null)
        //    //    {
        //    //        cobj.DGroupName = "";
        //    //    }
        //    //    else
        //    //    {
        //    //        cobj.DGroupName = db.DGroups.Where(x => x.ID == assobj.DGroupID && x.Companyid == companyid).FirstOrDefault().DGroupName;
        //    //    }
        //    //    // cobj.AssetIdentificationNo = item.AssetIdentificationNo;
        //    //    cobj.AssetName = item1.AssetName;
        //    //    cobj.VoucherNo = item1.VoucherNo;
        //    //    cobj.VoucherDate = Convert.ToDateTime(item1.VoucherDate).ToString("dd/MM/yyyy");
        //    //    cobj.DTPutUseCompany = Convert.ToDateTime(item1.DtPutToUse).ToString("dd/MM/yyyy");
        //    //    cobj.Qty = item1.Qty;
        //    //    cobj.DepRate = assobj.TotalRate;
        //    //    cobj.DepMethod = assobj.DepreciationMethod;
        //    //    cobj.AmountCapitalisedCompany = item1.AmountCapitalisedCompany;
        //    //    cobj.AmountCapitalisedIT = item1.AmountCApitalisedIT;
        //    //    cobj.DepreciationAmount = 0;
        //    //   //cobj.NetBalance = item.AmountCapitalisedCompany - item.OPAccDepreciation;
        //    //    cobj.TotalCedit = item1.TotalCredit;
        //    //    cobj.InvoiceAmount = item1.InvoiceAmt;
        //    //    cobj.TransactionType = "Addition";

        //    //    clist.Add(cobj);
        //    //}
        //    //--------------------Disposal-------------------------------
        //    List<Disposal> addlist = new List<Disposal>();
        //    addlist = db.Disposals.Where(x => x.Companyid == companyid && x.VoucherDate <= AsonDate).ToList();
        //    foreach (Disposal item2 in addlist)
        //    {
        //        Assets assobj1 = new Assets();
        //        FARReportViewmodel cobj1 = new FARReportViewmodel();         
        //        var AssId = db.Assetss.Where(x => x.ID == item2.AssetId && x.Companyid == companyid).FirstOrDefault().ID;
        //        assobj1 = db.Assetss.Where(x => x.ID == AssId && x.Companyid == companyid).FirstOrDefault();

        //        if (assobj1.AGroupID == 0 || assobj1.AGroupID == null)
        //        {
        //            cobj1.AGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.AGroupName = db.AGroups.Where(x => x.ID == assobj1.AGroupID && x.Companyid == companyid).FirstOrDefault().AGroupName;
        //        }
        //        if (assobj1.BGroupID == 0 || assobj1.BGroupID == null)
        //        {
        //            cobj1.BGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.BGroupName = db.BGroups.Where(x => x.ID == assobj1.BGroupID && x.Companyid == companyid).FirstOrDefault().BGroupName;
        //        }
        //        if (assobj1.CGroupID == 0 || assobj1.CGroupID == null)
        //        {
        //            cobj1.CGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.CGroupName = db.CGroups.Where(x => x.ID == assobj1.CGroupID && x.Companyid == companyid).FirstOrDefault().CGroupName;
        //        }
        //        if (assobj1.DGroupID == 0 || assobj1.DGroupID == null)
        //        {
        //            cobj1.DGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.DGroupName = db.DGroups.Where(x => x.ID == assobj1.DGroupID && x.Companyid == companyid).FirstOrDefault().DGroupName;
        //        }
        //        // cobj.AssetIdentificationNo = item.AssetIdentificationNo;
        //        cobj1.AssetNo = item2.AssetNo;
        //        cobj1.AssetName = item2.AssetName;
        //        cobj1.VoucherNo = item2.VoucherNo;
        //        cobj1.VoucherDate = Convert.ToDateTime(item2.VoucherDate).ToString("dd/MM/yyyy");
        //        //  cobj.DTPutUseCompany = Convert.ToDateTime(item1.DtPutToUse).ToString("dd/MM/yyyy");
        //        cobj1.Qty = (item2.Qty)-(assobj1.Qty);
        //        cobj1.DepRate = assobj1.TotalRate;
        //        cobj1.DepMethod = assobj1.DepreciationMethod;
        //        cobj1.AmountCapitalisedCompany = (item2.GrossAmount)-(assobj1.AmountCapitalisedCompany);
        //        cobj1.AmountCapitalisedIT = (item2.GrossAmount) - (assobj1.AmountCApitalisedIT);
        //        cobj1.DepreciationAmount = 0;
        //        //cobj.NetBalance = item.AmountCapitalisedCompany - item.OPAccDepreciation;
        //        cobj1.TotalCedit = 0;
        //        cobj1.InvoiceAmount = 0;
        //        cobj1.TransactionType = "disposal";

        //        clist.Add(cobj1);
        //    }
        //    //--------------------add Depreciation---------------------------------------
        //    List<Depreciation> deplist = new List<Depreciation>();
        //    deplist = db.Depreciations.Where(x => x.Companyid == companyid &&  x.ToDate <= AsonDate).ToList();
        //    foreach (Depreciation item3 in deplist)
        //    {
        //        Assets assobj1 = new Assets();
        //        FARReportViewmodel cobj1 = new FARReportViewmodel();
        //        var AssId = db.Assetss.Where(x => x.ID == item3.AssetId && x.Companyid == companyid).FirstOrDefault().ID;
        //        assobj1 = db.Assetss.Where(x => x.ID == AssId && x.Companyid == companyid).FirstOrDefault();

        //        if (assobj1.AGroupID == 0 || assobj1.AGroupID == null)
        //        {
        //            cobj1.AGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.AGroupName = db.AGroups.Where(x => x.ID == assobj1.AGroupID && x.Companyid == companyid).FirstOrDefault().AGroupName;
        //        }
        //        if (assobj1.BGroupID == 0 || assobj1.BGroupID == null)
        //        {
        //            cobj1.BGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.BGroupName = db.BGroups.Where(x => x.ID == assobj1.BGroupID && x.Companyid == companyid).FirstOrDefault().BGroupName;
        //        }
        //        if (assobj1.CGroupID == 0 || assobj1.CGroupID == null)
        //        {
        //            cobj1.CGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.CGroupName = db.CGroups.Where(x => x.ID == assobj1.CGroupID && x.Companyid == companyid).FirstOrDefault().CGroupName;
        //        }
        //        if (assobj1.DGroupID == 0 || assobj1.DGroupID == null)
        //        {
        //            cobj1.DGroupName = "";
        //        }
        //        else
        //        {
        //            cobj1.DGroupName = db.DGroups.Where(x => x.ID == assobj1.DGroupID && x.Companyid == companyid).FirstOrDefault().DGroupName;
        //        }
        //        // cobj.AssetIdentificationNo = item.AssetIdentificationNo;
        //        cobj1.AssetNo = item3.AssetNo;
        //        cobj1.AssetName = item3.AssetName;
        //        cobj1.VoucherNo = assobj1.VoucherNo;
        //        cobj1.VoucherDate = Convert.ToDateTime(item3.ToDate).ToString("dd/MM/yyyy");
        //        //  cobj.DTPutUseCompany = Convert.ToDateTime(item1.DtPutToUse).ToString("dd/MM/yyyy");
        //        cobj1.Qty = assobj1.Qty;
        //        cobj1.DepRate = item3.TotalRate;
        //        cobj1.DepMethod = item3.DepreciationMethod;
        //        cobj1.AmountCapitalisedCompany = assobj1.AmountCapitalisedCompany;
        //        cobj1.AmountCapitalisedIT = assobj1.AmountCApitalisedIT;
        //        cobj1.DepreciationAmount = item3.Amount;
        //        cobj1.NetBalance = 0;
        //        cobj1.TotalCedit = 0;
        //        cobj1.InvoiceAmount = 0;
        //        cobj1.TransactionType = "depreciation";

        //        clist.Add(cobj1);
        //    }
        //    //--------------------------------------------------------


        //    return clist;
        //}

        /*
        public List<FARReportViewmodel> getFARReport(int companyid, DateTime fromDate,DateTime toDate)
        {
            List<FARReportViewmodel> flist = new List<FARReportViewmodel>();
            string strSQL = null;
            string str_comid = companyid.ToString();
            string fromDate = AsonDate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call farreport(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + fromDate + "')";

            db.Database.CommandTimeout = 180;



            flist = db.Database.SqlQuery<FARReportViewmodel>(strSQL).ToList();
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

            }
            return flist;
            
        }
        */


        public List<FARReportViewmodel> getAllassetForFAR(int companyid, DateTime AsonDate)
        {
            List<FARReportViewmodel> flist = new List<FARReportViewmodel>();
            string strSQL = null;
            string str_comid = companyid.ToString();
            string fromDate = AsonDate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call farreport(";
            strSQL = strSQL + companyid + ",";
            strSQL = strSQL + "'" + fromDate + "')";

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

            }
            */
            return flist;
        }

        public List<FARReportViewmodel> getFAR_New(int companyid,DateTime FromDate,DateTime ToDate)
        {

            List<FARReportViewmodel> flist = new List<FARReportViewmodel>();
            string strSQL = null;
            string str_comid = companyid.ToString();

            string fromDate = FromDate.ToString("yyyy-MM-dd");
            string todate = ToDate.ToString("yyyy-MM-dd");


            strSQL = "";
            strSQL = "Call usp_report_FAR(";
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


    }
}