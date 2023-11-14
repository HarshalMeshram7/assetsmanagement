
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using System.Web;

namespace VerifyWebApp.Models
{    [Table("tblchild_asset_attachment")]
    public class Child_Asset_Attachment
    {
        [Key]
        public int ID { get; set; }
        public int? CreatedUserId { get; set; }
        public int? Companyid { get; set; }
        public int? Modified_Userid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int AssetNumber   { get; set; } // asset number assgiend by client users
        public int AssetID { get; set; } // internal asset id from tblAsset
        public string  Filename { get; set; }
        public string Ext  { get; set; }
        public string SourceEvent { get; set; }
        public DateTime UploadDate { get; set; }
        public long? FileSize { get; set; }
        //[AllowFileSize(FileSize = 5 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 5 MB")]
        public byte[] File_Bytes  { get; set; }
        [NotMapped]
        public string assetname { get; set; }
        [NotMapped]
        public string assetno { get; set; }
        [NotMapped]
        public string str_uploaddate { get; set; }
        [NotMapped]
        public string groupname { get; set; }
        [NotMapped]
        public string image_string { get; set; }

    }
    public class ChildAssetAttachment2
    {
        [Key]
        public int ID { get; set; }
        public string Filename { get; set; }
        public long? FileSize { get; set; }
        public int assetno { get; set; } // asset number assgiend by client users
        public byte[] File_Bytes { get; set; }
        public string assetname { get; set; }
        [NotMapped]
        public string image_string { get; set; }
        
        //[AllowFileSize(FileSize = 5 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 5 MB")]





    }
}