using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class ProMasterModel
    {
        public long ProjectMasterID { get; set; }
        public long PlanId { get; set; }
        public string PlanIds { get; set; }
        public string ProjectName { get; set; }
        public string PoCategory { get; set; }
        public int? OrderNumber { get; set; }
        public long? OrderQuantity { get; set; }
        public long? TotalOrderQuantity { get; set; }
        public int? OrderQty { get; set; }
    }
}