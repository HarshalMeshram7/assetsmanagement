using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.BusinessLogic
{
    public class AssetRepository
    {
        public VerifyDB db = new VerifyDB();

        public List<Assets> GetAssetDataSearch(int CompanyID, string Level, int GroupID, int startrec, int pagesize, string searchby = "", string searchstring = "")
        {

            List<Assets> AssetList = new List<Assets>();
            try
            {
                if (Level == "L1") // Asset on A Group
                {
                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.AGroupID == GroupID
                                            && x.DisposalFlag == 0
                                        ).ToList();
                }
                else if (Level == "L2") // Asset on bgroup
                {
                    int AGroupID = 0;

                    var objBGroup = db.BGroups.Where(x => x.ID == GroupID && x.Companyid == CompanyID).FirstOrDefault();

                    if (objBGroup != null)
                    {
                        AGroupID = objBGroup.AGrpID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.AGroupID == AGroupID
                                            && x.BGroupID == GroupID
                                            && x.DisposalFlag == 0
                              ).ToList();
                }
                else if (Level == "L3")  // Asset on cgroup
                {
                    int AGroupID = 0;
                    int BGroupID = 0;

                    var objCGroup = db.CGroups.Where(x => x.ID == GroupID && x.Companyid == CompanyID).FirstOrDefault();
                    if (objCGroup != null)
                    {
                        AGroupID = objCGroup.AGrpID;
                        BGroupID = objCGroup.BGrpID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                           && x.AGroupID == AGroupID
                                           && x.BGroupID == BGroupID
                                           && x.CGroupID == GroupID
                                           && x.DisposalFlag == 0
                                             ).ToList();

                }
                else if (Level == "L4") // Asset on dgroup  
                {
                    int AGroupID = 0;
                    int BGroupID = 0;
                    int CGroupID = 0;

                    var objDGroup = db.DGroups.Where(x => x.ID == GroupID && x.Companyid == CompanyID).FirstOrDefault();
                    if (objDGroup != null)
                    {
                        AGroupID = objDGroup.AGrpID;
                        BGroupID = objDGroup.BGrpID;
                        CGroupID = objDGroup.CGrpID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                           && x.AGroupID == AGroupID
                                           && x.BGroupID == BGroupID
                                           && x.CGroupID == CGroupID
                                           && x.DGroupID == GroupID
                                           && x.DisposalFlag == 0
                                             ).ToList();
                }
                else
                {
                    //  Should it be error -- Mandar ?


                    if (searchby == "1")
                    {

                        AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                               && x.DisposalFlag == 0
                                               && x.AssetNo.ToString().ToLower().StartsWith(searchstring.ToLower())
                                           ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();



                        //AssetList = AssetList.Where(p => p.AssetNo.ToString().ToLower().StartsWith(searchstring.ToLower())).ToList();
                    }

                    if (searchby == "2")
                    {

                        AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                               && x.DisposalFlag == 0
                                               && x.AssetName.ToString().ToLower().StartsWith(searchstring.ToLower())
                                           ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();

                        //AssetList = AssetList.Where(p => p.AssetName.ToString().ToLower().Contains(searchstring.ToLower())).ToList();
                    }

                    if (searchby == "3")
                    {
                        //AssetList = AssetList.Where(p => p.SrNo.ToString().ToLower().StartsWith(searchstring.ToLower())).ToList();

                        AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                               && x.DisposalFlag == 0
                                               && x.SrNo.ToString().ToLower().StartsWith(searchstring.ToLower())
                                           ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();

                    }
                    if (searchby == "4")
                    {
                        //AssetList = AssetList.Where(p => p.AssetIdentificationNo.ToString().ToLower().StartsWith(searchstring.ToLower())).ToList();

                        AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.DisposalFlag == 0
                                            && x.AssetIdentificationNo.ToString().ToLower().StartsWith(searchstring.ToLower())
                                        ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();
                    }

                }


                foreach (Assets item in AssetList)
                {
                    if (item.VoucherDate != null)
                    {
                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                    }

                }



            }
            catch (Exception ex)
            {
                // TODO Log Error message 
                AssetList = new List<Assets>();
            }
            return AssetList;
        }

        public List<Assets> GetAssetData(int CompanyID, string Level, int GroupID, int startrec, int pagesize)
        {

            List<Assets> AssetList = new List<Assets>();
            try
            {
                if (Level == "L1") // Asset on A Group
                {
                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.AGroupID == GroupID
                                            && x.DisposalFlag == 0
                                        ).OrderBy(x => x.ID).Skip(startrec).Take(pagesize).ToList();
                }
                else if (Level == "L2") // Asset on bgroup
                {
                    int AGroupID = 0;

                    var objBGroup = db.BGroups.Where(x => x.ID == GroupID && x.Companyid == CompanyID).FirstOrDefault();

                    if (objBGroup != null)
                    {
                        AGroupID = objBGroup.AGrpID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.AGroupID == AGroupID
                                            && x.BGroupID == GroupID
                                            && x.DisposalFlag == 0
                              ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();

                }
                else if (Level == "L3")  // Asset on cgroup
                {
                    int AGroupID = 0;
                    int BGroupID = 0;

                    var objCGroup = db.CGroups.Where(x => x.ID == GroupID && x.Companyid == CompanyID).FirstOrDefault();
                    if (objCGroup != null)
                    {
                        AGroupID = objCGroup.AGrpID;
                        BGroupID = objCGroup.BGrpID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                           && x.AGroupID == AGroupID
                                           && x.BGroupID == BGroupID
                                           && x.CGroupID == GroupID
                                           && x.DisposalFlag == 0
                                             ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();

                }
                else if (Level == "L4") // Asset on dgroup  
                {
                    int AGroupID = 0;
                    int BGroupID = 0;
                    int CGroupID = 0;

                    var objDGroup = db.DGroups.Where(x => x.ID == GroupID && x.Companyid == CompanyID).FirstOrDefault();
                    if (objDGroup != null)
                    {
                        AGroupID = objDGroup.AGrpID;
                        BGroupID = objDGroup.BGrpID;
                        CGroupID = objDGroup.CGrpID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                           && x.AGroupID == AGroupID
                                           && x.BGroupID == BGroupID
                                           && x.CGroupID == CGroupID
                                           && x.DGroupID == GroupID
                                           && x.DisposalFlag == 0
                                             ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();
                }
                else
                {
                    //  Should it be error -- Mandar ?


                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID && x.DisposalFlag == 0)
                        .OrderBy(x => x.ID)
                        .Skip(startrec).Take(pagesize).ToList();



                    // alist = alist.Skip(startRec).Take(pageSize).ToList();
                }

                foreach (Assets item in AssetList)
                {
                    if (item.VoucherDate != null)
                    {
                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                    }

                }
            }
            catch (Exception ex)
            {
                // TODO Log Error message 
                AssetList = new List<Assets>();
            }
            return AssetList;
        }

        public List<Assets> GetAssetDataIT(int CompanyID, string Level, int GroupID, int startrec, int pagesize)
        {
            List<Assets> Assetlist = new List<Assets>();
            try
            {
                if (Level == "L1")
                {
                    Assetlist = db.Assetss.Where(x => x.Companyid == CompanyID

                            && x.ITGroupIDID == GroupID
                             && x.DisposalFlag == 0)

                            .OrderBy(x => x.ID)
                            .Skip(startrec).Take(pagesize).ToList();

                }


                else
                {
                    Assetlist = db.Assetss.Where(x => x.Companyid == CompanyID && x.ITGroupIDID != 0 && x.DisposalFlag == 0)
                         .OrderBy(x => x.ID)
                        .Skip(startrec).Take(pagesize).ToList();
                }

                foreach (Assets item in Assetlist)
                {
                    if (item.VoucherDate != null)
                    {
                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                    }

                }

            }
            catch (Exception ex)
            {
                // TODO Log Error message 
                Assetlist = new List<Assets>();
            }
            return Assetlist;


        }

        public List<Assets> GetAssetDataSearchIT(int CompanyID, string Level, int GroupID, int startrec, int pagesize, string searchby = "", string searchstring = "")
        {

            List<Assets> AssetList = new List<Assets>();
            try
            {
                if (Level == "L1") // Asset 
                {
                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.ITGroupIDID == GroupID
                                            && x.DisposalFlag == 0
                                        ).ToList();
                }
                else
                {
                    //  Should it be error -- Mandar ?


                    if (searchby == "1")
                    {

                        AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                               && x.DisposalFlag == 0
                                               && x.AssetNo.ToString().ToLower().StartsWith(searchstring.ToLower())
                                           ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();



                        //AssetList = AssetList.Where(p => p.AssetNo.ToString().ToLower().StartsWith(searchstring.ToLower())).ToList();
                    }

                    if (searchby == "2")
                    {

                        AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                               && x.DisposalFlag == 0
                                               && x.AssetName.ToString().ToLower().StartsWith(searchstring.ToLower())
                                           ).OrderBy(x => x.ID)
                               .Skip(startrec).Take(pagesize).ToList();

                        //AssetList = AssetList.Where(p => p.AssetName.ToString().ToLower().Contains(searchstring.ToLower())).ToList();
                    }

                    //if (searchby == "3")
                    //{
                    //    //AssetList = AssetList.Where(p => p.SrNo.ToString().ToLower().StartsWith(searchstring.ToLower())).ToList();

                    //    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                    //                           && x.DisposalFlag == 0
                    //                           && x.SrNo.ToString().ToLower().StartsWith(searchstring.ToLower())
                    //                       ).OrderBy(x => x.ID)
                    //           .Skip(startrec).Take(pagesize).ToList();

                    //}
                    //if (searchby == "4")
                    //{
                    //    //AssetList = AssetList.Where(p => p.AssetIdentificationNo.ToString().ToLower().StartsWith(searchstring.ToLower())).ToList();

                    //    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                    //                        && x.DisposalFlag == 0
                    //                        && x.AssetIdentificationNo.ToString().ToLower().StartsWith(searchstring.ToLower())
                    //                    ).OrderBy(x => x.ID)
                    //           .Skip(startrec).Take(pagesize).ToList();
                    //}

                }


                foreach (Assets item in AssetList)
                {
                    if (item.VoucherDate != null)
                    {
                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                    }

                }



            }
            catch (Exception ex)
            {
                // TODO Log Error message 
                AssetList = new List<Assets>();
            }
            return AssetList;
        }

        public List<Assets> GetAssetData_Costcenter(int CompanyID, string Level, int CostcenterID)
        {

            List<Assets> AssetList = new List<Assets>();
            try
            {
                if (Level == "L1") // Asset on A costcenter
                {
                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.CostCenterAID == CostcenterID
                                            && x.DisposalFlag == 0
                                        ).ToList();
                }
                else if (Level == "L2") // Asset on b cc
                {
                    int ACostcenterID = 0;

                    var objBcostcenter = db.BCostCenters.Where(x => x.ID == CostcenterID && x.Companyid == CompanyID).FirstOrDefault();

                    if (objBcostcenter != null)
                    {
                        ACostcenterID = objBcostcenter.ID; ;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.CostCenterAID == ACostcenterID
                                            && x.CostCenterBID == CostcenterID
                                            && x.DisposalFlag == 0
                              ).ToList();
                }

                else
                {
                    //  Should it be error -- Mandar ?
                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID && x.DisposalFlag == 0).ToList();
                }

                foreach (Assets item in AssetList)
                {
                    if (item.VoucherDate != null)
                    {
                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                    }

                }
            }
            catch (Exception ex)
            {
                // TODO Log Error message 
                AssetList = new List<Assets>();
            }
            return AssetList;
        }
        public List<Assets> GetAssetData_Location(int CompanyID, string Level, int LocID)
        {

            List<Assets> AssetList = new List<Assets>();
            try
            {
                if (Level == "L1") // Asset on A loc
                {
                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.LocAID == LocID
                                            && x.DisposalFlag == 0
                                        ).ToList();
                }
                else if (Level == "L2") // Asset on bloc
                {
                    int AlocID = 0;

                    var objBloc = db.BLocations.Where(x => x.ID == LocID && x.Companyid == CompanyID).FirstOrDefault();

                    if (objBloc != null)
                    {
                        AlocID = objBloc.ALocID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                            && x.LocAID == AlocID
                                            && x.LocBID == LocID
                                            && x.DisposalFlag == 0
                              ).ToList();
                }
                else if (Level == "L3")  // Asset on c loc
                {
                    int AlocID = 0;
                    int BlocID = 0;

                    var objCloc = db.CLocations.Where(x => x.ID == LocID && x.Companyid == CompanyID).FirstOrDefault();
                    if (objCloc != null)
                    {
                        AlocID = objCloc.ALocID;
                        BlocID = objCloc.BLocID;
                    }

                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID
                                           && x.LocAID == AlocID
                                           && x.LocBID == AlocID
                                           && x.LocCID == LocID
                                           && x.DisposalFlag == 0
                                             ).ToList();

                }

                else
                {
                    //  Should it be error -- Mandar ?
                    AssetList = db.Assetss.Where(x => x.Companyid == CompanyID && x.DisposalFlag == 0).ToList();
                }

                foreach (Assets item in AssetList)
                {
                    if (item.VoucherDate != null)
                    {
                        item.str_VoucherDate = Convert.ToDateTime(item.VoucherDate).ToString("dd/MM/yyyy");
                    }

                }
            }
            catch (Exception ex)
            {
                // TODO Log Error message 
                AssetList = new List<Assets>();
            }
            return AssetList;
        }

        string AGroupName = "";
        string BGroupName = "";
        string CGroupName = "";
        string DGroupName = "";


        /*
        public bool GetAssetGroupName(string AssetID, out string AGroupName, out string BGroupName, out string CGroupName,out string DGroupName)
        {
            return true;
        }
        */

        public bool UpdateAsset(AssetGroupViewmodel assetGroup, int userId, ref string Message)
        {
            bool result = false;

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            var tnow = System.DateTime.Now.ToUniversalTime();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    // Validate what can be edited for asset 
                    bool ITPeriodLocked = false;
                    bool CompanyLawPeriodLocked = false;
                    bool DepreciationIsCalculatedForAssset = false;

                    List<ITPeriod> itperiodlock = new List<ITPeriod>();
                    itperiodlock = db.ITPeriods.Where(x => x.PeriodlockFlag == 1
                                    && x.Companyid == assetGroup.CompanyID).ToList();

                    ITPeriodLocked = false;

                    if (itperiodlock.Count > 0)
                    {
                        foreach (ITPeriod item in itperiodlock)
                        {
                            if (assetGroup.VoucherDate >= item.FromDate && assetGroup.VoucherDate <= item.ToDate)
                            {
                                ITPeriodLocked = true;
                                break;
                            }
                        }
                    }


                    /// Validate if Company Law Period is locked
                    /// 

                    List<SubPeriod> slist = new List<SubPeriod>();
                    slist = db.SubPeriods.Where(x => x.Companyid == assetGroup.CompanyID && x.PeriodLockFlag == "Y").ToList();
                    if (slist.Count > 0)
                    {
                        foreach (SubPeriod item in slist)
                        {
                            if (assetGroup.VoucherDate >= item.FromDate && assetGroup.VoucherDate <= item.ToDate)
                            {
                                CompanyLawPeriodLocked = true;
                                break;
                            }
                        }
                    }

                    /// Validate if Depreiation is calculated or not for this asset
                    int depcount = 0;
                    depcount = db.Depreciations.Where(x => x.Companyid == assetGroup.CompanyID
                    && x.AssetId == assetGroup.ID).Count();
                    if (depcount > 0)
                    {
                        DepreciationIsCalculatedForAssset = true;
                    }

                    Assets obj_asset;

                    obj_asset = db.Assetss.Where(x => x.ID == assetGroup.ID
                        && x.Companyid == assetGroup.CompanyID).FirstOrDefault();
                    if (obj_asset == null)
                    {
                        Message = "Asset not found.";
                        return false;
                    }

                    if (ITPeriodLocked == false && CompanyLawPeriodLocked == false
                         && DepreciationIsCalculatedForAssset == false)
                    {
                        obj_asset.AssetNo = assetGroup.AssetNo;
                        obj_asset.AssetName = assetGroup.AssetName;
                        obj_asset.VoucherNo = assetGroup.VoucherNo;

                        string parentassetno = assetGroup.Parent_AssetId.ToString();
                        if (parentassetno == "")
                        {
                            obj_asset.Parent_AssetId = 0;
                        }
                        else
                        {
                            var parentassetid = db.Assetss.Where(x => x.AssetNo == parentassetno
                                && x.Companyid == assetGroup.CompanyID).FirstOrDefault().ID;
                            obj_asset.Parent_AssetId = parentassetid;
                        }
                        obj_asset.iscomponent = assetGroup.iscomponent;

                        if (assetGroup.DtPutToUse == null)
                        {
                            obj_asset.DtPutToUse = null;
                        }
                        else
                        {
                            obj_asset.DtPutToUse = assetGroup.DtPutToUse;
                        }
                        if (assetGroup.DtPutToUseIT == null)
                        {
                            obj_asset.DtPutToUseIT = null;
                        }
                        else
                        {
                            obj_asset.DtPutToUseIT = assetGroup.DtPutToUseIT;
                        }


                        if (assetGroup.BillDate == null)
                        {
                            obj_asset.BillDate = null;
                        }
                        else
                        {

                            obj_asset.BillDate = assetGroup.BillDate;

                        }
                        if (assetGroup.ReceiptDate == null)
                        {
                            obj_asset.ReceiptDate = null;
                        }
                        else
                        {

                            obj_asset.ReceiptDate = assetGroup.ReceiptDate;

                        }

                        if (assetGroup.VoucherDate == null)
                        {
                            obj_asset.VoucherDate = null;
                        }
                        else
                        {

                            obj_asset.VoucherDate = assetGroup.VoucherDate;

                        }

                        if (assetGroup.CommissioningDate == null)
                        {
                            obj_asset.CommissioningDate = null;
                        }
                        else
                        {

                            obj_asset.CommissioningDate = assetGroup.CommissioningDate;

                        }
                        if (assetGroup.ExpiryDate == null)
                        {
                            obj_asset.ExpiryDate = null;
                        }
                        else
                        {

                            // asset.ExpiryDate = assetGroup.ExpiryDate;
                            DateTime caldate = Convert.ToDateTime(assetGroup.VoucherDate).AddYears(Convert.ToInt32(assetGroup.Usefullife));
                            if (caldate == assetGroup.ExpiryDate)
                            {
                                obj_asset.ExpiryDate = assetGroup.ExpiryDate;
                            }
                            else
                            {
                                obj_asset.ExpiryDate = caldate;
                            }


                        }


                        obj_asset.AssetName = assetGroup.AssetName;
                        obj_asset.AssetNo = assetGroup.AssetNo;
                        obj_asset.AssetIdentificationNo = assetGroup.AssetIdentificationNo;
                        obj_asset.PONo = assetGroup.PONo;
                        obj_asset.BillNo = assetGroup.BillNo;
                        obj_asset.Qty = assetGroup.Qty;
                        obj_asset.SupplierNo = assetGroup.SupplierNo;
                        //    asset.OPAccDepreciation = assetGroup.OPAccDepreciation;
                        obj_asset.UOMNo = assetGroup.UOMNo;

                        obj_asset.GrossVal = decimalToDecimal(assetGroup.GrossVal);
                        obj_asset.Discount = decimalToDecimal(assetGroup.Discount);
                        obj_asset.DutyDrawback = decimalToDecimal(assetGroup.DutyDrawback);
                        obj_asset.ServiceCharges = decimalToDecimal(assetGroup.ServiceCharges);
                        //  asset.OPAccDepreciation = assetGroup.OPAccDepreciation;
                        obj_asset.Roundingoff = decimalToDecimal(assetGroup.Roundingoff);
                        obj_asset.ExciseCredit = decimalToDecimal(assetGroup.ExciseCredit);
                        obj_asset.OtherExp = decimalToDecimal(assetGroup.OtherExp);
                        obj_asset.TotDeduction = decimalToDecimal(assetGroup.TotDeduction);
                        obj_asset.ServiceTaxCredit = decimalToDecimal(assetGroup.ServiceTaxCredit);
                        obj_asset.CustomDuty = decimalToDecimal(assetGroup.CustomDuty);

                        obj_asset.AnyOtherDutyCredit = decimalToDecimal(assetGroup.AnyOtherDutyCredit);
                        obj_asset.ExciseDuty = decimalToDecimal(assetGroup.ExciseDuty);
                        obj_asset.VATCredit = decimalToDecimal(assetGroup.VATCredit);
                        obj_asset.ServiceTax = decimalToDecimal(assetGroup.ServiceTax);
                        obj_asset.CSTCredit = decimalToDecimal(assetGroup.CSTCredit);
                        obj_asset.AnyOtherDuty = decimalToDecimal(assetGroup.AnyOtherDuty);
                        obj_asset.CGSTCredit = decimalToDecimal(assetGroup.CGSTCredit);
                        obj_asset.VAT = decimalToDecimal(assetGroup.VAT);
                        obj_asset.SGSTCredit = decimalToDecimal(assetGroup.SGSTCredit);
                        obj_asset.CSt = decimalToDecimal(assetGroup.CSt);
                        obj_asset.IGSTCredit = decimalToDecimal(assetGroup.IGSTCredit);
                        obj_asset.CGST = decimalToDecimal(assetGroup.CGST);
                        obj_asset.AnyOtherCredit = decimalToDecimal(assetGroup.AnyOtherCredit);
                        obj_asset.AnyOtherTax = decimalToDecimal(assetGroup.AnyOtherTax);
                        obj_asset.SGST = decimalToDecimal(assetGroup.SGST);
                        obj_asset.IGST = decimalToDecimal(assetGroup.IGST);
                        obj_asset.ResidualVal = decimalToDecimal(assetGroup.ResidualVal);
                        //ajinkya server side total calculation
                        obj_asset.TotalAddition = obj_asset.GrossVal + obj_asset.ServiceCharges + obj_asset.OtherExp + obj_asset.CustomDuty + obj_asset.ExciseDuty + obj_asset.ServiceTax;
                        obj_asset.TotalAddition = obj_asset.TotalAddition + obj_asset.AnyOtherDuty + obj_asset.VAT + obj_asset.CGST + obj_asset.IGST + obj_asset.SGST + obj_asset.CSt + obj_asset.AnyOtherTax;
                        obj_asset.InvoiceAmt = obj_asset.TotalAddition - obj_asset.Discount - obj_asset.Roundingoff - obj_asset.TotDeduction;
                        obj_asset.TotalCredit = obj_asset.DutyDrawback + obj_asset.ExciseCredit + obj_asset.ServiceTaxCredit + obj_asset.AnyOtherDutyCredit + obj_asset.AnyOtherCredit + obj_asset.VATCredit;
                        obj_asset.TotalCredit = obj_asset.TotalCredit + obj_asset.SGSTCredit + obj_asset.CGSTCredit + obj_asset.CSTCredit + obj_asset.IGSTCredit;
                        obj_asset.AmountCapitalised = obj_asset.InvoiceAmt - obj_asset.TotalCredit;
                        obj_asset.AmountCapitalisedCompany = obj_asset.InvoiceAmt - obj_asset.TotalCredit;
                        obj_asset.AmountCApitalisedIT = obj_asset.InvoiceAmt - obj_asset.TotalCredit;
                        ///////////
                        ////////////////////////////////////////////////////////////
                        obj_asset.Model = assetGroup.Model;
                        obj_asset.BrandName = assetGroup.BrandName;
                        obj_asset.SrNo = assetGroup.SrNo;
                        obj_asset.Remarks = assetGroup.Remarks;
                        obj_asset.IsImported = assetGroup.IsImported;
                        obj_asset.Currency = assetGroup.Currency;
                        obj_asset.Values = decimalToDecimal(assetGroup.Values);
                        //////////////////////////////////////////////////////////////
                        obj_asset.NormalRatae = decimalToDecimal(assetGroup.NormalRatae);
                        obj_asset.AdditionalRate = decimalToDecimal(assetGroup.AdditionalRate);
                        obj_asset.TotalRate = obj_asset.NormalRatae + obj_asset.AdditionalRate;
                        /////////////////////////////////////////////////////////////////
                        if (assetGroup.AccountID == null)
                        {
                            obj_asset.AccountID = 0;
                        }
                        else
                        {
                            obj_asset.AccountID = assetGroup.AccountID;
                        }
                        if (assetGroup.DepAccountId == null)
                        {
                            obj_asset.DepAccountId = 0;
                        }
                        else
                        {
                            obj_asset.DepAccountId = assetGroup.DepAccountId;
                        }
                        if (assetGroup.ITGroupIDID == null)
                        {
                            obj_asset.ITGroupIDID = 0;
                        }
                        else
                        {
                            obj_asset.ITGroupIDID = assetGroup.ITGroupIDID;
                        }


                        obj_asset.Usefullife = assetGroup.Usefullife;
                        obj_asset.YrofManufacturing = assetGroup.YrofManufacturing;
                        obj_asset.DepreciationMethod = assetGroup.DepreciationMethod;
                        obj_asset.ModifiedDate = istDate;
                        obj_asset.Modified_Userid = userId;
                        db.Entry(obj_asset).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        ////////////////////////////////////////////////////
                        //locationtable binding
                        List<Childlocation> clist = new List<Childlocation>();
                        clist = db.childlocations.Where(x => x.AssetID == assetGroup.ID && x.Companyid == assetGroup.CompanyID).ToList();
                        db.childlocations.RemoveRange(clist);
                        db.SaveChanges();

                        Childlocation childlocations = new Childlocation();
                        if (assetGroup.locationlist.Count() != 0)
                        {
                            foreach (var item in assetGroup.locationlist)
                            {
                                if (item.Date == null)
                                {
                                    item.Date = null;
                                }
                                childlocations.AssetID = assetGroup.ID;
                                childlocations.Date = item.Date;
                                childlocations.ALocID = item.ALocID;
                                childlocations.BLocID = item.BLocID;
                                childlocations.CLocID = item.CLocID;
                                childlocations.ModifiedDate = istDate;
                                childlocations.Modified_Userid = userId;
                                childlocations.Companyid = assetGroup.CompanyID;
                                db.childlocations.Add(childlocations);
                                db.SaveChanges();

                            }
                        }
                        List<Childcostcenter> costlist = new List<Childcostcenter>();
                        costlist = db.childcostcenters.Where(x => x.Ass_ID == assetGroup.ID
                                && x.Companyid == assetGroup.CompanyID).ToList();

                        db.childcostcenters.RemoveRange(costlist);
                        db.SaveChanges();
                        Childcostcenter childcostcenter = new Childcostcenter();
                        if (assetGroup.costcenterlist.Count() != 0)
                        {
                            foreach (var item in assetGroup.costcenterlist)
                            {
                                if (item.Date == null)
                                {
                                    item.Date = null;
                                }

                                childcostcenter.Ass_ID = assetGroup.ID;
                                childcostcenter.Date = item.Date;
                                childcostcenter.AcostcenterID = item.AcostcenterID;
                                childcostcenter.BcostcenterID = item.BcostcenterID;
                                childcostcenter.Percentage = item.Percentage;
                                childcostcenter.Modified_Userid = userId;
                                childcostcenter.ModifiedDate = istDate;
                                childcostcenter.Companyid = assetGroup.CompanyID;
                                db.childcostcenters.Add(childcostcenter);
                                db.SaveChanges();

                            }
                        }
                        List<Assetfreeofcost> assetlist = new List<Assetfreeofcost>();
                        assetlist = db.Assetfreeofcosts.Where(x => x.Asset_id == assetGroup.ID
                            && x.Companyid == assetGroup.CompanyID).ToList();

                        db.Assetfreeofcosts.RemoveRange(assetlist);
                        db.SaveChanges();
                        Assetfreeofcost assetfreeofcost = new Assetfreeofcost();
                        if (assetGroup.assetfreeofcostlist.Count() != 0)
                        {
                            foreach (var item in assetGroup.assetfreeofcostlist)
                            {
                                if (item.Date == null)
                                {
                                    item.Date = null;
                                }
                                // item.Asset_id = asse;
                                assetfreeofcost.Asset_id = assetGroup.ID;
                                assetfreeofcost.Date = item.Date;
                                assetfreeofcost.Description = item.Description;
                                assetfreeofcost.Uom = item.Uom;
                                assetfreeofcost.ModifiedDate = istDate;
                                assetfreeofcost.Modified_Userid = userId;
                                assetfreeofcost.Companyid = assetGroup.CompanyID;
                                db.Assetfreeofcosts.Add(assetfreeofcost);
                                db.SaveChanges();


                            }
                        }
                        //////update id lastlocation in asset table and costcenter id in asset table

                        var locid = db.childlocations.Where(x => x.AssetID == assetGroup.ID
                                && x.Companyid == assetGroup.CompanyID).OrderByDescending(x => x.ID).FirstOrDefault();

                        var costid = db.childcostcenters.Where(x => x.Ass_ID == assetGroup.ID
                        && x.Companyid == assetGroup.CompanyID).OrderByDescending(x => x.ID).FirstOrDefault();

                        Assets assetidupdate = new Assets();
                        assetidupdate = db.Assetss.Where(x => x.ID == assetGroup.ID
                        && x.Companyid == assetGroup.CompanyID).FirstOrDefault();
                        if (locid != null)
                        {
                            assetidupdate.LocAID = locid.ALocID;
                            assetidupdate.LocBID = locid.BLocID;
                            assetidupdate.LocCID = locid.CLocID;

                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            assetidupdate.LocAID = 0;
                            assetidupdate.LocBID = 0;
                            assetidupdate.LocCID = 0;

                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        if (costid != null)
                        {
                            assetidupdate.CostCenterAID = costid.AcostcenterID;
                            assetidupdate.CostCenterBID = costid.BcostcenterID;
                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            assetidupdate.CostCenterAID = 0;
                            assetidupdate.CostCenterBID = 0;
                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        transaction.Commit();

                    }
                    else
                    {

                        // only allow below fields in case of dep is calculated for asset for any period;


                        ////////////////////////////////////////////////////
                        //locationtable binding
                        List<Childlocation> clist = new List<Childlocation>();
                        clist = db.childlocations.Where(x => x.AssetID == assetGroup.ID && x.Companyid == assetGroup.CompanyID).ToList();
                        db.childlocations.RemoveRange(clist);
                        db.SaveChanges();

                        Childlocation childlocations = new Childlocation();
                        if (assetGroup.locationlist.Count() != 0)
                        {
                            foreach (var item in assetGroup.locationlist)
                            {
                                if (item.Date == null)
                                {
                                    item.Date = null;
                                }
                                childlocations.AssetID = assetGroup.ID;
                                childlocations.Date = item.Date;
                                childlocations.ALocID = item.ALocID;
                                childlocations.BLocID = item.BLocID;
                                childlocations.CLocID = item.CLocID;
                                childlocations.ModifiedDate = istDate;
                                childlocations.Modified_Userid = userId;
                                childlocations.Companyid = assetGroup.CompanyID;
                                db.childlocations.Add(childlocations);
                                db.SaveChanges();

                            }
                        }
                        List<Childcostcenter> costlist = new List<Childcostcenter>();
                        costlist = db.childcostcenters.Where(x => x.Ass_ID == assetGroup.ID
                                && x.Companyid == assetGroup.CompanyID).ToList();

                        db.childcostcenters.RemoveRange(costlist);
                        db.SaveChanges();
                        Childcostcenter childcostcenter = new Childcostcenter();
                        if (assetGroup.costcenterlist.Count() != 0)
                        {
                            foreach (var item in assetGroup.costcenterlist)
                            {
                                if (item.Date == null)
                                {
                                    item.Date = null;
                                }

                                childcostcenter.Ass_ID = assetGroup.ID;
                                childcostcenter.Date = item.Date;
                                childcostcenter.AcostcenterID = item.AcostcenterID;
                                childcostcenter.BcostcenterID = item.BcostcenterID;
                                childcostcenter.Percentage = item.Percentage;
                                childcostcenter.Modified_Userid = userId;
                                childcostcenter.ModifiedDate = istDate;
                                childcostcenter.Companyid = assetGroup.CompanyID;
                                db.childcostcenters.Add(childcostcenter);
                                db.SaveChanges();

                            }
                        }
                        List<Assetfreeofcost> assetlist = new List<Assetfreeofcost>();
                        assetlist = db.Assetfreeofcosts.Where(x => x.Asset_id == assetGroup.ID
                            && x.Companyid == assetGroup.CompanyID).ToList();

                        db.Assetfreeofcosts.RemoveRange(assetlist);
                        db.SaveChanges();
                        Assetfreeofcost assetfreeofcost = new Assetfreeofcost();
                        if (assetGroup.assetfreeofcostlist.Count() != 0)
                        {
                            foreach (var item in assetGroup.assetfreeofcostlist)
                            {
                                if (item.Date == null)
                                {
                                    item.Date = null;
                                }
                                // item.Asset_id = asse;
                                assetfreeofcost.Asset_id = assetGroup.ID;
                                assetfreeofcost.Date = item.Date;
                                assetfreeofcost.Description = item.Description;
                                assetfreeofcost.Uom = item.Uom;
                                assetfreeofcost.ModifiedDate = istDate;
                                assetfreeofcost.Modified_Userid = userId;
                                assetfreeofcost.Companyid = assetGroup.CompanyID;
                                db.Assetfreeofcosts.Add(assetfreeofcost);
                                db.SaveChanges();


                            }
                        }
                        //////update id lastlocation in asset table and costcenter id in asset table

                        var locid = db.childlocations.Where(x => x.AssetID == assetGroup.ID
                                && x.Companyid == assetGroup.CompanyID).OrderByDescending(x => x.ID).FirstOrDefault();

                        var costid = db.childcostcenters.Where(x => x.Ass_ID == assetGroup.ID
                        && x.Companyid == assetGroup.CompanyID).OrderByDescending(x => x.ID).FirstOrDefault();

                        Assets assetidupdate = new Assets();
                        assetidupdate = db.Assetss.Where(x => x.ID == assetGroup.ID
                        && x.Companyid == assetGroup.CompanyID).FirstOrDefault();
                        if (locid != null)
                        {
                            assetidupdate.LocAID = locid.ALocID;
                            assetidupdate.LocBID = locid.BLocID;
                            assetidupdate.LocCID = locid.CLocID;

                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            assetidupdate.LocAID = 0;
                            assetidupdate.LocBID = 0;
                            assetidupdate.LocCID = 0;

                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        if (costid != null)
                        {
                            assetidupdate.CostCenterAID = costid.AcostcenterID;
                            assetidupdate.CostCenterBID = costid.BcostcenterID;
                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            assetidupdate.CostCenterAID = 0;
                            assetidupdate.CostCenterBID = 0;
                            db.Entry(assetidupdate).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        transaction.Commit();

                    }

                    result = true;
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Message = "Error occurred.";
                    result = false;
                    return result;
                }
            }

        }
        public static Decimal decimalToDecimal(decimal? number)
        {
            if (number != null)
            {
                return Convert.ToDecimal(number);
            }
            else
            {
                return 0;
            }
        }
    }

}