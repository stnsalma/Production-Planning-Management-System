using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPMS.Models;
using PPMS.ViewModels.Production;

namespace PPMS.Infrastructures.Interfaces
{
    public interface IProductionRepository
    {
        List<GovernmentHolidayTableModel> GetHolidayDatasList();
        #region Mobile Production Plan New
        List<GovernmentHolidayTableModel> GetHoliday();
        string SaveHolidayDropData(string Id, string governmentHoliday, string holidayStartDate, string holidayEndDate);
        string DeleteHolidayData(string id);
        #endregion

        #region Capacity Planning
        List<Pro_Type_Model> GetProductionType();
        string SaveShift(List<Pro_Shift_Model> issueList, int mon, string monName, int years, string productionType);
        List<Pro_Shift_Model> GetShiftSavedData(int mons, string year, string productionType);
        List<Pro_Shift_Model> GetDailyShiftData(int mons, string year, string productionType);
        List<Pro_Shift_Model> GetLine(int mons, string year, string productionType);
        List<Pro_Shift_Model> GetShift(int mons, string year, string productionType, string phoneType);
        string SaveCapacityData(List<Pro_CapacityPlanning_Model> results);
        List<Pro_CapacityPlanning_Model> GetCapacity(int mons, string year, string productionType, string categories);
        List<Pro_CapacityPlanning_Model> GetTeam(int mons, string year, string productionType, string phoneType, string categories);
        List<Pro_CapacityPlanning_Model> GetPercentage(int mons, string year, string productionType, string phoneType, string categories);
        List<Pro_CapacityPlanning_Model> GetQuantityRange(int mons, string year, string productionType, string phoneType, string categories);
        List<Pro_CapacityPlanning_Model> GetAll(int mons, string year, string productionType, string phoneType, string categories);
        string SaveTeam(List<Pro_Shift_Model> issueList1, string productionType);
        List<Pro_Shift_Model> GetTeamForUpdate(string productionType);
        string UpdateTeam(long ids);
        List<string> GetAllTeam(string productionType);
        List<string> GetAllCategory(string productionType11, string phoneType);
        string SaveProduct(List<Pro_Shift_Model> issueList1, string productionType);
        string EditTeam(string id, string team);
        string SaveLine(List<Pro_Shift_Model> issueList1, string productionType);
        List<Pro_Shift_Model> GetLineForUpdate(string productionType);
        string EditLine(string id, string line, string lineType, string productionDaysPerMonth, string shiftPerDay, string hoursPerShift);
        string InActiveLine(long ids);
        List<Pro_Shift_Model> GetProductForUpdate(string productionType);
        string InActiveProduct(long ids);
        List<string> GetAllLine(string productionType11);
        string InActiveShift(long ids);
        List<Pro_Shift_Model> GetProductName(string productionType);
        List<Pro_Shift_Model> GetCategoryName(string productionType, string proPhoneName);
        List<Pro_CapacityPlanning_Model> GetAll1(int mons, string year, string productionType, string phoneType, string categories);
        string UpdateDailyPlan(long ids, DateTime effectiveDate, string line, string shift1, string shift2, string shift3, string productionType, string monNum, string month, string year);
        string SaveDailyPlan(List<Pro_Shift_Model> results);
        #endregion

        #region Capacity Report

        List<Pro_Shift_Model> ProductNameForReport(int mons, string year, string productionType);
        List<Pro_Shift_Model> TeamNameForReport(int mons, string year, string productionType);
        List<Pro_Shift_Model> CategoryNameForReport(int mons, string year, string productionType);
        List<Pro_CapacityPlanning_Model> GetPercentage1(int mons, string year, string productionType);
        List<Pro_CapacityPlanning_Model> GetQuantityRange1(int mons, string year, string productionType);
        List<Pro_CapacityPlanning_Model> GetTotalCapacities1(int mons, string year, string productionType);
        #endregion

        #region Project Categorize
        List<Pro_Shift_Model> GetProjectForCategorization();
        List<Pro_Shift_Model> GetAssemblyCategory(string projectType);
        List<Pro_Shift_Model> GetSmtCategory(string smtCategory);
        List<Pro_Shift_Model> GetHousingCategory(string housingCategory);
        string SaveCategorizeData(string projectName, string productFamily, string assemblyCategory, string smtCategory, string housingCategory);
        string CompleteCategorizeData(string projectName, string productFamily);
        List<Pro_Shift_Model> GetCompletedCategorization();
        string UpdateCategorizeData(long ids, string assemblyCategory1, string smtCategory1, string housingCategory1);
        List<Pro_Shift_Model> ChangedDailyPlanData(int mons, string year, string productionType);
        List<Pro_Shift_Model> GetDailyShiftData1(int mons, string year, string productionType);
        List<Pro_Shift_Model> DailySaved(int mons, string year, string productionType);
        string ForwardShift(string unitValues, string currentDate, string forwardedDate, string shiftForward);
        string ForwardCapacity(string unitValues, string currentDate, string forwardedDate, string capForward);
        #endregion

        #region Handset
        List<ProMasterModel> GetProjectList();
        List<Pro_MaterialRulesModel> GetMaterialRulesList();
        string SaveHandsetPlanningData(List<CustomHandsetPlan> results);
        string UpdateHandsetPlanningData(List<CustomHandsetPlan> results);
        List<CustomHandsetPlan> GetProjectPlanningHistoryData(long proIds, string projectName);
        List<CustomHandsetPlan> GetHandsetOldHistory(long proIds, long planId);
        string InActiveAnyPlan(long proIds, long planId);
        #endregion

        #region Inventory Plan
        List<ProMasterModel> GetInventoryProjectList();
        List<VmProductionPlan> GetHandsetSmtHistoryData(string planId, string category, string currentMonthYear);
        List<VmProductionPlan> GetHandsetHousingHistoryData(string planId, string category, string currentMonthYear);
        List<VmProductionPlan> GetHandsetBatteryHistoryData(string planId, string category, string currentMonthYear);
        List<VmProductionPlan> GetHandsetAssemblyHistoryData(string planId, string category, string currentMonthYear);
        #endregion
    }
}