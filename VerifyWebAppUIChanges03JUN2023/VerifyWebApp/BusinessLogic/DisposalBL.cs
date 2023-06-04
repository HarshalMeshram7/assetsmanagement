using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;

namespace VerifyWebApp.BusinessLogic
{
    public class DisposalBL
    {
        public VerifyDB db = new VerifyDB();

        /// <summary>
        /// Return true if valid disposal else return false in case invalid
        /// </summary>
        /// <param name="strDisposalDate"></param>
        /// <param name="disposaltype"></param>
        /// <param name="assetno"></param>
        /// <returns></returns>
        public bool IsValidateDisposalEntry(DateTime strDisposalDate,string assetno)
        {
            
           // DateTime DisposalDate;//= DateTime.Parse(strDisposalDate);
            //string Str_DisposalDate = Convert.ToDateTime(strDisposalDate).ToString("yyyy-MM-dd");
            
            //if (DateTime.TryParseExact(Str_DisposalDate, "yyyy-MM-dd",
            //System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DisposalDate))
            //{ DisposalDate = DisposalDate; }

                Assets assets = db.Assetss.Where(x => x.AssetNo == assetno).FirstOrDefault();

                if (strDisposalDate < assets.VoucherDate && strDisposalDate < assets.DtPutToUse)
                {
                    return false; // disposal is NOT valid for this asset
                }
                else
                {
                    return true; // disposal is valid for this asset
                }
             
        }
        //public bool IsValidateDisposalEntry(string strDisposalDate, string disposaltype,string assetno)
        //{

        //    DateTime DisposalDate = Convert.ToDateTime(strDisposalDate);

        //    Assets assets = db.Assetss.Where(x => x.AssetNo == assetno).FirstOrDefault();

        //    if (DisposalDate < assets.VoucherDate && DisposalDate < assets.DtPutToUse)
        //    {
        //        return false; // disposal is NOT valid for this asset
        //    }
        //    else
        //    {
        //        return true; // disposal is valid for this asset
        //    }

        //}
        /// <summary>
        /// below code is for displaying value of the disposed asset till date
        /// </summary>
        /// <param name="strAssetNo"></param>
        /// <param name="strDisposalDate"></param>
        /// <returns></returns>
        public DisposalViewModel GetDisplayValue(string strAssetNo, DateTime strDisposalDate)
        {

            DisposalViewModel disposalviewmodel = new DisposalViewModel();
            decimal? GrossValue = 0;
            
            string DepMethod;
            decimal? TotalRate = 0;
            decimal? depreciatontilldate = 0;
            decimal? disposaltilldate = 0;


          //  DateTime DisposalDate = DateTime.Parse(strDisposalDate);

            Assets assets = db.Assetss.Where(x => x.AssetNo == strAssetNo).FirstOrDefault();

            if (assets != null)
            {
                GrossValue = assets.AmountCapitalisedCompany;
              
                DepMethod = assets.DepreciationMethod;
                TotalRate = assets.TotalRate;
                disposalviewmodel.TotalRate = TotalRate;
                disposalviewmodel.DepMethod = DepMethod;
                disposalviewmodel.AssetAmtCapitalised = GrossValue;


                // 17 feb 2021
                disposalviewmodel.OpeningQty = assets.Qty ?? 0;



                List<Depreciation> lstdepreciation = new List<Depreciation>();

                lstdepreciation = db.Depreciations.Where(x => x.AssetId == assets.ID && x.ToDate <= strDisposalDate).ToList();
                if (lstdepreciation.Count != 0)
                {

                    foreach (var item in lstdepreciation)
                    {
                        /// new depreciaton field
                        depreciatontilldate += item.Amount;
                        disposalviewmodel.depreciationtilldate = depreciatontilldate;

                    }
                    // OpDepreciation2 = DepOpt1 - OpDepreciation1

                }
                else
                {
                    disposalviewmodel.depreciationtilldate = 0;

                }

                List<Disposal> lstdisp = new List<Disposal>();
                int v_disposedqty = 0;

                lstdisp = db.Disposals.Where(x => x.AssetId == assets.ID && x.DisposalDate <= strDisposalDate).ToList();
                if (lstdisp.Count != 0)
                {

                    foreach (var item in lstdisp)
                    {
                        /// new disposaltilldate field
                        disposaltilldate += item.DisposalAmount;
                        disposalviewmodel.disposaltilldate= disposaltilldate;
                        v_disposedqty = v_disposedqty + item.Qty;
                    }
                    // OpDepreciation2 = DepOpt1 - OpDepreciation1
                    disposalviewmodel.DisposedQtyTillDate = v_disposedqty;
                }
                else
                {
                    disposalviewmodel.disposaltilldate = 0;
                    disposalviewmodel.DisposedQtyTillDate = 0;

                }
                

            }

            return disposalviewmodel;

        }
        public DisposalViewModel GetDepreciationreversed(string strAssetNo, DateTime strDisposalDate, string disposaltype)
        {

            DisposalViewModel disposalviewmodel = new DisposalViewModel();
            decimal? disposalamount = 0;
           


         //   DateTime DisposalDate = DateTime.Parse(strDisposalDate);

            if (disposaltype == "Full")
            {
                List<Disposal> lstdisp = new List<Disposal>();
                Assets asset = new Assets();
                 asset = db.Assetss.Where(x => x.AssetNo == strAssetNo).FirstOrDefault();
                decimal disposaltilldate = 0;
                lstdisp = db.Disposals.Where(x => x.AssetId == asset.ID && x.DisposalDate <= strDisposalDate).ToList();
                if (lstdisp.Count != 0)
                {

                    foreach (var item in lstdisp)
                    {
                        /// new disposaltilldate field
                        disposaltilldate += item.DisposalAmount;

                    }
                    // OpDepreciation2 = DepOpt1 - OpDepreciation1

                }
                disposalviewmodel.depreciationreversed = asset.AmountCapitalisedCompany - disposaltilldate;

            }
            if (disposaltype == "Partial")
            {
                disposalviewmodel.depreciationreversed = 0;
            }


            return disposalviewmodel;

        }
        //public DisposalViewModel GetDepreciationreversed(string strAssetNo, string strDisposalDate,string disposaltype)
        //{

        //    DisposalViewModel disposalviewmodel = new DisposalViewModel();
        //    decimal? GrossValue = 0;
        //    decimal? OpAccumulatedTillDDate = 0;
        //    decimal? OpAccumulated = 0;
        //    decimal? DepChargeFromDDateToDepToDate = 0;
        //    decimal? TotalDepreciation = 0;
        //    decimal? NetAmount = 0;
        //    decimal? DepTillDate = 0;
        //    string DepMethod;
        //    decimal? TotalRate = 0;
        //    decimal? depreciatontilldate = 0;
        //    decimal? disposaltilldate = 0;


        //    DateTime DisposalDate = Convert.ToDateTime(strDisposalDate);

        //    Assets assets = db.Assetss.Where(x => x.AssetNo == strAssetNo).FirstOrDefault();

        //    if (assets != null)
        //    {
        //        GrossValue = assets.AmountCapitalisedCompany;
        //        //OpAccumulatedTillDDate = assets.OPAccDepreciation;
        //        //OpAccumulated = assets.OPAccDepreciation;
        //        DepMethod = assets.DepreciationMethod;
        //        TotalRate = assets.TotalRate;

        //        // Additiona to Asset till disposal date

        //        List<Addition> lstaddition = new List<Addition>();
        //        lstaddition = db.Additions.Where(x => x.AssetId == assets.ID && x.VoucherDate <= DisposalDate).ToList();


        //        if (lstaddition.Count != 0)
        //        {

        //            foreach (var item in lstaddition)
        //            {
        //                GrossValue += item.AmountCapitalisedCompany;
        //                // GrossValue++;
        //            }

        //        }


        //        List<Depreciation> lstdepreciation = new List<Depreciation>();

        //        lstdepreciation = db.Depreciations.Where(x => x.AssetId == assets.ID && x.ToDate <= DisposalDate).ToList();
        //        if (lstdepreciation.Count != 0)
        //        {

        //            foreach (var item in lstdepreciation)
        //            {

        //                OpAccumulatedTillDDate += item.Amount; //Asset + Depreciation (OpAccDep)                      
        //                OpAccumulated += item.Amount;//Only Depreciation (OpAccDep)

        //            }
        //            // OpDepreciation2 = DepOpt1 - OpDepreciation1

        //        }

        //        SubPeriod subperiod = new SubPeriod();
        //        var SubId = db.SubPeriods.Where(x => x.FromDate <= DisposalDate && DisposalDate <= x.ToDate).FirstOrDefault().ID;
        //        subperiod = db.SubPeriods.Where(x => x.ID == SubId).FirstOrDefault();
        //        DateTime DepFrom;
        //        DateTime PurDisDt;
        //        int count = 0;
        //        DateTime assetDate = Convert.ToDateTime(assets.VoucherDate);

        //        if (assets.VoucherDate > subperiod.FromDate)
        //        {
        //            DepFrom = assetDate;
        //            PurDisDt = assetDate;
        //        }
        //        else
        //        {
        //            DepFrom = subperiod.FromDate;
        //            PurDisDt = subperiod.FromDate;
        //        }

        //        //count = Convert.ToInt32(ddate - DepFrom);
        //        count = Convert.ToInt32((DisposalDate - DepFrom).TotalDays);

        //        //---------------------------------------------------------------------------
        //        DepChargeFromDDateToDepToDate = OpAccumulated - OpAccumulatedTillDDate;
        //        if (DepMethod == "SLM")
        //        {
        //            DepTillDate = GrossValue * TotalRate / 100 / 365 * count;
        //        }
        //        if (DepMethod == "WDV")
        //        {
        //            DepTillDate = (GrossValue - OpAccumulatedTillDDate) * TotalRate / 100 / 365 * count;
        //            //(BookVal1 - Dis:OpDepreciation1) *Ass:TotRate / 100 / 365 * Count
        //        }
        //        //TotalDepreciation = OpAccumulated + DepTillDate;
        //        NetAmount = GrossValue - TotalDepreciation;
        //        //------------------------------------------------------------------------
        //        disposalviewmodel.AssetAmtCapitalised = GrossValue;
        //        disposalviewmodel.OpAccDepTillDipodate = OpAccumulatedTillDDate;
        //        disposalviewmodel.OpAccDepreciation = OpAccumulated;
        //        disposalviewmodel.DepChargeFromDDateToDepToDate = DepChargeFromDDateToDepToDate;
        //        disposalviewmodel.DepMethod = DepMethod;
        //        disposalviewmodel.TotalRate = TotalRate;


        //    }
        //    return disposalviewmodel;

        //}

    }
}