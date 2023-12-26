using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class Pro_HandsetBatteryModel
    {
        public Pro_HandsetBatteryModel()
        {
            HBatteryMtlFlsDetails=new List<FilesDetail>();
            HBatteryIqcFlsDetails=new List<FilesDetail>();
            HBatteryTrialFlsDetails=new List<FilesDetail>();
            HBatteryReliabilityFlsDetails=new List<FilesDetail>();
            HBatteryMpFlsDetails=new List<FilesDetail>();
            HBatteryAgingFlsDetails=new List<FilesDetail>();
        }
        public List<FilesDetail> HBatteryMtlFlsDetails { get; set; }
        public List<FilesDetail> HBatteryIqcFlsDetails { get; set; }
        public List<FilesDetail> HBatteryTrialFlsDetails { get; set; }
        public List<FilesDetail> HBatteryReliabilityFlsDetails { get; set; }
        public List<FilesDetail> HBatteryMpFlsDetails { get; set; }
        public List<FilesDetail> HBatteryAgingFlsDetails { get; set; }

        public long HandsetBatteryId { get; set; }
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
        public Nullable<System.DateTime> AgingTest_SDate_Auto { get; set; }
        public Nullable<System.DateTime> AgingTest_EDate_Auto { get; set; }
        public Nullable<System.DateTime> AgingTest_SDate_Manual { get; set; }
        public Nullable<System.DateTime> AgingTest_EDate_Manual { get; set; }
        public Nullable<System.DateTime> Packing_SDate_Auto { get; set; }
        public Nullable<System.DateTime> Packing_EDate_Auto { get; set; }
        public Nullable<System.DateTime> Packing_SDate_Manual { get; set; }
        public Nullable<System.DateTime> Packing_EDate_Manual { get; set; }
        public Nullable<long> TotalOrderQuantity { get; set; }
        public string HandsetBatteryStatus { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string MaterialRules { get; set; }
        public Nullable<long> Added { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> Updated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string HBatteryMaterialDelayReason { get; set; }
        public string HBatteryIqcDelayReason { get; set; }
        public string HBatteryTrialDelayReason { get; set; }
        public string HBatteryReliabilityDelayReason { get; set; }
        public string HBatteryMpDelayReason { get; set; }
        public string HBatteryAgingDelayReason { get; set; }
        public string HBatteryPackingDelayReason { get; set; }
        //Attachment
        public string HBatteryMaterialAttachment { get; set; }
        public string HBatteryIqcAttachment { get; set; }
        public string HBatteryTrialAttachment { get; set; }
        public string HBatteryReliabilityAttachment { get; set; }
        public string HBatteryMpAttachment { get; set; }
        public string HBatteryAgingAttachment { get; set; }
    }
}