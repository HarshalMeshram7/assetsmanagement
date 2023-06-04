using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VerifyWebApp.Models;
using System.Transactions;
using Newtonsoft.Json;
using NLog;

namespace VerifyWebApp.Controllers.API
{
    public class AppUserController : ApiController
    {

        public VerifyDB db = new VerifyDB();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        [HttpGet]
        [Route("api/appuser/isappaccess")]
        public APIResponse AppAccess(string appuserid)
        {
            APIResponse resp = new APIResponse();
            //List<Batch> lstBatch = new List<Batch>();
           
                try
                {

                    //'b5efe6a6-0608-4209-a941-8230577fc989'

                    Login user = new Login();
                //List<Login> lstLogin = db.Logins.ToList();

                //user = lstLogin.Where(x => x.AppUserid.ToLower().Equals(appuserid.ToLower())).FirstOrDefault();

                 //user = db.Logins.Where(x => x.AppUserid == appuserid).FirstOrDefault();

                String strSQL = "select * from tbllogin where AppUserid='" + appuserid +"'" ;


                user = db.Database.SqlQuery<VerifyWebApp.Models.Login>(strSQL).FirstOrDefault();

                //filteredProductTransactions = db.Database.SqlQuery<VSpaSoft.Models.ProductTransaction>(sqlquery).ToArray();

                if (user != null)
                    {
                        if (user.IsAppAccess == "Yes")
                        {
                            resp.status = "True";
                            resp.data = "Valid Access";
                        }
                        else
                        {
                            resp.status = "False";
                            resp.data = "No Valid Access";
                        }

                    }else
                    {
                            resp.status = "False";
                            resp.data = "User not found";
                    }
                }
                catch (Exception ex)
                {
                    string strError;
                    strError = ex.Message + "|" + ex.InnerException;
                    logger.Log(LogLevel.Error, strError);


                    resp.status = "False";
                    resp.data = "ERROR" + strError;

                    //return resp;


                }
            
            return resp;
        }



        [HttpGet]
        [Route("api/appuser/v1/isappaccess")]
        public APIResponse AppAccessV1(string mobile)
        {
            APIResponse resp = new APIResponse();
            //List<Batch> lstBatch = new List<Batch>();

            try
            {

                //'b5efe6a6-0608-4209-a941-8230577fc989'

                Login user = null;

                user = db.Logins.Where(x => x.MobileNo == mobile).FirstOrDefault();

              
                if (user != null)
                {
                    if (user.IsAppAccess == "Yes")
                    {
                        resp.status = "True";
                        resp.data = "Valid Access";
                    }
                    else
                    {
                        resp.status = "False";
                        resp.data = "No Valid Access";
                    }

                }
                else
                {
                    resp.status = "False";
                    resp.data = "User not found";
                }
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);


                resp.status = "False";
                resp.data = "ERROR" + strError;

                //return resp;


            }

            return resp;
        }


        [HttpGet]
        [Route("api/appuser/commpanylist")]
        public APIResponse GeCompanyList(string appuserid)
        {
            APIResponse resp = new APIResponse();
           
            try
            {

                
                List<Company> lstCompany = new List<Company>();

                lstCompany = db.Companys.ToList();

                resp.status = "True";
                resp.data = JsonConvert.SerializeObject(lstCompany); 


            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);


                resp.status = "False";
                resp.data = "ERROR";

                //return resp;


            }

            return resp;
        }

        [HttpGet]
        [Route("api/appuser/checkvalid")]
        public APIResponse CheckvalidUser(string mobilenumber)
        {

            APIResponse resp = new APIResponse();

            try
            {

                Login login = db.Logins.Where(x => x.MobileNo == mobilenumber).FirstOrDefault();

                if (login != null)
                {
                    resp.status = "True";
                    resp.data = JsonConvert.SerializeObject(login);

                }
                else
                {
                    resp.status = "False";
                    resp.data = JsonConvert.SerializeObject(login);

                }
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);


                resp.status = "False";
                resp.data = "ERROR";

                //return resp;


            }

            return resp;
        }


        [HttpGet]
        [Route("api/appuser/updateappuserid")]
        public APIResponse UpdateAppUserID(string mobilenumber,string guid)
        {

            APIResponse resp = new APIResponse();

            try
            {

                Login login = db.Logins.Where(x => x.MobileNo == mobilenumber).FirstOrDefault();

                if (login != null)
                {


                    login.AppUserid = guid;
                    db.Entry(login).State = EntityState.Modified;
                    db.SaveChanges();

                    resp.status = "True";
                    resp.data = JsonConvert.SerializeObject(login);



                }
                else
                {
                    resp.status = "False";
                    resp.data = JsonConvert.SerializeObject(login);

                }
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);


                resp.status = "False";
                resp.data = "ERROR";

                //return resp;


            }

            return resp;
        }



        /// <summary>
        /// Login API from Mobile App 
        /// for clients who self hosted on their network
        /// </summary>
        /// <param name="mobilenumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public APIResponse AppLogin(string mobilenumber,string password)
        {

            APIResponse resp = new APIResponse();
            //List<Batch> lstBatch = new List<Batch>();

            try
            {

                Login user = new Login();
                user = db.Logins.Where(x => x.MobileNo == mobilenumber).FirstOrDefault();

                if (user.IsAppAccess == "Yes")
                {
                    resp.status = "True";
                    resp.data = "Valid Access";

                    
                }
                else
                {
                    resp.status = "False";
                    resp.data = "No Valid Access";
                }

            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                logger.Log(LogLevel.Error, strError);


                resp.status = "False";
                resp.data = "ERROR";

                //return resp;


            }

            return resp;

        }

    }
}
