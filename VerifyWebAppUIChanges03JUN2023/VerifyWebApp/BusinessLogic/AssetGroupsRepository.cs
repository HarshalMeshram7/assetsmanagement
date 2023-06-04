using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;
using VerifyWebApp.Validators;

namespace VerifyWebApp.BusinessLogic
{
    /// <summary>
    /// manage Asset groups
    /// 14 Jul 2022
    /// </summary>
    public class AssetGroupsRepository
    {
        private VerifyDB db;

        public AssetGroupsRepository(VerifyDB database)
        {
            this.db = database;
        }

        // save group
        public bool SaveGroup()
        {

            try
            {



                return true;
            }catch(Exception ex)
            {

                return false;
            }

        }

        public bool SaveAGroup(AGroup group)
        {
            try
            {


                AGroupValidator validator = new AGroupValidator();
                var result = validator.Validate(group);
                if (result.IsValid)
                {

                    db.AGroups.Add(group);
                    db.SaveChanges();
                    return true;
                }else
                {
                    return false;
                }

                

            }catch(Exception ex)
            {
                // TO
                return false;
            }
        }

        public bool SaveBGroup(BGroup group)
        {
            try
            {

                // validate agroup id present in database

                BGroupValidator validator = new BGroupValidator();
                var result = validator.Validate(group);
                if (result.IsValid)
                {

                    // Check FK Validation
                    AGroup parentAGroup = db.AGroups.Where(x => x.ID == group.AGrpID && x.Companyid == group.Companyid).FirstOrDefault();

                    if (parentAGroup != null)
                    {
                        db.BGroups.Add(group);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                // TO
                return false;
            }
        }


        public bool SaveCGroup(CGroup group)
        {
            try
            {
                CGroupValidator validator = new CGroupValidator();
                var result = validator.Validate(group);
                if (result.IsValid)
                {

                    db.CGroups.Add(group);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                // TO
                return false;
            }
        }

        public bool SaveDGroup(DGroup group)
        {
            try
            {
                DGroupValidator validator = new DGroupValidator();
                var result = validator.Validate(group);
                if (result.IsValid)
                {

                    db.DGroups.Add(group);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                // TO
                return false;
            }
        }
    }
}