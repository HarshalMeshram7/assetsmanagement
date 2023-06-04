using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;
using VerifyWebApp.ViewModel;



namespace VerifyWebApp.Controllers
{
    public class ITDepreciationController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: ITDepreciation

        [AuthUser]
        public ActionResult Index()
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            List<ITPeriod> itperiod = new List<ITPeriod>();
           
            try
            {

                //  lstadd = db.Depreciations.Where(x => x.Companyid == companyid).ToList();
                 itperiod = db.ITPeriods.Where(x => x.Companyid == companyid && x.DepFlag == "Y").ToList();
                foreach (ITPeriod item in itperiod)
                {
                    item.str_fromdate = Convert.ToDateTime(item.FromDate).ToString("dd/MM/yyyy");
                    item.str_todate = Convert.ToDateTime(item.ToDate).ToString("dd/MM/yyyy");
                }



              
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);

            }
           
            return View(itperiod);
        }
        [HttpGet]
        public ActionResult CalculateDepreciation()
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            //-------------------------------------------------
            ITPeriod lstperiod = new ITPeriod();
            lstperiod = db.ITPeriods.Where(x => x.DepFlag == "N" && x.Companyid == companyid).FirstOrDefault();
            if (lstperiod != null)
            {
                ViewBag.fromdate = lstperiod.FromDate.ToString("dd/MM/yyyy");
                ViewBag.todate = lstperiod.ToDate.ToString("dd/MM/yyyy");
            }
            else
            {
                ViewBag.fromdate = "";
                ViewBag.todate = "";

                // lstpobjperiod.str_fromdate = "";
                //lstpobjperiod.str_todate = "";
            }
            ViewBag.userid = userid;
            //---------------------------------------------------

            return PartialView(lstperiod);
        }

        [HttpPost]

        public ActionResult startcalculation(DateTime startdate, DateTime enddate,decimal roundoff,string cutoff)
        {

            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }


            List<ITDepreciation> DepList = new List<ITDepreciation>();
            //  alist=BusinessLogic.ReportRepository.
            BusinessLogic.DepreciationCalculationRepository reportRepository = new BusinessLogic.DepreciationCalculationRepository();

            //DepList = reportRepository.Getdepcal(companyid, startdate, enddate, userid);
            bool bresult;
            bresult = reportRepository.StartCalculationRequest_IncomeTax(companyid, startdate, enddate, userid);
            //bresult = reportRepository.StartCalculationITDepreciation(companyid, startdate, enddate, userid, cutoff);





            return new JsonResult()
            {

                Data = new { result = bresult }
            };



        }
        [AuthUser]

        public ActionResult RemoveDepreciationIndex()
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }


            //  List<SubPeriod> splist = new List<SubPeriod>();
            string checkdepexists = "";
            //check if deprecaition there validation message
            var itlist = db.ITPeriods.Where(x => x.Companyid == companyid && x.DepFlag == "Y").OrderByDescending(x => x.ToDate).First();

            if (itlist != null)
            {
                ViewBag.Fromdate = Convert.ToDateTime(itlist.FromDate).ToString("dd/MM/yyyy");
                ViewBag.Todate = Convert.ToDateTime(itlist.ToDate).ToString("dd/MM/yyyy");
                ViewBag.SubperiodId = itlist.ID;
                checkdepexists = "yes";

            }
            else
            {
                checkdepexists = "no";
            }
            ViewBag.checkdepexists = checkdepexists;

            return PartialView();
        }
        [AuthUser]
        [HttpPost]
        public ActionResult RemoveDepreciation(DateTime fromdate, DateTime todate, int subperiodid)
        {

            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            JsonResult res;
            res = new JsonResult();
            try
            {
                ITPeriod sp = new ITPeriod();
                sp = db.ITPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
                if (sp.PeriodlockFlag == 1)
                {
                    res.Data = "Periodlock";
                    return res;
                }
                else
                {
                   
                    string startdate = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");
                    string enddate = Convert.ToDateTime(todate).ToString("yyyy-MM-dd");

                    //string strSQL = "";
                    //strSQL = "Call removeitdepreciation(";
                    //strSQL = strSQL + companyid + ",";
                    //strSQL = strSQL + "'" + startdate + "'," + "'" + enddate + "')";

                    string strSQL = " delete from tblitdepreciation where FromDate >= '" + startdate + "' and ToDate <= '" + enddate + "' and companyid =" + companyid;
                    strSQL = strSQL + " and id > 0 ";

                    int rec = db.Database.ExecuteSqlCommand(strSQL);
                    if (rec > 0)
                    {
                        
                            ITPeriod modifiedperiod = new ITPeriod();
                            modifiedperiod = db.ITPeriods.Where(x => x.Companyid == companyid && x.ID == subperiodid).FirstOrDefault();
                            if (modifiedperiod != null)
                            {
                                modifiedperiod.DepFlag = "N";
                                db.Entry(modifiedperiod).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                            res.Data = "Success";
                       
                    }
                }




                return res;
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                // logger.Log(LogLevel.Error, strError);
                res.Data = "Failed";
                return res;
            }

            
        }
        // GET: ITDepreciation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ITDepreciation/Create
        public ActionResult ViewITDepreciation(int id)
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);

            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            List<ITDepreciation> itdep = new List<ITDepreciation>();
            try {

                var itperiod = db.ITPeriods.Where(x => x.Companyid == companyid && x.ID == id).FirstOrDefault();
                ViewBag.itperiodid = itperiod.ID;
                ViewBag.fromdate = itperiod.FromDate.ToString("dd/MM/yyyy");
                ViewBag.todate = itperiod.ToDate.ToString("dd/MM/yyyy"); ;
                itdep = db.ITDepreciation.Where(x => x.Companyid == companyid && x.FromDate == itperiod.FromDate && x.ToDate == itperiod.ToDate).ToList();

            }


            catch (Exception ex)
            {

            }


            return View(itdep);
        }

        [HttpGet]
        [AuthUser]
        public ActionResult ITDepreciationExport(int id)
        {
            int userid = 0;
            Login user = (Login)(Session["PUser"]);
            int itperiodid = id;
            if (user != null)
            {
                ViewBag.LogonUser = user.UserName;
                userid = user.ID;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            int companyid = 0;
            Company company = (Company)(Session["Cid"]);

            if (company != null)
            {
                ViewBag.LoggedCompany = company.CompanyName;
                companyid = company.ID;
                ViewBag.companyid = companyid;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            Response.ClearContent();
            Response.BinaryWrite(generateitdepreciationexcel(companyid,itperiodid));
            string excelName = "ITDepreciation";
            Response.AddHeader("content-dispostion", "attachment;filename=" + excelName + ".xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
            return RedirectToAction("ViewITDepreciation", "ITDepreciation");

        }
        
        public byte[] generateitdepreciationexcel(int companyid,int itperiodid)
        {
           
            List<ITDepreciation> lstins = new List<ITDepreciation>();
           
            var itperiod = db.ITPeriods.Where(x => x.Companyid == companyid && x.ID == itperiodid).FirstOrDefault();

            lstins = db.ITDepreciation.Where(x => x.Companyid == companyid && x.FromDate == itperiod.FromDate && x.ToDate == itperiod.ToDate).ToList();

           

            //foreach (var item in lstins)
            //{
            //    item.Srno = srno;

            //    srno++;
            //}

            var memoryStream = new MemoryStream();
            byte[] data;
            //ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {

                excel.Workbook.Worksheets.Add("Worksheet1");
                excel.Workbook.Worksheets.Add("Worksheet2");

                string[] headerRow ={ "Group Name", "Depreciation Rate", "Op WDV","Addition during year(>=180 days)","Addition during year(<180 days) ",
                        "Disposal < 180",  "Disposal > 180", "Total","Depreciation Half","Depreciation Full","Amount","WDV end"};


                // Determine the header range (e.g. A1:D1)
               // string headerRange = "C1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";


                // Target a worksheet
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                var worksheet2 = excel.Workbook.Worksheets["Worksheet2"];
                var currentSheet = excel.Workbook.Worksheets;
                // Popular header row data
              //  worksheet.Cells[headerRange].LoadFromArrays(headerRow);
                worksheet.Cells[1, 1].Value = "Depreciation IT law";
                worksheet.Cells[2, 2].Value = "FromDate:  " + itperiod.FromDate + " " + "ToDate:" + itperiod.ToDate;

                int col = 1;
                for (int i = 0; i <= headerRow.Count() - 1; i++)
                {

                    worksheet.Cells[4, col].Value = headerRow[i];
                    col++;

                }
                int rowIterator = 5;


               // int rowIterator = 4;
                foreach (var item in lstins)
                {
                    worksheet.Cells[rowIterator, 1].Value = item.ITGroupName;
                    worksheet.Cells[rowIterator, 2].Value = item.DepreciationRate;
                    worksheet.Cells[rowIterator, 3].Value = item.OpeningWDV;
                    worksheet.Cells[rowIterator, 4].Value = item.Additionbefore;
                    worksheet.Cells[rowIterator, 5].Value = item.AdditionAfter;
                    worksheet.Cells[rowIterator, 6].Value = item.DisposalBefore;
                    worksheet.Cells[rowIterator, 7].Value = item.DisposalAfter;

                    worksheet.Cells[rowIterator, 8].Value = item.FinalTotal;
                    worksheet.Cells[rowIterator, 9].Value = item.DepHalf;
                    worksheet.Cells[rowIterator, 10].Value = item.DepFull;

                    worksheet.Cells[rowIterator, 11].Value = item.TotalDep;
                    worksheet.Cells[rowIterator, 12].Value = item.ClosingWDV;



                    rowIterator = rowIterator + 1;

                }

                return excel.GetAsByteArray();

            }
        }

    }
}
