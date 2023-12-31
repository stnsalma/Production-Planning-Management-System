//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PPMS.DAL.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pro_HandsetHousing
    {
        public long HandsetHousingId { get; set; }
        public long PlanId { get; set; }
        public Nullable<long> ProjectMasterID { get; set; }
        public string ProjectName { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public string PoCategory { get; set; }
        public Nullable<System.DateTime> MaterialReceive_SDate_Auto { get; set; }
        public Nullable<System.DateTime> MaterialReceive_EDate_Auto { get; set; }
        public Nullable<System.DateTime> MaterialReceive_SDate_Manual { get; set; }
        public Nullable<System.DateTime> MaterialReceive_EDate_Manual { get; set; }
        public Nullable<System.DateTime> Iqc_SDate_Auto { get; set; }
        public Nullable<System.DateTime> Iqc_EDate_Auto { get; set; }
        public Nullable<System.DateTime> Iqc_SDate_Manual { get; set; }
        public Nullable<System.DateTime> Iqc_EDate_Manual { get; set; }
        public Nullable<System.DateTime> Trial_SDate_Auto { get; set; }
        public Nullable<System.DateTime> Trial_EDate_Auto { get; set; }
        public Nullable<System.DateTime> Trial_SDate_Manual { get; set; }
        public Nullable<System.DateTime> Trial_EDate_Manual { get; set; }
        public Nullable<System.DateTime> ReliabilityTest_SDate_Auto { get; set; }
        public Nullable<System.DateTime> ReliabilityTest_EDate_Auto { get; set; }
        public Nullable<System.DateTime> ReliabilityTest_SDate_Manual { get; set; }
        public Nullable<System.DateTime> ReliabilityTest_EDate_Manual { get; set; }
        public Nullable<System.DateTime> MassProduction_SDate_Auto { get; set; }
        public Nullable<System.DateTime> MassProduction_EDate_Auto { get; set; }
        public Nullable<System.DateTime> MassProduction_SDate_Manual { get; set; }
        public Nullable<System.DateTime> MassProduction_EDate_Manual { get; set; }
        public Nullable<System.DateTime> Packing_SDate_Auto { get; set; }
        public Nullable<System.DateTime> Packing_EDate_Auto { get; set; }
        public Nullable<System.DateTime> Packing_SDate_Manual { get; set; }
        public Nullable<System.DateTime> Packing_EDate_Manual { get; set; }
        public Nullable<long> TotalOrderQuantity { get; set; }
        public string HandsetHousingStatus { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string MaterialRules { get; set; }
        public Nullable<long> Added { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> Updated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string HHousingMaterialDelayReason { get; set; }
        public string HHousingIqcDelayReason { get; set; }
        public string HHousingTrialDelayReason { get; set; }
        public string HHousingReliabilityDelayReason { get; set; }
        public string HHousingMpDelayReason { get; set; }
        public string HHousingPackingDelayReason { get; set; }
        public string HHousingMaterialAttachment { get; set; }
        public string HHousingIqcAttachment { get; set; }
        public string HHousingTrialAttachment { get; set; }
        public string HHousingReliabilityAttachment { get; set; }
        public string HHousingMpAttachment { get; set; }
    }
}
