using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VerifyWebApp.Models;
using System.Transactions;
using Newtonsoft.Json;
using NLog;
using Newtonsoft.Json.Linq;

namespace VerifyWebApp.Controllers.API
{
    public class EmployeeController : ApiController
    {
        public VerifyDB db = new VerifyDB();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        [HttpGet]
        [Route("api/searchEmployee")]
        public APIResponse SearchAsset(int companyid, string searchQuery)
        {
            List<JObject> lstOutput = new List<JObject>();

            APIResponse resp = new APIResponse();
            try
            {
                List<Employee> lstEmployees = new List<Employee>();

                searchQuery = searchQuery.ToLower();
                lstEmployees = db.Employee.Where(x => x.Companyid == companyid && 
                (x.EmpId.Contains(searchQuery) || x.FirstName.ToLower().Contains(searchQuery))).ToList();

                foreach (var item in lstEmployees)
                {
                    dynamic obj = new JObject();
                    obj.id = item.ID;
                    obj.empid = item.EmpId;
                    obj.firstname = item.FirstName;
                    obj.lastname = item.LastName;
                    obj.mobile = item.Mobileno ?? "" ;
                    obj.email= item.Emailid ?? "";

                    lstOutput.Add(obj);

                }


                string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(lstOutput);
                resp.status = "true";
                resp.data = jsonstring;
            }
            catch (Exception ex)
            {
                resp.status = "False";
                resp.data = "ERROR";
            }
            return resp;
        }


    }
}
