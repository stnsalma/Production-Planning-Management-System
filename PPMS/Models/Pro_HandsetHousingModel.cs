using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class Pro_HandsetHousingModel
    {
        public Pro_HandsetHousingModel()
        {
            HHousingMaterialFlsDetails=new List<FilesDetail>();
            HHousingMaterialFlsDetails=new List<FilesDetail>();
            HHousingTrialFlsDetails=new List<FilesDetail>();
            HHousingReliabilityFlsDetails=new List<FilesDetail>();
            HHousingMpFlsDetails=new List<FilesDetail>();
        }
        public List<FilesDetail> HHousingMaterialFlsDetails { get; set; }
        public List<FilesDetail> HHousingIqcFlsDetails { get; set; }
        public List<FilesDetail> HHousingTrialFlsDetails { get; set; }
        public List<FilesDetail> HHousingReliabilityFlsDetails { get; set; }
        public List<FilesDetail> HHousingMpFlsDetails { get; set; }

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
        //
        public string HHousingMaterialAttachment { get; set; }
        public string HHousingIqcAttachment { get; set; }
        public string HHousingTrialAttachment { get; set; }
        public string HHousingReliabilityAttachment { get; set; }
        public string HHousingMpAttachment { get; set; }
    }
}