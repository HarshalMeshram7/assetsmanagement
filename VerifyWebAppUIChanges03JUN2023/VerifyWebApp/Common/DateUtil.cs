using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyWebApp.Common
{
    public class DateUtil
    {
        public bool GetDate(string strDate, out DateTime tDate)
        {
            DateTime dtValidDate;
            //if (DateTime.TryParseExact(strDate, "yyyyMMdd",System.Globalization.CultureInfo.InvariantCulture,
            if (DateTime.TryParseExact(strDate, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtValidDate))
            {
                tDate = dtValidDate;
                return true;
            }
            else
            {
                tDate = DateTime.MinValue;
                return false;
            }
        }
    }
}