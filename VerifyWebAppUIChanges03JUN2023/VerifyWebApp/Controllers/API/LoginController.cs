using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VerifyWebApp.DTO;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers.API
{
    public class LoginController : ApiController
    {

        public APIResponse Login(DTO.Login login)
        {
            APIResponse response = new APIResponse();
            try
            {


                return response;
            }
            catch(Exception ex)
            {
                return response;
            }

        }
    }
}
