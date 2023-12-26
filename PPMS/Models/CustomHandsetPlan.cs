using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPMS.Models
{
    public class CustomHandsetPlan
    {
        public long? AsmPlanId { get; set; }
        public long? AsmId { get; set; }
        public long? BbPlanId { get; set; }
        public long? BhPlanId { get; set; }
        public long? SmtPlanId { get; set; }
        public long ProjectMasterID { get; set; }
        public string AsmProjectName { get; set; }
        public string ProjectName { get; set; }
        public string PoCategory { get; set; }
        public int? OrderNumber { get; set; }
        public long? OrderQuantity { get; set; }

        //checkbox check
        public bool SmtChk { get; set; }
        public bool HouseChk { get; set; }
        public bool BatteryChk { get; set; }
        public bool AssemblyChk { get; set; }
        public string ActiveStatus { get; set; }

        //Assembly
        public long HandsetAssemblyId { get; set; }
        public long PlanId { get; set; }
        public DateTime? MaterialReceive_SDate_AutoAssembly { get; set; }
        public DateTime? MaterialReceive_EDate_AutoAssembly { get; set; }
        public DateTime? MaterialReceive_SDate_ManualAssembly { get; set; }
        public DateTime? MaterialReceive_EDate_ManualAssembly { get; set; }
        public DateTime? Iqc_SDate_AutoAssembly { get; set; }
        public DateTime? Iqc_EDate_AutoAssembly { get; set; }
        public DateTime? Iqc_SDate_ManualAssembly { get; set; }
        public DateTime? Iqc_EDate_ManualAssembly { get; set; }
        public DateTime? Trial_SDate_AutoAssembly { get; set; }
        public DateTime? Trial_EDate_AutoAssembly { get; set; }
        public DateTime? Trial_SDate_ManualAssembly { get; set; }
        public DateTime? Trial_EDate_ManualAssembly { get; set; }
        public DateTime? SoftwareConfirmation_SDate_Auto { get; set; }
        public DateTime? SoftwareConfirmation_EDate_Auto { get; set; }
        public DateTime? SoftwareConfirmation_SDate_Manual { get; set; }
        public DateTime? SoftwareConfirmation_EDate_Manual { get; set; }
        public DateTime? RndConfirmation_SDate_Auto { get; set; }
        public DateTime? RndConfirmation_EDate_Auto { get; set; }
        public DateTime? RndConfirmation_SDate_Manual { get; set; }
        public DateTime? RndConfirmation_EDate_Manual { get; set; }
        public DateTime? AssemblyProduction_SDate_Auto { get; set; }
        public DateTime? AssemblyProduction_EDate_Auto { get; set; }
        public DateTime? AssemblyProduction_SDate_Manual { get; set; }
        public DateTime? AssemblyProduction_EDate_Manual { get; set; }
        public DateTime? Packing_SDate_Auto { get; set; }
        public DateTime? Packing_EDate_Auto { get; set; }
        public DateTime? Packing_SDate_Manual { get; set; }
        public DateTime? Packing_EDate_Manual { get; set; }
        public long? TotalOrderQuantity { get; set; }
        public string HandsetAssemAndPackStatus { get; set; }
        public bool? IsActive { get; set; }
        public string MaterialRules { get; set; }
        //Battery
        public long HandsetBatteryId { get; set; }
        public DateTime? ReliabilityTest_SDate_Auto { get; set; }
        public DateTime? ReliabilityTest_EDate_Auto { get; set; }
        public DateTime? ReliabilityTest_SDate_Manual { get; set; }
        public DateTime? ReliabilityTest_EDate_Manual { get; set; }
        public DateTime? MassProduction_SDate_Auto { get; set; }
        public DateTime? MassProduction_EDate_Auto { get; set; }
        public DateTime? MassProduction_SDate_Manual { get; set; }
        public DateTime? MassProduction_EDate_Manual { get; set; }
        public DateTime? AgingTest_SDate_Auto { get; set; }
        public DateTime? AgingTest_EDate_Auto { get; set; }
        public DateTime? AgingTest_SDate_Manual { get; set; }
        public DateTime? AgingTest_EDate_Manual { get; set; }
        public string HandsetBatteryStatus { get; set; }
       //Housing
        public long HandsetHousingId { get; set; }
        public string HandsetHousingStatus { get; set; }
        public DateTime? MaterialReceive_SDate_AutoHousing { get; set; }
        public DateTime? MaterialReceive_EDate_AutoHousing { get; set; }
        public DateTime? MaterialReceive_SDate_ManualHousing { get; set; }
        public DateTime? MaterialReceive_EDate_ManualHousing { get; set; }
        public DateTime? Iqc_SDate_AutoHousing { get; set; }
        public DateTime? Iqc_EDate_AutoHousing { get; set; }
        public DateTime? Iqc_SDate_ManualHousing { get; set; }
        public DateTime? Iqc_EDate_ManualHousing { get; set; }
        public DateTime? Trial_SDate_AutoHousing { get; set; }
        public DateTime? Trial_EDate_AutoHousing { get; set; }
        public DateTime? Trial_SDate_ManualHousing { get; set; }
        public DateTime? Trial_EDate_ManualHousing { get; set; }
        public DateTime? ReliabilityTest_SDate_AutoHousing { get; set; }
        public DateTime? ReliabilityTest_EDate_AutoHousing { get; set; }
        public DateTime? ReliabilityTest_SDate_ManualHousing { get; set; }
        public DateTime? ReliabilityTest_EDate_ManualHousing { get; set; }
        public DateTime? MassProduction_SDate_AutoHousing { get; set; }
        public DateTime? MassProduction_EDate_AutoHousing { get; set; }
        public DateTime? MassProduction_SDate_ManualHousing { get; set; }
        public DateTime? MassProduction_EDate_ManualHousing { get; set; }

        //Smt
        public long HandsetSmtId { get; set; }
        public string HandsetSmtStatus { get; set; }
        public DateTime? MaterialReceive_SDate_AutoSmt { get; set; }
        public DateTime? MaterialReceive_EDate_AutoSmt { get; set; }
        public DateTime? MaterialReceive_SDate_ManualSmt { get; set; }
        public DateTime? MaterialReceive_EDate_ManualSmt { get; set; }
        public DateTime? Iqc_SDate_AutoSmt { get; set; }
        public DateTime? Iqc_EDate_AutoSmt { get; set; }
        public DateTime? Iqc_SDate_ManualSmt { get; set; }
        public DateTime? Iqc_EDate_ManualSmt { get; set; }
        public DateTime? Trial_SDate_AutoSmt { get; set; }
        public DateTime? Trial_EDate_AutoSmt { get; set; }
        public DateTime? Trial_SDate_ManualSmt { get; set; }
        public DateTime? Trial_EDate_ManualSmt { get; set; }
        public DateTime? MassProduction_SDate_AutoSmt { get; set; }
        public DateTime? MassProduction_EDate_AutoSmt { get; set; }
        public DateTime? MassProduction_SDate_ManualSmt { get; set; }
        public DateTime? MassProduction_EDate_ManualSmt { get; set; }

        //Battery
        public DateTime? MaterialReceive_SDate_AutoBattery { get; set; }
        public DateTime? MaterialReceive_EDate_AutoBattery { get; set; }
        public DateTime? MaterialReceive_SDate_ManualBattery { get; set; }
        public DateTime? MaterialReceive_EDate_ManualBattery { get; set; }
        public DateTime? Iqc_SDate_AutoBattery { get; set; }
        public DateTime? Iqc_EDate_AutoBattery { get; set; }
        public DateTime? Iqc_SDate_ManualBattery { get; set; }
        public DateTime? Iqc_EDate_ManualBattery { get; set; }
        public DateTime? Trial_SDate_AutoBattery { get; set; }
        public DateTime? Trial_EDate_AutoBattery { get; set; }
        public DateTime? Trial_SDate_ManualBattery { get; set; }
        public DateTime? Trial_EDate_ManualBattery { get; set; }
        public DateTime? ReliabilityTest_SDate_AutoBattery { get; set; }
        public DateTime? ReliabilityTest_EDate_AutoBattery { get; set; }
        public DateTime? ReliabilityTest_SDate_ManualBattery { get; set; }
        public DateTime? ReliabilityTest_EDate_ManualBattery { get; set; }
        public DateTime? MassProduction_SDate_AutoBattery { get; set; }
        public DateTime? MassProduction_EDate_AutoBattery { get; set; }
        public DateTime? MassProduction_SDate_ManualBattery { get; set; }
        public DateTime? MassProduction_EDate_ManualBattery { get; set; }
        public DateTime? AgingTest_SDate_AutoBattery { get; set; }
        public DateTime? AgingTest_EDate_AutoBattery { get; set; }
        public DateTime? AgingTest_SDate_ManualBattery { get; set; }
        public DateTime? AgingTest_EDate_ManualBattery { get; set; }
        public string HAssemblyMaterialDelayReason { get; set; }
        public string HAssemblyIqcDelayReason { get; set; }
        public string HAssemblyTrialDelayReason { get; set; }
        public string HAssemblySoftComDelayReason { get; set; }
        public string HAssemblyRndDelayReason { get; set; }
        public string HAssemblyDelayReason { get; set; }
        public string HAssemblyPackingDelayReason { get; set; }
        public string HBatteryMaterialDelayReason { get; set; }
        public string HBatteryIqcDelayReason { get; set; }
        public string HBatteryTrialDelayReason { get; set; }
        public string HBatteryReliabilityDelayReason { get; set; }
        public string HBatteryMpDelayReason { get; set; }
        public string HBatteryAgingDelayReason { get; set; }
        public string HBatteryPackingDelayReason { get; set; }
        public string HHousingMaterialDelayReason { get; set; }
        public string HHousingIqcDelayReason { get; set; }
        public string HHousingTrialDelayReason { get; set; }
        public string HHousingReliabilityDelayReason { get; set; }
        public string HHousingMpDelayReason { get; set; }
        public string HHousingPackingDelayReason { get; set; }
        public string HandsetSmtDelayReason { get; set; }
        public string HandsetIqcDelayReason { get; set; }
        public string HandsetTrialDelayReason { get; set; }
        public string HandsetMpDelayReason { get; set; }
        public string HandsetPackingDelayReason { get; set; }
    }
}