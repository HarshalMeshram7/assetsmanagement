using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VerifyWebApp.Models;

namespace VerifyWebApp.Controllers
{
    public class ImageGalleryController : Controller
    {
        public VerifyDB db = new VerifyDB();
        // GET: ImageGallery
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }
            return View();
        }
        
        // GET: Attachments
        public ActionResult AssetGallery()
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
                // ViewBag.LoggedCompany = company.CompanyName;
            }
            else
            {
                return RedirectToAction("CompanySelection", "Company");
            }

            List<Child_Asset_Attachment> clist = new List<Child_Asset_Attachment>();
            // clist = db.Child_Asset_Attachments.Where(x=>x.Ext=="image/jpg" || x.Ext == "image/jpeg"  || x.Ext == "image/png" || x.Ext == "jpg"  && x.Companyid==companyid).OrderByDescending(x=>x.AssetID).ToList();

            ////clist = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid).OrderByDescending(x => x.Ass_Id).ToList();

            long filesize = 0;
            var imagecount = 0;

            // Mandar 03 FEB 2021

            imagecount = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid).Count();
            if  (imagecount > 0)
            {
                filesize = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid).Sum(x => x.FileSize ?? 0);
            }else
            {
                filesize = 0;
            }
            
            // changed by Mandar 03 FEB 2021, fix error of sum in case no records in asset image table



            //if (clist.Count > 0)
            //{
            //    foreach (Child_Asset_Attachment item in clist)
            //    {


            //        item.assetno = db.Assetss.Where(x => x.ID == item.AssetID && x.Companyid == companyid).FirstOrDefault().AssetNo;
            //        item.assetname = db.Assetss.Where(x => x.ID == item.AssetID && x.Companyid == companyid).FirstOrDefault().AssetName;
            //        filesize += item.FileSize;
            //        var agroupid=db.Assetss.Where(x => x.ID == item.AssetID && x.Companyid == companyid).FirstOrDefault().AGroupID;
            //        if (agroupid != 0 && agroupid!=null)
            //        {
            //            item.groupname = db.AGroups.Where(x => x.Companyid == companyid && x.ID == agroupid).FirstOrDefault().AGroupName;
            //            var bgroupid = db.Assetss.Where(x => x.ID == item.AssetID && x.Companyid == companyid).FirstOrDefault().BGroupID;
            //            if (bgroupid != 0 && bgroupid!=null)
            //            {
            //                item.groupname = db.BGroups.Where(x => x.Companyid == companyid && x.ID == bgroupid).FirstOrDefault().BGroupName;
            //                var cgroupid = db.Assetss.Where(x => x.ID == item.AssetID && x.Companyid == companyid).FirstOrDefault().CGroupID;
            //                if (cgroupid != 0 && cgroupid!=null)
            //                {
            //                    item.groupname = db.CGroups.Where(x => x.Companyid == companyid && x.ID == cgroupid).FirstOrDefault().CGroupName;
            //                    var dgroupid = db.Assetss.Where(x => x.ID == item.AssetID && x.Companyid == companyid).FirstOrDefault().DGroupID;
            //                    if (dgroupid != 0 && dgroupid!=null)
            //                    {
            //                        item.groupname = db.DGroups.Where(x => x.Companyid == companyid && x.ID == dgroupid).FirstOrDefault().DGroupName;

            //                    }
            //                }
            //            }
            //        }
            //        imagecount++;
            //    }
            //}


            var convertedsize = FormatSize(filesize);
            ViewBag.filesize = convertedsize;
            ViewBag.imagecount = imagecount;
            return View(clist);


        }

        [HttpPost]
        public ActionResult GetImageData()
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

            pageSize = 3;

            JsonResult result = new JsonResult();

            try
            {

                int totalRecords = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid).Count();

                List<Child_Asset_Attachment> list = new List<Child_Asset_Attachment>();

                if (search.Length == 0)
                {

                    //list = db.Child_Asset_Attachments.Where(x => x.Companyid == companyid).ToList().Skip(startRec).Take(pageSize).ToList();

                    list = (from a  in db.Assetss
                                          join img_asset in db.Child_Asset_Attachments
                                          on a.ID equals img_asset.AssetID
                                          where a.Companyid == companyid
                                          && img_asset.Ext == "image/jpg" || img_asset.Ext == "image/jpeg" || img_asset.Ext == "image/png" || img_asset.Ext == "jpg"
                                          select new { a, img_asset }).AsEnumerable()
                                          .Select (e=> new Child_Asset_Attachment
                                          {
                                              assetno = e.a.AssetNo,
                                              assetname = e.a.AssetName,
                                              image_string = Convert.ToBase64String(e.img_asset.File_Bytes)
                                          }).ToList().Skip(startRec).Take(pageSize).ToList();

                    
                }
                else
                {
                    list = (from a in db.Assetss
                            join img_asset in db.Child_Asset_Attachments
                            on a.ID equals img_asset.AssetID
                            where a.Companyid == companyid && (a.AssetName.StartsWith(search) || a.AssetNo.StartsWith(search))
                            && img_asset.Ext == "image/jpg" || img_asset.Ext == "image/jpeg" || img_asset.Ext == "image/png" || img_asset.Ext == "jpg"
                            select new { a, img_asset }).AsEnumerable()
                                         .Select(e => new Child_Asset_Attachment
                                         {
                                             assetno = e.a.AssetNo,
                                             assetname = e.a.AssetName,
                                             image_string = Convert.ToBase64String(e.img_asset.File_Bytes)
                                         }).ToList().Skip(startRec).Take(pageSize).ToList();
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

        // GET: ImageGallery/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImageGallery/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageGallery/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageGallery/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImageGallery/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ImageGallery/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImageGallery/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
