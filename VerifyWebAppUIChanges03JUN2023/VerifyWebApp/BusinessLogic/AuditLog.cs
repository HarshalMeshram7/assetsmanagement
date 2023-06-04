using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerifyWebApp.Models;

namespace VerifyWebApp.BusinessLogic
{
    public class AuditLog
    {

        public const int Event_Login = 1;
        public  const int Event_Logout = 2;
        public  const int Event_Insert = 3;
        public  const int Event_Update = 4;
        public const int Event_Delete = 5;


        public const int Record_Type_CompanyLawGroup = 101;
        public const int Record_Type_ITGroups = 102;
        public const int Record_Type_Location = 103;
        public const int Record_Type_PeriodCompanyLaw = 104;
        public const int Record_Type_PeriodITLaw = 105;
        public const int Record_Type_Supplier = 106;
        public const int Record_Type_UOM = 107;
        public const int Record_Type_AccountDetails = 108;
        public const int Record_Type_EmployeeDetails = 109;
        public const int Record_Type_VerificationBatch = 110;
        const int Record_Type_LoanDetails = 111;
        const int Record_Type_InsuranceDetails = 112;
        const int Record_Type_AMCDetails = 113;



        public Dictionary<string, AuditRecord> values  { get; set; }

        public AuditLog()
        {
            values = new Dictionary<string, AuditRecord>();
        }

        public void SaveRecord(string _column,string _oldvalue,string _newvalue)
        {
            AuditRecord record = new AuditRecord();
            record.column = _column;
            record.oldvalue = _oldvalue;
            record.newvalue = _newvalue;

            values.Add(_column, record);



        }

        public bool InsertLog(int UserID,int CompanyID ,int EventID,int RecordType,
            VerifyDB context)
        {

            try
            {
                

                Guid guid = Guid.NewGuid();


                foreach (var item in values)
                {

                    AuditLogRecord record = new AuditLogRecord();

                    record.guid = guid.ToString();
                    record.eventid = EventID;
                    record.recordtype = RecordType;
                    record.userid = UserID;
                    record.companyid = CompanyID;
                    record.trandate = DateTime.Today;
                    record.column = item.Key;
                    AuditRecord audit = item.Value;
                    record.oldvalue = audit.oldvalue;
                    record.newvalue = audit.newvalue;


                    context.AuditLogs.Add(record);

                }
                //db.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                // Log ex
                return false;
            }

        }
    }
}