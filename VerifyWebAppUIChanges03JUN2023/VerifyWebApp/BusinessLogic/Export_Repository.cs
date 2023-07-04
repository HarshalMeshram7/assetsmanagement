using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;

namespace VerifyWebApp.BusinessLogic
{
    public class Export_Repository
    {
        private VerifyDB db = new VerifyDB();
        public List<Assets> getassets(int companyid)
        {
            List<Assets> alist = new List<Assets>();
            alist = db.Assetss.Where(x => x.Companyid == companyid).ToList();
            foreach (Assets item in alist)
            {
                item.companyname = db.Companys.Where(x => x.ID == companyid).FirstOrDefault().CompanyName;
                if (item.DtPutToUse == null)
                {
                    item.str_DtPutToUse = "";
                }
                else
                {


                    item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                }
                if (item.DtPutToUseIT == null)
                {
                    item.str_DtPutToUseIT = "";
                }
                else
                {


                    item.str_DtPutToUseIT = Convert.ToDateTime(item.DtPutToUseIT).ToString("dd/MM/yyyy");

                }
                if (item.CommissioningDate == null)
                {
                    item.str_CommissioningDate = "";
                }
                else
                {


                    item.str_CommissioningDate = Convert.ToDateTime(item.CommissioningDate).ToString("dd/MM/yyyy");

                }
                if (item.ExpiryDate == null)
                {
                    item.str_Expirydate = "";
                }
                else
                {


                    item.str_Expirydate = Convert.ToDateTime(item.ExpiryDate).ToString("dd/MM/yyyy");

                }
                if (item.ReceiptDate == null)
                {
                    item.str_ReceiptDate = "";
                }
                else
                {


                    item.str_ReceiptDate = Convert.ToDateTime(item.ReceiptDate).ToString("dd/MM/yyyy");

                }
                if (item.PODate == null)
                {
                    item.str_PODate = "";
                }
                else
                {


                    item.str_PODate = Convert.ToDateTime(item.PODate).ToString("dd/MM/yyyy");

                }
                //if (item.DtPutToUse == null)
                //{
                //    item.str_DtPutToUse = "";
                //}
                //else
                //{


                //    item.str_DtPutToUse = Convert.ToDateTime(item.DtPutToUse).ToString("dd/MM/yyyy");

                //}


                if (item.BillDate == null)
                {
                    item.str_BillDate = "";
                }
                else
                {

                    item.str_BillDate = Convert.ToDateTime(item.BillDate).ToString("dd/MM/yyyy");

                }

                if (item.VoucherDate == null)
                {
                    item.str_VoucherDate = "";
                }
                else
                {

                    item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");

                }

                if (item.SupplierNo == null || item.SupplierNo == 0)
                {
                    item.str_suppliername = "";
                }
                else
                {

                    item.str_suppliername = db.Suppliers.Where(x => x.ID == item.SupplierNo && x.Companyid == companyid).FirstOrDefault().SupplierName;

                }
                if (item.UOMNo == null || item.UOMNo == 0)
                {
                    item.uom_name = "";
                }
                else
                {
                    if (db.UOMs.Where(x => x.ID == item.UOMNo && x.Companyid == companyid).Count() > 0)
                    {
                        item.uom_name = db.UOMs.Where(x => x.ID == item.UOMNo && x.Companyid == companyid).FirstOrDefault().Unit;
                    }
                }
                if (item.LocAID == 0 || item.LocAID == null)
                {
                    item.str_mainlocation = "";
                }
                else
                {

                    item.str_mainlocation = db.ALocations.Where(x => x.ID == item.LocAID && x.Companyid == companyid).FirstOrDefault().ALocationName;

                }
                if (item.LocBID == 0 || item.LocBID == null)
                {
                    item.str_sublocation = "";
                }
                else
                {

                    item.str_sublocation = db.BLocations.Where(x => x.ID == item.LocBID && x.Companyid == companyid).FirstOrDefault().BLocationName;

                }
                if (item.LocCID == 0 || item.LocCID == null)
                {
                    item.str_sub_sublocation = "";
                }
                else
                {

                    item.str_sub_sublocation = db.CLocations.Where(x => x.ID == item.LocCID && x.Companyid == companyid).FirstOrDefault().CLocationName;

                }
                if (item.AccAccountID == null || item.AccAccountID == 0)
                {
                    item.str_accumulatedname = "";
                }
                else
                {

                    item.str_accumulatedname = db.Accounts.Where(x => x.ID == item.AccAccountID && x.Companyid == companyid).FirstOrDefault().AccountName;

                }

                if (item.AccountID == null || item.AccountID == 0)
                {
                    item.str_purchaseaccountname = "";
                }
                else
                {

                    item.str_purchaseaccountname = db.Accounts.Where(x => x.ID == item.AccountID && x.Companyid == companyid).FirstOrDefault().AccountName;

                }
                if (item.DepAccountId == null || item.DepAccountId == 0)
                {
                    item.str_depricationname = "";
                }
                else
                {

                    item.str_depricationname = db.Accounts.Where(x => x.ID == item.DepAccountId && x.Companyid == companyid).FirstOrDefault().AccountName;

                }
                if (item.ITGroupIDID == null || item.ITGroupIDID == 0)
                {
                    item.str_it_name = "";
                }
                else
                {

                    item.str_it_name = db.ITGroups.Where(x => x.ID == item.ITGroupIDID && x.Companyid == companyid).FirstOrDefault().GroupName;

                }
                if (item.CostCenterAID == null || item.CostCenterAID == 0)
                {
                    item.str_costcenteraname = "";
                }
                else
                {

                    item.str_costcenteraname = db.ACostCenters.Where(x => x.ID == item.CostCenterAID && x.Companyid == companyid).FirstOrDefault().CCDescription;

                }
                if (item.CostCenterBID == null || item.CostCenterBID == 0)
                {
                    item.str_costcenterbname = "";
                }
                else
                {

                    item.str_costcenterbname = db.BCostCenters.Where(x => x.ID == item.CostCenterBID && x.Companyid == companyid).FirstOrDefault().SCCDescription;

                }

                var locid = db.childlocations.Where(x => x.AssetID == item.ID && x.Companyid == companyid).OrderByDescending(x => x.ID).FirstOrDefault();
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
                if (item.Parent_AssetId == 0 || item.Parent_AssetId == null)
                {
                    item.ParentAssetName = "";
                }
                else
                {
                    item.Parent_assetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.Parent_AssetId).FirstOrDefault().AssetNo;
                    item.ParentAssetName = db.Assetss.Where(x => x.Companyid == companyid).FirstOrDefault().AssetName;
                }
                /////users
                if (item.CreatedUserId == null || item.CreatedUserId == 0)
                {
                    item.Createdusername = "";
                }
                else
                {
                    item.Createdusername = db.Logins.Where(x => x.CompanyId == companyid && x.ID == item.CreatedUserId).FirstOrDefault().FullName;
                }
                if (item.Modified_Userid == null || item.Modified_Userid == 0)
                {
                    item.Modifiedusername = "";
                }
                else
                {
                    item.Modifiedusername = db.Logins.Where(x => x.CompanyId == companyid && x.ID == item.Modified_Userid).FirstOrDefault().FullName;
                }
                if (item.CreatedDate == null)
                {
                    item.str_CreatedDate = "";
                }
                else
                {

                    item.str_CreatedDate = Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy");

                }

                if (item.ModifiedDate == null)
                {
                    item.str_MoidfiedDate = "";
                }
                else
                {

                    item.str_MoidfiedDate = Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy");

                }///groupnames
                if (item.AGroupID == 0 || item.AGroupID ==null)
                {
                    item.agroupname = "";
                }
                else{
                    item.agroupname = db.AGroups.Where(x => x.ID == item.AGroupID && x.Companyid == companyid).FirstOrDefault().AGroupName;
                }
                if (item.BGroupID == 0 || item.BGroupID == null)
                {
                    item.bgroupname = "";
                }
                else
                {
                    item.bgroupname = db.BGroups.Where(x => x.ID == item.BGroupID && x.Companyid == companyid && x.AGrpID == item.AGroupID).FirstOrDefault().BGroupName;
                }
                if (item.CGroupID == 0 || item.CGroupID == null)
                {
                    item.cgroupname = "";
                }
                else
                {
                    item.cgroupname = db.CGroups.Where(x => x.ID == item.CGroupID && x.Companyid == companyid && x.AGrpID == item.AGroupID && x.BGrpID == item.BGroupID).FirstOrDefault().CGroupName;
                }
                if (item.DGroupID ==0 || item.DGroupID == null)
                {
                    item.dgroupname = "";
                }
                else
                {
                    item.dgroupname = db.DGroups.Where(x => x.ID == item.DGroupID && x.Companyid == companyid && x.AGrpID == item.AGroupID && x.BGrpID == item.BGroupID && x.CGrpID == item.CGroupID).FirstOrDefault().DGroupName;
                }
            }
            return alist;
        }

        public List<Disposal> getdisposallist(int compnayid)
        {
            List<Disposal> deplist = new List<Disposal>();
            deplist = db.Disposals.Where(x => x.Companyid == compnayid).ToList();
            int srno = 1;
            foreach (Disposal item in deplist)
            {

                item.int_Srno = srno;
                item.str_billDate = item.BillDate.ToString("dd/MM/yyyy");
                item.str_voucherDate = item.VoucherDate.ToString("dd/MM/yyyy"); ;
                item.str_disposalDate = item.DisposalDate.ToString("dd/MM/yyyy");

                srno++;

            }
            
            return deplist;

        }
        public List<Depreciation> getdepreciationlist(int companyid)
        {
            List<Depreciation> dlist = new List<Depreciation>();
            dlist = db.Depreciations.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (Depreciation item in dlist)
            {
                item.int_Srno = srno;
                item.str_FromDate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
                item.str_ToDate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy"); ;

                srno++;

            }
            return dlist;
        }
        public List<Account> getaccountlist(int companyid)
        {
            List<Account> alist = new List<Account>();
            alist = db.Accounts.Where(x => x.Companyid == companyid).ToList();
          

            return alist;

        }

        public List<AMC> getamclist(int companyid)
        {
            List<AMC> alist = new List<AMC>();
            alist = db.AMCss.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in alist)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                srno++;
            }
            return alist;
        }
        public List<Insurance> getinsurancelist(int companyid)
        {
            List<Insurance> inlist = new List<Insurance>();
            inlist = db.Insurances.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in inlist)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                srno++;
            }
            return inlist;
        }
        public List<Loan> getloanlist(int companyid)
        {
            List<Loan> llist = new List<Loan>();
            llist = db.Loans.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in llist)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                srno++;
            }
            return llist;
        }
        public List<Period> getperiodlist(int companyid)
        {
            List<Period> plist = new List<Period>();
            plist = db.Periods.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in plist)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                srno++;
            }
            return plist;
        }
        public List<Batch> getbatchlist(int companyid)
        {
            List<Batch> blist = new List<Batch>();
            blist = db.Batchs.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in blist)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
                srno++;
            }
            return blist;
        }
        public List<ITPeriod> getitperiodlist(int companyid)
        {
            List<ITPeriod> itlist = new List<ITPeriod>();
            itlist = db.ITPeriods.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in itlist)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); 
              
                srno++;
            }
            return itlist;
        }
        public List<ITGroup> getitgrouplist(int companyid)
        {
            List<ITGroup> itlist = new List<ITGroup>();
            itlist = db.ITGroups.Where(x => x.Companyid == companyid).ToList();
            
            return itlist;
        }

        //////////////////////////////
        public List<SubAmc> getsubamclist(int companyid)
        {
            List<SubAmc> sublist = new List<SubAmc>();
            sublist = db.SubAmc.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in sublist)
            {

                item.AssetNo = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;

                srno++;
            }
            return sublist;
        }
        public List<SubInsurance> getsubinsurancelist(int companyid)
        {
            List<SubInsurance> inlist = new List<SubInsurance>();
            inlist = db.SubInsurances.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in inlist)
            {
                item.Srno = srno;
                item.AssetNo = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;
                srno++;
            }
            return inlist;
        }
        public List<SubLoan> getsubloanlist(int companyid)
        {
            List<SubLoan> llist = new List<SubLoan>();
            llist = db.SubLoans.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in llist)
            {
                item.Srno = srno;
                item.AssetNo = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetId).FirstOrDefault().AssetNo;
                srno++;
            }
            return llist;
        }
        public List<SubPeriod> getsubperiodlist(int companyid)
        {
            List<SubPeriod> plist = new List<SubPeriod>();
            plist = db.SubPeriods.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in plist)
            {
                item.Srno = srno;
                item.str_fromdate = item.FromDate.ToString("dd/MM/yyyy");
                item.str_todate = item.ToDate.ToString("dd/MM/yyyy"); ;
               
                srno++;
            }
            return plist;
        }
        public List<Subbatch> getsubbatchlist(int companyid)
        {
            List<Subbatch> blist = new List<Subbatch>();
            blist = db.SubBatchs.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in blist)
            {
                item.Srno = srno;
             
                srno++;
            }
            return blist;
        }
        public List<Childlocation> getchildlocationlist(int companyid)
        {
            List<Childlocation> clist = new List<Childlocation>();
            clist = db.childlocations.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in clist)
            {
                item.str_date= Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                item.assetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetID).FirstOrDefault().AssetNo;
                item.assetname= db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.AssetID).FirstOrDefault().AssetName;
                item.str_locaname = db.ALocations.Where(x => x.Companyid == companyid && x.ID == item.ALocID).FirstOrDefault().ALocationName;
                if (item.BLocID == 0 || item.BLocID == null)
                {
                    item.str_locbname = "";
                }
            else
                {
                    item.str_locbname = db.BLocations.Where(x => x.Companyid == companyid && x.ID == item.BLocID).FirstOrDefault().BLocationName;
                }
                if (item.CLocID == 0 || item.CLocID == null)
                {
                    item.str_loccname = "";
                }
                else
                {
                    item.str_loccname = db.CLocations.Where(x => x.Companyid == companyid && x.ID == item.CLocID).FirstOrDefault().CLocationName;
                }
                item.Srno = srno;

                srno++;
            }
            return clist;
        }
        public List<Childcostcenter> getchildcostcenterlist(int companyid)
        {
            List<Childcostcenter> clist = new List<Childcostcenter>();
            clist = db.childcostcenters.Where(x => x.Companyid == companyid).ToList();
            int srno = 1;
            foreach (var item in clist)
            {
                item.str_date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                item.assetno = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.Ass_ID).FirstOrDefault().AssetNo;
                item.assetname = db.Assetss.Where(x => x.Companyid == companyid && x.ID == item.Ass_ID).FirstOrDefault().AssetName;

                item.str_costcenteraname = db.ACostCenters.Where(x => x.Companyid == companyid && x.ID == item.AcostcenterID).FirstOrDefault().CCDescription;
                //if (item.BcostcenterID == 0 || item.BcostcenterID == null)
                //{
                //    item.str_costcenterbname = "";
                //}
                //if (item.BcostcenterID != 0 || item.BcostcenterID != null)
                //{
                //    item.str_costcenterbname = db.BCostCenters.Where(x => x.Companyid == companyid && x.ID == item.BcostcenterID).FirstOrDefault().SCCDescription;
                //}
               
                item.Srno = srno;

                srno++;
            }
            return clist;
        }
    }
}