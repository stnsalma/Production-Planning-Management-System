using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class Pro_MaterialRulesModel
    {
        public long MaterialRulesId { get; set; }
        public string Product { get; set; }
        public string ProductionUnit { get; set; }
        public string TypeOfWork { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> SerialNo { get; set; }
        public Nullable<int> ProductionUnitSerialNo { get; set; }
        public string MaterialRules { get; set; }
        public Nullable<int> Day_1 { get; set; }
        public Nullable<int> Day_2 { get; set; }
        public Nullable<int> Day_3 { get; set; }
        public Nullable<int> Day_4 { get; set; }
        public Nullable<int> Day_5 { get; set; }
        public Nullable<int> Day_6 { get; set; }
        public Nullable<int> Day_7 { get; set; }
        public Nullable<int> Day_8 { get; set; }
        public Nullable<int> Day_9 { get; set; }
        public Nullable<int> Day_10 { get; set; }
        public Nullable<int> Day_11 { get; set; }
        public Nullable<int> Day_12 { get; set; }
        public Nullable<long> Added { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> Updated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> StartDays { get; set; }
        public Nullable<int> EndDays { get; set; }
    }
}