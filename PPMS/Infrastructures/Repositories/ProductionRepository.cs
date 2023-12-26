using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web;
using PPMS.DAL.DbModel;
using PPMS.Infrastructures.Interfaces;
using PPMS.Infrastructures.Helper;
using PPMS.Models;
using PPMS.ViewModels.Production;

namespace PPMS.Infrastructures.Repositories
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly CellPhoneProjectEntities _cellPhoneProjectEntities;
        private readonly PPMSEntities _dbEntities;

        public ProductionRepository()
        {
            _cellPhoneProjectEntities = new CellPhoneProjectEntities();
            _cellPhoneProjectEntities.Configuration.LazyLoadingEnabled = false;
            _dbEntities = new PPMSEntities();
            _dbEntities.Configuration.LazyLoadingEnabled = false;
        }
        public List<GovernmentHolidayTableModel> GetHolidayDatasList()
        {
            _dbEntities.Database.CommandTimeout = 6000;
            string query = string.Format(@"select * from [PPMS].[dbo].[GovernmentHolidayTable]");
            var exe = _dbEntities.Database.SqlQuery<GovernmentHolidayTableModel>(query).ToList();
            return exe;
        }

        #region Mobile Production Plan New
        public List<GovernmentHolidayTableModel> GetHoliday()
        {
            var query = _dbEntities.Database.SqlQuery<GovernmentHolidayTableModel>(@"
             select * from [PPMS].[dbo].[GovernmentHolidayTable]").ToList();
            return query;
        }
        public string SaveHolidayDropData(string id, string governmentHoliday, string holidayStartDate, string holidayEndDate)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            DateTime sDate = Convert.ToDateTime(holidayStartDate);
            DateTime eDate = Convert.ToDateTime(holidayEndDate);

            if (id == null)
            {

                for (var ii = sDate; ii <= eDate; ii = ii.AddDays(1))
                {
                    var models = new GovernmentHolidayTable();
                    models.GovernmentHoliday = governmentHoliday;
                    models.HolidayDate = Convert.ToDateTime(ii);
                    models.HolidayStartDate = Convert.ToDateTime(ii);
                    models.HolidayEndDate = Convert.ToDateTime(ii);
                    models.Added = userId;
                    models.AddedDate = DateTime.Now;
                    _dbEntities.GovernmentHolidayTables.Add(models);
                    _dbEntities.SaveChanges();
                }

            }
            else
            {
                long ids;
                long.TryParse(id, out ids);


                for (var ii = sDate; ii <= eDate; ii = ii.AddDays(1))
                {
                    var model = _dbEntities.GovernmentHolidayTables.FirstOrDefault(x => x.Id == ids);
                    model.GovernmentHoliday = governmentHoliday;
                    model.HolidayDate = Convert.ToDateTime(ii);
                    model.HolidayStartDate = Convert.ToDateTime(ii);
                    model.HolidayEndDate = Convert.ToDateTime(ii);
                    model.Updated = userId;
                    model.UpdatedDate = DateTime.Now;

                    _dbEntities.SaveChanges();
                }
                //model.HolidayDate = Convert.ToDateTime(holidayStartDate);
                //model.HolidayStartDate = Convert.ToDateTime(holidayStartDate);
                //model.HolidayEndDate = Convert.ToDateTime(holidayEndDate);
            }

            return "ok";
        }

        public string DeleteHolidayData(string id)
        {
            long ids;
            long.TryParse(id, out ids);
            var deleteEvents = (from c in _dbEntities.GovernmentHolidayTables
                                where c.Id == ids
                                select c).FirstOrDefault();

            _dbEntities.GovernmentHolidayTables.Remove(deleteEvents);
            _dbEntities.SaveChanges();
            return "OK";
        }
        #endregion
        #region Capacity Planning

        public List<Pro_Type_Model> GetProductionType()
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Type_Model>(@"select * FROM [CellPhoneProject].[dbo].[Pro_Type] where IsActive=1").ToList();
            return query;
        }

        public string SaveShift(List<Pro_Shift_Model> issueList, int mon, string monName, int years, string productionType)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var proShift in issueList)
            {
                var model = new Pro_Shift();
                model.ProductionType = productionType;
                model.Month = monName;
                model.MonNum = mon;
                model.Year = years;
                model.Line = proShift.Line;
                model.Shift_1 = proShift.Shift_1;
                model.Shift_2 = proShift.Shift_2;
                model.Shift_3 = proShift.Shift_3;
                model.IsActive = true;

                model.Added = userId;
                model.AddedDate = DateTime.Now;

                _dbEntities.Pro_Shift.Add(model);
                _dbEntities.SaveChanges();
            }

            _dbEntities.SaveChanges();
            return "ok";
        }

        public List<Pro_Shift_Model> GetShiftSavedData(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select Id,Month,MonNum,ProductionType,Year,Line,Shift_1,case when Shift_2 in ('0',null) then NULL else Shift_2 end as Shift_2,
            case when Shift_3 in ('0',null) then NULL else Shift_3 end as Shift_3 from [PPMS].[dbo].[Pro_Shift] 
            where MonNum={0} and Year={1} and ProductionType={2} and IsActive=1  order by Line asc", mons, years, productionType).ToList();
            return query;
        }

        public List<Pro_Shift_Model> GetDailyShiftData(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            //            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"
            //	    	DECLARE @Month INT = {0}, @Year INT = {1};WITH MonthDays_CTE(DayNum)
            //	    AS
            //	    (
            //		    SELECT DATEFROMPARTS(@Year, @Month, 1) AS DayNum
            //			    UNION ALL
            //		    SELECT DATEADD(DAY, 1, DayNum)
            //		    FROM MonthDays_CTE
            //		    WHERE DayNum < EOMONTH(DATEFROMPARTS(@Year, @Month, 1))
            //	    )
            //	    select B.TotalDays, (B.dDayName+', '+ B.dMonth+' '+ cast(B.dDay as varchar(10))+', '+ cast(B.dYear as varchar(10))) as AllDays,sf.Line,sf.Shift_1,sf.Shift_2,sf.Shift_3 
            //	
            //	    from
            //	    (SELECT DayNum as TotalDays,DATEPART(YEAR,DayNum) as dYear,DATEPART(day,DayNum) as dDay, DATENAME(Month,DayNum) as dMonth, DATEPART(Month,DayNum) as dMonNum,
            //	    FORMAT(DayNum, 'dddd') as dDayName  FROM MonthDays_CTE A)B
            //
            //	    left join [CellPhoneProject].[dbo].[Pro_Shift] sf  on sf.MonNum=B.dMonNum", mons, years, productionType).ToList();

            //neww
            //            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"	    	
            //	        DECLARE @Month INT = {0}, @Year INT = {1};
            //	        WITH MonthDays_CTE(DayNum)
            //	        AS
            //	        (
            //		        SELECT DATEFROMPARTS(@Year, @Month, 1) AS DayNum
            //			        UNION ALL
            //		        SELECT DATEADD(DAY, 1, DayNum)
            //		        FROM MonthDays_CTE
            //		        WHERE DayNum < EOMONTH(DATEFROMPARTS(@Year, @Month, 1))
            //	        )
            //	        select B.TotalDays, (B.dDayName+', '+ B.dMonth+' '+ cast(B.dDay as varchar(10))+', '+ cast(B.dYear as varchar(10))) as AllDays
            //	
            //	        from
            //	        (SELECT DayNum as TotalDays,DATEPART(YEAR,DayNum) as dYear,DATEPART(day,DayNum) as dDay, DATENAME(Month,DayNum) as dMonth, DATEPART(Month,DayNum) as dMonNum,
            //	        FORMAT(DayNum, 'dddd') as dDayName  FROM MonthDays_CTE A)B  order by B.TotalDays asc", mons, years, productionType).ToList();

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"DECLARE @Month INT ={0}, @Year INT = {1};
            WITH MonthDays_CTE(DayNum)
            AS
            (
	            SELECT DATEFROMPARTS(@Year, @Month, 1) AS DayNum
		            UNION ALL
	            SELECT DATEADD(DAY, 1, DayNum)
	            FROM MonthDays_CTE
	            WHERE DayNum < EOMONTH(DATEFROMPARTS(@Year, @Month, 1))
            )
            select B.TotalDays, (B.dDayName+', '+ B.dMonth+' '+ cast(B.dDay as varchar(10))+', '+ cast(B.dYear as varchar(10))) as AllDays,
            case when B.TotalDays between gt.HolidayStartDate and gt.HolidayEndDate then gt.GovernmentHoliday end as Holidays
            from
            (SELECT DayNum as TotalDays,DATEPART(YEAR,DayNum) as dYear,DATEPART(day,DayNum) as dDay, DATENAME(Month,DayNum) as dMonth, DATEPART(Month,DayNum) as dMonNum,
            FORMAT(DayNum, 'dddd') as dDayName  FROM MonthDays_CTE A)B 
            left join  PPMS.dbo.GovernmentHolidayTable gt on B.TotalDays between gt.HolidayStartDate and gt.HolidayEndDate 
            order by B.TotalDays asc", mons, years, productionType).ToList();

            return query;
        }
        public List<Pro_Shift_Model> GetDailyShiftData1(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"DECLARE @Month INT ={0}, @Year INT = {1};
        WITH MonthDays_CTE(DayNum)
        AS
        (
	        SELECT DATEFROMPARTS(@Year, @Month, 1) AS DayNum
		        UNION ALL
	        SELECT DATEADD(DAY, 1, DayNum)
	        FROM MonthDays_CTE
	        WHERE DayNum < EOMONTH(DATEFROMPARTS(@Year, @Month, 1))
        )
        select B.TotalDays, (B.dDayName+', '+ B.dMonth+' '+ cast(B.dDay as varchar(10))+', '+ cast(B.dYear as varchar(10))) as AllDays,
case when B.TotalDays between gt.HolidayStartDate and gt.HolidayEndDate then gt.GovernmentHoliday end as Holidays,
        dp.ProductionType,dp.EffectiveDate, dp.Line,dp.Shift_1,
        case when dp.Shift_2 in ('0',null) then NULL else dp.Shift_2 end as Shift_2,
        case when dp.Shift_3 in ('0',null) then NULL else dp.Shift_3 end as Shift_3

        from
        (SELECT DayNum as TotalDays,DATEPART(YEAR,DayNum) as dYear,DATEPART(day,DayNum) as dDay, DATENAME(Month,DayNum) as dMonth, DATEPART(Month,DayNum) as dMonNum,
        FORMAT(DayNum, 'dddd') as dDayName  FROM MonthDays_CTE A)B 
        left join  PPMS.dbo.GovernmentHolidayTable gt on B.TotalDays between gt.HolidayStartDate and gt.HolidayEndDate 
        left join  [PPMS].[dbo].Pro_DailyPlan dp on dp.EffectiveDate=B.TotalDays
        and dp.MonNum={0} and dp.Year={1} and dp.ProductionType={2} 
        order by B.TotalDays asc", mons, years, productionType).ToList();

            return query;
        }

        public List<Pro_Shift_Model> DailySaved(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select * from [PPMS].[dbo].[Pro_DailyPlan] 
            where MonNum={0} and Year={1} and ProductionType={2} and IsActive=1 order by EffectiveDate asc", mons, years, productionType).ToList();
            return query;
        }


        public List<Pro_Shift_Model> GetLine(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select distinct Line from [PPMS].[dbo].[Pro_Shift] 
            where MonNum={0} and Year={1} and ProductionType={2} and IsActive=1 order by Line asc", mons, years, productionType).ToList();
            return query;
        }

        public List<Pro_Shift_Model> GetShift(int mons, string year, string productionType, string phoneType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"
            select Team as AllShift FROM [PPMS].[dbo].[Pro_Team]
            where ProductionType={2} and IsActive=1 and Team is not null",
            mons, years, productionType, phoneType).ToList();

            return query;
        }

        public string SaveCapacityData(List<Pro_CapacityPlanning_Model> results)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var res in results)
            {
                var vSplit = res.AllShift.Split(',');

                var phoneType = Convert.ToString(vSplit[8]);
                var categories = Convert.ToString(vSplit[3]);
                var produtionType = Convert.ToString(vSplit[9]);
                var mon = Convert.ToInt32(vSplit[6]);
                var yearss = Convert.ToInt32(vSplit[5]);

                var presentData = (from pm in _dbEntities.Pro_CapacityPlanning
                                   where pm.ProductName == phoneType
                                       && pm.ProductionType == produtionType
                                       && pm.CategoryName == categories
                                       && pm.MonNum == mon
                                       && pm.Year == yearss
                                   select pm).ToList();

                foreach (var pd in presentData)
                {
                    _dbEntities.Pro_CapacityPlanning.Remove(pd);
                    _dbEntities.SaveChanges();
                }

                if (vSplit[3] == "null")
                {
                    vSplit[3] = "";
                }
                if (vSplit[4] == "null")
                {
                    vSplit[4] = "";
                }

                if (vSplit[3] != "" && vSplit[4] != "")
                {

                    var model = new Pro_CapacityPlanning();
                    model.Percentage = Convert.ToInt32(vSplit[0]);
                    model.QuantityRange = Convert.ToString(vSplit[1]);
                    model.Team = Convert.ToString(vSplit[2]);
                    model.CategoryName = Convert.ToString(vSplit[3]);
                    model.TotalCapacity = Convert.ToDecimal(vSplit[4]);
                    model.Year = Convert.ToInt32(vSplit[5]);
                    model.MonNum = Convert.ToInt32(vSplit[6]);
                    model.Month = Convert.ToString(vSplit[7]);
                    model.ProductName = Convert.ToString(vSplit[8]);
                    model.ProductionType = Convert.ToString(vSplit[9]);
                    model.Product = "Mobile";
                    model.Added = userId;
                    model.IsActive = true;
                    model.AddedDate = DateTime.Now;

                    _dbEntities.Pro_CapacityPlanning.Add(model);
                }


            }
            _dbEntities.SaveChanges();

            return "ok";
        }

        public List<Pro_CapacityPlanning_Model> GetCapacity(int mons, string year, string productionType, string categories)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"SELECT Id,Team, ProductionType,CategoryName,Percentage,QuantityRange,TotalCapacity,ProductName
            FROM [PPMS].[dbo].[Pro_CapacityPlanning] where MonNum={0} and Year={1} and ProductionType={2} and CategoryName={3}
            group by Id,Team, ProductionType,CategoryName,Percentage,QuantityRange,TotalCapacity,ProductName
            order by Team,CategoryName asc", mons, years, productionType, categories).ToList();

            return query;
        }

        public List<Pro_CapacityPlanning_Model> GetTeam(int mons, string year, string productionType, string phoneType, string categories)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"select distinct Team from
            [PPMS].[dbo].[Pro_CapacityPlanning] where MonNum={0} and Year={1} and ProductionType={2} and ProductName={3} and CategoryName={4} order by Team asc", mons, years, productionType, phoneType, categories).ToList();

            return query;
        }
        public List<Pro_CapacityPlanning_Model> GetPercentage(int mons, string year, string productionType, string phoneType, string categories)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"select distinct Percentage from
            [PPMS].[dbo].[Pro_CapacityPlanning] where MonNum={0} and Year={1} and ProductionType={2} and ProductName={3} and CategoryName={4}  order by Percentage desc", mons, years, productionType, phoneType, categories).ToList();

            return query;
        }

        public List<Pro_CapacityPlanning_Model> GetQuantityRange(int mons, string year, string productionType, string phoneType, string categories)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"select distinct QuantityRange,Percentage from
            [PPMS].[dbo].[Pro_CapacityPlanning] where MonNum={0} and Year={1} and ProductionType={2} and ProductName={3} and CategoryName={4} order by Percentage desc", mons, years, productionType, phoneType, categories).ToList();

            return query;
        }

        public List<Pro_CapacityPlanning_Model> GetAll(int mons, string year, string productionType, string phoneType, string categories)
        {
            int years;
            int.TryParse(year, out years);

            var query1 = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"SELECT DISTINCT A.MonNum,A.Month,A.Year,A.ProductName,A.ProductionType,A.Team, A.CategoryName,
            STUFF((SELECT ',' + cast(cast(TotalCapacity AS int) as varchar) FROM [PPMS].[dbo].[Pro_CapacityPlanning] p
            where MonNum={0} and Year={1} and ProductionType=A.ProductionType
            and ProductName=A.ProductName  and Team=A.Team and CategoryName=A.CategoryName order by Percentage desc
            FOR XML PATH('')),1,1,'') AS TotalCapacities 

            FROM [PPMS].[dbo].[Pro_CapacityPlanning] AS A where MonNum={0} 
            and Year={1} and ProductionType={2} and ProductName={3}  and CategoryName={4} ", mons, years, productionType, phoneType, categories).ToList();

            foreach (var pro in query1)
            {
                var dd = pro.TotalCapacities.Split(',');
                pro.Team = pro.Team;
                pro.CategoryName = pro.CategoryName;
                for (int i = 0; i < dd.Length; i++)
                {
                    if (i == 0)
                    {
                        pro.TotalCap1 = dd[i];
                    }
                    if (i == 1)
                    {
                        pro.TotalCap2 = dd[i];
                    }
                    if (i == 2)
                    {
                        pro.TotalCap3 = dd[i];
                    }
                    if (i == 3)
                    {
                        pro.TotalCap4 = dd[i];
                    }
                    if (i == 4)
                    {
                        pro.TotalCap5 = dd[i];
                    }
                    if (i == 5)
                    {
                        pro.TotalCap6 = dd[i];
                    }
                    if (i == 6)
                    {
                        pro.TotalCap7 = dd[i];
                    }
                    if (i == 7)
                    {
                        pro.TotalCap8 = dd[i];
                    }
                    if (i == 8)
                    {
                        pro.TotalCap9 = dd[i];
                    }
                    if (i == 9)
                    {
                        pro.TotalCap10 = dd[i];
                    }
                }

            }


            return query1;
        }

        public string SaveTeam(List<Pro_Shift_Model> issueList1, string productionType)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var proTeam in issueList1)
            {
                var model = new Pro_Team();
                model.ProductionType = productionType;
                model.Team = proTeam.Team;
                model.IsActive = true;
                model.Added = userId;
                model.AddedDate = DateTime.Now;

                _dbEntities.Pro_Team.Add(model);
                _dbEntities.SaveChanges();
            }

            _dbEntities.SaveChanges();
            return "ok";
        }

        public List<string> GetAllTeam(string productionType)
        {
            List<String> list = (from emp in _dbEntities.Pro_Team
                                 where emp.IsActive == true && emp.ProductionType == productionType
                                 orderby emp.Team ascending
                                 group emp by emp.Team into empg
                                 select empg.Key).ToList();

            return list;
        }

        public List<string> GetAllCategory(string productionType11, string phoneType)
        {
            List<String> list = (from emp in _dbEntities.Pro_Product
                                 where emp.IsActive == true && emp.ProductionType == productionType11 && emp.ProductName == phoneType
                                 orderby emp.CategoryName ascending
                                 group emp by emp.CategoryName into empg
                                 select empg.Key).ToList();

            return list;
        }


        public List<Pro_Shift_Model> GetTeamForUpdate(string productionType)
        {

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select Id,Team,ProductionType from [PPMS].[dbo].[Pro_Team]
            where IsActive=1 and ProductionType={0}
            group by Id,Team,ProductionType
            order by ProductionType desc", productionType).ToList();

            return query;
        }

        public string UpdateTeam(long ids)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            var updateTms = (from c in _dbEntities.Pro_Team
                             where c.Id == ids
                             select c).FirstOrDefault();

            updateTms.IsActive = false;
            updateTms.Updated = userId;
            updateTms.UpdatedDate = DateTime.Now;
            _dbEntities.SaveChanges();
            return "OK";
        }


        public string SaveProduct(List<Pro_Shift_Model> issueList1, string productionType)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var proTeam in issueList1)
            {
                for (var ii = 0; ii < proTeam.Category.Count; ii++)
                {
                    var model = new Pro_Product();

                    if (proTeam.ProductName != null)
                    {
                        model.ProductionType = productionType;
                        model.Product = "Mobile";
                        model.ProductName = proTeam.ProductName;
                        model.ProductFamily = proTeam.ProductFamily;
                        model.ChangeOverTime = proTeam.ChangeOverTime;
                        model.CategoryName = proTeam.Category[ii];
                        model.IsActive = true;
                        model.Added = userId;
                        model.AddedDate = DateTime.Now;

                        _dbEntities.Pro_Product.Add(model);
                        _dbEntities.SaveChanges();
                    }

                }
            }

            _dbEntities.SaveChanges();
            return "OK";
        }

        public string EditTeam(string id, string team)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            long ids;
            long.TryParse(id, out ids);

            var updateTeam = (from c in _dbEntities.Pro_Team
                              where c.Id == ids
                              select c).FirstOrDefault();

            updateTeam.Team = team.Trim();
            updateTeam.Updated = userId;
            updateTeam.UpdatedDate = DateTime.Now;

            _dbEntities.SaveChanges();
            return "OK";
        }

        public string SaveLine(List<Pro_Shift_Model> issueList1, string productionType)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var proTeam in issueList1)
            {
                var model = new Pro_Line();

                model.ProductionType = productionType;
                model.Line = proTeam.Line;
                model.LineType = proTeam.LineType;
                model.ProductionDaysPerMonth = proTeam.ProductionDaysPerMonth;
                model.HoursPerShift = proTeam.HoursPerShift;
                model.ShiftPerDay = proTeam.ShiftPerDay;
                model.IsActive = true;
                model.Added = userId;
                model.AddedDate = DateTime.Now;

                _dbEntities.Pro_Line.Add(model);
                _dbEntities.SaveChanges();
            }

            _dbEntities.SaveChanges();
            return "ok";
        }

        public List<Pro_Shift_Model> GetLineForUpdate(string productionType)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select Id,ProductionType,Line,LineType,ProductionDaysPerMonth,HoursPerShift,
            ShiftPerDay
            from [PPMS].[dbo].[Pro_Line]
            where IsActive=1 and ProductionType={0}
            group by Id,ProductionType,Line,LineType,ProductionDaysPerMonth,HoursPerShift,ShiftPerDay
            order by ProductionType desc", productionType).ToList();

            return query;
        }

        public string EditLine(string id, string line, string lineType, string productionDaysPerMonth, string shiftPerDay,
            string hoursPerShift)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            long ids;
            long.TryParse(id, out ids);

            int productionDaysPerMonths;
            int.TryParse(productionDaysPerMonth, out productionDaysPerMonths);

            int shiftPerDays;
            int.TryParse(shiftPerDay, out shiftPerDays);

            decimal hoursPerShifts;
            decimal.TryParse(hoursPerShift, out hoursPerShifts);

            var updateTeam = (from c in _dbEntities.Pro_Line
                              where c.Id == ids
                              select c).FirstOrDefault();

            updateTeam.ProductionDaysPerMonth = productionDaysPerMonths;
            updateTeam.HoursPerShift = hoursPerShifts;
            updateTeam.ShiftPerDay = shiftPerDays;
            updateTeam.Updated = userId;
            updateTeam.UpdatedDate = DateTime.Now;

            _dbEntities.SaveChanges();
            return "OK";
        }

        public string InActiveLine(long ids)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            var updateTms = (from c in _dbEntities.Pro_Line
                             where c.Id == ids
                             select c).FirstOrDefault();

            updateTms.IsActive = false;
            updateTms.Updated = userId;
            updateTms.UpdatedDate = DateTime.Now;
            _dbEntities.SaveChanges();
            return "OK";
        }

        public List<Pro_Shift_Model> GetProductForUpdate(string productionType)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select Id,ProductionType,ProductFamily,ProductName,ChangeOverTime,
            CategoryName
            from [PPMS].[dbo].[Pro_Product]
            where IsActive=1 and ProductionType={0}
            group by Id,ProductionType,ProductName,ChangeOverTime,CategoryName,ProductFamily
            order by ProductionType desc", productionType).ToList();

            return query;
        }

        public string InActiveProduct(long ids)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            var updateTms = (from c in _dbEntities.Pro_Product
                             where c.Id == ids
                             select c).FirstOrDefault();

            updateTms.IsActive = false;
            updateTms.Updated = userId;
            updateTms.UpdatedDate = DateTime.Now;
            _dbEntities.SaveChanges();
            return "OK";
        }

        public List<string> GetAllLine(string productionType11)
        {
            List<String> list = (from emp in _dbEntities.Pro_Line
                                 where emp.IsActive == true && emp.ProductionType == productionType11
                                 orderby emp.Line ascending
                                 group emp by emp.Line into empg
                                 select empg.Key).ToList();

            return list;
        }

        public string InActiveShift(long ids)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            var updateTms = (from c in _dbEntities.Pro_Shift
                             where c.Id == ids
                             select c).FirstOrDefault();

            updateTms.IsActive = false;
            updateTms.Updated = userId;
            updateTms.UpdatedDate = DateTime.Now;
            _dbEntities.SaveChanges();
            return "OK";
        }

        public List<Pro_Shift_Model> GetProductName(string productionType)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select distinct ProductName
            from [PPMS].[dbo].[Pro_Product]
            where IsActive=1 and ProductionType={0}           
            order by ProductName desc", productionType).ToList();

            return query;
        }

        public List<Pro_Shift_Model> GetCategoryName(string productionType, string proPhoneName)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select distinct CategoryName
            from [PPMS].[dbo].[Pro_Product]
            where IsActive=1 and ProductionType={0} and ProductName={1}          
            order by CategoryName asc", productionType, proPhoneName).ToList();

            return query;
        }

        #endregion

        #region Capacity Report
        public List<Pro_CapacityPlanning_Model> GetAll1(int mons, string year, string productionType, string phoneType, string categories)
        {
            int years;
            int.TryParse(year, out years);

            var query1 = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"SELECT DISTINCT A.MonNum,A.Month,A.Year,A.ProductName,A.ProductionType,A.Team, A.CategoryName,
            STUFF((SELECT ',' + cast(cast(TotalCapacity AS int) as varchar) FROM [PPMS].[dbo].[Pro_CapacityPlanning] p
            where MonNum={0} and Year={1} and ProductionType=A.ProductionType
            and ProductName=A.ProductName  and Team=A.Team and CategoryName=A.CategoryName order by Percentage desc
            FOR XML PATH('')),1,1,'') AS TotalCapacities 

            FROM [PPMS].[dbo].[Pro_CapacityPlanning] AS A where MonNum={0}  and TotalCapacity not in (0.00)
            and Year={1} and ProductionType={2} and ProductName={3}  and CategoryName={4} ", mons, years, productionType, phoneType, categories).ToList();

            foreach (var pro in query1)
            {
                var dd = pro.TotalCapacities.Split(',');
                pro.Team = pro.Team;
                pro.CategoryName = pro.CategoryName;
                for (int i = 0; i < dd.Length; i++)
                {
                    if (i == 0)
                    {
                        pro.TotalCap1 = dd[i];
                    }
                    if (i == 1)
                    {
                        pro.TotalCap2 = dd[i];
                    }
                    if (i == 2)
                    {
                        pro.TotalCap3 = dd[i];
                    }
                    if (i == 3)
                    {
                        pro.TotalCap4 = dd[i];
                    }
                    if (i == 4)
                    {
                        pro.TotalCap5 = dd[i];
                    }
                    if (i == 5)
                    {
                        pro.TotalCap6 = dd[i];
                    }
                    if (i == 6)
                    {
                        pro.TotalCap7 = dd[i];
                    }
                    if (i == 7)
                    {
                        pro.TotalCap8 = dd[i];
                    }
                    if (i == 8)
                    {
                        pro.TotalCap9 = dd[i];
                    }
                    if (i == 9)
                    {
                        pro.TotalCap10 = dd[i];
                    }
                }

            }


            return query1;
        }



        private bool CheckedDailyPlanData(string productionType, DateTime effectiveDate, string line, int mon, int years)
        {
            List<Pro_Shift_Model> getIncentiveReports = null;
            if (productionType != "" && line != "")
            {
                string getIncentiveReportQuery = string.Format(@"select * from PPMS.dbo.Pro_DailyPlan
                where ProductionType='{0}' and EffectiveDate='{1}'
                and Line='{2}' and MonNum='{3}' and Year='{4}' ", productionType, effectiveDate, line, mon, years);
                getIncentiveReports = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(getIncentiveReportQuery).ToList();

            }
            if (getIncentiveReports != null && getIncentiveReports.Count != 0)
            {
                return true;
            }
            return false;
        }

        public string UpdateDailyPlan(long ids, DateTime effectiveDate, string line, string shift1, string shift2, string shift3,
          string productionType, string monNum, string month, string year)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            int mon;
            int.TryParse(monNum, out mon);

            int years;
            int.TryParse(year, out years);


            //bool issueCheck = CheckedDailyPlanData(productionType, effectiveDate, line, mon, years);


            var queries = (from pm in _dbEntities.Pro_DailyPlan
                           where pm.Id == ids
                           select pm).FirstOrDefault();
            if (queries != null)
            {
                if (shift1 != "")
                {
                    queries.Shift_1 = shift1;
                }
                if (shift2 != "")
                {
                    queries.Shift_2 = shift2;
                }
                if (shift3 != "")
                {
                    queries.Shift_3 = shift3;
                }
                queries.Updated = userId;
                queries.UpdatedDate = DateTime.Now;

                _dbEntities.Pro_DailyPlan.AddOrUpdate(queries);
                _dbEntities.SaveChanges();
                return "OK";
            }
            return "OK";
        }

        public string SaveDailyPlan(List<Pro_Shift_Model> results)//hi
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var res in results)
            {
                var vSplit = res.AllShift.Split(',');

                var effect = Convert.ToDateTime(vSplit[0]);
                var produtionType = Convert.ToString(vSplit[8]);
                var line = Convert.ToString(vSplit[1]);
                var mon = Convert.ToInt32(vSplit[6]);
                var yearss = Convert.ToInt32(vSplit[5]);

                var presentData = (from pm in _dbEntities.Pro_DailyPlan
                                   where pm.ProductionType == produtionType
                                       && pm.EffectiveDate == effect
                                       && pm.Line == line
                                       && pm.MonNum == mon
                                       && pm.Year == yearss
                                   select pm).FirstOrDefault();
                if (presentData != null)
                {
                    presentData.Shift_1 = Convert.ToString(vSplit[2]);
                    presentData.Shift_2 = Convert.ToString(vSplit[3]);
                    presentData.Shift_3 = Convert.ToString(vSplit[4]);

                    _dbEntities.Pro_DailyPlan.AddOrUpdate(presentData);
                    _dbEntities.SaveChanges();
                }
                else
                {
                    var model = new Pro_DailyPlan();
                    model.ProductionType = Convert.ToString(vSplit[8]);
                    model.EffectiveDate = Convert.ToDateTime(vSplit[0]);
                    model.Line = Convert.ToString(vSplit[1]);
                    if (vSplit[2] == "")
                    {
                        model.Shift_1 = null;
                    }
                    else
                    {
                        model.Shift_1 = Convert.ToString(vSplit[2]);
                    }

                    model.Shift_2 = Convert.ToString(vSplit[3]);

                    if (vSplit[4] == "")
                    {
                        model.Shift_3 = null;
                    }
                    else
                    {
                        model.Shift_3 = Convert.ToString(vSplit[4]);
                    }
                    model.MonNum = Convert.ToInt32(vSplit[6]);
                    model.Month = Convert.ToString(vSplit[7]);
                    model.Year = Convert.ToInt32(vSplit[5]);
                    model.Added = userId;
                    model.AddedDate = DateTime.Now;
                    model.IsActive = true;

                    _dbEntities.Pro_DailyPlan.Add(model);
                }


            }
            _dbEntities.SaveChanges();

            return "ok";
        }

        public List<Pro_Shift_Model> ProductNameForReport(int mons, string year, string productionType)
        {

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"
            select distinct B.ProductName,B.TotalTeam,C.ChangeOverTime from
            (select ProductName, count(Team) as TotalTeam
            from
            (select distinct Team,ProductName from [PPMS].[dbo].[Pro_CapacityPlanning] 
            where MonNum={0} and Year={1} and IsActive=1 and ProductionType={2} and TotalCapacity not in (0.00))A
            group by ProductName )B
            left join PPMS.dbo.Pro_Product C on C.ProductName=B.ProductName

            where ChangeOverTime = (select top 1 ChangeOverTime From PPMS.dbo.Pro_Product pr
            where pr.ProductName=B.ProductName and pr.ProductionType={2} and IsActive=1  and pr.ChangeOverTime is not null)

            order by B.ProductName asc", mons, year, productionType).ToList();

            return query;
        }

        public List<Pro_Shift_Model> TeamNameForReport(int mons, string year, string productionType)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select Team, count(CategoryName) as TotalCategory1,ProductName from
            (

                select distinct CategoryName,Team,ProductName from [PPMS].[dbo].[Pro_CapacityPlanning]
                where MonNum={0} and Year={1} and IsActive=1 and ProductionType={2} and TotalCapacity not in (0.00) 

            )A
            group by Team,ProductName order by Team asc", mons, year, productionType).ToList();

            return query;
        }

        public List<Pro_Shift_Model> CategoryNameForReport(int mons, string year, string productionType)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@" select CategoryName,Team,ProductName from
            (

            select distinct CategoryName,Team,ProductName from [PPMS].[dbo].[Pro_CapacityPlanning]
            where MonNum={0} and Year={1} and IsActive=1 and ProductionType={2} and TotalCapacity not in (0.00) 

            )A
            group by Team,ProductName,CategoryName order by Team asc", mons, year, productionType).ToList();

            return query;
        }

        public List<Pro_CapacityPlanning_Model> GetPercentage1(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"select distinct Percentage,ProductName from
            [PPMS].[dbo].[Pro_CapacityPlanning] where MonNum={0} and Year={1} and ProductionType={2}  order by Percentage desc", mons, years, productionType).ToList();

            return query;
        }

        public List<Pro_CapacityPlanning_Model> GetQuantityRange1(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"select distinct QuantityRange,Percentage,ProductName from
            [PPMS].[dbo].[Pro_CapacityPlanning] where MonNum={0} and Year={1} and ProductionType={2} order by Percentage desc", mons, years, productionType).ToList();

            return query;
        }

        public List<Pro_CapacityPlanning_Model> GetTotalCapacities1(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_CapacityPlanning_Model>(@"select CategoryName,cast(cast(TotalCapacity AS int) as varchar) as TotalCapacity2,Percentage,QuantityRange,Team,ProductName from
            (
                select distinct CategoryName,TotalCapacity,Percentage,QuantityRange,Team,ProductName from [PPMS].[dbo].[Pro_CapacityPlanning]
                where MonNum={0} and Year={1} and IsActive=1 and ProductionType={2} and TotalCapacity not in (0.00) 
            )A
            group by Team,ProductName,CategoryName,TotalCapacity,Percentage,QuantityRange order by Team,Percentage desc", mons, years, productionType).ToList();

            return query;
        }
        #endregion

        #region Project Categorize
        public List<Pro_Shift_Model> GetProjectForCategorization()
        {

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select distinct pm.ProjectName,
            case when ProjectType='Feature' then 'Featurephone'  
            when ProjectType='Smart' then 'Smartphone' else ProjectType end as ProjectType,pc.AssemblyCategory,pc.SmtCategory,pc.HousingCategory
            from CellPhoneProject.dbo.ProjectMasters pm 
            left join [PPMS].[dbo].[ProjectCategorization] pc on pm.ProjectName=pc.ProjectName

            where pm.IsActive=1 and pm.AddedDate between '2019-01-01' and GETDATE()
            and pm.ProjectName not in (select ProjectName from [PPMS].[dbo].[ProjectCategorization] where IsComplete=1 and ProjectName=pm.ProjectName)
            order by pm.ProjectName asc").ToList();

            return query;
        }

        public List<Pro_Shift_Model> GetAssemblyCategory(string projectType)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select ProductionType,ProductName,ProductFamily,CategoryName as AssemblyCategory 
            from  [PPMS].[dbo].[Pro_Product]   
            where ProductionType in ('Assembly','Charger','Battery','Earphone') 
            and ProductFamily={0} and IsActive=1", projectType).ToList();

            return query;
        }

        public List<Pro_Shift_Model> GetSmtCategory(string smtCategory)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select ProductionType,ProductName,ProductFamily,CategoryName as SmtCategory from  [PPMS].[dbo].[Pro_Product]   
              where ProductionType in ('SMT') and ProductFamily={0} and IsActive=1", smtCategory).ToList();

            return query;
        }

        public List<Pro_Shift_Model> GetHousingCategory(string housingCategory)
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select ProductionType,ProductName,ProductFamily,CategoryName as HousingCategory 
            from  [PPMS].[dbo].[Pro_Product]   
            where ProductionType in ('Housing') and ProductFamily={0} and IsActive=1", housingCategory).ToList();

            return query;
        }

        public bool CheckedCategorizedData(string projectName, string productFamily)
        {
            List<Custom_Sw_IncentiveModel> getIncentiveReports = null;
            if (projectName != "" && productFamily != "")
            {
                string getIncentiveReportQuery = string.Format(@"select *
                from [PPMS].[dbo].[ProjectCategorization] where ProjectName='{0}'
                and  ProductFamily='{1}' ", projectName, productFamily);
                getIncentiveReports = _dbEntities.Database.SqlQuery<Custom_Sw_IncentiveModel>(getIncentiveReportQuery).ToList();

            }
            if (getIncentiveReports != null && getIncentiveReports.Count != 0)
            {
                return true;
            }
            return false;
        }

        public string SaveCategorizeData(string projectName, string productFamily, string assemblyCategory, string smtCategory,
            string housingCategory)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            bool issueCheck = CheckedCategorizedData(projectName, productFamily);

            if (issueCheck)
            {
                var queries = (from pm in _dbEntities.ProjectCategorizations
                               where pm.ProjectName == projectName && pm.ProductFamily == productFamily
                               select pm).FirstOrDefault();

                if (assemblyCategory != "")
                {
                    queries.AssemblyCategory = assemblyCategory;
                }
                if (smtCategory != "")
                {
                    queries.SmtCategory = smtCategory;
                }
                if (assemblyCategory != "")
                {
                    queries.HousingCategory = housingCategory;
                }
                _dbEntities.ProjectCategorizations.AddOrUpdate(queries);
                _dbEntities.SaveChanges();

                return "Okis";
            }
            else
            {
                var model = new ProjectCategorization();
                model.ProjectName = projectName;
                model.ProductFamily = productFamily;
                model.AssemblyCategory = assemblyCategory;
                model.SmtCategory = smtCategory;
                model.HousingCategory = housingCategory;
                model.IsComplete = false;
                model.Added = userId;
                model.AddedDate = DateTime.Now;

                _dbEntities.ProjectCategorizations.Add(model);
                _dbEntities.SaveChanges();

            }
            return "OK";
        }

        public string CompleteCategorizeData(string projectName, string productFamily)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            bool issueCheck = CheckedCategorizedData(projectName, productFamily);

            if (issueCheck)
            {
                var queries = (from pm in _dbEntities.ProjectCategorizations
                               where pm.ProjectName == projectName && pm.ProductFamily == productFamily
                               select pm).FirstOrDefault();

                queries.IsComplete = true;
                queries.Updated = userId;
                queries.UpdatedDate = DateTime.Now;

                _dbEntities.ProjectCategorizations.AddOrUpdate(queries);
                _dbEntities.SaveChanges();
            }

            return "OK";
        }

        public List<Pro_Shift_Model> GetCompletedCategorization()
        {
            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select * from  [PPMS].[dbo].[ProjectCategorization]  
              where IsComplete=1 order by ProjectName asc").ToList();

            return query;
        }

        public string UpdateCategorizeData(long ids, string assemblyCategory1, string smtCategory1, string housingCategory1)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            var queries = (from pm in _dbEntities.ProjectCategorizations
                           where pm.Id == ids
                           select pm).FirstOrDefault();

            if (assemblyCategory1 != "")
            {
                queries.AssemblyCategory = assemblyCategory1;
            }
            if (smtCategory1 != "")
            {
                queries.SmtCategory = smtCategory1;
            }
            if (assemblyCategory1 != "")
            {
                queries.HousingCategory = housingCategory1;
            }
            _dbEntities.ProjectCategorizations.AddOrUpdate(queries);
            _dbEntities.SaveChanges();


            return "OK";
        }

        public List<Pro_Shift_Model> ChangedDailyPlanData(int mons, string year, string productionType)
        {
            int years;
            int.TryParse(year, out years);

            var query = _dbEntities.Database.SqlQuery<Pro_Shift_Model>(@"select ProductionType,EffectiveDate, Line,Shift_1,case when Shift_2 in ('0',null) then NULL else Shift_2 end as Shift_2,
            case when Shift_3 in ('0',null) then NULL else Shift_3 end as Shift_3,Month,Year,MonNum 
            from [PPMS].[dbo].Pro_DailyPlan 
            where MonNum={0} and Year={1} and ProductionType={2} and IsActive=1 ", mons, years, productionType).ToList();
            return query;
        }
        public bool CheckShiftDatas(string unitValues, int forwardedYear1, int forwardedMonNum1, string line)
        {
            var chkPro = new List<Pro_Shift_Model>();

            string proEv = string.Format(@"select * from [PPMS].[dbo].[Pro_Shift] pm
              where pm.ProductionType ='{0}' and
             pm.Line ='{1}' and pm.Year ='{2}' and pm.MonNum = '{3}' and pm.IsActive=1",
             unitValues, line, forwardedYear1, forwardedMonNum1);

            chkPro =
                   _dbEntities.Database.SqlQuery<Pro_Shift_Model>(proEv).ToList();

            if (chkPro != null && chkPro.Count != 0)
            {
                return true;
            }
            return false;
        }
        public string ForwardShift(string unitValues, string currentDate, string forwardedDate, string shiftForward)
        {

            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            var currentDate1 = currentDate.Split(',');
            var currentMonth = currentDate1[0].Trim();
            var currentYear = currentDate1[1].Trim();
            int currentYear1 = Convert.ToInt32(currentYear);
            int currentMonNum1 = DateTime.ParseExact(currentMonth, "MMMM", CultureInfo.CurrentCulture).Month;

            var forwardedDate1 = forwardedDate.Split(',');
            var forwardedMonth = forwardedDate1[0].Trim();
            var forwardedYear = forwardedDate1[1].Trim();
            int forwardedYear1 = Convert.ToInt32(forwardedYear);
            int forwardedMonNum1 = DateTime.ParseExact(forwardedMonth, "MMMM", CultureInfo.CurrentCulture).Month;

            var qryList = (from prSf in _dbEntities.Pro_Shift
                           where prSf.Year == currentYear1 && prSf.MonNum == currentMonNum1 && prSf.ProductionType == unitValues &&
                           prSf.IsActive != false

                           select prSf).ToList();


            foreach (var qq in qryList)
            {

                bool isSaved = CheckShiftDatas(unitValues, forwardedYear1, forwardedMonNum1, qq.Line);

                if (!isSaved)
                {
                    var model = new Pro_Shift();

                    model.ProductionType = qq.ProductionType;
                    model.Month = forwardedMonth;
                    model.MonNum = forwardedMonNum1;
                    model.Year = forwardedYear1;
                    model.Line = qq.Line;
                    if (qq.Shift_1 == "0")
                    {
                        qq.Shift_1 = null;
                    }
                    else
                    {
                        model.Shift_1 = qq.Shift_1;
                    }
                    if (qq.Shift_2 == "0")
                    {
                        qq.Shift_2 = null;
                    }
                    else
                    {
                        model.Shift_2 = qq.Shift_2;
                    }
                    if (qq.Shift_3 == "0")
                    {
                        qq.Shift_3 = null;
                    }
                    else
                    {
                        model.Shift_3 = qq.Shift_3;
                    }

                    model.IsActive = true;
                    model.Added = userId;
                    model.AddedDate = DateTime.Now;

                    _dbEntities.Pro_Shift.AddOrUpdate(model);
                    _dbEntities.SaveChanges();
                }
            }
            return "OK";
        }
        public bool CheckCapacityDatas(string unitValues, int forwardedYear1, int forwardedMonNum1, string Team,
                   int Percentage, string QuantityRange, string ProductName, string CategoryName, decimal TotalCapacity)
        {
            var chkPro = new List<Pro_Shift_Model>();

            string proEv = string.Format(@"select * from [PPMS].[dbo].[Pro_CapacityPlanning] pm
              where pm.ProductionType ='{0}' and pm.Year ='{1}' and pm.MonNum = '{2}' and pm.Team='{3}'
              and pm.Percentage='{4}' and pm.QuantityRange='{5}' and pm.ProductName='{6}' 
              and pm.CategoryName='{7}' and pm.TotalCapacity='{8}'
              and pm.IsActive=1",
             unitValues, forwardedYear1, forwardedMonNum1, Team,
             Percentage, QuantityRange, ProductName, CategoryName, TotalCapacity);

            chkPro =
                   _dbEntities.Database.SqlQuery<Pro_Shift_Model>(proEv).ToList();

            if (chkPro != null && chkPro.Count != 0)
            {
                return true;
            }
            return false;
        }
        public string ForwardCapacity(string unitValues, string currentDate, string forwardedDate, string capForward)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            var currentDate1 = currentDate.Split(',');
            var currentMonth = currentDate1[0].Trim();
            var currentYear = currentDate1[1].Trim();
            int currentYear1 = Convert.ToInt32(currentYear);
            int currentMonNum1 = DateTime.ParseExact(currentMonth, "MMMM", CultureInfo.CurrentCulture).Month;

            var forwardedDate1 = forwardedDate.Split(',');
            var forwardedMonth = forwardedDate1[0].Trim();
            var forwardedYear = forwardedDate1[1].Trim();
            int forwardedYear1 = Convert.ToInt32(forwardedYear);
            int forwardedMonNum1 = DateTime.ParseExact(forwardedMonth, "MMMM", CultureInfo.CurrentCulture).Month;

            var qryList = (from prSf in _dbEntities.Pro_CapacityPlanning
                           where prSf.Year == currentYear1 && prSf.MonNum == currentMonNum1 && prSf.ProductionType == unitValues
                           && prSf.IsActive != false
                           select prSf).ToList();

            foreach (var qq in qryList)
            {

                bool isSaved = CheckCapacityDatas(unitValues, forwardedYear1, forwardedMonNum1, qq.Team,
                    Convert.ToInt32(qq.Percentage), qq.QuantityRange, qq.ProductName, qq.CategoryName, Convert.ToDecimal(qq.TotalCapacity));

                if (!isSaved)
                {
                    var model = new Pro_CapacityPlanning();

                    model.ProductionType = qq.ProductionType;
                    model.Month = forwardedMonth;
                    model.MonNum = forwardedMonNum1;
                    model.Year = forwardedYear1;
                    model.Team = qq.Team;
                    model.CategoryName = qq.CategoryName;
                    model.Percentage = qq.Percentage;
                    model.QuantityRange = qq.QuantityRange;
                    model.TotalCapacity = qq.TotalCapacity;
                    model.ProductName = qq.ProductName;
                    model.Product = qq.Product;
                    model.IsActive = true;
                    model.Added = userId;
                    model.AddedDate = DateTime.Now;

                    _dbEntities.Pro_CapacityPlanning.AddOrUpdate(model);
                    _dbEntities.SaveChanges();
                }

            }
            return "OK";
        }

        #endregion

        #region Handset
        public List<ProMasterModel> GetProjectList()
        {
            _cellPhoneProjectEntities.Database.CommandTimeout = 6000;
            string proEv = string.Format(@"select distinct ppd.ProjectModel as ProjectName,ppd.ProjectMasterId as ProjectMasterID,pm.OrderNuber as OrderNumber,ppo.PoCategory,ppd.OrderQuantity from CellPhoneProject.dbo.[ProjectOrderQuantityDetails] ppd
            inner join CellPhoneProject.dbo.ProjectPurchaseOrderForms ppo on ppd.ProjectMasterId=ppo.ProjectMasterId
            inner join CellPhoneProject.dbo.ProjectMasters pm on pm.ProjectMasterId=ppd.ProjectMasterId
            where pm.ProjectMasterId=ppo.ProjectMasterId and ppo.PoCategory in ('SKD','CKD') and pm.IsActive=1 and ppd.IsActive=1
            order by ppd.ProjectModel,pm.OrderNuber asc");
            //OrderQuantity

            var proEvent = _dbEntities.Database.SqlQuery<ProMasterModel>(proEv).ToList();

            foreach (var model in proEvent)
            {
                var ext = !string.IsNullOrWhiteSpace(model.PoCategory) ? "/" + model.PoCategory : "";
                if (model.OrderNumber != null)
                {
                    model.ProjectName = model.ProjectName + "(" + CommonConversion.AddOrdinal((int)model.OrderNumber) +
                                        " Order)" + ext + "/" + Convert.ToString(model.OrderQuantity);
                }

            }
            return proEvent;
        }

        public List<Pro_MaterialRulesModel> GetMaterialRulesList()
        {
            _dbEntities.Database.CommandTimeout = 6000;
            string query = string.Format(@"select [MaterialRulesId],[Product],[ProductionUnit],[TypeOfWork],[IsActive],[SerialNo],[ProductionUnitSerialNo],[MaterialRules],[StartDays],[EndDays] 
            from [PPMS].[dbo].[Pro_MaterialRules] where IsActive=1 ");
            var exe = _dbEntities.Database.SqlQuery<Pro_MaterialRulesModel>(query).ToList();
            return exe;
        }

        public string SaveHandsetPlanningData(List<CustomHandsetPlan> results)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var insResult in results)
            {

                var query1 = (from c in _cellPhoneProjectEntities.ProjectMasters
                              where c.ProjectMasterId == insResult.ProjectMasterID
                              select c).FirstOrDefault();

                var query2 = (from c in _cellPhoneProjectEntities.ProjectPurchaseOrderForms
                              where c.ProjectMasterId == insResult.ProjectMasterID
                              select c).FirstOrDefault();

                var proPlan = new Pro_PlanTable();

                proPlan.ProjectMasterID = insResult.ProjectMasterID;
                proPlan.IsCharger = false;
                proPlan.IsHandset = true;
                proPlan.IsEarphone = false;
                proPlan.IsActive = true;
                proPlan.AddedDate = DateTime.Now;
                proPlan.Added = userId;
                _dbEntities.Pro_PlanTable.Add(proPlan);
                _dbEntities.SaveChanges();

                var proPlanIDs = proPlan.PlanId;

                if (proPlanIDs != null)
                {
                    if (insResult.AssemblyChk == true)
                    {
                        var assemb = new Pro_HandsetAssemblyAndPacking();
                        assemb.PlanId = proPlanIDs;
                        assemb.ProjectMasterID = insResult.ProjectMasterID;
                        assemb.ProjectName = query1.ProjectName;
                        assemb.OrderNumber = query1.OrderNuber;
                        assemb.PoCategory = query2.PoCategory;

                        assemb.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoAssembly;
                        assemb.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoAssembly;
                        assemb.HAssemblyMaterialDelayReason = insResult.HAssemblyMaterialDelayReason;
                        assemb.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoAssembly;
                        assemb.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoAssembly;
                        assemb.HAssemblyIqcDelayReason = insResult.HAssemblyIqcDelayReason;
                        assemb.Trial_SDate_Auto = insResult.Trial_SDate_AutoAssembly;
                        assemb.Trial_EDate_Auto = insResult.Trial_EDate_AutoAssembly;
                        assemb.HAssemblyTrialDelayReason = insResult.HAssemblyTrialDelayReason;
                        assemb.SoftwareConfirmation_SDate_Auto = insResult.SoftwareConfirmation_SDate_Auto;
                        assemb.SoftwareConfirmation_EDate_Auto = insResult.SoftwareConfirmation_EDate_Auto;
                        assemb.HAssemblySoftComDelayReason = insResult.HAssemblySoftComDelayReason;
                        assemb.RndConfirmation_SDate_Auto = insResult.RndConfirmation_SDate_Auto;
                        assemb.RndConfirmation_EDate_Auto = insResult.RndConfirmation_EDate_Auto;
                        assemb.HAssemblyRndDelayReason = insResult.HAssemblyRndDelayReason;
                        assemb.AssemblyProduction_SDate_Auto = insResult.AssemblyProduction_SDate_Auto;
                        assemb.AssemblyProduction_EDate_Auto = insResult.AssemblyProduction_EDate_Auto;
                        assemb.HAssemblyDelayReason = insResult.HAssemblyDelayReason;
                        assemb.Packing_SDate_Auto = insResult.Packing_SDate_Auto;
                        assemb.Packing_EDate_Auto = insResult.Packing_EDate_Auto;
                        assemb.HAssemblyPackingDelayReason = insResult.HAssemblyPackingDelayReason;
                        assemb.TotalOrderQuantity = insResult.TotalOrderQuantity;
                        assemb.MaterialRules = insResult.MaterialRules;
                        assemb.IsActive = true;
                        assemb.HandsetAssemAndPackStatus = "PARTIAL";
                        assemb.Added = userId;
                        assemb.AddedDate = DateTime.Now;

                        _dbEntities.Pro_HandsetAssemblyAndPacking.Add(assemb);
                        _dbEntities.SaveChanges();

                    }//end assembly

                    if (insResult.SmtChk == true)
                    {
                        var smt = new Pro_HandsetSmt();
                        smt.PlanId = proPlanIDs;
                        smt.ProjectMasterID = insResult.ProjectMasterID;
                        smt.ProjectName = query1.ProjectName;
                        smt.OrderNumber = query1.OrderNuber;
                        smt.PoCategory = query2.PoCategory;

                        smt.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoSmt;
                        smt.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoSmt;
                        smt.HandsetSmtDelayReason = insResult.HandsetSmtDelayReason;
                        smt.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoSmt;
                        smt.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoSmt;
                        smt.HandsetIqcDelayReason = insResult.HandsetIqcDelayReason;
                        smt.Trial_SDate_Auto = insResult.Trial_SDate_AutoSmt;
                        smt.Trial_EDate_Auto = insResult.Trial_EDate_AutoSmt;
                        smt.HandsetTrialDelayReason = insResult.HandsetTrialDelayReason;
                        smt.MassProduction_SDate_Auto = insResult.MassProduction_SDate_AutoSmt;
                        smt.MassProduction_EDate_Auto = insResult.MassProduction_EDate_AutoSmt;
                        smt.HandsetMpDelayReason = insResult.HandsetMpDelayReason;
                        smt.TotalOrderQuantity = insResult.TotalOrderQuantity;
                        smt.MaterialRules = insResult.MaterialRules;
                        smt.IsActive = true;
                        smt.HandsetSmtStatus = "PARTIAL";
                        smt.Added = userId;
                        smt.AddedDate = DateTime.Now;

                        _dbEntities.Pro_HandsetSmt.Add(smt);
                        _dbEntities.SaveChanges();
                    }//end smt
                    if (insResult.HouseChk == true)
                    {
                        var housing = new Pro_HandsetHousing();
                        housing.PlanId = proPlanIDs;
                        housing.ProjectMasterID = insResult.ProjectMasterID;
                        housing.ProjectName = query1.ProjectName;
                        housing.OrderNumber = query1.OrderNuber;
                        housing.PoCategory = query2.PoCategory;

                        housing.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoHousing;
                        housing.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoHousing;
                        housing.HHousingMaterialDelayReason = insResult.HHousingMaterialDelayReason;
                        housing.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoHousing;
                        housing.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoHousing;
                        housing.HHousingIqcDelayReason = insResult.HHousingIqcDelayReason;
                        housing.Trial_SDate_Auto = insResult.Trial_SDate_AutoHousing;
                        housing.Trial_EDate_Auto = insResult.Trial_EDate_AutoHousing;
                        housing.HHousingTrialDelayReason = insResult.HHousingTrialDelayReason;
                        housing.ReliabilityTest_SDate_Auto = insResult.ReliabilityTest_SDate_AutoHousing;
                        housing.ReliabilityTest_EDate_Auto = insResult.ReliabilityTest_EDate_AutoHousing;
                        housing.HHousingReliabilityDelayReason = insResult.HHousingReliabilityDelayReason;
                        housing.MassProduction_SDate_Auto = insResult.MassProduction_SDate_AutoHousing;
                        housing.MassProduction_EDate_Auto = insResult.MassProduction_EDate_AutoHousing;
                        housing.HHousingMpDelayReason = insResult.HHousingMpDelayReason;

                        housing.TotalOrderQuantity = insResult.TotalOrderQuantity;
                        housing.MaterialRules = insResult.MaterialRules;
                        housing.IsActive = true;
                        housing.HandsetHousingStatus = "PARTIAL";
                        housing.Added = userId;
                        housing.AddedDate = DateTime.Now;

                        _dbEntities.Pro_HandsetHousing.Add(housing);
                        _dbEntities.SaveChanges();
                    }//end housing
                    if (insResult.BatteryChk == true)
                    {
                        var battery = new Pro_HandsetBattery();
                        battery.PlanId = proPlanIDs;
                        battery.ProjectMasterID = insResult.ProjectMasterID;
                        battery.ProjectName = query1.ProjectName;
                        battery.OrderNumber = query1.OrderNuber;
                        battery.PoCategory = query2.PoCategory;

                        battery.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoBattery;
                        battery.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoBattery;
                        battery.HBatteryMaterialDelayReason = insResult.HBatteryMaterialDelayReason;
                        battery.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoBattery;
                        battery.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoBattery;
                        battery.HBatteryIqcDelayReason = insResult.HBatteryIqcDelayReason;
                        battery.Trial_SDate_Auto = insResult.Trial_SDate_AutoBattery;
                        battery.Trial_EDate_Auto = insResult.Trial_EDate_AutoBattery;
                        battery.HBatteryTrialDelayReason = insResult.HBatteryTrialDelayReason;
                        battery.ReliabilityTest_SDate_Auto = insResult.ReliabilityTest_SDate_AutoBattery;
                        battery.ReliabilityTest_EDate_Auto = insResult.ReliabilityTest_EDate_AutoBattery;
                        battery.HBatteryReliabilityDelayReason = insResult.HBatteryReliabilityDelayReason;
                        battery.MassProduction_SDate_Auto = insResult.MassProduction_SDate_AutoBattery;
                        battery.MassProduction_EDate_Auto = insResult.MassProduction_EDate_AutoBattery;
                        battery.HBatteryMpDelayReason = insResult.HBatteryMpDelayReason;
                        battery.AgingTest_SDate_Auto = insResult.AgingTest_SDate_AutoBattery;
                        battery.AgingTest_EDate_Auto = insResult.AgingTest_EDate_AutoBattery;
                        battery.HBatteryAgingDelayReason = insResult.HBatteryAgingDelayReason;

                        battery.TotalOrderQuantity = insResult.TotalOrderQuantity;
                        battery.MaterialRules = insResult.MaterialRules;
                        battery.IsActive = true;
                        battery.HandsetBatteryStatus = "PARTIAL";
                        battery.Added = userId;
                        battery.AddedDate = DateTime.Now;

                        _dbEntities.Pro_HandsetBattery.Add(battery);
                        _dbEntities.SaveChanges();
                    }//end Battery
                }
            }
            _dbEntities.SaveChanges();

            return "ok";
        }

        public string UpdateHandsetPlanningData(List<CustomHandsetPlan> results)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            foreach (var insResult in results)
            {
                var proPlanIDs = insResult.AsmPlanId;

                if (proPlanIDs != null)
                {
                    if (insResult.AssemblyChk == true)
                    {
                        var assemb = _dbEntities.Pro_HandsetAssemblyAndPacking.FirstOrDefault(i => i.PlanId == proPlanIDs);

                        if (assemb != null)
                        {
                            assemb.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoAssembly;
                            assemb.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoAssembly;
                            assemb.HAssemblyMaterialDelayReason = insResult.HAssemblyMaterialDelayReason;
                            assemb.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoAssembly;
                            assemb.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoAssembly;
                            assemb.HAssemblyIqcDelayReason = insResult.HAssemblyIqcDelayReason;
                            assemb.Trial_SDate_Auto = insResult.Trial_SDate_AutoAssembly;
                            assemb.Trial_EDate_Auto = insResult.Trial_EDate_AutoAssembly;
                            assemb.HAssemblyTrialDelayReason = insResult.HAssemblyTrialDelayReason;
                            assemb.SoftwareConfirmation_SDate_Auto = insResult.SoftwareConfirmation_SDate_Auto;
                            assemb.SoftwareConfirmation_EDate_Auto = insResult.SoftwareConfirmation_EDate_Auto;
                            assemb.HAssemblySoftComDelayReason = insResult.HAssemblySoftComDelayReason;
                            assemb.RndConfirmation_SDate_Auto = insResult.RndConfirmation_SDate_Auto;
                            assemb.RndConfirmation_EDate_Auto = insResult.RndConfirmation_EDate_Auto;
                            assemb.HAssemblyRndDelayReason = insResult.HAssemblyRndDelayReason;
                            assemb.AssemblyProduction_SDate_Auto = insResult.AssemblyProduction_SDate_Auto;
                            assemb.AssemblyProduction_EDate_Auto = insResult.AssemblyProduction_EDate_Auto;
                            assemb.HAssemblyDelayReason = insResult.HAssemblyDelayReason;
                            assemb.Packing_SDate_Auto = insResult.Packing_SDate_Auto;
                            assemb.Packing_EDate_Auto = insResult.Packing_EDate_Auto;
                            assemb.HAssemblyPackingDelayReason = insResult.HAssemblyPackingDelayReason;
                            assemb.TotalOrderQuantity = insResult.TotalOrderQuantity;

                            assemb.MaterialReceive_SDate_Manual = insResult.MaterialReceive_SDate_ManualAssembly;
                            assemb.MaterialReceive_EDate_Manual = insResult.MaterialReceive_EDate_ManualAssembly;
                            assemb.Iqc_SDate_Manual = insResult.Iqc_SDate_ManualAssembly;
                            assemb.Iqc_EDate_Manual = insResult.Iqc_EDate_ManualAssembly;
                            assemb.Trial_SDate_Manual = insResult.Trial_SDate_ManualAssembly;
                            assemb.Trial_EDate_Manual = insResult.Trial_EDate_ManualAssembly;
                            assemb.SoftwareConfirmation_SDate_Manual = insResult.SoftwareConfirmation_SDate_Manual;
                            assemb.SoftwareConfirmation_EDate_Manual = insResult.SoftwareConfirmation_EDate_Manual;
                            assemb.RndConfirmation_SDate_Manual = insResult.RndConfirmation_SDate_Manual;
                            assemb.RndConfirmation_EDate_Manual = insResult.RndConfirmation_EDate_Manual;
                            assemb.AssemblyProduction_SDate_Manual = insResult.AssemblyProduction_SDate_Manual;
                            assemb.AssemblyProduction_EDate_Manual = insResult.AssemblyProduction_EDate_Manual;
                            assemb.Packing_SDate_Manual = insResult.Packing_SDate_Manual;
                            assemb.Packing_EDate_Manual = insResult.Packing_EDate_Manual;

                            assemb.MaterialRules = insResult.MaterialRules;
                            assemb.IsActive = true;
                            assemb.HandsetAssemAndPackStatus = "PARTIAL";
                            assemb.Updated = userId;
                            assemb.UpdatedDate = DateTime.Now;

                            _dbEntities.Entry(assemb).State = EntityState.Modified;
                            _dbEntities.SaveChanges();
                        }

                    }//end assembly

                    //smt
                    if (insResult.SmtChk == true)
                    {
                        var smt = _dbEntities.Pro_HandsetSmt.FirstOrDefault(i => i.PlanId == proPlanIDs);

                        if (smt != null)
                        {
                            smt.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoSmt;
                            smt.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoSmt;
                            smt.HandsetSmtDelayReason = insResult.HandsetSmtDelayReason;
                            smt.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoSmt;
                            smt.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoSmt;
                            smt.HandsetIqcDelayReason = insResult.HandsetIqcDelayReason;
                            smt.Trial_SDate_Auto = insResult.Trial_SDate_AutoSmt;
                            smt.Trial_EDate_Auto = insResult.Trial_EDate_AutoSmt;
                            smt.HandsetTrialDelayReason = insResult.HandsetTrialDelayReason;
                            smt.MassProduction_SDate_Auto = insResult.MassProduction_SDate_AutoSmt;
                            smt.MassProduction_EDate_Auto = insResult.MassProduction_EDate_AutoSmt;
                            smt.HandsetMpDelayReason = insResult.HandsetMpDelayReason;

                            smt.MaterialReceive_SDate_Manual = insResult.MaterialReceive_SDate_ManualSmt;
                            smt.MaterialReceive_EDate_Manual = insResult.MaterialReceive_EDate_ManualSmt;
                            smt.Iqc_SDate_Manual = insResult.Iqc_SDate_ManualSmt;
                            smt.Iqc_EDate_Manual = insResult.Iqc_EDate_ManualSmt;
                            smt.Trial_SDate_Manual = insResult.Trial_SDate_ManualSmt;
                            smt.Trial_EDate_Manual = insResult.Trial_EDate_ManualSmt;
                            smt.MassProduction_SDate_Manual = insResult.MassProduction_SDate_ManualSmt;
                            smt.MassProduction_EDate_Manual = insResult.MassProduction_EDate_ManualSmt;

                            smt.TotalOrderQuantity = insResult.TotalOrderQuantity;
                            smt.MaterialRules = insResult.MaterialRules;
                            smt.IsActive = true;
                            smt.HandsetSmtStatus = "PARTIAL";
                            smt.Updated = userId;
                            smt.UpdatedDate = DateTime.Now;
                            _dbEntities.Entry(smt).State = EntityState.Modified;
                            _dbEntities.SaveChanges();
                        }
                    }//end smt
                    if (insResult.HouseChk == true)
                    {
                        var housing = _dbEntities.Pro_HandsetHousing.FirstOrDefault(i => i.PlanId == proPlanIDs);

                        if (housing != null)
                        {
                            housing.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoHousing;
                            housing.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoHousing;
                            housing.HHousingMaterialDelayReason = insResult.HHousingMaterialDelayReason;
                            housing.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoHousing;
                            housing.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoHousing;
                            housing.HHousingIqcDelayReason = insResult.HHousingIqcDelayReason;
                            housing.Trial_SDate_Auto = insResult.Trial_SDate_AutoHousing;
                            housing.Trial_EDate_Auto = insResult.Trial_EDate_AutoHousing;
                            housing.HHousingTrialDelayReason = insResult.HHousingTrialDelayReason;
                            housing.ReliabilityTest_SDate_Auto = insResult.ReliabilityTest_SDate_AutoHousing;
                            housing.ReliabilityTest_EDate_Auto = insResult.ReliabilityTest_EDate_AutoHousing;
                            housing.HHousingReliabilityDelayReason = insResult.HHousingReliabilityDelayReason;
                            housing.MassProduction_SDate_Auto = insResult.MassProduction_SDate_AutoHousing;
                            housing.MassProduction_EDate_Auto = insResult.MassProduction_EDate_AutoHousing;
                            housing.HHousingMpDelayReason = insResult.HHousingMpDelayReason;

                            housing.MaterialReceive_SDate_Manual = insResult.MaterialReceive_SDate_ManualHousing;
                            housing.MaterialReceive_EDate_Manual = insResult.MaterialReceive_EDate_ManualHousing;
                            housing.Iqc_SDate_Manual = insResult.Iqc_SDate_ManualHousing;
                            housing.Iqc_EDate_Manual = insResult.Iqc_EDate_ManualHousing;
                            housing.Trial_SDate_Manual = insResult.Trial_SDate_ManualHousing;
                            housing.Trial_EDate_Manual = insResult.Trial_EDate_ManualHousing;
                            housing.ReliabilityTest_SDate_Manual = insResult.ReliabilityTest_SDate_ManualHousing;
                            housing.ReliabilityTest_EDate_Manual = insResult.ReliabilityTest_EDate_ManualHousing;
                            housing.MassProduction_SDate_Manual = insResult.MassProduction_SDate_ManualHousing;
                            housing.MassProduction_EDate_Manual = insResult.MassProduction_EDate_ManualHousing;

                            housing.TotalOrderQuantity = insResult.TotalOrderQuantity;
                            housing.MaterialRules = insResult.MaterialRules;
                            housing.IsActive = true;
                            housing.HandsetHousingStatus = "PARTIAL";
                            housing.Updated = userId;
                            housing.UpdatedDate = DateTime.Now;

                            _dbEntities.Entry(housing).State = EntityState.Modified;
                            _dbEntities.SaveChanges();
                        }

                    }//end housing
                    if (insResult.BatteryChk == true)
                    {
                        var battery = _dbEntities.Pro_HandsetBattery.FirstOrDefault(i => i.PlanId == proPlanIDs);

                        if (battery != null)
                        {
                            battery.MaterialReceive_SDate_Auto = insResult.MaterialReceive_SDate_AutoBattery;
                            battery.MaterialReceive_EDate_Auto = insResult.MaterialReceive_EDate_AutoBattery;
                            battery.HBatteryMaterialDelayReason = insResult.HBatteryMaterialDelayReason;
                            battery.Iqc_SDate_Auto = insResult.Iqc_SDate_AutoBattery;
                            battery.Iqc_EDate_Auto = insResult.Iqc_EDate_AutoBattery;
                            battery.HBatteryIqcDelayReason = insResult.HBatteryIqcDelayReason;
                            battery.Trial_SDate_Auto = insResult.Trial_SDate_AutoBattery;
                            battery.Trial_EDate_Auto = insResult.Trial_EDate_AutoBattery;
                            battery.HBatteryTrialDelayReason = insResult.HBatteryTrialDelayReason;
                            battery.ReliabilityTest_SDate_Auto = insResult.ReliabilityTest_SDate_AutoBattery;
                            battery.ReliabilityTest_EDate_Auto = insResult.ReliabilityTest_EDate_AutoBattery;
                            battery.HBatteryReliabilityDelayReason = insResult.HBatteryReliabilityDelayReason;
                            battery.MassProduction_SDate_Auto = insResult.MassProduction_SDate_AutoBattery;
                            battery.MassProduction_EDate_Auto = insResult.MassProduction_EDate_AutoBattery;
                            battery.HBatteryMpDelayReason = insResult.HBatteryMpDelayReason;
                            battery.AgingTest_SDate_Auto = insResult.AgingTest_SDate_AutoBattery;
                            battery.AgingTest_EDate_Auto = insResult.AgingTest_EDate_AutoBattery;
                            battery.HBatteryAgingDelayReason = insResult.HBatteryAgingDelayReason;

                            battery.MaterialReceive_SDate_Manual = insResult.MaterialReceive_SDate_ManualBattery;
                            battery.MaterialReceive_EDate_Manual = insResult.MaterialReceive_EDate_ManualBattery;
                            battery.Iqc_SDate_Manual = insResult.Iqc_SDate_ManualBattery;
                            battery.Iqc_EDate_Manual = insResult.Iqc_EDate_ManualBattery;
                            battery.Trial_SDate_Manual = insResult.Trial_SDate_ManualBattery;
                            battery.Trial_EDate_Manual = insResult.Trial_EDate_ManualBattery;
                            battery.ReliabilityTest_SDate_Manual = insResult.ReliabilityTest_SDate_ManualBattery;
                            battery.ReliabilityTest_EDate_Manual = insResult.ReliabilityTest_EDate_ManualBattery;
                            battery.MassProduction_SDate_Manual = insResult.MassProduction_SDate_ManualBattery;
                            battery.MassProduction_EDate_Manual = insResult.MassProduction_EDate_ManualBattery;
                            battery.AgingTest_SDate_Manual = insResult.AgingTest_SDate_ManualBattery;
                            battery.AgingTest_EDate_Manual = insResult.AgingTest_EDate_ManualBattery;

                            battery.TotalOrderQuantity = insResult.TotalOrderQuantity;
                            battery.MaterialRules = insResult.MaterialRules;
                            battery.IsActive = true;
                            battery.HandsetBatteryStatus = "PARTIAL";
                            battery.Updated = userId;
                            battery.UpdatedDate = DateTime.Now;

                            _dbEntities.Entry(battery).State = EntityState.Modified;
                            _dbEntities.SaveChanges();
                        }
                    }//end Battery
                }
            }
            _dbEntities.SaveChanges();

            return "ok";
        }

        public List<CustomHandsetPlan> GetProjectPlanningHistoryData(long proIds, string projectName)
        {
            _dbEntities.Database.CommandTimeout = 6000;
            string query = string.Format(@"select  case when asm.IsActive=1 then 'ACTIVE' else 'INACTIVE' end as ActiveStatus,asm.MaterialRules,case when asm.TotalOrderQuantity is null then 0 else asm.TotalOrderQuantity end as TotalOrderQuantity,
            asm.ProjectMasterID,asm.PlanId as AsmPlanId,case when bb.PlanId is null then 0 else bb.PlanId end as BbPlanId, case when bh.PlanId is null then 0 else bh.PlanId end as BhPlanId,case when smt.PlanId is null then 0 else smt.PlanId end as SmtPlanId,case when smt.HandsetSmtId is null then 0 else smt.HandsetSmtId end as HandsetSmtId,case when asm.HandsetAssemblyId is null then 0 else asm.HandsetAssemblyId end as HandsetAssemblyId,
            case when bb.HandsetBatteryId is null then 0 else bb.HandsetBatteryId end as HandsetBatteryId,case when bh.HandsetHousingId is null then 0 else bh.HandsetHousingId end as HandsetHousingId,asm.ProjectName,asm.MaterialReceive_SDate_Auto as MaterialReceive_SDate_AutoAssembly,asm.MaterialReceive_EDate_Auto as MaterialReceive_EDate_AutoAssembly,
            asm.MaterialReceive_SDate_Manual as MaterialReceive_SDate_ManualAssembly,asm.MaterialReceive_EDate_Manual as MaterialReceive_EDate_ManualAssembly,
            asm.Iqc_SDate_Auto as Iqc_SDate_AutoAssembly,asm.Iqc_EDate_Auto as Iqc_EDate_AutoAssembly,asm.Iqc_SDate_Manual as Iqc_SDate_ManualAssembly, 
            asm.Iqc_EDate_Manual as Iqc_EDate_ManualAssembly,asm.Trial_SDate_Auto as Trial_SDate_AutoAssembly,asm.Trial_EDate_Auto as Trial_EDate_AutoAssembly,asm.Trial_SDate_Manual as Trial_SDate_ManualAssembly, 
            asm.Trial_EDate_Manual	as Trial_EDate_ManualAssembly,asm.SoftwareConfirmation_SDate_Auto, asm.SoftwareConfirmation_EDate_Auto,asm.SoftwareConfirmation_SDate_Manual,asm.SoftwareConfirmation_EDate_Manual,asm.RndConfirmation_SDate_Auto,
            asm.RndConfirmation_EDate_Auto,asm.RndConfirmation_SDate_Manual,asm.RndConfirmation_EDate_Manual,asm.AssemblyProduction_SDate_Auto,asm.AssemblyProduction_EDate_Auto,asm.AssemblyProduction_SDate_Manual,asm.AssemblyProduction_EDate_Manual,
            asm.Packing_SDate_Auto,asm.Packing_EDate_Auto,asm.Packing_SDate_Manual,asm.Packing_EDate_Manual,smt.MassProduction_SDate_Auto as  MaterialReceive_SDate_AutoSmt,smt.MassProduction_EDate_Auto as MaterialReceive_EDate_AutoSmt,
            smt.MaterialReceive_SDate_Manual as MaterialReceive_SDate_ManualSmt,smt.MaterialReceive_EDate_Manual as MaterialReceive_EDate_ManualSmt,
            smt.Iqc_SDate_Auto as Iqc_SDate_AutoSmt,smt.Iqc_EDate_Auto as Iqc_EDate_AutoSmt,smt.Iqc_SDate_Manual as Iqc_SDate_ManualSmt, smt.Iqc_EDate_Manual as Iqc_EDate_ManualSmt,
            smt.Trial_SDate_Auto as Trial_SDate_AutoSmt,smt.Trial_EDate_Auto as Trial_EDate_AutoSmt,smt.Trial_SDate_Manual as Trial_SDate_ManualSmt,smt.Trial_EDate_Manual as Trial_EDate_ManualSmt,
            smt.MassProduction_SDate_Auto as MassProduction_SDate_AutoSmt,smt.MassProduction_EDate_Auto as MassProduction_EDate_AutoSmt,
            smt.MassProduction_SDate_Manual as MassProduction_SDate_ManualSmt,smt.MassProduction_EDate_Manual as MassProduction_EDate_ManualSmt,
            bh.MaterialReceive_SDate_Auto as MaterialReceive_SDate_AutoHousing,bh.MaterialReceive_EDate_Auto as MaterialReceive_EDate_AutoHousing,bh.MaterialReceive_SDate_Manual	as MaterialReceive_SDate_ManualHousing,bh.MaterialReceive_EDate_Manual	as MaterialReceive_EDate_ManualHousing,
            bh.Iqc_SDate_Auto as Iqc_SDate_AutoHousing,bh.Iqc_EDate_Auto as Iqc_EDate_AutoHousing,bh.Iqc_SDate_Manual as Iqc_SDate_ManualHousing,bh.Iqc_EDate_Manual as Iqc_EDate_ManualHousing,
            bh.Trial_SDate_Auto	as Trial_SDate_AutoHousing,bh.Trial_EDate_Auto	as Trial_EDate_AutoHousing,bh.Trial_SDate_Manual as Trial_SDate_ManualHousing,bh.Trial_EDate_Manual as Trial_EDate_ManualHousing,
            bh.ReliabilityTest_SDate_Auto as ReliabilityTest_SDate_AutoHousing,bh.ReliabilityTest_EDate_Auto as ReliabilityTest_EDate_AutoHousing,bh.ReliabilityTest_SDate_Manual	as ReliabilityTest_SDate_ManualHousing,bh.ReliabilityTest_EDate_Manual	as ReliabilityTest_EDate_ManualHousing,bh.MassProduction_SDate_Auto as MassProduction_SDate_AutoHousing,
            bh.MassProduction_EDate_Auto as MassProduction_EDate_AutoHousing,bh.MassProduction_SDate_Manual as MassProduction_SDate_ManualHousing,
            bh.MassProduction_EDate_Manual as MassProduction_EDate_ManualHousing,bb.MaterialReceive_SDate_Auto as MaterialReceive_SDate_AutoBattery,bb.MaterialReceive_EDate_Auto as MaterialReceive_EDate_AutoBattery,bb.MaterialReceive_SDate_Manual as MaterialReceive_SDate_ManualBattery,bb.MaterialReceive_EDate_Manual as MaterialReceive_EDate_ManualBattery,bb.Iqc_SDate_Auto as Iqc_SDate_AutoBattery,bb.Iqc_EDate_Auto as Iqc_EDate_AutoBattery,
            bb.Iqc_SDate_Manual as Iqc_SDate_ManualBattery,bb.Iqc_EDate_Manual as Iqc_EDate_ManualBattery,bb.Trial_SDate_Auto as Trial_SDate_AutoBattery,
            bb.Trial_EDate_Auto as Trial_EDate_AutoBattery,bb.Trial_SDate_Manual as Trial_SDate_ManualBattery,bb.Trial_EDate_Manual as Trial_EDate_ManualBattery,bb.ReliabilityTest_SDate_Auto as ReliabilityTest_SDate_AutoBattery,
            bb.ReliabilityTest_EDate_Auto as ReliabilityTest_EDate_AutoBattery,bb.ReliabilityTest_SDate_Manual as ReliabilityTest_SDate_ManualBattery,bb.ReliabilityTest_EDate_Manual as ReliabilityTest_EDate_ManualBattery,
            bb.MassProduction_SDate_Auto as MassProduction_SDate_AutoBattery,bb.MassProduction_EDate_Auto as MassProduction_EDate_AutoBattery,bb.MassProduction_SDate_Manual as MassProduction_SDate_ManualBattery,
            bb.MassProduction_EDate_Manual as MassProduction_EDate_ManualBattery,bb.AgingTest_SDate_Auto as AgingTest_SDate_AutoBattery,bb.AgingTest_EDate_Auto	as AgingTest_EDate_AutoBattery,
            bb.AgingTest_SDate_Manual as AgingTest_SDate_ManualBattery,bb.AgingTest_EDate_Manual as AgingTest_EDate_ManualBattery,
            bb.HBatteryMaterialDelayReason,bb.HBatteryIqcDelayReason,bb.HBatteryTrialDelayReason,bb.HBatteryReliabilityDelayReason,bb.HBatteryMpDelayReason,
            bb.HBatteryAgingDelayReason,bb.HBatteryPackingDelayReason,asm.HAssemblyMaterialDelayReason,asm.HAssemblyIqcDelayReason,
            asm.HAssemblyTrialDelayReason,asm.HAssemblySoftComDelayReason,asm.HAssemblyRndDelayReason,asm.HAssemblyDelayReason,asm.HAssemblyPackingDelayReason,
            bh.HHousingMaterialDelayReason,bh.HHousingIqcDelayReason,bh.HHousingTrialDelayReason,bh.HHousingReliabilityDelayReason,bh.HHousingMpDelayReason,bh.HHousingPackingDelayReason,
            smt.HandsetSmtDelayReason, smt.HandsetIqcDelayReason,smt.HandsetTrialDelayReason,smt.HandsetMpDelayReason,smt.HandsetPackingDelayReason

            FROM [PPMS].[dbo].Pro_HandsetAssemblyAndPacking asm												 
            left join PPMS.dbo.Pro_HandsetSmt smt on smt.ProjectMasterID=asm.ProjectMasterID and smt.PlanId=asm.PlanId
            left join PPMS.dbo.Pro_HandsetBattery bb on bb.ProjectMasterID=asm.ProjectMasterID and bb.PlanId=asm.PlanId
            left join PPMS.dbo.Pro_HandsetHousing bh on bh.ProjectMasterID=asm.ProjectMasterID and bh.PlanId=asm.PlanId
            where asm.ProjectMasterID='{0}' and asm.IsActive=1 
            order by asm.AddedDate desc", proIds);
            var exe = _dbEntities.Database.SqlQuery<CustomHandsetPlan>(query).ToList();

            //foreach (var custBatteryPro in exe)
            //{
            //}
            return exe;
        }

        public List<CustomHandsetPlan> GetHandsetOldHistory(long proIds, long planId)
        {
            _dbEntities.Database.CommandTimeout = 6000;
            var query = string.Format(@"select  case when asm.IsActive=1 then 'ACTIVE' else 'INACTIVE' end as ActiveStatus,asm.MaterialRules,case when asm.TotalOrderQuantity is null then 0 else asm.TotalOrderQuantity end as TotalOrderQuantity,
            asm.ProjectMasterID,case when asm.PlanId is null then 0 else asm.PlanId end as AsmPlanId,case when bb.PlanId is null then 0 else bb.PlanId end as BbPlanId,case when bh.PlanId is null then 0 else bh.PlanId end as BhPlanId,case when smt.PlanId is null then 0 else smt.PlanId end as SmtPlanId,case when smt.HandsetSmtId is null then 0 else smt.HandsetSmtId end as HandsetSmtId,case when asm.HandsetAssemblyId is null then 0 else asm.HandsetAssemblyId end as  HandsetAssemblyId,
            case when bb.HandsetBatteryId is null then 0 else bb.HandsetBatteryId end as HandsetBatteryId,case when bh.HandsetHousingId is null then 0 else bh.HandsetHousingId end as HandsetHousingId,asm.ProjectName,asm.MaterialReceive_SDate_Auto as MaterialReceive_SDate_AutoAssembly,asm.MaterialReceive_EDate_Auto as MaterialReceive_EDate_AutoAssembly,
            asm.MaterialReceive_SDate_Manual as MaterialReceive_SDate_ManualAssembly,asm.MaterialReceive_EDate_Manual as MaterialReceive_EDate_ManualAssembly,
            asm.Iqc_SDate_Auto as Iqc_SDate_AutoAssembly,asm.Iqc_EDate_Auto as Iqc_EDate_AutoAssembly,asm.Iqc_SDate_Manual as Iqc_SDate_ManualAssembly, 
            asm.Iqc_EDate_Manual as Iqc_EDate_ManualAssembly,asm.Trial_SDate_Auto as Trial_SDate_AutoAssembly,asm.Trial_EDate_Auto as Trial_EDate_AutoAssembly,asm.Trial_SDate_Manual as Trial_SDate_ManualAssembly, 
            asm.Trial_EDate_Manual	as Trial_EDate_ManualAssembly,asm.SoftwareConfirmation_SDate_Auto, asm.SoftwareConfirmation_EDate_Auto,asm.SoftwareConfirmation_SDate_Manual,asm.SoftwareConfirmation_EDate_Manual,asm.RndConfirmation_SDate_Auto,
            asm.RndConfirmation_EDate_Auto,asm.RndConfirmation_SDate_Manual,asm.RndConfirmation_EDate_Manual,asm.AssemblyProduction_SDate_Auto,asm.AssemblyProduction_EDate_Auto,asm.AssemblyProduction_SDate_Manual,asm.AssemblyProduction_EDate_Manual,
            asm.Packing_SDate_Auto,asm.Packing_EDate_Auto,asm.Packing_SDate_Manual,asm.Packing_EDate_Manual,smt.MassProduction_SDate_Auto as  MaterialReceive_SDate_AutoSmt,smt.MassProduction_EDate_Auto as MaterialReceive_EDate_AutoSmt,
            smt.MaterialReceive_SDate_Manual as MaterialReceive_SDate_ManualSmt,smt.MaterialReceive_EDate_Manual as MaterialReceive_EDate_ManualSmt,
            smt.Iqc_SDate_Auto as Iqc_SDate_AutoSmt,smt.Iqc_EDate_Auto as Iqc_EDate_AutoSmt,smt.Iqc_SDate_Manual as Iqc_SDate_ManualSmt, smt.Iqc_EDate_Manual as Iqc_EDate_ManualSmt,
            smt.Trial_SDate_Auto as Trial_SDate_AutoSmt,smt.Trial_EDate_Auto as Trial_EDate_AutoSmt,smt.Trial_SDate_Manual as Trial_SDate_ManualSmt,smt.Trial_EDate_Manual as Trial_EDate_ManualSmt,
            smt.MassProduction_SDate_Auto as MassProduction_SDate_AutoSmt,smt.MassProduction_EDate_Auto as MassProduction_EDate_AutoSmt,
            smt.MassProduction_SDate_Manual as MassProduction_SDate_ManualSmt,smt.MassProduction_EDate_Manual as MassProduction_EDate_ManualSmt,
            bh.MaterialReceive_SDate_Auto as MaterialReceive_SDate_AutoHousing,bh.MaterialReceive_EDate_Auto as MaterialReceive_EDate_AutoHousing,bh.MaterialReceive_SDate_Manual	as MaterialReceive_SDate_ManualHousing,bh.MaterialReceive_EDate_Manual	as MaterialReceive_EDate_ManualHousing,
            bh.Iqc_SDate_Auto as Iqc_SDate_AutoHousing,bh.Iqc_EDate_Auto as Iqc_EDate_AutoHousing,bh.Iqc_SDate_Manual as Iqc_SDate_ManualHousing,bh.Iqc_EDate_Manual as Iqc_EDate_ManualHousing,
            bh.Trial_SDate_Auto	as Trial_SDate_AutoHousing,bh.Trial_EDate_Auto	as Trial_EDate_AutoHousing,bh.Trial_SDate_Manual as Trial_SDate_ManualHousing,bh.Trial_EDate_Manual as Trial_EDate_ManualHousing,
            bh.ReliabilityTest_SDate_Auto as ReliabilityTest_SDate_AutoHousing,bh.ReliabilityTest_EDate_Auto as ReliabilityTest_EDate_AutoHousing,bh.ReliabilityTest_SDate_Manual	as ReliabilityTest_SDate_ManualHousing,bh.ReliabilityTest_EDate_Manual	as ReliabilityTest_EDate_ManualHousing,bh.MassProduction_SDate_Auto as MassProduction_SDate_AutoHousing,
            bh.MassProduction_EDate_Auto as MassProduction_EDate_AutoHousing,bh.MassProduction_SDate_Manual as MassProduction_SDate_ManualHousing,
            bh.MassProduction_EDate_Manual as MassProduction_EDate_ManualHousing,bb.MaterialReceive_SDate_Auto as MaterialReceive_SDate_AutoBattery,bb.MaterialReceive_EDate_Auto as MaterialReceive_EDate_AutoBattery,bb.MaterialReceive_SDate_Manual as MaterialReceive_SDate_ManualBattery,bb.MaterialReceive_EDate_Manual as MaterialReceive_EDate_ManualBattery,bb.Iqc_SDate_Auto as Iqc_SDate_AutoBattery,bb.Iqc_EDate_Auto as Iqc_EDate_AutoBattery,
            bb.Iqc_SDate_Manual as Iqc_SDate_ManualBattery,bb.Iqc_EDate_Manual as Iqc_EDate_ManualBattery,bb.Trial_SDate_Auto as Trial_SDate_AutoBattery,
            bb.Trial_EDate_Auto as Trial_EDate_AutoBattery,bb.Trial_SDate_Manual as Trial_SDate_ManualBattery,bb.Trial_EDate_Manual as Trial_EDate_ManualBattery,bb.ReliabilityTest_SDate_Auto as ReliabilityTest_SDate_AutoBattery,
            bb.ReliabilityTest_EDate_Auto as ReliabilityTest_EDate_AutoBattery,bb.ReliabilityTest_SDate_Manual as ReliabilityTest_SDate_ManualBattery,bb.ReliabilityTest_EDate_Manual as ReliabilityTest_EDate_ManualBattery,
            bb.MassProduction_SDate_Auto as MassProduction_SDate_AutoBattery,bb.MassProduction_EDate_Auto as MassProduction_EDate_AutoBattery,bb.MassProduction_SDate_Manual as MassProduction_SDate_ManualBattery,
            bb.MassProduction_EDate_Manual as MassProduction_EDate_ManualBattery,bb.AgingTest_SDate_Auto as AgingTest_SDate_AutoBattery,bb.AgingTest_EDate_Auto	as AgingTest_EDate_AutoBattery,
            bb.AgingTest_SDate_Manual as AgingTest_SDate_ManualBattery,bb.AgingTest_EDate_Manual as AgingTest_EDate_ManualBattery,
            bb.HBatteryMaterialDelayReason,bb.HBatteryIqcDelayReason,bb.HBatteryTrialDelayReason,bb.HBatteryReliabilityDelayReason,bb.HBatteryMpDelayReason,
            bb.HBatteryAgingDelayReason,bb.HBatteryPackingDelayReason,asm.HAssemblyMaterialDelayReason,asm.HAssemblyIqcDelayReason,
            asm.HAssemblyTrialDelayReason,asm.HAssemblySoftComDelayReason,asm.HAssemblyRndDelayReason,asm.HAssemblyDelayReason,asm.HAssemblyPackingDelayReason,
            bh.HHousingMaterialDelayReason,bh.HHousingIqcDelayReason,bh.HHousingTrialDelayReason,bh.HHousingReliabilityDelayReason,bh.HHousingMpDelayReason,bh.HHousingPackingDelayReason,
            smt.HandsetSmtDelayReason, smt.HandsetIqcDelayReason,smt.HandsetTrialDelayReason,smt.HandsetMpDelayReason,smt.HandsetPackingDelayReason


            FROM [PPMS].[dbo].Pro_HandsetAssemblyAndPacking asm												 
            left join PPMS.dbo.Pro_HandsetSmt smt on smt.ProjectMasterID=asm.ProjectMasterID and smt.PlanId=asm.PlanId
            left join PPMS.dbo.Pro_HandsetBattery bb on bb.ProjectMasterID=asm.ProjectMasterID and bb.PlanId=asm.PlanId
            left join PPMS.dbo.Pro_HandsetHousing bh on bh.ProjectMasterID=asm.ProjectMasterID and bh.PlanId=asm.PlanId
            where asm.IsActive=1 and  (asm.planid={0})
            order by asm.AddedDate desc", planId);

            var exe = _dbEntities.Database.SqlQuery<CustomHandsetPlan>(query).ToList();

            return exe;
        }

        public string InActiveAnyPlan(long proIds, long planId)
        {
            String userIdentity = System.Web.HttpContext.Current.User.Identity.Name;
            long userId = Convert.ToInt64(userIdentity == "" ? "0" : userIdentity);

            //
            var plantbl = (from c in _dbEntities.Pro_PlanTable where c.PlanId == planId select c).FirstOrDefault();

            plantbl.IsActive = false;
            _dbEntities.Pro_PlanTable.AddOrUpdate(plantbl);
            _dbEntities.SaveChanges();
            //
            var assembly = (from c in _dbEntities.Pro_HandsetAssemblyAndPacking where c.PlanId == planId select c).FirstOrDefault();

            assembly.IsActive = false;
            _dbEntities.Pro_HandsetAssemblyAndPacking.AddOrUpdate(assembly);
            _dbEntities.SaveChanges();
            //
            var battery = (from c in _dbEntities.Pro_HandsetBattery where c.PlanId == planId select c).FirstOrDefault();
            if (battery != null)
            {
                battery.IsActive = false;
                _dbEntities.Pro_HandsetBattery.AddOrUpdate(battery);
                _dbEntities.SaveChanges();
            }
            //
            var housing = (from c in _dbEntities.Pro_HandsetHousing where c.PlanId == planId select c).FirstOrDefault();
            if (housing != null)
            {
                housing.IsActive = false;
                _dbEntities.Pro_HandsetHousing.AddOrUpdate(housing);
                _dbEntities.SaveChanges();
            }
            //
            var smt = (from c in _dbEntities.Pro_HandsetSmt where c.PlanId == planId select c).FirstOrDefault();
            if (smt != null)
            {
                smt.IsActive = false;
                _dbEntities.Pro_HandsetSmt.AddOrUpdate(smt);
                _dbEntities.SaveChanges();
            }
            return "ok";
        }

        #endregion

        #region Inventory Plan
        public List<ProMasterModel> GetInventoryProjectList()
        {
            _dbEntities.Database.CommandTimeout = 6000;
//            string proEv = string.Format(@"select ProjectMasterID,PlanId,ProjectName,OrderNumber,PoCategory,TotalOrderQuantity, 
//                        pm.ProjectName +' (Order '+cast(OrderNumber as varchar) + ')/'+PoCategory+'/'+cast(TotalOrderQuantity as varchar) as ProName
//                        FROM [PPMS].[dbo].[Pro_HandsetAssemblyAndPacking] pm");

            string proEv = string.Format(@"select ProjectMasterID,cast(PlanId as varchar) as PlanIds,ProjectName,OrderNumber,PoCategory,TotalOrderQuantity, 
            pm.ProjectName +' (Order '+cast(OrderNumber as varchar) + ')/'+PoCategory+'/'+cast(TotalOrderQuantity as varchar) as ProName
            FROM [PPMS].[dbo].[Pro_HandsetAssemblyAndPacking] pm

            union 

            select ProjectMasterID=0,PlanIds='0',ProjectName='',OrderNumber=0,PoCategory='',TotalOrderQuantity=0, 
            ProName='ALL'
            FROM [PPMS].[dbo].[Pro_HandsetAssemblyAndPacking] pm");

            var proEvent = _dbEntities.Database.SqlQuery<ProMasterModel>(proEv).ToList();

            foreach (var model in proEvent)
            {
                var ext = !string.IsNullOrWhiteSpace(model.PoCategory) ? " / " + model.PoCategory : "";
                if (model.OrderNumber != 0)
                {
                    var ordQty = model.TotalOrderQuantity;

                    decimal ordersQty = 0;
                    var oNa = "";
                    if (ordQty != 0)
                    {
                        ordersQty = Convert.ToDecimal(ordQty / 1000);

                        oNa = ordersQty + "K";
                    }

                    model.ProjectName = model.ProjectName + "-" + model.PoCategory + "-" + CommonConversion.AddOrdinal((int)model.OrderNumber) +
                                        " - " + oNa;
                }
                if (model.OrderNumber == 0 && Convert.ToString(model.PlanIds) == "0")
                {
                    model.PlanIds = "0";
                    model.ProjectName = "ALL";

                }

            }

            return proEvent;
        }

        public List<VmProductionPlan> GetHandsetSmtHistoryData(string planId, string category ,string currentMonthYear)
        {
            _dbEntities.Database.CommandTimeout = 6000;

            long planIds = 0;
            long.TryParse(planId, out planIds);

            string query = "";

            if (planId == "0")
            {
                query = string.Format(@"
                select sm.PlanId,sm.ProjectMasterID,sm.OrderNumber,sm.PoCategory,sm.ProjectName,sm.TotalOrderQuantity,

                case when cast(cast(sm.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(sm.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(sm.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(sm.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateSmt,

                case when cast(cast(sm.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(sm.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(sm.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(sm.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateSmt,

                case when cast(cast(sm.Iqc_SDate_Auto as date) as varchar)=cast(cast(sm.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(sm.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(sm.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateSmt,

                case when cast(cast(sm.Iqc_SDate_Manual as date) as varchar)=cast(cast(sm.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(sm.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(sm.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateSmt,

                case when cast(cast(sm.Trial_SDate_Auto as date) as varchar)=cast(cast(sm.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(sm.Trial_EDate_Auto as date) as varchar)
                else cast (cast(sm.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateSmt,

                case when cast(cast(sm.Trial_SDate_Manual as date) as varchar)=cast(cast(sm.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(sm.Trial_EDate_Manual as date) as varchar)
                else cast (cast(sm.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.Trial_EDate_Manual as date) as varchar) end as TrialActualDateSmt,

                case when cast(cast(sm.MassProduction_SDate_Auto as date) as varchar)=cast(cast(sm.MassProduction_EDate_Auto as date) as varchar) 
                then cast(cast(sm.MassProduction_EDate_Auto as date) as varchar)
                else cast (cast(sm.MassProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.MassProduction_EDate_Auto as date) as varchar) end as MassPlanningDateSmt,

                case when cast(cast(sm.MassProduction_SDate_Manual as date) as varchar)=cast(cast(sm.MassProduction_EDate_Manual as date) as varchar) 
                then cast(cast(sm.MassProduction_EDate_Manual as date) as varchar)
                else cast (cast(sm.MassProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.MassProduction_EDate_Manual as date) as varchar) end as MassActualDateSmt,
                sm.HandsetSmtDelayReason, sm.HandsetIqcDelayReason,sm.HandsetTrialDelayReason,sm.HandsetMpDelayReason,sm.HandsetPackingDelayReason

                from PPMS.[dbo].[Pro_HandsetSmt] sm");
            }
            else
            {
                query = string.Format(@"
                select sm.PlanId,sm.ProjectMasterID,sm.OrderNumber,sm.PoCategory,sm.ProjectName,sm.TotalOrderQuantity,

                case when cast(cast(sm.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(sm.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(sm.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(sm.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateSmt,

                case when cast(cast(sm.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(sm.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(sm.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(sm.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateSmt,

                case when cast(cast(sm.Iqc_SDate_Auto as date) as varchar)=cast(cast(sm.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(sm.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(sm.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateSmt,

                case when cast(cast(sm.Iqc_SDate_Manual as date) as varchar)=cast(cast(sm.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(sm.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(sm.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateSmt,

                case when cast(cast(sm.Trial_SDate_Auto as date) as varchar)=cast(cast(sm.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(sm.Trial_EDate_Auto as date) as varchar)
                else cast (cast(sm.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateSmt,

                case when cast(cast(sm.Trial_SDate_Manual as date) as varchar)=cast(cast(sm.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(sm.Trial_EDate_Manual as date) as varchar)
                else cast (cast(sm.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.Trial_EDate_Manual as date) as varchar) end as TrialActualDateSmt,

                case when cast(cast(sm.MassProduction_SDate_Auto as date) as varchar)=cast(cast(sm.MassProduction_EDate_Auto as date) as varchar) 
                then cast(cast(sm.MassProduction_EDate_Auto as date) as varchar)
                else cast (cast(sm.MassProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(sm.MassProduction_EDate_Auto as date) as varchar) end as MassPlanningDateSmt,

                case when cast(cast(sm.MassProduction_SDate_Manual as date) as varchar)=cast(cast(sm.MassProduction_EDate_Manual as date) as varchar) 
                then cast(cast(sm.MassProduction_EDate_Manual as date) as varchar)
                else cast (cast(sm.MassProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(sm.MassProduction_EDate_Manual as date) as varchar) end as MassActualDateSmt,
                sm.HandsetSmtDelayReason, sm.HandsetIqcDelayReason,sm.HandsetTrialDelayReason,sm.HandsetMpDelayReason,sm.HandsetPackingDelayReason

                from PPMS.[dbo].[Pro_HandsetSmt] sm where PlanId={0}", planIds);

                if (!String.IsNullOrWhiteSpace(currentMonthYear))
                {
                    query += string.Format(@"  AND (FORMAT(sm.MaterialReceive_SDate_Auto,'MMMM, yyyy') >= '" + currentMonthYear.Trim() + "' And  FORMAT(sm.MaterialReceive_EDate_Auto,'MMMM, yyyy') <= '" + currentMonthYear.Trim() + "')");
                }
            }
            var exe = _dbEntities.Database.SqlQuery<VmProductionPlan>(query).ToList();

            foreach (var model in exe)
            {
                var ext = !string.IsNullOrWhiteSpace(model.PoCategory) ? " / " + model.PoCategory : "";
                if (model.OrderNumber != 0)
                {
                    var ordQty = model.TotalOrderQuantity;

                    decimal ordersQty = 0;
                    var oNa = "";
                    if (ordQty != 0)
                    {
                        ordersQty = Convert.ToDecimal(ordQty / 1000);

                        oNa = ordersQty + "K";
                    }

                    string word = model.ProjectName;
                    if (word.Length > 0)
                    {
                        int i = word.IndexOf(" ") + 1;
                        string str = word.Substring(i);
                        model.ProjectName = str + "-" + model.PoCategory + "-" + CommonConversion.AddOrdinal((int)model.OrderNumber) +
                                     " - " + oNa;
                    }
                }
            }

            return exe;
        }

        public List<VmProductionPlan> GetHandsetHousingHistoryData(string planId, string category, string currentMonthYear)
        {
            _dbEntities.Database.CommandTimeout = 6000;

            long planIds = 0;
            long.TryParse(planId, out planIds);

            string query = "";

            if (planId == "0")
            {
                query = string.Format(@"
                select ph.PlanId,ph.ProjectMasterID,ph.OrderNumber,ph.PoCategory,ph.ProjectName,ph.TotalOrderQuantity,
                case when cast(cast(ph.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(ph.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(ph.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(ph.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateHousing,

                case when cast(cast(ph.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(ph.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(ph.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(ph.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateHousing,

                case when cast(cast(ph.Iqc_SDate_Auto as date) as varchar)=cast(cast(ph.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(ph.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(ph.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateHousing,

                case when cast(cast(ph.Iqc_SDate_Manual as date) as varchar)=cast(cast(ph.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(ph.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(ph.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateHousing,

                case when cast(cast(ph.Trial_SDate_Auto as date) as varchar)=cast(cast(ph.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(ph.Trial_EDate_Auto as date) as varchar)
                else cast (cast(ph.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateHousing,

                case when cast(cast(ph.Trial_SDate_Manual as date) as varchar)=cast(cast(ph.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(ph.Trial_EDate_Manual as date) as varchar)
                else cast (cast(ph.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.Trial_EDate_Manual as date) as varchar) end as TrialActualDateHousing,

                case when cast(cast(ph.ReliabilityTest_SDate_Auto as date) as varchar)=cast(cast(ph.ReliabilityTest_EDate_Auto as date) as varchar) 
                then cast(cast(ph.ReliabilityTest_EDate_Auto as date) as varchar)
                else cast (cast(ph.ReliabilityTest_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.ReliabilityTest_EDate_Auto as date) as varchar) end as ReliabilityPlanningDateHousing,

                case when cast(cast(ph.ReliabilityTest_SDate_Manual as date) as varchar)=cast(cast(ph.ReliabilityTest_EDate_Manual as date) as varchar) 
                then cast(cast(ph.ReliabilityTest_EDate_Manual as date) as varchar)
                else cast (cast(ph.ReliabilityTest_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.ReliabilityTest_EDate_Manual as date) as varchar) end as ReliabilityActualDateHousing,


                case when cast(cast(ph.MassProduction_SDate_Auto as date) as varchar)=cast(cast(ph.MassProduction_EDate_Auto as date) as varchar) 
                then cast(cast(ph.MassProduction_EDate_Auto as date) as varchar)
                else cast (cast(ph.MassProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.MassProduction_EDate_Auto as date) as varchar) end as MassPlanningDateHousing,

                case when cast(cast(ph.MassProduction_SDate_Manual as date) as varchar)=cast(cast(ph.MassProduction_EDate_Manual as date) as varchar) 
                then cast(cast(ph.MassProduction_EDate_Manual as date) as varchar)
                else cast (cast(ph.MassProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.MassProduction_EDate_Manual as date) as varchar) end as MassActualDateHousing,
                ph.HHousingMaterialDelayReason,ph.HHousingIqcDelayReason,ph.HHousingTrialDelayReason,ph.HHousingReliabilityDelayReason,ph.HHousingMpDelayReason,ph.HHousingPackingDelayReason

                from [PPMS].[dbo].[Pro_HandsetHousing] ph");
            }
            else
            {
                query = string.Format(@"select ph.PlanId,ph.ProjectMasterID,ph.OrderNumber,ph.PoCategory,ph.ProjectName,ph.TotalOrderQuantity,

                case when cast(cast(ph.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(ph.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(ph.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(ph.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateHousing,

                case when cast(cast(ph.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(ph.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(ph.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(ph.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateHousing,

                case when cast(cast(ph.Iqc_SDate_Auto as date) as varchar)=cast(cast(ph.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(ph.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(ph.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateHousing,

                case when cast(cast(ph.Iqc_SDate_Manual as date) as varchar)=cast(cast(ph.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(ph.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(ph.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateHousing,

                case when cast(cast(ph.Trial_SDate_Auto as date) as varchar)=cast(cast(ph.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(ph.Trial_EDate_Auto as date) as varchar)
                else cast (cast(ph.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateHousing,

                case when cast(cast(ph.Trial_SDate_Manual as date) as varchar)=cast(cast(ph.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(ph.Trial_EDate_Manual as date) as varchar)
                else cast (cast(ph.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.Trial_EDate_Manual as date) as varchar) end as TrialActualDateHousing,

                case when cast(cast(ph.ReliabilityTest_SDate_Auto as date) as varchar)=cast(cast(ph.ReliabilityTest_EDate_Auto as date) as varchar) 
                then cast(cast(ph.ReliabilityTest_EDate_Auto as date) as varchar)
                else cast (cast(ph.ReliabilityTest_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.ReliabilityTest_EDate_Auto as date) as varchar) end as ReliabilityPlanningDateHousing,

                case when cast(cast(ph.ReliabilityTest_SDate_Manual as date) as varchar)=cast(cast(ph.ReliabilityTest_EDate_Manual as date) as varchar) 
                then cast(cast(ph.ReliabilityTest_EDate_Manual as date) as varchar)
                else cast (cast(ph.ReliabilityTest_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.ReliabilityTest_EDate_Manual as date) as varchar) end as ReliabilityActualDateHousing,


                case when cast(cast(ph.MassProduction_SDate_Auto as date) as varchar)=cast(cast(ph.MassProduction_EDate_Auto as date) as varchar) 
                then cast(cast(ph.MassProduction_EDate_Auto as date) as varchar)
                else cast (cast(ph.MassProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(ph.MassProduction_EDate_Auto as date) as varchar) end as MassPlanningDateHousing,

                case when cast(cast(ph.MassProduction_SDate_Manual as date) as varchar)=cast(cast(ph.MassProduction_EDate_Manual as date) as varchar) 
                then cast(cast(ph.MassProduction_EDate_Manual as date) as varchar)
                else cast (cast(ph.MassProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(ph.MassProduction_EDate_Manual as date) as varchar) end as MassActualDateHousing,
                ph.HHousingMaterialDelayReason,ph.HHousingIqcDelayReason,ph.HHousingTrialDelayReason,ph.HHousingReliabilityDelayReason,ph.HHousingMpDelayReason,ph.HHousingPackingDelayReason

                from [PPMS].[dbo].[Pro_HandsetHousing] ph where ph.PlanId={0}", planIds);

                if (!String.IsNullOrWhiteSpace(currentMonthYear))
                {
                    query += string.Format(@"  AND (FORMAT(ph.MaterialReceive_SDate_Auto,'MMMM, yyyy') >= '" + currentMonthYear.Trim() + "' And  FORMAT(ph.MaterialReceive_EDate_Auto,'MMMM, yyyy') <= '" + currentMonthYear.Trim() + "')");
                }

            }

            var exe = _dbEntities.Database.SqlQuery<VmProductionPlan>(query).ToList();

            foreach (var model in exe)
            {
                var ext = !string.IsNullOrWhiteSpace(model.PoCategory) ? " / " + model.PoCategory : "";
                if (model.OrderNumber != 0)
                {
                    var ordQty = model.TotalOrderQuantity;

                    decimal ordersQty = 0;
                    var oNa = "";
                    if (ordQty != 0)
                    {
                        ordersQty = Convert.ToDecimal(ordQty / 1000);

                        oNa = ordersQty + "K";
                    }

                    string word = model.ProjectName;
                    if (word.Length > 0)
                    {
                        int i = word.IndexOf(" ") + 1;
                        string str = word.Substring(i);
                        model.ProjectName = str + "-" + model.PoCategory + "-" + CommonConversion.AddOrdinal((int)model.OrderNumber) +
                                     " - " + oNa;
                    }

                }
            }

            return exe;
        }

        public List<VmProductionPlan> GetHandsetBatteryHistoryData(string planId, string category, string currentMonthYear)
        {
            _dbEntities.Database.CommandTimeout = 6000;

            long planIds = 0;
            long.TryParse(planId, out planIds);

            string query = "";

            if (planId == "0")
            {
                query = string.Format(@"
                select phb.PlanId,phb.ProjectMasterID,phb.OrderNumber,phb.PoCategory,phb.ProjectName,phb.TotalOrderQuantity,

                case when cast(cast(phb.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(phb.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(phb.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(phb.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateBattery,

                case when cast(cast(phb.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(phb.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(phb.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(phb.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateBattery,

                case when cast(cast(phb.Iqc_SDate_Auto as date) as varchar)=cast(cast(phb.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(phb.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(phb.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateBattery,

                case when cast(cast(phb.Iqc_SDate_Manual as date) as varchar)=cast(cast(phb.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(phb.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(phb.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateBattery,

                case when cast(cast(phb.Trial_SDate_Auto as date) as varchar)=cast(cast(phb.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(phb.Trial_EDate_Auto as date) as varchar)
                else cast (cast(phb.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateBattery,

                case when cast(cast(phb.Trial_SDate_Manual as date) as varchar)=cast(cast(phb.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(phb.Trial_EDate_Manual as date) as varchar)
                else cast (cast(phb.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.Trial_EDate_Manual as date) as varchar) end as TrialActualDateBattery,

                case when cast(cast(phb.ReliabilityTest_SDate_Auto as date) as varchar)=cast(cast(phb.ReliabilityTest_EDate_Auto as date) as varchar) 
                then cast(cast(phb.ReliabilityTest_EDate_Auto as date) as varchar)
                else cast (cast(phb.ReliabilityTest_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.ReliabilityTest_EDate_Auto as date) as varchar) end as ReliabilityPlanningDateBattery,

                case when cast(cast(phb.ReliabilityTest_SDate_Manual as date) as varchar)=cast(cast(phb.ReliabilityTest_EDate_Manual as date) as varchar) 
                then cast(cast(phb.ReliabilityTest_EDate_Manual as date) as varchar)
                else cast (cast(phb.ReliabilityTest_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.ReliabilityTest_EDate_Manual as date) as varchar) end as ReliabilityActualDateBattery,

                case when cast(cast(phb.MassProduction_SDate_Auto as date) as varchar)=cast(cast(phb.MassProduction_EDate_Auto as date) as varchar) 
                then cast(cast(phb.MassProduction_EDate_Auto as date) as varchar)
                else cast (cast(phb.MassProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.MassProduction_EDate_Auto as date) as varchar) end as MassPlanningDateBattery,

                case when cast(cast(phb.MassProduction_SDate_Manual as date) as varchar)=cast(cast(phb.MassProduction_EDate_Manual as date) as varchar) 
                then cast(cast(phb.MassProduction_EDate_Manual as date) as varchar)
                else cast (cast(phb.MassProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.MassProduction_EDate_Manual as date) as varchar) end as MassActualDateBattery,

                case when cast(cast(phb.AgingTest_SDate_Auto as date) as varchar)=cast(cast(phb.AgingTest_EDate_Auto as date) as varchar) 
                then cast(cast(phb.AgingTest_EDate_Auto as date) as varchar)
                else cast (cast(phb.AgingTest_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.AgingTest_EDate_Auto as date) as varchar) end as AgingPlanningDateBattery,

                case when cast(cast(phb.AgingTest_SDate_Manual as date) as varchar)=cast(cast(phb.AgingTest_EDate_Manual as date) as varchar) 
                then cast(cast(phb.AgingTest_EDate_Manual as date) as varchar)
                else cast (cast(phb.AgingTest_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.AgingTest_EDate_Manual as date) as varchar) end as AgingActualDateBattery,
                phb.HBatteryMaterialDelayReason,phb.HBatteryIqcDelayReason,phb.HBatteryTrialDelayReason,phb.HBatteryReliabilityDelayReason,phb.HBatteryMpDelayReason, phb.HBatteryAgingDelayReason,phb.HBatteryPackingDelayReason
			
                from [PPMS].[dbo].[Pro_HandsetBattery] phb");
            }
            else
            {
                query = string.Format(@"select phb.PlanId,phb.ProjectMasterID,phb.OrderNumber,phb.PoCategory,phb.ProjectName,phb.TotalOrderQuantity,

                case when cast(cast(phb.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(phb.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(phb.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(phb.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateBattery,

                case when cast(cast(phb.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(phb.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(phb.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(phb.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateBattery,

                case when cast(cast(phb.Iqc_SDate_Auto as date) as varchar)=cast(cast(phb.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(phb.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(phb.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateBattery,

                case when cast(cast(phb.Iqc_SDate_Manual as date) as varchar)=cast(cast(phb.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(phb.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(phb.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateBattery,

                case when cast(cast(phb.Trial_SDate_Auto as date) as varchar)=cast(cast(phb.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(phb.Trial_EDate_Auto as date) as varchar)
                else cast (cast(phb.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateBattery,

                case when cast(cast(phb.Trial_SDate_Manual as date) as varchar)=cast(cast(phb.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(phb.Trial_EDate_Manual as date) as varchar)
                else cast (cast(phb.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.Trial_EDate_Manual as date) as varchar) end as TrialActualDateBattery,

                case when cast(cast(phb.ReliabilityTest_SDate_Auto as date) as varchar)=cast(cast(phb.ReliabilityTest_EDate_Auto as date) as varchar) 
                then cast(cast(phb.ReliabilityTest_EDate_Auto as date) as varchar)
                else cast (cast(phb.ReliabilityTest_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.ReliabilityTest_EDate_Auto as date) as varchar) end as ReliabilityPlanningDateBattery,

                case when cast(cast(phb.ReliabilityTest_SDate_Manual as date) as varchar)=cast(cast(phb.ReliabilityTest_EDate_Manual as date) as varchar) 
                then cast(cast(phb.ReliabilityTest_EDate_Manual as date) as varchar)
                else cast (cast(phb.ReliabilityTest_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.ReliabilityTest_EDate_Manual as date) as varchar) end as ReliabilityActualDateBattery,

                case when cast(cast(phb.MassProduction_SDate_Auto as date) as varchar)=cast(cast(phb.MassProduction_EDate_Auto as date) as varchar) 
                then cast(cast(phb.MassProduction_EDate_Auto as date) as varchar)
                else cast (cast(phb.MassProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.MassProduction_EDate_Auto as date) as varchar) end as MassPlanningDateBattery,

                case when cast(cast(phb.MassProduction_SDate_Manual as date) as varchar)=cast(cast(phb.MassProduction_EDate_Manual as date) as varchar) 
                then cast(cast(phb.MassProduction_EDate_Manual as date) as varchar)
                else cast (cast(phb.MassProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.MassProduction_EDate_Manual as date) as varchar) end as MassActualDateBattery,

                case when cast(cast(phb.AgingTest_SDate_Auto as date) as varchar)=cast(cast(phb.AgingTest_EDate_Auto as date) as varchar) 
                then cast(cast(phb.AgingTest_EDate_Auto as date) as varchar)
                else cast (cast(phb.AgingTest_SDate_Auto as date) as varchar)+' to '+cast(cast(phb.AgingTest_EDate_Auto as date) as varchar) end as AgingPlanningDateBattery,

                case when cast(cast(phb.AgingTest_SDate_Manual as date) as varchar)=cast(cast(phb.AgingTest_EDate_Manual as date) as varchar) 
                then cast(cast(phb.AgingTest_EDate_Manual as date) as varchar)
                else cast (cast(phb.AgingTest_SDate_Manual as date) as varchar)+' to '+cast(cast(phb.AgingTest_EDate_Manual as date) as varchar) end as AgingActualDateBattery,
                phb.HBatteryMaterialDelayReason,phb.HBatteryIqcDelayReason,phb.HBatteryTrialDelayReason,phb.HBatteryReliabilityDelayReason,phb.HBatteryMpDelayReason, phb.HBatteryAgingDelayReason,phb.HBatteryPackingDelayReason
			
                from [PPMS].[dbo].[Pro_HandsetBattery] phb where phb.PlanId={0}", planIds);

                if (!String.IsNullOrWhiteSpace(currentMonthYear))
                {
                    query += string.Format(@"  AND (FORMAT(phb.MaterialReceive_SDate_Auto,'MMMM, yyyy') >= '" + currentMonthYear.Trim() + "' And  FORMAT(phb.MaterialReceive_EDate_Auto,'MMMM, yyyy') <= '" + currentMonthYear.Trim() + "')");
                }

            }


            var exe = _dbEntities.Database.SqlQuery<VmProductionPlan>(query).ToList();

            foreach (var model in exe)
            {
                var ext = !string.IsNullOrWhiteSpace(model.PoCategory) ? " / " + model.PoCategory : "";
                if (model.OrderNumber != 0)
                {
                    var ordQty = model.TotalOrderQuantity;

                    decimal ordersQty = 0;
                    var oNa = "";
                    if (ordQty != 0)
                    {
                        ordersQty = Convert.ToDecimal(ordQty / 1000);

                        oNa = ordersQty + "K";
                    }

                    string word = model.ProjectName;
                    if (word.Length > 0)
                    {
                        int i = word.IndexOf(" ") + 1;
                        string str = word.Substring(i);
                        model.ProjectName = str + "-" + model.PoCategory + "-" + CommonConversion.AddOrdinal((int)model.OrderNumber) +
                                     " - " + oNa;
                    }


                }
            }

            return exe;
        }

        public List<VmProductionPlan> GetHandsetAssemblyHistoryData(string planId, string category, string currentMonthYear)
        {
            _dbEntities.Database.CommandTimeout = 6000;

            long planIds = 0;
            long.TryParse(planId, out planIds);

            string query = "";

            if (planId == "0")
            {
                query = string.Format(@"select asm.PlanId,asm.ProjectMasterID,asm.OrderNumber,asm.PoCategory,asm.ProjectName,asm.TotalOrderQuantity,
                case when cast(cast(asm.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(asm.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(asm.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(asm.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateAssembly,

                case when cast(cast(asm.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(asm.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(asm.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(asm.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateAssembly,

                case when cast(cast(asm.Iqc_SDate_Auto as date) as varchar)=cast(cast(asm.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(asm.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(asm.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateAssembly,

                case when cast(cast(asm.Iqc_SDate_Manual as date) as varchar)=cast(cast(asm.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(asm.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(asm.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateAssembly,

                case when cast(cast(asm.Trial_SDate_Auto as date) as varchar)=cast(cast(asm.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(asm.Trial_EDate_Auto as date) as varchar)
                else cast (cast(asm.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateAssembly,

                case when cast(cast(asm.Trial_SDate_Manual as date) as varchar)=cast(cast(asm.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(asm.Trial_EDate_Manual as date) as varchar)
                else cast (cast(asm.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.Trial_EDate_Manual as date) as varchar) end as TrialActualDateAssembly,

                case when cast(cast(asm.SoftwareConfirmation_SDate_Auto as date) as varchar)=cast(cast(asm.SoftwareConfirmation_EDate_Auto as date) as varchar) 
                then cast(cast(asm.SoftwareConfirmation_EDate_Auto as date) as varchar)
                else cast (cast(asm.SoftwareConfirmation_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.SoftwareConfirmation_EDate_Auto as date) as varchar) end as SoftwarePlanningDateAssembly,

                case when cast(cast(asm.SoftwareConfirmation_SDate_Manual as date) as varchar)=cast(cast(asm.SoftwareConfirmation_EDate_Manual as date) as varchar) 
                then cast(cast(asm.SoftwareConfirmation_EDate_Manual as date) as varchar)
                else cast (cast(asm.SoftwareConfirmation_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.SoftwareConfirmation_EDate_Manual as date) as varchar) end as SoftwareActualDateAssembly,

                case when cast(cast(asm.RndConfirmation_SDate_Auto as date) as varchar)=cast(cast(asm.RndConfirmation_EDate_Auto as date) as varchar) 
                then cast(cast(asm.RndConfirmation_EDate_Auto as date) as varchar)
                else cast (cast(asm.RndConfirmation_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.RndConfirmation_EDate_Auto as date) as varchar) end as RndPlanningDateAssembly,

                case when cast(cast(asm.RndConfirmation_SDate_Manual as date) as varchar)=cast(cast(asm.RndConfirmation_EDate_Manual as date) as varchar) 
                then cast(cast(asm.RndConfirmation_EDate_Manual as date) as varchar)
                else cast (cast(asm.RndConfirmation_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.RndConfirmation_EDate_Manual as date) as varchar) end as RndActualDateAssembly,               

                case when cast(cast(asm.AssemblyProduction_SDate_Auto as date) as varchar)=cast(cast(asm.AssemblyProduction_EDate_Auto as date) as varchar) 
                then cast(cast(asm.AssemblyProduction_EDate_Auto as date) as varchar)
                else cast (cast(asm.AssemblyProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.AssemblyProduction_EDate_Auto as date) as varchar) end as MassPlanningDateAssembly,

                case when cast(cast(asm.AssemblyProduction_SDate_Manual as date) as varchar)=cast(cast(asm.AssemblyProduction_EDate_Manual as date) as varchar) 
                then cast(cast(asm.AssemblyProduction_EDate_Manual as date) as varchar)
                else cast (cast(asm.AssemblyProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.AssemblyProduction_EDate_Manual as date) as varchar) end as MassActualDateAssembly,

                case when cast(cast(asm.Packing_SDate_Auto as date) as varchar)=cast(cast(asm.Packing_EDate_Auto as date) as varchar) 
                then cast(cast(asm.Packing_EDate_Auto as date) as varchar)
                else cast (cast(asm.Packing_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.Packing_EDate_Auto as date) as varchar) end as PackingPlanningDateAssembly,

                case when cast(cast(asm.Packing_SDate_Manual as date) as varchar)=cast(cast(asm.Packing_EDate_Manual as date) as varchar) 
                then cast(cast(asm.Packing_EDate_Manual as date) as varchar)
                else cast (cast(asm.Packing_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.Packing_EDate_Manual as date) as varchar) end as PackingActualDateAssembly,
                asm.HAssemblyMaterialDelayReason,asm.HAssemblyIqcDelayReason,asm.HAssemblyTrialDelayReason,asm.HAssemblySoftComDelayReason,asm.HAssemblyRndDelayReason,asm.HAssemblyDelayReason,asm.HAssemblyPackingDelayReason
           
                from [PPMS].[dbo].[Pro_HandsetAssemblyAndPacking] asm");
            }
            else
            {
                query = string.Format(@"select asm.PlanId,asm.ProjectMasterID,asm.OrderNumber,asm.PoCategory,asm.ProjectName,asm.TotalOrderQuantity,
                case when cast(cast(asm.MaterialReceive_SDate_Auto as date) as varchar)=cast(cast(asm.MaterialReceive_EDate_Auto as date) as varchar) 
                then cast(cast(asm.MaterialReceive_EDate_Auto as date) as varchar)
                else cast (cast(asm.MaterialReceive_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.MaterialReceive_EDate_Auto as date) as varchar) end as SoftPlanningDateAssembly,

                case when cast(cast(asm.MaterialReceive_SDate_Manual as date) as varchar)=cast(cast(asm.MaterialReceive_EDate_Manual as date) as varchar) 
                then cast(cast(asm.MaterialReceive_EDate_Manual as date) as varchar)
                else cast (cast(asm.MaterialReceive_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.MaterialReceive_EDate_Manual as date) as varchar) end as SoftActualDateAssembly,

                case when cast(cast(asm.Iqc_SDate_Auto as date) as varchar)=cast(cast(asm.Iqc_EDate_Auto as date) as varchar) 
                then cast(cast(asm.Iqc_EDate_Auto as date) as varchar)
                else cast (cast(asm.Iqc_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.Iqc_EDate_Auto as date) as varchar) end as IqcPlanningDateAssembly,

                case when cast(cast(asm.Iqc_SDate_Manual as date) as varchar)=cast(cast(asm.Iqc_EDate_Manual as date) as varchar) 
                then cast(cast(asm.Iqc_EDate_Manual as date) as varchar)
                else cast (cast(asm.Iqc_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.Iqc_EDate_Manual as date) as varchar) end as IqcActualDateAssembly,

                case when cast(cast(asm.Trial_SDate_Auto as date) as varchar)=cast(cast(asm.Trial_EDate_Auto as date) as varchar) 
                then cast(cast(asm.Trial_EDate_Auto as date) as varchar)
                else cast (cast(asm.Trial_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.Trial_EDate_Auto as date) as varchar) end as TrialPlanningDateAssembly,

                case when cast(cast(asm.Trial_SDate_Manual as date) as varchar)=cast(cast(asm.Trial_EDate_Manual as date) as varchar) 
                then cast(cast(asm.Trial_EDate_Manual as date) as varchar)
                else cast (cast(asm.Trial_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.Trial_EDate_Manual as date) as varchar) end as TrialActualDateAssembly,
                
                case when cast(cast(asm.SoftwareConfirmation_SDate_Auto as date) as varchar)=cast(cast(asm.SoftwareConfirmation_EDate_Auto as date) as varchar) 
                then cast(cast(asm.SoftwareConfirmation_EDate_Auto as date) as varchar)
                else cast (cast(asm.SoftwareConfirmation_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.SoftwareConfirmation_EDate_Auto as date) as varchar) end as SoftwarePlanningDateAssembly,

                case when cast(cast(asm.SoftwareConfirmation_SDate_Manual as date) as varchar)=cast(cast(asm.SoftwareConfirmation_EDate_Manual as date) as varchar) 
                then cast(cast(asm.SoftwareConfirmation_EDate_Manual as date) as varchar)
                else cast (cast(asm.SoftwareConfirmation_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.SoftwareConfirmation_EDate_Manual as date) as varchar) end as SoftwareActualDateAssembly,

                case when cast(cast(asm.RndConfirmation_SDate_Auto as date) as varchar)=cast(cast(asm.RndConfirmation_EDate_Auto as date) as varchar) 
                then cast(cast(asm.RndConfirmation_EDate_Auto as date) as varchar)
                else cast (cast(asm.RndConfirmation_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.RndConfirmation_EDate_Auto as date) as varchar) end as RndPlanningDateAssembly,

                case when cast(cast(asm.RndConfirmation_SDate_Manual as date) as varchar)=cast(cast(asm.RndConfirmation_EDate_Manual as date) as varchar) 
                then cast(cast(asm.RndConfirmation_EDate_Manual as date) as varchar)
                else cast (cast(asm.RndConfirmation_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.RndConfirmation_EDate_Manual as date) as varchar) end as RndActualDateAssembly,
                
                case when cast(cast(asm.AssemblyProduction_SDate_Auto as date) as varchar)=cast(cast(asm.AssemblyProduction_EDate_Auto as date) as varchar) 
                then cast(cast(asm.AssemblyProduction_EDate_Auto as date) as varchar)
                else cast (cast(asm.AssemblyProduction_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.AssemblyProduction_EDate_Auto as date) as varchar) end as MassPlanningDateAssembly,

                case when cast(cast(asm.AssemblyProduction_SDate_Manual as date) as varchar)=cast(cast(asm.AssemblyProduction_EDate_Manual as date) as varchar) 
                then cast(cast(asm.AssemblyProduction_EDate_Manual as date) as varchar)
                else cast (cast(asm.AssemblyProduction_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.AssemblyProduction_EDate_Manual as date) as varchar) end as MassActualDateAssembly,

                case when cast(cast(asm.Packing_SDate_Auto as date) as varchar)=cast(cast(asm.Packing_EDate_Auto as date) as varchar) 
                then cast(cast(asm.Packing_EDate_Auto as date) as varchar)
                else cast (cast(asm.Packing_SDate_Auto as date) as varchar)+' to '+cast(cast(asm.Packing_EDate_Auto as date) as varchar) end as PackingPlanningDateAssembly,

                case when cast(cast(asm.Packing_SDate_Manual as date) as varchar)=cast(cast(asm.Packing_EDate_Manual as date) as varchar) 
                then cast(cast(asm.Packing_EDate_Manual as date) as varchar)
                else cast (cast(asm.Packing_SDate_Manual as date) as varchar)+' to '+cast(cast(asm.Packing_EDate_Manual as date) as varchar) end as PackingActualDateAssembly,
                asm.HAssemblyMaterialDelayReason,asm.HAssemblyIqcDelayReason,asm.HAssemblyTrialDelayReason,asm.HAssemblySoftComDelayReason,asm.HAssemblyRndDelayReason,asm.HAssemblyDelayReason,asm.HAssemblyPackingDelayReason
           
                from [PPMS].[dbo].[Pro_HandsetAssemblyAndPacking] asm where asm.PlanId={0}", planIds);

                if (!String.IsNullOrWhiteSpace(currentMonthYear))
                {
                    query += string.Format(@"  AND (FORMAT(asm.MaterialReceive_SDate_Auto,'MMMM, yyyy') >= '" + currentMonthYear.Trim() + "' And  FORMAT(asm.MaterialReceive_EDate_Auto,'MMMM, yyyy') <= '" + currentMonthYear.Trim() + "')");
                }

            }

            var exe = _dbEntities.Database.SqlQuery<VmProductionPlan>(query).ToList();

            foreach (var model in exe)
            {
                var ext = !string.IsNullOrWhiteSpace(model.PoCategory) ? " / " + model.PoCategory : "";
                if (model.OrderNumber != 0)
                {
                    var ordQty = model.TotalOrderQuantity;

                    decimal ordersQty = 0;
                    var oNa = "";
                    if (ordQty != 0)
                    {
                        ordersQty = Convert.ToDecimal(ordQty / 1000);

                        oNa = ordersQty + "K";
                    }

                    string word = model.ProjectName;
                    if (word.Length > 0)
                    {
                        int i = word.IndexOf(" ") + 1;
                        string str = word.Substring(i);
                        model.ProjectName = str + "-" + model.PoCategory + "-" + CommonConversion.AddOrdinal((int)model.OrderNumber) +
                                     " - " + oNa;
                    }


                }
            }

            return exe;
        }

        #endregion
    }
}