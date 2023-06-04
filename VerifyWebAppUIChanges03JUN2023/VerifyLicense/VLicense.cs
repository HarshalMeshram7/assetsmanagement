using System;
using System.IO;
using System.Security.Cryptography;

namespace VerifyLicense
{
    public class VLicense
    {
        const string VSOFT_KEY = "VSOFT12345678912";
        public bool IsValidatLicense(string FileName , string CompanyCode)
        {


            string _CompanyCode = "";
            string _Company = "";
            string _LicExpDate = "" ;
            
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            using (StreamReader readtext = new StreamReader(fs))
            {
                _CompanyCode = readtext.ReadLine();
                _Company = readtext.ReadLine();
                _LicExpDate = readtext.ReadLine();
            }
             
           

            DateTime dtToday = DateTime.Today.Date;

            DateTime dtLicenseExp = DateTime.Today.Date.AddDays(-1);
            string strDecodedDate = DecryptString(VSOFT_KEY, _LicExpDate);
            string strDecodeComCode = DecryptString(VSOFT_KEY, _CompanyCode);

            if (CompanyCode != strDecodeComCode)
            {
                return false;
            }

            if (DateTime.TryParseExact(strDecodedDate, "ddMMyyyy",
                        System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtLicenseExp))
            {
                if (dtLicenseExp > dtToday)
                {
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
        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            string strDecoded = "";

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                strDecoded = streamReader.ReadToEnd();
                            }
                        }
                    }
                }

                return strDecoded;
            }
            catch (Exception ex)
            {
                return strDecoded;
            }
        }
    }
}
