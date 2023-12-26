using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class Pro_HandsetAssemblyAndPackingModel
    {
        public long HandsetAssemblyId { get; set; }
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
        public Nullable<System.DateTime> SoftwareConfirmation_SDate_Auto { get; set; }
        public Nullable<System.DateTime> SoftwareConfirmation_EDate_Auto { get; set; }
        public Nullable<System.DateTime> SoftwareConfirmation_SDate_Manual { get; set; }
        public Nullable<System.DateTime> SoftwareConfirmation_EDate_Manual { get; set; }
        public Nullable<System.DateTime> RndConfirmation_SDate_Auto { get; set; }
        public Nullable<System.DateTime> RndConfirmation_EDate_Auto { get; set; }
        public Nullable<System.DateTime> RndConfirmation_SDate_Manual { get; set; }
        public Nullable<System.DateTime> RndConfirmation_EDate_Manual { get; set; }
        public Nullable<System.DateTime> AssemblyProduction_SDate_Auto { get; set; }
        public Nullable<System.DateTime> AssemblyProduction_EDate_Auto { get; set; }
        public Nullable<System.DateTime> AssemblyProduction_SDate_Manual { get; set; }
        public Nullable<System.DateTime> AssemblyProduction_EDate_Manual { get; set; }
        public Nullable<System.DateTime> Packing_SDate_Auto { get; set; }
        public Nullable<System.DateTime> Packing_EDate_Auto { get; set; }
        public Nullable<System.DateTime> Packing_SDate_Manual { get; set; }
        public Nullable<System.DateTime> Packing_EDate_Manual { get; set; }
        public Nullable<long> TotalOrderQuantity { get; set; }
        public string HandsetAssemAndPackStatus { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string MaterialRules { get; set; }
        public Nullable<long> Added { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> Updated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string HAssemblyMaterialDelayReason { get; set; }
        public string HAssemblyIqcDelayReason { get; set; }
        public string HAssemblyTrialDelayReason { get; set; }
        public string HAssemblySoftComDelayReason { get; set; }
        public string HAssemblyRndDelayReason { get; set; }
        public string HAssemblyDelayReason { get; set; }
        public string HAssemblyPackingDelayReason { get; set; }
        //
        public string HAssemblyMaterialAttachment { get; set; }
        public string HAssemblyIqcAttachment { get; set; }
        public string HAssemblySoftComAttachment { get; set; }
        public string HAssemblyRndAttachment { get; set; }
        public string HAssemblyAttachment { get; set; }
    }
}