using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Filter;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class Asset_AttachmentsController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: Attachments
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
                //ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            //List<Assets> alist = new List<Assets>();
            //alist = db.Assetss.Where(x => x.Companyid == companyid).ToList();
            //foreach (Assets item in alist)
            //{
            //    // item.Attachemntcount 
            //    var attcount = db.Child_Asset_Attachments.Where(x => x.AssetID == item.ID && x.Companyid == companyid).ToList().Count();
            //    if (attcount == 0)
            //    {
            //        item.Attachemntcount = 0;
            //    }
            //    else
            //    {
            //        item.Attachemntcount = attcount;
            //    }
            //    if (item.AGroupID != 0 || item.AGroupID == null)
            //    {
            //        item.agroupname = db.AGroups.Where(x => x.Companyid == companyid && x.ID == item.AGroupID).FirstOrDefault().AGroupName;
            //        item.groupname = item.agroupname;
            //    }
            //    if (item.BGroupID != 0 || item.BGroupID == null)
            //    {
            //        item.bgroupname = db.BGroups.Where(x => x.Companyid == companyid && x.ID == item.BGroupID).FirstOrDefault().BGroupName;
            //        item.groupname = item.bgroupname;
            //    }
            //    if (item.CGroupID != 0 || item.CGroupID == null)
            //    {
            //        item.cgroupname = db.CGroups.Where(x => x.Companyid == companyid && x.ID == item.CGroupID).FirstOrDefault().CGroupName;
            //        item.groupname = item.cgroupname;
            //    }
            //    if (item.DGroupID != 0 || item.DGroupID == null)
            //    {
            //        item.dgroupname = db.DGroups.Where(x => x.Companyid == companyid && x.ID == item.DGroupID).FirstOrDefault().DGroupName;
            //        item.groupname = item.dgroupname;

            //    }

            //}
            long size = 0;
            int attachments = 0;
            List<Child_Asset_Attachment> clist = new List<Child_Asset_Attachment>();
            clist = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid).ToList();
            attachments = clist.Count;
            foreach (Child_Asset_Attachment item in clist)
            {
                size = size + (long) item.FileSize;
                //attachments += item.ID;
            }
            ViewBag.size = FormatSize(size);
            ViewBag.attachments = attachments;
            return View();
        }
        [HttpPost]
        public ActionResult GetData()
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


            int filteredResultsCount;

            List<Assets> lstAssts = db.Assetss.Where(x => x.Companyid == companyid).ToList();



            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

            pageSize = 10;

            JsonResult result = new JsonResult();

            try
            {

                int totalRecords = db.Assetss.Where(x => x.Companyid == companyid).Count();

                List<Assets> list = new List<Assets>();

                if (search.Length == 0)
                {

                    //list = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid).ToList().Skip(startRec).Take(pageSize).ToList();

                    list = db.Assetss.Where(x => x.Companyid == companyid).ToList().Skip(startRec).Take(pageSize).ToList();

                    foreach (Assets item in list)
                    {
                        var attcount = db.Child_Asset_Attachments.Where(x => x.AssetID == item.ID && x.Companyid == companyid).ToList().Count();
                        if (attcount == 0)
                        {
                            item.Attachemntcount = 0;
                        }
                        else
                        {
                            item.Attachemntcount = attcount;
                        }
                    }
                }
                else
                {
                    list = db.Assetss.Where(x => x.Companyid == companyid && x.AssetName.StartsWith(search) || x.AssetNo.StartsWith(search)).ToList().Skip(startRec).Take(pageSize).ToList();
                    foreach (Assets item in list)
                    {
                        var attcount = db.Child_Asset_Attachments.Where(x => x.AssetID == item.ID && x.Companyid == companyid).ToList().Count();
                        if (attcount == 0)
                        {
                            item.Attachemntcount = 0;
                        }
                        else
                        {
                            item.Attachemntcount = attcount;
                        }
                    }
                }



                //foreach (var item in list)
                //{
                //    item.image_string = Convert.ToBase64String(item.File_Bytes);
                //    Assets objAsset =  lstAssts.Where(x => x.ID == item.AssetID).FirstOrDefault();
                //    if (objAsset != null)
                //    {
                //        item.assetname = objAsset.AssetName;
                //        item.assetno = objAsset.AssetNo;
                //    }


                //}


                int recFilter = list.Count;
                // Apply pagination.   


                //   var lstAssets = alist.Select(x => new { x.AssetNo, x.AssetIdentificationNo, x.AssetName, x.str_VoucherDate, x.AmountCapitalisedCompany, x.BillNo, x.Qty }).ToList();
                //  totalResultsCount = lstAssets.Count;

                filteredResultsCount = recFilter;

                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecords,
                    data = list
                }, JsonRequestBehavior.AllowGet);


                result.MaxJsonLength = int.MaxValue;

            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }


            return result;


        }
        // GET: Attachments/Details/5
        public ActionResult UploadAttachments(int id)
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
            List<Child_Asset_Attachment> calist = new List<Child_Asset_Attachment>();
            calist = db.Child_Asset_Attachments.Where(x => x.AssetID == id && x.Companyid==companyid).ToList();
            ViewBag.assetno= db.Assetss.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault().AssetNo;
            ViewBag.assetname= db.Assetss.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault().AssetName;
            ViewBag.id = id;
            foreach (Child_Asset_Attachment item in calist)
            {
                item.assetname = db.Assetss.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault().AssetName;
                item.assetno = db.Assetss.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault().AssetNo;
                item.str_uploaddate = item.UploadDate.ToString("dd/MM/yyyy");
            }
           
            return View(calist);
        }
        [HttpPost]

        [ValidateJsonAntiForgeryToken]
        [ValidateJsonXssAttribute]

        public ActionResult Post_UploadAttachments(int id,string SourceEvent,int assetno)
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
            byte[] bytes;
          // APIResponse res = new APIResponse();
            //res.
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
           // var value1 = Request.Form.Get(keys[]);
            var tnow = System.DateTime.Now.ToUniversalTime();

            JsonResult res = new JsonResult();
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(tnow.Date, istZone);
            try
            {
                HttpPostedFileBase file;
                file = null;
                HttpFileCollectionBase files = Request.Files;
              //  var sourceevent = Request["SourceEvent"];
                
                file = files[0];
                //var src = Request.Form["SourceEvent"];
                using (BinaryReader br = new BinaryReader(file.InputStream))
                {
                    bytes = br.ReadBytes(file.ContentLength);
                }
             
                Child_Asset_Attachment child_Asset_Attachment = new Child_Asset_Attachment();
                child_Asset_Attachment.AssetID = id;
                child_Asset_Attachment.Filename = file.FileName;
                child_Asset_Attachment.Ext = file.ContentType;
                child_Asset_Attachment.File_Bytes = bytes;
                child_Asset_Attachment.UploadDate = istDate;
                child_Asset_Attachment.FileSize = bytes.Length;
                child_Asset_Attachment.SourceEvent = SourceEvent;
                child_Asset_Attachment.AssetNumber = assetno;
                child_Asset_Attachment.Companyid = companyid;
                child_Asset_Attachment.CreatedUserId = userid;
                child_Asset_Attachment.CreatedDate = istDate;
                db.Child_Asset_Attachments.Add(child_Asset_Attachment);
                db.SaveChanges();
                res.Data = "Success";
           
            }
            catch(Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                //  logger.Log(LogLevel.Error, strError);
               
                res.Data = "error";
             
            }
            return res;
        }

        public ActionResult Download(int id)
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

            Child_Asset_Attachment child_Asset_Attachment = new Child_Asset_Attachment();
            child_Asset_Attachment = db.Child_Asset_Attachments.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault();
           // var document = BusinessLayer.GetDocumentsByDocument(documentId, AuthenticationHandler.HostProtocol).FirstOrDefault();
            string fileName = child_Asset_Attachment.Filename;
            return File(child_Asset_Attachment.File_Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Delete(int id)
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
            JsonResult res = new JsonResult();
            try
            {
                Child_Asset_Attachment child_Asset_Attachment = new Child_Asset_Attachment();
                child_Asset_Attachment = db.Child_Asset_Attachments.Where(x => x.ID == id && x.Companyid==companyid).FirstOrDefault();
                db.Entry(child_Asset_Attachment).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                res.Data = "Success";
                return res;
            }
            catch (Exception ex)
            {
                string strError;
                strError = ex.Message + "|" + ex.InnerException;
                //  logger.Log(LogLevel.Error, strError);

                res.Data = "error";
            }

            return res;
        }
        static readonly string[] suffixes =
            { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        public static string FormatSize(Int64 bytes)
        {
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }
    }
}
