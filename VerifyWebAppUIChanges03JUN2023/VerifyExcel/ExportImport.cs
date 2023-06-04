using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C1.C1Excel;
using System.IO;

namespace VerifyExcel
{

    // generate template 
    public class ExportImport
    {
        public bool ExportAssetTemplate(string strPathFileName)
        {
            try
            {
                C1XLBook _c1xl = new C1.C1Excel.C1XLBook();
                _c1xl.Sheets.Clear();
                XLSheet sheet = _c1xl.Sheets.Add("Template");

                const int COL_SRNO = 0;
                const int COL_ASSETNO = 1;
                const int COL_ASSETNAME = 2;
                const int COL_VOUCHERDATE = 3;
                const int COL_DATEPUTTOUSE = 4;
                const int COL_DATEPUTTOUSE_IT = 5;

                
                sheet.Columns[COL_SRNO].Width = 2000;
                sheet.Columns[COL_ASSETNO].Width = 2000;
                sheet.Columns[COL_ASSETNAME].Width = 2000;
                sheet.Columns[COL_VOUCHERDATE].Width = 2000;
                sheet.Columns[COL_ASSETNO].Width = 2000;
                sheet.Columns[COL_DATEPUTTOUSE].Width = 2000;
                sheet.Columns[COL_DATEPUTTOUSE_IT].Width = 2000;

                int row = 0;
                sheet[row, COL_SRNO].Value = "SrNo";
                sheet[row, COL_ASSETNO].Value = "AssetNo";
                sheet[row, COL_ASSETNAME].Value = "AssetName";
                sheet[row, COL_VOUCHERDATE].Value = "VoucherDate";
                sheet[row, COL_DATEPUTTOUSE].Value = "Date Put To Use";
                sheet[row, COL_DATEPUTTOUSE_IT].Value = "Date put to Use IT";

                //string filename = Server.MapPath("~") + "\\Temp\\testexcel" + uid + ".xls";
                //_c1xl.CompatibilityMode = CompatibilityMode.NoLimits;
                _c1xl.Save(strPathFileName, FileFormat.OpenXml);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }


        public bool ImportAssetQuick(byte[] data)
        {

            C1XLBook _c1xl = new C1.C1Excel.C1XLBook();

            Stream stream = new MemoryStream(data);

            _c1xl.Load(stream,FileFormat.OpenXml);
            int RowCounter = 1;
            XLSheet sheet = _c1xl.Sheets[0];

            foreach (XLRow oRow in sheet.Rows)
            {
                
                XLCell cell = sheet.GetCell(RowCounter, 0);
                XLCell cell1 = sheet.GetCell(RowCounter, 4);
                XLCell cell2 = sheet.GetCell(RowCounter, 5);



            }

            return true;

        }
    }

}
