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
    
    public partial class Pro_Line
    {
        public long Id { get; set; }
        public string ProductionType { get; set; }
        public string Line { get; set; }
        public string LineType { get; set; }
        public Nullable<int> ProductionDaysPerMonth { get; set; }
        public Nullable<decimal> HoursPerShift { get; set; }
        public Nullable<int> ShiftPerDay { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<long> Added { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<long> Updated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
