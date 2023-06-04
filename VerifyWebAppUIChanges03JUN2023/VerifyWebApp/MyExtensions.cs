using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp
{
    public static class MyExtensions
    {
        public static Decimal ToDecimal(string number)
        {
            if (number.Length > 0)
            {
                return Convert.ToDecimal(number);
            }
            else
            {
                return 0;
            }
        }
        //public static DateTime ConvertDate(string strdate)
        //{
        //    if (strdate.Length > 0)
        //    {
        //        return Convert.ToDateTime(strdate);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
    }
}