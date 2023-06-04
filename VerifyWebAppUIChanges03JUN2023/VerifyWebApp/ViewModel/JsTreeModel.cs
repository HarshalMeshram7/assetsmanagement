using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;

namespace VerifyWebApp.ViewModel
{
    public class JsTreeModel
    {
      
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; } // if node has sub-nodes set true or not set false
      

       
    }

    public class JsTreeModel_New
    {
        public JsTreeModel_New()
        {
            // this.children = new List<JsTreeModel>();
        }
        public string id { get; set; }

        [JsonIgnore]
        public int internal_id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        //public bool children { get; set; } // if node has sub-nodes set true or not set false
        public List<JsTreeModel_New> children { get; set; }

        
    }

    public class JsTreeNode
    {
        public string id { get; set; }
        public string location { get; set; }
    }
    public class JsTreeNodeGroup
    {
        public string id { get; set; }
        public string GroupName { get; set; }
    }
    public class JsCostCenterTreeNode
    {
        public string id { get; set; }
        public string CCCode { get; set; }
        public string CCDescription { get; set; }
    }

    public class JsITGroupTreeNode
    {
        public string id { get; set; }
        public string GroupName { get; set; }
        public string DepMethod { get; set; }
        public decimal DepRate { get; set; }
        public decimal OPWDV { get; set; }
    }

    public class JsAssetGroupTreeNode
    {
        public string id { get; set; }
        public string AGroupName { get; set; }
        public string DepMethod { get; set; }
        public decimal NormalRate { get; set; }
        public decimal AdditionalRate { get; set; }
        public decimal TotalRate { get; set; }
    }
}