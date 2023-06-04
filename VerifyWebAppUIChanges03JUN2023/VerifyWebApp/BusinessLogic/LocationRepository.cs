using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.DTO;
using System.Data.Entity;

namespace VerifyWebApp.BusinessLogic
{
    public class LocationRepository
    {
        private VerifyDB db;

        public LocationRepository(VerifyDB databaseContext)
        {
            this.db = databaseContext;
        }

        public bool ImportLocations(List<LocationDTO> lstLocations,int companyid,int userid,bool UpdateAssetLocation)
        {
            bool bResult = false;


            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {


                    foreach (var item in lstLocations)
                    {

                            // check if already exists if not then insert location
                        
                        // LEVEL 1
                           
                        ALocation temp_aLocation = db.ALocations.Where(x => x.ALocationName.Equals(item.ALocationName)).FirstOrDefault();
                        if (temp_aLocation == null)
                        {

                            ALocation aLocation = new ALocation();
                            aLocation.Companyid = companyid;
                            aLocation.CreatedUserId = userid;
                            aLocation.CreatedDate = DateTime.Today;

                            aLocation.ALocationName = item.ALocationName;

                            db.ALocations.Add(aLocation);
                            db.SaveChanges();

                            item.ALocID = aLocation.ID;

                        }
                        else
                        {
                            item.ALocID = temp_aLocation.ID;
                        }

                        
                        if (item.BLocationName.Length > 0 && item.ALocationName.Length > 0)
                        {

                      
                            BLocation temp_bLocation = db.BLocations.Where(x => x.BLocationName.Equals(item.BLocationName)
                                        && x.ALocID == item.ALocID
                                     ).FirstOrDefault(); 

                            if (temp_bLocation == null)
                            {

                                BLocation bLocation = new BLocation();
                                bLocation.Companyid = companyid;
                                bLocation.CreatedUserId = userid;
                                bLocation.CreatedDate = DateTime.Today;

                                bLocation.BLocationName = item.BLocationName;
                                bLocation.ALocID = item.ALocID;

                                db.BLocations.Add(bLocation);
                                db.SaveChanges();
                                item.BLocID = bLocation.ID;

                            }
                            else
                            {
                                item.BLocID = temp_bLocation.ID;
                            }
                        }

                        if (item.CLocationName.Length > 0 && item.BLocationName.Length > 0)
                        {
                            CLocation temp_cLocation = db.CLocations.Where(x => x.CLocationName.Equals(item.CLocationName)
                                  && x.ALocID == item.ALocID
                                  && x.BLocID == item.BLocID
                               ).FirstOrDefault();


                            if (temp_cLocation == null)
                            {

                                CLocation cLocation = new CLocation();
                                cLocation.Companyid = companyid;
                                cLocation.CreatedUserId = userid;
                                cLocation.CreatedDate = DateTime.Today;

                                cLocation.CLocationName = item.CLocationName;
                                cLocation.BLocID = item.BLocID;
                                cLocation.ALocID = item.ALocID;

                                db.CLocations.Add(cLocation);
                                db.SaveChanges();
                                item.CLocID = cLocation.ID;
                            }
                            else
                            {
                                item.BLocID = temp_cLocation.ID;
                            }

                        }

                        // TODO Code to update  Asset Locations

                        if (UpdateAssetLocation)
                        {
                            Assets temp_asset = db.Assetss.Where(x => x.AssetNo == item.AssetNo
                                && x.Companyid == companyid).FirstOrDefault();

                            if (temp_asset != null)
                            {
                                Childlocation childlocation = new Childlocation();
                                if (item.ALocID > 0)
                                {
                                    childlocation.ALocID = item.ALocID;
                                }
                                if (item.BLocID > 0)
                                {
                                    childlocation.BLocID = item.BLocID;
                                }

                                if (item.CLocID > 0)
                                {
                                    childlocation.CLocID = item.CLocID;
                                }

                                temp_asset.LocAID = item.ALocID;
                                temp_asset.LocBID = item.BLocID;
                                temp_asset.LocCID = item.CLocID;

                                db.Entry(temp_asset).State = EntityState.Modified;
                                db.SaveChanges();


                                childlocation.AssetID = temp_asset.ID;
                                childlocation.Date = DateTime.Today;
                                childlocation.Companyid = companyid;
                                childlocation.CreatedDate = DateTime.Today;
                                childlocation.CreatedUserId = userid;

                                if (childlocation.ALocID != 0)
                                {
                                    db.childlocations.Add(childlocation);
                                    db.SaveChanges();
                                }

                            }
                        }
                    }
                    transaction.Commit();
                    bResult = true;
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    bResult = false;
                }
            }
            return bResult;

        }
    }

}