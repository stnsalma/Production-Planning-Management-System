using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PPMS.DAL.DbModel;
using PPMS.Infrastructures.Interfaces;
using PPMS.Infrastructures.Repositories;
using PPMS.Models;
using PPMS.ViewModels.Production;

namespace PPMS.Controllers
{
    public class ProductionController : Controller
    {
        private IProductionRepository _repository;

        public ProductionController(ProductionRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public JsonResult GetHolidayDataList()
        {
            List<GovernmentHolidayTableModel> getHoliday = _repository.GetHolidayDatasList();

            var json = JsonConvert.SerializeObject(getHoliday);

            return Json(new { data = json }, JsonRequestBehavior.AllowGet);
        }

        #region New Production Plan

        public ActionResult FactoryHoliday()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetHoliday()
        {
            var getHolidays = _repository.GetHoliday();
            var json = JsonConvert.SerializeObject(getHolidays);
            return Json(new { data = json }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveHolidayDropData(string Id, string GovernmentHoliday, string HolidayStartDate, string HolidayEndDate)
        {
            var saveData = _repository.SaveHolidayDropData(Id, GovernmentHoliday, HolidayStartDate, HolidayEndDate);

            return Json(new { data = saveData }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteHolidayData(string Id)
        {
            var deleteEvent = _repository.DeleteHolidayData(Id);

            return Json(new { data = deleteEvent }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Capacity Planning
        public ActionResult Shift(string monNum1, string year, string productionType, string phoneType)
        {
            int mons;
            int.TryParse(monNum1, out mons);

            var vmCapacity = new VmCapacityPlanning();
            //Month//
            List<SelectListItem> selectListItemsMonth = new List<SelectListItem>();
            selectListItemsMonth.Add(new SelectListItem() { Text = "SELECT MONTH", Value = "0" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "January", Value = "1" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "February", Value = "2" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "March", Value = "3" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "April", Value = "4" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "May", Value = "5" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "June", Value = "6" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "July", Value = "7" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "August", Value = "8" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "September", Value = "9" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "October", Value = "10" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "November", Value = "11" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "December", Value = "12" });

            ViewBag.ddlMonths = selectListItemsMonth;
            //Year//
            List<SelectListItem> selectListItemsYear = new List<SelectListItem>();
            selectListItemsYear.Add(new SelectListItem() { Text = "SELECT YEAR", Value = "0" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2019", Value = "2019" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2020", Value = "2020" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2021", Value = "2021" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2022", Value = "2022" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2023", Value = "2023" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2024", Value = "2024" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2025", Value = "2025" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2026", Value = "2026" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2027", Value = "2027" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2028", Value = "2028" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2029", Value = "2029" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2030", Value = "2030" });

            ViewBag.ddlYears = selectListItemsYear;
            vmCapacity.MonNum1 = monNum1;
            vmCapacity.Year1 = year;
            vmCapacity.ProductionType = productionType;

            vmCapacity.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vmCapacity.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            ViewBag.GetShiftSavedData = _repository.GetShiftSavedData(mons, year, productionType);

            return View(vmCapacity);
        }
        [HttpPost]
        public ActionResult Shift(List<Pro_Shift_Model> issueList, string monNum1, string year, string monName, string productionType)
        {

            issueList = issueList.Where(x => x.IsRemoved == 0).ToList();

            long userId = Convert.ToInt64(User.Identity.Name);

            int mon;
            int.TryParse(monNum1, out mon);

            int years;
            int.TryParse(year, out years);

            _repository.SaveShift(issueList, mon, monName, years, productionType);

            // MonNum1= monNum1 +"&year="+ year + "&productionType="+ productionType 

            return RedirectToAction("Shift", new { MonNum1 = monNum1, year = year, productionType = productionType });
        }
        public ActionResult DailyPlan(string monNum1, string year, string productionType, string phoneType)
        {
            int mons;
            int.TryParse(monNum1, out mons);

            var vmCapacity = new VmCapacityPlanning();
            //Month//
            List<SelectListItem> selectListItemsMonth = new List<SelectListItem>();
            selectListItemsMonth.Add(new SelectListItem() { Text = "SELECT MONTH", Value = "0" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "January", Value = "1" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "February", Value = "2" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "March", Value = "3" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "April", Value = "4" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "May", Value = "5" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "June", Value = "6" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "July", Value = "7" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "August", Value = "8" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "September", Value = "9" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "October", Value = "10" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "November", Value = "11" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "December", Value = "12" });

            ViewBag.ddlMonths = selectListItemsMonth;
            //Year//
            List<SelectListItem> selectListItemsYear = new List<SelectListItem>();
            selectListItemsYear.Add(new SelectListItem() { Text = "SELECT YEAR", Value = "0" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2019", Value = "2019" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2020", Value = "2020" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2021", Value = "2021" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2022", Value = "2022" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2023", Value = "2023" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2024", Value = "2024" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2025", Value = "2025" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2026", Value = "2026" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2027", Value = "2027" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2028", Value = "2028" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2029", Value = "2029" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2030", Value = "2030" });

            ViewBag.ddlYears = selectListItemsYear;
            vmCapacity.MonNum1 = monNum1;
            vmCapacity.Year1 = year;
            vmCapacity.ProductionType = productionType;

            vmCapacity.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vmCapacity.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            if (mons != 0 && year != null)
            {
                ViewBag.GetDailyShiftData = _repository.GetDailyShiftData(mons, year, productionType);
                ViewBag.GetLine = _repository.GetLine(mons, year, productionType);
                ViewBag.GetShiftSavedData = _repository.GetShiftSavedData(mons, year, productionType);

                ViewBag.DailySaved = _repository.DailySaved(mons, year, productionType);
                var dd = _repository.DailySaved(mons, year, productionType);
                if (ViewBag.DailySaved.Count == 0)
                {

                    ViewBag.GetDailyShiftData1 = _repository.GetDailyShiftData1(mons, year, productionType);
                    // ViewBag.ChangedDailyPlanData = _repository.ChangedDailyPlanData(mons, year, productionType);
                }

            }

            return View(vmCapacity);
        }

        public ActionResult CapacityPlanning(string monNum1, string year, string productionType, string phoneType, string categories)
        {
            int mons;
            int.TryParse(monNum1, out mons);

            var vmCapacity = new VmCapacityPlanning();
            //Month//
            List<SelectListItem> selectListItemsMonth = new List<SelectListItem>();
            selectListItemsMonth.Add(new SelectListItem() { Text = "SELECT MONTH", Value = "0" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "January", Value = "1" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "February", Value = "2" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "March", Value = "3" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "April", Value = "4" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "May", Value = "5" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "June", Value = "6" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "July", Value = "7" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "August", Value = "8" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "September", Value = "9" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "October", Value = "10" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "November", Value = "11" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "December", Value = "12" });

            ViewBag.ddlMonths = selectListItemsMonth;
            //Year//
            List<SelectListItem> selectListItemsYear = new List<SelectListItem>();
            selectListItemsYear.Add(new SelectListItem() { Text = "SELECT YEAR", Value = "0" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2019", Value = "2019" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2020", Value = "2020" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2021", Value = "2021" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2022", Value = "2022" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2023", Value = "2023" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2024", Value = "2024" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2025", Value = "2025" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2026", Value = "2026" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2027", Value = "2027" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2028", Value = "2028" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2029", Value = "2029" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2030", Value = "2030" });

            ViewBag.ddlYears = selectListItemsYear;
            //
            //Year//
            List<SelectListItem> selectListItemsPhoneType = new List<SelectListItem>();
            selectListItemsPhoneType.Add(new SelectListItem() { Text = "SELECT", Value = "0" });
            selectListItemsPhoneType.Add(new SelectListItem() { Text = "Smart", Value = "Smart" });
            selectListItemsPhoneType.Add(new SelectListItem() { Text = "Feature", Value = "Feature" });
            selectListItemsPhoneType.Add(new SelectListItem() { Text = "Charger", Value = "Charger" });
            ViewBag.ddlPhoneType = selectListItemsPhoneType;

            //
            vmCapacity.MonNum1 = monNum1;
            vmCapacity.Year1 = year;
            vmCapacity.ProductionType = productionType;
            vmCapacity.ProductName = phoneType;
            vmCapacity.CategoryName = categories;

            vmCapacity.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vmCapacity.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            vmCapacity.ProShiftModels = _repository.GetProductName(productionType);
            List<SelectListItem> itemsForProduct = vmCapacity.ProShiftModels.Select(model => new SelectListItem { Text = model.ProductName, Value = model.ProductName.ToString(CultureInfo.InvariantCulture) }).ToList();
            ViewBag.Products = itemsForProduct;

            vmCapacity.ProShiftModels1 = _repository.GetCategoryName(productionType, phoneType);
            List<SelectListItem> itemsForProduct1 = vmCapacity.ProShiftModels1.Select(model => new SelectListItem { Text = model.CategoryName, Value = model.CategoryName.ToString(CultureInfo.InvariantCulture) }).ToList();
            ViewBag.Categories = itemsForProduct1;

            if (mons != 0 && year != null)
            {
                ViewBag.GetShift = _repository.GetShift(mons, year, productionType, phoneType);
                // ViewBag.GetTeam = _repository.GetTeam(mons, year, productionType, phoneType);
                ViewBag.GetPercentage = _repository.GetPercentage(mons, year, productionType, phoneType, categories);
                ViewBag.GetQuantityRange = _repository.GetQuantityRange(mons, year, productionType, phoneType, categories);
                // ViewBag.GetCapacity = _repository.GetCapacity(mons, year, productionType, categories);
                ViewBag.GetAll1 = _repository.GetAll1(mons, year, productionType, phoneType, categories);

            }
            return View(vmCapacity);
        }
        public JsonResult GetProductName(string productionType)
        {
            var vmCapacity = new VmCapacityPlanning();

            if (productionType != null)
            {
                vmCapacity.ProShiftModels = _repository.GetProductName(productionType);

            }

            List<SelectListItem> items1 = vmCapacity.ProShiftModels.Select(model => new SelectListItem { Text = model.ProductName, Value = model.ProductName.ToString(CultureInfo.InvariantCulture) }).ToList();
            var json = JsonConvert.SerializeObject(items1);

            return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetCategoryName(string productionType, string proPhoneName)
        {
            var vmCapacity = new VmCapacityPlanning();

            if (productionType != null && proPhoneName != null)
            {
                vmCapacity.ProShiftModels1 = _repository.GetCategoryName(productionType, proPhoneName);

            }

            List<SelectListItem> items1 = vmCapacity.ProShiftModels1.Select(model => new SelectListItem { Text = model.CategoryName, Value = model.CategoryName.ToString(CultureInfo.InvariantCulture) }).ToList();
            var json = JsonConvert.SerializeObject(items1);

            return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult CapacityPlanning(string arrMain)
        {
            var results = JsonConvert.DeserializeObject<List<Pro_CapacityPlanning_Model>>(arrMain);

            var saveResult = "0";
            if (results.Count != 0)
            {
                saveResult = _repository.SaveCapacityData(results);
            }

            return Json(new { data = saveResult }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCapacityPlanning(string monNum1, string year, string productionType, string phoneType, string categories)
        {
            int mons;
            int.TryParse(monNum1, out mons);

            var vmCapacity = new VmCapacityPlanning();
            if (mons != 0 && year != null)
            {
                ViewBag.GetShift = _repository.GetShift(mons, year, productionType, phoneType);
                ViewBag.GetTeam = _repository.GetTeam(mons, year, productionType, phoneType, categories);
                ViewBag.GetPercentage = _repository.GetPercentage(mons, year, productionType, phoneType, categories);
                ViewBag.GetQuantityRange = _repository.GetQuantityRange(mons, year, productionType, phoneType, categories);
                ViewBag.GetCapacity = _repository.GetCapacity(mons, year, productionType, phoneType);
                ViewBag.GetAll = _repository.GetAll(mons, year, productionType, phoneType, categories);

            }
            return View(vmCapacity);
        }
        [HttpPost]
        public ActionResult UpdateCapacityPlanning(string arrMain)
        {
            var results = JsonConvert.DeserializeObject<List<Pro_CapacityPlanning_Model>>(arrMain);

            var saveResult = "0";
            if (results.Count != 0)
            {
                saveResult = _repository.SaveCapacityData(results);
            }

            return Json(new { data = saveResult }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateTeam(string productionType)
        {

            var vmCapacity = new VmCapacityPlanning();
            vmCapacity.ProductionType = productionType;

            vmCapacity.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vmCapacity.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            if (productionType != null)
            {

                ViewBag.GetTeamForUpdate = _repository.GetTeamForUpdate(productionType);

            }
            return View(vmCapacity);
        }
        [HttpPost]
        public ActionResult CreateTeam(List<Pro_Shift_Model> issueList1, string productionType)
        {

            issueList1 = issueList1.Where(x => x.IsRemoved == 0).ToList();

            long userId = Convert.ToInt64(User.Identity.Name);
            if (ModelState.IsValid)
            {
                _repository.SaveTeam(issueList1, productionType);
            }

            return RedirectToAction("CreateTeam", new { productionType = productionType });
        }

        public JsonResult InActiveTeam(string inactiveObj)
        {
            long ids;
            long.TryParse(inactiveObj, out ids);

            var SaveInactive = "0";

            if (ids != 0)
            {
                SaveInactive = _repository.UpdateTeam(ids);
            }

            return Json(new { data = SaveInactive }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditTeam(String Id, String Team)
        {

            var editTeam = _repository.EditTeam(Id, Team);

            return new JsonResult { Data = editTeam, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult CreateLine(string productionType)
        {
            var vmCapacity = new VmCapacityPlanning();
            vmCapacity.ProductionType = productionType;

            vmCapacity.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vmCapacity.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            if (productionType != null)
            {

                ViewBag.GetLineForUpdate = _repository.GetLineForUpdate(productionType);

            }
            return View(vmCapacity);
        }
        [HttpPost]
        public ActionResult CreateLine(List<Pro_Shift_Model> issueList1, string productionType)
        {

            issueList1 = issueList1.Where(x => x.IsRemoved == 0).ToList();

            long userId = Convert.ToInt64(User.Identity.Name);
            if (productionType != null)
            {
                _repository.SaveLine(issueList1, productionType);
            }

            return RedirectToAction("CreateLine", new { productionType = productionType });
        }
        [HttpPost]
        public JsonResult EditLine(String Id, String Line, String LineType, String ProductionDaysPerMonth, String ShiftPerDay, String HoursPerShift)
        {

            var editTeam = _repository.EditLine(Id, Line, LineType, ProductionDaysPerMonth, ShiftPerDay, HoursPerShift);

            return new JsonResult { Data = editTeam, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult InActiveLine(string inactiveObj)
        {
            long ids;
            long.TryParse(inactiveObj, out ids);

            var SaveInactive = "0";

            if (ids != 0)
            {
                SaveInactive = _repository.InActiveLine(ids);
            }

            return Json(new { data = SaveInactive }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InActiveShift(string inactiveObj)
        {
            long ids;
            long.TryParse(inactiveObj, out ids);

            var SaveInactive = "0";

            if (ids != 0)
            {
                SaveInactive = _repository.InActiveShift(ids);
            }

            return Json(new { data = SaveInactive }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllTeam(string productionType11)
        {
            var selectListItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT" } };

            List<String> moduleList = _repository.GetAllTeam(productionType11);
            foreach (var module in moduleList)
            {
                selectListItems.Add(new SelectListItem
                {
                    Value = module,
                    Text = module
                });
            }
            return Json(new { list = selectListItems }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllLine(string productionType11)
        {
            var selectListItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT" } };

            List<String> moduleList = _repository.GetAllLine(productionType11);
            foreach (var module in moduleList)
            {
                selectListItems.Add(new SelectListItem
                {
                    Value = module,
                    Text = module
                });
            }
            return Json(new { list = selectListItems }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllCategory(string productionType11, string PhoneType)
        {
            var selectListItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT" } };

            List<String> moduleList = _repository.GetAllCategory(productionType11, PhoneType);
            foreach (var module in moduleList)
            {
                selectListItems.Add(new SelectListItem
                {
                    Value = module,
                    Text = module
                });
            }
            return Json(new { list = selectListItems }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateProduct(string productionType)
        {
            var vmCapacity = new VmCapacityPlanning();
            vmCapacity.ProductionType = productionType;

            vmCapacity.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vmCapacity.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            if (productionType != null)
            {

                ViewBag.GetProductForUpdate = _repository.GetProductForUpdate(productionType);

            }
            return View(vmCapacity);
        }
        [HttpPost]
        public ActionResult CreateProduct(List<Pro_Shift_Model> issueList1, string productionType)
        {
            issueList1 = issueList1.Where(x => x.IsRemoved == 0).ToList();

            long userId = Convert.ToInt64(User.Identity.Name);

            if (ModelState.IsValid)
            {
                _repository.SaveProduct(issueList1, productionType);
            }
            return RedirectToAction("CreateProduct", new { productionType = productionType });
        }
        public JsonResult InActiveProduct(string inactiveObj)
        {
            long ids;
            long.TryParse(inactiveObj, out ids);

            var SaveInactive = "0";

            if (ids != 0)
            {
                SaveInactive = _repository.InActiveProduct(ids);
            }

            return Json(new { data = SaveInactive }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateDailyPlan(String Id, DateTime EffectiveDate, String Line, String Shift_1, String Shift_2, String Shift_3, String ProductionType,
            String MonNum, String Month, String Year)
        {
            long ids;
            long.TryParse(Id, out ids);

            var editPlan = _repository.UpdateDailyPlan(ids, EffectiveDate, Line, Shift_1, Shift_2, Shift_3, ProductionType,
                MonNum, Month, Year);

            return new JsonResult { Data = editPlan, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult SaveDailyPlan(string arrMain)
        {
            var results = JsonConvert.DeserializeObject<List<Pro_Shift_Model>>(arrMain);

            var saveResult = "0";
            if (results.Count != 0)
            {
                saveResult = _repository.SaveDailyPlan(results);
            }

            return Json(new { data = saveResult }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Capacity Report
        public ActionResult Report_CapacityPlanning(string monNum1, string year, string productionType)
        {
            int mons;
            int.TryParse(monNum1, out mons);

            var vmCapacity = new VmCapacityPlanning();
            //Month//
            List<SelectListItem> selectListItemsMonth = new List<SelectListItem>();
            selectListItemsMonth.Add(new SelectListItem() { Text = "SELECT MONTH", Value = "0" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "January", Value = "1" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "February", Value = "2" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "March", Value = "3" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "April", Value = "4" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "May", Value = "5" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "June", Value = "6" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "July", Value = "7" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "August", Value = "8" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "September", Value = "9" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "October", Value = "10" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "November", Value = "11" });
            selectListItemsMonth.Add(new SelectListItem() { Text = "December", Value = "12" });

            ViewBag.ddlMonths = selectListItemsMonth;
            //Year//
            List<SelectListItem> selectListItemsYear = new List<SelectListItem>();
            selectListItemsYear.Add(new SelectListItem() { Text = "SELECT YEAR", Value = "0" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2019", Value = "2019" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2020", Value = "2020" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2021", Value = "2021" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2022", Value = "2022" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2023", Value = "2023" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2024", Value = "2024" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2025", Value = "2025" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2026", Value = "2026" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2027", Value = "2027" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2028", Value = "2028" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2029", Value = "2029" });
            selectListItemsYear.Add(new SelectListItem() { Text = "2030", Value = "2030" });

            ViewBag.ddlYears = selectListItemsYear;
            //

            vmCapacity.MonNum1 = monNum1;
            vmCapacity.Year1 = year;
            vmCapacity.ProductionType = productionType;


            vmCapacity.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vmCapacity.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            vmCapacity.ProShiftModels = _repository.GetProductName(productionType);
            List<SelectListItem> itemsForProduct = vmCapacity.ProShiftModels.Select(model => new SelectListItem { Text = model.ProductName, Value = model.ProductName.ToString(CultureInfo.InvariantCulture) }).ToList();
            ViewBag.Products = itemsForProduct;

            if (mons != 0 && year != null)
            {
                ViewBag.ProductNameForReport = _repository.ProductNameForReport(mons, year, productionType);
                ViewBag.TeamNameForReport = _repository.TeamNameForReport(mons, year, productionType);
                ViewBag.CategoryNameForReport = _repository.CategoryNameForReport(mons, year, productionType);
                ViewBag.GetPercentage1 = _repository.GetPercentage1(mons, year, productionType);
                ViewBag.GetQuantityRange1 = _repository.GetQuantityRange1(mons, year, productionType);
                ViewBag.GetTotalCapacities1 = _repository.GetTotalCapacities1(mons, year, productionType);
            }
            return View(vmCapacity);
        }
        #endregion

        #region Project Categorize

        public ActionResult ProjectCategorize()
        {
            var vmCapacity = new VmCapacityPlanning();

            ViewBag.GetProjectForCategorization = _repository.GetProjectForCategorization();
            ViewBag.GetCompletedCategorization = _repository.GetCompletedCategorization();

            //List<SelectListItem> items =new List<SelectListItem>();
            //foreach (var mdd in ViewBag.GetProjectForCategorization)
            //{
            //    vmCapacity.ProShiftModels1 = _repository.GetAssemblyCategory(mdd.ProjectType);
            //    items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT ASSEMBLY" } };
            //    items.AddRange(vmCapacity.ProShiftModels1.Select(model => new SelectListItem { Text = model.CategoryName, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            //    ViewBag.GetAssemblyCategory = items;
            //}



            return View(vmCapacity);
        }
        public JsonResult GetAssemblyCategory(string AssemblyCategory)
        {
            var vmCapacity = new VmCapacityPlanning();
            var selectListItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "Select Assembly" } };

            vmCapacity.ProShiftModels1 = _repository.GetAssemblyCategory(AssemblyCategory);
            foreach (var module in vmCapacity.ProShiftModels1)
            {
                selectListItems.Add(new SelectListItem
                {
                    Value = module.ProductionType,
                    Text = module.AssemblyCategory
                });
            }
            return Json(new { list = selectListItems }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSmtCategory(string SmtCategory)
        {
            var vmCapacity = new VmCapacityPlanning();
            var selectListItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "Select Smt" } };

            vmCapacity.ProShiftModels1 = _repository.GetSmtCategory(SmtCategory);
            foreach (var module in vmCapacity.ProShiftModels1)
            {
                selectListItems.Add(new SelectListItem
                {
                    Value = module.ProductionType,
                    Text = module.SmtCategory
                });
            }
            return Json(new { list = selectListItems }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHousingCategory(string HousingCategory)
        {
            var vmCapacity = new VmCapacityPlanning();
            var selectListItems = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "Select Housing" } };

            vmCapacity.ProShiftModels1 = _repository.GetHousingCategory(HousingCategory);
            foreach (var module in vmCapacity.ProShiftModels1)
            {
                selectListItems.Add(new SelectListItem
                {
                    Value = module.ProductionType,
                    Text = module.HousingCategory
                });
            }
            return Json(new { list = selectListItems }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveCategorizeData(string ProjectName, string ProductFamily, string AssemblyCategory, string SmtCategory, string HousingCategory)
        {

            var saveData = "0";

            if (AssemblyCategory != "" || SmtCategory != "" || HousingCategory != "")
            {
                saveData = _repository.SaveCategorizeData(ProjectName, ProductFamily, AssemblyCategory, SmtCategory, HousingCategory);
            }

            return Json(new { data = saveData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CompleteCategorizeData(string ProjectName, string ProductFamily)
        {

            var saveData = "0";

            if (ProjectName != "" && ProductFamily != "")
            {
                saveData = _repository.CompleteCategorizeData(ProjectName, ProductFamily);
            }

            return Json(new { data = saveData }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateCategorizeData(string CatIds, string AssemblyCategory1, string SmtCategory1, string HousingCategory1)
        {
            long ids;
            long.TryParse(CatIds, out ids);

            var saveData = "0";

            if (AssemblyCategory1 != "" || SmtCategory1 != "" || HousingCategory1 != "")
            {
                saveData = _repository.UpdateCategorizeData(ids, AssemblyCategory1, SmtCategory1, HousingCategory1);
            }

            return Json(new { data = saveData }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Shift and Capacity Forward

        public ActionResult ForwardShiftAndCapacity()
        {
            var vm = new VmCapacityPlanning();

            vm.ProTypeModels = _repository.GetProductionType();
            List<SelectListItem> items = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "SELECT UNIT" } };
            items.AddRange(vm.ProTypeModels.Select(model => new SelectListItem { Text = model.ProductionType, Value = model.ProductionType.ToString(CultureInfo.InvariantCulture) }).ToList());
            ViewBag.GetProductionType = items;

            return View(vm);
        }

        [HttpPost]
        public JsonResult ForwardShift(string unitValues, string currentDate, string forwardedDate, string shiftForward)
        {
            var saveIncentive = "0";

            if (unitValues != "" && currentDate != "" && forwardedDate != "" & shiftForward != "")
            {
                saveIncentive = _repository.ForwardShift(unitValues, currentDate, forwardedDate, shiftForward);
            }

            return Json(new { data = saveIncentive }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ForwardCapacity(string unitValues, string currentDate, string forwardedDate, string capForward)
        {
            var saveIncentive = "0";

            if (unitValues != "" && currentDate != "" && forwardedDate != "" & capForward != "")
            {
                saveIncentive = _repository.ForwardCapacity(unitValues, currentDate, forwardedDate, capForward);
            }

            return Json(new { data = saveIncentive }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Handset Plan
        public ActionResult HandsetPlanning(string ProjectMasterID, string ProjectName)
        {
           
            var _dbEntities = new CellPhoneProjectEntities();
            long userId = Convert.ToInt64(System.Web.HttpContext.Current.User.Identity.Name);
            var user = _dbEntities.CmnUsers.FirstOrDefault(i => i.CmnUserId == userId && i.IsActive == true);

            ViewBag.RoleName = user.RoleName;

            var vmPro = new VmProductionPlan();
            vmPro.ProMasterModels = _repository.GetProjectList();
            //List<SelectListItem> items = vmPro.ProMasterModels.Select(model => new SelectListItem { Text = model.ProjectName, Value = model.ProjectMasterID.ToString(CultureInfo.InvariantCulture) }).ToList();
            List<SelectListItem> items = vmPro.ProMasterModels.Select(model => new SelectListItem { Text = model.ProjectName, Value = model.ProjectMasterID.ToString(CultureInfo.InvariantCulture) }).ToList();
            ViewBag.Projects = items;

            vmPro.ProjectName = ProjectName;
            vmPro.ProjectMasterID = Convert.ToInt64(ProjectMasterID);

            ViewBag.ProjectName = ProjectName;
            ViewBag.ProjectMasterID = Convert.ToInt64(ProjectMasterID);

            long proIds;
            long.TryParse(ProjectMasterID, out proIds);

            var chargerPro = "";
            if (proIds!=0)
            {
                vmPro.CustomHandsetPlans = _repository.GetProjectPlanningHistoryData(proIds, ProjectName);
            }
           

            return View(vmPro);
        }
        [HttpGet]
        public JsonResult GetMaterialRulesList()
        {
            List<Pro_MaterialRulesModel> getMat = _repository.GetMaterialRulesList();

            var json = JsonConvert.SerializeObject(getMat);

            return Json(new { data = json }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveHandsetPlanningData(string objArr)
        {
            var results = JsonConvert.DeserializeObject<List<CustomHandsetPlan>>(objArr);
            Console.Write("result :" + results);

            var saveHandset = "0";

            if (results.Count != 0)
            {
                saveHandset = _repository.SaveHandsetPlanningData(results);
            }

            return Json(new { SaveCharger = saveHandset }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateHandsetPlanningData(string objArr)
        {
            var results = JsonConvert.DeserializeObject<List<CustomHandsetPlan>>(objArr);
            Console.Write("result :" + results);

            var saveHandset = "0";

            if (results.Count != 0)
            {
                saveHandset = _repository.UpdateHandsetPlanningData(results);
            }

            return Json(new { SaveCharger = saveHandset }, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public JsonResult GetProjectPlanningHistoryData(string ProjectMasterID, string ProjectName)
        //{
        //    long proIds;
        //    long.TryParse(ProjectMasterID, out proIds);
        //    var chargerPro = _repository.GetProjectPlanningHistoryData(proIds, ProjectName);

        //    return Json(new { data = chargerPro }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetHandsetOldHistory(string projectId, string projectName, string planIds)
        {
            long proIds;
            long.TryParse(projectId, out proIds);

            long planId;
            long.TryParse(planIds, out planId);

            var batteryHistory = _repository.GetHandsetOldHistory(proIds, planId);


            return Json(new { data = batteryHistory }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InActiveAnyPlan(string projectId, string projectName, string planIds)
        {
            long proIds;
            long.TryParse(projectId, out proIds);

            long planId;
            long.TryParse(planIds, out planId);
            var getPacking = "";
            if (projectId != null && planIds != null)
            {
                getPacking = _repository.InActiveAnyPlan(proIds, planId);

            }
            return Json(new { data = getPacking }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Inventory Report
        public ActionResult InventoryReport(string planId, string category)
        {
            //var _dbEntities = new CellPhoneProjectEntities();
            //long userId = Convert.ToInt64(System.Web.HttpContext.Current.User.Identity.Name);
            //var user = _dbEntities.CmnUsers.FirstOrDefault(i => i.CmnUserId == userId && i.IsActive == true);

            //ViewBag.RoleName = user.RoleName;



            var vmPro = new VmProductionPlan();
            vmPro.ProMasterModels = _repository.GetInventoryProjectList();
            List<SelectListItem> items = vmPro.ProMasterModels.Select(model => new SelectListItem { Text = model.ProjectName, Value = model.PlanIds.ToString(CultureInfo.InvariantCulture) }).ToList();
            ViewBag.Projects = items;

            //vmPro.ProjectName = ProjectName;
            //vmPro.ProsIDs = ProsIDs;
       
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem() { Text = "ALL", Value = "ALL" });
            selectListItems.Add(new SelectListItem() { Text = "SMT", Value = "SMT" });
            selectListItems.Add(new SelectListItem() { Text = "HOUSING", Value = "HOUSING" });
            selectListItems.Add(new SelectListItem() { Text = "BATTERY", Value = "BATTERY" });
            selectListItems.Add(new SelectListItem() { Text = "ASSEMBLY", Value = "ASSEMBLY" });
    
            ViewBag.AllCategory = selectListItems;

            //vmPro.PlanId = PlanId;
            //vmPro.Category = category;
            //vmPro.PlanName = planName;

            return View(vmPro);
        }

        [HttpGet]
        public JsonResult GetHandsetSmtHistoryData(string planId, string category, string currentMonthYear)
        {
            var planData = new List<VmProductionPlan>();
           // if (category=="SMT")
           // {
            planData = _repository.GetHandsetSmtHistoryData(planId, category, currentMonthYear);
 
           // }
            return Json(new { data = planData }, JsonRequestBehavior.AllowGet);
        }
      
        [HttpGet]
        public JsonResult GetHandsetHousingHistoryData(string planId, string category, string currentMonthYear)
        {
            var planData = new List<VmProductionPlan>();
           // if (category == "HOUSING")
           // {
            planData = _repository.GetHandsetHousingHistoryData(planId, category, currentMonthYear);

           // }
            return Json(new { data = planData }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpGet]
        public JsonResult GetHandsetBatteryHistoryData(string planId, string category, string currentMonthYear)
        {
            var planData = new List<VmProductionPlan>();
           // if (category == "BATTERY")
           // {
            planData = _repository.GetHandsetBatteryHistoryData(planId, category, currentMonthYear);

           // }
            return Json(new { data = planData }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpGet]
        public JsonResult GetHandsetAssemblyHistoryData(string planId, string category, string currentMonthYear)
        {
            var planData = new List<VmProductionPlan>();
            //if (category == "ASSEMBLY")
            //{
            planData = _repository.GetHandsetAssemblyHistoryData(planId, category, currentMonthYear);

            //}
            return Json(new { data = planData }, JsonRequestBehavior.AllowGet);
        }
        #endregion
//test
    }
}
