using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPMS.DAL.DbModel;
using PPMS.Models;

namespace PPMS.ViewModels.Production
{
    public class VmProductionPlan
    {
        public VmProductionPlan() 
        {
            ProMasterModels = new List<ProMasterModel>();
            ProMasterModel = new ProMasterModel();
            HandsetSmtModels=new List<Pro_HandsetSmtModel>();
            HandsetSmtModel = new Pro_HandsetSmtModel();
            ProHandsetAssemblyModels=new List<Pro_HandsetAssemblyAndPackingModel>();
            ProHandsetAssemblyModel=new Pro_HandsetAssemblyAndPackingModel();
            ProHandsetBatteryModels=new List<Pro_HandsetBatteryModel>();
            ProHandsetBatteryModel=new Pro_HandsetBatteryModel();
            ProHandsetHousingModels=new List<Pro_HandsetHousingModel>();
            ProHandsetHousingModel=new Pro_HandsetHousingModel();
            CustomHandsetPlans=new List<CustomHandsetPlan>();

        }

        public List<CustomHandsetPlan> CustomHandsetPlans { get; set; }
        public List<ProMasterModel> ProMasterModels { get; set; }
        public ProMasterModel ProMasterModel { get; set; }
        public List<Pro_HandsetSmtModel> HandsetSmtModels { get; set; }
        public Pro_HandsetSmtModel HandsetSmtModel { get; set; }
        public List<Pro_HandsetAssemblyAndPackingModel> ProHandsetAssemblyModels { get; set; }
        public Pro_HandsetAssemblyAndPackingModel ProHandsetAssemblyModel { get; set; }
        public List<Pro_HandsetBatteryModel> ProHandsetBatteryModels { get; set; }
        public Pro_HandsetBatteryModel ProHandsetBatteryModel { get; set; }
        public List<Pro_HandsetHousingModel> ProHandsetHousingModels { get; set; }
        public Pro_HandsetHousingModel ProHandsetHousingModel { get; set; }
        public long Id { get; set; }
        public long? ProjectId { get; set; }
        public long? ProjectMasterID { get; set; }
        public string ProjectName { get; set; }
        public string RoleName { get; set; }
        public int? OrderNumber { get; set; }
        public string PoCategory { get; set; }
        public string ProsIDs { get; set; }

        //old//
        public DateTime ProductionDate { get; set; }
        public CustomPrdAssemblyAndPackingDetails AssemblyAndPackingDetailse { get; set; }
        //end old//
        public string Category { get; set; }
        public string PlanName { get; set; }
        public long? PlanId { get; set; }
        public string PlanIds { get; set; }

        //Inventory Report//
        public string SoftPlanningDateSmt { get; set; }
        public string SoftActualDateSmt { get; set; }
        public string IqcPlanningDateSmt { get; set; }
        public string IqcActualDateSmt { get; set; }
        public string TrialPlanningDateSmt { get; set; }
        public string TrialActualDateSmt { get; set; }
        public string MassPlanningDateSmt { get; set; }
        public string MassActualDateSmt { get; set; }
        public long? TotalOrderQuantity { get; set; }
        //
        public string SoftPlanningDateHousing { get; set; }
        public string SoftActualDateHousing { get; set; }
        public string IqcPlanningDateHousing { get; set; }
        public string IqcActualDateHousing { get; set; }
        public string TrialPlanningDateHousing { get; set; }
        public string TrialActualDateHousing { get; set; }
        public string ReliabilityPlanningDateHousing { get; set; }
        public string ReliabilityActualDateHousing { get; set; }
        public string MassPlanningDateHousing { get; set; }
        public string MassActualDateHousing { get; set; }

        //
        public string SoftPlanningDateBattery { get; set; }
        public string SoftActualDateBattery { get; set; }
        public string IqcPlanningDateBattery { get; set; }
        public string IqcActualDateBattery { get; set; }
        public string TrialPlanningDateBattery { get; set; }
        public string TrialActualDateBattery { get; set; }
        public string ReliabilityPlanningDateBattery { get; set; }
        public string ReliabilityActualDateBattery { get; set; }
        public string MassPlanningDateBattery { get; set; }
        public string MassActualDateBattery { get; set; }
        public string AgingPlanningDateBattery { get; set; }
        public string AgingActualDateBattery { get; set; }

        //
        public string SoftPlanningDateAssembly { get; set; }
        public string SoftActualDateAssembly { get; set; }
        public string IqcPlanningDateAssembly { get; set; }
        public string IqcActualDateAssembly { get; set; }
        public string TrialPlanningDateAssembly { get; set; }
        public string TrialActualDateAssembly { get; set; }
        public string SoftwarePlanningDateAssembly { get; set; }
        public string SoftwareActualDateAssembly { get; set; }
        public string RndPlanningDateAssembly { get; set; }
        public string RndActualDateAssembly { get; set; }
        public string MassPlanningDateAssembly { get; set; }
        public string MassActualDateAssembly { get; set; }
        public string PackingPlanningDateAssembly { get; set; }
        public string PackingActualDateAssembly { get; set; }

        //
        public string HandsetSmtDelayReason { get; set; }
        public string HandsetIqcDelayReason { get; set; }
        public string HandsetTrialDelayReason { get; set; }
        public string HandsetMpDelayReason { get; set; }
        public string HandsetPackingDelayReason { get; set; }

        public string HHousingMaterialDelayReason { get; set; }
        public string HHousingIqcDelayReason { get; set; }
        public string HHousingTrialDelayReason { get; set; }
        public string HHousingReliabilityDelayReason { get; set; }
        public string HHousingMpDelayReason { get; set; }
        public string HHousingPackingDelayReason { get; set; }

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

        //Attachment
        public string HandsetSmtAttachment { get; set; }
        public string HandsetIqcAttachment { get; set; }
        public string HandsetTrialAttachment { get; set; }
        public string HandsetMpAttachment { get; set; }
        //
        public string HHousingMaterialAttachment { get; set; }
        public string HHousingIqcAttachment { get; set; }
        public string HHousingTrialAttachment { get; set; }
        public string HHousingReliabilityAttachment { get; set; }
        public string HHousingMpAttachment { get; set; }
        //    
        public string HBatteryMaterialAttachment { get; set; }
        public string HBatteryIqcAttachment { get; set; }
        public string HBatteryTrialAttachment { get; set; }
        public string HBatteryReliabilityAttachment { get; set; }
        public string HBatteryMpAttachment { get; set; }
        public string HBatteryAgingAttachment { get; set; }
        //
        public string HAssemblyMaterialAttachment { get; set; }
        public string HAssemblyIqcAttachment { get; set; }
        public string HAssemblySoftComAttachment { get; set; }
        public string HAssemblyRndAttachment { get; set; }
        public string HAssemblyAttachment { get; set; }

    }
}