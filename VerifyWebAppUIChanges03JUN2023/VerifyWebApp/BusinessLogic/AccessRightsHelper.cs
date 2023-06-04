using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;

namespace VerifyWebApp.BusinessLogic
{


    public class AccessRightsHelper
    {

        public Dictionary<string, int> controller_codemap = new Dictionary<string, int>();

        public Dictionary<int, string> controller_codemap_reverse = new Dictionary<int, string>();



        const int USER_RIGHTS_UOM = 1;
        const int USER_RIGHTS_SUPPLIER = 2;
        const int USER_RIGHTS_ITGROUP = 3;
        const int USER_RIGHTS_ASSETGROUP = 4;
        const int USER_RIGHTS_PERIOD = 5;
        const int USER_RIGHTS_AMC = 6;
        const int USER_RIGHTS_INSURANCE = 7;
        const int USER_RIGHTS_ASSET_PURCHASE = 8;
        const int USER_RIGHTS_DEPRECIATION = 9;
        const int USER_RIGHTS_DISPOSAL = 10;
        const int USER_RIGHTS_ASSET_ATTACHEMENT = 11;
        const int USER_RIGHTS_BATCH = 12;
        const int USER_RIGHTS_COSTCENTER = 13;
        const int USER_RIGHTS_LOCATION = 14;
        const int USER_RIGHTS_ACCOUNTS = 15;
        const int USER_RIGHTS_LOAN = 16;
        const int USER_RIGHTS_ITPERIOD = 17;
        const int USER_RIGHTS_REPORTS = 18;
        const int USER_RIGHTS_EXPORTDATA = 19;
        const int USER_RIGHTS_EMPLOYEE = 20;
        const int USER_RIGHTS_EMPLOYEE_ASSET = 21;

        const string CONTROLLER_UOM = "UOM";
        const string CONTROLLER_SUPPLIER = "SUPPLIER";
        const string CONTROLLER_ITGROUP = "ITGROUP";
        const string CONTROLLER_ASSETGROUP = "ASSETGROUPS";
        const string CONTROLLER_PERIOD = "PERIOD";
        const string CONTROLLER_AMC = "AMC";
        const string CONTROLLER_INSURANCE = "INSURANCE";
        const string CONTROLLER_ASSET_PURCHASE = "ASSET";
        const string CONTROLLER_DEPRECIATION = "DEPRECIATION"; //DepreciationController
        const string CONTROLLER_DISPOSAL = "DISPOSAL";
        const string CONTROLLER_ASSET_ATTACHEMENT = "ASSET_ATTACHEMENTS"; //Asset_AttachmentsController
        const string CONTROLLER_BATCH = "BATCH";
        const string CONTROLLER_COSTCENTER = "COSTCENTER";
        const string CONTROLLER_LOCATION = "LOCATION";
        const string CONTROLLER_ACCOUNTS = "ACCOUNT";
        const string CONTROLLER_LOAN = "LOAN";
        const string CONTROLLER_ITPERIOD = "ITPERIOD";
        const string CONTROLLER_REPORTS = "REPORTS";
        const string CONTROLLER_EXPORTDATA = "EXPORTDATA";
        const string CONTROLLER_EMPLOYEE = "EMPLOYEE";
        const string CONTROLLER_EMPLOYEE_ASSET = "EMPLOYEEASSET";

        public AccessRightsHelper()
        {

            controller_codemap.Add(CONTROLLER_UOM, USER_RIGHTS_UOM);
            controller_codemap.Add(CONTROLLER_SUPPLIER, USER_RIGHTS_SUPPLIER);
            controller_codemap.Add(CONTROLLER_ITGROUP, USER_RIGHTS_ITGROUP);
            controller_codemap.Add(CONTROLLER_ASSETGROUP, USER_RIGHTS_ASSETGROUP);
            controller_codemap.Add(CONTROLLER_PERIOD, USER_RIGHTS_PERIOD);
            controller_codemap.Add(CONTROLLER_AMC, USER_RIGHTS_AMC);
            controller_codemap.Add(CONTROLLER_INSURANCE, USER_RIGHTS_INSURANCE);
            controller_codemap.Add(CONTROLLER_ASSET_PURCHASE, USER_RIGHTS_ASSET_PURCHASE);
            controller_codemap.Add(CONTROLLER_DEPRECIATION, USER_RIGHTS_DEPRECIATION);
            controller_codemap.Add(CONTROLLER_DISPOSAL, USER_RIGHTS_DISPOSAL);
            controller_codemap.Add(CONTROLLER_ASSET_ATTACHEMENT, USER_RIGHTS_ASSET_ATTACHEMENT);
            controller_codemap.Add(CONTROLLER_BATCH, USER_RIGHTS_BATCH);
            controller_codemap.Add(CONTROLLER_COSTCENTER, USER_RIGHTS_COSTCENTER);
            controller_codemap.Add(CONTROLLER_LOCATION, USER_RIGHTS_LOCATION);
            controller_codemap.Add(CONTROLLER_ACCOUNTS, USER_RIGHTS_ACCOUNTS);
            controller_codemap.Add(CONTROLLER_LOAN, USER_RIGHTS_LOAN);
            controller_codemap.Add(CONTROLLER_ITPERIOD, USER_RIGHTS_ITPERIOD);
            controller_codemap.Add(CONTROLLER_REPORTS, USER_RIGHTS_REPORTS);
            controller_codemap.Add("ExportData", USER_RIGHTS_EXPORTDATA);
            controller_codemap.Add(CONTROLLER_EMPLOYEE, USER_RIGHTS_EMPLOYEE);
            controller_codemap.Add(CONTROLLER_EMPLOYEE_ASSET, USER_RIGHTS_EMPLOYEE_ASSET);


            controller_codemap_reverse.Add(USER_RIGHTS_UOM, CONTROLLER_UOM);
            controller_codemap_reverse.Add( USER_RIGHTS_SUPPLIER, CONTROLLER_SUPPLIER);
            controller_codemap_reverse.Add( USER_RIGHTS_ITGROUP, CONTROLLER_ITGROUP);
            controller_codemap_reverse.Add( USER_RIGHTS_ASSETGROUP, CONTROLLER_ASSETGROUP);
            controller_codemap_reverse.Add( USER_RIGHTS_PERIOD, CONTROLLER_PERIOD);
            controller_codemap_reverse.Add( USER_RIGHTS_AMC, CONTROLLER_AMC);
            controller_codemap_reverse.Add( USER_RIGHTS_INSURANCE, CONTROLLER_INSURANCE);
            controller_codemap_reverse.Add( USER_RIGHTS_ASSET_PURCHASE, CONTROLLER_ASSET_PURCHASE);
            controller_codemap_reverse.Add( USER_RIGHTS_DEPRECIATION, CONTROLLER_DEPRECIATION);
            controller_codemap_reverse.Add( USER_RIGHTS_DISPOSAL, CONTROLLER_DISPOSAL);
            controller_codemap_reverse.Add( USER_RIGHTS_ASSET_ATTACHEMENT, CONTROLLER_ASSET_ATTACHEMENT);
            controller_codemap_reverse.Add( USER_RIGHTS_BATCH, CONTROLLER_BATCH);
            controller_codemap_reverse.Add( USER_RIGHTS_COSTCENTER, CONTROLLER_COSTCENTER);
            controller_codemap_reverse.Add( USER_RIGHTS_LOCATION, CONTROLLER_LOCATION);
            controller_codemap_reverse.Add( USER_RIGHTS_ACCOUNTS, CONTROLLER_ACCOUNTS);
            controller_codemap_reverse.Add( USER_RIGHTS_LOAN, CONTROLLER_LOAN);
            controller_codemap_reverse.Add( USER_RIGHTS_ITPERIOD, CONTROLLER_ITPERIOD);
            controller_codemap_reverse.Add( USER_RIGHTS_REPORTS, CONTROLLER_REPORTS);
            controller_codemap_reverse.Add( USER_RIGHTS_EXPORTDATA, "ExportData");
            controller_codemap_reverse.Add( USER_RIGHTS_EMPLOYEE, CONTROLLER_EMPLOYEE);
            controller_codemap_reverse.Add( USER_RIGHTS_EMPLOYEE_ASSET, CONTROLLER_EMPLOYEE_ASSET);


            



        }


        public bool IsAllowed(AccessRights objAccessRights, string currentController, string currentAction)
        {
            bool bPageAccess = false;
            try
            {

                if (objAccessRights != null)
                {

                    switch (currentController)
                    {

                        case CONTROLLER_SUPPLIER:
                            if (objAccessRights.ControllerName == USER_RIGHTS_SUPPLIER.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "SUPPLIEREXPORT" && objAccessRights.Export == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "IMPORTEXCEL" && objAccessRights.Import == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;

                        case CONTROLLER_UOM:
                            if (objAccessRights.ControllerName == USER_RIGHTS_UOM.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_LOAN:
                            if (objAccessRights.ControllerName == USER_RIGHTS_LOAN.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_PERIOD:
                            if (objAccessRights.ControllerName == USER_RIGHTS_PERIOD.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADDNEW" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_ITPERIOD:
                            if (objAccessRights.ControllerName == USER_RIGHTS_ITPERIOD.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADDNEW" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_AMC:
                            if (objAccessRights.ControllerName == USER_RIGHTS_AMC.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADDNEW" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDITNEW" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "IMPORTEXCEL" && objAccessRights.Import == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "AMCEXPORT" && objAccessRights.Export == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_INSURANCE:
                            if (objAccessRights.ControllerName == USER_RIGHTS_INSURANCE.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADDNEW" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDITNEW" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "IMPORTEXCEL" && objAccessRights.Import == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "AMCEXPORT" && objAccessRights.Export == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_ACCOUNTS:
                            if (objAccessRights.ControllerName == USER_RIGHTS_ACCOUNTS.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }


                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_BATCH:
                            if (objAccessRights.ControllerName == USER_RIGHTS_BATCH.ToString())
                            {
                                // bPageAccess = true;
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }


                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_LOCATION:
                            if (objAccessRights.ControllerName == USER_RIGHTS_LOCATION.ToString())
                            {
                                // bPageAccess = true;SaveLocationNodeEditSaveLocationNode
                                if (currentAction == "ADDSAVELOCATIONNODE" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDITSAVELOCATIONNODE" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }//
                                else if (currentAction == "LOCATIONASSETEXPORT" && objAccessRights.Export == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_COSTCENTER:
                            if (objAccessRights.ControllerName == USER_RIGHTS_COSTCENTER.ToString())
                            {
                                // bPageAccess = true;SaveLocationNodeEditSaveLocationNode
                                if (currentAction == "ADDSAVECOSTCENTERNODE" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDITSAVECOSTCENTERNODE" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }//CostCenterassetExport
                                else if (currentAction == "COSTCENTERASSETEXPORT" && objAccessRights.Export == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_ASSETGROUP:
                            if (objAccessRights.ControllerName == USER_RIGHTS_ASSETGROUP.ToString())
                            {
                                // bPageAccess = true;SaveLocationNodeEditSaveLocationNode
                                if (currentAction == "ADDSAVEASSETGROUPNODE" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDITSAVEASSETGROUPNODE" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }//CostCenterassetExport


                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_ITGROUP:
                            if (objAccessRights.ControllerName == USER_RIGHTS_ITGROUP.ToString())
                            {
                                // bPageAccess = true;SaveLocationNodeEditSaveLocationNode
                                if (currentAction == "ADDSAVELOCATIONNODE" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDITSAVELOCATIONNODE" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }//CostCenterassetExport


                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_ASSET_PURCHASE:
                            if (objAccessRights.ControllerName ==USER_RIGHTS_ASSET_PURCHASE.ToString())
                            {
                                // bPageAccess = true;SaveLocationNodeEditSaveLocationNode
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }//GroupassetExport
                                else if (currentAction == "GROUPASSETEXPORT" && objAccessRights.Export == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "IMPORTEXPORT" && objAccessRights.Import == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "CHANGEGROUP" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_DISPOSAL:
                            if (objAccessRights.ControllerName == USER_RIGHTS_DISPOSAL.ToString())
                            {
                                // bPageAccess = true;SaveLocationNodeEditSaveLocationNode
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }//GroupassetExport
                                else if (currentAction == "DISPOSALEXPORT" && objAccessRights.Export == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "IMPORTEXPORT" && objAccessRights.Import == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        case CONTROLLER_ASSET_ATTACHEMENT:
                            if (objAccessRights.ControllerName == USER_RIGHTS_ASSET_ATTACHEMENT.ToString())
                            {
                                // bPageAccess = true;SaveLocationNodeEditSaveLocationNode
                                if (currentAction == "ADD" && objAccessRights.Add == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "EDIT" && objAccessRights.Edit == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else if (currentAction == "DELETE" && objAccessRights.Delete == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }//GroupassetExport


                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        //
                        case CONTROLLER_REPORTS:
                            if (objAccessRights.ControllerName == USER_RIGHTS_REPORTS.ToString())
                            {
                                //// bPageAccess = true;ALLLOCATIONWISEREPORT_INDEX
                                //if (currentAction == "ALLLOCATIONWISEREPORT_INDEX" && objAccessRights.Index == "yes")
                                //{
                                //    bPageAccess = true;
                                //}//SINGLELOCATIONWISEREPORT_INDEX
                                //else if (currentAction == "SINGLELOCATIONWISEREPORT_INDEX" && objAccessRights.Index == "yes")
                                //{
                                //    bPageAccess = true;
                                //}
                                ////COMPONENTIAL_REPORT_INDEX
                                //else if (currentAction == "COMPONENTIAL_REPORT_INDEX" && objAccessRights.Index == "yes")
                                //{
                                //    bPageAccess = true;
                                //}
                                //else if (currentAction == "INDEX" && objAccessRights.Index == "yes")
                                //{
                                //    bPageAccess = true;
                                //}//GroupassetExport
                                //else if (currentAction == "GETFAREPORT_INDEX" && objAccessRights.Index == "yes")
                                //{
                                //    bPageAccess = true;
                                //}//SINGLELOCATIONWISEREPORT_INDEX

                                if (objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }
                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;

                        case CONTROLLER_EXPORTDATA:
                            if (objAccessRights.ControllerName ==  USER_RIGHTS_EXPORTDATA.ToString())
                            {
                                if (objAccessRights.Index == "yes")
                                {
                                    bPageAccess = true;
                                }

                                else
                                {
                                    bPageAccess = false;
                                }
                            }
                            break;
                        // bPageAccess = true;GETFAREPORT_INDEX



                        default:
                            bPageAccess = false;
                            break;
                    }


                    return bPageAccess;

                }
                else
                {
                    return false;
                }

                

            }catch(Exception ex)
            {
                return false;
            }
        }

        /*
        public string GetControllerName(string ControllerName)
        {
            //Controller Name - Name in Database
            Dictionary<string, string> lstControllerMap = new Dictionary<string, string>();

            lstControllerMap.Add("uom", "Uom");
            lstControllerMap.Add("supplier", "Supplier");
            lstControllerMap.Add("supplier", "Itgroup");

        }
        */
        public List<string> getcontrollernames()
        {
            List<string> list = new List<string>();

            list.Add(CONTROLLER_UOM);
            list.Add(CONTROLLER_SUPPLIER);
            list.Add(CONTROLLER_ITGROUP);
            list.Add(CONTROLLER_ASSETGROUP);
            list.Add(CONTROLLER_PERIOD);
            list.Add(CONTROLLER_AMC);
            list.Add(CONTROLLER_INSURANCE);
            list.Add(CONTROLLER_ASSET_PURCHASE);
            list.Add(CONTROLLER_DEPRECIATION);
            list.Add(CONTROLLER_DISPOSAL);
            list.Add(CONTROLLER_ASSET_ATTACHEMENT);
            list.Add(CONTROLLER_BATCH);
            list.Add(CONTROLLER_COSTCENTER);
            list.Add(CONTROLLER_LOCATION);
            list.Add(CONTROLLER_ACCOUNTS);
            list.Add(CONTROLLER_LOAN);
            list.Add(CONTROLLER_ITPERIOD);
            list.Add(CONTROLLER_REPORTS);
            list.Add(CONTROLLER_EXPORTDATA);
            list.Add(CONTROLLER_EMPLOYEE);
            list.Add(CONTROLLER_EMPLOYEE_ASSET);





            //list.Add("Uom");
            //list.Add("Supplier");
            //list.Add("Itgroup");
            //list.Add("Assetgroup");
            //list.Add("Period");
            //list.Add("Amc");
            //list.Add("Insurance");
            //list.Add("Purchased Assets");
            //list.Add("Depreciation");
            //list.Add("Disposal");
            //list.Add("Asset_Attachment");
            //list.Add("Batch");
            //list.Add("CostCenter");
            //list.Add("Location");
            //list.Add("Accounts");
            //list.Add("Loan");
            //list.Add("ItGroup");
            //list.Add("Itperiod");
            //list.Add("Reports");
            //list.Add("ExportData");
            //list.Add("Employee");
            //list.Add("EmployeeAsset");

            return list;
        }
    }

    
}