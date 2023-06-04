using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace VerifyWebApp.Models
{
    public class VerifyDB:DbContext
    {
        public  VerifyDB() : base("DBContext")
        {
            //return new VerifyDB();
             Database.Log = sql => System.Diagnostics.Debug.Write(sql);
        }

        //17sep2020
        public System.Data.Entity.DbSet<VerifyWebApp.Models.DepCalculation> DepCalculation { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.DepreciationLog> DepreciationLog { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.DepreciationRequest> DepreciationRequest { get; set; }
        //
        /// <summary>
        /// //
        /// </summary>
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Employee> Employee { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.EmployeeAsset> EmployeeAsset { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.BatchVerification> BatchVerification { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.CompanyPermission> CompanyPermissions { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.License> Licenses { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Child_Asset_Attachment> Child_Asset_Attachments { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Subbatch> SubBatchs { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.BatchLocation> BatchLocations { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Assetfreeofcost> Assetfreeofcosts { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Childcostcenter> childcostcenters { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Childlocation> childlocations { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.SubLoan> SubLoans { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.SubAmc> SubAmc { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.SubInsurance> SubInsurances { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Account> Accounts { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.ACostCenter> ACostCenters { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Addition> Additions { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.AGroup> AGroups { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.ALocation> ALocations { get; set; }
        //public System.Data.Entity.DbSet<VigilantAPI.Models.ClientGroup> ClientGroups { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Client> Clients { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.AMC> AMCss { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Assets> Assetss { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Batch> Batchs { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.BCostCenter> BCostCenters { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.BGroup> BGroups { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.BLocation> BLocations { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.CGroup> CGroups { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.CLocation> CLocations { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Company> Companys { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Depreciation> Depreciations { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.DGroup> DGroups { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Disposal> Disposals { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Insurance> Insurances { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.ITGroup> ITGroups { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.ITPeriod> ITPeriods { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Loan> Loans { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Login> Logins { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Period> Periods { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.SubPeriod> SubPeriods { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.Supplier> Suppliers { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.UOM> UOMs { get; set; }
        public System.Data.Entity.DbSet<VerifyWebApp.Models.AccessRights> AccessRights { get; set; }

        public System.Data.Entity.DbSet<VerifyWebApp.Models.ITDepreciation> ITDepreciation { get; set; } // Added on 02 dec 2020

        public System.Data.Entity.DbSet<VerifyWebApp.Models.UserEventLog> UserEventLogs { get; set; } // Added on 02 dec 2020

        public System.Data.Entity.DbSet<VerifyWebApp.Models.AuditLogRecord> AuditLogs { get; set; }


        public System.Data.Entity.DbSet<VerifyWebApp.Models.BatchAsset> BatchAssets { get; set; }

        public System.Data.Entity.DbSet<VerifyWebApp.Models.DepreciationRequestIncomeTax> DepreciationRequestIncomeTax { get; set; }

    }
}